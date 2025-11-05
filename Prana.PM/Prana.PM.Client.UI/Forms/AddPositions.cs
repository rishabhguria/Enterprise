//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using Prana.PM.BLL;

//namespace Prana.PM.Client.UI
//{
//    public partial class AddPositions : Form
//    {
//        public AddPositions()
//        {
//            InitializeComponent();
//        }

//        public void SetupForm(int companyID, int userID)
//        {
//            ctrlAddPositions.SetUpControl(companyID, userID);
//        }

//        private OTCPositionList _addPositionsList = new OTCPositionList();
//        private OTCPositionList _newlyAddedPositionsList = new OTCPositionList();
//        private bool _addButtonClicked = false;
//        private void btnAdd_Click(object sender, EventArgs e)
//        {
//            _addButtonClicked = true;
//            _addPositionsList = ctrlAddPositions.ExistingPositionsList;
//            _newlyAddedPositionsList = ctrlAddPositions.NewPositionsList;
//            if (_newlyAddedPositionsList.IsValid == true)
//            {
//                Dispose(true);
//            }
//            else
//            {
//                MessageBox.Show(this, "Please provide the necessary information for add positions !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        public bool IsAddButtonClicked
//        {
//            get
//            {
//                return _addButtonClicked;
//            }
//        }

//        public OTCPositionList ExistingPositionsList
//        {
//            get
//            {
//                return _addPositionsList;
//            }
//        }

//        public OTCPositionList NewAddedPositionsList
//        {
//            get
//            {
//                if (_newlyAddedPositionsList.IsValid == true)
//                {
//                    return _newlyAddedPositionsList;
//                }
//                else
//                {
//                    _newlyAddedPositionsList = new OTCPositionList();
//                    return _newlyAddedPositionsList;
//                }
//            }
//        }
//    }
//}