// ----------------------------------------------------------------------------
// Module:      GeoData.cs
// Project:     EPA Stormwater Calculator
// Description: Code module for a static class that retrieves locations
//              and descriptions of soil, rain, and evap data sources
//              nearest to a site's lat-long.
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.0
// Last Update: 10/14/13
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;

// Part of D4EMLite.dll
using atcUtility;
using D4EMLite;


namespace StormwaterCalculator
{
    // Data structure for a map polygon
    struct MapPolygon
    {
        public double[] lat;
        public double[] lng;
        public double kSat;
        public double slope;
        public string soilGroup;
    }

    public class GeoData
    {
        // Max. number of vertices for a polygon
        const int MaxCoordCount = 1000;//600;

        // Incremental radius used to search for data
        const double SoilsRadiusIncrement = 1000.0;  // meters

        public static string[] SoilGroups = {"A", "B", "C", "D"};
        public static ArrayList soilPolygons = new ArrayList();
        public static int maxMetStations = 5;

        // Declared in D4EMLite
        static EvapStationLocations evapStationLocations = new EvapStationLocations();
        static PrecStationLocations precStationLocations = new PrecStationLocations();
 
        static SortedList<Double, PointLocation> precStationsClosest;
        static SortedList<Double, PointLocation> evapStationsClosest; 

        // Display site data on MainForm's map page
        public static void ShowSiteData(MainForm mf)
        {
            // Delete current set of map overlays
            mf.InvokeBrowserScript("deleteOverlays");

            // Add soil group polygons to the map
            for (int i = 0; i < soilPolygons.Count; i++)
            {
                MapPolygon p = (MapPolygon)soilPolygons[i];
                if (p.soilGroup == "0" || p.soilGroup == "" || p.kSat < 0.0) continue;
                for (int j = 0; j < p.lat.Count(); j++)
                {
                    mf.InvokeBrowserScript("addCoords", p.lat[j], p.lng[j]);
                }
                string ksatGroup = GetKsatGroup(p.kSat);
                string slopeGroup = GetSlopeGroup(p.slope);
                mf.InvokeBrowserScript("addSoilPolygon", p.soilGroup, ksatGroup, slopeGroup);
            }

            // Add rain data source markers to the map
            List<string> sourceList = new List<string>();
            string txt;
            for (int i = 0; i < precStationsClosest.Count; i++)
            {
                mf.InvokeBrowserScript("addRainMarker",
                                       precStationsClosest.Values[i].Latitude,
                                       precStationsClosest.Values[i].Longitude);
                txt = precStationsClosest.Values[i].Description();
                sourceList.Add((i + 1) + " - " + txt.Insert(txt.IndexOf('(')-1, "\n    "));
            }
 
            // Add rain data source descriptions to main form's list box
            mf.RefreshRainSources(sourceList);
            sourceList.Clear();

            // Add evap data source markers to the map
            for (int i = 0; i < evapStationsClosest.Count; i++)
            {
                mf.InvokeBrowserScript("addEvapMarker",
                                       evapStationsClosest.Values[i].Latitude,
                                       evapStationsClosest.Values[i].Longitude);
                txt = evapStationsClosest.Values[i].Description();
                sourceList.Add((i + 1) + " - " +  GetEvapStationDescription(txt));
            }

            // Add evap data source descriptions to main form's list box
            mf.RefreshEvapSources(sourceList);
        }

        // Converts evaporation station description from
        // # - <station name> (yyyy-yyyy) xx.xx"
        // to
        // # - <station name>\n    (yyyy-yyyy) x.xx inches/day
        public static string GetEvapStationDescription(string s)
        {
            // 
            int index1 = s.IndexOf('(');
            string s1 = s.Substring(0, index1);
            string s2 = "\n    " + s.Substring(index1, 11);
            string s3 = "";
            index1 += 12;
            int index2 = s.IndexOf('"');
            if (index2 > index1)
            {
                s3 = s.Substring(index1, index2 - index1);
                try
                {
                    double e = Double.Parse(s3) / 365;
                    s3 = " " + e.ToString("F2") + " inches/day";
                }
                catch
                {
                    s3 = "";
                }
            }
            return s1 + s2 + s3;
        }

