using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using Prana.PM.BLL;

namespace Prana.PMLauncher
{
    public partial class SplitterExample : Form
    {
        SelectColumnsItemList items = null;
        public SplitterExample()
        {
            InitializeComponent();
            items = new SelectColumnsItemList();
            items.Add(new SelectColumnsItem());

            selectColumnsItemListBindingSource.DataSource = items;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SelectColumnsItem item = new SelectColumnsItem();
            items.Add(item);
        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            

        }

        private void ultraGrid2_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }
    }
}