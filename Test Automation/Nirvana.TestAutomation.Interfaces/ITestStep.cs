using Nirvana.TestAutomation.BussinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Interfaces
{
    public interface ITestStep
    {
        TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName);
    }

    public interface IUIAutomationTestStep
    {
        TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName);
    }

}
