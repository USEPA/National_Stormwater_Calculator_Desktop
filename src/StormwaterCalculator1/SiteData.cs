// ----------------------------------------------------------------------------
// Module:      SiteData.cs
// Project:     EPA Stormwater Calculator
// Description: Code module for the SiteData class that manages data for 
//              the site being analyzed.
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.1
// Last Update: 06/24/14
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Windows.Forms;

namespace StormwaterCalculator
{
    public enum LandCovers
    {
        FOREST  = 0,
        MEADOW  = 1,
        LAWN    = 2,
        DESERT  = 3,
        FILL    = 4,   //not currently used
        IMPERV  = 5,
        COUNT   = 6
    }

    public class SiteData
    {
        //public const int years = 30;                    // simulation duration (yrs)
        // Nominal site area (ac)
        public const double area = 10.0;

        // Default Land Cover parameters
        static double[] dStore = { 0.4, 0.3, 0.2, 0.25, 0.1, 0.05 };
        static double[] roughness = { 0.4, 0.2, 0.3, 0.04, 0.03, 0.01 };

        // Default Slope values
        public static string[] slope = { "2", "5", "10", "20" };

        // Default soil parameters by Hydologic Soil Group
        public static string[] soilGroupTxt = { "A", "B", "C", "D" };
        public static string[] ksat = { "4.0", "0.4", "0.04", "0.01" };
        static string[] suct = { "2.4", "5.1", "8.7", "10.5" };
        static string[] imd = { "0.38", "0.26", "0.15", "0.10" };

        // Simulation variables
        public static int errorCode;
        public static int startYear;
        public static int endYear;
        public static bool includePostDev;

        // Site data
        public static string name;
        public static int soilGroup;
        public static double soilKsat; 
        public static double[] landCover; 
        public static int slopeIndex;
        public static double pathLength;
        public static int rainSourceIndex;
        public static int evapSourceIndex;
        
        // File variables
        public static string tmpPath;      // Temporary file folder
        public static string rainFile;     // Rainfall data file
        public static string inpFile;      // Complete SWMM input file
        public static string inpFile1;     // Base SWMM input file
        public static string rptFile;      // SWMM status report file
        public static string outFile;      // SWMM binary results file

        // Subcatchment areas
        public static double fracImperv;    // Total imperv. area fraction
        public static double area1;         // Main subcatchment area
        public static double area2;         // Imperv. disconnection area
        public static double area3;         // Infil. basin source area
        public static double area4;         // Water harvesting source area
        public static double impervArea1;   // Main subcatch. imperv. area
        public static double pervArea1;     // Main subcatch. pervious area

        // Areas treated by LIDs
        public static double fracImpDiscon;
        public static double fracRainGarden;
        public static double fracGreenRoof;
        public static double fracPorousPave;
        public static double fracRainHarvest;
        public static double fracStreetPlanter;
        public static double fracInfilBasin;

        // Areas devoted to LIDs
        public static double idImpervArea;  // Impervious area disconnected
        public static double idPervArea;    // Pervious area receiving disconnected area
        public static double ibImpervArea;  // Impervious area to Infil. Basin
        public static double ibPervArea;    // Infil. Basin area
        public static double rhImpervArea;  // Rain harvesting impervious area
        public static double rgArea;        // Rain Garden area
        public static double grArea;        // Green Roof area
        public static double spArea;        // Street Planter area
        public static double ppArea;        // Porous Pavement area

        // Indexes of climate change option
        public static int climateScenarioIndex;
        public static int climateYear;

        static SiteData()
        {
            name = "";
            int n = (int)LandCovers.COUNT;
            landCover = new double[n];
        }

