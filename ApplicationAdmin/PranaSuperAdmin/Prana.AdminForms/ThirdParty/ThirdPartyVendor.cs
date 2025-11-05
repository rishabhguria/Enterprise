using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.ThirdPartyManager.DataAccess;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.AdminForms.ThirdParty
{
    public partial class ThirdPartyVendor : Form
    {
        const int TYPE_VENDOR = 2;
        private const string FORM_NAME = "ThirdPartyVendor: ";

        public ThirdPartyVendor()
        {
            InitializeComponent();
            BindVendorTree();
            SetUpMenuPermissions();
        }

        private bool chkAddVendor = false;
        private bool chkDeleteVendor = false;
        private bool chkEditVendor = false;
        //This method fetches the user permissions from the database.
        private void SetUpMenuPermissions()
        {
            Admin.BLL.Preferences preferences = Admin.BLL.Preferences.Instance;
            chkAddVendor = preferences.Add_Vendor;
            chkDeleteVendor = preferences.Delete_Vendor;
            chkEditVendor = preferences.Edit_Vendor;
            //If the user doesnt have the permissions to add or delete ThirdParties then the respecive Add or Delete buttons are
            //disabled so that he/she can't add or delete the ThirdParties.
            if (chkAddVendor == false)
            {
                btnAdd.Enabled = false;
            }
            if (chkDeleteVendor == false)
            {
                btnDelete.Enabled = false;
            }
            if (chkEditVendor == false)
            {
                btnSave.Enabled = false;
            }
        }

        /// <summary>
        /// Binds left tree with relevent data.
        /// </summary>
        private void BindVendorTree()
        {
            try
            {
                bool gotFirstNode = false;
                Font font = new Font("Vedana", 8.25F, System.Drawing.FontStyle.Bold);

                ////Creating PrimeBrokerClearer a root node.
                //TreeNode treeNodeVendorRoot = new TreeNode("Vendor");
                ////Making the root node to bold by assigning it to the font object defined above. 
                //treeNodeVendorRoot.NodeFont = font;
                //NodeDetails vendorNode = new NodeDetails(NodeType.Vendor, int.MinValue);
                //treeNodeVendorRoot.Tag = vendorNode;

                //Creating Vendor a root node.
                TreeNode treeNodeVendorRoot = new TreeNode("Vendor");
                //Making the root node to bold by assigning it to the font object defined above. 
                treeNodeVendorRoot.NodeFont = font;
                NodeDetails vendorNode = new NodeDetails(NodeType.Vendor, int.MinValue);
                treeNodeVendorRoot.Tag = vendorNode;

                trvVendor.Nodes.Clear();

                ThirdParties thirdParties = ThirdPartyDataManager.GetAllThirdPartiesForTree();
                ThirdParties thirdPartyVendors = new ThirdParties();
                foreach (Prana.BusinessObjects.ThirdParty thirdParty in thirdParties)
                {
                    if (ThirdPartyDataManager.GetThirdPartyTypeId(thirdParty) == TYPE_VENDOR)
                    {
                        thirdPartyVendors.Add(thirdParty);
                    }
                }

                int entityType = int.MinValue;
                //Loop through all the ThirdParties and Vendors, assigning each node an id 
                //corresponding to its unique value in the database.
                foreach (Prana.BusinessObjects.ThirdParty thirdParty in thirdPartyVendors)
                {
                    if (gotFirstNode == false)
                    {
                        gotFirstNode = true;
                        entityType = int.Parse(ThirdPartyDataManager.GetThirdPartyTypeId(thirdParty).ToString());
                    }
                    switch (ThirdPartyDataManager.GetThirdPartyTypeId(thirdParty))
                    {
                        case TYPE_VENDOR:
                            TreeNode treeNodeVendor = new TreeNode(thirdParty.ShortName);
                            vendorNode = new NodeDetails(NodeType.Vendor, thirdParty.ThirdPartyID);
                            treeNodeVendor.Tag = vendorNode;
                            treeNodeVendorRoot.Nodes.Add(treeNodeVendor);
                            break;
                    }
                }
                trvVendor.Nodes.Add(treeNodeVendorRoot);

                trvVendor.ExpandAll();
                if (thirdParties.Count > 0 && gotFirstNode == true)
                {

                    NodeDetails selectNodeDetails = new NodeDetails(NodeType.Vendor, 2);
                    if (entityType == 2)
                    {
                        trvVendor.SelectedNode = trvVendor.Nodes[0].Nodes[0];
                    }
                }
                else
                {
                    trvVendor.SelectedNode = trvVendor.Nodes[0];
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("BindThirdPartyTree",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "BindThirdPartyTree", null);

                #endregion
            }
        }

        #region NodeDetails
        //Creating class NodeDetails to be used for the purpose of tree giving it some methods & properties.
        class NodeDetails
        {
            private NodeType _type = NodeType.Vendor;
            private int _nodeID = int.MinValue;

            public NodeDetails()
            {
            }

            public NodeDetails(NodeType type, int nodeID)
            {
                _type = type;
                _nodeID = nodeID;
            }

            public NodeType Type
            {
                get { return _type; }
                set { _type = value; }
            }
            public int NodeID
            {
                get { return _nodeID; }
                set { _nodeID = value; }
            }
        }
        //Creating enumeration to be used to distinguish tree nodetype on the basis of Vendor
        enum NodeType
        {
            Vendor = 1
        }

        #endregion

        /// <summary>
        /// This method shows the details of the selected node on the click event of the tree. It fetches the
        /// details of the selected node from the database by sending the nodeID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvVendor_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            try
            {
                NodeDetails nodeDetails = (NodeDetails)trvVendor.SelectedNode.Tag;
                if (trvVendor.SelectedNode == null)
                {
                    //					Prana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
                    //					Prana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "Please select ThirdParty to be shown with the details!");
                }
                else
                {
                    uctVendor.SetupControl();
                    int thirdPartyID = nodeDetails.NodeID;
                    Prana.BusinessObjects.ThirdParty thirdParty = ThirdPartyDataManager.GetThirdParty(thirdPartyID);
                    uctVendor.SetVendorDetails(thirdParty);
                    //					tabThirdParty.Show();

                    if (chkAddVendor == false && nodeDetails.NodeID == int.MinValue)
                    {
                        uctVendor.DisableVendorControls();
                    }
                    else
                    {
                        uctVendor.EnableVendorControls();
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("trvThirdParty_AfterSelect",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "trvThirdParty_AfterSelect", null);

                #endregion
            }
        }

        /// <summary>
        /// This method adds the new Third Party or Vendor on the click event of the button. 
        /// It adds the Third Party or Vendor based on the tree selection before the add 
        /// button is clicked. It the selection was on the Third Party root or child then the Third Party is 
        /// selected. Similarly Vendors can be added depending upon the nodes selection in the tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>	
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (trvVendor.SelectedNode == null)
                {
                    //						Prana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
                    //						Prana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "Please select ThirdParty!");
                }
                else
                {
                    NodeDetails nodeDetails = (NodeDetails)trvVendor.SelectedNode.Tag;
                    //						stbThirdParty.Text = "Enter details.";					
                    uctVendor.RefreshVendorDetails();

                    if (nodeDetails.NodeID != int.MinValue)
                    {
                        trvVendor.SelectedNode = trvVendor.SelectedNode.Parent;
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnAdd_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnAdd_Click", null);

                #endregion
            }
        }

        /// <summary>
        /// This method saves the Vendor details or the vendor details as per the selected node in the tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                Prana.BusinessObjects.ThirdParty thirdPartyVendor = new Prana.BusinessObjects.ThirdParty();

                int result = int.MinValue;
                if (uctVendor.GetVendorDetailsForSave(thirdPartyVendor) != false)
                {
                    thirdPartyVendor.ThirdPartyID = ((NodeDetails)trvVendor.SelectedNode.Tag).NodeID;
                    result = ThirdPartyDataManager.SaveThirdParty(thirdPartyVendor);
                    if (result == -1)
                    {
                        MessageBox.Show("Vendor with the same short name already exists.", "Prana Alert", MessageBoxButtons.OK);
                    }
                    else
                    {
                        //						Prana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
                        //						Prana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "ThirdParty saved.");
                        BindVendorTree();

                        NodeDetails selectNodeDetails = new NodeDetails(NodeType.Vendor, result);
                        SelectTreeNode(selectNodeDetails);
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);

                #endregion
            }
        }

        /// <summary>
        /// This method closes the application on clicking of this button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// This method deletes the selected thirdparty or vendor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                string result = string.Empty;
                if (trvVendor.SelectedNode.Parent != null)
                {
                    if (trvVendor.SelectedNode == null)
                    {
                        //						Prana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
                        //						Prana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "Please select ThirdParty to be deleted!");
                    }
                    else
                    {
                        NodeDetails prevNodeDetails = new NodeDetails();
                        if (trvVendor.SelectedNode.PrevNode != null)
                        {
                            prevNodeDetails = (NodeDetails)trvVendor.SelectedNode.PrevNode.Tag;
                        }
                        else
                        {
                            prevNodeDetails = (NodeDetails)trvVendor.SelectedNode.Parent.Tag;
                        }

                        NodeDetails nodeDetails = (NodeDetails)trvVendor.SelectedNode.Tag;

                        int thirdPartyID = nodeDetails.NodeID;

                        bool isPermanentDeletion = CachedDataManager.GetInstance.IsPermanentDeletionEnabled();
                        if (MessageBox.Show(this, "Do you want to delete selected Vendor?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            result = ThirdPartyDataManager.DeleteThirdParty(thirdPartyID, isPermanentDeletion);
                            if (result != string.Empty)
                            {
                                MessageBox.Show(this, result, "Prana Alert");
                            }
                            else
                            {
                                BindVendorTree();
                                SelectTreeNode(prevNodeDetails);
                                //								Prana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
                                //								Prana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "ThirdParty deleted!");
                            }
                        }

                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("button1_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "button1_Click", null);

                #endregion
            }
        }

        /// <summary>
        /// This method selects the node in the tree based on the parameter passed to it in nodedetails. 
        /// </summary>
        /// <param name="nodeDetails"></param>
        private void SelectTreeNode(NodeDetails nodeDetails)
        {
            foreach (TreeNode node in trvVendor.Nodes[0].Nodes)
            {
                if (((NodeDetails)node.Tag).NodeID == nodeDetails.NodeID)
                {
                    trvVendor.SelectedNode = node;
                    break;
                }
            }
        }
    }
}