using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    class ViewAllocationDetailsSubOrder : BlotterUIMap, ITestStep
    {
        public bool RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            PranaApplication.BringToFrontOnAttach = false;
            Wait(10000);
            if (testData != null)
            {
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    InputEnter(dr);
                }
            }

            return true;
        }
        private void InputEnter(DataRow dr)
        {
            try
            {
                var msaaObj = DgBlotter2.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter2.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());                
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                DgBlotter2.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                if(ViewAllocationDetails1.IsVisible) ViewAllocationDetails1.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
