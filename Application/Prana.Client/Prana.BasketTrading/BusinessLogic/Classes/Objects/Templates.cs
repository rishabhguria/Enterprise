using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace Prana.BasketTrading
{
    class Templates
    {

        private ArrayList  _availableColumns = null ;
        private ArrayList  _selectedColumns = null;

        private void SaveTemplateForm()
        {

        }
        public ArrayList AvailableColumns
        {
            //BasketTradingConstants
            get { return _availableColumns; }

        }
        public ArrayList  SelectedColumns
        {
            get { return _selectedColumns; }
            set { _selectedColumns = value; }

        }
        public void AddSelectedColumn(string name, int index)
        {
            _selectedColumns.Insert(index, name);

        }
    }
}
