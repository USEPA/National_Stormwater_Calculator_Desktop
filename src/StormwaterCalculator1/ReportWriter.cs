// ----------------------------------------------------------------------------
// Module:      ReportWriter.cs
// Project:     EPA Stormwater Calculator
// Description: Code module for writing Calculator's results to a PDF file.
// Copyright:   N/A
// Authors:     L. Rossman, US EPA
// Version:     1.1.0.0
// Last Update: 10/14/13
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

// SharpPdf Library
using sharpPDF;
using sharpPDF.PDFControls;
using sharpPDF.Enumerators;
using sharpPDF.Fonts;
using sharpPDF.Tables;
using sharpPDF.Collections;

namespace StormwaterCalculator
{
    class ReportWriter
    {
        protected string strFont;
        protected int leftMargin;
        protected int rightMargin;
        protected int topMargin;
        protected int bottomMargin;
        protected int lineHeight;
        protected pdfDocument myDoc;
        protected string footer = "US EPA National Stormwater Calculator - ";
        static string newLine = System.Environment.NewLine;

        // Constructor
        public ReportWriter(string releaseVersion)
        {
            strFont = "Helvetica";
            leftMargin = 60;
            rightMargin = 60;
            topMargin = 60;
            bottomMargin = 60;
            lineHeight = 20;
            myDoc = new pdfDocument("Stormwater Calculator Report", "swc");
            int strWidth = myDoc.getFontReference(strFont).getWordWidth("Page x Of x", 10);
            int left = 612 - rightMargin - strWidth;
            myDoc.pageMarker = new pdfPageMarker(left, bottomMargin - lineHeight, predefinedMarkerStyle.csArabic,
                myDoc.getFontReference(strFont), 10);
            footer = footer + releaseVersion;
        }

        // Used to translate first quadrant coordinate to fourth for PDF page layout
        protected int GetTop(pdfPage myPage, int nTop)
        {
            return myPage.height - nTop;
        }

        protected int GetLineTop(pdfPage myPage, int lineNumber)
        {
            return myPage.height - topMargin - lineNumber * lineHeight;
        }

        protected int GetCenteredOffset(pdfPage myPage, string text, int fontSize)
        {
            // Get width of string
            int strWidth = myDoc.getFontReference(strFont).getWordWidth(text, fontSize);

            // Find left coordinate
            return (myPage.width - strWidth) / 2;
        }

        protected void WritePageTitle(pdfPage myPage, string pageTitle, string siteName)
        {
            string title = "National Stormwater Calculator Report";
            int line = 0;
            myPage.addText(title, GetCenteredOffset(myPage, title, 20),
               GetLineTop(myPage, line), myDoc.getFontReference(strFont), 20, pdfColor.Black);
            line = line + 2;

            if (pageTitle.Length > 0)
            {
                myPage.addText(pageTitle, GetCenteredOffset(myPage, pageTitle, 20),
                    GetLineTop(myPage, line), myDoc.getFontReference(strFont), 20, pdfColor.Black);
                line = line + 2;
            }

            if (siteName.Length > 0)
            {
                myPage.addText(siteName, GetCenteredOffset(myPage, siteName, 12),
                    GetLineTop(myPage, line), myDoc.getFontReference(strFont), 12, pdfColor.Black);
            }
        }

        protected void WriteFooter(pdfPage myPage)
        {
            myPage.drawLine(leftMargin, bottomMargin, myPage.width - rightMargin,
                bottomMargin, predefinedLineStyle.csNormal, pdfColor.Black, 1);
            myPage.addText(footer, leftMargin, bottomMargin - lineHeight,
                myDoc.getFontReference(strFont), 10);
        }

