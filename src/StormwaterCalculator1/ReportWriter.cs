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
            myDoc.pageMarker = new pdfPageMarker(left, bottomMargin-lineHeight, predefinedMarkerStyle.csArabic, 
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
            myPage.addText(footer, leftMargin, bottomMargin-lineHeight,
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
