// ----------------------------------------------------------------------------
// Module:      LidForm.cs
// Project:     EPA Stormwater Calculator
// Description: Code module for form that sets design parameters for LID controls.
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.00.00
// Last Update: 4/19/13
// ----------------------------------------------------------------------------

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
    public partial class LidForm : Form
    {
        public int activePageIndex;
        public decimal designStormDepth;
        public bool hasChanged;
 
        public LidForm()
        {
            InitializeComponent();
        }

        private void LidForm_Activated(object sender, EventArgs e)
        // Sets up the form each time it is activated.
        {
            tabControl1.SelectedIndex = activePageIndex;
            btnAutoSize.Enabled = (designStormDepth > 0);
            if (tabControl1.SelectedIndex == 0 ||
                tabControl1.SelectedIndex == 3) btnAutoSize.Enabled = false;
            LoadData();
            hasChanged = false;
        }

        private void btnAutoSize_Click(object sender, EventArgs e)
        // Automatically sizes an LID control when the AutoSize button is clicked.
        {
            decimal lidDepth;
            decimal captureRatio;

            // Reduced storm depth by avg. amount of infiltration over 24 hrs.
            decimal netStormDepth = designStormDepth -(decimal)SiteData.soilKsat * 24m / 2m;

            // Analyze the currently selected LID
            switch (activePageIndex)
            {
                // Rainwater Harvesting
                case 1:
                    // Number of cisterns needed per 1000 sq ft of area captured
                    decimal numCisterns = 1000m * designStormDepth / 12m * 7.85m /
                                          nudRHSize.Value;
                    nudRHNumber.Value = Math.Ceiling(numCisterns);
                    break;

                // Rain Garden
                case 2:
                    // Available depth
                    lidDepth = nudRGRimHeight.Value +
                        nudRGSoilHeight.Value * LidData.rgSoilPorosity / 100;
                    // Capture ratio is design storm depth / available depth
                    if (netStormDepth >= lidDepth) captureRatio = 1;
                    else captureRatio = designStormDepth / (lidDepth - netStormDepth);
                    captureRatio = Math.Min(captureRatio, 1);
                    nudRGCapture.Value = captureRatio * 100;
                    break;

                // Street Planter
                case 4:
                    // Available depth
                    lidDepth = nudSPRimHeight.Value +
                        nudSPSoilHeight.Value * LidData.spSoilPorosity / 100 +
                        nudSPGravelHeight.Value * LidData.spDrainVoid /
                        (100 + LidData.spDrainVoid);
                    // Capture ratio is design storm depth / available depth
                    if (netStormDepth >= lidDepth) captureRatio = 1;
                    else captureRatio = designStormDepth / (lidDepth - netStormDepth);
                    captureRatio = Math.Min(captureRatio, 1);
                    nudSPCapture.Value = captureRatio * 100;
                    break;

                // Infiltration Basin 
                case 5:
                    // Select depth to drain in 48 hours
                    lidDepth = (decimal)SiteData.soilKsat * 48;
                    if (nudIBHeight.Value > lidDepth) lidDepth = nudIBHeight.Value;
                    lidDepth = Math.Min(lidDepth, nudIBHeight.Maximum);

                    // Capture ratio is design storm depth / available basin depth
                    if (netStormDepth >= lidDepth) captureRatio = 1;
                    else captureRatio = designStormDepth / (lidDepth - netStormDepth);
                    nudIBHeight.Value = lidDepth;
                    nudIBCapture.Value = captureRatio * 100;
                    break;

                // Porous Pavement
                case 6:
                    // Capture ratio is design storm depth / available reservoir depth
                    lidDepth = nudPPGravelHeight.Value *
                        LidData.ppDrainVoid / (100 + LidData.ppDrainVoid);
                    if (netStormDepth >= lidDepth) captureRatio = 1;
                    else captureRatio = designStormDepth / (lidDepth - netStormDepth);
                    captureRatio = Math.Min(captureRatio, 1);
                    nudPPCapture.Value = captureRatio * 100;
                    break;
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            UnloadData();
            if (hasChanged) MainForm.inputHasChanged = true;
            Hide();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            LoadDefaultData();
        }

        private void nud_Enter(object sender, EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown)sender;
            nud.Select(0, 100);
        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            hasChanged = true;
        }

        private void LoadDefaultData()
        {
            switch (activePageIndex)
            {
                case 0: LoadDefaultIDData(); break;   // Impervious Disconnection
                case 1: LoadDefaultRHData(); break;   // Rainwater Harvesting
                case 2: LoadDefaultRGData(); break;   // Rain Garden
                case 3: LoadDefaultGRData(); break;   // Green Roof
                case 4: LoadDefaultSPData(); break;   // Street Planter
                case 5: LoadDefaultIBData(); break;   // Infiltration Basin
                case 6: LoadDefaultPPData(); break;   // Porous Pavement
            }
        }

        private void LoadDefaultIDData()
        {
            nudIDCapture.Value = Properties.LidSettings.Default.idCapture;
        }

        private void LoadDefaultRGData()
        {
            nudRGRimHeight.Value = Properties.LidSettings.Default.rgRimHeight;
            nudRGSoilHeight.Value = Properties.LidSettings.Default.rgSoilHeight;
            nudRGSoilKsat.Value = Properties.LidSettings.Default.rgSoilKsat;
            nudRGCapture.Value = Properties.LidSettings.Default.rgCapture;
        }

        private void LoadDefaultGRData()
        {
            nudGRSoilHeight.Value = Properties.LidSettings.Default.grSoilHeight;
            nudGRSoilKsat.Value = Properties.LidSettings.Default.grSoilKsat;
        }

        private void LoadDefaultPPData()
        {
            nudPPPavementHeight.Value = Properties.LidSettings.Default.ppPaveHeight;
            nudPPGravelHeight.Value = Properties.LidSettings.Default.ppDrainHeight;
            nudPPCapture.Value = Properties.LidSettings.Default.ppCapture;
        }

        private void LoadDefaultRHData()
        {
            nudRHSize.Value = Properties.LidSettings.Default.rhSize;
            nudRHDrainRate.Value = Properties.LidSettings.Default.rhDrainRate;
            nudRHNumber.Value = Properties.LidSettings.Default.rhNumber;
        }

        private void LoadDefaultSPData()
        {
            nudSPRimHeight.Value = Properties.LidSettings.Default.spRimHeight;
            nudSPSoilHeight.Value = Properties.LidSettings.Default.spSoilHeight;
            nudSPSoilKsat.Value = Properties.LidSettings.Default.spSoilKsat;
            nudSPGravelHeight.Value = Properties.LidSettings.Default.spDrainHeight;
            nudSPCapture.Value = Properties.LidSettings.Default.spCapture;
        }

        private void LoadDefaultIBData()
        {
            nudIBHeight.Value = Properties.LidSettings.Default.ibHeight;
            nudIBCapture.Value = Properties.LidSettings.Default.ibCapture;
        }

        private void LoadData()
        {
            switch (activePageIndex)
            {
                case 0: LoadIDData(); break;
                case 1: LoadRHData(); break;
                case 2: LoadRGData(); break;
                case 3: LoadGRData(); break;
                case 4: LoadSPData(); break;
                case 5: LoadIBData(); break;
                case 6: LoadPPData(); break;
            }
        }
        
        private void LoadIDData()
        {
            nudIDCapture.Value = LidData.idCapture;
        }

        private void LoadRGData()
        {
            nudRGRimHeight.Value = LidData.rgRimHeight;
            nudRGSoilHeight.Value = LidData.rgSoilHeight;
            nudRGSoilKsat.Value = LidData.rgSoilKsat;
            nudRGCapture.Value = LidData.rgCapture;
        }

        private void LoadGRData()
        {
            nudGRSoilHeight.Value = LidData.grSoilHeight;
            nudGRSoilKsat.Value = LidData.grSoilKsat;
        }

        private void LoadPPData()
        {
            nudPPPavementHeight.Value = LidData.ppPaveHeight;
            nudPPGravelHeight.Value = LidData.ppDrainHeight;
            nudPPCapture.Value = LidData.ppCapture;
        }

        private void LoadRHData()
        {
            nudRHSize.Value = LidData.rhSize;
            nudRHDrainRate.Value = LidData.rhDrainRate;
            nudRHNumber.Value = LidData.rhNumber;
        }

        private void LoadSPData()
        {
            nudSPRimHeight.Value = LidData.spRimHeight;
            nudSPSoilHeight.Value = LidData.spSoilHeight;
            nudSPSoilKsat.Value = LidData.spSoilKsat;
            nudSPGravelHeight.Value = LidData.spDrainHeight;
            nudSPCapture.Value = LidData.spCapture;
        }

        private void LoadIBData()
        {
            nudIBHeight.Value = LidData.ibHeight;
            nudIBCapture.Value = LidData.ibCapture;
        }

        private void UnloadData()
        {
            switch (activePageIndex)
            {
                case 0: UnloadIDData(); break;
                case 1: UnloadRHData(); break;
                case 2: UnloadRGData(); break;
                case 3: UnloadGRData(); break;
                case 4: UnloadSPData(); break;
                case 5: UnloadIBData(); break;
                case 6: UnloadPPData(); break;
            }
        }

        private void UnloadIDData()
        {
            LidData.idCapture = nudIDCapture.Value;
        }

        private void UnloadRGData()
        {
            LidData.rgRimHeight = nudRGRimHeight.Value;
            LidData.rgSoilHeight = nudRGSoilHeight.Value;
            LidData.rgSoilKsat = nudRGSoilKsat.Value;
            LidData.rgCapture = nudRGCapture.Value;
        }

        private void UnloadGRData()
        {
            LidData.grSoilHeight = nudGRSoilHeight.Value;
            LidData.grSoilKsat = nudGRSoilKsat.Value;
        }

        private void UnloadPPData()
        {
            LidData.ppPaveHeight = nudPPPavementHeight.Value;
            LidData.ppDrainHeight = nudPPGravelHeight.Value;
            LidData.ppCapture = nudPPCapture.Value;
        }

        private void UnloadRHData()
        {
            LidData.rhSize = nudRHSize.Value;
            LidData.rhDrainRate = nudRHDrainRate.Value;
            LidData.rhNumber = nudRHNumber.Value;
        }

        private void UnloadSPData()
        {
             LidData.spRimHeight = nudSPRimHeight.Value;
             LidData.spSoilHeight = nudSPSoilHeight.Value;
             LidData.spSoilKsat = nudSPSoilKsat.Value;
             LidData.spDrainHeight = nudSPGravelHeight.Value;
             LidData.spCapture = nudSPCapture.Value;
        }

        private void UnloadIBData()
        {
             LidData.ibHeight = nudIBHeight.Value;
             LidData.ibCapture = nudIBCapture.Value;
        }

        // The OnLinkClicked handlers below display a specified web page in the 
        // user's default browser when the "Learn More..." label is clicked for
        // each type of LID.

        private void lnkLblPorPave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "http://vwrrc.vt.edu/swc/NonPBMPSpecsMarch11/VASWMBMPSpec7PERMEABLEPAVEMENT.html");
        }

        private void lnkLblInfilBasin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cabmphandbooks.com/Documents/Development/TC-11.pdf");
        }

        private void lnkLblPlanter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "http://vwrrc.vt.edu/swc/NonPBMPSpecsMarch11/VASWMBMPSpec9BIORETENTION.html");
        }

        private void lnkLblDisconnect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "http://vwrrc.vt.edu/swc/NonPBMPSpecsMarch11/VASWMBMPSpec1DISCONNECTION.html");
        }

        private void lnlLblRainGarden_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "http://vwrrc.vt.edu/swc/NonPBMPSpecsMarch11/VASWMBMPSpec9BIORETENTION.html");
        }

        private void lnkLblRainHarvest_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "http://vwrrc.vt.edu/swc/NonPBMPSpecsMarch11/VASWMBMPSpec6RAINWATERHARVESTING.html");
        }

        private void lnkLblGreenRoof_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                "http://vwrrc.vt.edu/SWC/NonPBMPSpecsMarch11/VASWMBMPSpec5VEGETATEDROOF.html");
        }
    }
}
