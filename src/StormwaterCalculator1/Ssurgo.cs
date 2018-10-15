// ----------------------------------------------------------------------------
// Module:      Ssurgo.cs
// Project:     EPA Stormwater Calculator
// Description: Code module for extracting soil data from the SSURGO
//              web service
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.0
// Last Update: 10/14/13
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Data;
using System.IO;

using System.Windows.Forms;

namespace StormwaterCalculator
{
    public class Ssurgo
    {
        public class XYPair
        {
            public XYPair(double x, double y)
            {
                Lng = x;
                Lat = y;
            }

            public double Lat;
            public double Lng;
        }

        public class Polygon
        {
            public List<XYPair> Coord = new List<XYPair>();
            public bool Outer = true;
        }

        public class SoilLayer
        {
            public double DepthToTop;
            public double DepthToBottom;
            public double KSAT;
            public string HSG;
            public double Slope_R;
            public int CompPct_R;
        }

        public class Soil
        {
            // <summary>Unique Key (eg '124246') from SoilDataAccess MuKey property</summary>
            public string Key;
            // <summary>Area Symbol (eg 'GA089') from SoilDataAccess AreaSymbol property</summary>
            public string Area;
            // <summary>Soil Symbol (eg 'CuC') from SoilDataAccess MuSym property</summary>
            public string Symbol;
            // <summary>Hydrologic Soil Group (A, B, C or D), blank if not available</summary>
            public string HSG = "";
            // <summary>Saturated Hydraulic Conductivity (micrometers/second) in top horizon</summary>
            public double KSAT_Surface;
            // <summary>Saturated Hydraulic Conductivity (micrometers/second) depth weighted</summary>
            public double KSAT;
            // <summary>Representative slope in %</summary>
            public double Slope_R;
            // <summary>Layers associated with soil</summary>
            public List<SoilLayer> Layers;
            // <summary>Polygons describing spatial extent of soil</summary>
            public List<Polygon> Polygons = new List<Polygon>();
        }

        public static bool FoundValidHSG;

//-------------------------------------------------------------------------------------------------

        public static List<Soil> FindSoils(double aLatitude, double aLongitude,
                                           double aDistance = 1000)
        {
            // Create a list of Soil objects to return
            FoundValidHSG = false;
            List<Soil> Soils = new List<Soil>();

            // Create an HTTP request to get all of the SSURGO mapping units
            // within Distance of the target location
            string Filter = "<Filter>"
                              + "  <DWithin>"
                              + "    <PropertyName>Geometry</PropertyName>"
                              + "    <gml:Point>"
                              + "       <gml:coordinates>"
                              + aLongitude.ToString()
                              + ","
                              + aLatitude.ToString()
                              + "</gml:coordinates>"
                              + "    </gml:Point>"
                              + "    <Distance%20units='m'>"
                              + aDistance.ToString()
                              + "</Distance>"
                              + "  </DWithin>"
                              + "</Filter>";



            string URL = "https://sdmdataaccess.nrcs.usda.gov/Spatial/SDMNAD83Geographic.wfs?Service=WFS&Version=1.0.0&Request=GetFeature&Typename=MapunitPoly&Filter="
                         + Filter;
            string ResultXml;

            // Create a WebClient object to submit the HTTP request
            WebClient client = new WebClient();
            try
            {
                // Submit the request and retrieve the response
                ResultXml = client.DownloadString(URL);
   
                // Parse the resulting XML with the mapping units info in it
                // to obtain boundaries and ultimately soil data for each unit
                Ssurgo.ParseResultXml(ResultXml, Soils);

            }
            catch
            {
                //MessageBox.Show(e.Message);
                throw;
            }
            client.Dispose();
            return Soils;
        }

//-------------------------------------------------------------------------------------------------

