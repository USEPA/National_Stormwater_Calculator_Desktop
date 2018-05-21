// ----------------------------------------------------------------------------
// Module:      Graphs.cs
// Project:     EPA Stormwater Calculator
// Description: Code module that handles graphing functions 
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.0
// Last Update: 10/14/13
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ZedGraph;

namespace StormwaterCalculator
{
    class Graphs
    {
        // Curve colors
        static Color[] colors =
            { Color.SteelBlue, Color.RosyBrown, Color.PowderBlue, Color.Wheat };
            //{ Color.Blue, Color.Green, Color.Cyan, Color.Lime };

        // Slices for the water budget pie charts
        static PieItem[] pieSlices7 = new PieItem[3];
        static PieItem[] pieSlices8 = new PieItem[3];

        // Rainfall label for water budget pie charts
        static string annualRainfall;

        // Axis labels for the extreme event chart
        static string[] returnPeriodLabels1 =
        { "", "5", "", "10", "", "15", "", "30", "", "50", "", "100", "" };
        static string[] returnPeriodLabels2 =
        { "5", "5", "", "10", "10", "", "15", "15", "", "30", "30", "", "50", "50", "", "100", "100" };

        // Baseline Bar items
        static BarItem baselineBar1;
        static BarItem baselineBar2;
        static BarItem baselineBar3;
        static BarItem baselineBar4;

        // Baseline Line items
        static LineItem baselineCurve1;
        static LineItem baselineCurve2;

        static void SetCommonProperties(GraphPane myPane)
        {
            myPane.IsFontsScaled = false;
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;


            myPane.XAxis.Scale.FontSpec.IsAntiAlias = true; // false;
            myPane.YAxis.Scale.FontSpec.IsAntiAlias = true;  // false;
            myPane.YAxis.Scale.FontSpec.Size = 10;
            myPane.XAxis.Scale.FontSpec.Size = 10;
            myPane.YAxis.Scale.FontSpec.IsBold = true;
            myPane.XAxis.Scale.FontSpec.IsBold = true;

            myPane.Legend.FontSpec.Size = 10;
            myPane.Title.FontSpec.Size = 14;
            myPane.XAxis.Title.FontSpec.Size = 12;
            myPane.YAxis.Title.FontSpec.Size = 12;
            myPane.XAxis.Title.FontSpec.IsAntiAlias = true; // false;
            myPane.YAxis.Title.FontSpec.IsAntiAlias = true;  // false;

            myPane.Legend.Border.IsVisible = false;
            myPane.Legend.Position = LegendPos.TopCenter;
            myPane.Legend.FontSpec.IsBold = true;
            myPane.Legend.FontSpec.IsAntiAlias = false;
            myPane.Legend.IsVisible = false;

            // Apply a gradient fill to the chart area
            myPane.Chart.Fill = new Fill(Color.FromArgb(220, 220, 255), Color.White, 45);
            //myPane.Chart.Fill = new Fill(Color.AliceBlue);
        }