        public void WriteSiteSummaryPage(string siteName, ListView lv)
        {
            pdfPage myPage = myDoc.addPage(predefinedPageSize.csSharpPDFFormat);
            int width = myPage.width - leftMargin - rightMargin;
            WritePageTitle(myPage, "Site Description", siteName);

            pdfTable myTable = new pdfTable(myDoc, 1, pdfColor.Black, 5,
                new pdfTableStyle(myDoc.getFontReference(strFont), 12, pdfColor.Black, new pdfColor("d9d1b3")),
                new pdfTableStyle(myDoc.getFontReference(strFont), 12, pdfColor.Black, pdfColor.White),
                new pdfTableStyle(myDoc.getFontReference(strFont), 12, pdfColor.Black, pdfColor.White));
            myTable.coordX = leftMargin;
            myTable.coordY = GetLineTop(myPage, 5);
            myTable.tableHeader.rowHeight = 25;
            myTable.tableHeader.addColumn(175, predefinedAlignment.csLeft);
            myTable.tableHeader[0].addText("Parameter");
            myTable.tableHeader.addColumn(160, predefinedAlignment.csLeft);
            myTable.tableHeader[1].addText("Current Scenario");
            myTable.tableHeader.addColumn(160, predefinedAlignment.csLeft);
            myTable.tableHeader[2].addText("Baseline Scenario");

            int tableRowHt = 15;
            pdfTableRow myRow;
            for (int i = 0; i < lv.Items.Count; i++)
            {
                if (i > 11 && i <= 18) continue;
                myRow = myTable.createRow();
                myRow.rowHeight = tableRowHt;
                for (int j = 0; j < 3; j++)
                {
                    myRow[j].addText(lv.Items[i].SubItems[j].Text);
                }
                myTable.addRow(myRow);
            }
            myPage.addTable(myTable);
            WriteLidSummaryTable(myPage, lv);
            WriteFooter(myPage);
        }

        protected void WriteLidSummaryTable(pdfPage myPage, ListView lv)
        {
            pdfTable myTable = new pdfTable(myDoc, 1, pdfColor.Black, 5,
                new pdfTableStyle(myDoc.getFontReference(strFont), 12, pdfColor.Black, new pdfColor("d9d1b3")),
                new pdfTableStyle(myDoc.getFontReference(strFont), 12, pdfColor.Black, pdfColor.White),
                new pdfTableStyle(myDoc.getFontReference(strFont), 12, pdfColor.Black, pdfColor.White));
            myTable.coordX = leftMargin;
            myTable.coordY = GetLineTop(myPage, 23);
            myTable.tableHeader.rowHeight = 25;
            myTable.tableHeader.addColumn(175, predefinedAlignment.csLeft);
            myTable.tableHeader[0].addText("LID Control");
            myTable.tableHeader.addColumn(160, predefinedAlignment.csLeft);
            myTable.tableHeader[1].addText("Current Scenario");
            myTable.tableHeader.addColumn(160, predefinedAlignment.csLeft);
            myTable.tableHeader[2].addText("Baseline Scenario");

            int tableRowHt = 15;
            pdfTableRow myRow;
            for (int i = 12; i <= 18; i++)
            {
                myRow = myTable.createRow();
                myRow.rowHeight = tableRowHt;
                for (int j = 0; j < 3; j++)
                {
                    myRow[j].addText(lv.Items[i].SubItems[j].Text);
                }
                myTable.addRow(myRow);
            }
            myPage.addTable(myTable);
            myPage.addText("% of impervious area treated / % of treated area used for LID",
                leftMargin, bottomMargin + lineHeight, myDoc.getFontReference(strFont), 10);
        }