        static void ParseResultXml(string ResultXml, List<Soil> Soils)
        {
            XmlDocument XMLdoc = new XmlDocument();
            try
            {
                XMLdoc.LoadXml(ResultXml);

                for (int FeatureIndex = 0; FeatureIndex < XMLdoc.GetElementsByTagName("gml:featureMember").Count; FeatureIndex++)
                {
                    XmlDocument XMLdocFeature = new XmlDocument();
                    XMLdocFeature.LoadXml(XMLdoc.GetElementsByTagName("gml:featureMember").Item(FeatureIndex).InnerXml);

                    for (int Index = 0; Index < XMLdocFeature.GetElementsByTagName("ms:mukey").Count; Index++)
                    {
                        // Skip surface water mapping units
                        if (XMLdocFeature.GetElementsByTagName("ms:musym").Item(Index).InnerText == "W") continue;
                        Soils.Add(Ssurgo.AddSoil(XMLdocFeature, Index));
                    }
                }
                SortedList<String, Soil> UniqueLayerProperties = new SortedList<String, Soil>();
                foreach (Soil aSoil in Soils)
                {
                    Ssurgo.GetLayerProperties(aSoil, UniqueLayerProperties);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid response from SSURGO web service.");
                Ssurgo.FoundValidHSG = false;
            }
        }

//-------------------------------------------------------------------------------------------------

        static Soil AddSoil(XmlDocument XMLdocFeature, int Index)
        {
            double x, y;
            char[] delimiters = new char[] { ' ', ',' };
            Soil aSoil = new Soil();
            aSoil.Area = XMLdocFeature.GetElementsByTagName("ms:areasymbol").Item(Index).InnerText;
            aSoil.Symbol = XMLdocFeature.GetElementsByTagName("ms:musym").Item(Index).InnerText;
            aSoil.Key = XMLdocFeature.GetElementsByTagName("ms:mukey").Item(Index).InnerText;

            // Mapping Unit's outer boundary polygon
            string OuterBoundary = XMLdocFeature.GetElementsByTagName("gml:outerBoundaryIs").Item(Index).InnerText;
            string[] OuterCoords = OuterBoundary.Split(delimiters);
            Polygon aPolygon = new Polygon();
            for (int i = 1; i < OuterCoords.Length; i = i + 2)
            {
                if (Double.TryParse(OuterCoords[i - 1], out x) && Double.TryParse(OuterCoords[i], out y))
                {
                    aPolygon.Coord.Add(new XYPair(x, y));
                }
            }
            aSoil.Polygons.Add(aPolygon);
/*
 * 
 * Skip inner polygons (holes) since there's no easy way to display them on the Bing map
 * 
            // Mapping Unit's inner polygons (i.e., holes)
            if (XMLdocFeature.GetElementsByTagName("gml:innerBoundaryIs").Item(Index) != null)
            {
                for (int IndexInnerBoundary = 0;
                         IndexInnerBoundary < XMLdocFeature.GetElementsByTagName("gml:innerBoundaryIs").Count;
                         IndexInnerBoundary++)
                {
                    string InnerBoundary = XMLdocFeature.GetElementsByTagName("gml:innerBoundaryIs").Item(IndexInnerBoundary).InnerText;
                    string[] InnerCoords = InnerBoundary.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    Polygon innerPolygon = new Polygon();
                    innerPolygon.Outer = false;
                    for (int i = 1; i < InnerCoords.Length; i = i + 2)
                    {
                        if (Double.TryParse(InnerCoords[i - 1], out x) &&
                            Double.TryParse(InnerCoords[i], out y))
                        {
                            innerPolygon.Coord.Add(new XYPair(x, y));
                        }
                    }
                    aSoil.Polygons.Add(innerPolygon);
                }
            }
 */ 
            return aSoil;
        }

//-------------------------------------------------------------------------------------------------

        static void GetLayerProperties(Soil aSoil, SortedList<String, Soil> UniqueLayerProperties)
        {
            if (UniqueLayerProperties.ContainsKey(aSoil.Key))
            {
                aSoil.Layers = UniqueLayerProperties[aSoil.Key].Layers;
            }
            else
            {
                aSoil.Layers = FindSoilLayerProperties(aSoil.Key, aSoil.Area).Values.ToList();
                UniqueLayerProperties.Add(aSoil.Key, aSoil);
            }

            // determine dominant component
            int DomPct = 0;
            foreach (SoilLayer aSoilLayer in aSoil.Layers)
            {
                if (aSoilLayer.CompPct_R > DomPct) DomPct = aSoilLayer.CompPct_R;
            }
            double DepthTotal = 0.0;
            double KsatTotal = 0.0;
            foreach (SoilLayer aSoilLayer in aSoil.Layers)
            {
                if (aSoilLayer.CompPct_R == DomPct)
                {
                    //only use dominant component in weighted average calculation
                    double DepthNow = aSoilLayer.DepthToBottom - aSoilLayer.DepthToTop;
                    DepthTotal += DepthNow;
                    KsatTotal += (DepthNow * aSoilLayer.KSAT);
                    if (aSoilLayer.DepthToTop == 0)
                    {
                        aSoil.KSAT_Surface = aSoilLayer.KSAT;
                        aSoil.Slope_R = aSoilLayer.Slope_R;
                    }
                    if (aSoilLayer.DepthToTop == 0)
                    {
                        Ssurgo.FoundValidHSG = true;
                        aSoil.HSG = aSoilLayer.HSG;
                    }
                }
                if (DepthTotal > 0)
                    aSoil.KSAT = KsatTotal / DepthTotal;
                else
                {
                    aSoil.KSAT = -999;
                    aSoil.KSAT_Surface = -999;
                }
            }
        }

//-------------------------------------------------------------------------------------------------

        enum FieldNumbers
        {
            musym = 6,
            hydgrpdcd = 12,
            slope_r = 11,
            comppct_r = 10,
            hzdept_r = 13,
            hzdepb_r = 14,
            ksat_r = 15,
            chkey = 16
        }

//-------------------------------------------------------------------------------------------------

        static SortedList<string, SoilLayer> FindSoilLayerProperties(string aKey, string aArea)
        {
            const string CR = "\r";
            if (aKey == null || aKey.Length == 0) return null;
            SortedList<String, SoilLayer> SoilLayers = new SortedList<String, SoilLayer>();

            // This is the SQL query to be submitted to the SSURGO web service
            string Query = "SELECT" + CR +
                           "saversion, saverest," + CR +
                           "l.areasymbol, l.areaname, l.lkey," + CR +
                           "mu.musym, mu.muname, museq, mu.mukey," + CR +
                           "compname, comppct_r, slope_r, hydgrpdcd, " + CR +
                           "hzdept_r, hzdepb_r, ksat_r, ch.chkey " + CR +
                           "FROM legend l" + CR +
                           "INNER JOIN mapunit mu ON mu.lkey = l.lkey" + CR +
                           "AND mu.mukey = " + aKey + CR +
                           "INNER JOIN sacatalog sac ON l.areasymbol = sac.areasymbol" + CR +
                           "INNER JOIN muaggatt m ON m.mukey = mu.mukey" + CR +
                           "LEFT OUTER JOIN component c ON c.mukey = mu.mukey" + CR +
                           "LEFT OUTER JOIN chorizon ch ON ch.cokey = c.cokey" + CR +
                           "ORDER BY comppct_r DESC, c.cokey, hzdept_r ASC" + CR;

            // Here we use a Service Reference to run the SQL query against the SSURGO
            // database. See http://msdn.microsoft.com/en-us/library/bb628652.aspx for
            // instructions on how to create a Service Referenc. The URL of NRCS's 
            // SSURGO web service required by the Service Reference is:
            //"https://sdmdataaccess.nrcs.usda.gov/Tabular/SDMTabularService.asmx".

            ServiceReference1.SDMTabularServiceSoapClient Soap = new ServiceReference1.SDMTabularServiceSoapClient();
            System.Data.DataSet SystemDataSet = Soap.RunQuery(Query);
            Soap.Close();

            // Now we extract the data for each soil layer in the mapping unit we queried.
            if (SystemDataSet.Tables.Count > 0)
            {
                string Name = "";
                DataTable Table = SystemDataSet.Tables[0];
                foreach (DataRow Row in Table.Rows)
                {
                    Object[] RowItemArray = Row.ItemArray;
                    Name = (string)RowItemArray[(int)FieldNumbers.musym];
                    string Key = (string)RowItemArray[(int)FieldNumbers.chkey];
                    string HSG = (string)RowItemArray[(int)FieldNumbers.hydgrpdcd];
                    if (HSG.Length > 0)
                    {
                        string Hzdept_r = (string)RowItemArray[(int)FieldNumbers.hzdept_r];
                        string Hzdepb_r = (string)RowItemArray[(int)FieldNumbers.hzdepb_r];
                        string Ksat_r = (string)RowItemArray[(int)FieldNumbers.ksat_r];
                        SoilLayer Layer = new SoilLayer();
                        if (!Double.TryParse(Hzdept_r, out Layer.DepthToTop)) Layer.DepthToTop = -999;
                        if (!Double.TryParse(Hzdepb_r, out Layer.DepthToBottom)) Layer.DepthToBottom = -999;
                        if (!Double.TryParse(Ksat_r, out Layer.KSAT)) Layer.KSAT = -999;
                        Layer.HSG = HSG;
                        if (!Int32.TryParse((string)RowItemArray[(int)FieldNumbers.comppct_r],
                            out Layer.CompPct_R)) Layer.CompPct_R = 0;
                        if (!Double.TryParse((string)RowItemArray[(int)FieldNumbers.slope_r],
                            out Layer.Slope_R)) Layer.Slope_R = 0;
                        if (!SoilLayers.ContainsKey(Key)) SoilLayers.Add(Key, Layer);
                    }
                }
            }
            return SoilLayers;
        }

    }  // End of class Ssurgo
}