        public static void SetSymbolProperties(LineItem myCurve, Color c)
        {
            myCurve.Symbol.Size = 8;
            myCurve.Symbol.Fill.Type = FillType.Solid;
            myCurve.Symbol.Fill.Color = c;
            myCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        public static void CreateRainRunoffPlot(ZedGraphControl zgc)
        {
            // Display point values
            zgc.PointValueFormat = "F2";
            zgc.IsShowPointValues = true;

            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            SetCommonProperties(myPane);
            //myPane.Legend.IsVisible = true;

            // Set the Titles
            myPane.Title.Text = "Rainfall / Runoff Events";
            myPane.XAxis.Title.Text = "Daily Rainfall (inches)";
            myPane.YAxis.Title.Text = "Daily Runoff (inches)";

            // Create data arrays
            Runoff.rainRunoffList = new PointPairList();
            Runoff.baseRainRunoffList = new PointPairList();

            // Generate curves for current and baseline results
            LineItem myCurve1 = myPane.AddCurve("Current Scenario",
                  Runoff.rainRunoffList, Color.Black, SymbolType.Square);
            LineItem myCurve2 = myPane.AddCurve("Baseline Scenario",
                  Runoff.baseRainRunoffList, Color.Black, SymbolType.Square);

            // Set curve widths
            myCurve1.Line.IsVisible = false;
            myCurve2.Line.IsVisible = false;
            SetSymbolProperties(myCurve1, colors[0]);
            SetSymbolProperties(myCurve2, colors[1]);
            myCurve1.Symbol.Size = 6;
            myCurve2.Symbol.Size = 6;

            // Tell ZedGraph to refigure the axes since the data have changed
            zgc.AxisChange();
        }

        public static void CreateRunoffFreqPlot(ZedGraphControl zgc)
        {
            // Display point values
            zgc.PointValueFormat = "F2";
            zgc.IsShowPointValues = true;

            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            SetCommonProperties(myPane);
            myPane.Legend.IsVisible = true;
 
            // Set the Titles
            myPane.Title.Text = "Rainfall / Runoff Exceedance Frequency";
            myPane.XAxis.Title.Text = "Depth (inches)";
            myPane.YAxis.Title.Text = "Days per Year Exceeded";

            // Axis grid lines
            myPane.XAxis.MajorGrid.DashOff = 0;

            myPane.YAxis.MinorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.DashOff = 0;
            myPane.YAxis.MinorGrid.DashOff = 0;

            // Y axis scale properties
            myPane.YAxis.Type = AxisType.Log;
            myPane.YAxis.Scale.IsUseTenPower = false;
            myPane.YAxis.Scale.Min = 0.1;
            myPane.YAxis.Scale.MinAuto = false;

            // Create data arrays
            Runoff.runoffFreqList = new PointPairList();
            Runoff.baseRunoffFreqList = new PointPairList();
            Runoff.rainFreqList = new PointPairList();
            Runoff.baseRainFreqList = new PointPairList();

            // Create a curve for current rainfall frequency
            LineItem myCurve1 = myPane.AddCurve("Rainfall", 
                  Runoff.rainFreqList, colors[0], SymbolType.None);
            myCurve1.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            myCurve1.Line.Width = 3;
            myCurve1.Line.IsAntiAlias = true;
            //myCurve1.Line.IsSmooth = true;

            // Create a curve for current runoff frequency
            LineItem myCurve2 = myPane.AddCurve("Runoff",  //"Current Scenario",
                  Runoff.runoffFreqList, colors[0], SymbolType.None);
            myCurve2.Line.Width = 3;
            myCurve2.Line.IsAntiAlias = true;
            //myCurve2.Line.IsSmooth = true;

            // Create a curve for baseline rainfall frequency
            baselineCurve1 = myPane.AddCurve("Baseline Rainfall",
                  Runoff.baseRainFreqList, colors[1], SymbolType.None);
            baselineCurve1.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            baselineCurve1.Line.Width = 3;
            baselineCurve1.Line.IsAntiAlias = true;
            //baselineCurve1.Line.IsSmooth = true;
            baselineCurve1.Label.IsVisible = false;

            // Create a curve for baseline runoff frequency
            baselineCurve2 = myPane.AddCurve("Baseline Runoff",
                  Runoff.baseRunoffFreqList, colors[1], SymbolType.None);
            baselineCurve2.Line.Width = 3;
            baselineCurve2.Line.IsAntiAlias = true;
            //baselineCurve2.Line.IsSmooth = true;
            baselineCurve2.Label.IsVisible = false;

            // Tell ZedGraph to refigure the axes since the data have changed
            zgc.AxisChange();
        }

        public static void UpdateRunoffFreqPlot(ZedGraphControl zgc, bool showBaseline)
        {
            baselineCurve1.Label.IsVisible = showBaseline;
            baselineCurve2.Label.IsVisible = showBaseline;
        }

        public static void CreateRunoffPcntPlot(ZedGraphControl zgc)
        {
            // Display point values
            zgc.PointValueFormat = "F2";
            zgc.IsShowPointValues = true;

            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            SetCommonProperties(myPane);

            // Set the titles
            myPane.Title.Text = "Runoff Contribution by Rainfall Percentile";
            myPane.XAxis.Title.Text = "Daily Rainfall Percentile";
            myPane.X2Axis.Title.Text = "Daily Rainfall Depth (inches)";
            myPane.YAxis.Title.Text = "Percentage of Total Runoff";

            // Set legend properties
            myPane.Legend.Position = LegendPos.InsideTopLeft;

            // Create data arrays
            Runoff.runoffPcntList = new PointPairList();
            Runoff.baseRunoffPcntList = new PointPairList();

            // Generate bar items
            BarItem myCurve3 = myPane.AddBar("Current Scenario",
                Runoff.runoffPcntList, colors[0]);
            BarItem myCurve4 = myPane.AddBar("Baseline Scenario",
                Runoff.baseRunoffPcntList, colors[1]);

            // Assign labels to both bottom and top X-axes
            myPane.XAxis.Scale.TextLabels = Runoff.PcntLabels;
            myPane.XAxis.Type = AxisType.Text;
            myPane.X2Axis.IsVisible = true;
            myPane.X2Axis.Type = AxisType.Text;
            myPane.X2Axis.Scale.FontSpec.Size = 12;
            myPane.X2Axis.Scale.FontSpec.IsAntiAlias = false;

            // Tell ZedGraph to refigure the axes since the data have changed
            zgc.AxisChange();
        }

        public static void CreateExtremeEventPlot(ZedGraphControl zgc)
        {
            // Display point values
            zgc.PointValueFormat = "F2";
            zgc.IsShowPointValues = true;

            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            SetCommonProperties(myPane);
            myPane.Legend.IsVisible = true;
/*
            myPane.Legend.FontSpec.Size = 10;
            myPane.Title.FontSpec.Size = 12;
            myPane.XAxis.Title.FontSpec.Size = 12;
            myPane.YAxis.Title.FontSpec.Size = 12;
*/
            // Set the graph's titles
            myPane.Title.Text = "Extreme Event Rainfall / Runoff Depth";
            myPane.XAxis.Title.Text = ""; // "Return Period (years)";
            myPane.XAxis.Type = AxisType.Text;
            myPane.YAxis.Title.Text = "Depth (inches)";
            myPane.XAxis.Scale.TextLabels = returnPeriodLabels1;
            myPane.XAxis.MajorTic.Size = 0;
            myPane.XAxis.MajorGrid.IsVisible = false;

            // Create data arrays
            Xevent.extremeRainfallList = new PointPairList();
            Xevent.extremeRunoffList = new PointPairList();
            Xevent.baseExtremeRainfallList = new PointPairList();
            Xevent.baseExtremeRunoffList = new PointPairList();

            // Add bars for rainfall and runoff
            BarItem myBar1 = myPane.AddBar(
                "Rainfall", Xevent.extremeRainfallList, colors[2]);
            BarItem myBar2 = myPane.AddBar(
                "Runoff", Xevent.extremeRunoffList, colors[0]);
            baselineBar1 = myPane.AddBar(
                "Base Rainfall", Xevent.baseExtremeRainfallList, colors[3]);
            baselineBar2 = myPane.AddBar(
                "Base Runoff", Xevent.baseExtremeRunoffList, colors[1]);

            // Overlay the bars
            myPane.BarSettings.Type = BarType.SortedOverlay;
            baselineBar1.Label.IsVisible = false;
            baselineBar2.Label.IsVisible = false;

            // Tell ZedGraph to refigure the axes since the data have changed
            zgc.AxisChange();
        }

        public static void UpdateExtremeEventPlot(ZedGraphControl zgc, bool showBaseline)
        {
            GraphPane myPane = zgc.GraphPane;
            if (showBaseline) myPane.XAxis.Scale.TextLabels = returnPeriodLabels2;
            else myPane.XAxis.Scale.TextLabels = returnPeriodLabels1;
            baselineBar1.Label.IsVisible = showBaseline;
            baselineBar2.Label.IsVisible = showBaseline;
        }

        public static void CreateExtremePeakPlot(ZedGraphControl zgc)
        {
            // Display point values
            zgc.PointValueFormat = "F2";
            zgc.IsShowPointValues = true;

            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            SetCommonProperties(myPane);
            myPane.Legend.IsVisible = false;
/*
            myPane.Title.FontSpec.Size = 12;
            myPane.XAxis.Title.FontSpec.Size = 12;
            myPane.YAxis.Title.FontSpec.Size = 12;
*/
            // Set the graph's titles
            myPane.Title.Text = "Extreme Event Peak Rainfall / Runoff";
            myPane.XAxis.Title.Text = "Return Period (years)";
            myPane.XAxis.Type = AxisType.Text;
            myPane.YAxis.Title.Text = "Intensity (in/hr)";
            myPane.XAxis.Scale.TextLabels = returnPeriodLabels1;
            myPane.XAxis.MajorTic.Size = 0;
            myPane.XAxis.MajorGrid.IsVisible = false;

            // Create data arrays
            Xevent.peakRainfallList = new PointPairList();
            Xevent.peakRunoffList = new PointPairList();
            Xevent.basePeakRainfallList = new PointPairList();
            Xevent.basePeakRunoffList = new PointPairList();

            // Add bars for rainfall and runoff
            BarItem myBar1 = myPane.AddBar(
                "Rainfall", Xevent.peakRainfallList, colors[2]);
            BarItem myBar2 = myPane.AddBar(
                "Runoff", Xevent.peakRunoffList, colors[0]);
            baselineBar3 = myPane.AddBar(
                "Base Rainfall", Xevent.basePeakRainfallList, colors[3]);
            baselineBar4 = myPane.AddBar(
                "Base Runoff", Xevent.basePeakRunoffList, colors[1]);

            // Overlay the bars
            myPane.BarSettings.Type = BarType.SortedOverlay;
            baselineBar3.Label.IsVisible = false;
            baselineBar4.Label.IsVisible = false;

            // Tell ZedGraph to refigure the axes since the data have changed
            zgc.AxisChange();
        }

        public static void CreateRainfallCapturePlot(ZedGraphControl zgc)
        {
            // Display point values
            zgc.PointValueFormat = "F2";
            zgc.IsShowPointValues = true;

            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            SetCommonProperties(myPane);

            // Set the graph's titles
            myPane.Title.Text = "Rainfall Retention Frequency";
            myPane.XAxis.Title.Text = "Daily Rainfall (inches)";
            myPane.YAxis.Title.Text = "Percent of Time Retained";

            // Create data arrays
            Runoff.retentionPcntList = new PointPairList();
            Runoff.baseRetentionPcntList = new PointPairList();

            // Generate curves for current and baseline results
            LineItem myCurve1 = myPane.AddCurve("Current Scenario",
                  Runoff.retentionPcntList, colors[0], SymbolType.None);
            LineItem myCurve2 = myPane.AddCurve("Baseline Scenario",
                  Runoff.baseRetentionPcntList, colors[1], SymbolType.None);

            // Set curve widths
            myCurve1.Line.Width = 3;
            myCurve2.Line.Width = 3;

            // Tell ZedGraph to refigure the axes since the data have changed
            zgc.AxisChange();
        }

        public static void CreateMonthlyAdjustPlot(ZedGraphControl zgc)
        {
            // Display point values
            zgc.PointValueFormat = "F2";
            zgc.IsShowPointValues = true;

            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            SetCommonProperties(myPane);

            // Set the graph's titles
            myPane.Title.Text = "Percentage Change in Monthly Rainfall";
            //myPane.Title.FontSpec.Size = 14;
            myPane.XAxis.Title.Text = "";
            myPane.YAxis.Title.Text = "";

            // Set the legend
            myPane.Legend.FontSpec.IsBold = true;
            myPane.Legend.IsVisible = true;

            // Create data arrays for monthly avg. rainfall changes
            Climate.rainDelta1 = new PointPairList();
            Climate.rainDelta2 = new PointPairList();
            Climate.rainDelta3 = new PointPairList();            

            // Generate curves for each data array
            LineItem myCurve1 = myPane.AddCurve("Hot/Dry", Climate.rainDelta1,
                Color.Black, SymbolType.TriangleDown);
            LineItem myCurve2 = myPane.AddCurve("Median", Climate.rainDelta2,
                Color.Black, SymbolType.Diamond);
            LineItem myCurve3 = myPane.AddCurve("Warm/Wet", Climate.rainDelta3,
                Color.Black, SymbolType.Triangle);
            //myCurve1.Line.Width = 2;
            SetSymbolProperties(myCurve1, Color.Red);
            SetSymbolProperties(myCurve2, Color.Gray);
            SetSymbolProperties(myCurve3, Color.Cyan);

            // Assign labels to the X-axis
            myPane.XAxis.Scale.TextLabels = Climate.monthLabels;
            myPane.XAxis.Type = AxisType.Text;
            //myPane.XAxis.Scale.FontSpec.IsBold = true;
            //myPane.YAxis.Scale.FontSpec.IsBold = true;

            // Tell ZedGraph to refigure the axes since the data have changed
            zgc.AxisChange();
        }

        public static void CreateAnnualMaxAdjustPlot(ZedGraphControl zgc)
        {
            // Display point values
            zgc.PointValueFormat = "F2";
            zgc.IsShowPointValues = true;

            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            SetCommonProperties(myPane);

            // Set the graph's titles
            myPane.Title.Text = "Maximum Day Rainfall (inches)";
            //myPane.Title.FontSpec.Size = 14;
            myPane.XAxis.Title.Text = "Return Period (years)";
            myPane.YAxis.Title.Text = "";

            // Set the legend
            myPane.Legend.FontSpec.IsBold = true;
            myPane.Legend.IsVisible = true;

            // Create data arrays for annual max. rainfall
            Climate.maxrain1 = new PointPairList();
            Climate.maxrain2 = new PointPairList();
            Climate.maxrain3 = new PointPairList();
            Climate.maxrain0 = new PointPairList();

            // Create data arrays for monthly ET
            // (Used only in Climate.cs)
            Climate.evap1 = new PointPairList();
            Climate.evap2 = new PointPairList();
            Climate.evap3 = new PointPairList();
            Climate.evap0 = new PointPairList();

            // Generate curves for each data array
            LineItem myCurve1 = myPane.AddCurve("Hot/Dry", Climate.maxrain1,
                Color.Black, SymbolType.Triangle);
            LineItem myCurve2 = myPane.AddCurve("Median", Climate.maxrain2,
                Color.Black, SymbolType.Diamond);
            LineItem myCurve3 = myPane.AddCurve("Warm/Wet", Climate.maxrain3,
                Color.Black, SymbolType.TriangleDown);
            LineItem myCurve4 = myPane.AddCurve("Historical", Climate.maxrain0,
                Color.Black, SymbolType.Circle);
            SetSymbolProperties(myCurve1, Color.Red);
            SetSymbolProperties(myCurve2, Color.Gray);
            SetSymbolProperties(myCurve3, Color.Cyan);
            SetSymbolProperties(myCurve4, Color.Lime);

            // Assign labels to the X-axis
            myPane.XAxis.Scale.TextLabels = Climate.returnPeriods;
            myPane.XAxis.Type = AxisType.Text;
            //myPane.XAxis.Scale.FontSpec.IsBold = true;
            //myPane.YAxis.Scale.FontSpec.IsBold = true;
           
            // Tell ZedGraph to refigure the axes since the data have changed
            zgc.AxisChange();
        }

        public static void SetMonthlyAdjustPlotTitle(ZedGraphControl zgc)
        {
            string period;
            if (SiteData.climateYear == 2035) period = "Near Term";
            else period = "Far Term";
            string title = "Percentage Change in Monthly Rainfall for " + 
                period + " Projections";
            zgc.GraphPane.Title.Text = title;
        }

        public static void SetAnnualMaxAdjustPlotTitle(ZedGraphControl zgc)
        {
            string period;
            if (SiteData.climateYear == 2035) period = "Near Term";
            else period = "Far Term";
            string title = "Annual Max. Day Rainfall (inches) for " + 
                period + " Projections";
            zgc.GraphPane.Title.Text = title;
        }

        public static void CreateAnnualBudgetPlot(ZedGraphControl zgc)
        {
            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            SetCommonProperties(myPane);
            myPane.Chart.Fill = new Fill(Color.White);

            // Set the Titles
            myPane.Title.Text = annualRainfall;
            myPane.Title.FontSpec.Size = 12f;
            myPane.Border.IsVisible = false;

            // Set the legend properties
            myPane.Legend.IsVisible = true;
            myPane.Legend.Position = LegendPos.BottomCenter;
           
            // Add some pie slices
            pieSlices7[0] = myPane.AddPieSlice(Runoff.annualRunoff, Color.Blue, Color.White, 45f, 0, "Runoff");
            pieSlices7[1] = myPane.AddPieSlice(Runoff.annualInfil, Color.Green, Color.White, 45f, 0, "Infil.");
            pieSlices7[2] = myPane.AddPieSlice(Runoff.annualEvap, Color.Red, Color.White, 45f, 0, "Evap.");

            for (int i = 0; i < 3; i++)
            {
                pieSlices7[i].LabelType = PieLabelType.Percent;
                pieSlices7[i].LabelDetail.FontSpec.Size = 12f;
                pieSlices7[i].LabelDetail.FontSpec.Border.IsVisible = false;
                pieSlices7[i].Border.IsAntiAlias = true;
                pieSlices7[i].PercentDecimalDigits = 0;
             }

            // Tell ZedGraph to refigure the axes since the data have changed
            zgc.AxisChange();
        }

        public static void UpdateAnnualBudgetPlot(ZedGraphControl zgc, double rainfall,
            double runoff, double infil, double evap)
        {
            annualRainfall = rainfall.ToString("F2");
            zgc.GraphPane.Title.Text =
                "Current Scenario\n\n Annual Rainfall = " + annualRainfall + " inches";
            pieSlices7[0].Value = runoff;
            pieSlices7[1].Value = infil;
            pieSlices7[2].Value = evap;
        }

        public static void CreateBaseAnnualBudgetPlot(ZedGraphControl zgc)
        {
            // Get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            SetCommonProperties(myPane);
            myPane.Chart.Fill = new Fill(Color.White);

            // Set the Titles
            myPane.Title.Text = annualRainfall;
            myPane.Title.FontSpec.Size = 12f;
            myPane.Border.IsVisible = false;

            // Set the legend properties
            myPane.Legend.IsVisible = true;
            myPane.Legend.Position = LegendPos.BottomCenter;

            // Add some pie slices
            pieSlices8[0] = myPane.AddPieSlice(0, Color.Blue, Color.White, 45f, 0, "Runoff");
            pieSlices8[1] = myPane.AddPieSlice(0, Color.Green, Color.White, 45f, 0, "Infil.");
            pieSlices8[2] = myPane.AddPieSlice(0, Color.Red, Color.White, 45f, 0, "Evap.");

            for (int i = 0; i < 3; i++)
            {
                pieSlices8[i].LabelType = PieLabelType.Percent;
                pieSlices8[i].LabelDetail.FontSpec.Size = 12f;
                pieSlices8[i].LabelDetail.FontSpec.Border.IsVisible = false;
                pieSlices8[i].Border.IsAntiAlias = true;
                pieSlices8[i].PercentDecimalDigits = 0;
            }

            // Tell ZedGraph to refigure the axes since the data have changed
            zgc.AxisChange();
            zgc.Visible = false;
        }

        public static void UpdateBaseAnnualBudgetPlot(ZedGraphControl zgc)
        {
            zgc.GraphPane.Title.Text =
                "Baseline Scenario\n\n Annual Rainfall = " + annualRainfall + " inches";
            pieSlices8[0].Value = pieSlices7[0].Value;
            pieSlices8[1].Value = pieSlices7[1].Value;
            pieSlices8[2].Value = pieSlices7[2].Value;
        }

        public static void SetBorderVisible(ZedGraphControl zgc, bool isVisible)
        {
            zgc.GraphPane.Border.IsVisible = isVisible;
            zgc.AxisChange();
        }

        public static void Refresh(ZedGraphControl zgc)
        {
            zgc.Visible = true;
            zgc.AxisChange();
            zgc.Refresh();
        }

    }
}
