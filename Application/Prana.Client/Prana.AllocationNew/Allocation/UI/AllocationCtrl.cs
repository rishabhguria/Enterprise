using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Utilities.StringUtilities;
using Prana.CommonDataCache;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.Misc;
using Prana.Utilities.UIUtilities;
//using Prana.PostTrade;
namespace Prana.AllocationNew
{
    partial class AllocationCtrl : Infragistics.Win.Misc.UltraPanel
    {
        bool _multipleGroupsSelected = false;
        private int _strategyID = int.MinValue;
        private UltraTextEditor[] txtPercentageCollection;
        private UltraTextEditor[] txtQtyCollection;
        UltraTextEditor txtbxTotalQty;
        UltraTextEditor txtbxTotalPercentage;
        UltraTextEditor txtbxRemainingQty;
        UltraTextEditor txtbxRemainingPercentage;
        AccountCollection _accounts = null;
        AllocationCtrl _parentAccountAllocationCtrl = null;
        int txtbxPerLength = 30;
        int txtbxQtyLength = 35;
        int txtbxheight = 10;
        double  _allowedQty = 0;
        private bool _longside = true ;
        bool _shouldAllowFractionalValues = false;
        bool _shouldChangeQty = false ;
        bool _shouldChangePercentage = false ;
        bool firsttime = true;
        string defaultvalueInTextBox = "";
        #region UI Methods

