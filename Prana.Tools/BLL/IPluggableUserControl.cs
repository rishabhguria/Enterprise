using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
namespace Prana.Tools
{
    public interface IPluggableUserControl
    {
        DataTable Data
        {
            get;
            set;
        }
        List<string> DisplayedColumnList
        {
            set;
        }
        void BindData();
        void ExportToExcel();
        UserControl Control
        {
            get;
        }
        void SetUp(DataTable dt, string name);
        event EventHandler DataReloaded;
        bool Validate();
        void Reload();
        event EventHandler FilterChanged;
        void ApplyFilters(object sender, EventArgs e);
        void ValueSelected(object sender, EventArgs e);
        string GetSelectedValue(int type);
        event EventHandler SelectedValueChanged;
    }
}
