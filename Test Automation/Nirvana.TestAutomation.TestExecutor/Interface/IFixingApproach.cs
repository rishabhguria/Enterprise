using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.TestExecutor
{
    public interface IFixingApproach
    {
        void Execute(
            DataSet originalTestCaseDataSet,
            DataSet createdDataSet,
            Dictionary<string, List<string>> stepxDetails,
            Dictionary<string, List<string>> stepxUpdatedColumns,
            string testCaseID
        );
    }
}
