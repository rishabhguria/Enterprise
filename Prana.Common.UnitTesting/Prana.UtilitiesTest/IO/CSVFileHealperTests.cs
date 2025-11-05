using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.IO
{
    public class CSVFileHealperTests
    {
        [Fact]
        [Trait("Prana.Utilities", "CSVFileHealper")]
        public void ProduceCSV_PassDataWithHeader_CreateFileProduceCSV1()
        {
            try
            {
                // Arrange
                DataTable dt = CreateDataTable.GetTable();
                DataSet dsSummary = null;
                string csvpath = MockDataPath.GetFilePath("ProduceCSV1.csv");
                bool WriteHeader = true;

                // Act
                CSVFileHealper.ProduceCSV(dt, dsSummary, csvpath, WriteHeader);

                // Assert
                if (System.IO.File.Exists(csvpath))
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

        [Fact]
        [Trait("Prana.Utilities", "CSVFileHealper")]
        public void ProduceCSV_PassDataWithoutHeader_CreateFileProduceCSV2()
        {
            try
            {
                // Arrange
                DataTable dt = CreateDataTable.GetTable();
                DataSet dsSummary = null;
                string csvpath = MockDataPath.GetFilePath("ProduceCSV2.csv");
                bool WriteHeader = false;

                // Act
                CSVFileHealper.ProduceCSV(dt, dsSummary, csvpath, WriteHeader);

                // Assert
                if (System.IO.File.Exists(csvpath))
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
}
