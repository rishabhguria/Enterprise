using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.UIAutomation
{
    public class FocusGridData
    {
        public string WindowAutomationId { get; set; }
        public string ExtractGridData { get; set; }
        public string DataExtractionScriptName { get; set; }
        public string CustomWait { get; set; }
        public string LogAutomationID { get; set; }
        public string GridExtractionType { get; set; }
        public string isGridExtractionTypeAvailable { get; set; }

        public FocusGridData()
        {
            DataExtractionScriptName = ""; // Default value assignment moved to constructor
        }

        public override string ToString()
        {
            return " WindowAutomationId: " + WindowAutomationId +
                   ", ExtractGridData: " + ExtractGridData +
                   ", DataExtractionScriptName: " + DataExtractionScriptName +
                   ",  CustomWait: " + CustomWait +
                   " , LogAutomationID: " + LogAutomationID +
                   " ,GridExtractionType : " + GridExtractionType;
        }
    }

}