        public void WriteSummaryResultsPage(string siteName, ListView lv, Image plotImage1,
            Image plotImage2, double aspectRatio)
        {
            pdfPage myPage = myDoc.addPage(predefinedPageSize.csSharpPDFFormat);
            int width = myPage.width - leftMargin - rightMargin;
            WritePageTitle(myPage, "Summary Results", siteName);

            pdfTable myTable = new pdfTable(myDoc, 1, pdfColor.Black, 5,
                new pdfTableStyle(myDoc.getFontReference(strFont), 12, pdfColor.Black, new pdfColor("d9d1b3")),
                new pdfTableStyle(myDoc.getFontReference(strFont), 12, pdfColor.Black, pdfColor.White),
                new pdfTableStyle(myDoc.getFontReference(strFont), 12, pdfColor.Black, pdfColor.White));
            myTable.coordX = leftMargin;
            myTable.coordY = GetLineTop(myPage, 5);
            myTable.tableHeader.rowHeight = 25;
            myTable.tableHeader.addColumn(225, predefinedAlignment.csLeft);
            myTable.tableHeader[0].addText("Statistic");
            myTable.tableHeader.addColumn(120, predefinedAlignment.csRight);
            myTable.tableHeader[1].addText("Current Scenario");
            myTable.tableHeader.addColumn(120, predefinedAlignment.csRight);
            myTable.tableHeader[2].addText("Baseline Scenario");

            int tableRowHt = 15;
            pdfTableRow myRow;
            for (int i = 0; i < lv.Items.Count; i++)
            {
                myRow = myTable.createRow();
                myRow.rowHeight = tableRowHt;
                for (int j = 0; j < 3; j++)
                {
                    myRow[j].addText(lv.Items[i].SubItems[j].Text);
                }
                myTable.addRow(myRow);
            }
            myPage.addTable(myTable);

            int plotWidth = width / 2;
            int plotHeight = (int)Math.Round((double)plotWidth * aspectRatio);
            int yPos = bottomMargin + 4 * lineHeight;
            myDoc.addImageReference(plotImage1, "pieChart1");
            myPage.addImage(myDoc.getImageReference("pieChart1"),
                leftMargin, yPos, plotHeight, plotWidth);
            if (plotImage2 != null)
            {
                myDoc.addImageReference(plotImage2, "pieChart2");
                myPage.addImage(myDoc.getImageReference("pieChart2"),
                    leftMargin + plotWidth, yPos, plotHeight, plotWidth);
            }
            WriteFooter(myPage);
        }

        public void WriteImagePage(string siteName, Image plotImage1, Image plotImage2,
            string imageName1, string imageName2, double aspectRatio)
        {
            pdfPage myPage = myDoc.addPage(predefinedPageSize.csSharpPDFFormat);
            int width = myPage.width - leftMargin - rightMargin;
            WritePageTitle(myPage, "", siteName);
            myDoc.addImageReference(plotImage1, imageName1);

            int plotHeight = (myPage.height - topMargin - bottomMargin - 6 * lineHeight) / 2;
            int plotWidth = (int)Math.Round((double)plotHeight / aspectRatio);
            plotWidth = Math.Min(plotWidth, myPage.width - leftMargin - rightMargin);
            int xPos = (myPage.width - plotWidth) / 2;
            int yPos = GetLineTop(myPage, 3) - plotHeight;
            myPage.addImage(myDoc.getImageReference(imageName1), xPos, yPos, plotHeight, plotWidth);
            myPage.drawLine(xPos, yPos, xPos + plotWidth, yPos, predefinedLineStyle.csNormal, pdfColor.Black, 1);

            if (plotImage2 != null)
            {
                myDoc.addImageReference(plotImage2, imageName2);
                yPos = yPos - plotHeight - lineHeight;
                myPage.addImage(myDoc.getImageReference(imageName2), xPos, yPos, plotHeight, plotWidth);
                myPage.drawLine(xPos, yPos, xPos + plotWidth, yPos, predefinedLineStyle.csNormal, pdfColor.Black, 1);
            }
            WriteFooter(myPage);
        }
        
