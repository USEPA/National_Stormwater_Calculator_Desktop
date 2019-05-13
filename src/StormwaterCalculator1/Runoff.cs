// ----------------------------------------------------------------------------
// Module:      Runoff.cs
// Project:     EPA Stormwater Calculator
// Description: Code module for post-processing the time series of rainfall / 
//              runoff values produced by a SWMM run.
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.1
// Last Update: 07/01/14
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ZedGraph;

using System.Windows.Forms;

namespace StormwaterCalculator
{
    public class Runoff
    {
        // Variables for Reading SWMM Output
        const int recdSize = 4;                  // record size in SWMM output file
        static FileStream fs;                    // SWMM output file
        static BinaryReader reader;
        static long offset;                      // position of output results (bytes)
        static long totalPeriods;                // number of reporting periods
        static long totalDays;                   // number of reporting days
        static long rptStep;                     // seconds per reporting period

        // Rainfall/runoff Totals
        static double totalRainfall;
        static double totalRunoff;

        // Event Counts
        static int rainDayCount;                 // # days w/ rainfall
        static int runoffDayCount;               // # days w/ runoff
        static string eventCountStr;             // text of runoff event count

        // Daily Runoff Threshold (in)
        public static double runoffThreshold = 0.10;

        // Annual water budget variables
        public static double annualRainfall;     // annual rainfall (inches)
        public static double annualRunoff;       // annual runoff (inches)
        public static double annualInfil;        // annual infiltration (inches)
        public static double annualEvap;         // annual evaporation (inches)

        // Summary Statistics
        public enum Statistics
        {
            ANNUAL_RAINFALL = 0,
            ANNUAL_RUNOFF = 1,
            RAINFALL_DAYS = 2,
            RUNOFF_DAYS = 3,
            PCT_STORMS_CAP = 4,
            MIN_RUNOFF_DEPTH = 5,
            MAX_CAPTURE_DEPTH = 6,
            MAX_RETENTION = 7,
            COUNT = 8
        }
        public static double[] runoffStats;

        // Data for zgcRainRunoff plot on MainForm
        public static PointPairList rainRunoffList;    // rainfall-runoff pairs
        public static PointPairList baseRainRunoffList;

        // Data for zgcRunoffFreq plot on MainForm
        public static PointPairList runoffFreqList;    // runoff - frequency pairs
        public static PointPairList baseRunoffFreqList;

        public static PointPairList rainFreqList;
        public static PointPairList baseRainFreqList;

        // Data for zgcRunoffPcnt plot on MainForm
        public static PointPairList runoffPcntList;   // rain depth interval - runoff pairs
        public static PointPairList baseRunoffPcntList;

        // Data for zgcRainfallCapture plot on MainForm
        public static PointPairList retentionPcntList;
        public static PointPairList baseRetentionPcntList;

        // (Make sure following 3 arrays are of equal size)
        public static string[] PcntLabels = { "10", "20", "30", "40", "50", "60", "70",
                                              "75", "80", "85", "90", "95", "99" };
        public static string[] DepthLabels = { "0", "0", "0", "0", "0", "0", "0", "0",
                                               "0", "0", "0", "0", "0" };
        public static double[] PcntValues = { 10, 20, 30, 40, 50, 60, 70, 75, 80, 85, 90, 95, 99 };

        // Daily Rainfall and Runoff Results
        static double[] rainDepths;
        static double[] runoffDepths;
        static int[] dryDays;
        static int[] rainIndex;

        // Rainfall Capture Results
        static double minRunoffDepth;  // Min. depth storm producing runoff
        static double maxCaptureDepth; // Max. depth storm fully captured
        static double maxRetention;    // Max. retention over all storms
        static double captureRatio;    // Events Captured / Total Rain Events
        static double captureCoeff;    // Total Rain Captured / Total Rain

    //---------------------------------------------------------------------------------------

        static Runoff()
        {
            int n = (int)Statistics.COUNT;
            runoffStats = new double[n];
        }

