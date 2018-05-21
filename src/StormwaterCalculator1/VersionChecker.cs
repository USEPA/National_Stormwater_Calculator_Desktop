// ----------------------------------------------------------------------------
// Module:      MainForm.cs
// Project:     EPA Stormwater Calculator
// Description: Code module to check if a more recent version of the
//              calculator exists on the EPA web site.
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.0
// Last Update: 10/14/13
//
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.IO;
using System.Net;

namespace StormwaterCalculator
{
    static class VersionChecker
    {
        static string webSite = @"http://www.epa.gov/nrmrl/wswrd/wq/models/swc/";
        // This is for the NRMRL staging server
        //static string webSite = @"http://epastage.epa.gov/nrmrldev/swc/";
        static string versionPath = webSite + "version.txt";

        internal static bool NewVersion()
        {
            // Current program version and newest version posted to web site 
            string curVersion = Application.ProductVersion;
            string newVersion = GetNewVersion(curVersion);

            // Convert versions to integers
            int curNumber = GetVersionNumber(curVersion);
            int newNumber = GetVersionNumber(newVersion);

            // Both versions are the same 
            if (newNumber <= curNumber) return false; 

            // A newer version exists so direct user to SWC web page
            string message =
                "A new release of the Stormwater Calculator is now available. " +
                "Would you like to install it?";
            string caption = "Stormwater Calculator Update";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult answer = MessageBox.Show(message, caption, buttons,
                MessageBoxIcon.Information);
            if (answer == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(webSite);
                return true;
            }
            return false;
        }

        static string GetNewVersion(string curVersion)
        {
            string newVersion = curVersion;
            try
            {
                WebClient client = new System.Net.WebClient();
                Byte[] data = client.DownloadData(versionPath);
                newVersion = Encoding.ASCII.GetString(data);
            }
            catch //(Exception webEx)
            {
                //MessageBox.Show("Web Exception: " + webEx.ToString());
                return curVersion;
            }
            return newVersion;
        }

        static int GetVersionNumber(string version)
        {
            int[] tens = {1000, 100, 10, 1};
            string[] digits = version.Split(new Char [] {'.'});
            int result = 0;
            for (int i = 0; i < Math.Min(4, digits.Length); i++)
            {
                try
                {
                    result += Int32.Parse(digits[i]) * tens[i];
                }
                catch
                {
                }
            }
            return result;
        }
    }
}
