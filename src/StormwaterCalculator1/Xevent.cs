// ----------------------------------------------------------------------------
// Module:      Xevent.cs
// Project:     EPA Stormwater Calculator
// Description: Code module for running single extreme event SWMM simulations
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.0
// Last Update: 10/14/13
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ZedGraph;

namespace StormwaterCalculator
{
    public class Xevent
    {
        // Extreme event return periods
        public static string[] returnPeriod = { "5", "10", "15", "30", "50", "100" };
        public static int nEvents = returnPeriod.Length;
        public static double[] rainfall = new double[nEvents];
        public static double[] runoff = new double[nEvents];
        public static double[] peakRainfall = new double[nEvents];
        public static double[] peakRunoff = new double[nEvents];

        // Data for extreme event runoff plot (zgcExtremeEvent plot on MainForm)
        public static PointPairList extremeRainfallList;
        public static PointPairList extremeRunoffList;
        public static PointPairList baseExtremeRainfallList;
        public static PointPairList baseExtremeRunoffList;

        // Data for peak runoff plot (zgcExtremePeak plot on MainForm)
        public static PointPairList peakRainfallList;
        public static PointPairList peakRunoffList;
        public static PointPairList basePeakRainfallList;
        public static PointPairList basePeakRunoffList;


        // SCS 24-hour design storm distributions
        public static double[] scsI = new double[241];
        public static double[] scsIa = new double[241];
        public static double[] scsII = new double[241];
        public static double[] scsIII = new double[241];