        public static int GetStats(string outFile)
        {
            // Open the SWMM binary output file and create a reader for it

            int result = -1;
            if (File.Exists(outFile) == false) return result;
            fs = new FileStream(outFile, FileMode.Open, FileAccess.Read, FileShare.None);
            reader = new BinaryReader(fs);

            // Check that the file is complete
            if (OutFileComplete())
            {
                    // Initialize statistics
                    totalRainfall = 0.0;
                    totalRunoff = 0.0;
                    rainDayCount = 0;
                    runoffDayCount = 0;

                    // Read the contents of the file
                    ReadOutFile();
                    totalRunoff = Math.Min(totalRunoff, totalRainfall);

                    // Determine mean inter-event time (MIT) to use for counting rainfall events
                    int mit = 0;
                    if (MainForm.ignoreConsecDays) mit = 2;

                    // Populate rainfall - runoff plot
                    CreateRainRunoffPlot(mit);

                    // Compute runoff by storm size plot 
                    CreateRainIntervalPlot(mit);

                    // Compute retention frequency plot
                    CreateRetentionPcntPlot(mit);

                    // Compute runoff frequency plot
                    runoffDayCount = CreateRunoffFreqPlot(mit);

                    // Compute summary statistics
                    ComputeSummaryStats(mit);
                    result = 0;
            }

            // Close the reader and the file stream
            reader.Close();
            fs.Close();
            return result;
        }

        public static void GetPeakValues(string outFile, ref double rainfall, ref double runoff)
        {
            long periods = 0;
            double rain, roff;
            rainfall = 0;
            runoff = 0;
            if (File.Exists(outFile) == false) return;
            fs = new FileStream(outFile, FileMode.Open, FileAccess.Read, FileShare.None);
            reader = new BinaryReader(fs);
            if (OutFileComplete())
            {
                // Move file pointer 1 record before start of output results
                fs.Seek(offset - recdSize, SeekOrigin.Begin);

                // Read reporting step
                rptStep = reader.ReadInt32();

                // Read output for each reporting period
                while (periods < totalPeriods && rainDayCount < totalDays)
                {
                    // Skip date, depth, head, volume & lateral inflow for outfall node
                    fs.Seek(6 * recdSize, SeekOrigin.Current);
                
                    // Read total inflow into outfall node
                    roff = reader.ReadSingle();

                    // Skip overflow for outfall node and depth, head, volume, lateral inflow,
                    // and total inflow to cistern node
                    fs.Seek(6 * recdSize, SeekOrigin.Current);

                    // Add on any overflow from rainwater harvesting cisterns node
                    roff += reader.ReadSingle();
                    if (roff > runoff) runoff = roff;

                    // Read rainfall for site
                    fs.Seek(1 * recdSize, SeekOrigin.Current);
                    rain = reader.ReadSingle();
                    if (rain > rainfall) rainfall = rain;

                    // Move to start of next reporting period
                    fs.Seek(12 * recdSize, SeekOrigin.Current);
                    periods++;
                }

                // Convert cfs to in/hr
                runoff *= 3600.0 * 12.0 / SiteData.area / 43560.0;
            }
            reader.Close();
            fs.Close();
        }

        static bool OutFileComplete()
        {
            // Check that the file is complete
            long outLength = fs.Length;
            if (outLength > 20)
            {
                // Position file pointer to 4 records before the end of the file
                fs.Seek(-4 * recdSize, SeekOrigin.End);

                // Read byte offset where time series results begin
                offset = reader.ReadInt32();

                // Read total number of reporting periods
                totalPeriods = reader.ReadInt32();

                // Check for SWMM errors
                int errorCode = reader.ReadInt32();
                int magic2 = reader.ReadInt32();
                if (magic2 == 516114522 &&
                    errorCode == 0 && totalPeriods > 0) return true;
            }
            return false;
        }