        //writes capital cost estimates summary to the pdf report
        protected void WriteCapitalCostSummaryTable(pdfPage myPage, string tableTitle,int tblStartLine, dynamic costModuleResults)
        {
            int line = tblStartLine;
            myPage.addText(tableTitle, GetCenteredOffset(myPage, tableTitle, 20),
               GetLineTop(myPage, line), myDoc.getFontReference(strFont), 16, pdfColor.Black);
            line = line + 1;

            pdfTable myTable = new pdfTable(myDoc, 1, pdfColor.Black, 5,
                new pdfTableStyle(myDoc.getFontReference(strFont), 10, pdfColor.Black, new pdfColor("d9d1b3")),
                new pdfTableStyle(myDoc.getFontReference(strFont), 10, pdfColor.Black, pdfColor.White),
                new pdfTableStyle(myDoc.getFontReference(strFont), 10, pdfColor.Black, pdfColor.White));
            myTable.coordX = leftMargin;
            myTable.coordY = GetLineTop(myPage, line);
            myTable.tableHeader.rowHeight = 25;
            myTable.tableHeader.addColumn(120, predefinedAlignment.csLeft);
            myTable.tableHeader[0].addText("LID Control");
            myTable.tableHeader.addColumn(130, predefinedAlignment.csLeft);
            myTable.tableHeader[1].addText("Current Scenario");
            myTable.tableHeader.addColumn(130, predefinedAlignment.csLeft);
            myTable.tableHeader[2].addText("Baseline Scenario");
            myTable.tableHeader.addColumn(130, predefinedAlignment.csLeft);
            myTable.tableHeader[3].addText("Cost Difference");

            int tableRowHt = 15;
            string tempCostLow;
            string tempCostHigh;
            string tempCostDiffLow;
            string tempCostDiffHigh;
            pdfTableRow myRow;
            //foreach (dynamic o in costModuleResults)
            for (int i = 0; i < costModuleResults.Count - 1; i++)
            {
                dynamic o = costModuleResults[i];
                myRow = myTable.createRow();
                myRow.rowHeight = tableRowHt;

                myRow[0].addText(Convert.ToString(o.name));//lid control name

                if (o.currentScenarioCapCostLow != null)
                {
                    tempCostLow = String.Format("{0:#,###,###.##}", Math.Round((Double)o.currentScenarioCapCostLow / 100.0, 0) * 100);
                    tempCostHigh = String.Format("{0:#,###,###.##}", Math.Round((Double)o.currentScenarioCapCostHigh / 100.0, 0) * 100);
                    myRow[1].addText("$ " + tempCostLow + " - " + "$ " + tempCostHigh);//current capital cost low
                }


                if (o.baseScenarioCapCostLow != null)
                {
                    tempCostLow = String.Format("{0:#,###,###.##}", Math.Round((Double)o.baseScenarioCapCostLow / 100.0, 0) * 100);
                    tempCostHigh = String.Format("{0:#,###,###.##}", Math.Round((Double)o.baseScenarioCapCostHigh / 100.0, 0) * 100);
                    myRow[2].addText("$ " + tempCostLow + " - " + "$ " + tempCostHigh);//base capital cost high

                    //populate cost difference column
                    tempCostDiffLow = String.Format("{0:#,###,###.##}", Math.Round(((Double)o.currentScenarioCapCostLow - (Double)o.baseScenarioCapCostLow) / 100.0, 0) * 100);
                    tempCostDiffHigh = String.Format("{0:#,###,###.##}", Math.Round(((Double)o.currentScenarioCapCostHigh - (Double)o.baseScenarioCapCostHigh) / 100.0, 0) * 100);
                    myRow[3].addText("$ " + tempCostDiffLow + " - " + "$ " + tempCostDiffHigh);//cost difference
                }

                myTable.addRow(myRow);
            }

            //last row is totals
            myRow = myTable.createRow();
            myRow.rowHeight = tableRowHt;
            dynamic oTotal = costModuleResults[costModuleResults.Count-1];
            myRow[0].addText("Total");

            //current capital cost low and high total
            tempCostLow = String.Format("{0:#,###,###.##}", Math.Round((Double)oTotal.currentScenarioCapCostLowSum / 100.0, 0) * 100);
            tempCostHigh = String.Format("{0:#,###,###.##}", Math.Round((Double)oTotal.currentScenarioCapCostHighSum / 100.0, 0) * 100);
            myRow[1].addText("$ " + tempCostLow + " - " + "$ " + tempCostHigh);//current capital cost low

            //base capital cost difference total
            if (costModuleResults[costModuleResults.Count - 2].baseScenarioCapCostLow != null)
            {
                tempCostLow = String.Format("{0:#,###,###.##}", Math.Round((Double)oTotal.baseScenarioCapCostLowSum / 100.0, 0) * 100);
                tempCostHigh = String.Format("{0:#,###,###.##}", Math.Round((Double)oTotal.baseScenarioCapCostHighSum / 100.0, 0) * 100);
                myRow[2].addText("$ " + tempCostLow + " - " + "$ " + tempCostHigh);//base capital cost high

                //populate cost difference column
                tempCostDiffLow = String.Format("{0:#,###,###.##}", Math.Round(((Double)oTotal.currentScenarioCapCostLowSum - (Double)oTotal.baseScenarioCapCostLowSum) / 100.0, 0) * 100);
                tempCostDiffHigh = String.Format("{0:#,###,###.##}", Math.Round(((Double)oTotal.currentScenarioCapCostHighSum - (Double)oTotal.baseScenarioCapCostHighSum) / 100.0, 0) * 100);
                myRow[3].addText("$ " + tempCostDiffLow + " - " + "$ " + tempCostDiffHigh);//cost difference
            }
            myTable.addRow(myRow);

            myPage.addTable(myTable);

        }

