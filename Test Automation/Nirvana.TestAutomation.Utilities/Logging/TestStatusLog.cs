using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;
using Nirvana.TestAutomation.Utilities;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.FormulaParsing.Utilities;
using Color = System.Drawing.Color;
using System.Net;
using System.Net.Sockets;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Linq;
using Nirvana.TestAutomation.BussinessObjects.Definitions;
using System.Configuration;

namespace Nirvana.TestAutomation.Utilities
{
    public static class TestStatusLog
    {
        ///// <summary>
        ///// Prevents a default instance of the <see cref="TestStatusLog"/> class from being created.
        ///// </summary>
        //private TestStatusLog()
        //{

        //}
        /// <summary>
        /// The save logs MSG file
        /// </summary>
        private static string _saveLogsMsgFile = String.Empty;
        /// <summary>
        /// The document
        /// </summary>
        private static XmlDocument _document = null;
        /// <summary>
        /// The root node
        /// </summary>
        private static XmlNode _rootNode = null;
        /// <summary>
        /// The excelfile
        /// </summary>
        private static string _excelfile = string.Empty;
        /// <summary>
        /// Initializes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        //static bool IsAllowCopyLOGTrue = false;
        public static void Initialize(String file)
        {

            try
            {
                _saveLogsMsgFile = file;
                _document = new XmlDocument();
                _excelfile = file.Replace("xml", "xlsx");
                if (!File.Exists(_excelfile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_excelfile));
                    CreateExcelReport();
                }
                if (File.Exists(_saveLogsMsgFile))
                {
                    _document.Load(_saveLogsMsgFile);
                    _rootNode = _document.GetElementsByTagName("LogEntries")[0];
                }
                else
                {
                    _document.AppendChild(_document.CreateProcessingInstruction("xml-stylesheet", "type='text/xsl' href='ReportViewer.xsl'"));
                    _rootNode = _document.CreateElement("LogEntries");
                    _document.AppendChild(_rootNode);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public static int _startTimeFlag = 1;

        /// <summary>
        /// Tests the case result.
        /// </summary>
        /// <param name="testCaseId">The test case identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="error">The error.</param>
        /// <param name="category">The category.</param>
        /// <param name="timeElapsed">The time elapsed.</param>
        public static void TestCaseResult(String moduleName, String testCaseId, String description, String error, String category, TimeSpan timeElapsed, int flag = 0)
        {
            try
            {
                ResultStatus resultStatus;
                if (String.IsNullOrEmpty(error))
                    resultStatus = ResultStatus.Pass;
                else if (error.StartsWith(MessageConstants.LOGIN_ERROR) || error.StartsWith(MessageConstants.STARTUP_ERROR))
                    resultStatus = ResultStatus.NotRun;
                else
                    resultStatus = ResultStatus.Fail;
                if (!String.IsNullOrEmpty(error) && ApplicationArguments.RetryCount < ApplicationArguments.RetrySize)
                {
                    if (ApplicationArguments.TestCasesDictionary[ApplicationArguments.Workbook][ApplicationArguments.SheetName].Contains(testCaseId))
                    {
                        ApplicationArguments.TestCasesDictionary[ApplicationArguments.Workbook][ApplicationArguments.SheetName].Add(testCaseId);
                    }
                }
                else if (CheckStartUpStatus(description))
                {
                    int testCaseWeight = 1;
                    if (ApplicationArguments.TestCaseWeightDict.ContainsKey(testCaseId))
                        testCaseWeight = ApplicationArguments.TestCaseWeightDict[testCaseId];

                    
                    // For Retry test cases Test case id shown with asterisk mark
                    if (ApplicationArguments.RetryCount >= ApplicationArguments.RetrySize)
                    {
                        if (ApplicationArguments.RetrySize != 0)
                        {
                            testCaseId = "*" + testCaseId;
                        }
                    }
                    //ExcelPackage x = new ExcelPackage(new FileInfo(@"\\192.168.2.108\DistributedAutomation_SlaveWithWrite\TestAutomationDev\TestResults_Current\2_94\TestLogs\Dev (Distributed)_2_94-2022-10-06_07-55-00-PM.xlsx"));
                    using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(_excelfile)))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[TestDataConstants.CAP_TEST_REPORT];
                        //ExcelWorksheet worksheet1 = xlPackage.Workbook.Worksheets[0];
                        worksheet.Cells[14, 3].Value = String.Format("{0:T}", DateTime.Now);
                        int rowCount = worksheet.Dimension.Rows;
                        rowCount = rowCount + 3;//for blank rows
                        string specialChar = String.Empty;
                        XmlNode logEntry = _document.CreateElement("LogEntry");
                        if (worksheet.Cells[rowCount - 1, 3].Value != null && worksheet.Cells[rowCount - 1, 5].Value.ToString() != string.Empty && testCaseId.Equals(worksheet.Cells[rowCount - 1, 2].Value.ToString()))
                        {
                            rowCount = rowCount - 1;
                            specialChar = "**";
                            //worksheet.Cells[7, 3].Value = int.Parse(worksheet.Cells[7, 3].Value.ToString()) - 1;
                            switch(resultStatus)
                            {
                                case ResultStatus.NotRun:
                                    break;
                                default:
                                    worksheet.Cells[9, 3].Value = int.Parse(worksheet.Cells[9, 3].Value.ToString()) - testCaseWeight;
                                    break;
                            }
                        }

                        XmlAttribute attribute = _document.CreateAttribute("moduleName");
                        attribute.Value = moduleName;
                        logEntry.Attributes.Append(attribute);
                       //Akash-TO HANDLE PARALLEL SHEETS during report log
                       string moduleNamer= removeDigitsFromModule(moduleName);
                       try
                       {
                           string moduleNameChecker = testCaseId.Substring(0, testCaseId.IndexOf("-"));
                           if (moduleNamer.Contains(moduleNameChecker))
                               worksheet.Cells[rowCount, 2].Value = moduleNameChecker;
                           else
                               worksheet.Cells[rowCount, 2].Value = moduleNamer;
                       }
                       catch { }

                        attribute.Value = testCaseId;
                        logEntry.Attributes.Append(attribute);
                        worksheet.Cells[rowCount, 3].Value = testCaseId;

                        attribute = _document.CreateAttribute("Description");
                        attribute.Value = description;
                        logEntry.Attributes.Append(attribute);
                        worksheet.Cells[rowCount, 4].Value = specialChar + description;
                        worksheet.Cells[rowCount, 4].Style.WrapText = true;
                        worksheet.Cells[rowCount, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        attribute = _document.CreateAttribute("Category");
                        attribute.Value = category;
                        logEntry.Attributes.Append(attribute);
                        worksheet.Cells[rowCount, 5].Value = category;

                        attribute = _document.CreateAttribute("Error");
                        attribute.Value = String.IsNullOrEmpty(error) ? "" : error;
                        logEntry.Attributes.Append(attribute);
                        worksheet.Cells[rowCount, 6].Value = String.IsNullOrEmpty(error) ? "" : error;
                        worksheet.Cells[rowCount, 6].Style.WrapText = true;
                        worksheet.Cells[rowCount, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        attribute = _document.CreateAttribute("Result");
                        attribute.Value = resultStatus.ToString();
                        logEntry.Attributes.Append(attribute);
                        worksheet.Cells[rowCount, 7].Value = EnumHelper.GetDescription(resultStatus);
                        worksheet.Cells[rowCount, 8].Value = timeElapsed.ToString("mm\\:ss");
                        
                        int failed = int.Parse(worksheet.Cells[9, 3].Value.ToString());
                        int passed = int.Parse(worksheet.Cells[8, 3].Value.ToString());
                        worksheet.Row(rowCount).Style.Fill.PatternType = ExcelFillStyle.Solid;


                        var range = worksheet.Cells[rowCount, 2, rowCount, 8];
                        if (ConfigurationManager.AppSettings["MemberName"].ToString().ToLower().Equals("true") && ApplicationArguments.MemberPath.ToString() != "")
                        { 
                            worksheet.Cells[rowCount, 9].Value = ApplicationArguments.MemberPath;
                            range = worksheet.Cells[rowCount, 2, rowCount, 9];
                        }
                        TimeSpan tim = TimeSpan.Parse(worksheet.Cells[11, 3].Value.ToString());
                      //  worksheet.Cells[11, 3].Value = tim.Add(timeElapsed).ToString("hh\\:mm\\:ss");
                        TimeSpan StartTime = DateTime.Parse(worksheet.Cells[13, 3].Value.ToString()).TimeOfDay;
                        TimeSpan EndTime = DateTime.Parse(worksheet.Cells[14, 3].Value.ToString()).TimeOfDay;
                       var FinalTime = StartTime.Subtract(EndTime).Duration();
                       worksheet.Cells[11, 3].Value = FinalTime.ToString();
                        bool isnum = false;
                        isnum = worksheet.Cells[8, 3].Value.IsNumeric();
                        worksheet.Cells[7, 3].Value = int.Parse(worksheet.Cells[7, 3].Value.ToString()) + 1;
                        switch (resultStatus)
                        {
                            case ResultStatus.Pass:
                                passed += testCaseWeight;
                                worksheet.Cells[8, 3].Value = passed;
                                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Green);
                                range.Style.Fill.BackgroundColor.SetColor(Color.PaleGreen);
                                break;
                            case ResultStatus.NotRun:
                                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Yellow);
                                range.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                                break;
                            default:
                                failed += testCaseWeight;
                                worksheet.Cells[9, 3].Value = failed;
                                range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Red);
                                range.Style.Fill.BackgroundColor.SetColor(Color.MistyRose);
                                break;
                        }
                        //worksheet.Cells[7, 3].Value = ApplicationArguments.TestCasesDictionary.Values.Select(x => x.Values.SelectMany(y => y)).Count();
                        worksheet.Cells[7, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        xlPackage.Save();
                        _rootNode.AppendChild(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Creates the excel report.
        /// </summary>
        private static void CreateExcelReport()
        {
            try
            {
                var op = new FileInfo(_excelfile);
                // Custom Handling to find the correct description of the report - Karan
                int dashIndex = ApplicationArguments.RunDescription.IndexOf('-');
                using (var package = new ExcelPackage(op))
                {
                    var excelWorksheet = package.Workbook.Worksheets.Add(TestDataConstants.CAP_TEST_REPORT);
                    string includeMember = ConfigurationManager.AppSettings["MemberName"];

                    #region setting column headers
                    excelWorksheet.Cells[5, 2, 15, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    excelWorksheet.Cells[5, 2, 15, 2].Style.Font.Bold = true;
                    excelWorksheet.Cells[5, 2, 15, 3].Style.Font.Size = 12;
                    excelWorksheet.Cells[5, 2, 15, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    excelWorksheet.Cells[3, 2, 3, 3].Style.Font.Bold = true;
                    excelWorksheet.Cells[3, 2, 3, 3].Style.Font.Size = 14;
                    excelWorksheet.Cells[3, 2].Value = ApplicationArguments.RunDescription.Substring(0, dashIndex) + " Test Report ";
                    excelWorksheet.Cells[5, 2].Value = TestDataConstants.CAP_PRANA_VERSION;
                    excelWorksheet.Cells[6, 2].Value = TestDataConstants.CAP_RELEASE_DATE_TIME;
                    excelWorksheet.Cells[6, 3].Value = DateTime.Now.ToString();
                    excelWorksheet.Cells[7, 2].Value = TestDataConstants.CAP_TOTAL_TEST_CASES;
                    //excelWorksheet.Cells[7, 3].Value = ApplicationArguments.TestCasesDictionary.Values.SelectMany(x => x.Values.SelectMany(y => y)).Sum(tc => ApplicationArguments.TestCaseWeightDict[tc]);
                    excelWorksheet.Cells[7, 3].Value = 0;
                    excelWorksheet.Cells[8, 2].Value = TestDataConstants.CAP_PASSED;
                    excelWorksheet.Cells[8, 3].Value = 0;
                    excelWorksheet.Cells[9, 2].Value = TestDataConstants.CAP_FAILED;
                    excelWorksheet.Cells[9, 3].Value = 0;
                    excelWorksheet.Cells[10, 2].Value = TestDataConstants.NOT_RUN;
                    excelWorksheet.Cells[10, 3].Formula = "=C7-C8-C9";
                    excelWorksheet.Cells[11, 2].Value = TestDataConstants.CAP_TOTAL_TIME_TAKEN;
                    excelWorksheet.Cells[11, 3].Value = new TimeSpan(0, 0, 0, 0).ToString();
                    excelWorksheet.Cells[12, 2].Value = TestDataConstants.CAP_SYSTEM_IP;
                    excelWorksheet.Cells[12, 3].Value = GetSytemIP().ToString();
                    excelWorksheet.Cells[13, 2].Value = TestDataConstants.TestCase_Start_Time;
                    excelWorksheet.Cells[13, 3].Value = String.Format("{0:T}", DateTime.Now);
                    excelWorksheet.Cells[14, 2].Value = TestDataConstants.TestCase_End_Time;
                    excelWorksheet.Cells[15, 2].Value = TestDataConstants.CAP_AUTOMATION_CODE_REVISION;
                    string testAutomationPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\Test Automation "));
                    if (testAutomationPath != null)
                    {
                        excelWorksheet.Cells[15, 3].Value = GetRevision(testAutomationPath);
                        // excelWorksheet.Cells[14, 2].Value = TestDataConstants.CAP_PRANA_CODE_REVISION;
                        string DevPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\.."));
                        excelWorksheet.Cells[5, 3].Value = ApplicationArguments.RunDescription.Substring(0, dashIndex) + "." + ((DevPath != null) ? GetRevision(DevPath) : 0).ToString();
                    }
                    excelWorksheet.Cells[17, 2].Value = TestDataConstants.CAP_MODULE_NAME;
                    excelWorksheet.Cells[17, 3].Value = TestDataConstants.CAP_TEST_CASE_ID;
                    excelWorksheet.Cells[17, 4].Value = TestDataConstants.CAP_TEST_CASE_DESCRIPTION;
                    excelWorksheet.Cells[17, 5].Value = TestDataConstants.CAP_CATEGORY;
                    excelWorksheet.Cells[17, 6].Value = TestDataConstants.CAP_ERROR;
                    excelWorksheet.Cells[17, 7].Value = TestDataConstants.CAP_RESULT;
                    excelWorksheet.Cells[17, 8].Value = TestDataConstants.CAP_RUNNING_TIME;
                    if (includeMember.ToLower().Equals("true") && ApplicationArguments.MemberPath.ToString() != "")
                    {
                        excelWorksheet.Cells[17, 9].Value = TestDataConstants.COL_MEMBER;
                        excelWorksheet.Column(9).AutoFit();
                    }

                    excelWorksheet.Column(2).AutoFit();
                    excelWorksheet.Column(3).AutoFit();
                    excelWorksheet.Column(4).Width = 40.0;
                    excelWorksheet.Column(5).Width = 20;
                    excelWorksheet.Column(6).Width = 40.0;
                    excelWorksheet.Column(8).AutoFit();

                    int columnCount= DataUtilities.GetLastUsedColumn(excelWorksheet); ;

                    for (var i = 2; i <= columnCount; i++)//
                    {
                        excelWorksheet.Column(i).Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    }

                    var range = excelWorksheet.Cells[17, 2, 17, columnCount];
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Brown);
                    range.Style.Font.Color.SetColor(Color.WhiteSmoke);
                    range.Style.Locked = true;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.ShrinkToFit = false;

                    #endregion

                    //Code for Pie chart
                    var chart = (ExcelPieChart)excelWorksheet.Drawings.AddChart("Test Summary", eChartType.Pie3D);
                    chart.Fill.Color = Color.LightSteelBlue;
                    chart.Border.Fill.Color = Color.LightSkyBlue;
                    chart.PlotArea.Fill.Color = Color.LightSteelBlue;
                    chart.Border.Width = 3;
                    chart.Title.Text = TestDataConstants.CAP_TEST_SUMMARY;
                    chart.SetPosition(0, 0, 3, 4);
                    chart.SetSize(570, 200);
                    chart.Series.Add(excelWorksheet.Cells[8, 3, 10, 3], excelWorksheet.Cells[8, 2, 10, 2]);
                    chart.Legend.Border.LineStyle = eLineStyle.Solid;
                    chart.Legend.Border.Fill.Style = eFillStyle.SolidFill;
                    chart.Legend.Border.Fill.Color = Color.DarkBlue;
                    chart.DataLabel.ShowPercent = true;
                    chart.DataLabel.ShowLeaderLines = true;

                    //Code to set different color of segments in pie chart
                    //Get the nodes
                    var ws = chart.WorkSheet;
                    var nsm = ws.Drawings.NameSpaceManager;
                    var nschart = nsm.LookupNamespace("c");
                    var nsa = nsm.LookupNamespace("a");
                    var node = chart.ChartXml.SelectSingleNode(TestDataConstants.PIE_PATH, nsm);
                    var doc = chart.ChartXml;

                    SetPieChartSegmentColor(doc, nschart, 0, node, nsa, Color.ForestGreen);
                    SetPieChartSegmentColor(doc, nschart, 1, node, nsa, Color.Red);
                    SetPieChartSegmentColor(doc, nschart, 2, node, nsa, Color.Yellow);
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the color of the pie chart segment.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="nschart">The nschart.</param>
        /// <param name="i">The i.</param>
        /// <param name="node">The node.</param>
        /// <param name="nsa">The nsa.</param>
        /// <param name="color">The color.</param>
        private static void SetPieChartSegmentColor(XmlDocument doc, string nschart, int i, XmlNode node, string nsa, Color color)
        {
            try
            {
                //Add the node
                var dPt = doc.CreateElement("dPt", nschart);
                var idx = dPt.AppendChild(doc.CreateElement("idx", nschart));
                var valattrib = idx.Attributes.Append(doc.CreateAttribute("val"));
                valattrib.Value = i.ToString(CultureInfo.InvariantCulture);
                node.AppendChild(dPt);

                //Add the solid fill node
                var spPr = doc.CreateElement("spPr", nschart);
                var solidFill = spPr.AppendChild(doc.CreateElement("solidFill", nsa));
                var srgbClr = solidFill.AppendChild(doc.CreateElement("srgbClr", nsa));
                valattrib = srgbClr.Attributes.Append(doc.CreateAttribute("val"));

                //Set the color
                var colorPie = Color.FromArgb(color.ToArgb());
                valattrib.Value = ColorTranslator.ToHtml(colorPie).Replace("#", String.Empty);
                dPt.AppendChild(spPr);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Publishes the log.
        /// </summary>
        public static void PublishLog()
        {
            try
            {
                if (!File.Exists(_saveLogsMsgFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_saveLogsMsgFile));
                    var fs = File.CreateText(_saveLogsMsgFile);
                    fs.Close();
                }

                _document.Save(_saveLogsMsgFile);
                String targetFolder = Path.GetDirectoryName(_saveLogsMsgFile);
                //System.IO.File.Copy(@"html\ReportViewer.xslt", Path.Combine(targetFolder, "ReportViewer.xslt"), true);
                //System.IO.File.Copy(@"html\ReportViewer.xsl", Path.Combine(targetFolder, "ReportViewer.xsl"), true);
                //System.IO.File.Copy(@"html\gstatic_charts.js", Path.Combine(targetFolder, "gstatic_charts.js"), true);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Stop to write application start up and login to the applicatoin in the excel file.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Boolean CheckStartUpStatus(String description)
        {
            try
            {
                if (description.CompareTo(MessageConstants.APPLICATION_START_UP) == 0 || description.CompareTo(MessageConstants.LOGIN_TO_APPLICATION) == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return true;
        }

        /// <summary>
        /// Gets the sytem ip.
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetSytemIP()
        {
            try
            {
                string hostname = Dns.GetHostName();
                IPHostEntry ipHostEntry = Dns.GetHostEntry(hostname);
                foreach (var addr in ipHostEntry.AddressList)
                {
                    if (addr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return addr;
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }
        public static long GetRevision(string REPO_PATH)
        {
            try
            {
                //using (SvnClient client = new SvnClient())
                //{
                //    SvnInfoEventArgs info;
                //    SvnPathTarget path = SvnPathTarget.FromString(REPO_PATH);
                //    if (path == null ||client==null)
                //        return 0;
                //    else
                //        return client.GetInfo(path, out info) ? info.Revision : 0;
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return 0;
        }
        public static string removeDigitsFromModule(string moduleName)
        {
            string onlyString = string.Empty;
            int count = moduleName.Count(char.IsDigit);
           // string modularName = string.Join("", moduleName.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));//to remove space inbetween
            if (count > 0)
            {

                onlyString = new string(moduleName.Where(c => !char.IsDigit(c)).ToArray());
                //onlyString = onlyString.Remove(onlyString.Length - 1);
            }
            else
                onlyString = moduleName;
            return onlyString;
        }
    }
}
