// ----------------------------------------------------------------------------
// Module:      LidData.cs
// Project:     EPA Stormwater Calculator
// Description: Code module that manages data associated with LID controls.
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.0
// Last Update: 10/14/13
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace StormwaterCalculator
{
    public class LidData
    {
        public const double cisternHt = 48;  //generic cistern height in inches

        // Impervious Disconnection
        public static decimal idCapture;

        // Rainwater Harvesting
        public static decimal rhSize;
        public static decimal rhDrainRate;
        public static decimal rhNumber;

        // Rain Garden
        public static decimal rgRimHeight;
        public static decimal rgSoilHeight;
        public static decimal rgSoilPorosity;
        public static decimal rgSoilKsat;
        public static decimal rgCapture;

        // Green Roof
        public static decimal grSoilHeight;
        public static decimal grSoilPorosity;
        public static decimal grSoilKsat;
        public static decimal grDrainHeight;
        public static decimal grDrainVoid;

        // Street Planter
        public static decimal spRimHeight;
        public static decimal spSoilHeight;
        public static decimal spSoilPorosity;
        public static decimal spSoilKsat;
        public static decimal spDrainHeight;
        public static decimal spDrainVoid;
        public static decimal spCapture;

        // Infiltration Basin
        public static decimal ibHeight;
        public static decimal ibCapture;

        // Porous Pavement
        public static decimal ppPaveHeight;
        public static decimal ppPaveVoid;
        public static decimal ppDrainHeight;
        public static decimal ppDrainVoid;
        public static decimal ppCapture;

        public static void Init()
        // Initializes all LID properties to their default values
        {
            idCapture = Properties.LidSettings.Default.idCapture;
        
            rhSize = Properties.LidSettings.Default.rhSize;
            rhDrainRate = Properties.LidSettings.Default.rhDrainRate;
            rhNumber = Properties.LidSettings.Default.rhNumber;
        
            rgRimHeight = Properties.LidSettings.Default.rgRimHeight;
            rgSoilHeight = Properties.LidSettings.Default.rgSoilHeight;
            rgSoilPorosity = Properties.LidSettings.Default.rgSoilPorosity;
            rgSoilKsat = Properties.LidSettings.Default.rgSoilKsat;
            rgCapture = Properties.LidSettings.Default.rgCapture;

            grSoilHeight = Properties.LidSettings.Default.grSoilHeight;
            grSoilPorosity = Properties.LidSettings.Default.grSoilPorosity;
            grSoilKsat = Properties.LidSettings.Default.grSoilKsat;
            grDrainHeight = Properties.LidSettings.Default.grDrainHeight;
            grDrainVoid = Properties.LidSettings.Default.grDrainVoid;
        
            spRimHeight = Properties.LidSettings.Default.spRimHeight;
            spSoilHeight = Properties.LidSettings.Default.spSoilHeight;
            spSoilPorosity = Properties.LidSettings.Default.spSoilPorosity;
            spSoilKsat = Properties.LidSettings.Default.spSoilKsat;
            spDrainHeight = Properties.LidSettings.Default.spDrainHeight;
            spDrainVoid = Properties.LidSettings.Default.spDrainVoid;
            spCapture = Properties.LidSettings.Default.spCapture;
        
            ibHeight = Properties.LidSettings.Default.ibHeight;
            ibCapture = Properties.LidSettings.Default.ibCapture;
        
            ppPaveHeight = Properties.LidSettings.Default.ppPaveHeight;
            ppPaveVoid = Properties.LidSettings.Default.ppPaveVoid;
            ppDrainHeight = Properties.LidSettings.Default.ppDrainHeight;
            ppDrainVoid = Properties.LidSettings.Default.ppDrainVoid;
            ppCapture = Properties.LidSettings.Default.ppCapture;
        }

        public static bool Validate()
        {
            // Check that % of impervious area treated is <= 100
            if (SiteData.fracImpDiscon + SiteData.fracRainHarvest +
                SiteData.fracRainGarden + SiteData.fracStreetPlanter +
                SiteData.fracInfilBasin + SiteData.fracPorousPave > 1.01)
            {
                MessageBox.Show(
                    "The amount of impervious area assigned for treatment by\n" +
                    "LID controls exceeds the total impervious area on the site.",
                    "LID Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check there's enough pervious area to accommodate LID controls
            double fracPervArea =
                (SiteData.fracImpDiscon * (double)idCapture / 100 +
                 SiteData.fracRainGarden * (double)rgCapture / 100 +
                 SiteData.fracInfilBasin * (double)ibCapture / 100)
                * SiteData.fracImperv;
            if (fracPervArea > 1.0 - SiteData.fracImperv)
            {
                MessageBox.Show(
                    "There is not enough pervious land area on the site to handle \n" +
                    "the disconnected impervious area or to construct rain gardens or \n" +
                    "an infiltration basin. Try decreasing either the amount of \n" +
                    "impervious area treated by these controls or their Capture Ratios.", "LID Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public static void WriteLidControls(StreamWriter writer)
        // Writes generic LID descriptions to a SWMM input file
        // (LIDs not listed here, like Impervious Disconnection, are
        // not modeled using SWMM's LID extensions.)
        {
            writer.WriteLine("[LID_CONTROLS]");
            string line;
            double porosity;
            double vRatio;

            // Rain Garden
            writer.WriteLine("RainGarden  BC");
            line = "RainGarden  SURFACE  " + rgRimHeight.ToString() +
                   "  0  0  0  0";
            writer.WriteLine(line);
            porosity = (double)rgSoilPorosity / 100;
            line = "RainGarden  SOIL  " + rgSoilHeight.ToString() + "  " +
                   porosity.ToString("F3") + "  0.1  0.05  " +
                   rgSoilKsat.ToString() + "  10  1.6";
            writer.WriteLine(line);
            line = "RainGarden  STORAGE  0  0  " +
                   SiteData.soilKsat.ToString("F4") + "  0";
            writer.WriteLine(line);
            writer.WriteLine();

            // Green Roof
            writer.WriteLine("GreenRoof  BC");
            line = "GreenRoof  SURFACE  0  0  0  0  0";
            writer.WriteLine(line);
            porosity = (double)grSoilPorosity / 100;
            line = "GreenRoof  SOIL  " + grSoilHeight.ToString() + "  " +
                porosity.ToString("F3") + "  0.1  0.05  " +
                grSoilKsat.ToString() + "  10  1.6";
            writer.WriteLine(line);
            line = "GreenRoof  STORAGE  3  0.75  0  0";
            writer.WriteLine(line);
            line = "GreenRoof  DRAIN  " + grSoilKsat.ToString() + "  0.5  0  0";
            writer.WriteLine(line);
            writer.WriteLine();

            // Street Planter
            writer.WriteLine("StreetPlanter  BC");
            line = "StreetPlanter  SURFACE  " + spRimHeight.ToString() +
                   "  0  0  0  0";
            writer.WriteLine(line);
            porosity = (double)spSoilPorosity / 100;
            line = "StreetPlanter  SOIL  " + spSoilHeight.ToString() + "  " +
                   porosity.ToString("F3") + "  0.1  0.05  " +
                   spSoilKsat.ToString() + "  10  1.6";
            writer.WriteLine(line);
            vRatio = (double)spDrainVoid / 100;
            line = "StreetPlanter  STORAGE  " + spDrainHeight.ToString() +
                   "  " + vRatio.ToString("F3") + "  " +
                   SiteData.soilKsat.ToString("F4") + "  0";
            writer.WriteLine(line);
            writer.WriteLine();

            // Porous Pavement
            writer.WriteLine("PorousPavement  PP");
            writer.WriteLine("PorousPavement  SURFACE  0.05  0  0.01  3  0");
            vRatio = (double)ppPaveVoid / 100;
            line = "PorousPavement  PAVEMENT  " + ppPaveHeight.ToString() +
                   "  " + vRatio.ToString("F3") + "  0  400  0";
            writer.WriteLine(line);
            vRatio = (double)ppDrainVoid / 100;
            line = "PorousPavement  STORAGE  " + ppDrainHeight.ToString() +
                   "  " + vRatio.ToString("F3") + "  " +
                   SiteData.soilKsat.ToString("F4") + "  0";
            writer.WriteLine(line);
            writer.WriteLine();
        }

        public static void WriteLidUsage(StreamWriter writer)
        // Writes LID usage data to a SWMM input file.
        // (LIDs not listed here, like Impervious Disconnection, are
        // not modeled using SWMM's LID extensions.)
        {
            double area = 0;
            double width = 0;
            double fromImperv = 0;
            double capturedPct = 0;
            string line;

            writer.WriteLine("[LID_USAGE]");

            // Porous Pavement
            if (SiteData.ppArea > 0)
            {
                area = SiteData.ppArea * 43560;
                width = area / SiteData.pathLength;
                if (SiteData.impervArea1 > 0)
                {
                    capturedPct = (100 - (double)ppCapture) / (double)ppCapture * 100;
                    fromImperv = SiteData.ppArea / SiteData.impervArea1 * capturedPct;
                    fromImperv = Math.Min(100, fromImperv);
                }
                else fromImperv = 0.0;
                line = "Subcatch1  PorousPavement  1  " + area.ToString("F2") + "  " +
                       width.ToString("F2") + "  0  " + fromImperv.ToString("F2") + "  0";
                writer.WriteLine(line);
            }

            // Rain Garden
            if (SiteData.rgArea > 0)
            {
                area = SiteData.rgArea * 43560;
                width = 0;
                if (SiteData.impervArea1 > 0)
                {
                    fromImperv = SiteData.fracRainGarden * 100.0;
                }
                else fromImperv = 0.0;
                line = "Subcatch1  RainGarden  1  " + area.ToString("F2") +
                       "  0  0  " + fromImperv.ToString("F2") + "  0";
                writer.WriteLine(line);
            }

            // Green Roof
            if (SiteData.grArea > 0)
            {
                area = SiteData.grArea * 43560;
                width = 0;
                line = "Subcatch1  GreenRoof  1  " + area.ToString("F2") +
                    "  0  0  0  0";
                writer.WriteLine(line);
            }

            // Street Planter
            if (SiteData.spArea > 0)
            {
                area = SiteData.spArea * 43560;
                width = 0;
                if (SiteData.impervArea1 > 0)
                {
                    capturedPct = (100 - (double)spCapture) / (double)spCapture * 100;
                    fromImperv = SiteData.spArea / SiteData.impervArea1 * capturedPct;
                    fromImperv = Math.Min(100, fromImperv);
                }
                else fromImperv = 0.0;
                line = "Subcatch1  StreetPlanter  1  " + area.ToString("F2") +
                       "  0  0  " + fromImperv.ToString("F2") + "  0";
                writer.WriteLine(line);
            }
            writer.WriteLine();
        }
    }
}