        static void ReadOutFile()
        {
            long periods = 0;               // current reporting time period
            double theDate;                 // date/time as double
            double currentDay = 0.0;        // current date portion of Date/Time
            double previousDay = 0.0;       // date of previous rain day
            double rainfall;                // rainfall (in/hr)
            double runoff;                  // runoff (cfs)
            double depth;                   // runoff depth (in)
            double dailyRainfall = 0.0;     // day's rainfall (in)
            double dailyRunoff = 0.0;       // day's runoff (in)
            double cfsToIn;                 // conversion from cfs to in

            // Move file pointer 1 record before start of output results
            fs.Seek(offset - recdSize, SeekOrigin.Begin);

            // Read reporting step
            rptStep = reader.ReadInt32();
            cfsToIn = rptStep * 12.0 / SiteData.area / 43560.0;

            // Create arrays to hold daily rainfall & runoff totals
            totalDays = totalPeriods * rptStep / 86400;
            runoffDepths = new double[totalDays];
            rainDepths = new double[totalDays];
            dryDays = new int[totalDays];
            rainIndex = new int[totalDays];

            // Read output for each reporting period
            while (periods < totalPeriods && rainDayCount < totalDays)
            {
                // Read the date for the current reporting period
                theDate = reader.ReadDouble();

                // Check if a new day has arrived
                if (Math.Floor(theDate) > currentDay)
                {
                    currentDay = Math.Floor(theDate);

                    // Day has measureable rainfall
                    if (dailyRainfall > runoffThreshold)
                    {
                        rainDepths[rainDayCount] = dailyRainfall;
                        runoffDepths[rainDayCount] = dailyRunoff;
                        if (rainDayCount == 0) dryDays[rainDayCount] = 365;
                        else dryDays[rainDayCount] = (int)(currentDay - previousDay) - 1;
                        rainIndex[rainDayCount] = rainDayCount;
                        previousDay = currentDay;
                        rainDayCount++;
                    }

                    // If no rainfall on this day then assign any runoff to previous wet day
                    else if (dailyRunoff > 0)
                    {
                        runoffDepths[rainDayCount] += dailyRunoff;
                    }

                    // Re-initialize daily rainfall & runoff
                    dailyRainfall = 0.0;
                    dailyRunoff = 0.0;
                }

                // Skip depth, head, volume & lateral inflow for outfall node
                fs.Seek(4 * recdSize, SeekOrigin.Current);
                
                // Read total inflow into outfall node
                runoff = reader.ReadSingle();

                // Skip overflow for outfall node and depth, head, volume, lateral inflow,
                // and total inflow to cistern node
                fs.Seek(6 * recdSize, SeekOrigin.Current);

                // Add on any overflow from rainwater harvesting cisterns node
                runoff += reader.ReadSingle();
                depth = runoff * cfsToIn;
                dailyRunoff += depth;
                totalRunoff += depth;

                // Read rainfall for site
                fs.Seek(1 * recdSize, SeekOrigin.Current);
                rainfall = reader.ReadSingle() * rptStep / 3600.0;
                dailyRainfall += rainfall;
                totalRainfall += rainfall;

                // Move to start of next reporting period
                fs.Seek(12 * recdSize, SeekOrigin.Current);
                periods++;
            }
        }

        static void CreateRainRunoffPlot(int mit)
        {
            for (int i = 0; i < rainDayCount; i++)
            {
                if (dryDays[i] < mit) continue;
                if (runoffDepths[i] <= runoffThreshold)
                {
                    rainRunoffList.Add(new PointPair(rainDepths[i], 0));
                }
                else
                {
                    rainRunoffList.Add(new PointPair(rainDepths[i], runoffDepths[i]));
                }
            }
        }

        static void CreateRainIntervalPlot(int mit)
        {
            // Create temporary array to hold rainfall
            double[] sortedRain = new double[rainDayCount];
            for (int i = 0; i < rainDayCount; i++)
            {
                sortedRain[i] = rainDepths[i];
            }

            // Do an associatve sort of rainfall and rain day indexes
            Array.Sort(sortedRain, rainIndex, 0, rainDayCount);

            // Create a temporary array to hold runoff fractions 
            int pcntCount = PcntLabels.Length;
            double[] runoffPcnt = new double[pcntCount];
            double totalRunoff = 0.0;

            // Compute runoff for each rainfall percentile interval
            int index1 = 0;
            int index2 = 0;
            double rainPcnt = 0;
            for (int i = 0; i < pcntCount; i++)
            {
                index2 = (int)Math.Round(PcntValues[i] / 100 * rainDayCount);
                if (index2 >= rainDayCount) index2 = rainDayCount - 1;
                double rainSum = 0.0;
                double runoffSum = 0.0;
                for (int j = index1; j < index2; j++)
                {
                    int k = rainIndex[j];
                    rainPcnt = rainDepths[k];
                    if (dryDays[k] < mit) continue;
                    rainSum += rainDepths[k];
                    if (runoffDepths[k] > runoffThreshold)
                    {
                        runoffSum += runoffDepths[k];
                        totalRunoff += runoffDepths[k];
                    }
                }
                DepthLabels[i] = rainPcnt.ToString("F2");
                runoffPcnt[i] = runoffSum;
                index1 = index2;
            }
            if (totalRunoff > 0)
            {
                for (int i = 0; i < pcntCount; i++) runoffPcnt[i] *= 100 / totalRunoff;
            }

            // Add runoff fractions to list used for plotting
            runoffPcntList.Add(null, runoffPcnt);
        }

