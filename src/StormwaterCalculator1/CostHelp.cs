// ----------------------------------------------------------------------------
// Module:      CostHelp.cs
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
    public partial class CostHelp : Form
    {
        public int activePageIndex;
        public decimal designStormDepth;
        public bool hasChanged;
 
        public CostHelp()
        {
            InitializeComponent();
        }

        private void CostHelp_Activated(object sender, EventArgs e)
        // Sets up the form each time it is activated.
        {
            tabControl1.SelectedIndex = activePageIndex;
            hasChanged = false;
        }

    
    }
}
