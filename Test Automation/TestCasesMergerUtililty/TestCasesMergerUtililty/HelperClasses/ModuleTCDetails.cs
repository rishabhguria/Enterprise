using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using TestCasesMergerUtililty.IAdaptees;

namespace TestCasesMergerUtililty.HelperClasses
{
    class ModuleTCDetails
    {
        string _moduleName;
        public string ModuleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }

        int _passedTC;
        public int PassedTC
        {
            get { return _passedTC; }
            set { _passedTC = value; }
        }

        int _failedTC;
        public int FailedTC
        {
            get { return _failedTC; }
            set { _failedTC = value; }
        }

        public int NotRunTC
        {
            get { return _totalTC - (_passedTC + _failedTC); }
        }

        int _totalTC;
        public int TotalTC
        {
            get { return _totalTC; }
            set { _totalTC = value; }
        }

        public ModuleTCDetails(string moduleName, int passedTC = 0, int failedTC = 0, int totalTC = 0)
        {
            _moduleName = moduleName;
            _passedTC = passedTC;
            _failedTC = failedTC;
            _totalTC = totalTC;
        }
    }
}
