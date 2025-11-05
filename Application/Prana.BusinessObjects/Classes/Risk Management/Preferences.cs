using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class Preferences
    {
        private List<string> _columns;

        public List<string> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public Preferences()
        {
            //Columns = new List<string>();
            //Columns.Clear();
            //Columns.Add("AssetName");
            //Columns.Add("AvgPrice");
            //Columns.Add("CumQty");  
        }
    }
}
