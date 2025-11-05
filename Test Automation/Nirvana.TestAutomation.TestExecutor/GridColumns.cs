using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.TestExecutor
{
    public class GridColumns: BindableBase
    {
        
        /// <summary>
        /// Gets or Sets the Workbook value
        /// </summary>
        public string workbook = "Select";
        public string Workbook
        {
            get { return workbook; }
            set { SetProperty(ref workbook, value);}
        }

        /// <summary>
        /// Gets or Sets the Module value
        /// </summary>
        public string modules = "Select";
        public string Modules
        {
            get { return modules; }
            set { SetProperty(ref modules, value); }
        }
        
        /// <summary>
        /// Gets or Sets the Selected Method
        /// </summary>
        public string selectMethod = "Select";
        public string SelectMethod
        {
            get { return selectMethod; }
            set { SetProperty(ref selectMethod, value); }
        }

        /// <summary>
        /// Gets or Sets the TestCase
        /// </summary>
        public string testCasesList = "Select";
        public string TestCases
        {
            get { return testCasesList; }
            set { SetProperty(ref testCasesList, value); }
        }
    }
}