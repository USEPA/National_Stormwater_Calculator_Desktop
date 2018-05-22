// ----------------------------------------------------------------------------
// Module:      MainForm.cs
// Project:     EPA Stormwater Calculator
// Description: Code module for a Form class that implements the main
//              form of the Stormwater Calculator.
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.2
// Last Update: 02/03/2016
//
// This form implements a tabbed page application.
//
// The following prefixes are used to name the form's controls:
//   btn = button or link label
//   cb  = combo box
//   gb  = group box
//   lv  = list view
//   nud = numeric up down
//   sc  = splitter container
//   tb  = text box
//   tc  = tabbed control
//   tp  = tab page
// 
// The layout of the form looks as follows:
// 
//   |---------------------------------------------------------------|
//   |  tcMainTop (tabs visible)                                     |
//   |---------------------------------------------------------------|
//   |   tcMainLeft             |    tcMainRight                     |
//   |   (tabs hidden)          |    (tabs hidden)                   |
//   |                          |                                    |
//   |---------------------------------------------------------------|
//   |   statusStrip1                                                |
//   |---------------------------------------------------------------|
//
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using ZedGraph;
using CefSharp;
using CefSharp.WinForms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace StormwaterCalculator
{
    // These declarations are required by the WebBrowser control
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]

    public partial class MainForm : Form
    {
        // Declaration for the SWMM engine DLL
        [DllImport("swmm5.dll")]
        public static extern int swmm_run(string f1, string f2, string f3);

        // Release Number
        public string releaseVersion = "Release 1.2.0.0";

        // Actions carried out by a background worker thread
        public enum Actions { GET_SOIL_DATA, GET_RAINFALL, RUN_SWMM, CALC_STATS };

        // Definition of the newLine string
        static string newLine = System.Environment.NewLine;

        //---------------------- Text messages used by the Calculator -----------------------

        static string[] statusTxt = {
            "Select the Location tab to begin analyzing a new site.",
            "Locate the site on the map.",
            "Select a soil type for the site.",
            "Enter the soil's drainage rate.",
            "Describe how steep the site is.",
            "Select a source of long-term hourly rainfall data.",
            "Select a source of monthly average evaporation rates.",
            "Select a climate change scenario to use.",
            "Describe the site's land cover.",
            "Assign LID practices to capture runoff from impervious areas.",
            ""
            };

        static string upToDateTxt = "Runoff results are up to date.";
        static string needsUpdatingTxt = "Site data have changed - results need to be refreshed.";

        string IntroTxt =
            "This calculator estimates the amount of stormwater runoff " +
            "generated from a land parcel under different development and " +
            "control scenarios over a long-term period of historical rainfall. " +
            newLine + newLine +
            "The analysis takes into account local soil conditions, topography, " +
            "land cover and meteorology. Different types of low impact " +
            "development (LID) practices can be employed to help capture " +
            "and retain rainfall on-site. Localized climate change " +
            "scenarios can also be analyzed." +
            newLine + newLine +
            "Site information is provided to the calculator using the tabbed " +
            "pages listed above. The Results page is where the site's runoff " +
            "is computed and displayed." +
            newLine + newLine +
            "This program was produced by the U.S. Environmental Protection " +
            "Agency and was subject to both internal and external technical " +
            "review. Please check with local authorities about whether and " +
            "how it can be used to support local stormwater management " +
            "goals and requirements.";

        public static bool ignoreConsecDays;  // true if consecutive rain days ignored
        public static bool inputHasChanged;   // true if input data has changed
        public static bool inputSaved;        // true if input data were saved
        public static bool resultsAvailable;  // true if runoff results available
        public static int dpi;                // dots per inch for form scaling

        static SoundPlayer waitingSound;      // sound effect for progress bar
        static int soundStatus;               // 1 if sound effect is on, 0 if not

        public int errorCode;                 // internal error code
        public bool retrieveSoilData;         // true if soil data to be retrieved
        bool soilDataRetrieved;               // true if soil data has been retrieved
        bool latLngChanged;                   // true if site location has changed
        bool mapBoundsSaved;                  // true if map bounds are saved/restored
        int ItemMargin = 3;                   // item margin for multi-line list boxes
        string[] siteSummary;                 // summary of site's input data

        LidForm lidForm;                      // Low Impact Development dialog form
        HelpForm helpForm;                    // Help form
        CostHelp CostHelpForm;                //additions for cost module dialog for cost module help

        //additions for cost module
        public int costResultsTabIndexNum = 7;  //index of cost module tab on results screen
        private CostRegionalization CostRegionalization = new CostRegionalization(); //cost regionalization helper object
        //public ChromiumWebBrowser browser;      // embedded chrome browser used for cost module
        public static dynamic costModuleResults; //holds cost results computed in JavaScript

        //These declarations are required by additional chromium based web browser control
        //added for the cost component
        private static readonly string _myPath = Application.StartupPath;           //used to locate embedded browser files to load
        private static readonly string _pagesPath = Path.Combine(_myPath, "www");   //used to locate embedded browser files to load
        public ChromiumWebBrowser _browser;     // embedded chrome browser used for cost module
        private BrowserLiason BrowserCommunicator; // helper object for managing interactions with embedded browser
        public delegate void Del(string message); // callback used for communicating with JavaScript

        //used for comunicating with cost module being executed in embedded browser
        //communication controlled by javascript. When browser is loaded, javascript sends
        //message to C# to indicate it is ready to accept cost variables, C# sends cost variables
        //JS computes and sends results back.
        public class BrowserLiason
        {
            public int operationCode = 0; //signals to JS to compute costs. Watched on $scope
            public string siteData = "";
            public string jsObj = "";
            public double lat = 0;
            public double lng = 0;
            public bool isBrowserLoaded = false;
            private int chartWidth = 350; //defaults overwritten later
            private int chartHeight = 250; //defaults overwritten later
            dynamic costVariables = new System.Dynamic.ExpandoObject();
            public CostRegionalization.BlsCenter selectedBLSCenter;
            private ChromiumWebBrowser _browser;

            public BrowserLiason(ChromiumWebBrowser browser, int width, int height)
            {
                this._browser = browser;
                chartWidth = width - 40;
                chartHeight = height - 40;
            }
            
            //helper function that allows embedded browser content to better utilize available space when the
            //application is resized
            public void onParentContainerResizeHandler(int width,int height)
            {
                var chartSize = new
                {
                    width = (width / 2) - 20,
                    height = (height / 2) - 20
                };
                string jsObj = JsonConvert.SerializeObject(chartSize);
                if (jsObj != "") executeInJsNoReturn(@"window.browserLiason.operationCode = 3; window.browserLiason.chartSize =" + jsObj + ";");
                var cmd1 = "var myScope = angular.element(document.querySelector('#mainPage')).scope(); myScope.resizeChart();";
                executeInJsNoReturn(cmd1);
            }

            //re-packages site data / user inputs in a format that JavaScript code expects
            public dynamic updateSiteData(int scenarioID, ListView lvs, double inputSiteArea = -1.0)
            {

                    int soilGroup = 0;
                    string hydSoilGroup = lvs.Items[1].SubItems[2 - scenarioID].Text;

                    switch (hydSoilGroup)
                    {
                        case "A":
                            soilGroup = 0;
                            break;
                        case "B":
                            soilGroup = 1;
                            break;
                        case "C":
                            soilGroup = 2;
                            break;
                        case "D":
                            soilGroup = 3;
                            break;
                        default:
                            soilGroup = 0;
                            break;
                    }
                    var complexity = new
                    {
                        siteimperv = lvs.Items[11].SubItems[2 - scenarioID].Text,
                        sitearea = lvs.Items[0].SubItems[2 - scenarioID].Text,
                        sitesoilksat = lvs.Items[2].SubItems[2 - scenarioID].Text,
                        isNewDevelopment = SiteData.isNewDevelopment,
                        isReDevelopment = !SiteData.isNewDevelopment,
                        siteSuitability = SiteData.siteSuitability,
                        topography = Array.FindIndex(SiteData.slope, row => row.Contains(lvs.Items[3].SubItems[2 - scenarioID].Text)),
                        soilType = soilGroup
                    };

                    double conv = Convert.ToDouble(43560.0 / (100.0 * 100.0)); //conversion factor to convert from % *% * acres to sq. feet
                  
                    string[] tempArr; //temporary array for splitting "lidArray % of area value / % of area used for lid control value"
                    double[] fpAreas = new Double[12]; //temporary array for holding lid control surface areas in sqft
                    double[] percentDrainArea = new Double[12]; //temporary array for holding % area draining to lid control 
                    double[] percentFPRatio = new Double[12]; //temporary array for holding % area draining to lid control 
                double dblSiteArea = 0.0;
                if (lvs.Items[0].SubItems[2 - scenarioID].Text != "N/A")
                {
                    dblSiteArea = Convert.ToDouble(lvs.Items[0].SubItems[2 - scenarioID].Text);
                }

                for (int i = 0; i < 7; i++)
                {
                    tempArr = lvs.Items[i + 12].SubItems[2 - scenarioID].Text.Split('/');
                    if (tempArr.Length > 1)
                    {
                        percentDrainArea[i] = Convert.ToDouble(tempArr[0]);
                        fpAreas[i] = conv * dblSiteArea * Convert.ToDouble(tempArr[0]) * Convert.ToDouble(tempArr[1]);
                    }
                    else
                    {
                        percentDrainArea[i] = 0;
                        fpAreas[i] = 0;
                    }
                }
                    //cisterns treated differently
                    if(fpAreas[1] > 0) //if area was assigned to rain harvesting recompute volume in gallons
                        fpAreas[1] = 0.001* 43560 * dblSiteArea * (percentDrainArea[1]/100)*Convert.ToDouble(LidData.rhSize * LidData.rhNumber); //rhNumber is # per 1000 so mult by 0.001

                var scenarioLids = new
                    {
                        id = new { id = "id", totSiteArea = dblSiteArea, percentDrainArea = percentDrainArea[0], name = "Disconnection", footprintAreaSqFt = Math.Round(fpAreas[0]), area = lvs.Items[12].SubItems[2 - scenarioID].Text, hasPretreatment = false },
                        rh = new { id = "rh", totSiteArea = dblSiteArea, percentDrainArea = percentDrainArea[1], name = "Rainwater Harvesting", footprintAreaSqFt = Math.Round(fpAreas[1]), area = lvs.Items[13].SubItems[2 - scenarioID].Text, hasPretreatment = false },
                        rg = new { id = "rg", totSiteArea = dblSiteArea, percentDrainArea = percentDrainArea[2], name = "Rain Gardens", footprintAreaSqFt = Math.Round(fpAreas[2]), area = lvs.Items[14].SubItems[2 - scenarioID].Text, hasPretreatment = LidData.rgHasPreTreat },
                        gr = new { id = "gr", totSiteArea = dblSiteArea, percentDrainArea = percentDrainArea[3], name = "Green Roofs", footprintAreaSqFt = Math.Round(fpAreas[3]), area = lvs.Items[15].SubItems[2 - scenarioID].Text, hasPretreatment = false },
                        sp = new { id = "sp", totSiteArea = dblSiteArea, percentDrainArea = percentDrainArea[4], name = "Street Planters", footprintAreaSqFt = Math.Round(fpAreas[4]), area = lvs.Items[16].SubItems[2 - scenarioID].Text, hasPretreatment = false },
                        ib = new { id = "ib", totSiteArea = dblSiteArea, percentDrainArea = percentDrainArea[5], name = "Infiltration Basins", footprintAreaSqFt = Math.Round(fpAreas[5]), area = lvs.Items[17].SubItems[2 - scenarioID].Text, hasPretreatment = LidData.ibHasPreTreat },
                        pp = new { id = "pp", totSiteArea = dblSiteArea, percentDrainArea = percentDrainArea[6], name = "Permeable Pavement", footprintAreaSqFt = Math.Round(fpAreas[6]), area = lvs.Items[18].SubItems[2 - scenarioID].Text, hasPretreatment = LidData.ppHasPreTreat }
                    };

                var tempObj = new
                {
                    complexity = complexity,
                    costvars = scenarioLids,
                    blsRegionStr = selectedBLSCenter.selectString,
                    regionalFactor = selectedBLSCenter.regionalFactor,
                    inflation = selectedBLSCenter.inflationFactor,
                    dataYear = selectedBLSCenter.dataYear,
                    chartSize = new
                    {
                        width = chartWidth / 2,
                        height = chartHeight / 2
                    }
                };

                jsObj = JsonConvert.SerializeObject(costVariables);
                Console.WriteLine(jsObj);
                if (scenarioID == 0)
                {
                    costVariables.baseScenario = tempObj;
                } else {
                    costVariables.currentScenario = tempObj;
                }
                return costVariables;
            }

            //called to stash baseline or current scenario site and lid 
            //variables for passing to the cost module later 
            //Scenario 0 - base, Scenario 1 - current
            public bool updateCostVariables(int scenarioID, ListView lvs, double inputSiteArea = -1.0)
            {
                /*if(checkSiteDataValid(inputSiteArea) == false)
                {
                    return false;
                }*/
                var tempObj = new
                {
                    complexity = "",
                    costvars = "",
                    lng = lng, //updated in Mainform.tcMainTop_SelectedIndexChanged if latLngChanged is true
                    lat = lat //updated in Mainform.tcMainTop_SelectedIndexChanged if latLngChanged is true
                };

                /*//if sitearea = 0 exit function
                if (inputSiteArea == 0)
                    scenarioID = -1;*/

                switch (scenarioID)
                {
                    case -1:
                        //costVariables.baseScenario = tempObj;
                        costVariables.currentScenario = tempObj;
                        break;
                    case 3:
                        costVariables.baseScenario = tempObj;
                        break;
                    default:
                        costVariables = updateSiteData(scenarioID, lvs, inputSiteArea);
                        break;
                }
                jsObj = JsonConvert.SerializeObject(costVariables);
                //Console.WriteLine(jsObj);

                //if browser is ready send updated siteData
                if (isBrowserLoaded)
                    sendToCSharp("", 2);
                return true;
            }

            //used to communicate from JS to C#
            public void sendToCSharp(string browserJson, int statusFlag)
            {
                switch (statusFlag)
                {
                    case 1://recieve computed costs from JS
                        dynamic tempObj = JsonConvert.DeserializeObject<dynamic>(browserJson);
                        costModuleResults = tempObj;                        
                        break;
                    case 2://JS wants site data to compute costs with
                        if (jsObj != "") executeInJsNoReturn(@"window.browserLiason.jsObj =" + jsObj + ";");
                        var cmd1 = "var myScope = angular.element(document.querySelector('#mainPage')).scope(); myScope.updateSiteData();";
                        executeInJsNoReturn(cmd1);
                        break;
                    default:
                        Console.WriteLine("An error occured in BrowserLiason.sendToCSharp");
                        break;
                }
                Console.WriteLine(browserJson);
            }

            //executes code in JS and does not wait for a return value
            public void executeInJsNoReturn(string scriptTxtToExec)
            {
                //check to see if browser ready before attempting to execute script
               if(_browser.Created) {
                    _browser.ExecuteScriptAsync(scriptTxtToExec);
                }
                
            }

            //executes code in JS and can receive a response after execution via a callback
            public void executeInJSWithReturn(string scriptTxtToExec, Del callback)
            {
                var task = _browser.EvaluateScriptAsync(scriptTxtToExec);
                task.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        //Recieve value from JS
                        //var response = t.Result;
                        if (t.Result.Success == true)
                        {
                            // Use JSON.net to convert to object;
                            if (t.Result.Result != null)
                            {
                                Console.WriteLine(t.Result.Result.ToString());
                                callback(t.Result.Result.ToString());
                            }                            
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }

        }
        private string GetPagePath(string pageName)
        {
            return Path.Combine(_pagesPath, pageName);
        }

        //initializes embedded chrome browser
        private void InitializeBrowserControl()
        {
            //for debugging var settings = new CefSettings { RemoteDebuggingPort = 8088 };
            var settings = new CefSettings { };
            //Perform dependency check to make sure all relevant resources are in our output directory.
            bool flag = Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: true);
            if (!flag)
            {
                Console.WriteLine("Failed to initialize CefSharp browser");
            }

            string pagePath = GetPagePath("costModule.html");
            _browser = new ChromiumWebBrowser(pagePath);
            BrowserCommunicator = new BrowserLiason(_browser, tpCost.Width, tpCost.Height);

            tpCost.Controls.Add(_browser);
            _browser.Dock = DockStyle.Fill;
            _browser.BringToFront();

            _browser.AddressChanged += BrowserAddressChanged;
            _browser.LoadError += BrowserLoadError;
            _browser.StatusMessage += BrowserStatusMessage;
            _browser.TitleChanged += BrowserTitleChanged;

            _browser.RegisterJsObject("browserLiason", BrowserCommunicator);
        }
        void BrowserTitleChanged(object sender, TitleChangedEventArgs e)
        {
            LogMessage("TITLE CHANGED: " + e.Title);
        }

        void BrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
            LogMessage("STATUS MESSAGE: " + e.Value);
        }

        void BrowserLoadError(object sender, LoadErrorEventArgs e)
        {
            string messageText = String.Format("{0} ({1} [{2}])", e.ErrorText, e.FailedUrl, e.ErrorCode);
            LogMessage("LOAD ERROR: " + messageText);
        }

        void BrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            string messageText = String.Format("{0}: {1} ({2})", e.Line, e.Message, e.Source);
            LogMessage("CONSOLE MESSAGE: " + messageText);
        }

        void BrowserAddressChanged(object sender, AddressChangedEventArgs e)
        {
            //let the browser communicator know that the browser has been loaded
            BrowserCommunicator.isBrowserLoaded = true;
            LogMessage("ADDRESS CHANGED: " + e.Address);
        }

        private void LogMessage(string messageText)
        {
            Console.WriteLine(messageText);
        }

        //-----------------------------------------------------------------------------
        //                   Form creation, initialization and closing 
        //-----------------------------------------------------------------------------
        public MainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            // Create temporary files
            try
            {
                SiteData.CreateFiles();
            }
            catch
            {
                MessageBox.Show("Cannot create temporary files. Application terminated.",
                    "Stormwater Calculator");
                Close();
            }

            // Set up the web browser component that hosts the Map Server
            webBrowser1.AllowWebBrowserDrop = false;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            webBrowser1.WebBrowserShortcutsEnabled = false;
            webBrowser1.ObjectForScripting = this;
            // Uncomment the following line when you are finished debugging.
            webBrowser1.ScriptErrorsSuppressed = true;

            // Load the Map Server's JavaScript HTML file into the web browser
            string path = SiteData.tmpPath + "\\map.htm";
            if (!File.Exists(path)) CreateMapServerFile(path);
            if (!File.Exists(path))
            {
                MessageBox.Show("Cannot find required file map.htm. Application terminated.",
                    "Stormwater Calculator");
                Close();
            }
            else
            {
                path = path.Replace("\\", "/");
                path = "file://" + path;
                webBrowser1.Navigate(path);
            }
            //InitBrowser();
            InitializeBrowserControl();

            // Load SCS 24-hour rainfall distributions
            if (!Xevent.ReadScsDistributions())
            {
                MessageBox.Show("Cannot load SCS distributions. Application terminated.",
                    "Stormwater Calculator");
                Close();
            }

            // Scale the form's size to the screen's dots per inch
            Graphics g = CreateGraphics();
            dpi = (int)g.DpiX;
            g.Dispose();
            Width = Width * dpi / 96;
            Height = Height * dpi / 96;
            
            // Center the form on the primary screen
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            Left = (workingRectangle.Width - Width) / 2;
            Top = (workingRectangle.Height - Height) / 2;
            
            // Create LID and Help forms
            lidForm = new LidForm();
            helpForm = new HelpForm();
            CostHelpForm = new CostHelp();
            helpForm.Owner = this;
            helpForm.Width = 270 * MainForm.dpi / 96;
            helpForm.Height = 300 * MainForm.dpi / 96;
            helpForm.Left = 0;
            helpForm.Top = 0;

            // Load LID data
            LidData.Init();

            // Create plots
            Graphs.CreateAnnualBudgetPlot(zgcAnnualBudget);
            Graphs.CreateBaseAnnualBudgetPlot(zgcBaseAnnualBudget);
            Graphs.CreateRainRunoffPlot(zgcRainRunoff);
            Graphs.CreateRunoffFreqPlot(zgcRunoffFreq);
            Graphs.CreateRunoffPcntPlot(zgcRunoffPcnt);
            Graphs.CreateRainfallCapturePlot(zgcRainfallCapture);
            Graphs.CreateExtremeEventPlot(zgcExtremeEvent);
            Graphs.CreateExtremePeakPlot(zgcExtremePeak);
            Graphs.CreateMonthlyAdjustPlot(zgcMonthlyAdjust);
            Graphs.CreateAnnualMaxAdjustPlot(zgcAnnualMaxAdjust);

            // Initialize state of form's controls
            tcMainTop.SelectedIndex = 0;
            lblIntro.Text = IntroTxt;
            InitControls();
            mapBoundsSaved = false;

            // Disable the sound button
            soundStatus = 0;
            btnSound.Image = imageList1.Images[soundStatus];
            btnSound.Visible = false;

            // Display the release number
            lblRelease.Text = releaseVersion;

            // Load the Jeopardy wav file
            try
            {
                waitingSound = new SoundPlayer(
                    StormwaterCalculator.Properties.Resources.jeapordy);  //"jeapordy.wav");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.StackTrace.ToString(),
                                       "Error");
            }

            // Add sub-items to the Site Summary & Results Tables listview control
            for (int i = 0; i < lvSiteSummary.Items.Count; i++)
            {
                lvSiteSummary.Items[i].SubItems.Add("");
                lvSiteSummary.Items[i].SubItems.Add("");
            }
            for (int i = 0; i < lvRunoffSummary.Items.Count; i++)
            {
                lvRunoffSummary.Items[i].SubItems.Add("");
                lvRunoffSummary.Items[i].SubItems.Add("");
            }
            siteSummary = new string[lvSiteSummary.Items.Count];
            statusLabel.Text = statusTxt[0];
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Control control = (Control)tpWebBrowser;
            helpForm.Location = control.PointToScreen(control.Location);
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
        }

        private void InitControls()
        {
            // Site location controls
            tbSiteName.Clear();
            tbSiteAddress.Clear();
            latLngChanged = true;
            nudSiteArea.Value = 0;

            // Soil data controls
            rbGroupB.Checked = true;
            tbKsat.Clear();
            cbSoilGroup.Checked = false;
            cbKsat.Checked = false;
            cbSlope.Checked = false;
            rbModFlatSlope.Checked = true;
            retrieveSoilData = false;
            soilDataRetrieved = false;

            // Met station selections
            SiteData.rainSourceIndex = 0;
            SiteData.evapSourceIndex = 0;

            // Climate change control
            rbClimate1.Checked = true;
            rbClimate2035.Checked = true;
            SiteData.climateYear = 2035;

            // Land cover controls
            nudForest.Value = 0;
            nudMeadow.Value = 0;
            nudLawn.Value = 40;
            nudDesert.Value = 0;
            tbImperv.Text = "60";

            // LID treatment controls
            nudImpDiscon.Value = 0;
            nudRainHarvest.Value = 0;
            nudInfilBasin.Value = 0;
            nudRainGarden.Value = 0;
            nudGreenRoof.Value = 0;
            nudStreetPlanter.Value = 0;
            nudPorousPave.Value = 0;
            nudDesignStorm.Value = 0;

            // Analysis controls
            nudYearsAnalyzed.Value = 20;
            nudEventThreshold.Value = 0.10M;
            cbIgnoreConsecDays.Checked = false;
            mnuRefreshResults.Enabled = true;
            mnuAddBaseline.Enabled = false;
            mnuRemoveBaseline.Enabled = false;
            mnuPrintResults.Enabled = false;

            // Graphs
            Graphs.UpdateAnnualBudgetPlot(zgcAnnualBudget, 0, 0, 0, 0);

            // Status variables
            errorCode = 0;
            tcResults.SelectedIndex = 1;
            rbSummaryResults.Checked = true;
            inputHasChanged = false;
            inputSaved = true;
            resultsAvailable = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prompt user to save current site
            e.Cancel = (SaveSiteData() == DialogResult.Cancel);

            //additions for cost module to shut down embedded chrominum browser
            Cef.Shutdown();
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GeoData.Close();
            SiteData.DeleteFiles();
        }

        //-----------------------------------------------------------------------------
        //                                Event Handlers
        //-----------------------------------------------------------------------------

        // KeyPress handler that suppresses the annoying beep when Enter is pressed
        private void mainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return) e.Handled = true;
        }

        // OnChange handler for main tab control visible to user
        private void tcMainTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear map of soil polygons and met station markers
            InvokeBrowserScript("hideSoilPolygons");
            InvokeBrowserScript("hideRainMarkers");
            InvokeBrowserScript("hideEvapMarkers");

            // Site location has changed
            if (latLngChanged)
            {
                double lat;
                double lng;
                GetLatLng(out lat, out lng);
                GeoData.GetMetStations(lat, lng);
                if (soilDataRetrieved) GetSoilData();
                GeoData.ShowSiteData(this);
                latLngChanged = false;

                //additions for cost module
                getBLSDataAndPopulateControls( lat,  lng);
            }
            if (mapBoundsSaved) RestoreMapBounds();

            // Change the page displayed in the left panel
            tcMainLeft.SelectedIndex = tcMainTop.SelectedIndex;
        }

        //additions for cost module obtains bls regional cost data for lat lng and populates controls
        private void getBLSDataAndPopulateControls(double lat, double lng, int SelectedIndex=-1)
        {
            //if coordinates have not changed do not re-aquire data for efficiency reasons
            if (SelectedIndex == -1 && BrowserCommunicator.lat == lat && BrowserCommunicator.lng == lng) return;
            
            statusLabel.Text = "Retrieving regional cost data...";

            BrowserCommunicator.lat = lat;
            BrowserCommunicator.lng = lng;
            //prep bls data
            List<CostRegionalization.BlsCenter> BlsCenterData = CostRegionalization.parseRegDataAndComputeDist(lat, lng);
            //Setup data binding
            this.cmbCostRegion.DataSource = BlsCenterData;
            this.cmbCostRegion.DisplayMember = "selectString";

            // leave out value member so can get whole object this.cmbCostRegion.ValueMember = "regionalFactor";
            if (SelectedIndex != -1)
            {
                cmbCostRegion.SelectedIndex = SelectedIndex;
                cmbCostRegion_SelectedIndexChanged(cmbCostRegion, new EventArgs());
            }else
            {
                //select the nearest center if it is with minimum distance of the center or use national as the default
                if(BlsCenterData[0].distToCurrentPoint > CostRegionalization.BlsCenter.maxValidityDistance)
                {
                    cmbCostRegion.SelectedIndex = CostRegionalization.BlsCenter.positionOfNationalDefaultInList; 
                }
                else
                {
                    cmbCostRegion.SelectedIndex = 0; 
                }
                cmbCostRegion_SelectedIndexChanged(cmbCostRegion, new EventArgs());
            }
        }

        // OnChange handler for the left panel's tab control
        private void tcMainLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            helpForm.Hide();
            tcMainLeft.Refresh();
            statusLabel.Text = statusTxt[tcMainLeft.SelectedIndex];

            // Check if Results page should be displayed
            if (tcMainLeft.SelectedTab == tpResultsLeft)
            {
                tcMainRight.SelectedTab = tpResultsRight;
                ChangeRefreshResultsState();
                DisplaySiteSummary();

                //additions for cost module
                //check to see if chrome browser loaded and load if not alread loaded
                if (BrowserCommunicator.isBrowserLoaded == false)
                {
                    //momentarily switch to costmodule to load embedded browser
                    int tempIndex = tcResults.SelectedIndex;
                    tcResults.SelectedIndex = costResultsTabIndexNum;
                    tcResults.SelectedIndex = tempIndex;
            }
                //scenarioid 0 is baseline, 1 is current scenario, 3 - remove baseline results
                //make sure form variables read in and updated
                GetSiteInfo();
                BrowserCommunicator.updateCostVariables(1, lvSiteSummary,(Double)nudSiteArea.Value);
                this.Cursor = Cursors.Default;
            }
            else if (tcMainLeft.SelectedTab == tpClimateLeft)
            {
                tcMainRight.SelectedTab = tpClimateRight;
            }
            else tcMainRight.SelectedTab = tpWebBrowser;

            // Control what gets displayed on the web browser's map
            string status = "OFF"; // indicates if site selection is on or off
            switch (tcMainLeft.SelectedIndex)
            {
                case 1:                                        // Site Location Page
                    status = "ON";
                    tbSiteName.Focus();
                    break;
                case 2:                                        // Soil Type Page
                    cbSoilGroup_CheckedChanged(sender, e);
                    cbSoilGroup.Focus();
                    break;
                case 3:                                        // Conductivity Page
                    cbKsat_CheckedChanged(sender, e);
                    tbKsat.Focus();
                    break;
                case 4:                                        // Slope Page
                    cbSlope_CheckedChanged(sender, e);
                    cbSlope.Focus();
                    break;
                case 5:                                        // Precip Page
                    InvokeBrowserScript("showRainMarkers");
                    lbRainSource.Focus();
                    break;
                case 6:
                    InvokeBrowserScript("showEvapMarkers");
                    lbEvapSource.Focus();
                    break;
                case 7:                                        // Climate Change Page
                    rbClimate1.Focus();
                    Climate.UpdateAdjustments();
                    Graphs.SetMonthlyAdjustPlotTitle(zgcMonthlyAdjust);
                    Graphs.SetAnnualMaxAdjustPlotTitle(zgcAnnualMaxAdjust);
                    Graphs.Refresh(zgcMonthlyAdjust);
                    Graphs.Refresh(zgcAnnualMaxAdjust);
                    break;
                case 8:                                        // Land Cover Page
                    nudForest.Focus();
                    break;
                case 9:                                        // LID Controls Page
                    nudImpDiscon.Focus();
                    break;
                case 10:                                       // Runoff Results Page
                    GeoData.GetStartEndYears(lbRainSource.SelectedIndex);
                    nudYearsAnalyzed.Maximum =
                        SiteData.endYear - SiteData.startYear + 1;
                    nudYearsAnalyzed.Focus();
                    break;
                default:
                    break;
            }

            // Tell the Map Server if click-on-site locator is on or off
            InvokeBrowserScript("setLocatorStatus", status);
        }

        // OnClickLink handler for the Help link label on all tab pages
        private void helpLabel_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            helpForm.ShowHelp(tcMainLeft.SelectedIndex);
        }

        // OnClickLink handler for the Help link label on the Results tab pages
        private void resultsHelpLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            helpForm.ShowHelp(100 + tcResults.SelectedIndex);
        }

        // KeyDown event handler for the Site Address text box
        private void tbSiteAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                InvokeBrowserScript("locateAddress", tbSiteAddress.Text);
            }
        }

        // OnClick handler for the Search button on the Site Location page
        private void btnSearch_Click(object sender, EventArgs e)
        {
            InvokeBrowserScript("locateAddress", tbSiteAddress.Text);
        }

        // OnChange event handler for the Site Location text box
        private void tbSiteLocation_TextChanged(object sender, EventArgs e)
        {
            cbSoilGroup.Checked = false;
            cbKsat.Checked = false;
            cbSlope.Checked = false;

            InvokeBrowserScript("deleteOverlays");
            soilDataRetrieved = false;
            retrieveSoilData = false;
            latLngChanged = true;
            RemoveBaselineResults();
            ClearResults();
            mnuAddBaseline.Enabled = false;
            mnuRemoveBaseline.Enabled = false;
            mnuPrintResults.Enabled = false;
            mapBoundsSaved = true;
        }

        // OnValueChanged event handler for the Site Area numeric box
        private void nudSiteArea_ValueChanged(object sender, EventArgs e)
        {
            double radius = Math.Sqrt((Double)nudSiteArea.Value * 0.00404686 / Math.PI);
            InvokeBrowserScript("setSiteRadius", radius);
        }

        // OnLinkClicked handler for the Open Previously Saved Site button
        private void btnOpen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ReadSiteData();            
        }

        // OnCheckChanged handler for the "View Soil Survey" check boxes
        private void cbSoilGroup_CheckedChanged(object sender, EventArgs e)
        {
            cbSoilGroup.Refresh();
            if (cbSoilGroup.Checked)
            {
                if (!soilDataRetrieved)
                {
                    retrieveSoilData = true;
                    GetSoilData();
                }
                InvokeBrowserScript("showSoilGroupPolygons");
            }
            else InvokeBrowserScript("hideSoilPolygons");
        }

        private void cbKsat_CheckedChanged(object sender, EventArgs e)
        {
            cbKsat.Refresh();
            if (cbKsat.Checked)
            {
                if (!soilDataRetrieved)
                {
                    retrieveSoilData = true;
                    GetSoilData();
                }
                InvokeBrowserScript("showKsatPolygons");
            }
            else InvokeBrowserScript("hideSoilPolygons");
        }

        private void cbSlope_CheckedChanged(object sender, EventArgs e)
        {
            cbSlope.Refresh();
            if (cbSlope.Checked)
            {
                if (!soilDataRetrieved)
                {
                    retrieveSoilData = true;
                    GetSoilData();
                }
                InvokeBrowserScript("showSlopePolygons");
            }
            else InvokeBrowserScript("hideSoilPolygons");
        }

        // OnKeyPress handler for the Conductivity textbox - allows only numeric input
        private void tbKsat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                 && !char.IsDigit(e.KeyChar)
                 && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        // OnSelectedIndexChanged handler for the rainfall data source listbox
        private void lbRainSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            SiteData.rainSourceIndex = lbRainSource.SelectedIndex;
            inputHasChanged = true;
        }

        // OnLinkClicked handler for the Save Rainfall Data link label 
        private void btnSaveRainfall_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SiteData.SaveRainfallData(lbRainSource.SelectedIndex);
        }

        // OnSelectedIndexChanged handler for the evaporation data source listbox
        private void lbEvapSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            SiteData.evapSourceIndex = lbRainSource.SelectedIndex;
            inputHasChanged = true;
        }

        // OnLinkClicked handler for the Save Evaporation Data link label 
        private void btnSaveEvap_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string txt = lbEvapSource.Text;
            txt = txt.Substring(4, txt.IndexOf("\n") - 4);
            SiteData.SaveEvapData(lbEvapSource.SelectedIndex, txt);
        }

        // CheckedChanged handler for the radio buttons that select a climate change year
        private void rbClimateYear_CheckedChanged(object sender, EventArgs e)
        {
            int year = 2060;
            if (rbClimate2035.Checked) year = 2035;
            if (year != SiteData.climateYear)
            {
                SiteData.climateYear = year;
                Climate.UpdateAdjustments();
                Graphs.SetMonthlyAdjustPlotTitle(zgcMonthlyAdjust);
                Graphs.SetAnnualMaxAdjustPlotTitle(zgcAnnualMaxAdjust);
                Graphs.Refresh(zgcMonthlyAdjust);
                Graphs.Refresh(zgcAnnualMaxAdjust);
                inputHasChanged = true;
            }
        }

        // ValueChanged handlers for the numeric up/down controls on the Land Cover
        // page -- automatically adjusts the percent impervious value
        private void nudLandCover_ValueChanged(object sender, EventArgs e)
        {
            decimal pctPerv, pctImperv;
            pctPerv = nudForest.Value + nudMeadow.Value + nudLawn.Value + nudDesert.Value;
            pctImperv = 100 - pctPerv;
            if (pctImperv < 0) pctImperv = 0;
            tbImperv.Text = pctImperv.ToString();
            inputHasChanged = true;
        }

        // OnEnter handler for all of the numeric up/down controls
        private void NumericUpDown_Enter(object sender, EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown)sender;
            nud.Select(0, 100);
        }

        // OnChanged handler for the Soil Group radio buttons
        private void rbGroup_Changed(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int soilGroupIndex = Int32.Parse((String)rb.Tag);
            lblDefaultKsat.Text = "(Default = " + SiteData.ksat[soilGroupIndex] + ")";
            inputHasChanged = true;
        }

        // Common LinkClick event handler for the Link Labels on the tpLidsLeft page
        private void LidLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel linkLabel = (LinkLabel)sender;
            lidForm.activePageIndex = Int32.Parse((String)linkLabel.Tag);
            lidForm.designStormDepth = nudDesignStorm.Value;
            lidForm.ShowDialog();
        }

        // OnChange handler for all analysis options 
        private void AnalysisOptionsChanged(object sender, EventArgs e)
        {
            inputHasChanged = true;
            ChangeRefreshResultsState();
        }

        // Common ValueChanged event handler for input entry controls
        private void InputControlChanged(object sender, EventArgs e)
        {
            inputHasChanged = true;
        }

        // OnLinkClicked event for the Refresh Results button 
        private void mnuRefreshResults_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Clear previous results and disable all controls
            ClearResults();
            this.Cursor = Cursors.WaitCursor; 
            EnablePanels(false);

            // Compute new runoff results
            PlaySound();
            ComputeResults();
            StopSound();

            // Enable controls and display results
            EnablePanels(true);
            this.Cursor = Cursors.Default;
            if (errorCode == 0)
            {
                //additions for cost module update costs by loading browser here and then continue displaying results
                if (BrowserCommunicator.isBrowserLoaded == false)
                {
                    int tempIndex = tcResults.SelectedIndex;
                    tcResults.SelectedIndex = costResultsTabIndexNum;
                    tcResults.SelectedIndex = tempIndex;
                }

                //additions for cost module scenarioid 0 is baseline, 1 is current scenario, 3 - remove baseline results
                GetSiteInfo();
                BrowserCommunicator.updateCostVariables(1, lvSiteSummary, (Double)nudSiteArea.Value);

                DisplayAnalysisOptions();
                DisplayResults();
                inputHasChanged = false;
                inputSaved = false;
                mnuRefreshResults.Enabled = false;
                ChangeRefreshResultsState();
                mnuAddBaseline.Enabled = !mnuRemoveBaseline.Enabled;
                mnuPrintResults.Enabled = true;
            }
        }

        // OnLinkClicked handler for the Use as Baseline Scenario link label
        private void mnuAddBaseline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DisplayBaselineResults();
            mnuAddBaseline.Enabled = false;
            mnuRemoveBaseline.Enabled = true;

            //additions for cost module update costs by loading browser here and then continue displaying results
            if (BrowserCommunicator.isBrowserLoaded == false)
            {
                int tempIndex = tcResults.SelectedIndex;
                tcResults.SelectedIndex = costResultsTabIndexNum;
                tcResults.SelectedIndex = tempIndex;
        }

            //scenarioid 0 is baseline, 1 is current scenario, 3 - remove baseline results
            GetSiteInfo();
            BrowserCommunicator.updateCostVariables(0, lvSiteSummary, (Double)nudSiteArea.Value);

        }

        // OnLinkClicked handler for the Remove Baseline Scenario link label
        private void mnuRemoveBaseline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RemoveBaselineResults();
            mnuRemoveBaseline.Enabled = false;
            mnuAddBaseline.Enabled = true;

            //additions for cost module
            //scenarioid 0 is baseline, 1 is current scenario, 3 - remove baseline results
            GetSiteInfo();
            BrowserCommunicator.updateCostVariables(3, lvSiteSummary);
        }

        // OnLinkClicked handler for the Print Results link label
        private void mnuPrintResults_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int tempIndex = tcResults.SelectedIndex;
            //additions for cost module check if costmodule has been loaded in browser and load it if not
            if (BrowserCommunicator.isBrowserLoaded == false)
            {
                //momentarily switch to costmodule to load in browser
                tcResults.SelectedIndex = costResultsTabIndexNum;
                tcResults.SelectedIndex = tempIndex;
            }
            if (Object.ReferenceEquals(null, costModuleResults))
            {
                //momentarily switch to costmodule to load in browser
                tcResults.SelectedIndex = costResultsTabIndexNum;
                tcResults.SelectedIndex = tempIndex;
            }
            WriteResultsToFile();
        }

        // OnClick handler for the various results selection radio buttons
        private void ResultsBtn_Clicked(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            //additions for cost module
            if (rb.TabIndex == costResultsTabIndexNum)
            {
                this.Cursor = Cursors.WaitCursor;
                //scenarioid 0 is baseline, 1 is current scenario, 3 - remove baseline results
                GetSiteInfo();
                BrowserCommunicator.updateCostVariables(1, lvSiteSummary, (Double)nudSiteArea.Value);
                this.Cursor = Cursors.Default;
            }
            helpForm.Hide();
            tcResults.SelectedIndex = rb.TabIndex;
            rb.Focus();
        }

        // OnClick handler for the Analyze a New Site label
        private void btnRestart_Click(object sender, EventArgs e)
        {
            if (SaveSiteData() == DialogResult.Cancel) return;
            InvokeBrowserScript("deleteOverlays");
            RemoveBaselineResults();
            ClearResults();
            InitControls();
            LidData.Init();
            tcMainTop.SelectedIndex = 1;
            //additions for cost module
            //reset current scenario
            BrowserCommunicator.updateCostVariables(1, lvSiteSummary, -1);
        }

        // OnClick handler for the Save Current Site label
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSiteDataToFile();
        }

        // OnClick handler for the Exit label
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Changes text and status of the Compute Runoff button
        private void ChangeRefreshResultsState()
        {
            // Enable button if input data have changed
            if (inputHasChanged) mnuRefreshResults.Enabled = true;

            // Revise text shown in status bar
            if (mnuRefreshResults.Enabled) statusLabel.Text = needsUpdatingTxt;
            else statusLabel.Text = upToDateTxt;
        }

        //---------------------------------------------------------------------
        //                    Computing Runoff with SWMM
        //---------------------------------------------------------------------

        private void ComputeResults()
        {
            errorCode = 0;

            // Transfer entries from form's controls to SiteData variables
            if (!GetSiteInfo())
            {
                errorCode = 1;
                return;
            }

            // Save user's choices for event threshold & ignoring consecutive events
            Runoff.runoffThreshold = (double)nudEventThreshold.Value;
            ignoreConsecDays = cbIgnoreConsecDays.Checked;

            // Determine the start and ending years for long term simulation
            // NOTE: Must do this before retrieving rainfall file
            int years = (int)nudYearsAnalyzed.Value;
            SiteData.SetStartEndYears(years);  

            // Retrieve rainfall data file
            RetrieveRainfallData();
            if (errorCode > 0)
            {
                MessageBox.Show("Could not retrieve rainfall data for this site.", "Run Time Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ////  Deprecated for release 1.1.0.1.  ////
            // Apply climate change adjustments
            //Climate.AdjustRainfallFile(SiteData.climateScenarioIndex);
            Climate.UpdateAdjustments();

            // Create a base SWMM input file for the site
            SiteData.WriteBaseInpFile();

            // Modify the base input file for a long term simulation
            SiteData.WriteLongTermInpFile();

            // Run SWMM to generate long term runoff time series
            RunSwmm();
            if (errorCode > 0)
            {
                MessageBox.Show("SWMM encountered an error: error code = " + errorCode,
                    "Run Time Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get annual water budget for long term run
            Runoff.GetWaterBudget(years, ref Runoff.annualRainfall, ref Runoff.annualRunoff,
                ref Runoff.annualInfil, ref Runoff.annualEvap);

            // Calculate statistics of runoff time series
            CalcStatistics();
            if (errorCode > 0)
            {
                MessageBox.Show("Could not compute runoff statistics.", "Run Time Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Run series of single extreme event simulations
            Xevent.RunExtremeEvents(this);
            if (errorCode > 0)
            {
                MessageBox.Show("Could not analyze extreme storm events.", "Run Time Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            resultsAvailable = true;
        }

        // Transfers site data from form's controls to the SiteData object
        private bool GetSiteInfo()
        {
            // Site soil group
            SiteData.soilGroup = GetSiteSoilGroup();

            // Ksat value
            SiteData.soilKsat = 0.0;
            if (tbKsat.TextLength > 0)
            {
                try
                {
                    SiteData.soilKsat = Double.Parse(tbKsat.Text);
                }
                catch
                {
                    MessageBox.Show("Illegal numeric value assigned to conductivity.",
                        "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Rain & evap data sources
            SiteData.rainSourceIndex = lbRainSource.SelectedIndex;
            SiteData.evapSourceIndex = lbEvapSource.SelectedIndex;
            SiteData.climateScenarioIndex = GetClimateIndex();

            // Land cover, LID controls, slope, and overland flow length
            if (!GetLandCover()) return false;
            if (!GetLidControls()) return false;
            SiteData.slopeIndex = GetSlopeIndex();
            SiteData.pathLength = 150;             // NOTE: path length hard-coded here
            return true;
        }

        private int GetSiteSoilGroup()
        {
            if (rbGroupA.Checked) return 0;
            if (rbGroupB.Checked) return 1;
            if (rbGroupC.Checked) return 2;
            return 3;
        }

        private int GetSlopeIndex()
        {
            if (rbFlatSlope.Checked) return 0;
            if (rbModFlatSlope.Checked) return 1;
            if (rbModSteepSlope.Checked) return 2;
            return 3;
        }

        private int GetClimateIndex()
        {
            if (rbClimate1.Checked) return 0;
            if (rbClimate2.Checked) return 1;
            if (rbClimate3.Checked) return 2;
            return 3;
        }

        private bool GetLandCover()
        {
            SiteData.landCover[0] = (double)nudForest.Value;
            SiteData.landCover[1] = (double)nudMeadow.Value;
            SiteData.landCover[2] = (double)nudLawn.Value;
            SiteData.landCover[3] = (double)nudDesert.Value;
            SiteData.landCover[4] = 0;
            SiteData.landCover[5] = Double.Parse(tbImperv.Text);
            SiteData.fracImperv = SiteData.landCover[5] / 100;

            double totalCover = 0.0;
            for (int i = 0; i < SiteData.landCover.Length; i++) totalCover += SiteData.landCover[i];
            if (totalCover > 100)
            {
                MessageBox.Show(
                    "Total land cover exceeds 100%",
                    "Land Cover Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool GetLidControls()
        {
            SiteData.fracImpDiscon = (double)nudImpDiscon.Value / 100.0;
            SiteData.fracRainHarvest = (double)nudRainHarvest.Value / 100.0;
            SiteData.fracRainGarden = (double)nudRainGarden.Value / 100.0;
            SiteData.fracGreenRoof = (double)nudGreenRoof.Value / 100.0;
            SiteData.fracStreetPlanter = (double)nudStreetPlanter.Value / 100.0;
            SiteData.fracInfilBasin = (double)nudInfilBasin.Value / 100.0;
            SiteData.fracPorousPave = (double)nudPorousPave.Value / 100.0;

            //additions for cost module
            SiteData.totalLIDAreaFrac = SiteData.fracImpDiscon + SiteData.fracRainHarvest + SiteData.fracRainGarden + SiteData.fracGreenRoof + SiteData.fracStreetPlanter + SiteData.fracInfilBasin + SiteData.fracPorousPave;
            SiteData.isNewDevelopment =rbCostNewDev.Checked;
            SiteData.isReDevelopment= rbCostRedev.Checked;

            //Cost complexity siteSuitabiltiy
            SiteData.siteSuitability = 0;
            if(rbCostSitePoor.Checked) SiteData.siteSuitability = 0;
            if (rbCostSiteModerate.Checked) SiteData.siteSuitability = 1;
            if (rbCostSiteExcellent.Checked) SiteData.siteSuitability = 2;

            // Check for valid LID inputs
            return LidData.Validate();
        }

        private void RetrieveRainfallData()
        {
            statusLabel.Text = "Retrieving rainfall data...";
            progressBar.Visible = true;
            this.backgroundWorker1.RunWorkerAsync(Actions.GET_RAINFALL);
            while (this.backgroundWorker1.IsBusy)
            {
                Application.DoEvents();
            }
        }

        public void RunSwmm()
        {
            statusLabel.Text = "Running SWMM...";
            progressBar.Visible = true;
            this.backgroundWorker1.RunWorkerAsync(Actions.RUN_SWMM);
            while (this.backgroundWorker1.IsBusy)
            {
                Application.DoEvents();
            }
        }

        private void CalcStatistics()
        {
            statusLabel.Text = "Computing statistics...";
            progressBar.Visible = true;
            this.backgroundWorker1.RunWorkerAsync(Actions.CALC_STATS);
            while (this.backgroundWorker1.IsBusy)
            {
                Application.DoEvents();
            }
        }

        //----------------------------------------------------------------------------
        //                             Displaying Results
        //----------------------------------------------------------------------------

        private void DisplaySiteSummary()
        {
            // Site area
            if (nudSiteArea.Value == 0) lvSiteSummary.Items[0].SubItems[1].Text = "N/A";
            else lvSiteSummary.Items[0].SubItems[1].Text = nudSiteArea.Value.ToString();
            
            // Hydrologic Soil Group
            string[] hsg = { "A", "B", "C", "D" };
            int hsgIndex = GetSiteSoilGroup();
            lvSiteSummary.Items[1].SubItems[1].Text = hsg[hsgIndex];

            // Hydraulic conductivity
            if (tbKsat.TextLength > 0)
                lvSiteSummary.Items[2].SubItems[1].Text = tbKsat.Text;
            else
                lvSiteSummary.Items[2].SubItems[1].Text = SiteData.ksat[hsgIndex];

            // Slope
            lvSiteSummary.Items[3].SubItems[1].Text = SiteData.slope[GetSlopeIndex()];

            // Rainfall & evap. data sources
            string txt = lbRainSource.Text;
            int length = txt.IndexOf("\n") - 4; 
            lvSiteSummary.Items[4].SubItems[1].Text = txt.Substring(4, length);
            txt = lbEvapSource.Text;
            length = txt.IndexOf("\n") - 4;
            lvSiteSummary.Items[5].SubItems[1].Text = txt.Substring(4, length);
            txt = Climate.scenarioNames[GetClimateIndex()];
            if (GetClimateIndex() > 0)
            {
                if (SiteData.climateYear == 2035) txt = txt + "/Near Term";
                else txt = txt + "/Far Term";
                //txt = txt + "/" + SiteData.climateYear.ToString();
            }
            lvSiteSummary.Items[6].SubItems[1].Text = txt;                 

            // Land cover
            lvSiteSummary.Items[7].SubItems[1].Text = nudForest.Value.ToString();
            lvSiteSummary.Items[8].SubItems[1].Text = nudMeadow.Value.ToString();
            lvSiteSummary.Items[9].SubItems[1].Text = nudLawn.Value.ToString();
            lvSiteSummary.Items[10].SubItems[1].Text = nudDesert.Value.ToString();
            lvSiteSummary.Items[11].SubItems[1].Text = tbImperv.Text;

            // LIDs which use following prefixes:
            //   id = impervious disconnection
            //   rh = rainwater harvesting
            //   rg = rain garden
            //   gr = green roof
            //   sp = street planter
            //   ib = infiltration basin
            //   pp = porous pavement
            txt = nudImpDiscon.Value.ToString();
            if (txt != "0") txt += " / " + LidData.idCapture.ToString("F0");
            lvSiteSummary.Items[12].SubItems[1].Text = txt;
            txt = nudRainHarvest.Value.ToString();
            if (txt != "0") txt += " / " + LidData.rhNumber.ToString("F0");
            lvSiteSummary.Items[13].SubItems[1].Text = txt;
            txt = nudRainGarden.Value.ToString();
            if (txt != "0") txt += " / " + LidData.rgCapture.ToString("F0");
            lvSiteSummary.Items[14].SubItems[1].Text = txt;
            txt = nudGreenRoof.Value.ToString();
            if (txt != "0") txt += " / 100";
            lvSiteSummary.Items[15].SubItems[1].Text = txt;
            txt = nudStreetPlanter.Value.ToString();
            if (txt != "0") txt += " / " + LidData.spCapture.ToString("F0");
            lvSiteSummary.Items[16].SubItems[1].Text = txt;
            txt = nudInfilBasin.Value.ToString();
            if (txt != "0") txt += " / " + LidData.ibCapture.ToString("F0");
            lvSiteSummary.Items[17].SubItems[1].Text = txt;
            txt = nudPorousPave.Value.ToString();
            if (txt != "0") txt += " / " + LidData.ppCapture.ToString("F0");
            lvSiteSummary.Items[18].SubItems[1].Text = txt;
        }

        private void ClearAnalysisOptions()
        {
            lvSiteSummary.Items[19].SubItems[1].Text = "";
            lvSiteSummary.Items[20].SubItems[1].Text = "";
            lvSiteSummary.Items[21].SubItems[1].Text = "";
        }

        private void DisplayAnalysisOptions()
        {
            lvSiteSummary.Items[19].SubItems[1].Text = nudYearsAnalyzed.Value.ToString();
            lvSiteSummary.Items[20].SubItems[1].Text = cbIgnoreConsecDays.Checked.ToString();
            lvSiteSummary.Items[21].SubItems[1].Text = Runoff.runoffThreshold.ToString("F2");

            // Save contents of Site Summary for use in Baseline Summary
            for (int i = 0; i < lvSiteSummary.Items.Count; i++)
            {
                siteSummary[i] = lvSiteSummary.Items[i].SubItems[1].Text;
            }

        }

        private void DisplayBaselineResults()
        {
            // Site characteristics
            for (int i = 0; i < lvSiteSummary.Items.Count; i++)
                lvSiteSummary.Items[i].SubItems[2].Text = siteSummary[i];

            // Basic rainfall & runoff statistics
            for (int i = 0; i < (int)Runoff.Statistics.COUNT; i++)
                lvRunoffSummary.Items[i].SubItems[2].Text = lvRunoffSummary.Items[i].SubItems[1].Text;

            // Water Balance plot
            Graphs.UpdateBaseAnnualBudgetPlot(zgcBaseAnnualBudget);
            zgcBaseAnnualBudget.Visible = true;
            Graphs.Refresh(zgcBaseAnnualBudget);

            // Rainfall - Runoff plot
            Runoff.baseRainRunoffList.Clear();
            Runoff.baseRainRunoffList.AddRange(Runoff.rainRunoffList);
            zgcRainRunoff.GraphPane.Legend.IsVisible = true;
            Graphs.Refresh(zgcRainRunoff);

            // Runoff frequency plot
            Runoff.baseRainFreqList.Clear();
            Runoff.baseRainFreqList.AddRange(Runoff.rainFreqList);
            Runoff.baseRunoffFreqList.Clear();
            Runoff.baseRunoffFreqList.AddRange(Runoff.runoffFreqList);
            Graphs.UpdateRunoffFreqPlot(zgcRunoffFreq, true);
            Graphs.Refresh(zgcRunoffFreq);

            // Runoff percentage by storm size plot
            Runoff.baseRunoffPcntList.Clear();
            Runoff.baseRunoffPcntList.AddRange(Runoff.runoffPcntList);
            zgcRunoffPcnt.GraphPane.Legend.IsVisible = true;
            Graphs.Refresh(zgcRunoffPcnt);

            // Extreme event plot
            Xevent.SetBasePlottingData();
            Graphs.UpdateExtremeEventPlot(zgcExtremeEvent, true);
            Graphs.Refresh(zgcExtremeEvent);
            Graphs.UpdateExtremeEventPlot(zgcExtremePeak, true);
            Graphs.Refresh(zgcExtremePeak);

            Runoff.baseRetentionPcntList.Clear();
            Runoff.baseRetentionPcntList.AddRange(Runoff.retentionPcntList);
            zgcRainfallCapture.GraphPane.Legend.IsVisible = true;
            Graphs.Refresh(zgcRainfallCapture);
        }

        private void RemoveBaselineResults()
        {
            // Remove site characteristics display items
            for (int i = 0; i < lvSiteSummary.Items.Count; i++)
            {
                lvSiteSummary.Items[i].SubItems[2].Text = "";
            }

            // Remove summary statistics display items
            for (int i = 0; i < (int)Runoff.Statistics.COUNT; i++)
                lvRunoffSummary.Items[i].SubItems[2].Text = "";

            // Erase water budget plot
            zgcBaseAnnualBudget.Visible = false;

            // Erase rainfall - runoff plot
            Runoff.baseRainRunoffList.Clear();
            zgcRainRunoff.GraphPane.Legend.IsVisible = false;
            Graphs.Refresh(zgcRainRunoff);

            // Erase runoff frequency plot
            Runoff.baseRainFreqList.Clear();
            Runoff.baseRunoffFreqList.Clear();
            Graphs.UpdateRunoffFreqPlot(zgcRunoffFreq, false);
            Graphs.Refresh(zgcRunoffFreq);

            // Erase runoff percentage plot
            Runoff.baseRunoffPcntList.Clear();
            zgcRunoffPcnt.GraphPane.Legend.IsVisible = false;
            Graphs.Refresh(zgcRunoffPcnt);

            // Erase extreme event plot
            Xevent.baseExtremeRainfallList.Clear();
            Xevent.baseExtremeRunoffList.Clear();
            Xevent.basePeakRainfallList.Clear();
            Xevent.basePeakRunoffList.Clear();
            Xevent.SetPlottingData();
            Graphs.UpdateExtremeEventPlot(zgcExtremeEvent, false);
            Graphs.Refresh(zgcExtremeEvent);
            Graphs.UpdateExtremeEventPlot(zgcExtremePeak, false);
            Graphs.Refresh(zgcExtremePeak);

            // Erase retention frequency plot
            Runoff.baseRetentionPcntList.Clear();
            zgcRainfallCapture.GraphPane.Legend.IsVisible = false;
            Graphs.Refresh(zgcRainfallCapture);
        }

        private void ClearResults()
        {
            for (int i = 0; i < lvRunoffSummary.Items.Count; i++)
            {
                lvRunoffSummary.Items[i].SubItems[1].Text = "";
            }
            ClearAnalysisOptions();
            Runoff.rainRunoffList.Clear();
            Runoff.rainFreqList.Clear();
            Runoff.runoffFreqList.Clear();
            Runoff.runoffPcntList.Clear();
            Runoff.retentionPcntList.Clear();
            Xevent.extremeRainfallList.Clear();
            Xevent.extremeRunoffList.Clear();
            Xevent.peakRainfallList.Clear();
            Xevent.peakRunoffList.Clear();
            zgcRunoffPcnt.GraphPane.X2Axis.IsVisible = false;
            inputHasChanged = false;
            inputSaved = true;
            resultsAvailable = false;
        }

        private void DisplayResults()
        {
            // Populate the Summary listview with the runoff statistics
            for (int i = 0; i < (int)Runoff.Statistics.COUNT; i++)
            {
                lvRunoffSummary.Items[i].SubItems[1].Text =
                    Runoff.runoffStats[i].ToString("F2");
            }

            // Update the water budget chart
            Graphs.UpdateAnnualBudgetPlot(zgcAnnualBudget, Runoff.annualRainfall,
                Runoff.annualRunoff, Runoff.annualInfil, Runoff.annualEvap);
            Graphs.Refresh(zgcAnnualBudget);

            // Refresh rainfall - runoff plot
            Graphs.Refresh(zgcRainRunoff);

            // Update the runoff frequency plot
            Graphs.Refresh(zgcRunoffFreq);

            // Update the runoff by storm size plot
            zgcRunoffPcnt.GraphPane.X2Axis.Scale.TextLabels = Runoff.DepthLabels;
            zgcRunoffPcnt.GraphPane.X2Axis.IsVisible = true;
            Graphs.Refresh(zgcRunoffPcnt);

            // Update the extreme event runoff plot
            Graphs.Refresh(zgcExtremeEvent);
            Graphs.Refresh(zgcExtremePeak);

            // Update retention frequency plot
            Graphs.Refresh(zgcRainfallCapture);
        }

        //-------------------------------------------------------------------------
        //                  Saving and Reading Site Data From a File
        //-------------------------------------------------------------------------

        // Saves site data to a file
        private DialogResult SaveSiteData()
        {
            // Do nothing if data hasn't changed
            if (!inputHasChanged && inputSaved) return DialogResult.OK;

            // Ask user if they want to save the data
            var result = MessageBox.Show("Do you wish to save the data for this site?",
                "Save Data", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, 0);
            if (result == DialogResult.Cancel) return DialogResult.Cancel;

            // Display the Save dialog
            if (result == DialogResult.Yes) return SaveSiteDataToFile();
            return DialogResult.OK;
        }

        private DialogResult SaveSiteDataToFile()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Stormwater Calculator files (*.swc)|*.swc|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.FileName = "*.swc";

            // Save data to file
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                WriteSiteData(saveFileDialog1.FileName);
                inputSaved = true;
                return DialogResult.OK;
            }
            return DialogResult.Cancel;
        }

        private void WriteSiteData(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("Stormwater Calculator Site Data");
                sw.WriteLine("Version              1.1");
                sw.WriteLine("SiteName             " + tbSiteName.Text);

                sw.WriteLine("SiteLocation         " + tbSiteLocation.Text);   //0       
                sw.WriteLine("SiteArea             " + nudSiteArea.Value);     //1

                int index;
                index = 0;
                if (rbGroupB.Checked) index = 1;
                if (rbGroupC.Checked) index = 2;
                if (rbGroupD.Checked) index = 3;
                sw.WriteLine("HydSoilGroup         " +                         //2
                    SiteData.soilGroupTxt[index]);

                String txt;
                if (tbKsat.TextLength > 0) txt = tbKsat.Text;
                else txt = SiteData.ksat[index];
                sw.WriteLine("HydConductivity      " + txt);                   //3

                if (rbFlatSlope.Checked) index = 0;
                else if (rbModFlatSlope.Checked) index = 1;
                else if (rbModSteepSlope.Checked) index = 2;
                else index = 3;
                sw.WriteLine("SurfaceSlope         " + SiteData.slope[index]); //4

                sw.WriteLine("RainSource           " + lbRainSource.SelectedIndex); //5
                sw.WriteLine("EvapSource           " + lbEvapSource.SelectedIndex); //6

                sw.WriteLine("%Forest              " + nudForest.Value);            //7
                sw.WriteLine("%Meadow              " + nudMeadow.Value);            //8
                sw.WriteLine("%Lawn                " + nudLawn.Value);              //9
                sw.WriteLine("%Desert              " + nudDesert.Value);            //10
                sw.WriteLine("%Impervious          " + tbImperv.Text);              //11

                sw.WriteLine("%Disconnection       " + nudImpDiscon.Value);         //12
                sw.WriteLine("  CaptureRatio       " + LidData.idCapture);          //13
                sw.WriteLine("%Harvesting          " + nudRainHarvest.Value);       //14
                sw.WriteLine("  CisternSize        " + LidData.rhSize);             //15
                sw.WriteLine("  CisternNumber      " + LidData.rhNumber);           //16
                sw.WriteLine("  EmptyingRate       " + LidData.rhDrainRate);        //17
                sw.WriteLine("%RainGardens         " + nudRainGarden.Value);        //18
                sw.WriteLine("  PondingHeight      " + LidData.rgRimHeight);        //19
                sw.WriteLine("  SoilThickness      " + LidData.rgSoilHeight);       //20
                sw.WriteLine("  SoilKsat           " + LidData.rgSoilKsat);         //21
                sw.WriteLine("  CaptureRatio       " + LidData.rgCapture);          //22
                sw.WriteLine("%GreenRoofs          " + nudGreenRoof.Value);         //23
                sw.WriteLine("  SoilThickness      " + LidData.grSoilHeight);       //24
                sw.WriteLine("  SoilKsat           " + LidData.grSoilKsat);         //25
                sw.WriteLine("%StreetPlanters      " + nudStreetPlanter.Value);     //26
                sw.WriteLine("  PondingHeight      " + LidData.spRimHeight);        //27
                sw.WriteLine("  SoilThickness      " + LidData.spSoilHeight);       //28
                sw.WriteLine("  SoilKsat           " + LidData.spSoilKsat);         //29
                sw.WriteLine("  GravelThickness    " + LidData.spDrainHeight);      //30
                sw.WriteLine("  CaptureRatio       " + LidData.spCapture);          //31
                sw.WriteLine("%InfilBasin          " + nudInfilBasin.Value);        //32
                sw.WriteLine("  BasinDepth         " + LidData.ibHeight);           //33
                sw.WriteLine("  CaptureRatio       " + LidData.ibCapture);          //34
                sw.WriteLine("%PorousPavement      " + nudPorousPave.Value);        //35
                sw.WriteLine("  PavementThickness  " + LidData.ppPaveHeight);       //36
                sw.WriteLine("  GravelThickness    " + LidData.ppDrainHeight);      //37
                sw.WriteLine("  CaptureRatio       " + LidData.ppCapture);          //38

                sw.WriteLine("DesignStorm          " + nudDesignStorm.Value);       //39
                sw.WriteLine("YearsAnalyzed        " + nudYearsAnalyzed.Value);     //40
                sw.WriteLine("RunoffThreshold      " + nudEventThreshold.Value);    //41
                sw.WriteLine("IgnoreConsecStorms   " + cbIgnoreConsecDays.Checked); //42

                sw.WriteLine("ClimateScenario      " + GetClimateIndex());          //43
                sw.WriteLine("ClimateYear          " + SiteData.climateYear);       //44

                //additions for cost module
                sw.WriteLine("%BEGIN VARS ADDED FOR COST          ");               //45
                sw.WriteLine("isNewDevelopment   " + rbCostNewDev.Checked);         //46
                sw.WriteLine("isReDevelopment   " + rbCostRedev.Checked);           //47
                sw.WriteLine("siteSuitabilityPoor   " + rbCostSitePoor.Checked);    //48
                sw.WriteLine("siteSuitabilityModerate   " + rbCostSiteModerate.Checked); //49
                sw.WriteLine("siteSuitabilityExcellent   " + rbCostSiteExcellent.Checked); //50
                sw.WriteLine("rgPretreatment   " + LidData.rgHasPreTreat);          //51
                sw.WriteLine("ibPretreatment   " + LidData.ibHasPreTreat);          //52
                sw.WriteLine("ppPretreatment   " + LidData.ppHasPreTreat);          //53
                sw.WriteLine("cmbCostRegion   " + cmbCostRegion.SelectedIndex);     //54
                sw.WriteLine("tbRegMultiplier   " + tbRegMultiplier.Text);          //55
                sw.WriteLine("%END VARS ADDED FOR COST          ");                 //56
            }
        }

        // Reads site data from a previously saved file
        private void ReadSiteData()
        {
            // Display an Open File dialog to get name of input file
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Stormwater Calculator files (*.swc)|*.swc|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.CheckFileExists = true;

            // If user supplies a file name
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(openFileDialog1.FileName);

                    // Cheak if we have a SWC file
                    if (lines[0] != "Stormwater Calculator Site Data")
                        throw new System.FormatException("File is not a Stormwater Calculator file.");

                    // Skip version and read site name
                    string siteName = lines[2].Remove(0, 8).Trim();
                    tbSiteName.Text = siteName;

                    // Extract value for each site data attribute
                    string[] values = new string[lines.Length - 3];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = GetInputValue(lines[i + 3]);
                        if (values[i].Length == 0)
                        {
                            int lineNumber = i + 4;
                            throw new System.FormatException("No data at line "
                                + lineNumber + " of file.");
                        }
                    }

                    // Populate the main form with the site data
                    RemoveBaselineResults();
                    ClearResults();
                    InitControls();
                    LidData.Init();
                    PopulateInputData(values);

                    // Zoom the map display to the site location
                    double lat, lng;
                    GetLatLng(out lat, out lng);
                    InvokeBrowserScript("zoomToLocation", lat, lng);
                    double radius = Math.Sqrt((Double)nudSiteArea.Value * 0.00404686 / Math.PI);
                    InvokeBrowserScript("setSiteRadius", radius);
                    inputHasChanged = false;
                    inputSaved = true;

                    //additions for cost module now obtain bls data and compute cost regionalization variables
                    int selectedCostCmbIndex = 0;
                    if(values.Length > 54) //older saved files for previous version will not have saved this
                    {
                        selectedCostCmbIndex = Convert.ToInt16(values[54]);
                }
                    getBLSDataAndPopulateControls(lat, lng, selectedCostCmbIndex);
                }
                catch (System.FormatException ex)
                {
                    MessageBox.Show(ex.Message, "File Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Parses the Value portion from a line of Attribute/Value text
        private string GetInputValue(string line)
        {
            string[] words = line.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 2) return "";
            else return words[1];
        }

        // Populates the main form with site data read from a file
        private void PopulateInputData(string[] values)
        {
            // Site Location
            tbSiteLocation.Text = values[0];

            // Site Area
            nudSiteArea.Value = Decimal.Parse(values[1]);

            // Hydrologic Soil Group
            if      (values[2] == "D") rbGroupD.Checked = true;
            else if (values[2] == "C") rbGroupC.Checked = true;
            else if (values[2] == "B") rbGroupB.Checked = true;
            else if (values[2] == "A") rbGroupA.Checked = true;
            
            // Hydraulic Conductivity
            tbKsat.Text = values[3];
            
            // Surface Slope
            if (values[4] == SiteData.slope[0]) rbFlatSlope.Checked = true;
            else if (values[4] == SiteData.slope[1]) rbModFlatSlope.Checked = true;
            else if (values[4] == SiteData.slope[2]) rbModSteepSlope.Checked = true;
            else if (values[4] == SiteData.slope[3]) rbSteepSlope.Checked = true;

            // Choice of Rainfall & Evap. Sources
            int index = Int32.Parse(values[5]);
            if (index < 0 || index >= GeoData.maxMetStations) index = 0;
            SiteData.rainSourceIndex = index;
            index = Int32.Parse(values[6]);
            if (index < 0 || index >= GeoData.maxMetStations) index = 0;
            SiteData.evapSourceIndex = index;
            
            // Land Covers
            nudForest.Value = Decimal.Parse(values[7]);
            nudMeadow.Value = Decimal.Parse(values[8]);
            nudLawn.Value = Decimal.Parse(values[9]);
            nudDesert.Value = Decimal.Parse(values[10]);
            tbImperv.Text = values[11];

            // Impervious Disconnection LID
            nudImpDiscon.Value = Decimal.Parse(values[12]);
            LidData.idCapture = Decimal.Parse(values[13]);

            // Rainwater Harvesting LID
            nudRainHarvest.Value = Decimal.Parse(values[14]);
            LidData.rhSize = Decimal.Parse(values[15]);
            LidData.rhNumber = Decimal.Parse(values[16]);
            LidData.rhDrainRate = Decimal.Parse(values[17]);

            // Rain Garden LID
            nudRainGarden.Value = Decimal.Parse(values[18]);
            LidData.rgRimHeight = Decimal.Parse(values[19]);
            LidData.rgSoilHeight = Decimal.Parse(values[20]);
            LidData.rgSoilKsat = Decimal.Parse(values[21]);
            LidData.rgCapture = Decimal.Parse(values[22]);

            // Green Roof LID
            nudGreenRoof.Value = Decimal.Parse(values[23]);
            LidData.grSoilHeight = Decimal.Parse(values[24]);
            LidData.grSoilKsat = Decimal.Parse(values[25]);

            // Street Planter LID
            nudStreetPlanter.Value = Decimal.Parse(values[26]);
            LidData.spRimHeight = Decimal.Parse(values[27]);
            LidData.spSoilHeight = Decimal.Parse(values[28]);
            LidData.spSoilKsat = Decimal.Parse(values[29]);
            LidData.spDrainHeight = Decimal.Parse(values[30]);
            LidData.spCapture = Decimal.Parse(values[31]);

            // Infiltration Basin LID
            nudInfilBasin.Value = Decimal.Parse(values[32]);
            LidData.ibHeight = Decimal.Parse(values[33]);
            LidData.ibCapture = Decimal.Parse(values[34]);

            // Porous Pavement LID
            nudPorousPave.Value = Decimal.Parse(values[35]);
            LidData.ppPaveHeight = Decimal.Parse(values[36]);
            LidData.ppDrainHeight = Decimal.Parse(values[37]);
            LidData.ppCapture = Decimal.Parse(values[38]);

            // Analysis Parameters
            nudDesignStorm.Value = Decimal.Parse(values[39]);
            nudYearsAnalyzed.Value = Decimal.Parse(values[40]);
            nudEventThreshold.Value = Decimal.Parse(values[41]);
            cbIgnoreConsecDays.Checked = (values[42] == "True");

            // Climate change scenario
            //additions for cost module cost module change if (values.Length == 45 )
            if (values.Length >= 45)
            {
                switch (Int32.Parse(values[43]))
                {
                    case 0: rbClimate1.Checked = true; break;
                    case 1: rbClimate2.Checked = true; break;
                    case 2: rbClimate3.Checked = true; break;
                    case 3: rbClimate4.Checked = true; break;
                }
                if (Int32.Parse(values[44]) == 2035) rbClimate2035.Checked = true;
                if (Int32.Parse(values[44]) == 2060) rbClimate2060.Checked = true;
            }

            //additions for cost module
            if (values.Length > 45)
            {
                //Cost complexity isNewDevelopment
                rbCostNewDev.Checked = (values[46] == "True");
                rbCostRedev.Checked = (values[47] == "True");

                //Cost complexity siteSuitabiltiy
                rbCostSitePoor.Checked = (values[48] == "True");
                rbCostSiteModerate.Checked = (values[49] == "True");
                rbCostSiteExcellent.Checked = (values[50] == "True");

                //Rain garden pre-treatment
                LidData.rgHasPreTreat = (values[51] == "True");

                //Infiltration basin pre-treatment
                LidData.ibHasPreTreat = (values[52] == "True");

                //Permeable pavement pre-treatment
                LidData.ppHasPreTreat = (values[53] == "True");

                //cost regionalization variables
                // done outside this function after lat long obtained
        }
        }

        private void WriteResultsToFile()
        {
            // Get name of PDF file to write results to
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.FileName = "*.pdf";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Write site description
                string pdfFileName = saveFileDialog1.FileName;
                ReportWriter rw = new ReportWriter(releaseVersion);
                rw.WriteSiteSummaryPage(tbSiteName.Text, lvSiteSummary);

                // Write summary results
                double aspectRatio = (double)zgcAnnualBudget.Height /
                    (double)zgcAnnualBudget.Width;
                Image myImage1 = new Bitmap(zgcAnnualBudget.GetImage());
                Image myImage2 = null;
                if (zgcBaseAnnualBudget.Visible)
                    myImage2 = new Bitmap(zgcBaseAnnualBudget.GetImage());
                rw.WriteSummaryResultsPage(tbSiteName.Text, lvRunoffSummary,
                    myImage1, myImage2, aspectRatio);

                // Write rainfall - runoff plots
                aspectRatio = (double)zgcRunoffFreq.Height /
                    (double)zgcRunoffFreq.Width;
                myImage1 = new Bitmap(zgcRainRunoff.GetImage());
                myImage2 = new Bitmap(zgcRunoffFreq.GetImage());
                rw.WriteImagePage(tbSiteName.Text, myImage1, myImage2, "plot1",
                    "plot2", aspectRatio);

                // Write retention frequency & runoff by rainfall interval plot
                myImage1 = new Bitmap(zgcRainfallCapture.GetImage());
                myImage2 = new Bitmap(zgcRunoffPcnt.GetImage());
                rw.WriteImagePage(tbSiteName.Text, myImage1, myImage2, "plot3",
                    "plot4", aspectRatio);
                
                // Write extreme value plots
                aspectRatio = (double)zgcExtremeEvent.Height /
                    (double)zgcExtremeEvent.Width;
                myImage1 = new Bitmap(zgcExtremeEvent.GetImage());
                myImage2 = new Bitmap(zgcExtremePeak.GetImage());
                rw.WriteImagePage(tbSiteName.Text, myImage1, myImage2, "plot5",
                    "plot6", aspectRatio);

                // Write capital cost summary
                rw.WriteCostSummaryPage(tbSiteName.Text, costModuleResults);
                
                rw.CreatePDF(pdfFileName);
            }
        }
    
        //--------------------------------------------------------------------------
        //                    Map Server & Web Browser Methods
        //--------------------------------------------------------------------------

        // Creates the Map Server HTML file
        private void CreateMapServerFile(string path)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            StreamReader sr = new StreamReader(_assembly.GetManifestResourceStream(
                "StormwaterCalculator.maphtml.txt"));
            using ( StreamWriter sw = new StreamWriter(path) )
            {
                while (sr.Peek() >= 0) sw.WriteLine(sr.ReadLine());
            }
            sr.Close();
        }

        // Convenience method for calling JavaScript functions with arguments
        public object InvokeBrowserScript(string name, params object[] args)
        {
            return webBrowser1.Document.InvokeScript(name, args);
        }

        // Updates display of site's lat-long -- called from JavaScript function
        // in the web browser
        public void SetSiteLocation(String LatLng)
        {
            tbSiteLocation.Text = LatLng;
        }

        // Displays/selects a soil polygon's value when its clicked on the map
        public void SetSelectedPolygon(int i)
        {
            if (i >= 0 && i < GeoData.soilPolygons.Count)
            {
                MapPolygon p = (MapPolygon)GeoData.soilPolygons[i];

                // Soil Group being displayed
                if (tcMainLeft.SelectedTab == tpSoilType) switch (p.soilGroup)
                {
                    case "A": rbGroupA.Checked = true; break;
                    case "B": rbGroupB.Checked = true; break;
                    case "C": rbGroupC.Checked = true; break;
                    case "D": rbGroupD.Checked = true; break;
                }

                // Hydraulic Conductivity being displayed
                if (tcMainLeft.SelectedTab == tpSoilDrainage)
                {
                    tbKsat.Text = p.kSat.ToString("f3");
                }

                // Surface Slope being displayed
                if (tcMainLeft.SelectedTab == tpTopography)
                {
                    string slopeGroup = GeoData.GetSlopeGroup(p.slope);
                    switch (slopeGroup)
                    {
                        case "A":
                            rbFlatSlope.Checked = true;
                            break;
                        case "B":
                            rbModFlatSlope.Checked = true;
                            break;
                        case "C":
                            rbModSteepSlope.Checked = true;
                            break;
                        default:
                            rbSteepSlope.Checked = true;
                            break;
                    }
                }
            }
        }

        // Retrieves soil data from SSURGO database
        private void GetSoilData()
        {
            String statusLabelText = statusLabel.Text;
            statusLabel.Text = "Retrieving site data...";
            this.Cursor = Cursors.WaitCursor;
            progressBar.Visible = true;
            EnablePanels(false);
            this.backgroundWorker1.RunWorkerAsync(Actions.GET_SOIL_DATA);
            MainForm.PlaySound();
            while (this.backgroundWorker1.IsBusy)
            {
                Application.DoEvents();
            }
            MainForm.StopSound();
            progressBar.Visible = false;
            GeoData.ShowSiteData(this);
            EnablePanels(true);
            this.Cursor = Cursors.Default;
            statusLabel.Text = statusLabelText;
        }

        // Replaces listing of rain data sources -- called by GeoData.RetrieveMarkers
        public void RefreshRainSources(List<string> sources)
        {
            lbRainSource.Items.Clear();
            lbRainSource.Items.AddRange(sources.ToArray());
            if (SiteData.rainSourceIndex >= 0)
                lbRainSource.SelectedIndex = SiteData.rainSourceIndex;
            else
                lbRainSource.SelectedIndex = 0;
        }

        // Replaces listing of evap data sources -- called by GeoData.RetrieveMarkers
        public void RefreshEvapSources(List<string> sources)
        {
            lbEvapSource.Items.Clear();
            lbEvapSource.Items.AddRange(sources.ToArray());
            if (SiteData.evapSourceIndex >= 0)
                lbEvapSource.SelectedIndex = SiteData.evapSourceIndex;
            else
                lbEvapSource.SelectedIndex = 0;
        }

        // Restores map boundaries after viewing rainfall or evap stations
        private void RestoreMapBounds()
        {
            // Case 1: selecting to view rainfall stations from non-evap view
            if (tcMainTop.SelectedTab == rainTab && tcMainLeft.SelectedTab != tpEvapData)
            {
                InvokeBrowserScript("saveMapBounds");
            }

            // Case 2: selecting to view evap stations from a non-rainfall view
            else if (tcMainTop.SelectedTab == evapTab && tcMainLeft.SelectedTab != tpRainData)
            {
                InvokeBrowserScript("saveMapBounds");
            }

            // Case 3: selecting a non-evap view from a rainfall view
            else if (tcMainLeft.SelectedTab == tpRainData && tcMainTop.SelectedTab != evapTab)
            {
                InvokeBrowserScript("restoreMapBounds");
            }

            // Case 4: selecting a non-rainfall view from an evap view
            else if (tcMainLeft.SelectedTab == tpEvapData && tcMainTop.SelectedTab != rainTab)
            {
                InvokeBrowserScript("restoreMapBounds");
            }
        }

        // Enables/disables the form's user-input controls
        private void EnablePanels(bool enabled)
        {
            tcMainTop.Enabled = enabled;
            tcMainLeft.Enabled = enabled;
            btnRestart.Enabled = enabled;
            btnSave.Enabled = enabled;
        }

        //--------------------------------------------------------------------------
        //                         Background Worker Methods
        //--------------------------------------------------------------------------

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if ((Actions)e.Argument == Actions.GET_SOIL_DATA)
                e.Result = RetrieveSoilData();

            else if ((Actions)e.Argument == Actions.GET_RAINFALL)
                e.Result = GeoData.GetSwmmRainfall(SiteData.rainSourceIndex);

            else if ((Actions)e.Argument == Actions.RUN_SWMM)
                e.Result = swmm_run(SiteData.inpFile, SiteData.rptFile, SiteData.outFile);

            else if ((Actions)e.Argument == Actions.CALC_STATS)
                e.Result = Runoff.GetStats(SiteData.outFile);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            errorCode = (int)e.Result;
            progressBar.Visible = false;
        }

        // Retrieves coordinates of overlay polygons for soils data
        private int RetrieveSoilData()
        {
            double lat;
            double lng;
            GetLatLng(out lat, out lng);
            soilDataRetrieved = GeoData.GetSoilData(lat, lng);
            return 0;
        }

        // Retrieves lat, long values from text box entry
        public void GetLatLng(out double lat, out double lng)
        {
            char[] delimiters = { ',' };
            string[] tokens = tbSiteLocation.Text.Split(delimiters,
                StringSplitOptions.RemoveEmptyEntries);
            lat = Double.Parse(tokens[0]);
            lng = Double.Parse(tokens[1]);
        }

        //---------------------------------------------------------------------
        //                         Sound Effects Methods
        //---------------------------------------------------------------------

        public static void PlaySound()
        {
            if (soundStatus == 1) waitingSound.PlayLooping();
        }

        public static void StopSound()
        {
            if (soundStatus == 1) waitingSound.Stop();
        }

        private void btnSound_Click(object sender, EventArgs e)
        {
            if (soundStatus == 1)
            {
                waitingSound.Stop();
                soundStatus = 0;
                btnSound.ToolTipText = "Click to turn sound on.";
            }
            else
            {
                soundStatus = 1;
                btnSound.ToolTipText = "Click to turn sound off.";
            }
            btnSound.Image = imageList1.Images[soundStatus];
        }
        
        public void WriteDebugLine(string s)
        {
            //tbDebug.Text += s + Environment.NewLine;
        }

        //----------------------------------------------------------------------
        //             Custom Drawing of Met Station Data List Boxes
        //----------------------------------------------------------------------

        private void lbSource_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            // Get the ListBox and the item.
            ListBox lb = sender as ListBox;
            string txt = (string)lb.Items[e.Index];

            // Measure the string.
            SizeF txt_size = e.Graphics.MeasureString(txt, this.Font);

            // Set the required size.
            e.ItemHeight = (int)txt_size.Height + 2 * ItemMargin;
            e.ItemWidth = (int)txt_size.Width;
        }

        private void lbSource_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Get the ListBox and the item.
            ListBox lb = sender as ListBox;
            string txt = (string)lb.Items[e.Index];

            // Determine the brush used to draw the item
            Brush br = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                br = SystemBrushes.HighlightText;

            // Draw the background.
            e.DrawBackground();

            // Draw the item
            e.Graphics.DrawString(txt, lb.Font, br, e.Bounds.Left,
                e.Bounds.Top + ItemMargin, StringFormat.GenericDefault);

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }

        //additions for cost module
        //added to resize cost module web browser graphics when form changes
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (BrowserCommunicator.isBrowserLoaded == true)
            {
                BrowserCommunicator.onParentContainerResizeHandler(tpCost.Width, tpCost.Height);
            }
        }

        //additions for cost module
        private void cmbCostRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentMyComboBoxIndex = cmbCostRegion.SelectedIndex;
            CostRegionalization.BlsCenter selectedBLSCenter = cmbCostRegion.SelectedValue as CostRegionalization.BlsCenter;
            tbRegMultiplier.Text = selectedBLSCenter.regionalFactor.ToString();
            if (selectedBLSCenter.blsCity == "Other")
            {
                tbRegMultiplier.Enabled = true;
            }
            else
            {
                tbRegMultiplier.Enabled = false;
            }
            BrowserCommunicator.selectedBLSCenter = selectedBLSCenter;
        }

        private void llCostModuleHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel linkLabel = (LinkLabel)sender;
            CostHelpForm.activePageIndex = Int32.Parse((String)linkLabel.Tag);
            CostHelpForm.ShowDialog();
        }
    }
}