        static void CreateRetentionPcntPlot(int mit)
        {
            int totalEvents = 0;
            double retained;
            int pcntCount = PcntLabels.Length;
            double[] depths = new double[pcntCount];
            int[] events = new int[pcntCount];
            for (int i = 0; i < pcntCount; i++)
            {
                depths[i] = Double.Parse(DepthLabels[i]);
                events[i] = 0;
            }

            for (int j = 0; j < rainDayCount; j++)
            {
                if (dryDays[j] < mit) continue;
                totalEvents++;
                if (runoffDepths[j] <= runoffThreshold)
                {
                    for (int i = 0; i < pcntCount; i++) events[i]++;
                }
                else
                {
                    double rainfall = rainDepths[j];
                    retained = rainfall - runoffDepths[j];
                    retained = Math.Max(retained, 0);
                    for (int i = 0; i < pcntCount; i++)
                    {
                        if (rainfall >= depths[i] && retained >= depths[i])
                        {
                            events[i]++;
                        }
                    }
                }
            }

            double r = (double)totalEvents / 100;
            for (int i = 0; i < pcntCount; i++)
            {
                double pcnt = 0;
                if (totalEvents > 0) pcnt = (double)events[i] / r;
                retentionPcntList.Add(depths[i], pcnt);
            }
        }

        static int CreateRunoffFreqPlot(int mit)
        {
            // Case of no rainfall
            if (rainDayCount == 0) return 0;

            // Create temporary array to hold rainfall & runoff depths
            double[] sortedRain = new double[rainDayCount];
            double[] sortedRunoff = new double[rainDayCount];

            // Place measureable rainfall / runoff days into temp array
            int rainDays = 0;
            int runoffDays = 0;
            for (int i = 0; i < rainDayCount; i++)
            {
                if (rainDepths[i] > runoffThreshold)
                {
                    sortedRain[rainDays] = rainDepths[i];
                    rainDays++;
                }
                if (dryDays[i] < mit) continue;
                if (runoffDepths[i] > runoffThreshold)
                {
                    sortedRunoff[runoffDays] = runoffDepths[i];
                    runoffDays++;
                }
            }

            //Sort temporary array
            if (rainDays == 0 && runoffDays == 0) return 0;
            if (runoffDays > 0) Array.Sort(sortedRunoff, 0, runoffDays);
            if (rainDays > 0) Array.Sort(sortedRain, 0, rainDays);

            // Find total years simulated
            double years = totalDays / 365.0;

            //For each day with measureable rainfall
            int tenths = (int)(sortedRain[0] * 10.0);
            for (int j = 0; j < rainDays; j++)
            {
                // Only plot at intervals of 0.2 inches
                if ((int)(sortedRain[j] * 10.0) < tenths) continue;

                // Convert frequency to days per year
                double f = (rainDays - j) / years;

                // Add runoff and days/year to frequency plot 
                rainFreqList.Add(new PointPair(sortedRain[j], f));
                tenths += 2;
            }

            // For each day with measureable runoff
            tenths = (int)(sortedRunoff[0] * 10.0);
            for (int j = 0; j < runoffDays; j++)
            {
                // Only plot at intervals of 0.2 inches
                if ((int)(sortedRunoff[j] * 10.0) < tenths) continue;

                // Convert frequency to days per year
                double f = (runoffDays - j) / years;

                // Add runoff and days/year to frequency plot 
                runoffFreqList.Add(new PointPair(sortedRunoff[j], f));
                tenths += 2;
            }
            return runoffDays;
        }

        static void ComputeSummaryStats(int mit)
        {
            double years = totalDays / 365.0;

            runoffStats[(int)Statistics.ANNUAL_RAINFALL] = annualRainfall;
            runoffStats[(int)Statistics.ANNUAL_RUNOFF] = annualRunoff;

            runoffStats[(int)Statistics.RAINFALL_DAYS] = rainDayCount / years;
            runoffStats[(int)Statistics.RUNOFF_DAYS] = runoffDayCount / years;

            ComputeCaptureStats(mit);
            runoffStats[(int)Statistics.PCT_STORMS_CAP] = captureRatio * 100;
            runoffStats[(int)Statistics.MIN_RUNOFF_DEPTH] = minRunoffDepth;
            runoffStats[(int)Statistics.MAX_CAPTURE_DEPTH] = maxCaptureDepth;
            runoffStats[(int)Statistics.MAX_RETENTION] = maxRetention;
        }

