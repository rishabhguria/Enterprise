using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlAddressDetails : UserControl
    {

        public CtrlAddressDetails()
        {
            InitializeComponent();            
            ResetComboBoxes();
            //PopulateAddressDetails();

            
        }


        #region Expose Binding Properties

        ///TODO : Need to reenable the design time attribute and change from Ilistsource to some class which is derived in normal classes.
        //[AttributeProvider(typeof(IListSource))]
        public object DataSource
        {

            get { return bindingSourceAddressDetails.DataSource; }
            set { bindingSourceAddressDetails.DataSource = value; }
        }

        ///TODO : Need to reenable the design time attribute and change from Ilistsource to some class which is derived in normal classes.
        //[Editor("System.Windows.Forms.Design.DataMemberListEditor,System.Design, Version=2.0.0.0, Culture=neutral,PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string DataMember
        {
            get { return bindingSourceAddressDetails.DataMember; }
            set { bindingSourceAddressDetails.DataMember = value; }
        }

      
	
        #endregion Expose Binding Properties

        #region methods

        /// <summary>
        /// Populates the address details.
        /// </summary>
        public void PopulateAddressDetails()
        {
            this.bindingSourceCountryList.Clear();
            this.bindingSourceCountryList.DataSource = CountryList.Retrieve;
            this.bindingSourceStateList.Clear();
            this.bindingSourceStateList.DataSource = StateList.Retrieve;

            this.txtFaxNumber.DataBindings.Clear();
            this.txtFaxNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceAddressDetails, "FaxNumber", true));
            this.txtWorkNumber.DataBindings.Clear();
            this.txtWorkNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceAddressDetails, "WorkNumber", true));
            this.txtZip.DataBindings.Clear();
            this.txtZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceAddressDetails, "Zip", true));
            this.cmbStateTerritory.DataBindings.Clear();
            this.cmbStateTerritory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSourceAddressDetails, "StateId", true));
            this.cmbCountry.DataBindings.Clear();
            this.cmbCountry.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSourceAddressDetails, "CountryId", true));
            this.txtAddress2.DataBindings.Clear();
            this.txtAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceAddressDetails, "Address2", true));
            this.txtAddress1.DataBindings.Clear();
            this.txtAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceAddressDetails, "Address1", true));
        }

        /// <summary>
        /// Resets the combo boxes.
        /// </summary>
        private void ResetComboBoxes()
        {
            //// Hide Column Header for the Country Combo Box
            //// Only show the Country Name Column
            ColumnsCollection columnsCountry = cmbCountry.DisplayLayout.Bands[0].Columns;
            cmbCountry.DisplayLayout.Bands[0].ColHeadersVisible = false;

            foreach (UltraGridColumn column in columnsCountry)
            {
                if (column.Key != "Name")
                {
                    column.Hidden = true;
                }
            }

           

            //// Hide Column Header for the State Combo Box
            //// Only show the State Column
            ColumnsCollection columnsState = cmbStateTerritory.DisplayLayout.Bands[0].Columns;
            cmbStateTerritory.DisplayLayout.Bands[0].ColHeadersVisible = false;

            foreach (UltraGridColumn column in columnsState)
            {
                if (column.Key != "StateName")
                {

                    column.Hidden = true;
                }
            }

        }

        #endregion 

       
        /// <summary>
        /// Handles the ValueChanged event of the cmbCountry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cmbCountry_ValueChanged(object sender, EventArgs e)
        {
            if (cmbCountry.Value != null)
            {
                int countryID = int.Parse(cmbCountry.Value.ToString());
                this.bindingSourceStateList.Clear();
                if (countryID > 0)
                {                    
                    SortableSearchableList<State> sourceCollection = new SortableSearchableList<State>();
                    SortableSearchableList<State> statesForCountry = StateList.Retrieve;
                    foreach (State state in statesForCountry)
                    {
                        if (state.CountryID == countryID)
                        {
                            sourceCollection.Add(state);
                        }
                    }
                    sourceCollection.Insert(0, new State(0, "--Select--", 0));
                    this.bindingSourceStateList.DataSource = sourceCollection;
                  //  this.cmbStateTerritory.DataBindings.Clear();
                }      

            }
        }
    }
}