        public static string GetStationID(int rainSourceIndex)
        {
            PrecStationLocation precStationLocation =
                (PrecStationLocation)precStationsClosest.Values[rainSourceIndex];
            return precStationLocation.Id;
        }

        public static string GetEvapStationID(int evapSourceIndex)
        {
            EvapStationLocation evapStationLocation =
                (EvapStationLocation)evapStationsClosest.Values[evapSourceIndex];
            return evapStationLocation.Id;
        }

        public static string GetKsatGroup(double kSat)
        {
            if (kSat > 1.0) return "D";
            if (kSat > 0.1) return "C";
            if (kSat > 0.01) return "B";
            return "A";
        }

        public static string GetSlopeGroup(double slope)
        {
            if (slope <= 2) return "A";
            if (slope <= 7) return "B";
            if (slope <= 15) return "C";
            return "D";
        }

        // Retrieve SSURGO soil data and polygons
        public static bool GetSoilData(double lat, double lng)
        {
            // Initialize soil data
            soilPolygons.Clear();
            List<Ssurgo.Soil> soils = null;

            // Establish a search radius around the site's lat/long
            double SoilsSearchRadius = SoilsRadiusIncrement;
            double SoilsMaxRadius = 5 * SoilsRadiusIncrement;

            // Keep searching the USDA-NRCS's SSURGO DataMart web service within an expanding radius
            for (; ; )
            {
                try
                {
                    // Get soil data within current radius
                    soils = Ssurgo.FindSoils(lat, lng, SoilsSearchRadius);
                }
                catch
                {
                    MessageBox.Show("Could not access the SSURGO soils data base.\n" +
                        "The service may be unavailable.", "Soils Data Search",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                // Data was found
                if (soils != null && soils.Count > 0)
                {
                    // Transfer polygons with soil data retrieved from web service to MapPolygon objects
                    int polyCount = 0;
                    for (int i = 0; i < soils.Count; i++) polyCount += transferSoilPolygon(soils[i]);
                    if (polyCount > 0)
                    {
                        return true;
                    }
                }

                // No data found -- ask user to increase the search radius
                if (SoilsSearchRadius < SoilsMaxRadius)
                {
                    string msg = "No soils data were found within a radius of " +
                        SoilsSearchRadius + " meters. Expand the search?";
                    if (MessageBox.Show(msg, "Soils Data Search", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        return false;
                    }
                    SoilsSearchRadius += SoilsRadiusIncrement;
                }

                // Max. search radius reached
                else
                {
                    MessageBox.Show("No soils data were found for this site.", "Soils Data Search",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
        }

        public static int transferSoilPolygon(Ssurgo.Soil soil)
        {
             // Analyze each polygon in the retrieved soil data set
            int result = 0;
            for (int j = 0; j < soil.Polygons.Count; j++)
            {
                // skip inner polygons and ones with no soil group
                if (!soil.Polygons[j].Outer || soil.HSG == null ||
                     soil.HSG.Length == 0 || soil.KSAT_Surface < 0) continue;

                // find skip rate for polygon vertices
                int coordCount = soil.Polygons[j].Coord.Count;
                int skipCount = coordCount / MaxCoordCount + 1;
                int reducedCoordCount = coordCount / skipCount;

                // transfer vertices from soil.Polygon to a MapPolygon
                MapPolygon p = new MapPolygon();
                p.lat = new double[reducedCoordCount];
                p.lng = new double[reducedCoordCount];
                int k = 0;
                int m = 0;
                while (k < coordCount && m < reducedCoordCount)
                {
                    p.lat[m] = soil.Polygons[j].Coord[k].Lat;
                    p.lng[m] = soil.Polygons[j].Coord[k].Lng;
                    m += 1;
                    k += skipCount;
                }

                // save first character of HSG string to MapPolygon object
                p.soilGroup = soil.HSG.Substring(0, 1);

                // convert KSAT from micrometers/sec to in/hr
                p.kSat = soil.KSAT_Surface * 0.012;

                // save slope value and soil polygon
                p.slope = soil.Slope_R;

                // add the new MapPolygon to the collection of map soil polygons
                soilPolygons.Add(p);
                result = 1;
            }
            return result;
        }


        public static void GetStartEndYears(int rainSourceIndex)
        {
            PrecStationLocation precStationLocation =
                (PrecStationLocation)precStationsClosest.Values[rainSourceIndex];
            char[] delimiterChars = { (char)9 };
            string[] tokens = precStationLocation.Record.Split(delimiterChars,
                              StringSplitOptions.RemoveEmptyEntries);
            SiteData.startYear = Int32.Parse(tokens[8].Substring(1,4));
            SiteData.endYear = Int32.Parse(tokens[9].Substring(1,4));
        }

//=======================================================================================
// The following methods make calls to D4EMLite.dll supplied by Aqua Terra Consultants
//=======================================================================================

        // Locate nearest precip & evap stations
        public static void GetMetStations(double lat, double lng)
        {
            // Lookup the maxMetStations closest precip & evap locations from internal tables
            precStationsClosest = precStationLocations.Closest(lat, lng, maxMetStations);
            evapStationsClosest = evapStationLocations.Closest(lat, lng, maxMetStations);
        }

        public static string GetEvapData(int evapSourceIndex, Boolean dailyFlag)
        {
            string s;
            /*
            if (dailyFlag)
            {
                s = ((EvapStationLocation)evapStationsClosest.Values[evapSourceIndex]).GetData(
                                       Download.TimeseriesFormat.SWMM_Daily, 1, 0);
            }
            else
             */
            {
                s = ((EvapStationLocation)evapStationsClosest.Values[evapSourceIndex]).GetData(
                                       Download.TimeseriesFormat.SWMM_Monthly_Average);
            }
            return s;
        }

        public static int GetSwmmRainfall(int rainSourceIndex)
        {
            // Get information for the nearest rain gage selected by the user (rainSourceIndex)
            PrecStationLocation precStationLocation =
                (PrecStationLocation)precStationsClosest.Values[rainSourceIndex];

            // Save a reference to the current working directory
            string currentDirectory = System.IO.Directory.GetCurrentDirectory();

            try
            {
                // Change the current directory to the temporary one
                System.IO.Directory.SetCurrentDirectory(SiteData.tmpPath);
                
                // Download the rainfall data (converted to SWMM format) into rainFile
                System.IO.File.WriteAllText(SiteData.rainFile,
                    precStationLocation.GetData(Download.TimeseriesFormat.SWMM_Hourly));
                
                // Delete the original WDM-formatted rain file
                string wdmFile = SiteData.tmpPath + "\\" + precStationLocation.FileName;
                System.IO.File.Delete(wdmFile);
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not retrieve hourly rainfall data from file " + 
                    precStationLocation.FileName + "\nSystem generated error message is:\n" +
                    e.Message);
                System.IO.Directory.SetCurrentDirectory(currentDirectory);
                return 1;
            }

            // Re-set the current directory to its original location
            System.IO.Directory.SetCurrentDirectory(currentDirectory);
            return 0;
        }

        // Adjusts precip file to contain start-of-interval dates instead of end-of-interval
        // <<<<<<<<<<<<<<<<  May not be needed >>>>>>>>>>>>>>>>>>
        private static void AdjustPrecipDates()
        {
            char [] delimiter = {' '};
            StringBuilder sb = new StringBuilder();
            foreach (string line in System.IO.File.ReadLines(SiteData.rainFile))
            {
                string[] items = line.Split(delimiter);
                if (items.Length > 5)
                {
                    items[4] = (Convert.ToInt32(items[4]) - 1).ToString();
                    sb.Append(String.Join(" ", items));
                    sb.Append("\n");
                }
            }
            System.IO.File.WriteAllText(SiteData.rainFile, sb.ToString());
        }

        public static void Close()
        {
            D4EMLite.D4EMLiteUtility.CloseD4EMLite();
        }
    }
}
