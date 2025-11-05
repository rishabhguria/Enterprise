using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nirvana.PMLauncher
{
    public partial class Drag_CloseColumnsTest : Form
    {
        public Drag_CloseColumnsTest()
        {
            InitializeComponent();
            BindGrids();
        }

        private void BindGrids()
        {
            Nirvana.PM.BLL.AllocatedTradesList allocatedTradesListIst = new Nirvana.PM.BLL.AllocatedTradesList();
            Nirvana.PM.BLL.AllocatedTradesList allocatedTradesIst = new Nirvana.PM.BLL.AllocatedTradesList();
            //allocatedTradesIst.AveragePrice = 12;
            //allocatedTradesIst.ID = 1;
            //allocatedTradesIst.Quantity = 100;
            //allocatedTradesIst.Side = "Buy";
            //allocatedTradesIst.Symbol = "MSFT";
            //allocatedTradesListIst.Add(allocatedTradesIst);
            //grdSourceGrid.DataSource = allocatedTradesListIst;

            //Nirvana.PM.BLL.AllocatedTradesList allocatedTradesList2nd = new Nirvana.PM.BLL.AllocatedTradesList();
            //Nirvana.PM.BLL.AllocatedTrades allocatedTrades2nd = new Nirvana.PM.BLL.AllocatedTrades();
            //allocatedTrades2nd.AveragePrice = 15;
            //allocatedTrades2nd.ID = 2;
            //allocatedTrades2nd.Quantity = 200;
            //allocatedTrades2nd.Side = "Sell";
            //allocatedTrades2nd.Symbol = "MSFT";
            //allocatedTradesList2nd.Add(allocatedTrades2nd);
            //grdTargetGrid.DataSource = allocatedTradesList2nd;
        }

        private void grdTargetGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void grdTargetGrid_DragDrop(object sender, DragEventArgs e)
        {
            MessageBox.Show("DragDrop");
        }

        private void grdTargetGrid_DragEnter(object sender, DragEventArgs e)
        {
            MessageBox.Show("DragEnter");
        }

        private void grdTargetGrid_DragLeave(object sender, EventArgs e)
        {
            MessageBox.Show("DragLeave");
        }

        private void grdTargetGrid_DragOver(object sender, DragEventArgs e)
        {
            MessageBox.Show("DragOver");               
        }

        private void grdTargetGrid_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Double Click");
        }

        private void grdTargetGrid_SelectionDrag(object sender, CancelEventArgs e)
        {
            MessageBox.Show("Selection Drag");
        }

        private void grdTargetGrid_MouseUp(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Mouse up");
        }
    }
}
