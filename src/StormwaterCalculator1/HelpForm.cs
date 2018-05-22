// ----------------------------------------------------------------------------
// Module:      HelpForm.cs
// Project:     EPA Stormwater Calculator
// Description: Code module for a Form class that displays context sensitive
//              help for the Stormwater Calculator.
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.0
// Last Update: 10/14/13
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StormwaterCalculator
{
    public partial class HelpForm : Form
    {

        static string newLine = System.Environment.NewLine;

        int topic = -1;

        string soilHelp =
        "Soil type is identified by its Hydrologic Soil " +
        "Group, a classification used by soil scientists to " +
        "characterize the physical nature and runoff potential " +
        "of a soil. Roughly speaking, Group A is sand, Group B " +
        "sandy loam, Group C clay loam, and Group D clay. The " +
        "Calculator uses soil type to infer a site's " +
        "infiltration properties." +
        newLine + newLine +
        "Check the 'View Soil Survey' box to view the " +
        "soil types around your site. (Soil type data may " +
        "not be available for your particular site.) Select " +
        "a soil type from the choices listed or click " +
        "a shaded region of the map to select its value.";

        string ksatHelp =
        "The rate at which standing water infiltrates into a " +
        "soil is measured by its saturated hydraulic conductivity. " +
        "Soils with higher conductivity produce less runoff." +
        newLine + newLine +
        "Check the 'View Soil Survey' box to view soil " +
        "conductivity around your site. (Conductivity " +
        "data might not be available for your particular site.)" +
        newLine + newLine +
        "Click a shaded region on the map to select its " +
        "conductivity value into the edit box, or you can enter " +
        "your own conductivity value directly." +
        newLine + newLine +
        "If you leave the edit box blank, the default conductivity " +
        "associated with the site's soil type will be used.";

        string slopeHelp =
        "Site topography, as measured by surface slope " +
        "(feet of drop per 100 feet of length), affects " +
        "how fast stormwater will run off a site. Flatter " +
        "slopes produce slower runoff flow rates and " +
        "provide more opportunity for rainfall to " +
        "infiltrate into the soil." +
        newLine + newLine +
        "Check the 'View Soil Survey' box to view land " +
        "surface slope on the map. (Slope data may not be " +
        "available for your particular site.)" +
        newLine + newLine +
        "Select a slope from the choices listed above or click " +
        "a shaded region on the map to select its value.";

        string rainHelp =
        "The Calculator computes runoff for your site using " +
        "a long-term record of historical hourly rainfall data " +
        "recorded at a nearby National Weather Service rain gage. " +
        newLine + newLine +
        "Select the rain gage you would like the Calculator to use. " +
        "The period of record and average annual rainfall are listed " +
        "below the name of each gaging station." +
        newLine + newLine +
        "Use the Save Rainfall Data option only if you need to " +
        "use the data in some other application.";

        string evapHelp =
        "Monthly evaporation rates have been calculated from " +
        "historical daily temperature measurements recorded at " +
        "the closest National Weather Service weather stations " +
        "to your site. The period of record and average potential " +
        "daily rate are listed next to each station's name." +
        newLine + newLine +
        "Select the weather station you would like to use " +
        "as the source of evaporation rates for your site." +
        newLine + newLine +
        "Use the Save Evaporation Data option only if you need to " +
        "use the data in some other application.";

        string climateHelp =
        "The effect that climate change has on site hydrology can be " +
        "examined by selecting from a set of downscaled climate " +
        "projections produced by the World Climate Research Programme. " +
        "A range of future changes in temperature and precipitation have " +
        "been generated for each National Weather Service station in the " +
        "calculator's database." +
        newLine + newLine +
        "There are three climate scenarios to choose from that " +
        "span the range from hot and dry conditions to warm and " +
        "wet ones. Projections for each scenario are available for " +
        "two future periods in time, 2020-2049 and 2045-2074." +
        newLine + newLine +
        "The effect that these climate projections have on monthly " +
        "average rainfall and annual extreme rainfall events are " +
        "shown graphically in the right hand panel. These effects " +
        "will change with choice of rain gage location and projection " +
        "year." +
        newLine + newLine +
        "(An annual max. day rainfall X with a return period of Y " +
        "years means that on average, a 24-hour storm greater than X " +
        "inches will occur only once every Y years.)" +
        newLine + newLine +
        "The effect of future temperature changes on monthly evaporation " +
        "rates were also considered but are not shown because the " + 
        "variation between scenarios is quite small."; 

        string landCoverHelp =
        "Enter the percentage of the site's area covered by each type " +
        "of non-impervious surface. The remaining area is considered " +
        "to be directly connected impervious surfaces (roofs, sidewalks, " +
        "streets, parking lots, etc. that drain directly off-site). " +
        "Disconnecting some of this area, to run onto lawns for example, " +
        "is an LID option appearing on the next page." +
        newLine + newLine +
        "Choose a land cover distribution that reflects the stage of " +
        "development being analyzed, such as pre-development, current or " +
        "future development. Total runoff volume is highly dependent on " +
        "the amount of impervious area and less so on how the non-impervious " +
        "area is divided between the different land cover categories." +
        newLine + newLine +
        "Non-impervious land cover type will affect the amount of rainfall " +
        "captured on vegetation or in natural depressions. It also determines " +
        "surface roughness. Rougher surfaces slow down overland flow allowing " +
        "more opportunity for infiltration.";
       
        string lidHelp =
        "Low Impact Development (LID) controls are landscaping " +
        "practices designed to collect runoff from impervious " +
        "surfaces and retain it on site." +
        newLine + newLine +
        "Enter the percent of the site's impervious area you " +
        "would like to be treated by the listed LID practices." +
        newLine + newLine +
        "Click a practice to learn more about it or to change " +
        "its design parameters." +
        newLine + newLine +
        "Entering a non-zero design storm depth will allow you " +
        "to automatically size an LID control to capture storms " +
        "of that size when you click on the LID's name to bring " +
        "up its design form.";

        string resultsHelp =
        "The Results page is where the site's hydrologic response to a " +
        "long term period of historical hourly rainfall is computed " +
        "and reported on. Statistics for both annual and daily " +
        "rainfall/runoff are presented." +
        newLine + newLine +
        "The user controls on this page are grouped together in three " +
        "sections: Options, Actions, and Reports." +
        newLine + newLine +
        "The Options section allows you to specify how the rainfall " +
        "record should be analyzed with respect to:" +
        newLine +
        "- the number of most recent years of rainfall record to use, " +
        newLine +
        "- the minimum amount of daily rainfall (or runoff) that will " +
        "constitute a measureable event," +
        newLine +
        "- whether subsequent days of back to back daily " +
        "events should be counted or not." +
        newLine + newLine +
        "The Actions section contains commands that allow you to:" +
        newLine +
        "- Refresh results after site data have changed." +
        newLine +
        "- Use the most current results as a baseline scenario " +
        "that can be compared with results from subsequent runs." +
        newLine +
        "- Print the current (and baseline) results to a PDF file." +
        newLine + newLine +
        "The Reports section allows you to select how the rainfall/runoff " +
        "results generated for the site should be displayed. To learn more " +
        "about the contents of a selected report click the Help link at " +
        "the bottom right of the page.";

        string SiteDescriptionTxt =
            "The Site Description report summarizes the data " +
            "supplied for your site, including the choice of LID " +
            "controls used." +
            newLine + newLine +
            "Pairs of values for each LID control represent the percent of " +
            "impervious area treated by the LID and the area of the LID " +
            "unit itself as a percentage of the treated area.";

        string SummaryResultsTxt =
            "The Summary Results report summarizes runoff results for your site." +
            newLine + newLine +
            "The pie chart shows how the total annual rainfall for the site " +
            "gets divided between runoff, infiltration, and evaporation. Note " +
            "that because the calculator does not explicitly compute " +
            "vegetative transpiration, the latter quantity shows up as " +
            "infiltration." +
            newLine + newLine +
            "The table below the pie chart lists the annual average rainfall " +
            "and runoff plus various statistics for days where the rainfall " +
            "or runoff is above the Event Threshold value.";

        string RunoffEventTxt =
            "The Rainfall/Runoff report contains a scatter plot of the daily " +
            "runoff depth associated with each daily rainfall event " +
            "over the period of record analyzed. Only days with rainfall " +
            "above the Event Threshold are plotted. Events that are completely " +
            "captured on site (i.e., have runoff below the Event Threshold) " +
            "show up as points that lie along the horizontal axis." +
            newLine + newLine +
            "There is not always a consistent relationship between rainfall " +
            "and runoff. Days with similar rainfall amounts can produce different " +
            "amounts of runoff depending on how that rainfall was distributed " +
            "over the day and on how much rain occurred in prior days.";

        string RunoffFrequencyTxt =
            "The Rainfall / Runoff Frequency report shows how many times per year, " +
            "on average, a given depth of daily rainfall or runoff will be exceeded." +
            newLine + newLine +
            "Only daily rainfall/runoff amounts above the stipulated Event " +
            "Threshold depth are included in this plot." +
            newLine + newLine +
            "Curves like these are useful in comparing the complete range of " +
            "rainfall / runoff results between different development, control " +
            "and climate change scenarios."; 

        string RetentionFrequencyTxt =
            "The Rainfall Retention Frequency report shows how frequently daily " +
            "rainfall up to a given depth will be retained on site." +
            newLine + newLine +
            "For a given daily rainfall depth X the corresponding percent of " +
            "time retained represents the fraction of storms below this depth " +
            "that are completely captured plus the fraction of storms above it " +
            "where at least X inches are captured." +
            newLine + newLine +
            "This plot can thus be used to estimate the reliability in " +
            "meeting a specific rainfall retention target." +
            newLine + newLine +
            "A rainfall event is considered to be completely captured if its " +
            "corresponding runoff is below the stipulated Event Threshold.";

        string RainfallPercentileTxt =
            "The Runoff By Rainfall Percentile report shows what percentage of " +
            "total measureable runoff is attributable to different size " +
            "rainfall events." +
            newLine + newLine +
            "The bottom axis is divided into intervals of " +
            "daily rainfall event percentiles. The top axis shows the " +
            "rainfall depth corresponding to each percentile. " +
            "The bars indicate what percentage of total measureable runoff is " +
            "generated by the rainfall within each size interval." +
            newLine + newLine +
            "Note: the Xth-percentile event is simply the X-th highest " +
            "daily rainfall amount, in terms of percentage, among all " +
            "days with rainfall above the Event Threshold.";

        string ExtremeEventTxt =
            "The Extreme Event Rainfall / Runoff report shows how much runoff is " +
            "produced by annual maximum daily rainfalls with different return periods." +
            newLine + newLine +
            "Each bar on the chart represents a different return period and shows " +
            "both the maximum daily rainfall amount and the runoff it produces." +
            newLine + newLine +
            "Note: an X inch rainfall with a Y year return period means that on " +
            "average, daily rainfalls above X inches will occur only once every Y " +
            "years.";
 
        public HelpForm()
        {
            InitializeComponent();
        }

        // Displays help text for a specific topic
        // (The topic numbers correspond to the tabbed page
        // indexes of the Calcualtor's main form)
        public void ShowHelp(int newTopic)
        {
            if (topic != newTopic)
            {
                topic = newTopic;
                switch (topic)
                {
                    case 2:
                        textBox1.Text = soilHelp;
                        break;
                    case 3:
                        textBox1.Text = ksatHelp;
                        break;
                    case 4:
                        textBox1.Text = slopeHelp;
                        break;
                    case 5:
                        textBox1.Text = rainHelp;
                        break;
                    case 6:
                        textBox1.Text = evapHelp;
                        break;
                    case 7:
                        textBox1.Text = climateHelp;
                        break;
                    case 8:
                        textBox1.Text = landCoverHelp;
                        break;
                    case 9:
                        textBox1.Text = lidHelp;
                        break;
                    case 10:
                        textBox1.Text = resultsHelp;
                        break;
                    case 100:
                        textBox1.Text = SiteDescriptionTxt;
                        break;
                    case 101:
                        textBox1.Text = SummaryResultsTxt;
                        break;
                    case 102:
                        textBox1.Text = RunoffEventTxt;
                        break;
                    case 103:
                        textBox1.Text = RunoffFrequencyTxt;
                        break;
                    case 104:
                        textBox1.Text = RetentionFrequencyTxt;
                        break;
                    case 105:
                        textBox1.Text = RainfallPercentileTxt;
                        break;
                    case 106:
                        textBox1.Text = ExtremeEventTxt;
                        break;
                }
            }
            textBox1.Select(0, 0);
            Show();
        }

        // Hides the form when the user tries to close it
        private void HelpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        // Hides the form when the user presses the Escape key
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Hide();
        }
    }
}
