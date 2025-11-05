using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Prana.PM.Client
{
    public partial class ImportTradesDisplayForm : Form
    {
        //needed as cell updated is listened to for change in OrderIDs
        //we do not want the event to be raise until once the grid is binded
        bool _isBinded = false;

        public List<OrderSingle> _validatedTrades;

        public List<OrderSingle> ValidatedTrades
        {
            get { return _validatedTrades; }
        }

        private List<int> _editedOrderIDRowIndices = new List<int>();


        public ImportTradesDisplayForm()
        {
            InitializeComponent();

        }

        public void BindImportOrders(List<OrderSingle> importOrders)
        {
            ultraGrid1.DataSource = null;
            ultraGrid1.DataSource = importOrders;
            ultraGrid1.DisplayLayout.Bands[0].Columns["OrderID"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ultraGrid1.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortSingle;
            ultraGrid1.DisplayLayout.Override.AllowColMoving = AllowColMoving.Default;
        }

        private void ImportTradesDisplayForm_Load(object sender, EventArgs e)
        {
            try
            {
                System.Collections.ArrayList columnList = Global.OrderFields.ImportTradeColumnList;


                foreach (UltraGridColumn col in ultraGrid1.DisplayLayout.Bands[0].Columns)
                {
                    col.Hidden = true;
                    if (columnList.Contains(col.Key))
                    {
                        col.Hidden = false;
                        col.CellClickAction = CellClickAction.RowSelect;
                    }

                }
                //ultraGrid1.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.Select;
                ultraGrid1.DisplayLayout.Bands[0].Columns.Add("validated", "Validation Status");

                UltraGridColumn validationStatus = ultraGrid1.DisplayLayout.Bands[0].Columns["validated"];
                validationStatus.DefaultCellValue = "Validated";
                validationStatus.DataType = typeof(string);
                validationStatus.CellClickAction = CellClickAction.RowSelect;
                //validationStatus.IsReadOnly = true;
                CheckForValidRows();
                _isBinded = true;
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                ultraButton1.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton1.ForeColor = System.Drawing.Color.White;
                ultraButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton1.UseAppStyling = false;
                ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton2.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton2.ForeColor = System.Drawing.Color.White;
                ultraButton2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton2.UseAppStyling = false;
                ultraButton2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CheckForValidRows()
        {
            foreach (UltraGridRow row in ultraGrid1.DisplayLayout.Rows)
            {
                row.ExpansionIndicator = ShowExpansionIndicator.Never;
                if (int.Parse(row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["AUECID"]).ToString()) == int.MinValue)
                {
                    row.Cells["validated"].Value = "Order not Validated";
                    row.Cells["Text"].Value = "Cannot Import; Symbol not Valid";
                }
                else if (row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["Symbol"]).ToString() == string.Empty)
                {
                    row.Cells["validated"].Value = "Order not Validated";
                    row.Cells["Text"].Value = "Cannot Import; Symbol not Valid";
                }
                else if (row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["CounterPartyName"]).ToString() == string.Empty ||
                    row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["Venue"]).ToString() == string.Empty)
                {
                    row.Cells["validated"].Value = "Order not Validated";
                    row.Cells["Text"].Value = "Cannot Import; Broker not Valid";
                }
                else if (double.Parse(row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["Quantity"]).ToString()) <= 0.0)
                {
                    row.Cells["validated"].Value = "Order not Validated";
                    row.Cells["Text"].Value = "Cannot Import; Quantity not Valid";
                }
                else if (row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["OrderSide"]).ToString() == string.Empty)
                {
                    row.Cells["validated"].Value = "Order not Validated";
                    row.Cells["Text"].Value = "Cannot Import; OrderSide not Validated";
                }
                else if (row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["OrderID"]).ToString() == string.Empty)
                {

                    row.Cells["validated"].Value = "Order ID generated internally";
                    //row.Cells["Text"].Value = "Cannot Import; OrderID not Found";
                    if (!_isBinded)
                    {
                        row.Cells["OrderID"].Value = System.Guid.NewGuid().ToString();

                    }
                    row.Cells["OrderID"].Column.CellClickAction = CellClickAction.Edit;
                    row.Appearance.BackColor = System.Drawing.Color.Gray;
                }
                else if (double.Parse(row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["Price"]).ToString()) < 0.0)
                {
                    row.Cells["validated"].Value = "Order not Validated";
                    row.Cells["Text"].Value = "Cannot Import; Price not Valid";
                }
                else if (Convert.ToDouble(row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["Quantity"]).ToString()) <
                    Convert.ToDouble(row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["LastShares"]).ToString()))
                {
                    row.Cells["validated"].Value = "Order not Validated";
                    row.Cells["Text"].Value = "Cannot Import; Executed Qty greater than target Quantity";
                }
                else if (row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["CounterPartyName"]).ToString() == string.Empty ||
                        row.GetCellValue(ultraGrid1.DisplayLayout.Bands[0].Columns["Venue"]).ToString() == string.Empty)
                {
                    row.Cells["validated"].Value = "Order not Validated";
                    row.Cells["Text"].Value = "Cannot Import; CV information not found";
                }
                else
                {
                    row.Cells["validated"].Value = "Symbol Validated";
                    row.Cells["Text"].Value = "Symbol Validated";

                }
            }
            //_isBinded = true; 

        }


        private void ultraButton1_Click(object sender, EventArgs e)
        {
            if (ValidateOrdersBeforeImport())
            {
                _validatedTrades = new List<OrderSingle>();

                foreach (UltraGridRow row in ultraGrid1.Rows.GetAllNonGroupByRows())
                {
                    if (row.Cells["validated"].Value.ToString() == "Symbol Validated" ||
                        row.Cells["validated"].Value.ToString() == "Order ID generated internally")
                    {
                        _validatedTrades.Add((OrderSingle)row.ListObject);
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void ultraGrid1_CellChange(object sender, CellEventArgs e)
        {

        }


        private void ultraGrid1_CellDataError(object sender, CellDataErrorEventArgs e)
        {

        }

        private void ultraGrid1_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (_isBinded)
            {
                if (!_editedOrderIDRowIndices.Contains(e.Cell.Row.Index))
                {
                    _editedOrderIDRowIndices.Add(e.Cell.Row.Index);
                }
            }
        }

        private void ultraGrid1_AfterExitEditMode(object sender, EventArgs e)
        {




        }

        private bool ValidateOrdersBeforeImport()
        {
            //if (_editedOrderIDRowIndices.Count == 0)
            //{
            //    return true;
            //}

            int invalidOrderCount = 0;
            foreach (int _modifierOrderIDRowIndex in _editedOrderIDRowIndices)
            {
                UltraGridRow currentRow = ultraGrid1.Rows.GetRowWithListIndex(_modifierOrderIDRowIndex);
                string orderID = currentRow.Cells["OrderID"].Value.ToString();

                if (orderID == string.Empty)
                {
                    currentRow.Cells["Text"].Value = "Cannot import; Order ID not found";
                    invalidOrderCount++;
                }
                else
                {
                    bool valid = true;
                    double executedQuantity = 0.0;// Convert.ToDouble(currentRow.Cells["LastShares"].Value);

                    foreach (UltraGridRow existingRow in ultraGrid1.Rows.GetAllNonGroupByRows())
                    {
                        if (existingRow.Cells["OrderID"].Value.ToString() == orderID)
                        {
                            if (currentRow.Cells["Symbol"].Value.ToString() != existingRow.Cells["Symbol"].Value.ToString())
                            {
                                valid = false;
                                currentRow.Cells["validated"].Value = "Order ID not valid";
                                currentRow.Cells["Text"].Value = "Cannot import; Symbol confict between same Order IDs";
                                invalidOrderCount++;
                                break;
                            }
                            else if (currentRow.Cells["OrderSide"].Value.ToString() != existingRow.Cells["OrderSide"].Value.ToString())
                            {
                                valid = false;
                                currentRow.Cells["validated"].Value = "Order ID not valid";
                                currentRow.Cells["Text"].Value = "Cannot import; Side confict between same Order IDs";
                                invalidOrderCount++;
                                break;
                            }
                            else if (currentRow.Cells["Quantity"].Value.ToString() != existingRow.Cells["Quantity"].Value.ToString())
                            {
                                valid = false;
                                currentRow.Cells["validated"].Value = "Order ID not valid";
                                currentRow.Cells["Text"].Value = "Cannot import; Quantity confict between same Order IDs";
                                invalidOrderCount++;
                                break;
                            }
                            else
                            {
                                if (!CheckRowBasicValidationFields(currentRow))
                                {
                                    valid = false;
                                }
                            }
                            executedQuantity += Convert.ToDouble(existingRow.Cells["LastShares"].Value);
                        }
                    }
                    if (executedQuantity > Convert.ToDouble(currentRow.Cells["Quantity"].Value))
                    {
                        currentRow.Cells["validated"].Value = "Order ID not valid";
                        currentRow.Cells["Text"].Value = "Cannot import; Executed Quantity exceeds target Quantity";
                        invalidOrderCount++;
                        valid = false;
                        //MessageBox.Show("This OrderID already exists and would cause executed quantity greater than Target Quantity");
                    }
                    else if (valid)
                    {
                        currentRow.Cells["validated"].Value = "Order ID generated internally";
                        currentRow.Cells["Text"].Value = "Symbol Validated";
                    }
                }
            }
            foreach (UltraGridRow otherRows in ultraGrid1.Rows.GetAllNonGroupByRows())
            {
                if (!_editedOrderIDRowIndices.Contains(otherRows.Index))
                {
                    if (!(otherRows.Cells["validated"].Value.ToString() == "Symbol Validated" ||
                        otherRows.Cells["validated"].Value.ToString() == "Order ID generated internally"))
                    {
                        invalidOrderCount++;
                    }

                }
            }
            if (invalidOrderCount > 0)
            {
                if (MessageBox.Show("Some Orders could not be validated.Do You want to continue with the valid trades?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }

        private bool CheckRowBasicValidationFields(UltraGridRow currentRow)
        {
            if (int.Parse(currentRow.Cells["AUECID"].Value.ToString()) == int.MinValue)
            {
                currentRow.Cells["validated"].Value = "Order not Validated";
                currentRow.Cells["Text"].Value = "Cannot Import; Symbol not Valid";
                return false;
            }
            else if (currentRow.Cells["CounterPartyName"].Value.ToString() == string.Empty ||
                    currentRow.Cells["Venue"].Value.ToString() == string.Empty)
            {
                currentRow.Cells["validated"].Value = "Order not Validated";
                currentRow.Cells["Text"].Value = "Cannot Import; Broker not Valid";
                return false;
            }
            else if (double.Parse(currentRow.Cells["Quantity"].Value.ToString()) <= 0.0)
            {
                currentRow.Cells["validated"].Value = "Order not Validated";
                currentRow.Cells["Text"].Value = "Cannot Import; Quantity not Valid";
                return false;
            }
            else if (currentRow.Cells["OrderSide"].Value.ToString() == string.Empty)
            {
                currentRow.Cells["validated"].Value = "Order not Validated";
                currentRow.Cells["Text"].Value = "Cannot Import; OrderSide not Validated";
                return false;
            }

            else if (double.Parse(currentRow.Cells["Price"].Value.ToString()) <= 0.0)
            {
                currentRow.Cells["validated"].Value = "Order not Validated";
                currentRow.Cells["Text"].Value = "Cannot Import; Price not Valid";
                return false;
            }
            else if (Convert.ToDouble(currentRow.Cells["Quantity"].Value.ToString()) < Convert.ToDouble(currentRow.Cells["LastShares"].Value.ToString()))
            {
                currentRow.Cells["validated"].Value = "Order not Validated";
                currentRow.Cells["Text"].Value = "Cannot Import; Executed Qty greater than target Quantity";
                return false;
            }
            else if (currentRow.Cells["CounterPartyName"].Value.ToString() == string.Empty ||
                     currentRow.Cells["Venue"].Value.ToString() == string.Empty)
            {
                currentRow.Cells["validated"].Value = "Order not Validated";
                currentRow.Cells["Text"].Value = "Cannot Import; CV information not found";
                return false;
            }
            else
            {
                currentRow.Cells["validated"].Value = "Symbol Validated";
                currentRow.Cells["Text"].Value = "Symbol Validated";
                return true;
            }
        }




    }
}