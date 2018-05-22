namespace StormwaterCalculator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblPcntImperv = new System.Windows.Forms.Label();
            this.lblPcntDesert = new System.Windows.Forms.Label();
            this.lblPcntLawn = new System.Windows.Forms.Label();
            this.lblPcntMeadow = new System.Windows.Forms.Label();
            this.lblPcntForest = new System.Windows.Forms.Label();
            this.cbIgnoreConsecDays = new System.Windows.Forms.CheckBox();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.tcMainTop = new System.Windows.Forms.TabControl();
            this.overviewTab = new System.Windows.Forms.TabPage();
            this.locationTab = new System.Windows.Forms.TabPage();
            this.soilTab = new System.Windows.Forms.TabPage();
            this.kSatTab = new System.Windows.Forms.TabPage();
            this.slopeTab = new System.Windows.Forms.TabPage();
            this.rainTab = new System.Windows.Forms.TabPage();
            this.evapTab = new System.Windows.Forms.TabPage();
            this.climateTab = new System.Windows.Forms.TabPage();
            this.coverTab = new System.Windows.Forms.TabPage();
            this.lidTab = new System.Windows.Forms.TabPage();
            this.resultsTab = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.spacerStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnRestart = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSave = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnExit = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSound = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.tcMainRight = new System.Windows.Forms.TabControl();
            this.tpWebBrowser = new System.Windows.Forms.TabPage();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tpClimateRight = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.zgcMonthlyAdjust = new ZedGraph.ZedGraphControl();
            this.zgcAnnualMaxAdjust = new ZedGraph.ZedGraphControl();
            this.tpResultsRight = new System.Windows.Forms.TabPage();
            this.tcResults = new System.Windows.Forms.TabControl();
            this.tpSiteDescription = new System.Windows.Forms.TabPage();
            this.lvSiteSummary = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpSummaryResults = new System.Windows.Forms.TabPage();
            this.panel14 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.zgcBaseAnnualBudget = new ZedGraph.ZedGraphControl();
            this.zgcAnnualBudget = new ZedGraph.ZedGraphControl();
            this.lvRunoffSummary = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpEventPlot = new System.Windows.Forms.TabPage();
            this.zgcRainRunoff = new ZedGraph.ZedGraphControl();
            this.tpRunoffFreqPlot = new System.Windows.Forms.TabPage();
            this.zgcRunoffFreq = new ZedGraph.ZedGraphControl();
            this.tpRetentionFreqPlot = new System.Windows.Forms.TabPage();
            this.zgcRainfallCapture = new ZedGraph.ZedGraphControl();
            this.tpRunoffBySizePlot = new System.Windows.Forms.TabPage();
            this.zgcRunoffPcnt = new ZedGraph.ZedGraphControl();
            this.tpRainRunoffPlot = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.zgcExtremePeak = new ZedGraph.ZedGraphControl();
            this.zgcExtremeEvent = new ZedGraph.ZedGraphControl();
            this.tpCost = new System.Windows.Forms.TabPage();
            this.resultsHelpLabel = new System.Windows.Forms.LinkLabel();
            this.tcMainLeft = new System.Windows.Forms.TabControl();
            this.tpIntro = new System.Windows.Forms.TabPage();
            this.lblIntro = new System.Windows.Forms.Label();
            this.lblRelease = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tpLocation = new System.Windows.Forms.TabPage();
            this.nudSiteArea = new System.Windows.Forms.NumericUpDown();
            this.label28 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.tbSiteAddress = new System.Windows.Forms.TextBox();
            this.tbSiteLocation = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tbSiteName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.LinkLabel();
            this.tpSoilType = new System.Windows.Forms.TabPage();
            this.soilHelpLabel = new System.Windows.Forms.LinkLabel();
            this.soilGroupPanel = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSoilGroup = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbGroupD = new System.Windows.Forms.RadioButton();
            this.rbGroupC = new System.Windows.Forms.RadioButton();
            this.rbGroupB = new System.Windows.Forms.RadioButton();
            this.rbGroupA = new System.Windows.Forms.RadioButton();
            this.tpSoilDrainage = new System.Windows.Forms.TabPage();
            this.ksatHelpLabel = new System.Windows.Forms.LinkLabel();
            this.conductivityPanel = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDefaultKsat = new System.Windows.Forms.Label();
            this.cbKsat = new System.Windows.Forms.CheckBox();
            this.tbKsat = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.tpTopography = new System.Windows.Forms.TabPage();
            this.slopeHelpLabel = new System.Windows.Forms.LinkLabel();
            this.slopePanel = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.rbSteepSlope = new System.Windows.Forms.RadioButton();
            this.cbSlope = new System.Windows.Forms.CheckBox();
            this.rbModSteepSlope = new System.Windows.Forms.RadioButton();
            this.rbModFlatSlope = new System.Windows.Forms.RadioButton();
            this.rbFlatSlope = new System.Windows.Forms.RadioButton();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.tpRainData = new System.Windows.Forms.TabPage();
            this.rainHelpLabel = new System.Windows.Forms.LinkLabel();
            this.rainfallPanel = new System.Windows.Forms.Panel();
            this.btnSaveRainfall = new System.Windows.Forms.LinkLabel();
            this.lbRainSource = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tpEvapData = new System.Windows.Forms.TabPage();
            this.evapHelpLabel = new System.Windows.Forms.LinkLabel();
            this.evapPanel = new System.Windows.Forms.Panel();
            this.btnSaveEvap = new System.Windows.Forms.LinkLabel();
            this.lbEvapSource = new System.Windows.Forms.ListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tpClimateLeft = new System.Windows.Forms.TabPage();
            this.panel13 = new System.Windows.Forms.Panel();
            this.rbClimate2060 = new System.Windows.Forms.RadioButton();
            this.rbClimate2035 = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.climateHelpLabel = new System.Windows.Forms.LinkLabel();
            this.rbClimate4 = new System.Windows.Forms.RadioButton();
            this.rbClimate3 = new System.Windows.Forms.RadioButton();
            this.rbClimate2 = new System.Windows.Forms.RadioButton();
            this.rbClimate1 = new System.Windows.Forms.RadioButton();
            this.label30 = new System.Windows.Forms.Label();
            this.tpLandCover = new System.Windows.Forms.TabPage();
            this.landCoverHelpLabel = new System.Windows.Forms.LinkLabel();
            this.landCoverPanel = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.tlpLandCover = new System.Windows.Forms.TableLayoutPanel();
            this.tbImperv = new System.Windows.Forms.TextBox();
            this.nudDesert = new System.Windows.Forms.NumericUpDown();
            this.nudLawn = new System.Windows.Forms.NumericUpDown();
            this.nudMeadow = new System.Windows.Forms.NumericUpDown();
            this.nudForest = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.tpLidsLeft = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.llSiteSuitExcellent = new System.Windows.Forms.LinkLabel();
            this.llSiteSuitModerate = new System.Windows.Forms.LinkLabel();
            this.llSiteSuitPoor = new System.Windows.Forms.LinkLabel();
            this.rbCostSiteExcellent = new System.Windows.Forms.RadioButton();
            this.rbCostSitePoor = new System.Windows.Forms.RadioButton();
            this.rbCostSiteModerate = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.llNewDev = new System.Windows.Forms.LinkLabel();
            this.llRedev = new System.Windows.Forms.LinkLabel();
            this.rbCostRedev = new System.Windows.Forms.RadioButton();
            this.rbCostNewDev = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label24 = new System.Windows.Forms.Label();
            this.tbRegMultiplier = new System.Windows.Forms.TextBox();
            this.llCostRegion = new System.Windows.Forms.LinkLabel();
            this.cmbCostRegion = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.lidHelpLabel = new System.Windows.Forms.LinkLabel();
            this.lidControlsPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tlpLidUsage = new System.Windows.Forms.TableLayoutPanel();
            this.nudImpDiscon = new System.Windows.Forms.NumericUpDown();
            this.nudRainHarvest = new System.Windows.Forms.NumericUpDown();
            this.nudRainGarden = new System.Windows.Forms.NumericUpDown();
            this.nudGreenRoof = new System.Windows.Forms.NumericUpDown();
            this.nudStreetPlanter = new System.Windows.Forms.NumericUpDown();
            this.nudInfilBasin = new System.Windows.Forms.NumericUpDown();
            this.nudPorousPave = new System.Windows.Forms.NumericUpDown();
            this.label32 = new System.Windows.Forms.Label();
            this.lidLabel1 = new System.Windows.Forms.LinkLabel();
            this.lidLabel2 = new System.Windows.Forms.LinkLabel();
            this.lidLabel3 = new System.Windows.Forms.LinkLabel();
            this.lidLabel4 = new System.Windows.Forms.LinkLabel();
            this.lidLabel5 = new System.Windows.Forms.LinkLabel();
            this.lidLabel6 = new System.Windows.Forms.LinkLabel();
            this.lidLabel7 = new System.Windows.Forms.LinkLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.nudDesignStorm = new System.Windows.Forms.NumericUpDown();
            this.tpResultsLeft = new System.Windows.Forms.TabPage();
            this.panel19 = new System.Windows.Forms.Panel();
            this.resultsLinkLabel = new System.Windows.Forms.LinkLabel();
            this.resultsPanel = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbCostCapSmry = new System.Windows.Forms.RadioButton();
            this.rbRainRunoffEvents = new System.Windows.Forms.RadioButton();
            this.rbXeventRainRunoff = new System.Windows.Forms.RadioButton();
            this.rbRunoffRainPcnt = new System.Windows.Forms.RadioButton();
            this.rbRainRetentionFreq = new System.Windows.Forms.RadioButton();
            this.rbRainRunoffFreq = new System.Windows.Forms.RadioButton();
            this.rbSummaryResults = new System.Windows.Forms.RadioButton();
            this.rbSiteDescription = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mnuRefreshResults = new System.Windows.Forms.LinkLabel();
            this.mnuPrintResults = new System.Windows.Forms.LinkLabel();
            this.mnuRemoveBaseline = new System.Windows.Forms.LinkLabel();
            this.mnuAddBaseline = new System.Windows.Forms.LinkLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nudEventThreshold = new System.Windows.Forms.NumericUpDown();
            this.nudYearsAnalyzed = new System.Windows.Forms.NumericUpDown();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tcMainTop.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.tcMainRight.SuspendLayout();
            this.tpWebBrowser.SuspendLayout();
            this.tpClimateRight.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tpResultsRight.SuspendLayout();
            this.tcResults.SuspendLayout();
            this.tpSiteDescription.SuspendLayout();
            this.tpSummaryResults.SuspendLayout();
            this.panel14.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tpEventPlot.SuspendLayout();
            this.tpRunoffFreqPlot.SuspendLayout();
            this.tpRetentionFreqPlot.SuspendLayout();
            this.tpRunoffBySizePlot.SuspendLayout();
            this.tpRainRunoffPlot.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tcMainLeft.SuspendLayout();
            this.tpIntro.SuspendLayout();
            this.tpLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSiteArea)).BeginInit();
            this.tpSoilType.SuspendLayout();
            this.soilGroupPanel.SuspendLayout();
            this.tpSoilDrainage.SuspendLayout();
            this.conductivityPanel.SuspendLayout();
            this.tpTopography.SuspendLayout();
            this.slopePanel.SuspendLayout();
            this.tpRainData.SuspendLayout();
            this.rainfallPanel.SuspendLayout();
            this.tpEvapData.SuspendLayout();
            this.evapPanel.SuspendLayout();
            this.tpClimateLeft.SuspendLayout();
            this.panel13.SuspendLayout();
            this.tpLandCover.SuspendLayout();
            this.landCoverPanel.SuspendLayout();
            this.tlpLandCover.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDesert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLawn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeadow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudForest)).BeginInit();
            this.tpLidsLeft.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.lidControlsPanel.SuspendLayout();
            this.tlpLidUsage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImpDiscon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRainHarvest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRainGarden)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGreenRoof)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStreetPlanter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInfilBasin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPorousPave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDesignStorm)).BeginInit();
            this.tpResultsLeft.SuspendLayout();
            this.panel19.SuspendLayout();
            this.resultsPanel.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEventThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYearsAnalyzed)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.toolTip1.SetToolTip(this.btnSearch, resources.GetString("btnSearch.ToolTip"));
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblPcntImperv
            // 
            resources.ApplyResources(this.lblPcntImperv, "lblPcntImperv");
            this.lblPcntImperv.Name = "lblPcntImperv";
            this.toolTip1.SetToolTip(this.lblPcntImperv, resources.GetString("lblPcntImperv.ToolTip"));
            // 
            // lblPcntDesert
            // 
            resources.ApplyResources(this.lblPcntDesert, "lblPcntDesert");
            this.lblPcntDesert.Name = "lblPcntDesert";
            this.toolTip1.SetToolTip(this.lblPcntDesert, resources.GetString("lblPcntDesert.ToolTip"));
            // 
            // lblPcntLawn
            // 
            resources.ApplyResources(this.lblPcntLawn, "lblPcntLawn");
            this.lblPcntLawn.Name = "lblPcntLawn";
            this.toolTip1.SetToolTip(this.lblPcntLawn, resources.GetString("lblPcntLawn.ToolTip"));
            // 
            // lblPcntMeadow
            // 
            resources.ApplyResources(this.lblPcntMeadow, "lblPcntMeadow");
            this.lblPcntMeadow.Name = "lblPcntMeadow";
            this.toolTip1.SetToolTip(this.lblPcntMeadow, resources.GetString("lblPcntMeadow.ToolTip"));
            // 
            // lblPcntForest
            // 
            resources.ApplyResources(this.lblPcntForest, "lblPcntForest");
            this.lblPcntForest.Name = "lblPcntForest";
            this.toolTip1.SetToolTip(this.lblPcntForest, resources.GetString("lblPcntForest.ToolTip"));
            // 
            // cbIgnoreConsecDays
            // 
            resources.ApplyResources(this.cbIgnoreConsecDays, "cbIgnoreConsecDays");
            this.cbIgnoreConsecDays.Name = "cbIgnoreConsecDays";
            this.toolTip1.SetToolTip(this.cbIgnoreConsecDays, resources.GetString("cbIgnoreConsecDays.ToolTip"));
            this.cbIgnoreConsecDays.UseVisualStyleBackColor = true;
            this.cbIgnoreConsecDays.CheckedChanged += new System.EventHandler(this.AnalysisOptionsChanged);
            // 
            // lblThreshold
            // 
            resources.ApplyResources(this.lblThreshold, "lblThreshold");
            this.lblThreshold.Name = "lblThreshold";
            this.toolTip1.SetToolTip(this.lblThreshold, resources.GetString("lblThreshold.ToolTip"));
            // 
            // lblDuration
            // 
            resources.ApplyResources(this.lblDuration, "lblDuration");
            this.lblDuration.Name = "lblDuration";
            this.toolTip1.SetToolTip(this.lblDuration, resources.GetString("lblDuration.ToolTip"));
            // 
            // tcMainTop
            // 
            this.tcMainTop.Controls.Add(this.overviewTab);
            this.tcMainTop.Controls.Add(this.locationTab);
            this.tcMainTop.Controls.Add(this.soilTab);
            this.tcMainTop.Controls.Add(this.kSatTab);
            this.tcMainTop.Controls.Add(this.slopeTab);
            this.tcMainTop.Controls.Add(this.rainTab);
            this.tcMainTop.Controls.Add(this.evapTab);
            this.tcMainTop.Controls.Add(this.climateTab);
            this.tcMainTop.Controls.Add(this.coverTab);
            this.tcMainTop.Controls.Add(this.lidTab);
            this.tcMainTop.Controls.Add(this.resultsTab);
            resources.ApplyResources(this.tcMainTop, "tcMainTop");
            this.tcMainTop.Name = "tcMainTop";
            this.tcMainTop.SelectedIndex = 0;
            this.tcMainTop.SelectedIndexChanged += new System.EventHandler(this.tcMainTop_SelectedIndexChanged);
            // 
            // overviewTab
            // 
            resources.ApplyResources(this.overviewTab, "overviewTab");
            this.overviewTab.Name = "overviewTab";
            this.overviewTab.UseVisualStyleBackColor = true;
            // 
            // locationTab
            // 
            resources.ApplyResources(this.locationTab, "locationTab");
            this.locationTab.Name = "locationTab";
            this.locationTab.UseVisualStyleBackColor = true;
            // 
            // soilTab
            // 
            resources.ApplyResources(this.soilTab, "soilTab");
            this.soilTab.Name = "soilTab";
            this.soilTab.UseVisualStyleBackColor = true;
            // 
            // kSatTab
            // 
            resources.ApplyResources(this.kSatTab, "kSatTab");
            this.kSatTab.Name = "kSatTab";
            this.kSatTab.UseVisualStyleBackColor = true;
            // 
            // slopeTab
            // 
            resources.ApplyResources(this.slopeTab, "slopeTab");
            this.slopeTab.Name = "slopeTab";
            this.slopeTab.UseVisualStyleBackColor = true;
            // 
            // rainTab
            // 
            resources.ApplyResources(this.rainTab, "rainTab");
            this.rainTab.Name = "rainTab";
            this.rainTab.UseVisualStyleBackColor = true;
            // 
            // evapTab
            // 
            resources.ApplyResources(this.evapTab, "evapTab");
            this.evapTab.Name = "evapTab";
            this.evapTab.UseVisualStyleBackColor = true;
            // 
            // climateTab
            // 
            resources.ApplyResources(this.climateTab, "climateTab");
            this.climateTab.Name = "climateTab";
            this.climateTab.UseVisualStyleBackColor = true;
            // 
            // coverTab
            // 
            resources.ApplyResources(this.coverTab, "coverTab");
            this.coverTab.Name = "coverTab";
            this.coverTab.UseVisualStyleBackColor = true;
            // 
            // lidTab
            // 
            resources.ApplyResources(this.lidTab, "lidTab");
            this.lidTab.Name = "lidTab";
            this.lidTab.UseVisualStyleBackColor = true;
            // 
            // resultsTab
            // 
            resources.ApplyResources(this.resultsTab, "resultsTab");
            this.resultsTab.Name = "resultsTab";
            this.resultsTab.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar,
            this.spacerStatusLabel,
            this.btnRestart,
            this.btnSave,
            this.btnExit,
            this.btnSound});
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            // 
            // statusLabel
            // 
            resources.ApplyResources(this.statusLabel, "statusLabel");
            this.statusLabel.Name = "statusLabel";
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Margin = new System.Windows.Forms.Padding(1, 8, 1, 7);
            this.progressBar.Name = "progressBar";
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // spacerStatusLabel
            // 
            this.spacerStatusLabel.Name = "spacerStatusLabel";
            resources.ApplyResources(this.spacerStatusLabel, "spacerStatusLabel");
            this.spacerStatusLabel.Spring = true;
            // 
            // btnRestart
            // 
            resources.ApplyResources(this.btnRestart, "btnRestart");
            this.btnRestart.IsLink = true;
            this.btnRestart.Margin = new System.Windows.Forms.Padding(0, 3, 5, 2);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.IsLink = true;
            this.btnSave.Margin = new System.Windows.Forms.Padding(0, 3, 5, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.IsLink = true;
            this.btnExit.Margin = new System.Windows.Forms.Padding(0, 3, 5, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSound
            // 
            resources.ApplyResources(this.btnSound, "btnSound");
            this.btnSound.Name = "btnSound";
            this.btnSound.Click += new System.EventHandler(this.btnSound_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.tcMainRight);
            this.mainPanel.Controls.Add(this.tcMainLeft);
            resources.ApplyResources(this.mainPanel, "mainPanel");
            this.mainPanel.Name = "mainPanel";
            // 
            // tcMainRight
            // 
            resources.ApplyResources(this.tcMainRight, "tcMainRight");
            this.tcMainRight.Controls.Add(this.tpWebBrowser);
            this.tcMainRight.Controls.Add(this.tpClimateRight);
            this.tcMainRight.Controls.Add(this.tpResultsRight);
            this.tcMainRight.Name = "tcMainRight";
            this.tcMainRight.SelectedIndex = 0;
            this.tcMainRight.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // tpWebBrowser
            // 
            this.tpWebBrowser.BackColor = System.Drawing.Color.Transparent;
            this.tpWebBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpWebBrowser.Controls.Add(this.webBrowser1);
            resources.ApplyResources(this.tpWebBrowser, "tpWebBrowser");
            this.tpWebBrowser.Name = "tpWebBrowser";
            // 
            // webBrowser1
            // 
            resources.ApplyResources(this.webBrowser1, "webBrowser1");
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.TabStop = false;
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // tpClimateRight
            // 
            this.tpClimateRight.BackColor = System.Drawing.Color.Transparent;
            this.tpClimateRight.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.tpClimateRight, "tpClimateRight");
            this.tpClimateRight.Name = "tpClimateRight";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.zgcMonthlyAdjust, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.zgcAnnualMaxAdjust, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // zgcMonthlyAdjust
            // 
            resources.ApplyResources(this.zgcMonthlyAdjust, "zgcMonthlyAdjust");
            this.zgcMonthlyAdjust.IsAntiAlias = true;
            this.zgcMonthlyAdjust.Name = "zgcMonthlyAdjust";
            this.zgcMonthlyAdjust.ScrollGrace = 0D;
            this.zgcMonthlyAdjust.ScrollMaxX = 0D;
            this.zgcMonthlyAdjust.ScrollMaxY = 0D;
            this.zgcMonthlyAdjust.ScrollMaxY2 = 0D;
            this.zgcMonthlyAdjust.ScrollMinX = 0D;
            this.zgcMonthlyAdjust.ScrollMinY = 0D;
            this.zgcMonthlyAdjust.ScrollMinY2 = 0D;
            // 
            // zgcAnnualMaxAdjust
            // 
            resources.ApplyResources(this.zgcAnnualMaxAdjust, "zgcAnnualMaxAdjust");
            this.zgcAnnualMaxAdjust.IsAntiAlias = true;
            this.zgcAnnualMaxAdjust.Name = "zgcAnnualMaxAdjust";
            this.zgcAnnualMaxAdjust.ScrollGrace = 0D;
            this.zgcAnnualMaxAdjust.ScrollMaxX = 0D;
            this.zgcAnnualMaxAdjust.ScrollMaxY = 0D;
            this.zgcAnnualMaxAdjust.ScrollMaxY2 = 0D;
            this.zgcAnnualMaxAdjust.ScrollMinX = 0D;
            this.zgcAnnualMaxAdjust.ScrollMinY = 0D;
            this.zgcAnnualMaxAdjust.ScrollMinY2 = 0D;
            // 
            // tpResultsRight
            // 
            this.tpResultsRight.BackColor = System.Drawing.SystemColors.Window;
            this.tpResultsRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpResultsRight.Controls.Add(this.tcResults);
            this.tpResultsRight.Controls.Add(this.resultsHelpLabel);
            resources.ApplyResources(this.tpResultsRight, "tpResultsRight");
            this.tpResultsRight.Name = "tpResultsRight";
            // 
            // tcResults
            // 
            resources.ApplyResources(this.tcResults, "tcResults");
            this.tcResults.Controls.Add(this.tpSiteDescription);
            this.tcResults.Controls.Add(this.tpSummaryResults);
            this.tcResults.Controls.Add(this.tpEventPlot);
            this.tcResults.Controls.Add(this.tpRunoffFreqPlot);
            this.tcResults.Controls.Add(this.tpRetentionFreqPlot);
            this.tcResults.Controls.Add(this.tpRunoffBySizePlot);
            this.tcResults.Controls.Add(this.tpRainRunoffPlot);
            this.tcResults.Controls.Add(this.tpCost);
            this.tcResults.Name = "tcResults";
            this.tcResults.SelectedIndex = 0;
            this.tcResults.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // tpSiteDescription
            // 
            this.tpSiteDescription.BackColor = System.Drawing.SystemColors.Window;
            this.tpSiteDescription.Controls.Add(this.lvSiteSummary);
            resources.ApplyResources(this.tpSiteDescription, "tpSiteDescription");
            this.tpSiteDescription.Name = "tpSiteDescription";
            // 
            // lvSiteSummary
            // 
            this.lvSiteSummary.BackColor = System.Drawing.SystemColors.Window;
            this.lvSiteSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSiteSummary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            resources.ApplyResources(this.lvSiteSummary, "lvSiteSummary");
            this.lvSiteSummary.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvSiteSummary.Groups"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvSiteSummary.Groups1"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvSiteSummary.Groups2"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lvSiteSummary.Groups3")))});
            this.lvSiteSummary.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvSiteSummary.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items1"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items2"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items3"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items4"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items5"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items6"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items7"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items8"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items9"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items10"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items11"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items12"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items13"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items14"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items15"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items16"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items17"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items18"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items19"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items20"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvSiteSummary.Items21")))});
            this.lvSiteSummary.Name = "lvSiteSummary";
            this.lvSiteSummary.ShowItemToolTips = true;
            this.lvSiteSummary.TabStop = false;
            this.lvSiteSummary.UseCompatibleStateImageBehavior = false;
            this.lvSiteSummary.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // tpSummaryResults
            // 
            this.tpSummaryResults.BackColor = System.Drawing.SystemColors.Window;
            this.tpSummaryResults.Controls.Add(this.panel14);
            this.tpSummaryResults.Controls.Add(this.lvRunoffSummary);
            resources.ApplyResources(this.tpSummaryResults, "tpSummaryResults");
            this.tpSummaryResults.Name = "tpSummaryResults";
            // 
            // panel14
            // 
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel14.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.panel14, "panel14");
            this.panel14.Name = "panel14";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.zgcBaseAnnualBudget, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.zgcAnnualBudget, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // zgcBaseAnnualBudget
            // 
            this.zgcBaseAnnualBudget.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.zgcBaseAnnualBudget, "zgcBaseAnnualBudget");
            this.zgcBaseAnnualBudget.Name = "zgcBaseAnnualBudget";
            this.zgcBaseAnnualBudget.ScrollGrace = 0D;
            this.zgcBaseAnnualBudget.ScrollMaxX = 0D;
            this.zgcBaseAnnualBudget.ScrollMaxY = 0D;
            this.zgcBaseAnnualBudget.ScrollMaxY2 = 0D;
            this.zgcBaseAnnualBudget.ScrollMinX = 0D;
            this.zgcBaseAnnualBudget.ScrollMinY = 0D;
            this.zgcBaseAnnualBudget.ScrollMinY2 = 0D;
            // 
            // zgcAnnualBudget
            // 
            this.zgcAnnualBudget.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.zgcAnnualBudget, "zgcAnnualBudget");
            this.zgcAnnualBudget.Name = "zgcAnnualBudget";
            this.zgcAnnualBudget.ScrollGrace = 0D;
            this.zgcAnnualBudget.ScrollMaxX = 0D;
            this.zgcAnnualBudget.ScrollMaxY = 0D;
            this.zgcAnnualBudget.ScrollMaxY2 = 0D;
            this.zgcAnnualBudget.ScrollMinX = 0D;
            this.zgcAnnualBudget.ScrollMinY = 0D;
            this.zgcAnnualBudget.ScrollMinY2 = 0D;
            // 
            // lvRunoffSummary
            // 
            this.lvRunoffSummary.BackColor = System.Drawing.SystemColors.Window;
            this.lvRunoffSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvRunoffSummary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader6});
            resources.ApplyResources(this.lvRunoffSummary, "lvRunoffSummary");
            this.lvRunoffSummary.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lvRunoffSummary.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvRunoffSummary.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvRunoffSummary.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvRunoffSummary.Items1"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvRunoffSummary.Items2"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvRunoffSummary.Items3"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvRunoffSummary.Items4"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvRunoffSummary.Items5"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvRunoffSummary.Items6"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvRunoffSummary.Items7")))});
            this.lvRunoffSummary.Name = "lvRunoffSummary";
            this.lvRunoffSummary.ShowItemToolTips = true;
            this.lvRunoffSummary.TabStop = false;
            this.lvRunoffSummary.UseCompatibleStateImageBehavior = false;
            this.lvRunoffSummary.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // tpEventPlot
            // 
            this.tpEventPlot.Controls.Add(this.zgcRainRunoff);
            resources.ApplyResources(this.tpEventPlot, "tpEventPlot");
            this.tpEventPlot.Name = "tpEventPlot";
            this.tpEventPlot.UseVisualStyleBackColor = true;
            // 
            // zgcRainRunoff
            // 
            this.zgcRainRunoff.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.zgcRainRunoff, "zgcRainRunoff");
            this.zgcRainRunoff.IsAntiAlias = true;
            this.zgcRainRunoff.Name = "zgcRainRunoff";
            this.zgcRainRunoff.ScrollGrace = 0D;
            this.zgcRainRunoff.ScrollMaxX = 0D;
            this.zgcRainRunoff.ScrollMaxY = 0D;
            this.zgcRainRunoff.ScrollMaxY2 = 0D;
            this.zgcRainRunoff.ScrollMinX = 0D;
            this.zgcRainRunoff.ScrollMinY = 0D;
            this.zgcRainRunoff.ScrollMinY2 = 0D;
            // 
            // tpRunoffFreqPlot
            // 
            this.tpRunoffFreqPlot.BackColor = System.Drawing.SystemColors.Window;
            this.tpRunoffFreqPlot.Controls.Add(this.zgcRunoffFreq);
            resources.ApplyResources(this.tpRunoffFreqPlot, "tpRunoffFreqPlot");
            this.tpRunoffFreqPlot.Name = "tpRunoffFreqPlot";
            // 
            // zgcRunoffFreq
            // 
            this.zgcRunoffFreq.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.zgcRunoffFreq, "zgcRunoffFreq");
            this.zgcRunoffFreq.IsAntiAlias = true;
            this.zgcRunoffFreq.Name = "zgcRunoffFreq";
            this.zgcRunoffFreq.ScrollGrace = 0D;
            this.zgcRunoffFreq.ScrollMaxX = 0D;
            this.zgcRunoffFreq.ScrollMaxY = 0D;
            this.zgcRunoffFreq.ScrollMaxY2 = 0D;
            this.zgcRunoffFreq.ScrollMinX = 0D;
            this.zgcRunoffFreq.ScrollMinY = 0D;
            this.zgcRunoffFreq.ScrollMinY2 = 0D;
            // 
            // tpRetentionFreqPlot
            // 
            this.tpRetentionFreqPlot.BackColor = System.Drawing.SystemColors.Window;
            this.tpRetentionFreqPlot.Controls.Add(this.zgcRainfallCapture);
            resources.ApplyResources(this.tpRetentionFreqPlot, "tpRetentionFreqPlot");
            this.tpRetentionFreqPlot.Name = "tpRetentionFreqPlot";
            // 
            // zgcRainfallCapture
            // 
            this.zgcRainfallCapture.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.zgcRainfallCapture, "zgcRainfallCapture");
            this.zgcRainfallCapture.IsAntiAlias = true;
            this.zgcRainfallCapture.Name = "zgcRainfallCapture";
            this.zgcRainfallCapture.ScrollGrace = 0D;
            this.zgcRainfallCapture.ScrollMaxX = 0D;
            this.zgcRainfallCapture.ScrollMaxY = 0D;
            this.zgcRainfallCapture.ScrollMaxY2 = 0D;
            this.zgcRainfallCapture.ScrollMinX = 0D;
            this.zgcRainfallCapture.ScrollMinY = 0D;
            this.zgcRainfallCapture.ScrollMinY2 = 0D;
            // 
            // tpRunoffBySizePlot
            // 
            this.tpRunoffBySizePlot.BackColor = System.Drawing.SystemColors.Window;
            this.tpRunoffBySizePlot.Controls.Add(this.zgcRunoffPcnt);
            resources.ApplyResources(this.tpRunoffBySizePlot, "tpRunoffBySizePlot");
            this.tpRunoffBySizePlot.Name = "tpRunoffBySizePlot";
            // 
            // zgcRunoffPcnt
            // 
            this.zgcRunoffPcnt.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.zgcRunoffPcnt, "zgcRunoffPcnt");
            this.zgcRunoffPcnt.IsAntiAlias = true;
            this.zgcRunoffPcnt.Name = "zgcRunoffPcnt";
            this.zgcRunoffPcnt.ScrollGrace = 0D;
            this.zgcRunoffPcnt.ScrollMaxX = 0D;
            this.zgcRunoffPcnt.ScrollMaxY = 0D;
            this.zgcRunoffPcnt.ScrollMaxY2 = 0D;
            this.zgcRunoffPcnt.ScrollMinX = 0D;
            this.zgcRunoffPcnt.ScrollMinY = 0D;
            this.zgcRunoffPcnt.ScrollMinY2 = 0D;
            // 
            // tpRainRunoffPlot
            // 
            this.tpRainRunoffPlot.BackColor = System.Drawing.SystemColors.Window;
            this.tpRainRunoffPlot.Controls.Add(this.tableLayoutPanel3);
            resources.ApplyResources(this.tpRainRunoffPlot, "tpRainRunoffPlot");
            this.tpRainRunoffPlot.Name = "tpRainRunoffPlot";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.zgcExtremePeak, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.zgcExtremeEvent, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // zgcExtremePeak
            // 
            this.zgcExtremePeak.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.zgcExtremePeak, "zgcExtremePeak");
            this.zgcExtremePeak.IsAntiAlias = true;
            this.zgcExtremePeak.Name = "zgcExtremePeak";
            this.zgcExtremePeak.ScrollGrace = 0D;
            this.zgcExtremePeak.ScrollMaxX = 0D;
            this.zgcExtremePeak.ScrollMaxY = 0D;
            this.zgcExtremePeak.ScrollMaxY2 = 0D;
            this.zgcExtremePeak.ScrollMinX = 0D;
            this.zgcExtremePeak.ScrollMinY = 0D;
            this.zgcExtremePeak.ScrollMinY2 = 0D;
            // 
            // zgcExtremeEvent
            // 
            this.zgcExtremeEvent.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.zgcExtremeEvent, "zgcExtremeEvent");
            this.zgcExtremeEvent.IsAntiAlias = true;
            this.zgcExtremeEvent.Name = "zgcExtremeEvent";
            this.zgcExtremeEvent.ScrollGrace = 0D;
            this.zgcExtremeEvent.ScrollMaxX = 0D;
            this.zgcExtremeEvent.ScrollMaxY = 0D;
            this.zgcExtremeEvent.ScrollMaxY2 = 0D;
            this.zgcExtremeEvent.ScrollMinX = 0D;
            this.zgcExtremeEvent.ScrollMinY = 0D;
            this.zgcExtremeEvent.ScrollMinY2 = 0D;
            // 
            // tpCost
            // 
            resources.ApplyResources(this.tpCost, "tpCost");
            this.tpCost.Name = "tpCost";
            this.tpCost.UseVisualStyleBackColor = true;
            // 
            // resultsHelpLabel
            // 
            this.resultsHelpLabel.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.resultsHelpLabel, "resultsHelpLabel");
            this.resultsHelpLabel.Name = "resultsHelpLabel";
            this.resultsHelpLabel.TabStop = true;
            this.resultsHelpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.resultsHelpLabel_LinkClicked);
            // 
            // tcMainLeft
            // 
            resources.ApplyResources(this.tcMainLeft, "tcMainLeft");
            this.tcMainLeft.Controls.Add(this.tpIntro);
            this.tcMainLeft.Controls.Add(this.tpLocation);
            this.tcMainLeft.Controls.Add(this.tpSoilType);
            this.tcMainLeft.Controls.Add(this.tpSoilDrainage);
            this.tcMainLeft.Controls.Add(this.tpTopography);
            this.tcMainLeft.Controls.Add(this.tpRainData);
            this.tcMainLeft.Controls.Add(this.tpEvapData);
            this.tcMainLeft.Controls.Add(this.tpClimateLeft);
            this.tcMainLeft.Controls.Add(this.tpLandCover);
            this.tcMainLeft.Controls.Add(this.tpLidsLeft);
            this.tcMainLeft.Controls.Add(this.tpResultsLeft);
            this.tcMainLeft.Name = "tcMainLeft";
            this.tcMainLeft.SelectedIndex = 0;
            this.tcMainLeft.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcMainLeft.SelectedIndexChanged += new System.EventHandler(this.tcMainLeft_SelectedIndexChanged);
            // 
            // tpIntro
            // 
            this.tpIntro.BackColor = System.Drawing.Color.AliceBlue;
            this.tpIntro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpIntro.Controls.Add(this.lblIntro);
            this.tpIntro.Controls.Add(this.lblRelease);
            this.tpIntro.Controls.Add(this.label15);
            resources.ApplyResources(this.tpIntro, "tpIntro");
            this.tpIntro.Name = "tpIntro";
            // 
            // lblIntro
            // 
            resources.ApplyResources(this.lblIntro, "lblIntro");
            this.lblIntro.Name = "lblIntro";
            // 
            // lblRelease
            // 
            resources.ApplyResources(this.lblRelease, "lblRelease");
            this.lblRelease.Name = "lblRelease";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // tpLocation
            // 
            this.tpLocation.BackColor = System.Drawing.Color.AliceBlue;
            this.tpLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpLocation.Controls.Add(this.nudSiteArea);
            this.tpLocation.Controls.Add(this.label28);
            this.tpLocation.Controls.Add(this.label4);
            this.tpLocation.Controls.Add(this.btnSearch);
            this.tpLocation.Controls.Add(this.label26);
            this.tpLocation.Controls.Add(this.tbSiteAddress);
            this.tpLocation.Controls.Add(this.tbSiteLocation);
            this.tpLocation.Controls.Add(this.label18);
            this.tpLocation.Controls.Add(this.tbSiteName);
            this.tpLocation.Controls.Add(this.label2);
            this.tpLocation.Controls.Add(this.btnOpen);
            resources.ApplyResources(this.tpLocation, "tpLocation");
            this.tpLocation.Name = "tpLocation";
            // 
            // nudSiteArea
            // 
            this.nudSiteArea.DecimalPlaces = 1;
            resources.ApplyResources(this.nudSiteArea, "nudSiteArea");
            this.nudSiteArea.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudSiteArea.Name = "nudSiteArea";
            this.nudSiteArea.ValueChanged += new System.EventHandler(this.nudSiteArea_ValueChanged);
            this.nudSiteArea.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // tbSiteAddress
            // 
            resources.ApplyResources(this.tbSiteAddress, "tbSiteAddress");
            this.tbSiteAddress.Name = "tbSiteAddress";
            this.tbSiteAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSiteAddress_KeyDown);
            // 
            // tbSiteLocation
            // 
            this.tbSiteLocation.BackColor = System.Drawing.Color.AliceBlue;
            resources.ApplyResources(this.tbSiteLocation, "tbSiteLocation");
            this.tbSiteLocation.Name = "tbSiteLocation";
            this.tbSiteLocation.ReadOnly = true;
            this.tbSiteLocation.TabStop = false;
            this.tbSiteLocation.TextChanged += new System.EventHandler(this.tbSiteLocation_TextChanged);
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // tbSiteName
            // 
            resources.ApplyResources(this.tbSiteName, "tbSiteName");
            this.tbSiteName.Name = "tbSiteName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnOpen
            // 
            resources.ApplyResources(this.btnOpen, "btnOpen");
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.TabStop = true;
            this.btnOpen.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnOpen_LinkClicked);
            // 
            // tpSoilType
            // 
            this.tpSoilType.BackColor = System.Drawing.Color.AliceBlue;
            this.tpSoilType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpSoilType.Controls.Add(this.soilHelpLabel);
            this.tpSoilType.Controls.Add(this.soilGroupPanel);
            resources.ApplyResources(this.tpSoilType, "tpSoilType");
            this.tpSoilType.Name = "tpSoilType";
            // 
            // soilHelpLabel
            // 
            resources.ApplyResources(this.soilHelpLabel, "soilHelpLabel");
            this.soilHelpLabel.Name = "soilHelpLabel";
            this.soilHelpLabel.TabStop = true;
            this.soilHelpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked_1);
            // 
            // soilGroupPanel
            // 
            this.soilGroupPanel.Controls.Add(this.label9);
            this.soilGroupPanel.Controls.Add(this.label3);
            this.soilGroupPanel.Controls.Add(this.cbSoilGroup);
            this.soilGroupPanel.Controls.Add(this.panel4);
            this.soilGroupPanel.Controls.Add(this.panel3);
            this.soilGroupPanel.Controls.Add(this.panel2);
            this.soilGroupPanel.Controls.Add(this.panel1);
            this.soilGroupPanel.Controls.Add(this.rbGroupD);
            this.soilGroupPanel.Controls.Add(this.rbGroupC);
            this.soilGroupPanel.Controls.Add(this.rbGroupB);
            this.soilGroupPanel.Controls.Add(this.rbGroupA);
            resources.ApplyResources(this.soilGroupPanel, "soilGroupPanel");
            this.soilGroupPanel.Name = "soilGroupPanel";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cbSoilGroup
            // 
            resources.ApplyResources(this.cbSoilGroup, "cbSoilGroup");
            this.cbSoilGroup.Name = "cbSoilGroup";
            this.cbSoilGroup.UseVisualStyleBackColor = true;
            this.cbSoilGroup.CheckedChanged += new System.EventHandler(this.cbSoilGroup_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Fuchsia;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Yellow;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Lime;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Cyan;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // rbGroupD
            // 
            resources.ApplyResources(this.rbGroupD, "rbGroupD");
            this.rbGroupD.Name = "rbGroupD";
            this.rbGroupD.TabStop = true;
            this.rbGroupD.Tag = "3";
            this.rbGroupD.UseVisualStyleBackColor = true;
            this.rbGroupD.CheckedChanged += new System.EventHandler(this.rbGroup_Changed);
            // 
            // rbGroupC
            // 
            resources.ApplyResources(this.rbGroupC, "rbGroupC");
            this.rbGroupC.Name = "rbGroupC";
            this.rbGroupC.TabStop = true;
            this.rbGroupC.Tag = "2";
            this.rbGroupC.UseVisualStyleBackColor = true;
            this.rbGroupC.CheckedChanged += new System.EventHandler(this.rbGroup_Changed);
            // 
            // rbGroupB
            // 
            resources.ApplyResources(this.rbGroupB, "rbGroupB");
            this.rbGroupB.Name = "rbGroupB";
            this.rbGroupB.TabStop = true;
            this.rbGroupB.Tag = "1";
            this.rbGroupB.UseVisualStyleBackColor = true;
            this.rbGroupB.CheckedChanged += new System.EventHandler(this.rbGroup_Changed);
            // 
            // rbGroupA
            // 
            resources.ApplyResources(this.rbGroupA, "rbGroupA");
            this.rbGroupA.Checked = true;
            this.rbGroupA.Name = "rbGroupA";
            this.rbGroupA.TabStop = true;
            this.rbGroupA.Tag = "0";
            this.rbGroupA.UseVisualStyleBackColor = true;
            this.rbGroupA.CheckedChanged += new System.EventHandler(this.rbGroup_Changed);
            // 
            // tpSoilDrainage
            // 
            this.tpSoilDrainage.BackColor = System.Drawing.Color.AliceBlue;
            this.tpSoilDrainage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpSoilDrainage.Controls.Add(this.ksatHelpLabel);
            this.tpSoilDrainage.Controls.Add(this.conductivityPanel);
            resources.ApplyResources(this.tpSoilDrainage, "tpSoilDrainage");
            this.tpSoilDrainage.Name = "tpSoilDrainage";
            // 
            // ksatHelpLabel
            // 
            resources.ApplyResources(this.ksatHelpLabel, "ksatHelpLabel");
            this.ksatHelpLabel.Name = "ksatHelpLabel";
            this.ksatHelpLabel.TabStop = true;
            this.ksatHelpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked_1);
            // 
            // conductivityPanel
            // 
            this.conductivityPanel.Controls.Add(this.label11);
            this.conductivityPanel.Controls.Add(this.label6);
            this.conductivityPanel.Controls.Add(this.lblDefaultKsat);
            this.conductivityPanel.Controls.Add(this.cbKsat);
            this.conductivityPanel.Controls.Add(this.tbKsat);
            this.conductivityPanel.Controls.Add(this.label21);
            this.conductivityPanel.Controls.Add(this.label19);
            this.conductivityPanel.Controls.Add(this.label17);
            this.conductivityPanel.Controls.Add(this.label16);
            this.conductivityPanel.Controls.Add(this.panel5);
            this.conductivityPanel.Controls.Add(this.panel6);
            this.conductivityPanel.Controls.Add(this.panel7);
            this.conductivityPanel.Controls.Add(this.panel8);
            resources.ApplyResources(this.conductivityPanel, "conductivityPanel");
            this.conductivityPanel.Name = "conductivityPanel";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblDefaultKsat
            // 
            resources.ApplyResources(this.lblDefaultKsat, "lblDefaultKsat");
            this.lblDefaultKsat.Name = "lblDefaultKsat";
            // 
            // cbKsat
            // 
            resources.ApplyResources(this.cbKsat, "cbKsat");
            this.cbKsat.Name = "cbKsat";
            this.cbKsat.UseVisualStyleBackColor = true;
            this.cbKsat.CheckedChanged += new System.EventHandler(this.cbKsat_CheckedChanged);
            // 
            // tbKsat
            // 
            resources.ApplyResources(this.tbKsat, "tbKsat");
            this.tbKsat.Name = "tbKsat";
            this.toolTip1.SetToolTip(this.tbKsat, resources.GetString("tbKsat.ToolTip"));
            this.tbKsat.TextChanged += new System.EventHandler(this.InputControlChanged);
            this.tbKsat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbKsat_KeyPress);
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Cyan;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Lime;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Yellow;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel7, "panel7");
            this.panel7.Name = "panel7";
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Fuchsia;
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel8, "panel8");
            this.panel8.Name = "panel8";
            // 
            // tpTopography
            // 
            this.tpTopography.BackColor = System.Drawing.Color.AliceBlue;
            this.tpTopography.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpTopography.Controls.Add(this.slopeHelpLabel);
            this.tpTopography.Controls.Add(this.slopePanel);
            resources.ApplyResources(this.tpTopography, "tpTopography");
            this.tpTopography.Name = "tpTopography";
            // 
            // slopeHelpLabel
            // 
            resources.ApplyResources(this.slopeHelpLabel, "slopeHelpLabel");
            this.slopeHelpLabel.Name = "slopeHelpLabel";
            this.slopeHelpLabel.TabStop = true;
            this.slopeHelpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked_1);
            // 
            // slopePanel
            // 
            this.slopePanel.Controls.Add(this.label10);
            this.slopePanel.Controls.Add(this.label8);
            this.slopePanel.Controls.Add(this.panel12);
            this.slopePanel.Controls.Add(this.rbSteepSlope);
            this.slopePanel.Controls.Add(this.cbSlope);
            this.slopePanel.Controls.Add(this.rbModSteepSlope);
            this.slopePanel.Controls.Add(this.rbModFlatSlope);
            this.slopePanel.Controls.Add(this.rbFlatSlope);
            this.slopePanel.Controls.Add(this.panel9);
            this.slopePanel.Controls.Add(this.panel10);
            this.slopePanel.Controls.Add(this.panel11);
            resources.ApplyResources(this.slopePanel, "slopePanel");
            this.slopePanel.Name = "slopePanel";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.Fuchsia;
            this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel12, "panel12");
            this.panel12.Name = "panel12";
            // 
            // rbSteepSlope
            // 
            resources.ApplyResources(this.rbSteepSlope, "rbSteepSlope");
            this.rbSteepSlope.Name = "rbSteepSlope";
            this.rbSteepSlope.TabStop = true;
            this.rbSteepSlope.UseVisualStyleBackColor = true;
            this.rbSteepSlope.CheckedChanged += new System.EventHandler(this.InputControlChanged);
            // 
            // cbSlope
            // 
            resources.ApplyResources(this.cbSlope, "cbSlope");
            this.cbSlope.Name = "cbSlope";
            this.cbSlope.UseVisualStyleBackColor = true;
            this.cbSlope.CheckedChanged += new System.EventHandler(this.cbSlope_CheckedChanged);
            // 
            // rbModSteepSlope
            // 
            resources.ApplyResources(this.rbModSteepSlope, "rbModSteepSlope");
            this.rbModSteepSlope.Name = "rbModSteepSlope";
            this.rbModSteepSlope.TabStop = true;
            this.rbModSteepSlope.UseVisualStyleBackColor = true;
            this.rbModSteepSlope.CheckedChanged += new System.EventHandler(this.InputControlChanged);
            // 
            // rbModFlatSlope
            // 
            resources.ApplyResources(this.rbModFlatSlope, "rbModFlatSlope");
            this.rbModFlatSlope.Name = "rbModFlatSlope";
            this.rbModFlatSlope.TabStop = true;
            this.rbModFlatSlope.UseVisualStyleBackColor = true;
            this.rbModFlatSlope.CheckedChanged += new System.EventHandler(this.InputControlChanged);
            // 
            // rbFlatSlope
            // 
            resources.ApplyResources(this.rbFlatSlope, "rbFlatSlope");
            this.rbFlatSlope.Checked = true;
            this.rbFlatSlope.Name = "rbFlatSlope";
            this.rbFlatSlope.TabStop = true;
            this.rbFlatSlope.UseVisualStyleBackColor = true;
            this.rbFlatSlope.CheckedChanged += new System.EventHandler(this.InputControlChanged);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Cyan;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel9, "panel9");
            this.panel9.Name = "panel9";
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Lime;
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel10, "panel10");
            this.panel10.Name = "panel10";
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.Yellow;
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.panel11, "panel11");
            this.panel11.Name = "panel11";
            // 
            // tpRainData
            // 
            this.tpRainData.BackColor = System.Drawing.Color.AliceBlue;
            this.tpRainData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpRainData.Controls.Add(this.rainHelpLabel);
            this.tpRainData.Controls.Add(this.rainfallPanel);
            this.tpRainData.Controls.Add(this.label14);
            resources.ApplyResources(this.tpRainData, "tpRainData");
            this.tpRainData.Name = "tpRainData";
            // 
            // rainHelpLabel
            // 
            resources.ApplyResources(this.rainHelpLabel, "rainHelpLabel");
            this.rainHelpLabel.Name = "rainHelpLabel";
            this.rainHelpLabel.TabStop = true;
            this.rainHelpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked_1);
            // 
            // rainfallPanel
            // 
            this.rainfallPanel.Controls.Add(this.btnSaveRainfall);
            this.rainfallPanel.Controls.Add(this.lbRainSource);
            this.rainfallPanel.Controls.Add(this.label5);
            resources.ApplyResources(this.rainfallPanel, "rainfallPanel");
            this.rainfallPanel.Name = "rainfallPanel";
            // 
            // btnSaveRainfall
            // 
            resources.ApplyResources(this.btnSaveRainfall, "btnSaveRainfall");
            this.btnSaveRainfall.Name = "btnSaveRainfall";
            this.btnSaveRainfall.TabStop = true;
            this.btnSaveRainfall.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnSaveRainfall_LinkClicked);
            // 
            // lbRainSource
            // 
            this.lbRainSource.BackColor = System.Drawing.Color.AliceBlue;
            this.lbRainSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lbRainSource, "lbRainSource");
            this.lbRainSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbRainSource.FormattingEnabled = true;
            this.lbRainSource.Name = "lbRainSource";
            this.lbRainSource.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbSource_DrawItem);
            this.lbRainSource.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbSource_MeasureItem);
            this.lbRainSource.SelectedIndexChanged += new System.EventHandler(this.lbRainSource_SelectedIndexChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // tpEvapData
            // 
            this.tpEvapData.BackColor = System.Drawing.Color.AliceBlue;
            this.tpEvapData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpEvapData.Controls.Add(this.evapHelpLabel);
            this.tpEvapData.Controls.Add(this.evapPanel);
            resources.ApplyResources(this.tpEvapData, "tpEvapData");
            this.tpEvapData.Name = "tpEvapData";
            // 
            // evapHelpLabel
            // 
            resources.ApplyResources(this.evapHelpLabel, "evapHelpLabel");
            this.evapHelpLabel.Name = "evapHelpLabel";
            this.evapHelpLabel.TabStop = true;
            this.evapHelpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked_1);
            // 
            // evapPanel
            // 
            this.evapPanel.Controls.Add(this.btnSaveEvap);
            this.evapPanel.Controls.Add(this.lbEvapSource);
            this.evapPanel.Controls.Add(this.label13);
            resources.ApplyResources(this.evapPanel, "evapPanel");
            this.evapPanel.Name = "evapPanel";
            // 
            // btnSaveEvap
            // 
            resources.ApplyResources(this.btnSaveEvap, "btnSaveEvap");
            this.btnSaveEvap.Name = "btnSaveEvap";
            this.btnSaveEvap.TabStop = true;
            this.btnSaveEvap.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnSaveEvap_LinkClicked);
            // 
            // lbEvapSource
            // 
            this.lbEvapSource.BackColor = System.Drawing.Color.AliceBlue;
            this.lbEvapSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lbEvapSource, "lbEvapSource");
            this.lbEvapSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbEvapSource.FormattingEnabled = true;
            this.lbEvapSource.Name = "lbEvapSource";
            this.lbEvapSource.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbSource_DrawItem);
            this.lbEvapSource.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbSource_MeasureItem);
            this.lbEvapSource.SelectedIndexChanged += new System.EventHandler(this.lbEvapSource_SelectedIndexChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // tpClimateLeft
            // 
            this.tpClimateLeft.BackColor = System.Drawing.Color.AliceBlue;
            this.tpClimateLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpClimateLeft.Controls.Add(this.panel13);
            this.tpClimateLeft.Controls.Add(this.label12);
            this.tpClimateLeft.Controls.Add(this.climateHelpLabel);
            this.tpClimateLeft.Controls.Add(this.rbClimate4);
            this.tpClimateLeft.Controls.Add(this.rbClimate3);
            this.tpClimateLeft.Controls.Add(this.rbClimate2);
            this.tpClimateLeft.Controls.Add(this.rbClimate1);
            this.tpClimateLeft.Controls.Add(this.label30);
            resources.ApplyResources(this.tpClimateLeft, "tpClimateLeft");
            this.tpClimateLeft.Name = "tpClimateLeft";
            // 
            // panel13
            // 
            resources.ApplyResources(this.panel13, "panel13");
            this.panel13.Controls.Add(this.rbClimate2060);
            this.panel13.Controls.Add(this.rbClimate2035);
            this.panel13.Name = "panel13";
            // 
            // rbClimate2060
            // 
            resources.ApplyResources(this.rbClimate2060, "rbClimate2060");
            this.rbClimate2060.Name = "rbClimate2060";
            this.rbClimate2060.UseVisualStyleBackColor = true;
            this.rbClimate2060.CheckedChanged += new System.EventHandler(this.rbClimateYear_CheckedChanged);
            // 
            // rbClimate2035
            // 
            resources.ApplyResources(this.rbClimate2035, "rbClimate2035");
            this.rbClimate2035.Checked = true;
            this.rbClimate2035.Name = "rbClimate2035";
            this.rbClimate2035.TabStop = true;
            this.rbClimate2035.UseVisualStyleBackColor = true;
            this.rbClimate2035.CheckedChanged += new System.EventHandler(this.rbClimateYear_CheckedChanged);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // climateHelpLabel
            // 
            resources.ApplyResources(this.climateHelpLabel, "climateHelpLabel");
            this.climateHelpLabel.Name = "climateHelpLabel";
            this.climateHelpLabel.TabStop = true;
            this.climateHelpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked_1);
            // 
            // rbClimate4
            // 
            resources.ApplyResources(this.rbClimate4, "rbClimate4");
            this.rbClimate4.Name = "rbClimate4";
            this.rbClimate4.UseVisualStyleBackColor = true;
            this.rbClimate4.CheckedChanged += new System.EventHandler(this.InputControlChanged);
            // 
            // rbClimate3
            // 
            resources.ApplyResources(this.rbClimate3, "rbClimate3");
            this.rbClimate3.Name = "rbClimate3";
            this.rbClimate3.UseVisualStyleBackColor = true;
            this.rbClimate3.CheckedChanged += new System.EventHandler(this.InputControlChanged);
            // 
            // rbClimate2
            // 
            resources.ApplyResources(this.rbClimate2, "rbClimate2");
            this.rbClimate2.Name = "rbClimate2";
            this.rbClimate2.UseVisualStyleBackColor = true;
            this.rbClimate2.CheckedChanged += new System.EventHandler(this.InputControlChanged);
            // 
            // rbClimate1
            // 
            resources.ApplyResources(this.rbClimate1, "rbClimate1");
            this.rbClimate1.Checked = true;
            this.rbClimate1.Name = "rbClimate1";
            this.rbClimate1.TabStop = true;
            this.rbClimate1.UseVisualStyleBackColor = true;
            this.rbClimate1.CheckedChanged += new System.EventHandler(this.InputControlChanged);
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // tpLandCover
            // 
            this.tpLandCover.BackColor = System.Drawing.Color.AliceBlue;
            this.tpLandCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpLandCover.Controls.Add(this.landCoverHelpLabel);
            this.tpLandCover.Controls.Add(this.landCoverPanel);
            resources.ApplyResources(this.tpLandCover, "tpLandCover");
            this.tpLandCover.Name = "tpLandCover";
            // 
            // landCoverHelpLabel
            // 
            resources.ApplyResources(this.landCoverHelpLabel, "landCoverHelpLabel");
            this.landCoverHelpLabel.Name = "landCoverHelpLabel";
            this.landCoverHelpLabel.TabStop = true;
            this.landCoverHelpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked_1);
            // 
            // landCoverPanel
            // 
            this.landCoverPanel.Controls.Add(this.label20);
            this.landCoverPanel.Controls.Add(this.tlpLandCover);
            resources.ApplyResources(this.landCoverPanel, "landCoverPanel");
            this.landCoverPanel.Name = "landCoverPanel";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // tlpLandCover
            // 
            resources.ApplyResources(this.tlpLandCover, "tlpLandCover");
            this.tlpLandCover.Controls.Add(this.lblPcntImperv, 0, 5);
            this.tlpLandCover.Controls.Add(this.tbImperv, 1, 5);
            this.tlpLandCover.Controls.Add(this.nudDesert, 1, 4);
            this.tlpLandCover.Controls.Add(this.nudLawn, 1, 3);
            this.tlpLandCover.Controls.Add(this.nudMeadow, 1, 2);
            this.tlpLandCover.Controls.Add(this.nudForest, 1, 1);
            this.tlpLandCover.Controls.Add(this.lblPcntDesert, 0, 4);
            this.tlpLandCover.Controls.Add(this.lblPcntLawn, 0, 3);
            this.tlpLandCover.Controls.Add(this.lblPcntMeadow, 0, 2);
            this.tlpLandCover.Controls.Add(this.lblPcntForest, 0, 1);
            this.tlpLandCover.Controls.Add(this.label22, 0, 0);
            this.tlpLandCover.Name = "tlpLandCover";
            // 
            // tbImperv
            // 
            this.tbImperv.BackColor = System.Drawing.Color.AliceBlue;
            resources.ApplyResources(this.tbImperv, "tbImperv");
            this.tbImperv.Name = "tbImperv";
            this.tbImperv.ReadOnly = true;
            this.tbImperv.TabStop = false;
            // 
            // nudDesert
            // 
            resources.ApplyResources(this.nudDesert, "nudDesert");
            this.nudDesert.Name = "nudDesert";
            this.nudDesert.ValueChanged += new System.EventHandler(this.nudLandCover_ValueChanged);
            this.nudDesert.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // nudLawn
            // 
            resources.ApplyResources(this.nudLawn, "nudLawn");
            this.nudLawn.Name = "nudLawn";
            this.nudLawn.ValueChanged += new System.EventHandler(this.nudLandCover_ValueChanged);
            this.nudLawn.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // nudMeadow
            // 
            resources.ApplyResources(this.nudMeadow, "nudMeadow");
            this.nudMeadow.Name = "nudMeadow";
            this.nudMeadow.ValueChanged += new System.EventHandler(this.nudLandCover_ValueChanged);
            this.nudMeadow.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // nudForest
            // 
            resources.ApplyResources(this.nudForest, "nudForest");
            this.nudForest.Name = "nudForest";
            this.nudForest.ValueChanged += new System.EventHandler(this.nudLandCover_ValueChanged);
            this.nudForest.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.tlpLandCover.SetColumnSpan(this.label22, 2);
            this.label22.Name = "label22";
            // 
            // tpLidsLeft
            // 
            this.tpLidsLeft.BackColor = System.Drawing.Color.AliceBlue;
            this.tpLidsLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpLidsLeft.Controls.Add(this.groupBox5);
            this.tpLidsLeft.Controls.Add(this.groupBox4);
            this.tpLidsLeft.Controls.Add(this.groupBox6);
            this.tpLidsLeft.Controls.Add(this.label23);
            this.tpLidsLeft.Controls.Add(this.lidHelpLabel);
            this.tpLidsLeft.Controls.Add(this.lidControlsPanel);
            resources.ApplyResources(this.tpLidsLeft, "tpLidsLeft");
            this.tpLidsLeft.Name = "tpLidsLeft";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.llSiteSuitExcellent);
            this.groupBox5.Controls.Add(this.llSiteSuitModerate);
            this.groupBox5.Controls.Add(this.llSiteSuitPoor);
            this.groupBox5.Controls.Add(this.rbCostSiteExcellent);
            this.groupBox5.Controls.Add(this.rbCostSitePoor);
            this.groupBox5.Controls.Add(this.rbCostSiteModerate);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // llSiteSuitExcellent
            // 
            resources.ApplyResources(this.llSiteSuitExcellent, "llSiteSuitExcellent");
            this.llSiteSuitExcellent.Name = "llSiteSuitExcellent";
            this.llSiteSuitExcellent.TabStop = true;
            this.llSiteSuitExcellent.Tag = "4";
            this.llSiteSuitExcellent.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCostModuleHelp_LinkClicked);
            // 
            // llSiteSuitModerate
            // 
            resources.ApplyResources(this.llSiteSuitModerate, "llSiteSuitModerate");
            this.llSiteSuitModerate.Name = "llSiteSuitModerate";
            this.llSiteSuitModerate.TabStop = true;
            this.llSiteSuitModerate.Tag = "3";
            this.llSiteSuitModerate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCostModuleHelp_LinkClicked);
            // 
            // llSiteSuitPoor
            // 
            resources.ApplyResources(this.llSiteSuitPoor, "llSiteSuitPoor");
            this.llSiteSuitPoor.Name = "llSiteSuitPoor";
            this.llSiteSuitPoor.TabStop = true;
            this.llSiteSuitPoor.Tag = "2";
            this.llSiteSuitPoor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCostModuleHelp_LinkClicked);
            // 
            // rbCostSiteExcellent
            // 
            resources.ApplyResources(this.rbCostSiteExcellent, "rbCostSiteExcellent");
            this.rbCostSiteExcellent.ForeColor = System.Drawing.Color.Black;
            this.rbCostSiteExcellent.Name = "rbCostSiteExcellent";
            this.rbCostSiteExcellent.Tag = "0";
            this.rbCostSiteExcellent.UseVisualStyleBackColor = true;
            // 
            // rbCostSitePoor
            // 
            resources.ApplyResources(this.rbCostSitePoor, "rbCostSitePoor");
            this.rbCostSitePoor.Checked = true;
            this.rbCostSitePoor.ForeColor = System.Drawing.Color.Black;
            this.rbCostSitePoor.Name = "rbCostSitePoor";
            this.rbCostSitePoor.TabStop = true;
            this.rbCostSitePoor.Tag = "0";
            this.rbCostSitePoor.UseVisualStyleBackColor = true;
            // 
            // rbCostSiteModerate
            // 
            resources.ApplyResources(this.rbCostSiteModerate, "rbCostSiteModerate");
            this.rbCostSiteModerate.ForeColor = System.Drawing.Color.Black;
            this.rbCostSiteModerate.Name = "rbCostSiteModerate";
            this.rbCostSiteModerate.Tag = "0";
            this.rbCostSiteModerate.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.llNewDev);
            this.groupBox4.Controls.Add(this.llRedev);
            this.groupBox4.Controls.Add(this.rbCostRedev);
            this.groupBox4.Controls.Add(this.rbCostNewDev);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // llNewDev
            // 
            resources.ApplyResources(this.llNewDev, "llNewDev");
            this.llNewDev.Name = "llNewDev";
            this.llNewDev.TabStop = true;
            this.llNewDev.Tag = "1";
            this.llNewDev.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCostModuleHelp_LinkClicked);
            // 
            // llRedev
            // 
            resources.ApplyResources(this.llRedev, "llRedev");
            this.llRedev.Name = "llRedev";
            this.llRedev.TabStop = true;
            this.llRedev.Tag = "0";
            this.llRedev.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCostModuleHelp_LinkClicked);
            // 
            // rbCostRedev
            // 
            resources.ApplyResources(this.rbCostRedev, "rbCostRedev");
            this.rbCostRedev.Checked = true;
            this.rbCostRedev.ForeColor = System.Drawing.Color.Black;
            this.rbCostRedev.Name = "rbCostRedev";
            this.rbCostRedev.TabStop = true;
            this.rbCostRedev.Tag = "0";
            this.rbCostRedev.UseVisualStyleBackColor = true;
            // 
            // rbCostNewDev
            // 
            resources.ApplyResources(this.rbCostNewDev, "rbCostNewDev");
            this.rbCostNewDev.ForeColor = System.Drawing.Color.Black;
            this.rbCostNewDev.Name = "rbCostNewDev";
            this.rbCostNewDev.Tag = "0";
            this.rbCostNewDev.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Controls.Add(this.tbRegMultiplier);
            this.groupBox6.Controls.Add(this.llCostRegion);
            this.groupBox6.Controls.Add(this.cmbCostRegion);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // tbRegMultiplier
            // 
            resources.ApplyResources(this.tbRegMultiplier, "tbRegMultiplier");
            this.tbRegMultiplier.Name = "tbRegMultiplier";
            // 
            // llCostRegion
            // 
            resources.ApplyResources(this.llCostRegion, "llCostRegion");
            this.llCostRegion.Name = "llCostRegion";
            this.llCostRegion.TabStop = true;
            this.llCostRegion.Tag = "5";
            this.llCostRegion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCostModuleHelp_LinkClicked);
            // 
            // cmbCostRegion
            // 
            this.cmbCostRegion.FormattingEnabled = true;
            this.cmbCostRegion.Items.AddRange(new object[] {
            resources.GetString("cmbCostRegion.Items")});
            resources.ApplyResources(this.cmbCostRegion, "cmbCostRegion");
            this.cmbCostRegion.Name = "cmbCostRegion";
            this.cmbCostRegion.SelectedIndexChanged += new System.EventHandler(this.cmbCostRegion_SelectedIndexChanged);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // lidHelpLabel
            // 
            resources.ApplyResources(this.lidHelpLabel, "lidHelpLabel");
            this.lidHelpLabel.Name = "lidHelpLabel";
            this.lidHelpLabel.TabStop = true;
            this.lidHelpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked_1);
            // 
            // lidControlsPanel
            // 
            this.lidControlsPanel.Controls.Add(this.label1);
            this.lidControlsPanel.Controls.Add(this.tlpLidUsage);
            resources.ApplyResources(this.lidControlsPanel, "lidControlsPanel");
            this.lidControlsPanel.Name = "lidControlsPanel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tlpLidUsage
            // 
            resources.ApplyResources(this.tlpLidUsage, "tlpLidUsage");
            this.tlpLidUsage.Controls.Add(this.nudImpDiscon, 1, 1);
            this.tlpLidUsage.Controls.Add(this.nudRainHarvest, 1, 2);
            this.tlpLidUsage.Controls.Add(this.nudRainGarden, 1, 3);
            this.tlpLidUsage.Controls.Add(this.nudGreenRoof, 1, 4);
            this.tlpLidUsage.Controls.Add(this.nudStreetPlanter, 1, 5);
            this.tlpLidUsage.Controls.Add(this.nudInfilBasin, 1, 6);
            this.tlpLidUsage.Controls.Add(this.nudPorousPave, 1, 7);
            this.tlpLidUsage.Controls.Add(this.label32, 0, 0);
            this.tlpLidUsage.Controls.Add(this.lidLabel1, 0, 1);
            this.tlpLidUsage.Controls.Add(this.lidLabel2, 0, 2);
            this.tlpLidUsage.Controls.Add(this.lidLabel3, 0, 3);
            this.tlpLidUsage.Controls.Add(this.lidLabel4, 0, 4);
            this.tlpLidUsage.Controls.Add(this.lidLabel5, 0, 5);
            this.tlpLidUsage.Controls.Add(this.lidLabel6, 0, 6);
            this.tlpLidUsage.Controls.Add(this.lidLabel7, 0, 7);
            this.tlpLidUsage.Controls.Add(this.label7, 0, 8);
            this.tlpLidUsage.Controls.Add(this.nudDesignStorm, 1, 8);
            this.tlpLidUsage.Name = "tlpLidUsage";
            // 
            // nudImpDiscon
            // 
            resources.ApplyResources(this.nudImpDiscon, "nudImpDiscon");
            this.nudImpDiscon.Name = "nudImpDiscon";
            this.nudImpDiscon.ValueChanged += new System.EventHandler(this.InputControlChanged);
            this.nudImpDiscon.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // nudRainHarvest
            // 
            resources.ApplyResources(this.nudRainHarvest, "nudRainHarvest");
            this.nudRainHarvest.Name = "nudRainHarvest";
            this.nudRainHarvest.ValueChanged += new System.EventHandler(this.InputControlChanged);
            this.nudRainHarvest.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // nudRainGarden
            // 
            resources.ApplyResources(this.nudRainGarden, "nudRainGarden");
            this.nudRainGarden.Name = "nudRainGarden";
            this.nudRainGarden.ValueChanged += new System.EventHandler(this.InputControlChanged);
            this.nudRainGarden.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // nudGreenRoof
            // 
            resources.ApplyResources(this.nudGreenRoof, "nudGreenRoof");
            this.nudGreenRoof.Name = "nudGreenRoof";
            this.nudGreenRoof.ValueChanged += new System.EventHandler(this.InputControlChanged);
            this.nudGreenRoof.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // nudStreetPlanter
            // 
            resources.ApplyResources(this.nudStreetPlanter, "nudStreetPlanter");
            this.nudStreetPlanter.Name = "nudStreetPlanter";
            this.nudStreetPlanter.ValueChanged += new System.EventHandler(this.InputControlChanged);
            this.nudStreetPlanter.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // nudInfilBasin
            // 
            resources.ApplyResources(this.nudInfilBasin, "nudInfilBasin");
            this.nudInfilBasin.Name = "nudInfilBasin";
            this.nudInfilBasin.ValueChanged += new System.EventHandler(this.InputControlChanged);
            this.nudInfilBasin.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // nudPorousPave
            // 
            resources.ApplyResources(this.nudPorousPave, "nudPorousPave");
            this.nudPorousPave.Name = "nudPorousPave";
            this.nudPorousPave.ValueChanged += new System.EventHandler(this.InputControlChanged);
            this.nudPorousPave.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.tlpLidUsage.SetColumnSpan(this.label32, 2);
            this.label32.Name = "label32";
            this.tlpLidUsage.SetRowSpan(this.label32, 2);
            // 
            // lidLabel1
            // 
            resources.ApplyResources(this.lidLabel1, "lidLabel1");
            this.lidLabel1.Name = "lidLabel1";
            this.lidLabel1.TabStop = true;
            this.lidLabel1.Tag = "0";
            this.lidLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LidLabel1_LinkClicked);
            // 
            // lidLabel2
            // 
            resources.ApplyResources(this.lidLabel2, "lidLabel2");
            this.lidLabel2.Name = "lidLabel2";
            this.lidLabel2.TabStop = true;
            this.lidLabel2.Tag = "1";
            this.lidLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LidLabel1_LinkClicked);
            // 
            // lidLabel3
            // 
            resources.ApplyResources(this.lidLabel3, "lidLabel3");
            this.lidLabel3.Name = "lidLabel3";
            this.lidLabel3.TabStop = true;
            this.lidLabel3.Tag = "2";
            this.lidLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LidLabel1_LinkClicked);
            // 
            // lidLabel4
            // 
            resources.ApplyResources(this.lidLabel4, "lidLabel4");
            this.lidLabel4.Name = "lidLabel4";
            this.lidLabel4.TabStop = true;
            this.lidLabel4.Tag = "3";
            this.lidLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LidLabel1_LinkClicked);
            // 
            // lidLabel5
            // 
            resources.ApplyResources(this.lidLabel5, "lidLabel5");
            this.lidLabel5.Name = "lidLabel5";
            this.lidLabel5.TabStop = true;
            this.lidLabel5.Tag = "4";
            this.lidLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LidLabel1_LinkClicked);
            // 
            // lidLabel6
            // 
            resources.ApplyResources(this.lidLabel6, "lidLabel6");
            this.lidLabel6.Name = "lidLabel6";
            this.lidLabel6.TabStop = true;
            this.lidLabel6.Tag = "5";
            this.lidLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LidLabel1_LinkClicked);
            // 
            // lidLabel7
            // 
            resources.ApplyResources(this.lidLabel7, "lidLabel7");
            this.lidLabel7.Name = "lidLabel7";
            this.lidLabel7.TabStop = true;
            this.lidLabel7.Tag = "6";
            this.lidLabel7.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LidLabel1_LinkClicked);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // nudDesignStorm
            // 
            resources.ApplyResources(this.nudDesignStorm, "nudDesignStorm");
            this.nudDesignStorm.DecimalPlaces = 2;
            this.nudDesignStorm.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudDesignStorm.Name = "nudDesignStorm";
            this.nudDesignStorm.ValueChanged += new System.EventHandler(this.InputControlChanged);
            this.nudDesignStorm.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // tpResultsLeft
            // 
            this.tpResultsLeft.BackColor = System.Drawing.Color.AliceBlue;
            this.tpResultsLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpResultsLeft.Controls.Add(this.panel19);
            resources.ApplyResources(this.tpResultsLeft, "tpResultsLeft");
            this.tpResultsLeft.Name = "tpResultsLeft";
            // 
            // panel19
            // 
            this.panel19.Controls.Add(this.resultsLinkLabel);
            this.panel19.Controls.Add(this.resultsPanel);
            resources.ApplyResources(this.panel19, "panel19");
            this.panel19.Name = "panel19";
            // 
            // resultsLinkLabel
            // 
            resources.ApplyResources(this.resultsLinkLabel, "resultsLinkLabel");
            this.resultsLinkLabel.Name = "resultsLinkLabel";
            this.resultsLinkLabel.TabStop = true;
            this.resultsLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked_1);
            // 
            // resultsPanel
            // 
            this.resultsPanel.BackColor = System.Drawing.Color.AliceBlue;
            this.resultsPanel.Controls.Add(this.groupBox3);
            this.resultsPanel.Controls.Add(this.groupBox1);
            this.resultsPanel.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.resultsPanel, "resultsPanel");
            this.resultsPanel.Name = "resultsPanel";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbCostCapSmry);
            this.groupBox3.Controls.Add(this.rbRainRunoffEvents);
            this.groupBox3.Controls.Add(this.rbXeventRainRunoff);
            this.groupBox3.Controls.Add(this.rbRunoffRainPcnt);
            this.groupBox3.Controls.Add(this.rbRainRetentionFreq);
            this.groupBox3.Controls.Add(this.rbRainRunoffFreq);
            this.groupBox3.Controls.Add(this.rbSummaryResults);
            this.groupBox3.Controls.Add(this.rbSiteDescription);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // rbCostCapSmry
            // 
            resources.ApplyResources(this.rbCostCapSmry, "rbCostCapSmry");
            this.rbCostCapSmry.Name = "rbCostCapSmry";
            this.rbCostCapSmry.TabStop = true;
            this.rbCostCapSmry.UseVisualStyleBackColor = true;
            this.rbCostCapSmry.Click += new System.EventHandler(this.ResultsBtn_Clicked);
            // 
            // rbRainRunoffEvents
            // 
            resources.ApplyResources(this.rbRainRunoffEvents, "rbRainRunoffEvents");
            this.rbRainRunoffEvents.Name = "rbRainRunoffEvents";
            this.rbRainRunoffEvents.TabStop = true;
            this.rbRainRunoffEvents.UseVisualStyleBackColor = true;
            this.rbRainRunoffEvents.Click += new System.EventHandler(this.ResultsBtn_Clicked);
            // 
            // rbXeventRainRunoff
            // 
            resources.ApplyResources(this.rbXeventRainRunoff, "rbXeventRainRunoff");
            this.rbXeventRainRunoff.Name = "rbXeventRainRunoff";
            this.rbXeventRainRunoff.TabStop = true;
            this.rbXeventRainRunoff.UseVisualStyleBackColor = true;
            this.rbXeventRainRunoff.Click += new System.EventHandler(this.ResultsBtn_Clicked);
            // 
            // rbRunoffRainPcnt
            // 
            resources.ApplyResources(this.rbRunoffRainPcnt, "rbRunoffRainPcnt");
            this.rbRunoffRainPcnt.Name = "rbRunoffRainPcnt";
            this.rbRunoffRainPcnt.TabStop = true;
            this.rbRunoffRainPcnt.UseVisualStyleBackColor = true;
            this.rbRunoffRainPcnt.Click += new System.EventHandler(this.ResultsBtn_Clicked);
            // 
            // rbRainRetentionFreq
            // 
            resources.ApplyResources(this.rbRainRetentionFreq, "rbRainRetentionFreq");
            this.rbRainRetentionFreq.Name = "rbRainRetentionFreq";
            this.rbRainRetentionFreq.TabStop = true;
            this.rbRainRetentionFreq.UseVisualStyleBackColor = true;
            this.rbRainRetentionFreq.Click += new System.EventHandler(this.ResultsBtn_Clicked);
            // 
            // rbRainRunoffFreq
            // 
            resources.ApplyResources(this.rbRainRunoffFreq, "rbRainRunoffFreq");
            this.rbRainRunoffFreq.Name = "rbRainRunoffFreq";
            this.rbRainRunoffFreq.TabStop = true;
            this.rbRainRunoffFreq.UseVisualStyleBackColor = true;
            this.rbRainRunoffFreq.Click += new System.EventHandler(this.ResultsBtn_Clicked);
            // 
            // rbSummaryResults
            // 
            resources.ApplyResources(this.rbSummaryResults, "rbSummaryResults");
            this.rbSummaryResults.Name = "rbSummaryResults";
            this.rbSummaryResults.TabStop = true;
            this.rbSummaryResults.UseVisualStyleBackColor = true;
            this.rbSummaryResults.Click += new System.EventHandler(this.ResultsBtn_Clicked);
            // 
            // rbSiteDescription
            // 
            resources.ApplyResources(this.rbSiteDescription, "rbSiteDescription");
            this.rbSiteDescription.Name = "rbSiteDescription";
            this.rbSiteDescription.TabStop = true;
            this.rbSiteDescription.UseVisualStyleBackColor = true;
            this.rbSiteDescription.Click += new System.EventHandler(this.ResultsBtn_Clicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mnuRefreshResults);
            this.groupBox1.Controls.Add(this.mnuPrintResults);
            this.groupBox1.Controls.Add(this.mnuRemoveBaseline);
            this.groupBox1.Controls.Add(this.mnuAddBaseline);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // mnuRefreshResults
            // 
            resources.ApplyResources(this.mnuRefreshResults, "mnuRefreshResults");
            this.mnuRefreshResults.Name = "mnuRefreshResults";
            this.mnuRefreshResults.TabStop = true;
            this.mnuRefreshResults.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mnuRefreshResults_LinkClicked);
            // 
            // mnuPrintResults
            // 
            resources.ApplyResources(this.mnuPrintResults, "mnuPrintResults");
            this.mnuPrintResults.Name = "mnuPrintResults";
            this.mnuPrintResults.TabStop = true;
            this.toolTip1.SetToolTip(this.mnuPrintResults, resources.GetString("mnuPrintResults.ToolTip"));
            this.mnuPrintResults.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mnuPrintResults_LinkClicked);
            // 
            // mnuRemoveBaseline
            // 
            resources.ApplyResources(this.mnuRemoveBaseline, "mnuRemoveBaseline");
            this.mnuRemoveBaseline.Name = "mnuRemoveBaseline";
            this.mnuRemoveBaseline.TabStop = true;
            this.toolTip1.SetToolTip(this.mnuRemoveBaseline, resources.GetString("mnuRemoveBaseline.ToolTip"));
            this.mnuRemoveBaseline.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mnuRemoveBaseline_LinkClicked);
            // 
            // mnuAddBaseline
            // 
            resources.ApplyResources(this.mnuAddBaseline, "mnuAddBaseline");
            this.mnuAddBaseline.Name = "mnuAddBaseline";
            this.mnuAddBaseline.TabStop = true;
            this.toolTip1.SetToolTip(this.mnuAddBaseline, resources.GetString("mnuAddBaseline.ToolTip"));
            this.mnuAddBaseline.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mnuAddBaseline_LinkClicked);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbIgnoreConsecDays);
            this.groupBox2.Controls.Add(this.lblThreshold);
            this.groupBox2.Controls.Add(this.nudEventThreshold);
            this.groupBox2.Controls.Add(this.nudYearsAnalyzed);
            this.groupBox2.Controls.Add(this.lblDuration);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // nudEventThreshold
            // 
            this.nudEventThreshold.DecimalPlaces = 2;
            resources.ApplyResources(this.nudEventThreshold, "nudEventThreshold");
            this.nudEventThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudEventThreshold.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.nudEventThreshold.Name = "nudEventThreshold";
            this.nudEventThreshold.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudEventThreshold.ValueChanged += new System.EventHandler(this.AnalysisOptionsChanged);
            this.nudEventThreshold.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // nudYearsAnalyzed
            // 
            resources.ApplyResources(this.nudYearsAnalyzed, "nudYearsAnalyzed");
            this.nudYearsAnalyzed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudYearsAnalyzed.Name = "nudYearsAnalyzed";
            this.nudYearsAnalyzed.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudYearsAnalyzed.ValueChanged += new System.EventHandler(this.AnalysisOptionsChanged);
            this.nudYearsAnalyzed.Enter += new System.EventHandler(this.NumericUpDown_Enter);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "no_sound.png");
            this.imageList1.Images.SetKeyName(1, "sound.png");
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 32000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.tcMainTop);
            this.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainForm_FormClosed);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mainForm_KeyPress);
            this.tcMainTop.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.tcMainRight.ResumeLayout(false);
            this.tpWebBrowser.ResumeLayout(false);
            this.tpClimateRight.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tpResultsRight.ResumeLayout(false);
            this.tcResults.ResumeLayout(false);
            this.tpSiteDescription.ResumeLayout(false);
            this.tpSummaryResults.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tpEventPlot.ResumeLayout(false);
            this.tpRunoffFreqPlot.ResumeLayout(false);
            this.tpRetentionFreqPlot.ResumeLayout(false);
            this.tpRunoffBySizePlot.ResumeLayout(false);
            this.tpRainRunoffPlot.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tcMainLeft.ResumeLayout(false);
            this.tpIntro.ResumeLayout(false);
            this.tpLocation.ResumeLayout(false);
            this.tpLocation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSiteArea)).EndInit();
            this.tpSoilType.ResumeLayout(false);
            this.tpSoilType.PerformLayout();
            this.soilGroupPanel.ResumeLayout(false);
            this.soilGroupPanel.PerformLayout();
            this.tpSoilDrainage.ResumeLayout(false);
            this.tpSoilDrainage.PerformLayout();
            this.conductivityPanel.ResumeLayout(false);
            this.conductivityPanel.PerformLayout();
            this.tpTopography.ResumeLayout(false);
            this.tpTopography.PerformLayout();
            this.slopePanel.ResumeLayout(false);
            this.slopePanel.PerformLayout();
            this.tpRainData.ResumeLayout(false);
            this.tpRainData.PerformLayout();
            this.rainfallPanel.ResumeLayout(false);
            this.rainfallPanel.PerformLayout();
            this.tpEvapData.ResumeLayout(false);
            this.tpEvapData.PerformLayout();
            this.evapPanel.ResumeLayout(false);
            this.evapPanel.PerformLayout();
            this.tpClimateLeft.ResumeLayout(false);
            this.tpClimateLeft.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.tpLandCover.ResumeLayout(false);
            this.tpLandCover.PerformLayout();
            this.landCoverPanel.ResumeLayout(false);
            this.landCoverPanel.PerformLayout();
            this.tlpLandCover.ResumeLayout(false);
            this.tlpLandCover.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDesert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLawn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeadow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudForest)).EndInit();
            this.tpLidsLeft.ResumeLayout(false);
            this.tpLidsLeft.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.lidControlsPanel.ResumeLayout(false);
            this.tlpLidUsage.ResumeLayout(false);
            this.tlpLidUsage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImpDiscon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRainHarvest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRainGarden)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGreenRoof)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStreetPlanter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInfilBasin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPorousPave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDesignStorm)).EndInit();
            this.tpResultsLeft.ResumeLayout(false);
            this.panel19.ResumeLayout(false);
            this.panel19.PerformLayout();
            this.resultsPanel.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEventThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYearsAnalyzed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabControl tcMainTop;
        private System.Windows.Forms.TabPage overviewTab;
        private System.Windows.Forms.TabPage locationTab;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.TabPage soilTab;
        private System.Windows.Forms.TabPage kSatTab;
        private System.Windows.Forms.TabPage slopeTab;
        private System.Windows.Forms.TabPage rainTab;
        private System.Windows.Forms.TabPage evapTab;
        private System.Windows.Forms.TabPage coverTab;
        private System.Windows.Forms.TabPage lidTab;
        private System.Windows.Forms.TabPage resultsTab;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TabControl tcMainRight;
        private System.Windows.Forms.TabPage tpWebBrowser;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TabPage tpResultsRight;
        private System.Windows.Forms.TabControl tcMainLeft;
        private System.Windows.Forms.TabPage tpLocation;
        private System.Windows.Forms.NumericUpDown nudSiteArea;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox tbSiteAddress;
        private System.Windows.Forms.TextBox tbSiteLocation;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tbSiteName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel btnOpen;
        private System.Windows.Forms.TabPage tpSoilDrainage;
        private System.Windows.Forms.TabPage tpTopography;
        private System.Windows.Forms.TabPage tpRainData;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabPage tpEvapData;
        private System.Windows.Forms.TabPage tpLandCover;
        private System.Windows.Forms.TabPage tpLidsLeft;
        private System.Windows.Forms.TabPage tpResultsLeft;
        private System.Windows.Forms.Panel panel19;
        private System.Windows.Forms.TabPage tpIntro;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel btnRestart;
        private System.Windows.Forms.ToolStripStatusLabel btnSave;
        private System.Windows.Forms.ToolStripStatusLabel btnExit;
        private System.Windows.Forms.TabPage tpSoilType;
        private System.Windows.Forms.Panel soilGroupPanel;
        private System.Windows.Forms.CheckBox cbSoilGroup;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbGroupD;
        private System.Windows.Forms.RadioButton rbGroupC;
        private System.Windows.Forms.RadioButton rbGroupB;
        private System.Windows.Forms.RadioButton rbGroupA;
        private System.Windows.Forms.Panel conductivityPanel;
        private System.Windows.Forms.Label lblDefaultKsat;
        private System.Windows.Forms.CheckBox cbKsat;
        private System.Windows.Forms.TextBox tbKsat;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel slopePanel;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.RadioButton rbSteepSlope;
        private System.Windows.Forms.CheckBox cbSlope;
        private System.Windows.Forms.RadioButton rbModSteepSlope;
        private System.Windows.Forms.RadioButton rbModFlatSlope;
        private System.Windows.Forms.RadioButton rbFlatSlope;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel rainfallPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel evapPanel;
        private System.Windows.Forms.ListBox lbEvapSource;
        private System.Windows.Forms.Panel landCoverPanel;
        private System.Windows.Forms.TableLayoutPanel tlpLandCover;
        private System.Windows.Forms.Label lblPcntImperv;
        private System.Windows.Forms.TextBox tbImperv;
        private System.Windows.Forms.NumericUpDown nudDesert;
        private System.Windows.Forms.NumericUpDown nudLawn;
        private System.Windows.Forms.NumericUpDown nudMeadow;
        private System.Windows.Forms.NumericUpDown nudForest;
        private System.Windows.Forms.Label lblPcntDesert;
        private System.Windows.Forms.Label lblPcntLawn;
        private System.Windows.Forms.Label lblPcntMeadow;
        private System.Windows.Forms.Label lblPcntForest;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel lidControlsPanel;
        private System.Windows.Forms.TableLayoutPanel tlpLidUsage;
        private System.Windows.Forms.NumericUpDown nudImpDiscon;
        private System.Windows.Forms.NumericUpDown nudRainHarvest;
        private System.Windows.Forms.NumericUpDown nudRainGarden;
        private System.Windows.Forms.NumericUpDown nudGreenRoof;
        private System.Windows.Forms.NumericUpDown nudStreetPlanter;
        private System.Windows.Forms.NumericUpDown nudInfilBasin;
        private System.Windows.Forms.NumericUpDown nudPorousPave;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.LinkLabel lidLabel1;
        private System.Windows.Forms.LinkLabel lidLabel2;
        private System.Windows.Forms.LinkLabel lidLabel3;
        private System.Windows.Forms.LinkLabel lidLabel4;
        private System.Windows.Forms.LinkLabel lidLabel5;
        private System.Windows.Forms.LinkLabel lidLabel6;
        private System.Windows.Forms.LinkLabel lidLabel7;
        private System.Windows.Forms.Panel resultsPanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbIgnoreConsecDays;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.NumericUpDown nudEventThreshold;
        private System.Windows.Forms.NumericUpDown nudYearsAnalyzed;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.ToolStripStatusLabel btnSound;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListBox lbRainSource;
        private System.Windows.Forms.LinkLabel soilHelpLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel ksatHelpLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel slopeHelpLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel rainHelpLabel;
        private System.Windows.Forms.LinkLabel evapHelpLabel;
        private System.Windows.Forms.LinkLabel landCoverHelpLabel;
        private System.Windows.Forms.LinkLabel lidHelpLabel;
        private System.Windows.Forms.LinkLabel resultsLinkLabel;
        private System.Windows.Forms.ToolStripStatusLabel spacerStatusLabel;
        private System.Windows.Forms.LinkLabel btnSaveRainfall;
        private System.Windows.Forms.LinkLabel btnSaveEvap;
        private System.Windows.Forms.Label lblRelease;
        private System.Windows.Forms.Label lblIntro;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TabPage climateTab;
        private System.Windows.Forms.TabPage tpClimateRight;
        private System.Windows.Forms.TabPage tpClimateLeft;
        private System.Windows.Forms.LinkLabel climateHelpLabel;
        private System.Windows.Forms.RadioButton rbClimate4;
        private System.Windows.Forms.RadioButton rbClimate3;
        private System.Windows.Forms.RadioButton rbClimate2;
        private System.Windows.Forms.RadioButton rbClimate1;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ZedGraph.ZedGraphControl zgcMonthlyAdjust;
        private ZedGraph.ZedGraphControl zgcAnnualMaxAdjust;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudDesignStorm;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.RadioButton rbClimate2060;
        private System.Windows.Forms.RadioButton rbClimate2035;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.LinkLabel resultsHelpLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel mnuRefreshResults;
        private System.Windows.Forms.LinkLabel mnuPrintResults;
        private System.Windows.Forms.LinkLabel mnuRemoveBaseline;
        private System.Windows.Forms.LinkLabel mnuAddBaseline;
        private System.Windows.Forms.RadioButton rbXeventRainRunoff;
        private System.Windows.Forms.RadioButton rbRunoffRainPcnt;
        private System.Windows.Forms.RadioButton rbRainRetentionFreq;
        private System.Windows.Forms.RadioButton rbRainRunoffFreq;
        private System.Windows.Forms.RadioButton rbSummaryResults;
        private System.Windows.Forms.RadioButton rbSiteDescription;
        private System.Windows.Forms.TabControl tcResults;
        private System.Windows.Forms.TabPage tpSiteDescription;
        private System.Windows.Forms.ListView lvSiteSummary;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TabPage tpSummaryResults;
        private System.Windows.Forms.ListView lvRunoffSummary;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TabPage tpRunoffBySizePlot;
        private ZedGraph.ZedGraphControl zgcRunoffPcnt;
        private System.Windows.Forms.TabPage tpRainRunoffPlot;
        private System.Windows.Forms.TabPage tpRunoffFreqPlot;
        private ZedGraph.ZedGraphControl zgcRunoffFreq;
        private System.Windows.Forms.TabPage tpRetentionFreqPlot;
        private ZedGraph.ZedGraphControl zgcRainfallCapture;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private ZedGraph.ZedGraphControl zgcBaseAnnualBudget;
        private ZedGraph.ZedGraphControl zgcAnnualBudget;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private ZedGraph.ZedGraphControl zgcExtremeEvent;
        private ZedGraph.ZedGraphControl zgcExtremePeak;
        private System.Windows.Forms.RadioButton rbRainRunoffEvents;
        private System.Windows.Forms.TabPage tpEventPlot;
        private ZedGraph.ZedGraphControl zgcRainRunoff;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.LinkLabel llSiteSuitExcellent;
        private System.Windows.Forms.LinkLabel llSiteSuitModerate;
        private System.Windows.Forms.LinkLabel llSiteSuitPoor;
        private System.Windows.Forms.RadioButton rbCostSiteExcellent;
        private System.Windows.Forms.RadioButton rbCostSitePoor;
        private System.Windows.Forms.RadioButton rbCostSiteModerate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.LinkLabel llNewDev;
        private System.Windows.Forms.LinkLabel llRedev;
        private System.Windows.Forms.RadioButton rbCostRedev;
        private System.Windows.Forms.RadioButton rbCostNewDev;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tbRegMultiplier;
        private System.Windows.Forms.LinkLabel llCostRegion;
        private System.Windows.Forms.ComboBox cmbCostRegion;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.RadioButton rbCostCapSmry;
        private System.Windows.Forms.TabPage tpCost;
    }
}

