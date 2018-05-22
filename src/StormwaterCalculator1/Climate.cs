// ----------------------------------------------------------------------------
// Module:      Climate.cs
// Project:     EPA Stormwater Calculator
// Description: Code module that handles climate change adjustments
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.1
// Last Update: 06/24/14
//
// Notes:
//   This module accesses tables of rainfall and evaporation adjustments
//   that are compiled as Resources within the project. These tables are
//   named as follows:
//
//   PRECyyyyxxx - climate change adjustments to monthly average rainfall
//                 for each precip. station where yyyy is either 2035 or
//                 2060 and xxx can be either Hot, Med, or Dry.
//
//   PMETyyyyxxx - climate adjusted daily evaporation rates (by month of
//                 year) for each evap. station where yyyy is either 2035
//                 or 2060 and xxx can be either Hot, Med, or Dry.
//
//   GEVdepthyyyyxxx - climate change extreme event 24-hour rainfall depths
//                     for six different return periods for each precip.
//                     station where yyyy is either 2035 or 2060 and xxx can
//                     be either Hot, Med, or Dry.
//
//   GEVdepth_historical - extreme event 24-hour rainfall depths for six 
//                         different return periods at each precip. station
//                         for the historical rainfall record.
//                        
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ZedGraph;

namespace StormwaterCalculator
{
    class Climate
    {
        // Data for ZedGraph zgcMonthlyAdjust on MainForm
        public static PointPairList rainDelta1;
        public static PointPairList rainDelta2;
        public static PointPairList rainDelta3;

        public static PointPairList evap1;
        public static PointPairList evap2;
        public static PointPairList evap3;
        public static PointPairList evap0;

        // Data for ZedGraph zgcAnnualMaxAdjust on MainForm
        public static PointPairList maxrain1;
        public static PointPairList maxrain2;
        public static PointPairList maxrain3;
        public static PointPairList maxrain0;

        public static string[] monthLabels =
        { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public static string[] returnPeriods = { "5", "10", "15", "30", "50", "100" };

        public static string[] scenarioNames = { "None", "Hot/Dry", "Median", "Warm/Wet" };

        // Updates the climate change adjustments displayed on the MainForm's
        // Climate Change page when a new precip. station or climate projection
        // year is selected.
        public static void UpdateAdjustments()
        {
            UpdateRainfallAdjustments();
            UpdateMaxRainAdjustments();
        }

        // Finds adjustments to monthly average rainfall depths for a choice of
        // precip. station location and climate projection year.
        private static void UpdateRainfallAdjustments()
        {
            // Get rainfall adjustments for each scenario
            string rainID = GeoData.GetStationID(SiteData.rainSourceIndex);
            double[] y = new double[12];
            string precTable;

            rainDelta1.Clear();
            if (SiteData.climateYear == 2035)
                precTable = StormwaterCalculator.Properties.Resources.PREC2035Hot;
            else
                precTable = StormwaterCalculator.Properties.Resources.PREC2060Hot;
            if (GetRainfallAdjustments(precTable, rainID, ref y)) rainDelta1.Add(null, y);
                        
            rainDelta2.Clear();
            if (SiteData.climateYear == 2035)
                precTable = StormwaterCalculator.Properties.Resources.PREC2035Med;
            else
                precTable = StormwaterCalculator.Properties.Resources.PREC2060Med;
            if (GetRainfallAdjustments(precTable, rainID, ref y)) rainDelta2.Add(null, y);

            rainDelta3.Clear();
            if (SiteData.climateYear == 2035)
                precTable = StormwaterCalculator.Properties.Resources.PREC2035Wet;
            else
                precTable = StormwaterCalculator.Properties.Resources.PREC2060Wet;
            if (GetRainfallAdjustments(precTable, rainID, ref y)) rainDelta3.Add(null, y);
        }

        // Finds extreme event rainfall depths for a choice of precip. station and
        // climate projection year.
        private static void UpdateMaxRainAdjustments()
        {
            string rainID = GeoData.GetStationID(SiteData.rainSourceIndex);
            double[] y = new double[6];
            string precTable;

            maxrain1.Clear();
            if (SiteData.climateYear == 2035)
                precTable = StormwaterCalculator.Properties.Resources.GEVdepth2035Hot;
            else
                precTable = StormwaterCalculator.Properties.Resources.GEVdepth2060Hot;
            if (GetMaxRainAdjustments(precTable, rainID, ref y)) maxrain1.Add(null, y);
                        
            maxrain2.Clear();
            if (SiteData.climateYear == 2035)
                precTable = StormwaterCalculator.Properties.Resources.GEVdepth2035Med;
            else
                precTable = StormwaterCalculator.Properties.Resources.GEVdepth2060Med;
            if (GetMaxRainAdjustments(precTable, rainID, ref y)) maxrain2.Add(null, y);

            maxrain3.Clear();
            if (SiteData.climateYear == 2035)
                precTable = StormwaterCalculator.Properties.Resources.GEVdepth2035Wet;
            else
                precTable = StormwaterCalculator.Properties.Resources.GEVdepth2060Wet;
            if (GetMaxRainAdjustments(precTable, rainID, ref y)) maxrain3.Add(null, y);

            maxrain0.Clear();
            precTable = StormwaterCalculator.Properties.Resources.GEVdepth_historical;
            if (GetMaxRainAdjustments(precTable, rainID, ref y)) maxrain0.Add(null, y);
        }

        // Finds adjustments to monthly evap. rates for a choice of evap. station
        // and climate projection year.
        private static void UpdateEvapAdjustments()
        {
            // Get evap values for each scenario
            string evapID = GeoData.GetEvapStationID(SiteData.evapSourceIndex);
            double[] y = new double[12];
            string evapTable;

            evap1.Clear();
            if (SiteData.climateYear == 2035)
                evapTable = StormwaterCalculator.Properties.Resources.Pmet2035Hot;
            else
                evapTable = StormwaterCalculator.Properties.Resources.Pmet2060Hot;
            if (GetEvapAdjustments(evapTable, evapID, ref y)) evap1.Add(null, y);

            evap2.Clear();
            if (SiteData.climateYear == 2035)
                evapTable = StormwaterCalculator.Properties.Resources.Pmet2035Med;
            else
                evapTable = StormwaterCalculator.Properties.Resources.Pmet2060Med;
            if (GetEvapAdjustments(evapTable, evapID, ref y)) evap2.Add(null, y);

            evap3.Clear();
            if (SiteData.climateYear == 2035)
                evapTable = StormwaterCalculator.Properties.Resources.Pmet2035Wet;
            else
                evapTable = StormwaterCalculator.Properties.Resources.Pmet2060Wet;
            if (GetEvapAdjustments(evapTable, evapID, ref y)) evap3.Add(null, y);

            evap0.Clear();
            string evapData = GeoData.GetEvapData(SiteData.evapSourceIndex, false);
            string[] values = evapData.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);
            if (values.Length >= 12)
            {
                for (int i = 0; i < 12; i++) y[i] = Double.Parse(values[i]);
                evap0.Add(null, y);
            }
        }

