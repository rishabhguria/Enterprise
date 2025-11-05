using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Interfaces
{
    public interface ITestDataProvider
    {
        DataSet GetTestData(String filePath, int rowHeaderIndex = 1, int startColumnFrom = 1, string fileType = "");
    }
}