        //------------------------------------------------------------------------
        // Retrieves the SCS design storm distribution type for a precip. station.
        //------------------------------------------------------------------------
        public static string GetScsType()
        {
            string rainID = GeoData.GetStationID(SiteData.rainSourceIndex);
            string table = StormwaterCalculator.Properties.Resources.PREC_SCS_Types;
            string line;
            try
            {
                using (StringReader sr = new StringReader(table))
                {
                    line = sr.ReadLine();
                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null) return "II";
                        if (line.StartsWith(rainID))
                        {
                            string[] values = line.Split(default(Char[]),
                                    StringSplitOptions.RemoveEmptyEntries);
                            if (values.Length >= 2) return values[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "File Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "II";
            }
        }

        //--------------------------------------------------------------
        // Reads the SCS 24-hour design storm distributions from a file.
        //--------------------------------------------------------------
        public static bool ReadScsDistributions()
        {
            string source = StormwaterCalculator.Properties.Resources.SCS24Hour;
            using (StringReader sr = new StringReader(source))
            {
                // Skip header line
                sr.ReadLine();

                // Read next 241 lines (one per six minute time interval)
                for (int i = 0; i < 241; i++)
                {
                    string line = sr.ReadLine();
                    if (line == null) return false;

                    // Parse the line for 5 values
                    string[] values = line.Split(default(Char[]),
                        StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length < 5) return false;

                    // Save the cumulative percentages for each of the
                    // four SCS distributions at the current time interval
                    scsI[i] = Double.Parse(values[1]);
                    scsIa[i] = Double.Parse(values[2]);
                    scsII[i] = Double.Parse(values[3]);
                    scsIII[i] = Double.Parse(values[4]);
                }
            }
            return true;
        }

        //--------------------------------------------------
        // Runs SWMM for a series of 24-hour extreme events.
        //--------------------------------------------------
        public static void RunExtremeEvents(MainForm mf)
        {
            // Rainfall time series array for SCS 24-hr
            // distribution (at 6-minute intervals)
            double[] rSeries = new double[241];
            
            // Dummy water budget variables
            double infil = 0;
            double evap = 0;

            // Determine which SCS distribution to apply
            string scsType = GetScsType();

            // Get the extreme event rainfall depths for the current
            // precip. station and climate scenario
            Climate.GetExtremeRainfall(SiteData.climateScenarioIndex, ref rainfall);

            // Analyze each return period event
            for (int i = 0; i < returnPeriod.Length; i++)
            {
                // Generate entries in a 24-hour rainfall time series
                switch (scsType)
                {
                    case "I": FillRainTimeSeries(rainfall[i], scsI, ref rSeries); break;
                    case "Ia": FillRainTimeSeries(rainfall[i], scsIa, ref rSeries); break;
                    case "II": FillRainTimeSeries(rainfall[i], scsII, ref rSeries); break;
                    case "III": FillRainTimeSeries(rainfall[i], scsIII, ref rSeries); break;
                }

                // Run SWMM for this storm event
                WriteExtremeEventInpFile(rSeries);
                mf.RunSwmm();
                if (mf.errorCode > 0) return;
                Runoff.GetWaterBudget(1, ref rainfall[i], ref runoff[i], ref infil, ref evap);
                Runoff.GetPeakValues(SiteData.outFile, ref peakRainfall[i], ref peakRunoff[i]);
            }
            SetPlottingData();
        }

        //-------------------------------------------------------------
        // Sets rainfall / runoff plotting values for current scenario. 
        //-------------------------------------------------------------
        public static void SetPlottingData()
        {
            bool showingBaseline = (baseExtremeRainfallList.Count > 0);
            extremeRainfallList.Clear();
            extremeRunoffList.Clear();
            peakRainfallList.Clear();
            peakRunoffList.Clear();

            if (!showingBaseline)
            {
                extremeRainfallList.Add(0, 0);
                extremeRunoffList.Add(0, 0);

                peakRainfallList.Add(0, 0);
                peakRunoffList.Add(0, 0);
            }

            for (int i = 0; i < nEvents; i++)
            {
                extremeRainfallList.Add(0, rainfall[i]);
                extremeRunoffList.Add(0, runoff[i]);
                extremeRainfallList.Add(0, 0);
                extremeRunoffList.Add(0, 0);

                peakRainfallList.Add(0, peakRainfall[i]);
                peakRunoffList.Add(0, peakRunoff[i]);
                peakRainfallList.Add(0, 0);
                peakRunoffList.Add(0, 0);

                if (showingBaseline && i < nEvents-1)
                {
                    extremeRainfallList.Add(0, 0);
                    extremeRunoffList.Add(0, 0);

                    peakRainfallList.Add(0, 0);
                    peakRunoffList.Add(0, 0);
                }
            }
         }

        //----------------------------------------------------------------
        // Sets rainfall / runoff plotting values for a baseline scenario.
        //----------------------------------------------------------------
        public static void SetBasePlottingData()
        {
            baseExtremeRainfallList.Clear();
            baseExtremeRunoffList.Clear();
            basePeakRainfallList.Clear();
            basePeakRunoffList.Clear();

            for (int i = 0; i < returnPeriod.Length; i++)
            {
                baseExtremeRainfallList.Add(0, 0);
                baseExtremeRunoffList.Add(0, 0);
                baseExtremeRainfallList.Add(0, rainfall[i]);
                baseExtremeRunoffList.Add(0, runoff[i]);

                basePeakRainfallList.Add(0, 0);
                basePeakRunoffList.Add(0, 0);
                basePeakRainfallList.Add(0, peakRainfall[i]);
                basePeakRunoffList.Add(0, peakRunoff[i]);

                if (i < returnPeriod.Length - 1)
                {
                    baseExtremeRainfallList.Add(0, 0);
                    baseExtremeRunoffList.Add(0, 0);

                    basePeakRainfallList.Add(0, 0);
                    basePeakRunoffList.Add(0, 0);
                }
            }
            SetPlottingData();
        }

        //---------------------------------------------------------------
        // Generates a rainfall intensity time series from total rainfall
        // depth and a cumulative time distribution.
        //---------------------------------------------------------------
        private static void FillRainTimeSeries(double rTotal, double[] dist, ref double[] r)
        {
            for (int i = 0; i < r.Length; i++) r[i] = rTotal * dist[i] / 100;
        }

        //----------------------------------------------------------------
        // Augments a base SWMM input file with additional lines needed to
        // run a single extreme event analysis.
        //----------------------------------------------------------------
        private static void WriteExtremeEventInpFile(double[] ts)
        {
            string newLine = System.Environment.NewLine;
            File.Copy(SiteData.inpFile1, SiteData.inpFile, true);
            using (StreamWriter sw = File.AppendText(SiteData.inpFile))
            {
                sw.WriteLine("[RAINGAGES]");
                sw.WriteLine("RainGage1  CUMULATIVE  0:06  1.0  TIMESERIES  TS24");
                sw.WriteLine();
                sw.WriteLine("[TIMESERIES]");
                int count = 0;
                int minutes = 6;
                int hours = 0;
                StringBuilder line = new StringBuilder();
                sw.Write("TS24");
                for (int j = 1; j < ts.Length; j++)
                {
                    sw.Write(String.Format("  {0:00}:{1:00}  {2:0.0000}", hours, minutes, ts[j]));
                    count++;
                    if (j == ts.Length - 1) sw.Write(newLine);
                    else if (count == 6)
                    {
                        sw.Write(newLine + "TS24");
                        count = 0;
                    }
                    minutes += 6;
                    if (minutes == 60)
                    {
                        hours += 1;
                        minutes = 0;
                    }
                }
                sw.WriteLine();
                sw.WriteLine("[OPTIONS]");
                sw.WriteLine("WET_STEP  0:06:00");
                sw.WriteLine("DRY_STEP  0:06:00");
                sw.WriteLine("START_DATE  06/01/" + SiteData.endYear.ToString());
                sw.WriteLine("END_DATE    06/03/" + SiteData.endYear.ToString());
            }
        }

    }
}
