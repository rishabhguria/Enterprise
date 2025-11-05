using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCasesMergerUtililty.HelperClasses;
using TestCasesMergerUtililty.IAdapter;

namespace TestCasesMergerUtililty
{
    class Program
    {
        static void Main(string[] args)
        {
         
               
                TestDataConstants.RUN_DESCR = args[0];
                TestDataConstants.MASTER_TESTLOG_PATH= args[1];
                TestDataConstants.SVN_REVISION_NUMBER= args[2];
                TestDataConstants.BUILD_NUMBER = args[3];
                TestDataConstants.Release_Version = args[4];
                TestDataConstants.Release_VersionForCharts = args[5];
                if (args.Length > 6)
                {
                    TestDataConstants.DELETEBEFORECOPY = args[6];
 
                }
                TestCaseMergerManager _testCaseManager = new TestCaseMergerManager();

                

        }
    }
}
