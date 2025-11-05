using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.IO
{
    public class CSVFileFormatterTests
    {
        [Fact]
        [Trait("Prana.Utilities", "CSVFileFormatter")]
        public void CreateFile_PassNullData_ReturnsException()
        {
            // Arrange
            CSVFileFormatter formatter = new CSVFileFormatter();

            List<string> DataToEnterInCSV = new List<string>();
            List<string> summary = null;
            string filePath = "";
            Dictionary<string, string> columnsWithSpecifiedNames = null;

            // Act
            Action action = () => formatter.CreateFile(DataToEnterInCSV, summary, filePath, columnsWithSpecifiedNames);

            // Assert
            Assert.Throws<Exception>(action);
        }

        [Fact]
        [Trait("Prana.Utilities", "CSVFileFormatter")]
        public void CreateFile_PassNullValues_ReturnsTrueAndCreateCSV()
        {
            try
            {
                // Arrange
                CSVFileFormatter formatter = new CSVFileFormatter();

                List<TestInfo> DataToEnterInCSV = new List<TestInfo>() {
                    new TestInfo() {Symbol="AAPL",MarkPrice=1,AUECID=1,AUECIdentifier="101" },
                    new TestInfo() {Symbol="AAPL",MarkPrice=1,AUECID=1,AUECIdentifier="101" },
                    new TestInfo() {Symbol="AAPL",MarkPrice=1,AUECID=1,AUECIdentifier="101" },
                    new TestInfo() {Symbol="AAPL",MarkPrice=1,AUECID=1,AUECIdentifier="101" }
                };
                List<string> summary = null;
                string csvpath = MockDataPath.GetFilePath("TestingCsvFile.csv");
                Dictionary<string, string> columnsWithSpecifiedNames = null;

                // Act
                var result = formatter.CreateFile(DataToEnterInCSV, summary, csvpath, columnsWithSpecifiedNames);

                if (System.IO.File.Exists(csvpath) && result)
                {
                    Assert.True(true);
                    System.IO.File.Delete(csvpath);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            
        }
    }
    [Serializable]
    public class TestInfo
    {
        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private double _markPrice = 0;

        public double MarkPrice
        {
            get { return _markPrice; }
            set { _markPrice = value; }
        }

        private int _auecID;

        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private string _auecIdentifier;

        public string AUECIdentifier
        {
            get { return _auecIdentifier; }
            set { _auecIdentifier = value; }
        }
    }
}
