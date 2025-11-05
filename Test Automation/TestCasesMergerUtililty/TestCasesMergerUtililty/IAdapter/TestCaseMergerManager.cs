using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCasesMergerUtililty.HelperClasses;
using TestCasesMergerUtililty.IAdaptees;

namespace TestCasesMergerUtililty.IAdapter
{
    class TestCaseMergerManager
    {
       private MergeGoogleSheetWorker _mergeWorkSheetWorker;

        public TestCaseMergerManager()
        {
            //CopyOutputFilesFromSlave();
            _mergeWorkSheetWorker = new MergeGoogleSheetWorker();
            _mergeWorkSheetWorker.MergeWorkSheets();
        }
    }
}