        public static void CreateFiles()
        {
            try
            {
                tmpPath = Path.GetTempPath() + "swc";
                Directory.CreateDirectory(tmpPath);
                inpFile = tmpPath + "\\inpFile.inp";
                inpFile1 = tmpPath + "\\inpFile1.inp";
                rptFile = tmpPath + "\\rptFile.rpt";
                outFile = tmpPath + "\\outFile.out";
                rainFile = tmpPath + "\\rainFile.dat";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void DeleteFiles()
        {
            Directory.Delete(tmpPath, true);
        }

        public static void WriteBaseInpFile()
        {
            string line;
            StreamWriter writer = new StreamWriter(inpFile1);
            
            writer.WriteLine("[OPTIONS]");
            writer.WriteLine("FLOW_UNITS  CFS");
            writer.WriteLine("INFILTRATION  GREEN_AMPT");
            writer.WriteLine("FLOW_ROUTING  KINWAVE");
            writer.WriteLine("REPORT_STEP  0:15:00");
            writer.WriteLine("WET_STEP  0:05:00");
            writer.WriteLine("DRY_STEP  1:00:00");
            writer.WriteLine("ROUTING_STEP  60");
            writer.WriteLine("START_TIME  0:00:00");
            writer.WriteLine("END_TIME  23:59:59");
            writer.WriteLine("START_DATE  01/01/" + SiteData.startYear.ToString());
            writer.WriteLine("END_DATE  12/31/" + SiteData.endYear.ToString());
            writer.WriteLine("TEMPDIR  \"" + tmpPath + "\"");
            writer.WriteLine();

            writer.WriteLine("[EVAPORATION]");
            line = "MONTHLY  " + Climate.GetEvapData(climateScenarioIndex);
            writer.WriteLine(line);
            writer.WriteLine("DRY_ONLY  YES");
            writer.WriteLine();

            // Raingages (dummy RainGage2 used for retention BMP)
            writer.WriteLine("[RAINGAGES]");
            writer.WriteLine("RainGage2  INTENSITY  1:00  1.0  TIMESERIES  TS1");
            writer.WriteLine();

            // Subcatchments
            writer.WriteLine("[SUBCATCHMENTS]");
            WriteSubcatchments(writer);
            writer.WriteLine();
            writer.WriteLine("[SUBAREAS]");
            WriteSubareas(writer);
            writer.WriteLine();

            // Infiltration parameters
            writer.WriteLine("[INFILTRATION]");
            line = suct[soilGroup] + "  ";
            if (soilKsat <= 0.0) soilKsat = Double.Parse(ksat[soilGroup]);
            line += soilKsat.ToString();
            line += "  " + imd[soilGroup];
            writer.WriteLine("Subcatch1  " + line);
            writer.WriteLine("Subcatch2  " + line);
            writer.WriteLine("Subcatch3  " + line);
            writer.WriteLine("Subcatch4  " + line);
            writer.WriteLine();

            // LID controls
            LidData.WriteLidControls(writer);
            writer.WriteLine();
            LidData.WriteLidUsage(writer);
            writer.WriteLine();

            // Outfall Node
            writer.WriteLine("[OUTFALLS]");
            writer.WriteLine("Outfall1  0.0  FREE");
            writer.WriteLine();

            // Rainwater Harvesting
            if (rhImpervArea > 0)
            {
                // Model cisterns as a storage node with infiltration
                writer.WriteLine("[STORAGE]");

                // Storage height = cistern height (ft)
                double maxHt = LidData.cisternHt / 12;

                // Cistern area in sq ft
                double cisternArea = (double)LidData.rhSize / 7.85 / maxHt;

                // Total storage surface area (sqft) =  # cisterns/1000 sqft *
                //    capture area (1000 sqft) * cistern area (sqft)
                double a0 = (double)LidData.rhNumber * rhImpervArea * 43560 /
                            1000 * cisternArea;

                // Infil. rate from storage (in/hr) =  cistern drain rate (ft3/day) /
                //    cistern area (ft2) * 12 in/ft / 24 hr/day
                double infilRate = (double)LidData.rhDrainRate / 7.85 / cisternArea / 2;

                // Input storage node line
                line = "Cisterns  0  " + maxHt.ToString("F2") +
                       "  0  FUNCTIONAL  0  0  " + a0.ToString("F2") + "  0  0  2.4  " +
                       infilRate.ToString() + "  0";
                writer.WriteLine(line);
                writer.WriteLine();

                // Connect cistern overflow to system outlet with a dummy conduit
                writer.WriteLine("[CONDUITS]");
                writer.WriteLine("Dummy2  Cisterns  Outfall1  400  0.01  0  0  0");
                writer.WriteLine();
                writer.WriteLine("[XSECTIONS]");
                writer.WriteLine("Dummy2  DUMMY  0  0  0  0  1");
                writer.WriteLine();
            }
            else
            {
                writer.WriteLine("[JUNCTIONS]");
                writer.WriteLine("Cisterns  0");
                writer.WriteLine();
            }

            // Dummy time series for no rainfall
            writer.WriteLine("[TIMESERIES]");
            writer.WriteLine("TS1  0  0");
            writer.WriteLine();

            // Only save node results to binary output file
            writer.WriteLine("[REPORT]");
            writer.WriteLine("SUBCATCHMENTS  NONE");
            writer.WriteLine("NODES ALL");
            writer.WriteLine("LINKS NONE");
            writer.WriteLine();

            writer.Close();
        }

        public static void SetStartEndYears(int years)
        {
            GeoData.GetStartEndYears(rainSourceIndex);
            int yrs = endYear - startYear + 1;
            if (yrs >= years) yrs = years;
            startYear = endYear - yrs + 1;
        }

        public static void WriteSubcatchments(StreamWriter writer)
        {
            // Total impervious and pervious areas for site w/o LIDs
            double impervArea = area * fracImperv;
            double pervArea = area - impervArea;

            // Areas associated with Impervious Disconnection LID (Subcatch2)
            idImpervArea = fracImpDiscon * impervArea;
            idPervArea = (double)LidData.idCapture / 100 * idImpervArea;

            // Infiltration basin areas (Subcatch3)
            ibImpervArea = fracInfilBasin * impervArea;
            ibPervArea = ibImpervArea * (double)LidData.ibCapture / 100;

            // Rain harvesting areas (Subcatch4)
            rhImpervArea = fracRainHarvest * impervArea;

            // Areas of LIDs placed in main subcatchment (Subcatch1)
            ppArea = fracPorousPave * impervArea * (double)LidData.ppCapture / 100;
            rgArea = fracRainGarden * impervArea * (double)LidData.rgCapture / 100;
            spArea = fracStreetPlanter * impervArea * (double)LidData.spCapture / 100;
            grArea = fracGreenRoof * impervArea;

            // Area of main subcatchment (Subcatch1)
            double lidArea = ppArea + rgArea + spArea + grArea;      // LID area
            impervArea1 = impervArea - idImpervArea - ibImpervArea - // impervious area
                          ppArea - spArea - grArea - rhImpervArea;                   
            pervArea1 = pervArea - idPervArea - ibPervArea - rgArea; // pervious area
            area1 = impervArea1 + pervArea1 + lidArea;               // total area

            // Input line for Subcatch1 (main subcatchment)
            if (area1 == 0.0) writer.WriteLine("Subcatch1  RainGage2  Outfall1  0  100  0  0  0");
            else
            {
                string line = "Subcatch1  RainGage1  Outfall1  " + area1.ToString();
                double imp1 = impervArea1 / (area1 - lidArea) * 100.0;
                line = line + "  " + imp1.ToString();
                double width = (area1 - lidArea) * 43560 / pathLength;
                line = line + "  " + width.ToString();
                line = line + "  " + slope[slopeIndex] + "  0";
                writer.WriteLine(line);
            }

            // Input line for Subcatch2 (disconnected area subcatchment)
            area2 = idImpervArea + idPervArea;
            if (area2 == 0.0) writer.WriteLine("Subcatch2  RainGage2  Outfall1  0  100  0  0  0");
            else
            {
                string line = "Subcatch2  RainGage1  Outfall1 " + area2.ToString();
                double imp2 = idImpervArea / area2 * 100.0;
                line = line + "  " + imp2.ToString();
                double width = area2 * 43560 / pathLength;
                line = line + "  " + width.ToString();
                line = line + "  " + slope[slopeIndex] + "  0";
                writer.WriteLine(line);
            }

            // Input line for Subcatch3 (infiltration basin subcatchment)
            area3 = ibImpervArea + ibPervArea;
            if (area3 == 0.0) writer.WriteLine("Subcatch3  RainGage2  Outfall1  0  100  0  0  0");
            else
            {
                string line = "Subcatch3  RainGage1  Outfall1 " + area3.ToString();
                double imp3 = ibImpervArea / area3 * 100.0;
                line = line + "  " + imp3.ToString();
                double width = area3 * 43560 / pathLength;
                line = line + "  " + width.ToString();
                line = line + "  " + slope[slopeIndex] + "  0";
                writer.WriteLine(line);
            }

            // Input line for Subcatch4 (rainwater harvesting subcatchment)
            area4 = rhImpervArea;
            if (area4 == 0) writer.WriteLine("Subcatch4  RainGage2  Outfall1  0  100  0  0  0");
            else
            {
                string line = "Subcatch4  RainGage1  Cisterns  " + area4.ToString() +  "  100";
                double width = area4 * 43560 / pathLength;
                line = line + "  " + width.ToString();
                line = line + "  " + slope[slopeIndex] + "  0";
                writer.WriteLine(line);
            }

        }

        public static void WriteSubareas(StreamWriter writer)
        {
            // Impervious roughness & depression storage
            double impervRoughness = roughness[(int)LandCovers.IMPERV];
            double impervDstore = dStore[(int)LandCovers.IMPERV];

            // Pervious roughness & depression storage
            double pervRoughness = 0.0;
            double pervDstore = 0.0;
            double pctPerv = 100.0 * (1.0 - fracImperv);
            if (pctPerv > 0.0)
            {
                for (int i = 0; i < (int)LandCovers.IMPERV; i++)
                {
                    double ratio = landCover[i] / pctPerv;
                    pervRoughness += ratio * roughness[i];
                    pervDstore += ratio * dStore[i];
                }
            }

            // Special case for infiltration basin
            double ibPervDstore = 0.0;
            if (area3 > 0.0)
            {
                ibPervDstore = (double)LidData.ibHeight;
            }

            // Input line for roughness & depression storage
            string line = impervRoughness.ToString();
            line = line + "  " + pervRoughness.ToString("F2") + "  ";
            line = line + impervDstore.ToString();
            line = line + "  " + pervDstore.ToString("F2");

            // Main subcatchment with disconnected areas removed
            if (area1 == 0.0) writer.WriteLine("Subcatch1  0  0  0  0  0  OUTLET");
            else writer.WriteLine("Subcatch1  " + line + "  0  OUTLET");

            // Lawn spreading contributing area
            if (area2 == 0.0) writer.WriteLine("Subcatch2  0  0  0  0  0  OUTLET");
            else writer.WriteLine("Subcatch2  " + line + "  0  PERVIOUS");

            // Infiltration Basin contributing area
            if (area3 == 0.0) writer.WriteLine("Subcatch3  0  0  0  0  0  OUTLET");
            else writer.WriteLine("Subcatch3  " + impervRoughness.ToString() + "  0  " +
                impervDstore.ToString() + "  " + ibPervDstore.ToString() + "  0  PERVIOUS");

            // Rain Harvesting contributing area
            if (area4 == 0.0) writer.WriteLine("Subcatch4  0  0  0  0  0  OUTLET");
            else writer.WriteLine("Subcatch4  " + line + "  0  OUTLET");
        }

        public static void WriteLongTermInpFile()
        {
            File.Copy(inpFile1, inpFile, true);
            using (StreamWriter sw = File.AppendText(inpFile))
            {
                sw.WriteLine("[RAINGAGES]");
                string stationID = GeoData.GetStationID(SiteData.rainSourceIndex);
                string fname = rainFile;
                sw.WriteLine(
                    "RainGage1  INTENSITY  1:00  1.0  FILE  \"" + fname + "\" " + stationID + " IN");
                sw.WriteLine();

                // Rainfall adjustments
                if (climateScenarioIndex > 0)
                {
                    sw.WriteLine("[ADJUSTMENTS]");
                    sw.WriteLine(Climate.GetPrecipData(climateScenarioIndex));
                    sw.WriteLine();
                }
            }
        }

        public static void SaveRainfallData(int stationIndex)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Data files (*.dat)|*.dat|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.FileName =
                "Rain - " + GeoData.GetStationID(stationIndex) + ".dat";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (GeoData.GetSwmmRainfall(stationIndex) == 0)
                    System.IO.File.Copy(SiteData.rainFile, saveFileDialog1.FileName, true);
            }
        }

        public static void SaveEvapData(int stationIndex, string stationName)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Data files (*.dat)|*.dat|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.FileName = "Evap - " + stationName + ".dat";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
                writer.WriteLine(GeoData.GetEvapData(evapSourceIndex, false));
                writer.Close();
            }
        }
    }
}