        public AllocationCtrl()
        {
            InitializeComponent();
        }
        public void SetUp(int strategyID, AccountCollection accounts, AllocationCtrl parentAccountAllocationCtrl,string name)
        {

            try
            {
                lblName.Text = name;
                txtbxPerLength = 34;
                txtbxQtyLength = 64;
                _parentAccountAllocationCtrl = parentAccountAllocationCtrl;
                _accounts = accounts;
                _strategyID = strategyID;
                int startLabel_X = 0;
                if (!_onlyPercentage)
                {
                    AddAccountsQtyTextBoxes(startLabel_X);
                    int widthPercentage = AddStartegyPercenytageLabels(startLabel_X);
                    AddStartegyQtyLabels(startLabel_X, widthPercentage);
                    
                }
                AddAccountsPercentageTextBoxes(startLabel_X);
                BindEventsFirstTime();

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void AddAccountsQtyTextBoxes(int startTxtBox_X)
        {

            txtQtyCollection = new UltraTextEditor[_accounts.Count];
            int i = 0;
            int start_Y = 40;
            int yIncrement = 20;

            try
            {
                for (i = 0; i < _accounts.Count; i++)
                {
                    Account account = (Account)(_accounts[i]);

                    txtQtyCollection[i] = new UltraTextEditor();
                    txtQtyCollection[i].Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        txtQtyCollection[i].BackColor = Color.LemonChiffon;

                    }
                    txtQtyCollection[i].Location = new System.Drawing.Point(startTxtBox_X + txtbxPerLength, start_Y + i * yIncrement);
                    txtQtyCollection[i].Size = new System.Drawing.Size(txtbxQtyLength, txtbxheight);
                    txtQtyCollection[i].Text = "";
                    txtQtyCollection[i].Name = account.AccountID.ToString() + "Qty";
                    this.ClientArea.Controls.Add(txtQtyCollection[i]);
                    txtQtyCollection[i].TabStop = false;
                    //txtQtyCollection[i].TabIndex = _accounts.Count+i;
                }


                txtbxTotalQty = new UltraTextEditor();
                txtbxTotalQty.ForeColor = Color.Green;
                txtbxTotalQty.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                txtbxTotalQty.Location = new System.Drawing.Point(startTxtBox_X + txtbxPerLength, start_Y + i * yIncrement);
                txtbxTotalQty.Size = new System.Drawing.Size(txtbxQtyLength, txtbxheight);
                txtbxTotalQty.Text = "";
                if (!CustomThemeHelper.ApplyTheme)
                {
                    txtbxTotalQty.BackColor = Color.Bisque;

                }
                txtbxTotalQty.TabStop = false;

                i++;

                txtbxRemainingQty = new UltraTextEditor();
                txtbxRemainingQty.ForeColor = Color.Green;
                txtbxRemainingQty.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                txtbxRemainingQty.Location = new System.Drawing.Point(startTxtBox_X + txtbxPerLength, start_Y + i * yIncrement);
                txtbxRemainingQty.Size = new System.Drawing.Size(txtbxQtyLength, txtbxheight);
                txtbxRemainingQty.Text = "";
                if (!CustomThemeHelper.ApplyTheme)
                {
                    txtbxRemainingQty.BackColor = Color.Bisque;

                }
                txtbxRemainingQty.TabStop = false;

                this.ClientArea.Controls.Add(txtbxTotalQty);
                this.ClientArea.Controls.Add(txtbxRemainingQty);

                

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            #endregion
        }
        public void AddAccountsPercentageTextBoxes(int startTxtBox_X)
        {


            txtPercentageCollection = new UltraTextEditor[_accounts.Count];
            int i = 0;
            //int startTxtBox_X = 0;
            int start_Y = 40;
            int yIncrement = 20;

            try
            {
                for (i = 0; i < _accounts.Count; i++)
                {
                    Account account = (Account)(_accounts[i]);

                    txtPercentageCollection[i] = new UltraTextEditor();
                    txtPercentageCollection[i].Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        txtPercentageCollection[i].BackColor = Color.LemonChiffon;

                    }
                    txtPercentageCollection[i].Location = new System.Drawing.Point(startTxtBox_X, start_Y + i * yIncrement);
                    txtPercentageCollection[i].Size = new System.Drawing.Size(txtbxPerLength, txtbxheight);
                    txtPercentageCollection[i].Text = "";
                    txtPercentageCollection[i].Name = account.AccountID.ToString();
                    txtPercentageCollection[i].WordWrap = true ;
                    txtPercentageCollection[i].Multiline = false;
                    txtPercentageCollection[i].MaxLength = 10;
                    this.ClientArea.Controls.Add(txtPercentageCollection[i]);
                    txtPercentageCollection[i].TabIndex = i;
                    txtPercentageCollection[i].SelectAll();
                    //if (!_onlyPercentage)
                    //{
                   // }
                }

                txtbxTotalPercentage = new UltraTextEditor();
                txtbxTotalPercentage.Location = new System.Drawing.Point(startTxtBox_X, start_Y + i * yIncrement);
                txtbxTotalPercentage.Size = new System.Drawing.Size(txtbxPerLength, txtbxheight);
                txtbxTotalPercentage.Text = "100";
                if (!CustomThemeHelper.ApplyTheme)
                {
                    txtbxTotalPercentage.BackColor = Color.Bisque;

                }
                txtbxTotalPercentage.Enabled = false;
                txtbxTotalPercentage.ForeColor = Color.Green ;
                txtbxTotalPercentage.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.ClientArea.Controls.Add(txtbxTotalPercentage);
                txtbxTotalPercentage.TabStop = false;

                i++;

                txtbxRemainingPercentage = new UltraTextEditor();
                txtbxRemainingPercentage.Location = new System.Drawing.Point(startTxtBox_X, start_Y + i * yIncrement);
                txtbxRemainingPercentage.Size = new System.Drawing.Size(txtbxPerLength, txtbxheight);
                txtbxRemainingPercentage.Text = "100";
                if (!CustomThemeHelper.ApplyTheme)
                {
                    txtbxRemainingPercentage.BackColor = Color.Bisque;

                }
                txtbxRemainingPercentage.Enabled = false;
                txtbxRemainingPercentage.ForeColor = Color.Green;
                txtbxRemainingPercentage.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.ClientArea.Controls.Add(txtbxRemainingPercentage);
                txtbxRemainingPercentage.TabStop = false;

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            #endregion
        }
        public void AddStartegyQtyLabels(int startTxtBox_X, int widthPercentage)
        {
            UltraLabel Qty = new UltraLabel();
            try
            {
                Qty.Text = "Qty";
                Qty.AutoSize = true;
                Qty.Location = new System.Drawing.Point(startTxtBox_X + widthPercentage, 20);
                this.ClientArea.Controls.Add(Qty);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public int AddStartegyPercenytageLabels(int startTxtBox_X)
        {
            UltraLabel percentage = new UltraLabel();
            try
            {
                percentage.Text = "%";
                percentage.AutoSize = true;
                percentage.Location = new System.Drawing.Point(startTxtBox_X, 20);
                this.ClientArea.Controls.Add(percentage);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 30;
        }
        public void SetSelectionStatus(bool multipleSelected)
        {
            try
            {
                if (_onlyPercentage)
                    return;
                _multipleGroupsSelected = multipleSelected;
                if (_multipleGroupsSelected)
                {
                    foreach (UltraTextEditor txtbxQty in txtQtyCollection)
                    {
                        txtbxQty.Enabled = false;
                        //txtbxQty.ForeColor = txtbxQty.dis;
                        txtbxQty.Text = "N/A";
                    }
                    txtbxTotalQty.Enabled = false;
                    txtbxRemainingQty.Enabled = false;
                }
                else
                {
                    foreach (UltraTextEditor txtbxQty in txtQtyCollection)
                    {
                        txtbxQty.Enabled = true;
                        //txtbxQty.ForeColor = Color.Black;
                    }
                    txtbxTotalQty.Enabled = true;
                    txtbxRemainingQty.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region events for precentage and qty change
        private void BindEventsFirstTime()
        {
            try
            {
                BindTextBoxQtyEvents();
                BindTextBoxPercentageEvents();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindTextBoxPercentageEvents()
        {
            try
            {
                if (txtPercentageCollection != null)
                {
                    foreach (UltraTextEditor txtbxPer in txtPercentageCollection)
                    {
                        txtbxPer.TextChanged += new EventHandler(txtbxPer_TextChanged);
                        txtbxPer.KeyDown += new KeyEventHandler(txtbxPer_KeyDown);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool _isPercentageChanged = false;
        private void txtbxPer_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                _isPercentageChanged = true;
                _isQuantityChanged = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if(rethrow)
                {
                    throw;
                }
            }
            
        }

        private void BindTextBoxQtyEvents()
        {
            try
            {
                if (txtQtyCollection != null)
                {
                    foreach (UltraTextEditor txtbxQty in txtQtyCollection)
                    {
                        txtbxQty.TextChanged += new EventHandler(txtbxQty_TextChanged);
                        txtbxQty.KeyDown += new KeyEventHandler(txtbxQty_KeyDown);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtbxQty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                _isPercentageChanged = false;
                _isQuantityChanged = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool _isQuantityChanged = false;
        void txtbxQty_TextChanged(object sender, EventArgs e)
        {
           
            if (!firsttime && !_shouldChangeQty)
            {
                return;
            }
            firsttime = false;
            BlockChanges();
            double qty = 0;
             UltraTextEditor txtbxSender = (UltraTextEditor)sender;
             int location = GetSelectedQtyTextBoxIndex(txtbxSender);
            try
            {

                if (!_multipleGroupsSelected)
                {

                    string value = txtbxSender.Text.Trim();
                    if (_shouldAllowFractionalValues)
                    {
                        if (!RegularExpressionValidation.IsNumber(value))
                        {
                            txtbxSender.Text = defaultvalueInTextBox;
                        }
                    }
                    else
                    {
                        if (!RegularExpressionValidation.IsInteger(value))
                        {
                            txtbxSender.Text = defaultvalueInTextBox;
                        }
                    }

                    CalculatePercentageBasedOnQty(location);
                    double totalQty = CalculateTotalQty();
                    CalculateTotalPercentage();
                    if (_isPercentageChanged)
                    {
                        qty = GetQtyIfValid(location);
                        qty = _longside.Equals(true) ? qty : -qty;
                    }
                    if (_isQuantityChanged)
                    {
                        if (CalculateTotalStrategyPercentage != null && !_multipleGroupsSelected && _shouldValidate)
                        {
                            CalculateTotalStrategyPercentage(location, qty, _shouldValidate);
                        }

                        if (_shouldValidate && (totalQty.Equals(_allowedQty) || totalQty.Equals(0)))
                        {
                            if (ValueChanged != null)
                                ValueChanged(this, new EventArgs());
                        }

                        if (totalQty.Equals(_allowedQty) && !_multipleGroupsSelected && _shouldValidate)
                        {
                            if(CheckSumPercentage != null)
                            {
                                CheckSumPercentage(this, EventArgs.Empty);
                            }
                        }

                        if (!_shouldValidate)
                        {
                            float totalPercentage = CalculateTotalPercentage(true);

                            if (Math.Round(totalPercentage).Equals(100) && !_multipleGroupsSelected)
                            {
                                _shouldValidate = true;
                                if (CheckSumQty != null)
                                {
                                    CheckSumQty(this, EventArgs.Empty);
                                }
                                
                            }
                        }
                        _isQuantityChanged = false;
                    }
                }
                if (QtyPerChanged != null && !_multipleGroupsSelected)
                {
                    QtyPerChanged(location, qty, _shouldValidate);
                }
                
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                AllowChanges();
            }
        }

        private bool _shouldValidate = false;
        void txtbxPer_TextChanged(object sender, EventArgs e)
        {
           
            if (! firsttime && !_shouldChangePercentage)
            {
                return;
            }
            BlockChanges();
            firsttime = false;
             UltraTextEditor txtbxSender = (UltraTextEditor)sender;
             double qty = 0;
             int location = GetSelectedPercentageTextBoxIndex(txtbxSender);
            try
            {

                SetPercentageDefautValues(txtbxSender);
                if (!_multipleGroupsSelected && !_onlyPercentage && _isPercentageChanged)
                {
                   
                    CalculateQtyBasedOnPercentage(location);
                    CalculateTotalQty();
                    qty = GetQtyIfValid(location);
                    qty = _longside.Equals(true) ? qty : -qty;
                }
               
                float totalPercentage = CalculateTotalPercentage(true);
                if (_isPercentageChanged && (totalPercentage.Equals(100F)|| totalPercentage.Equals(0F)))
                {
                    if (ValueChanged != null)
                        ValueChanged(this, new EventArgs());
                }
                if (_isPercentageChanged && totalPercentage.Equals(100F) && !_multipleGroupsSelected)
                {
                    //if (ValueChanged != null)
                    //    ValueChanged(this, new EventArgs());

                    if (CheckSumQty != null)
                    {
                        CheckSumQty(this, EventArgs.Empty);
                    }
                    _isPercentageChanged = false;
                }

                if (QtyPerChanged != null && !_multipleGroupsSelected && !_onlyPercentage)
                {
                    QtyPerChanged(location, qty,false);
                }
                if (_isPercentageChanged && CalculateTotalStrategyPercentage != null && !_multipleGroupsSelected)
                {
                    _isPercentageChanged = false;
                    CalculateTotalStrategyPercentage(location, qty,false);
                   
                }
                CalculateTotalPercentage();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                AllowChanges();
               
               
            }
        }
        private int GetSelectedPercentageTextBoxIndex(UltraTextEditor txtbx)
        {
            int i = 0;
            try
            {
                foreach (UltraTextEditor txtbxPer in txtPercentageCollection)
                {
                    if (txtbxPer == txtbx)
                    {
                        break;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return i;
        }
        private int GetSelectedQtyTextBoxIndex(UltraTextEditor txtbx)
        {
            int i = 0;
            try
            {
                foreach (UltraTextEditor txtbxQty in txtQtyCollection)
                {
                    if (txtbxQty == txtbx)
                    {
                        break;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return i;
        }
        private void SetPercentageDefautValues(UltraTextEditor txtbxPer)
        {
            try
            {
                string value = txtbxPer.Text.Trim();
                float valueFloat = 0;
                float.TryParse(value, out valueFloat);

                if (!RegularExpressionValidation.IsNumber(value))
                {
                    value = defaultvalueInTextBox;
                    txtbxPer.Text = value;
                }
                else if (valueFloat == 0.0)
                {
                    txtbxPer.Text = defaultvalueInTextBox;
                }
                else if (valueFloat > 100)
                {
                    value = valueFloat.ToString();
                    value = value.Substring(0, value.Length - 1);
                    txtbxPer.Text = value;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
        private void SetQtyDefautValues(int location)
        {
            try
            {
                UltraTextEditor txtbxQty = txtQtyCollection[location];
                string value = txtbxQty.Text.Trim();
                if (!RegularExpressionValidation.IsInteger(value))
                {
                    value = defaultvalueInTextBox;
                    txtbxQty.Text = value;
                }
                else if (float.Parse(value) > GetAllowedQty(location))
                {
                    value = value.Substring(0, value.Length - 1);
                    txtbxQty.Text = value;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }
        public double  CalculateTotalQty()
        {
            if (_onlyPercentage)
                return 0;
            if (_multipleGroupsSelected)
            {
                txtbxTotalQty.Text = "N/A";
                txtbxRemainingQty.Text = "N/A";
                return _allowedQty;
            }
                decimal totalQty = 0;
                foreach (UltraTextEditor txtbxQty in txtQtyCollection)
                {
                    double qty =Math.Abs(GetQtyIfValid(txtbxQty));
                    totalQty += Convert.ToDecimal(qty);
                }
                double remainingQty = _allowedQty - Convert.ToDouble(totalQty);
                txtbxTotalQty.Text = totalQty.ToString()+"/"+_allowedQty.ToString();
                txtbxRemainingQty.Text = remainingQty.ToString() + "/" + _allowedQty.ToString();
                return Convert.ToDouble(totalQty);
        }
        public void CalculateTotalPercentage()
        {
            try
            {
                if (_strategyID == int.MinValue)
                {
                    //errorProvider.SetError(txtbxTotalPercentage, "");
                    decimal totalPercentage = 0;
                    int location = 0;
                    foreach (UltraTextEditor txtbxPer in txtPercentageCollection)
                    {
                        decimal percentage = (decimal)GetPercentageIfValid(txtbxPer);


                        location++;

                        totalPercentage += percentage;
                    }
                    totalPercentage = Math.Round(totalPercentage, 2);
                    decimal remainingPercentage = 100 - totalPercentage;
                    txtbxTotalPercentage.Text = totalPercentage.ToString();
                    txtbxRemainingPercentage.Text = Math.Round(remainingPercentage).ToString();
                    //if (totalPercentage == 100 && !_multipleGroupsSelected)
                    //{
                    //RecheckSumOfQty();
                    //}
                }
                else
                {
                    int location = 0;
                    float totalPercentage = 0;
                    foreach (UltraTextEditor txtbxPer in txtPercentageCollection)
                    {
                        double qty = _parentAccountAllocationCtrl.GetAllowedQty(location);
                        totalPercentage += ((_parentAccountAllocationCtrl.GetPercentageIfValid(location) * GetPercentageIfValid(location))) / 100;
                        //if (qty > 0 && _isPercentageChanged)
                        //{
                        //    _isPercentageChanged = false;
                        //    if (CalculateTotalStraegyPercentage != null)
                        //    {
                        //        CalculateTotalStraegyPercentage(location, GetAllowedQty(location));
                        //    }
                        //}
                        location++;
                    }
                    float remainingPercentage = 100 - totalPercentage;
                    txtbxTotalPercentage.Text = totalPercentage.ToString();
                    txtbxRemainingPercentage.Text = remainingPercentage.ToString();

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public float CalculateTotalPercentage(bool shouldCalculate)
        {
            float totalPercentage = 0F;
            try
            {
                if (_strategyID == int.MinValue)
                {
                    //errorProvider.SetError(txtbxTotalPercentage, "");
                    int location = 0;
                    foreach (UltraTextEditor txtbxPer in txtPercentageCollection)
                    {
                        float percentage = GetPercentageIfValid(txtbxPer);

                        location++;

                        totalPercentage += percentage;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return totalPercentage;
        }

        public void ClearQty()
        {

            try
            {
                foreach (UltraTextEditor txtQty in txtQtyCollection)
                {
                    txtQty.Text = defaultvalueInTextBox;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
           

        }
        public void SetGivenValuesToAll(string value)
        {
            try
            {
                BlockChanges();
                foreach (UltraTextEditor txtQty in txtQtyCollection)
                {
                    txtQty.Text = value;
                }
                foreach (UltraTextEditor txtPercentage in txtPercentageCollection)
                {
                    txtPercentage.Text = value;
                }
                AllowChanges();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void BlockChanges()
        {
            try
            {
                _shouldChangeQty = false;
                _shouldChangePercentage = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void AllowChanges()
        {
            try
            {
                _shouldChangeQty = true;
                _shouldChangePercentage = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void ClearPercentage()
        {
            try
            {
                foreach (UltraTextEditor txtPer in txtPercentageCollection)
                {
                    txtPer.Clear();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CalculateQtyBasedOnPercentage(int location)
        {
            try
            {
                if (!_multipleGroupsSelected)
                {
                    UltraTextEditor txtbxQty = txtQtyCollection[location];
                    //UnBindTextBoxQtyEvents(txtbxQty);
                    float percentage = GetPercentageIfValid(location);
                    double qty = Convert.ToDouble(Convert.ToDecimal(GetAllowedQty(location)) * Convert.ToDecimal(percentage)) / 100.0;
                    // qty = Math.Round(qty, 2);
                    //if (!_shouldAllowFractionalValues)
                    //{
                    //    qty = Math.Round(qty);
                    //}
                    SetTextInTextBox(qty, txtbxQty);

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
        private void CalculateQtyBasedOnPercentage()
        {
            try
            {
                //UnBindTextBoxQtyEvents();
                for (int i = 0; i < txtQtyCollection.Length; i++)
                {
                    UltraTextEditor txtbxQty = txtQtyCollection[i];
                    float percentage = GetPercentageIfValid(i);
                    double qty = (GetAllowedQty(i) * percentage) / 100;
                    if (!_shouldAllowFractionalValues)
                    {
                        qty = Convert.ToInt64(qty);
                    }
                    SetTextInTextBox(qty, txtbxQty);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void SetTextInTextBox(object   value,UltraTextEditor txtbx)
        {
            try
            {
                string strvalue = value.ToString();
                if (strvalue == "0")
                {
                    txtbx.Text = defaultvalueInTextBox;
                }
                else
                {
                    txtbx.Text = strvalue;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void CalculatePercentageBasedOnQty(int location)
        {
            try
            {
                UltraTextEditor txtbx = txtPercentageCollection[location];
                //BindTextBoxPercentageEvents(txtbx);
                double qty = GetQtyIfValid(location);
                float percentage = 0;
                if (qty != 0)
                {
                    double allowedQty = GetAllowedQty(location);
                    if (allowedQty != 0)
                    {
                        percentage = Convert.ToSingle(Convert.ToDecimal(qty) * Convert.ToDecimal(100) / Convert.ToDecimal(allowedQty));
                        //Rounding off to 4 decimals will provide better accuracy in percentages
                        //and server validation will not change the quantities further.
                        //http://jira.nirvanasolutions.com:8080/browse/SS-49
                        percentage = Math.Abs(Convert.ToSingle(Math.Round(percentage, 4)));
                    }
                }
                SetTextInTextBox(percentage, txtbx);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion
        public AllocationObjColl  GetAllocations(double cumQty)
        {
            AllocationObjColl allocationObjColl = new AllocationObjColl();
            try
            {
                //AllocationLevelList accounts = new AllocationLevelList();
                allocationObjColl.StrategyID = _strategyID;
                // double totalQty = 0;
                int i = 0;
                //CalculateTotalPercentage();
                foreach (UltraTextEditor txtPercentage in txtPercentageCollection)
                {
                    AllocationCtrlObject allocationCtrlObject = new AllocationCtrlObject();
                    if (txtPercentage.Text != string.Empty)
                    {
                        float percentage = float.Parse(txtPercentage.Text.Trim());

                        if (percentage > 0)
                        {
                            double qty = 0;
                            if (!_onlyPercentage)
                            {
                                if (_multipleGroupsSelected)
                                {
                                    qty = Convert.ToDouble((Convert.ToDecimal(cumQty) * Convert.ToDecimal(percentage) * Convert.ToDecimal(GetParentPercentage(i))) / Convert.ToDecimal(100));

                                    if (!_shouldAllowFractionalValues)
                                    {
                                        qty = Convert.ToInt64(qty);
                                    }

                                }
                                else
                                {
                                    qty = GetQtyIfValid(i);

                                    if (!_shouldAllowFractionalValues)
                                    {
                                        qty = Convert.ToInt64(qty);
                                    }
                                }
                            }

                            allocationCtrlObject.Percentage = percentage;
                            allocationCtrlObject.Qty = qty;
                            allocationCtrlObject.ID = AccountStrategyMapping.GetAccountIDByLocation(i);

                        }

                    }

                    allocationObjColl.Collection.Add(allocationCtrlObject);

                    i++;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            //RecheckSumOfQty();
            return allocationObjColl;

           // foreach (UltraTextEditor txtPercentage in txtPercentageCollection)
            //{
            //    if (txtPercentage.Text != string.Empty)
            //    {
            //        float percentage = float.Parse(txtPercentage.Text.Trim());
            //        if (percentage > 0)
            //        {
            //            double qty = 0;
            //            if (_multipleGroupsSelected)
            //            {
            //                qty = Convert.ToInt64((group.CumQty * percentage) / 100);
            //            }
            //            else
            //            {
            //                qty = double.Parse(txtQtyCollection[i].Text.Trim());
            //            }
            //            AllocationLevelClass account = new AllocationLevelClass(group.GroupID);
            //            account.Percentage = percentage;
            //            account.AllocatedQty = qty;
            //            account.LevelnID = AccountStrategyMapping.GetAccountIDByLocation(i);
            //            accounts.Add(account);
            //        }
            //    }
            //    i++;
            //}
            //return accounts;
        }

        private void SetValues(AllocationObjColl objAllocation, bool shouldClear)
        {
            try
            {
                
                int i = 0;
                //ClearQty();
                //foreach (UltraTextEditor txtbxPer in txtPercentageCollection)
                //{
                //    SetPercentageDefautValues(txtbxPer);
                //}
                foreach (UltraTextEditor txtbxQty in txtQtyCollection)
                {
                    UltraTextEditor txtPercentage = txtPercentageCollection[i];

                    if (objAllocation.ShouldClearPercentage || shouldClear) // if allocated clear percentage
                    {
                        double qty = objAllocation.Collection[i].Qty;
                        SetTextInTextBox(qty, txtbxQty);
                        SetTextInTextBox(objAllocation.Collection[i].Percentage, txtPercentage);
                        CalculatePercentageBasedOnQty(i);
                        _shouldValidate = shouldClear;
                    }
                    else // dont clear percentage and calculate qty on the basis of percentage
                    {
                        CalculateQtyBasedOnPercentage(i);
                        _shouldValidate = shouldClear;
                        _isPercentageChanged = shouldClear;
                        _isQuantityChanged = shouldClear;
                    }
                    i++;
                }
                CalculateTotalQty();
               
              
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public event EventHandler CheckSumQty;
        public event EventHandler CheckSumPercentage;


        //Obsolete function because of fractional shares implementation
        //It has been checked at the server side.
        private void RecheckSumOfQty()
        {
            try
            {
                if (_onlyPercentage)
                {
                    return;
                }
                double totalQty = CalculateTotalQty();
                if (_allowedQty != totalQty)
                {
                    double remainingQty = _allowedQty - totalQty;
                    if (remainingQty > 0)
                    {
                        for (int location = 0; location < txtQtyCollection.Length; location++)
                        {
                            double newQty = GetQtyIfValid(location);
                            if (newQty > 0)
                            {
                                newQty += remainingQty;
                                SetTextInTextBox(newQty, txtQtyCollection[location]);
                                break;
                            }


                        }
                    }
                    else // if allocated qty is greater than allowed qty than reduce the
                    // qty by 1 on last allocations till it is equal to allowed Qty=allocated QTY.
                    {
                        for (int location = txtQtyCollection.Length - 1; location >= 0; location--)
                        {
                            double newQty = GetQtyIfValid(location);
                            if (remainingQty != 0)
                            {
                                if (newQty > 0)
                                {
                                    newQty--;
                                    remainingQty++;
                                    SetTextInTextBox(newQty, txtQtyCollection[location]);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    for (int location = 0; location < txtQtyCollection.Length; location++)
                    {
                        CalculatePercentageBasedOnQty(location);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
          
        }
        public void SetOnlyPercentageValues(AllocationObjColl objAllocation, double totalQty, bool longSide, int assetID)
        {
            try
            {
                _shouldAllowFractionalValues = PostTradeHelper.ISFractionalAllocationAllowed(assetID);
                _longside = longSide;
                int i = 0;
                if (!_shouldAllowFractionalValues)
                {
                    totalQty = Convert.ToInt64(totalQty);
                }
                _allowedQty = Math.Abs(totalQty);

                foreach (UltraTextEditor txtPercentage in txtPercentageCollection)
                {
                    SetTextInTextBox(objAllocation.Collection[i].Percentage, txtPercentage);
                    CalculateQtyBasedOnPercentage(i);
                    i++;
                }
                CalculateTotalQty();
                CalculateTotalPercentage();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void SetDefaults(AllocationObjColl objAllocation)
        {
            try
            {
                int i=0;
                foreach (UltraTextEditor txtPercentage in txtPercentageCollection)
                {
                    SetTextInTextBox(objAllocation.Collection[i].Percentage, txtPercentage);
                    if (!_onlyPercentage)
                    {
                        CalculateQtyBasedOnPercentage(i);
                    }
                    i++;
                }
                CalculateTotalQty();
                CalculateTotalPercentage();

               
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
            }
        }
     
        public void SetAllocationAccounts(AllocationObjColl objAllocation, double  totalQty,bool longSide,int assetID, bool shouldClear)
        {
            try
            {
                if (!_shouldAllowFractionalValues)
                {
                    totalQty = Convert.ToInt64(totalQty);
                }
                _allowedQty = Math.Abs(totalQty);
                _shouldAllowFractionalValues = PostTradeHelper.ISFractionalAllocationAllowed(assetID);
                _longside = longSide;
            
                SetValues(objAllocation,shouldClear);
                CalculateTotalPercentage();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            finally
            {
               
            }
        }

        public delegate void QtyPercentageChangedHandler(int location,double qty, bool shouldValidate);
        public event QtyPercentageChangedHandler QtyPerChanged;
        public event QtyPercentageChangedHandler CalculateTotalStrategyPercentage;
        public void SetQty(int location, double qty)
        {

            try
            {
                if (qty != double.MinValue)
                {
                    SetTextInTextBox(qty, txtQtyCollection[location]);
                }
                CalculatePercentageBasedOnQty(location);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
            }
        }
        public void ReSetQty(int location)
        {

            try
            {
                CalculateQtyBasedOnPercentage(location);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
            }
        }

        private  double GetQtyIfValid(UltraTextEditor txtbx)
        {
            try
            {
                string value = txtbx.Text.Trim();
                if (_shouldAllowFractionalValues)
                {
                    if (RegularExpressionValidation.IsNumber(value))
                    {
                        return double.Parse(value);
                    }

                }
                else
                {
                    if (RegularExpressionValidation.IsInteger(value))
                    {
                        return double.Parse(value);
                    }

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return 0;
        }
        public double GetQtyIfValid(int location)
        {
            if (txtQtyCollection != null)
            {
                return GetQtyIfValid(txtQtyCollection[location]);
            }
            else
            {
                return 0.0;
            }
        }
        public float GetPercentageIfValid(int location)
        {
                return GetPercentageIfValid(txtPercentageCollection[location]);
        }
        private float GetPercentageIfValid(UltraTextEditor txtbx)
        {
            string value = txtbx.Text.Trim();
            if (RegularExpressionValidation.IsPositiveNumber(value))
            {
                return Math.Abs(float.Parse(value));
            }
            else
            {
                return 0;
            }
        }

        public double GetAllowedQty(int location)
        {

            if (_parentAccountAllocationCtrl != null)
            {
                return _parentAccountAllocationCtrl.GetQtyIfValid(location); 
            }
            else
            {
                    return _allowedQty;
               
            }
        }
        public double GetParentPercentage(int location)
        {
            if (_parentAccountAllocationCtrl != null)
            {
                return (_parentAccountAllocationCtrl.GetPercentageIfValid(location))/100.0;
            }
            else
            {
                return 1;

            }
        }

        public int StarategyID
        {
            get { return _strategyID; }
        }
        private bool  _onlyPercentage;

        public bool  OnlyPercentage
        {
            get { return _onlyPercentage; }
            set { _onlyPercentage = value; }
        }	

        public event EventHandler ValueChanged;
    }
}