        static void ComputeCaptureStats(int mit)
        {
            // Initialize results
            minRunoffDepth = 0;
            maxCaptureDepth = 0;
            maxRetention = 0;
            captureCoeff = 0;
            captureRatio = 0;
            if (rainDayCount == 0) return;

            // Initialize cumulative totals
            double rainfall = 0;
            double runoff = 0;
            double retention = 0;
            int captureCount = 0;
            int eventCount = 0;
            bool firstRunoff = false;

            // Check each wet day (previously sorted by rainfall amount)
            for (int i = 0; i < rainDayCount; i++)
            {
                // Skip back to back wet days 
                int k = rainIndex[i];
                if (dryDays[k] < mit) continue;
                eventCount++;

                // Add to total rainfall & runoff
                rainfall += rainDepths[k];

                // Check if runoff occurs
                if (runoffDepths[k] > runoffThreshold)
                {
                    if (!firstRunoff)
                    {
                        minRunoffDepth = rainDepths[k];
                        firstRunoff = true;
                    }
                    runoff += runoffDepths[k];
                }

                // Otherwise update capture depth and days captured
                else
                {
                    maxCaptureDepth = rainDepths[k];
                    captureCount++;
                }

                // Update max. retention depth
                retention = rainDepths[k] - runoffDepths[k];
                if (retention > maxRetention) maxRetention = retention;
            }

            // Compute capture coeff. (volume based) and capture ratio (event based)
            captureCoeff = 1 - runoff / rainfall;
            captureCoeff = Math.Max(0, captureCoeff);
            if (eventCount == 0) captureRatio = 0;
            else if (captureCoeff == 0.0) captureRatio = 0.0;
            else captureRatio = (double)captureCount / (double)eventCount;
            int runoffCount = eventCount - captureCount;
            eventCountStr = runoffCount.ToString() + " runoff events out of " +
                eventCount.ToString() + " total.";
        }

        public static void FindPcntRetention(double target, bool isPcntTarget,
            out double rainRetained, out double eventsRetained)
        {
            int captureCount = 0;
            int eventCount = 0;
            double runoffSum = 0;
            double rainfallSum = 0;
            double targetDepth = target;
            if (isPcntTarget) targetDepth = GetDepthFromPcnt(target);
            int mit = 0;
            if (MainForm.ignoreConsecDays) mit = 2;
            for (int i = 0; i < rainDayCount; i++)
            {
                int k = rainIndex[i];
                if (rainDepths[k] > targetDepth) break;
                if (dryDays[k] < mit) continue;
                eventCount++;
                rainfallSum += rainDepths[k];
                if (runoffDepths[k] > runoffThreshold) runoffSum += runoffDepths[k];
                else captureCount++;
            }
            if (rainfallSum == 0) rainRetained = 0;
            else rainRetained = 1 - runoffSum / rainfallSum;
            rainRetained = Math.Max(0, rainRetained) * 100;
            if (eventCount == 0) eventsRetained = 0;
            else if (rainRetained == 0.0) eventsRetained = 0.0;
            else eventsRetained = (double)captureCount / (double)eventCount * 100;
        }

        public static double GetDepthFromPcnt(double target)
        {
            if (rainDayCount == 0) return 0;
            int index = (int)Math.Ceiling(target / 100 * rainDayCount) - 1;
            if (index < 0) index = 0;
            if (index >= rainDayCount) index = rainDayCount - 1;
            return rainDepths[rainIndex[index]];
        }

        public static string GetEventCounts()
        {
            return eventCountStr;
        }

        public static void GetWaterBudget(int years, ref double rainfall,
            ref double runoff, ref double infil, ref double evap)
        {
            rainfall = 0;
            runoff = 0;
            infil = 0;
            evap = 0;
            string line;
            double factor = 12.0 / SiteData.area / years;
            StreamReader sr = new StreamReader(SiteData.rptFile);
            try
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.Contains("  Total Precipitation"))
                    {
                        rainfall = GetValueFromLine(line) * factor;
                        evap = GetValueFromLine(sr.ReadLine()) * factor;
                        infil = GetValueFromLine(sr.ReadLine()) * factor;
                    }
                    if (line.Contains("  External Outflow"))
                    {
                        runoff += GetValueFromLine(line) * factor;
                        runoff += GetValueFromLine(sr.ReadLine()) * factor;
                    }

                    if (line.Contains("  Exfiltration Loss"))
                    {
                        infil += GetValueFromLine(line) * factor;
                        break;
                    }
                }
            }
            finally
            {
                sr.Close();
            }
        }

        private static double GetValueFromLine(string line)
        {
            string[] words = line.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);
            if (words.Length <= 3) return 0;
            return Double.Parse(words[3]);
        }

    }
}
