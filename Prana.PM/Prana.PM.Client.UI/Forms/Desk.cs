//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using Prana.Interfaces;
//using Prana.BusinessObjects;

//namespace Prana.PM.Client.UI.Forms
//{
//    public partial class Desk : Form
//    {
//        public Desk(CompanyUser presentLoginUser)
//        {
//            InitializeComponent();
//            CurrentLoginUser = presentLoginUser;
//        }

//        private void Desk_Load(object sender, EventArgs e)
//        {
//            ctrlSendToDesk1.InitControl(CurrentLoginUser);
//        }

//        ICommunicationManager _communicationManager = null;
//        public ICommunicationManager CommunicationManagerInstance
//        {
//            set
//            {
//                _communicationManager = value;
//                ctrlSendToDesk1.CommunicationManagerInstance = _communicationManager;
//            }
//        }

//        private CompanyUser _currentLoginUser = new CompanyUser();

//        public CompanyUser CurrentLoginUser
//        {
//            get { return _currentLoginUser; }
//            set 
//            { 
//                _currentLoginUser = value;
//                ctrlSendToDesk1.CurrentLoginUser = _currentLoginUser;
//            }
//        }

//    }
//}