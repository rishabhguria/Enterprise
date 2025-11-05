using Prana.LogManager;
using System;
using System.Collections;
using System.Windows.Forms;

namespace Prana.LiveFeed.UI
{
    /// <summary>
    /// Summary description for ColumnPreferenceControl.
    /// </summary>
    public class ColumnPreferenceControl : System.Windows.Forms.UserControl
    {
        #region Private members

        private const string FORM_NAME = "ColumnPreferenceControl : ";
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnMoveRight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lstAvailableCol;
        private System.Windows.Forms.ListBox lstDisplayedCol;
        private System.Windows.Forms.Button btnMoveAllRight;
        private System.Windows.Forms.Button btnMoveLeft;
        private System.Windows.Forms.Button btnMoveAllLeft;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;


        #endregion Private members

        #region Constructor

        public ColumnPreferenceControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            lstAvailableCol.DataSourceChanged += new EventHandler(lstAvailableCol_DataSourceChanged);
            lstDisplayedCol.DataSourceChanged += new EventHandler(lstDisplayedCol_DataSourceChanged);
        }

        #endregion Constructor

        #region Dispose

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (btnDown != null)
                {
                    btnDown.Dispose();
                }
                if (btnUp != null)
                {
                    btnUp.Dispose();
                }
                if (btnMoveRight != null)
                {
                    btnMoveRight.Dispose();
                }
                if (btnMoveAllRight != null)
                {
                    btnMoveAllRight.Dispose();
                }
                if (btnMoveAllLeft != null)
                {
                    btnMoveAllLeft.Dispose();
                }
                if (btnMoveLeft != null)
                {
                    btnMoveLeft.Dispose();
                }
                if (lstAvailableCol != null)
                {
                    lstAvailableCol.Dispose();
                }
                if (lstDisplayedCol != null)
                {
                    lstDisplayedCol.Dispose();
                }
            }
            base.Dispose(disposing);
            lstAvailableCol.DataSourceChanged -= new EventHandler(lstAvailableCol_DataSourceChanged);
            lstDisplayedCol.DataSourceChanged += new EventHandler(lstDisplayedCol_DataSourceChanged);
        }

        #endregion Dispose

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ColumnPreferenceControl));
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lstAvailableCol = new System.Windows.Forms.ListBox();
            this.lstDisplayedCol = new System.Windows.Forms.ListBox();
            this.btnMoveAllRight = new System.Windows.Forms.Button();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.btnMoveAllLeft = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
            this.btnDown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDown.BackgroundImage")));
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDown.Location = new System.Drawing.Point(254, 94);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(32, 24);
            this.btnDown.TabIndex = 36;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
            this.btnUp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUp.BackgroundImage")));
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUp.Location = new System.Drawing.Point(254, 56);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(32, 24);
            this.btnUp.TabIndex = 35;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
            this.btnMoveRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMoveRight.BackgroundImage")));
            this.btnMoveRight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveRight.Location = new System.Drawing.Point(108, 26);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(32, 24);
            this.btnMoveRight.TabIndex = 34;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 14);
            this.label2.TabIndex = 27;
            this.label2.Text = "Available Columns";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(154, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 14);
            this.label3.TabIndex = 28;
            this.label3.Text = "Display Columns";
            // 
            // lstAvailableCol
            // 
            this.lstAvailableCol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstAvailableCol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lstAvailableCol.Location = new System.Drawing.Point(8, 26);
            this.lstAvailableCol.Name = "lstAvailableCol";
            this.lstAvailableCol.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAvailableCol.Size = new System.Drawing.Size(90, 132);
            this.lstAvailableCol.TabIndex = 30;
            // 
            // lstDisplayedCol
            // 
            this.lstDisplayedCol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstDisplayedCol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lstDisplayedCol.Location = new System.Drawing.Point(154, 26);
            this.lstDisplayedCol.Name = "lstDisplayedCol";
            this.lstDisplayedCol.Size = new System.Drawing.Size(90, 132);
            this.lstDisplayedCol.TabIndex = 29;
            // 
            // btnMoveAllRight
            // 
            this.btnMoveAllRight.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
            this.btnMoveAllRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMoveAllRight.BackgroundImage")));
            this.btnMoveAllRight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveAllRight.Location = new System.Drawing.Point(108, 54);
            this.btnMoveAllRight.Name = "btnMoveAllRight";
            this.btnMoveAllRight.Size = new System.Drawing.Size(32, 24);
            this.btnMoveAllRight.TabIndex = 31;
            this.btnMoveAllRight.Click += new System.EventHandler(this.btnMoveAllRight_Click);
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
            this.btnMoveLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMoveLeft.BackgroundImage")));
            this.btnMoveLeft.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveLeft.Location = new System.Drawing.Point(108, 106);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(32, 24);
            this.btnMoveLeft.TabIndex = 32;
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            // 
            // btnMoveAllLeft
            // 
            this.btnMoveAllLeft.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
            this.btnMoveAllLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMoveAllLeft.BackgroundImage")));
            this.btnMoveAllLeft.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveAllLeft.Location = new System.Drawing.Point(108, 134);
            this.btnMoveAllLeft.Name = "btnMoveAllLeft";
            this.btnMoveAllLeft.Size = new System.Drawing.Size(32, 24);
            this.btnMoveAllLeft.TabIndex = 33;
            this.btnMoveAllLeft.Click += new System.EventHandler(this.btnMoveAllLeft_Click);
            // 
            // ColumnPreferenceControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnMoveRight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstAvailableCol);
            this.Controls.Add(this.lstDisplayedCol);
            this.Controls.Add(this.btnMoveAllRight);
            this.Controls.Add(this.btnMoveLeft);
            this.Controls.Add(this.btnMoveAllLeft);
            this.Name = "ColumnPreferenceControl";
            this.Size = new System.Drawing.Size(296, 166);
            this.ResumeLayout(false);

        }
        #endregion

        #region Public Methods

        public void ClearAvailableColumns()
        {
            _availableColumns.Clear();
            lstAvailableCol.DataSource = _availableColumns;
        }

        public void ClearDisplayedColumns()
        {
            _displayedColumns.Clear();
            lstDisplayedCol.DataSource = _displayedColumns;
        }

        #endregion Public Methods

        #region Public Properties/ Events

        /// <summary>
        /// Available columns are of string type.
        /// </summary>
        private ArrayList _availableColumns;
        public ArrayList AvailableColumns
        {
            get
            {
                _availableColumns = (ArrayList)lstAvailableCol.DataSource;
                return _availableColumns;
            }
            set
            {
                if (value != null)
                {
                    _availableColumns = value;
                    lstAvailableCol.DataSource = _availableColumns;
                }
            }
        }

        /// <summary>
        /// Display Column list is the collection of string
        /// </summary>
        private ArrayList _displayedColumns;
        public ArrayList DisplayedColumns
        {
            get
            {
                _displayedColumns = (ArrayList)lstDisplayedCol.DataSource;
                return _displayedColumns;
            }
            set
            {
                if (value != null)
                {
                    _displayedColumns = value;
                    SubstractAvailableList(_displayedColumns);
                    lstDisplayedCol.DataSource = _displayedColumns;
                }
            }
        }

        #endregion Public Properties

        #region Control Events


        /// <summary>
        /// Whenever the source changes, fire the event that datasource has changed
        /// listener will simply take the latest values of list boxes from public properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstAvailableCol_DataSourceChanged(object sender, EventArgs e)
        {
            //if(AvailableColumnsChanged != null)
            //    AvailableColumnsChanged();
        }

        /// <summary>
        /// Whenever the source changes, fire the event that datasource has changed
        /// listener will simply take the latest values of list boxes from public properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstDisplayedCol_DataSourceChanged(object sender, EventArgs e)
        {
            //if(DisplayColumnsChanged != null)
            //    DisplayColumnsChanged();

        }

        private void btnMoveRight_Click(object sender, System.EventArgs e)
        {
            try
            {
                MoveItem(lstAvailableCol, lstDisplayedCol);
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

        private void btnMoveAllRight_Click(object sender, System.EventArgs e)
        {
            try
            {
                MoveAllItemsRight(lstAvailableCol, lstDisplayedCol);
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

        private void btnMoveLeft_Click(object sender, System.EventArgs e)
        {
            try
            {

                if (lstDisplayedCol.Items.Count > 1)
                {
                    MoveItem(lstDisplayedCol, lstAvailableCol);
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

        private void btnMoveAllLeft_Click(object sender, System.EventArgs e)
        {
            try
            {
                MoveAllItemsLeft(lstDisplayedCol, lstAvailableCol);
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

        private void btnUp_Click(object sender, System.EventArgs e)
        {
            try
            {
                MoveUpDown(lstDisplayedCol, true);
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

        private void btnDown_Click(object sender, System.EventArgs e)
        {
            try
            {
                MoveUpDown(lstDisplayedCol, false);
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

        #endregion Button clicks

        #region Item moving functions in between the list boxes


        public event EventHandler ColumnPreferencesChanged;

        /// <summary>
        /// Single item transferred from source to destination listbox
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void MoveItem(ListBox source, ListBox destination)
        {
            try
            {
                if (source.Items.Count > 0)
                {
                    System.Windows.Forms.ListBox.SelectedObjectCollection selected = source.SelectedItems;

                    string[] list = new string[selected.Count];
                    int index = 0;

                    //destination.DataSource = null;
                    ArrayList dsDestination = null;
                    if (destination.DataSource == null)
                    {
                        dsDestination = new ArrayList();
                    }
                    else
                    {
                        dsDestination = (ArrayList)destination.DataSource;
                    }

                    // add to destination list
                    foreach (object obj in selected)
                    {
                        //destination.Items.Add(obj.ToString());
                        // list the items to be copied to destination (to be deleted later)
                        list[index++] = obj.ToString();
                        dsDestination.Add(obj.ToString());
                    }

                    destination.DataSource = null;
                    destination.DataSource = dsDestination;


                    // delete from source the objects that have been added to destination
                    ArrayList dsList = new ArrayList();
                    foreach (object obj in source.Items)
                    {
                        dsList.Add(obj.ToString());
                    }

                    for (int i = 0; i < list.Length; i++)
                    {
                        if (dsList.Contains(list[i]))
                        {
                            dsList.Remove(list[i]);
                        }
                    }

                    source.DataSource = null;
                    source.Items.Clear();
                    source.DataSource = dsList;
                }
                if (ColumnPreferencesChanged != null)
                    ColumnPreferencesChanged(this, new EventArgs());
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

        /// <summary>
        /// All items moved to left list box
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void MoveAllItemsLeft(ListBox source, ListBox destination)
        {
            try
            {
                // copy source items in  a list
                object[] objects = new object[source.Items.Count + destination.Items.Count - 1];

                int index = 0;
                int sourceCount = int.Parse(source.Items.Count.ToString());
                if (sourceCount != 1)
                {
                    int i = 1;
                    ArrayList arrList = new ArrayList();
                    foreach (object obj in source.Items)
                    {
                        if (i <= sourceCount)
                        {
                            if (i != 1)
                            {
                                objects[index++] = obj.ToString();
                            }
                            else
                            {
                                arrList.Add(obj);
                            }
                        }
                        else
                        {
                            break;
                        }

                        i++;
                    }


                    //				foreach(object obj in source.Items)
                    //				{
                    //					objects[index++] = obj.ToString();
                    //				}

                    // add destination items in the list
                    foreach (object obj in destination.Items)
                    {
                        objects[index++] = obj.ToString();
                    }

                    //MessageBox.Show("AfterDestination" + sourceCount);
                    ///Temporarily save data in another arraylist.


                    destination.DataSource = null;
                    destination.DataSource = new ArrayList((ICollection)objects);
                    //destination.Items.Clear();
                    //destination.Items.AddRange(objects);

                    // delete all items from source and assign it to the source list
                    source.DataSource = null;
                    source.DataSource = new ArrayList((ICollection)arrList);

                    if (ColumnPreferencesChanged != null)
                        ColumnPreferencesChanged(this, new EventArgs());
                    //source.Items.Clear();
                }
                else
                {
                    MessageBox.Show("Atleast one Column should be displayed!!");
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

        /// <summary>
        /// All items moved to right list box
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void MoveAllItemsRight(ListBox source, ListBox destination)
        {
            try
            {

                // copy source items in  a list
                object[] objects = new object[source.Items.Count + destination.Items.Count];
                int index = 0;
                foreach (object obj in source.Items)
                {
                    objects[index++] = obj.ToString();
                }

                // add destination items in the list
                foreach (object obj in destination.Items)
                {
                    objects[index++] = obj.ToString();
                }

                destination.DataSource = null;
                destination.DataSource = new ArrayList((ICollection)objects);

                //destination.Items.Clear();
                //destination.Items.AddRange(objects);

                // delete from source 
                source.DataSource = null;
                source.Items.Clear();

                if (ColumnPreferencesChanged != null)
                    ColumnPreferencesChanged(this, new EventArgs());

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

        /// <summary>
        /// Functionality to move up or down
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="moveUp"></param>
        /// <param name="name"></param>
        private void MoveUpDown(ListBox listBox, bool moveUp)
        {
            try
            {
                if (listBox.SelectedItem == null)
                {
                    return;
                }

                string selectedItem = listBox.SelectedItem.ToString();
                int selectedIndex = Convert.ToInt32(listBox.SelectedIndex);

                // copy source items in a list
                ArrayList list = new ArrayList();
                foreach (object obj in listBox.Items)
                {
                    list.Add(obj.ToString());
                }

                if (moveUp && selectedIndex != 0)
                {
                    list[selectedIndex] = list[selectedIndex - 1].ToString();
                    list[selectedIndex - 1] = selectedItem;
                    selectedIndex--;
                }
                else if (!moveUp && (selectedIndex != (listBox.Items.Count - 1)))
                {
                    list[selectedIndex] = list[selectedIndex + 1].ToString();
                    list[selectedIndex + 1] = selectedItem;
                    selectedIndex++;
                }

                listBox.DataSource = null;
                listBox.Items.Clear();
                listBox.DataSource = list;
                listBox.SelectedIndex = selectedIndex;

                if (ColumnPreferencesChanged != null)
                    ColumnPreferencesChanged(this, new EventArgs());
            }
            catch
            {
            }

        }

        /// <summary>
        /// Removes the displaylist from the original available columns list and bind to display into
        /// available columns
        /// </summary>
        /// <param name="displayList"></param>
        private void SubstractAvailableList(ArrayList displayList)
        {
            try
            {
                //We can not change the list box directly as the data source has been assigned.
                ///If any of the values from _availableColumns are in the displaylist then simply, remove those  
                ArrayList originallist = _availableColumns;

                if (originallist.Count > 0)
                {
                    foreach (string s in displayList)
                    {
                        if (originallist.Contains(s))
                            originallist.Remove(s);
                    }
                }

                lstAvailableCol.DataSource = originallist;
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

        #endregion Item moving functions in between the list boxes


    }
}
