//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using Prana.PM.BLL;
//using Prana.PM.DAL;

//using Prana.BusinessObjects;

//namespace Prana.PM.Client.UI.Forms
//{
//    public partial class CounterParty : Form
//    {
//        int _userID;

//        //private int myVar;
//        private CompanyUser _companyUser;


//        /// <summary>
//        /// Initializes a new instance of the <see cref="CounterParty"/> class.
//        /// </summary>
//        public CounterParty()
//        {
//            InitializeComponent();
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="CounterParty"/> class.
//        /// </summary>
//        /// <param name="userID">The user ID.</param>
//        public CounterParty(int userID)
//        {
//            this._userID = userID;
//            InitializeComponent();
//        }

//        /// <summary>
//        /// An overloaded constructor gets the login user in parameter
//        /// </summary>
//        /// <param name="loginUser"> Value of login user</param>
//        public CounterParty(CompanyUser loginUser)
//        {
//            _companyUser = loginUser;
//            InitializeComponent();
//        }

//        private BindingList<Prana.BusinessObjects.CounterParty> _counterParties = new BindingList<Prana.BusinessObjects.CounterParty>();

//        /// <summary>
//        /// Handles the Load event of the CounterParty control.
//        /// </summary>
//        /// <param name="sender">The source of the event.</param>
//        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
//        private void CounterParty_Load(object sender, EventArgs e)
//        {
//            _counterParties = CreatePositionManager.GetCounterParties(_userID);
//            optionSetCounterParties.DisplayMember = "Name";
//            optionSetCounterParties.ValueMember = "CounterPartyID";
//            optionSetCounterParties.DataSource = _counterParties; //CreateSourceColumns(_mapAccounts.CompanyNameID.ID, _mapAccounts.DataSourceNameID.ID);          
//            //Utils.UltraDropDownFilter(cmbCounterParty, "Name");

//            ctrlOptionBooks1.SetupControl(_companyUser);


//        }


//        private Prana.BusinessObjects.CounterParty selectedCounterParty = new Prana.BusinessObjects.CounterParty(int.MinValue, Global.ApplicationConstants.C_COMBO_SELECT);
//        /// <summary>
//        /// Gets the get selected counter party.
//        /// </summary>
//        /// <value>The get selected counter party.</value>
//        public Prana.BusinessObjects.CounterParty GetSelectedCounterParty
//        {
//            get 
//            {
//                return selectedCounterParty; 
//            }
//        }


//        /// <summary>
//        /// Handles the ValueChanged event of the optionSetCounterParties control.
//        /// </summary>
//        /// <param name="sender">The source of the event.</param>
//        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
//        private void optionSetCounterParties_ValueChanged(object sender, EventArgs e)
//        {
//            if (optionSetCounterParties.CheckedItem != null)
//            {
//                selectedCounterParty = (Prana.BusinessObjects.CounterParty)optionSetCounterParties.CheckedItem.ListObject;
//                this.Close();
//            }
//        }
//    }
//}