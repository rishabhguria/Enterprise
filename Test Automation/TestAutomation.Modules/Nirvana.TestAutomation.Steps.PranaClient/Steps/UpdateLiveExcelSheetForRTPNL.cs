using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Configuration;
using System.IO;
using OfficeOpenXml;

namespace Nirvana.TestAutomation.Steps.PranaClient
{
    class UpdateLiveExcelSheetForRTPNL : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            string filePath = ConfigurationManager.AppSettings["SamsaraDirectory"] + @"\public\config\live-excel-config.json";
            string newJson = @"
            [
              {
                ""userID"": 35,
                ""excelMapping"": [
                  {
                    ""excelName"": ""Account"",
                    ""widgetType"": ""Account"",
                    ""excelLocation"": ""E:\\Account.xlsx"",
                    ""sheetName"": ""SheetFund""
                  },
                  {
                    ""excelName"": ""Fund"",
                    ""widgetType"": ""Fund"",
                    ""excelLocation"": ""E:\\Fund.xlsx"",
                    ""sheetName"": ""SheetFund""
                  },
                  {
                    ""excelName"": ""Symbol"",
                    ""widgetType"": ""Symbol"",
                    ""excelLocation"": ""E:\\Symbol.xlsx"",
                    ""sheetName"": ""SheetFund""
                  },
                  {
                    ""excelName"": ""SymbolFund"",
                    ""widgetType"": ""Symbol Fund"",
                    ""excelLocation"": ""E:\\SymbolFund.xlsx"",
                    ""sheetName"": ""SheetFund""
                  },
                  {
                    ""excelName"": ""SymbolAccount"",
                    ""widgetType"": ""Symbol Account"",
                    ""excelLocation"": ""E:\\SymbolAccount.xlsx"",
                    ""sheetName"": ""SheetFund""
                  }
                ]
              },
              {
                ""userID"": 17,
                ""excelMapping"": [
                  {
                    ""excelName"": ""Account"",
                    ""widgetType"": ""Account"",
                    ""excelLocation"": ""E:\\Account.xlsx"",
                    ""sheetName"": ""SheetFund""
                  },
                  {
                    ""excelName"": ""Fund"",
                    ""widgetType"": ""Fund"",
                    ""excelLocation"": ""E:\\Fund.xlsx"",
                    ""sheetName"": ""SheetFund""
                  },
                  {
                    ""excelName"": ""Symbol"",
                    ""widgetType"": ""Symbol"",
                    ""excelLocation"": ""E:\\Symbol.xlsx"",
                    ""sheetName"": ""SheetFund""
                  },
                  {
                    ""excelName"": ""SymbolFund"",
                    ""widgetType"": ""Symbol Fund"",
                    ""excelLocation"": ""E:\\SymbolFund.xlsx"",
                    ""sheetName"": ""SheetFund""
                  },
                  {
                    ""excelName"": ""SymbolAccount"",
                    ""widgetType"": ""Symbol Account"",
                    ""excelLocation"": ""E:\\SymbolAccount.xlsx"",
                    ""sheetName"": ""SheetFund""
                  }
                ]
              }
            ]
            ";
            DataUtilities.UpdateLiveExcel(filePath, newJson);
            createExcel("E:\\Account.xlsx");
            createExcel("E:\\Fund.xlsx");
            createExcel("E:\\SymbolAccount.xlsx");
            createExcel("E:\\SymbolFund.xlsx");
            createExcel("E:\\Symbol.xlsx");
            return _res;
        }

        private void createExcel(string path) 
        {
            using (var package = new ExcelPackage())
            {
                package.Workbook.Worksheets.Add("SheetFund");
                FileInfo fileInfo = new FileInfo(path);
                package.SaveAs(fileInfo);
            }

        }   
    }
}
