//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using Prana.PM.BLL;
//using Prana.BusinessObjects;

//namespace Prana.PM.Client.UI.Forms
//{
//    public partial class OptionSimulator : Form
//    {
//        public OptionSimulator()
//        {
//            InitializeComponent();
//        }

//        private CompanyUser _companyUser = new CompanyUser();
//        public OptionSimulator(CompanyUser companyUser)
//        {
//            InitializeComponent();
//            _companyUser = companyUser;
//            ctrlOptionBooks1.SetupControl(_companyUser);
//        }
//        private AddPositions frmAddPositions;

//        /// <summary>
//        /// Handles the Paint event of the tableLayoutPanel1 control.
//        /// </summary>
//        /// <param name="sender">The source of the event.</param>
//        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
//        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
//        {

//        }

//        /// <summary>
//        /// Handles the Load event of the ctrlOptionBooks1 control.
//        /// </summary>
//        /// <param name="sender">The source of the event.</param>
//        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
//        private void ctrlOptionBooks1_Load(object sender, EventArgs e)
//        {

//        }

//        private void btnImportPositions_Click(object sender, EventArgs e)
//        {
//            if (frmAddPositions == null)
//            {
//                frmAddPositions = new AddPositions();
//                frmAddPositions.Owner = this;
//                frmAddPositions.ShowInTaskbar = false;
//            }

//            //Hardcoded for now.
//            int companyID = 1;
//            int userID = 1;
//            frmAddPositions.SetupForm(companyID, userID);
//            frmAddPositions.Show();
//            frmAddPositions.Activate();
//            frmAddPositions.Disposed += new EventHandler(frmAddPositions_Disposed);
//        }

//        /// <summary>
//        /// Handles the Disposed event of the frmAddPositions form.
//        /// </summary>
//        /// <param name="sender">The source of the event.</param>
//        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
//        private void frmAddPositions_Disposed(object sender, EventArgs e)
//        {
//            if (frmAddPositions.IsAddButtonClicked == true)
//            {
//                OTCPositionList existingPositions = frmAddPositions.ExistingPositionsList;
//                OTCPositionList newAddedPositions = frmAddPositions.NewAddedPositionsList;
//                OTCPositionList totalPositions = new OTCPositionList();

//                //CreatePositionManager.SaveSimulatedPositions(newAddedPositions);
//                foreach (OTCPosition addPosition in existingPositions)
//                {
//                    totalPositions.Add(addPosition);
//                }
//                foreach (OTCPosition addPositionNew in newAddedPositions)
//                {
//                    totalPositions.Add(addPositionNew);
//                }
//                ctrlOptionBooks1.Positions = totalPositions;
//            }
//            frmAddPositions = null;
//        }
//    }
//}