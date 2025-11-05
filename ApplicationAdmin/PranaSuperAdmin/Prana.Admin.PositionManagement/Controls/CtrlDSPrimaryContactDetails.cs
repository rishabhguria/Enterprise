using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Design;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlDSPrimaryContactDetails : UserControl
    {
        ///Rajat commented
        //private DataSourcePrimaryContact _primaryContact = new DataSourcePrimaryContact();

        /// <summary>
        /// Gets or sets the primary contact.
        /// </summary>
        /// <value>The primary contact.</value>
        //public DataSourcePrimaryContact PrimaryContact
        //{
        //    get
        //    {
        //        return _primaryContact;

        //    }
        //    set
        //    {
        //        _primaryContact = value;
        //    }
        //}

        #region Expose Binding Properties

        ///TODO : Need to reenable the design time attribute and change from Ilistsource to some class which is derived in normal classes.
        //[AttributeProvider(typeof(IListSource))]
        public object DataSource
        {
        
            get { return bindingSourcePrimaryContact.DataSource; }
            set { bindingSourcePrimaryContact.DataSource = value; }
        }

        ///TODO : Need to reenable the design time attribute and change from Ilistsource to some class which is derived in normal classes.
        //[Editor("System.Windows.Forms.Design.DataMemberListEditor,System.Design, Version=2.0.0.0, Culture=neutral,PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string DataMember
        {
            get { return bindingSourcePrimaryContact.DataMember; }
            set { bindingSourcePrimaryContact.DataMember = value; }
        }

        #endregion Expose Binding Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlDSPrimaryContactDetails"/> class.
        /// </summary>
        public CtrlDSPrimaryContactDetails()
        {
            InitializeComponent();
            

           
        }

        //private Binding _currentBinding;

        //public Binding CurrentBindging
        //{
        //    get { return _currentBiding; }
        //    set 
        //    { 
        //        _currentBiding = value; 
        //        bindingSourcePrimaryContact.DataSource = ((DataSourcePrimaryContact)_currentBinding)
        //    }
        //}


       // private DataSourcePrimaryContact _datasourcePrimaryContact;

       // DataSource 

        //public DataSourcePrimaryContact DatasourcePrimaryContact
        //{
        //    get { return _datasourcePrimaryContact; }
        //    set 
        //    { 
        //        _datasourcePrimaryContact = value;
        //        bindingSourcePrimaryContact.DataSource = _datasourcePrimaryContact;
                
        //    }
        //}



        //TODO -- Incomplete
        /// <summary>
        /// Validates the primary contact details.
        /// </summary>
        /// <returns></returns>
        public bool ValidatePrimaryContactDetails()
        {

            bool isValidated = false;

            

            if (txtPCFirstName == null || txtPCFirstName.Text.Length == 0)
            {
                errPrimaryDetails.SetError(txtPCFirstName, "Please Enter First Name");
                isValidated = false;
                return isValidated;
            }
            else if (txtPCLastName == null || txtPCLastName.Text.Length == 0)
            {
                errPrimaryDetails.SetError(txtPCLastName, "Please Enter Last Name");
                isValidated = false;
                return isValidated;
            }
            else if (txtPCTitle == null || txtPCTitle.Text.Length == 0)
            {
                errPrimaryDetails.SetError(txtPCTitle, "Please Enter the Title.");
                isValidated = false;
                return isValidated;
            }
            else if (txtPCWorkNumber == null || txtPCWorkNumber.Text.Length == 0)
            {
                errPrimaryDetails.SetError(txtPCWorkNumber, "Please Enter Work Number for Primary Contact.");
                isValidated = false;
                return isValidated;
            }
            else if (txtPCEmail == null || txtPCEmail.Text.Length == 0)
            {
                errPrimaryDetails.SetError(txtPCEmail, "Please Enter the Email Address for Primary Contact.");
                isValidated = false;
                return isValidated;
            }
            else if (txtPCEmail.Text.Length > 0)
            {
                //Check the E-mail address is valid or not.
                string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                Regex emailRegex = new Regex(emailCheck);
                Match emailMatch = emailRegex.Match(txtPCEmail.Text);
                bool isValidEmail = emailMatch.Success;
                if (!isValidEmail)
                {
                    errPrimaryDetails.SetError(txtPCEmail, "Plesase enter a valid E-Mail Address.");
                    isValidated = false;
                    return isValidated;
                }
                else
                {
                    isValidated = true;                  
                }

            }
            if(isValidated)
            {
                isValidated = true;
                //PrimaryContact.FirstName = txtPCFirstName.Text;
                //PrimaryContact.LastName = txtPCLastName.Text;
                //PrimaryContact.Title = txtPCTitle.Text;
                //PrimaryContact.EMail = txtPCEmail.Text;
                //PrimaryContact.WorkNumber = txtPCWorkNumber.Text;
                //PrimaryContact.CellNumber = txtPCCellNumber.Text;
            }

           // else if(txt)

            return isValidated;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            //txtPCFirstName.Text = string.Empty;
            //txtPCLastName.Text = string.Empty;
            //txtPCTitle.Text = string.Empty;
            //txtPCEmail.Text = string.Empty;
            //txtPCCellNumber.Text = string.Empty;
            //txtPCWorkNumber.Text = string.Empty;
        }

        /// <summary>
        /// This is called from the Parent Control of this control after the bindingSourcePrimaryContact is populated with appropriate DataSource Object.
        /// Adding these bindings in the Constructor i.e. at design time throws the error that we can not bind to the property.
        /// Clearing datasources so that the same control's same property is not bound twice.
        /// </summary>
        public void PopulateDetails()
        {
            //Clear DataBindings and add them again for the controls. 
            this.txtPCFirstName.DataBindings.Clear();
            this.txtPCFirstName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourcePrimaryContact, "FirstName", true));

            this.txtPCCellNumber.DataBindings.Clear();
            this.txtPCCellNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourcePrimaryContact, "CellNumber", true));

            this.txtPCWorkNumber.DataBindings.Clear();
            this.txtPCWorkNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourcePrimaryContact, "WorkNumber", true));

            this.txtPCEmail.DataBindings.Clear();
            this.txtPCEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourcePrimaryContact, "EMail", true));

            this.txtPCLastName.DataBindings.Clear();
            this.txtPCLastName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourcePrimaryContact, "LastName", true));

            this.txtPCTitle.DataBindings.Clear();
            this.txtPCTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourcePrimaryContact, "Title", true));
            //this.txtPCFirstName.Text = this.PrimaryContact.FirstName;
            //this.txtPCLastName.Text = this.PrimaryContact.LastName;
            //this.txtPCTitle.Text = this.PrimaryContact.Title;
            //this.txtPCEmail.Text = this.PrimaryContact.EMail;
            //this.txtPCWorkNumber.Text = this.PrimaryContact.WorkNumber;
            //this.txtPCCellNumber.Text = this.PrimaryContact.CellNumber;
        }

       

        
    }
}