        // Returns the climate-adjusted set of monthly evaporation rates for the
        // current evap. station and choice of climate change scenario.
        public static string GetEvapData(int climateIndex)
        {
            PointPairList evapData;
            UpdateEvapAdjustments();
            switch (climateIndex)
            {
                case 0: evapData = evap0;
                    break;
                case 1: evapData = evap1;
                    break;
                case 2: evapData = evap2;
                    break;
                case 3: evapData = evap3;
                    break;
                default: return "";
            }
            string s = "";
            for (int i = 0; i < 12; i++)
                s = s + evapData[i].Y.ToString() + "  ";
            return s;
        }

        public static string GetPrecipData(int climateIndex)
        {
            // Update the adjustment factors for the various climate change
            // scenarios in case they haven't already been made
            UpdateAdjustments();

            // If no climate change scenario selected then do nothing
            if (climateIndex == 0) return "";

            // Identify the rainfall adjustments for the selected climate scenario
            PointPairList rainDelta;
            if (climateIndex == 1) rainDelta = rainDelta1;
            else if (climateIndex == 2) rainDelta = rainDelta2;
            else rainDelta = rainDelta3;

            // Concatenate monthly adjustment factors into SWMM input line
            string s = "Rainfall";
            double x;
            for (int i = 0; i < 12; i++)
            {
                x = 1.0 + rainDelta[i].Y / 100;
                s = s + "  " + x.ToString();
            }
            return s;
        }

        // Retrieves the set of extreme event rainfall depths for the current
        // choice of climate change scenario.
        public static void GetExtremeRainfall(int climateIndex, ref double[] r)
        {
            PointPairList maxRain;
            if (climateIndex == 0) maxRain = maxrain0;
            else if (climateIndex == 1) maxRain = maxrain1;
            else if (climateIndex == 2) maxRain = maxrain2;
            else maxRain = maxrain3;
            for (int i = 0; i < maxRain.Count; i++) r[i] = maxRain[i].Y;
        }

        // Retrieves the adjustments to monthly average rainfall for
        // a specific precip. station and climate change table.
        private static bool GetRainfallAdjustments(string precTable, string rainID, ref double[] x)
        {
            string line;
            for (int i = 0; i < 12; i++) x[i] = 0;
            try
            {
                using (StringReader sr = new StringReader(precTable))
                {
                    line = sr.ReadLine();
                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null) return false;
                        if (line.StartsWith(rainID))
                        {
                            string[] values = line.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);
                            if (values.Length >= 13) 
                                for (int i = 1; i <= 12; i++) x[i - 1] = Double.Parse(values[i]);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "File Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Retrieves extreme event storm depths at different return periods for
        // a specific precip. station and climate change table.
        private static bool GetMaxRainAdjustments(string maxRainTable, string rainID, ref double[] y)
        {
            string line;
            for (int i = 0; i < 6; i++) y[i] = 0;
            try
            {
                using (StringReader sr = new StringReader(maxRainTable))
                {
                    line = sr.ReadLine();
                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null) return false;
                        if (line.StartsWith(rainID))
                        {
                            string[] values = line.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);
                            if (values.Length >= 7)
                                for (int i = 1; i <= 6; i++)
                                    y[i - 1] = Double.Parse(values[i]) / 25.4;  // convert from mm to inches
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "File Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        // Retrieves the climate-adjusted set of monthly evaporation rates for
        // a specific precip. station and climate change table.
        private static bool GetEvapAdjustments(string evapTable, string evapID, ref double[] x)
        {
            string line;
            for (int i = 0; i < 12; i++) x[i] = 0;
            try
            {
                using (StringReader sr = new StringReader(evapTable))
                {
                    line = sr.ReadLine();
                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null) return false;
                        if (line.StartsWith(evapID))
                        {
                            string[] values = line.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);
                            if (values.Length >= 14)
                                for (int i = 2; i <= 13; i++) x[i - 2] = Double.Parse(values[i]);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "File Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}