        //writes maintenance cost estimates summary to the pdf report
        protected void WriteMaintCostSummaryTable(pdfPage myPage, string tableTitle, int tblStartLine, dynamic costModuleResults)
        {
            int line = tblStartLine;
            myPage.addText(tableTitle, GetCenteredOffset(myPage, tableTitle, 20),
               GetLineTop(myPage, line), myDoc.getFontReference(strFont), 16, pdfColor.Black);
            line = line + 1;
            //myPage.addText(tableTitle, leftMargin, bottomMargin + lineHeight, myDoc.getFontReference(strFont), 10);
            pdfTable myTable = new pdfTable(myDoc, 1, pdfColor.Black, 5,
                new pdfTableStyle(myDoc.getFontReference(strFont), 10, pdfColor.Black, new pdfColor("d9d1b3")),
                new pdfTableStyle(myDoc.getFontReference(strFont), 10, pdfColor.Black, pdfColor.White),
                new pdfTableStyle(myDoc.getFontReference(strFont), 10, pdfColor.Black, pdfColor.White));
            myTable.coordX = leftMargin;
            myTable.coordY = GetLineTop(myPage, line);
            myTable.tableHeader.rowHeight = 25;
            myTable.tableHeader.addColumn(120, predefinedAlignment.csLeft);
            myTable.tableHeader[0].addText("LID Control");
            myTable.tableHeader.addColumn(130, predefinedAlignment.csLeft);
            myTable.tableHeader[1].addText("Current Scenario");
            myTable.tableHeader.addColumn(130, predefinedAlignment.csLeft);
            myTable.tableHeader[2].addText("Baseline Scenario");
            myTable.tableHeader.addColumn(130, predefinedAlignment.csLeft);
            myTable.tableHeader[3].addText("Cost Difference");


            int tableRowHt = 15;
            string tempCostLow;
            string tempCostHigh;
            string tempCostDiffLow;
            string tempCostDiffHigh;
            pdfTableRow myRow;
            //foreach (dynamic o in costModuleResults)
            for (int i = 0; i < costModuleResults.Count-1; i++) 
            {
                dynamic o = costModuleResults[i];
                myRow = myTable.createRow();
                myRow.rowHeight = tableRowHt;


                myRow[0].addText(Convert.ToString(o.name));//lid control name

                if (o.currentScenarioMaintCostLow != null)
                {
                    tempCostLow = String.Format("{0:#,###,###.##}", Math.Round((Double)o.currentScenarioMaintCostLow / 100.0, 0) * 100);
                    tempCostHigh = String.Format("{0:#,###,###.##}", Math.Round((Double)o.currentScenarioMaintCostHigh / 100.0, 0) * 100);
                    myRow[1].addText("$ " + tempCostLow + " - " + "$ " + tempCostHigh);//current maint cost low
                }

                if (o.baseScenarioMaintCostLow != null)
                {
                    tempCostLow = String.Format("{0:#,###,###.##}", Math.Round((Double)o.baseScenarioMaintCostLow / 100.0, 0) * 100);
                    tempCostHigh = String.Format("{0:#,###,###.##}", Math.Round((Double)o.baseScenarioMaintCostHigh / 100.0, 0) * 100);
                    myRow[2].addText("$ " + tempCostLow + " - " + "$ " + tempCostHigh);//base maint cost high

                    //populate cost difference column
                    tempCostDiffLow = String.Format("{0:#,###,###.##}", Math.Round(((Double)o.currentScenarioMaintCostLow - (Double)o.baseScenarioMaintCostLow) / 100.0, 0) * 100);
                    tempCostDiffHigh = String.Format("{0:#,###,###.##}", Math.Round(((Double)o.currentScenarioMaintCostHigh - (Double)o.baseScenarioMaintCostHigh) / 100.0, 0) * 100);
                    myRow[3].addText("$ " + tempCostDiffLow + " - " + "$ " + tempCostDiffHigh);//cost difference
                }  
                myTable.addRow(myRow);
            }

            //last row is totals
            myRow = myTable.createRow();
            myRow.rowHeight = tableRowHt;
            dynamic oTotal = costModuleResults[costModuleResults.Count - 1];
            myRow[0].addText("Total");

            //current maintenance cost low and high total
            tempCostLow = String.Format("{0:#,###,###.##}", Math.Round((Double)oTotal.currentScenarioMaintCostLowSum / 100.0, 0) * 100);
            tempCostHigh = String.Format("{0:#,###,###.##}", Math.Round((Double)oTotal.currentScenarioMaintCostHighSum / 100.0, 0) * 100);
            myRow[1].addText("$ " + tempCostLow + " - " + "$ " + tempCostHigh);//current maintenance cost low

            //base maintenance cost difference total
            if (costModuleResults[costModuleResults.Count - 2].baseScenarioCapCostLow != null)
            {
                tempCostLow = String.Format("{0:#,###,###.##}", Math.Round((Double)oTotal.baseScenarioMaintCostLowSum / 100.0, 0) * 100);
                tempCostHigh = String.Format("{0:#,###,###.##}", Math.Round((Double)oTotal.baseScenarioMaintCostHighSum / 100.0, 0) * 100);
                myRow[2].addText("$ " + tempCostLow + " - " + "$ " + tempCostHigh);//base maintenance cost high

                //populate cost difference column
                tempCostDiffLow = String.Format("{0:#,###,###.##}", Math.Round(((Double)oTotal.currentScenarioMaintCostLowSum - (Double)oTotal.baseScenarioMaintCostLowSum) / 100.0, 0) * 100);
                tempCostDiffHigh = String.Format("{0:#,###,###.##}", Math.Round(((Double)oTotal.currentScenarioMaintCostHighSum - (Double)oTotal.baseScenarioMaintCostHighSum) / 100.0, 0) * 100);
                myRow[3].addText("$ " + tempCostDiffLow + " - " + "$ " + tempCostDiffHigh);//cost difference
            }
            myTable.addRow(myRow);

            myPage.addTable(myTable);
        }

        //writes capital and maintenance cost estimates summary to the pdf report
        public void WriteCostSummaryPage(string siteName, dynamic costModuleResults)
        {
            pdfPage myPage = myDoc.addPage(predefinedPageSize.csSharpPDFFormat);
            int width = myPage.width - leftMargin - rightMargin;
            WritePageTitle(myPage, "Estimate of Probable Costs", siteName);
            WriteCapitalCostSummaryTable(myPage, "Capital Costs", 4, costModuleResults);
            WriteMaintCostSummaryTable(myPage, "Maintenance Costs", 17, costModuleResults);
            WriteFooter(myPage);
        }

        public void CreatePDF(string pdfFileName)
        {
            try
            {
                myDoc.createPDF(pdfFileName);
            }
            catch
            {
                MessageBox.Show("Could not write results to PDF file." +
                    System.Environment.NewLine +
                    "Make sure file is not currently being viewed.", "PDF Error");
            }
        }
    }
}
