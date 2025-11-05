using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Controls;
using System.Text;

using Nirvana.Admin.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for ThirdParty.
	/// </summary>
	public class ThirdPartyForm : System.Windows.Forms.Form
	{
		#region Constant Definitions
		private const string FORM_NAME = "ThirdPartyForm: ";
		//Tab Constants
		const int C_TAB_THIRDPARTY = 0;
		const int C_TAB_COMMISSIONS = 1;

		//Third party type constants
		const int C_TYPE_PRIMEBROKERCLEARER = 1;
		const int C_TYPE_VENDOR = 2;
		const int C_TYPE_CUSTODIAN = 3;
		const int C_TYPE_ADMINISTRATOR = 4;
	
		const string C_COMBO_SELECT = "- Select -";	
		#endregion

		
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TreeView trvThirdParty;
		private System.Windows.Forms.Panel panel1;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcThirdParty;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Nirvana.Admin.Controls.ThirdPartyForm uctThirdPartyContactDetails;
		//private Nirvana.Admin.Controls.ThirdPartyForm uctThirdPartyContactDetails;
		
		
		

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ThirdPartyForm()
		{
			InitializeComponent();			
			//Binds the tree as soon as the class constructor is called.
			BindThirdPartyTree();
		}
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ThirdPartyForm));
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.uctThirdPartyContactDetails = new Nirvana.Admin.Controls.ThirdPartyForm();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.trvThirdParty = new System.Windows.Forms.TreeView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tbcThirdParty = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.ultraTabPageControl1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbcThirdParty)).BeginInit();
			this.tbcThirdParty.SuspendLayout();
			this.SuspendLayout();
			// 
			// ultraTabPageControl1
			// 
			this.ultraTabPageControl1.Controls.Add(this.uctThirdPartyContactDetails);
			this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl1.Name = "ultraTabPageControl1";
			this.ultraTabPageControl1.Size = new System.Drawing.Size(358, 293);
			// 
			// uctThirdPartyContactDetails
			// 
			this.uctThirdPartyContactDetails.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctThirdPartyContactDetails.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctThirdPartyContactDetails.Location = new System.Drawing.Point(10, 6);
			this.uctThirdPartyContactDetails.Name = "uctThirdPartyContactDetails";
			this.uctThirdPartyContactDetails.Size = new System.Drawing.Size(336, 284);
			this.uctThirdPartyContactDetails.TabIndex = 0;
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDelete.Location = new System.Drawing.Point(80, 2);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Click += new System.EventHandler(this.button1_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnAdd.Location = new System.Drawing.Point(0, 2);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(366, 2);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 5;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(288, 2);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 4;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// trvThirdParty
			// 
			this.trvThirdParty.BackColor = System.Drawing.Color.White;
			this.trvThirdParty.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.trvThirdParty.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.trvThirdParty.FullRowSelect = true;
			this.trvThirdParty.HideSelection = false;
			this.trvThirdParty.ImageIndex = -1;
			this.trvThirdParty.Location = new System.Drawing.Point(2, 2);
			this.trvThirdParty.Name = "trvThirdParty";
			this.trvThirdParty.SelectedImageIndex = -1;
			this.trvThirdParty.Size = new System.Drawing.Size(156, 322);
			this.trvThirdParty.TabIndex = 7;
			this.trvThirdParty.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvThirdParty_AfterSelect);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnDelete);
			this.panel1.Controls.Add(this.btnAdd);
			this.panel1.Controls.Add(this.btnSave);
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Location = new System.Drawing.Point(2, 328);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(612, 34);
			this.panel1.TabIndex = 9;
			// 
			// tbcThirdParty
			// 
			appearance1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance1.BackColor2 = System.Drawing.Color.White;
			appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tbcThirdParty.ActiveTabAppearance = appearance1;
			this.tbcThirdParty.Controls.Add(this.ultraTabSharedControlsPage1);
			this.tbcThirdParty.Controls.Add(this.ultraTabPageControl1);
			this.tbcThirdParty.Location = new System.Drawing.Point(164, 8);
			this.tbcThirdParty.Name = "tbcThirdParty";
			this.tbcThirdParty.SharedControlsPage = this.ultraTabSharedControlsPage1;
			this.tbcThirdParty.Size = new System.Drawing.Size(360, 314);
			this.tbcThirdParty.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tbcThirdParty.TabIndex = 10;
			ultraTab1.Key = "tabThirdParty";
			ultraTab1.TabPage = this.ultraTabPageControl1;
			ultraTab1.Text = "ThirdParty";
			this.tbcThirdParty.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																									ultraTab1});
			// 
			// ultraTabSharedControlsPage1
			// 
			this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
			this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(358, 293);
			// 
			// ThirdPartyForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(532, 389);
			this.Controls.Add(this.tbcThirdParty);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.trvThirdParty);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "ThirdPartyForm";
			this.Text = "ThirdParty";
			this.Load += new System.EventHandler(this.ThirdPartyForm_Load);
			this.ultraTabPageControl1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbcThirdParty)).EndInit();
			this.tbcThirdParty.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// This method deletes the selected thirdparty or vendor.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, System.EventArgs e)
		{
			try
			{
				bool result = false;
				if(trvThirdParty.SelectedNode.Parent != null)
				{
					if(trvThirdParty.SelectedNode == null)
					{
//						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
//						Nirvana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "Please select ThirdParty to be deleted!");
					}
					else
					{
						NodeDetails prevNodeDetails = new NodeDetails();
						if(trvThirdParty.SelectedNode.PrevNode != null)
						{
							prevNodeDetails = (NodeDetails)trvThirdParty.SelectedNode.PrevNode.Tag;
						}
						else
						{
							prevNodeDetails = (NodeDetails)trvThirdParty.SelectedNode.Parent.Tag;
						}

						NodeDetails nodeDetails = (NodeDetails)trvThirdParty.SelectedNode.Tag;
				
						int thirdPartyID = nodeDetails.NodeID;
						if(MessageBox.Show(this, "Do you want to delete selected Third Party?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
						{	
							result = ThirdPartyManager.DeleteThirdParty(thirdPartyID);
							if(result == false)
							{
								MessageBox.Show(this, "ThirdParty is referenced in CompanyThirdParty.\n You Please remove references first to delete it.", "Nirvana Alert");
							}
							else
							{
								BindThirdPartyTree();
								SelectTreeNode(prevNodeDetails);
//								Nirvana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
//								Nirvana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "ThirdParty deleted!");
							}
						}
					
					}
				}
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				#endregion

			finally
			{
				#region LogEntry
				LogEntry logEntry = new LogEntry("button1_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "button1_Click"); 
				Logger.Write(logEntry); 
				#endregion
			}
		}

		/// <summary>
		/// Binds left tree with relevent data.
		/// </summary>
		private void BindThirdPartyTree()		
		{
			try
			{
				bool gotFirstNode = false;
				Font font = new Font("Vedana",8.25F, System.Drawing.FontStyle.Bold);
				
				//Creating PrimeBrokerClearer a root node.
				TreeNode treeNodePrimeBrokerClearerRoot = new TreeNode("PrimeBrokerClearer");
				//Making the root node to bold by assigning it to the font object defined above. 
				treeNodePrimeBrokerClearerRoot.NodeFont = font;
				NodeDetails primeBrokerClearerNode = new NodeDetails(NodeType.PrimeBrokerClearer, int.MinValue); 
				treeNodePrimeBrokerClearerRoot.Tag = primeBrokerClearerNode;

				//Creating Vendor a root node.
				TreeNode treeNodeVendorRoot = new TreeNode("Vendor");
				//Making the root node to bold by assigning it to the font object defined above. 
				treeNodeVendorRoot.NodeFont = font;
				NodeDetails vendorNode = new NodeDetails(NodeType.Vendor, int.MinValue); 
				treeNodeVendorRoot.Tag = vendorNode;

				//Creating Custodian a root node.
				TreeNode treeNodeCustodianRoot = new TreeNode("Custodian");
				//Making the root node to bold by assigning it to the font object defined above. 
				treeNodeCustodianRoot.NodeFont = font;
				NodeDetails custodianNode = new NodeDetails(NodeType.Custodian, int.MinValue); 
				treeNodeCustodianRoot.Tag = custodianNode;

				//Creating Administrator a root node.
				TreeNode treeNodeAdministratorRoot = new TreeNode("Administrator");
				//Making the root node to bold by assigning it to the font object defined above. 
				treeNodeAdministratorRoot.NodeFont = font;
				NodeDetails administratorNode = new NodeDetails(NodeType.Administrator, int.MinValue); 
				treeNodeAdministratorRoot.Tag = administratorNode;
			
				trvThirdParty.Nodes.Clear();

				ThirdParties thirdParties = ThirdPartyManager.GetAllThirdPartiesForTree();

				int entityType = int.MinValue;
				//Loop through all the ThirdParties and Vendors, assigning each node an id 
				//corresponding to its unique value in the database.
				foreach(ThirdParty thirdParty in thirdParties)
				{					
					if(gotFirstNode == false)
					{
						gotFirstNode = true;
						entityType = int.Parse(thirdParty.ThirdPartyTypeID.ToString());
					}
					switch(thirdParty.ThirdPartyTypeID)
					{
						case C_TYPE_PRIMEBROKERCLEARER:
							TreeNode treeNodePrimeBrokerClearer = new TreeNode(thirdParty.ShortName);
							primeBrokerClearerNode = new NodeDetails(NodeType.PrimeBrokerClearer, thirdParty.ThirdPartyID); 
							treeNodePrimeBrokerClearer.Tag = primeBrokerClearerNode;
							treeNodePrimeBrokerClearerRoot.Nodes.Add(treeNodePrimeBrokerClearer);
							break;
						case C_TYPE_VENDOR:
							TreeNode treeNodeVendor = new TreeNode(thirdParty.ShortName);
							vendorNode = new NodeDetails(NodeType.Vendor, thirdParty.ThirdPartyID); 
							treeNodeVendor.Tag = vendorNode;
							treeNodeVendorRoot.Nodes.Add(treeNodeVendor);
							break;
						case C_TYPE_CUSTODIAN:
							TreeNode treeNodeCustodian = new TreeNode(thirdParty.ShortName);
							custodianNode = new NodeDetails(NodeType.Custodian, thirdParty.ThirdPartyID); 
							treeNodeCustodian.Tag = custodianNode;
							treeNodeCustodianRoot.Nodes.Add(treeNodeCustodian);
							break;
						case C_TYPE_ADMINISTRATOR:
							TreeNode treeNodeAdministrator = new TreeNode(thirdParty.ShortName);
							administratorNode = new NodeDetails(NodeType.Administrator, thirdParty.ThirdPartyID); 
							treeNodeAdministrator.Tag = administratorNode;
							treeNodeAdministratorRoot.Nodes.Add(treeNodeAdministrator);
							break;
					}
				}
				trvThirdParty.Nodes.Add(treeNodePrimeBrokerClearerRoot);
				trvThirdParty.Nodes.Add(treeNodeVendorRoot);
				trvThirdParty.Nodes.Add(treeNodeCustodianRoot);
				trvThirdParty.Nodes.Add(treeNodeAdministratorRoot);
			
				trvThirdParty.ExpandAll();
				if(thirdParties.Count > 0 && gotFirstNode == true)
				{
						
					NodeDetails selectNodeDetails = new NodeDetails(NodeType.PrimeBrokerClearer, 1);
					if (entityType == 1)
					{
						trvThirdParty.SelectedNode = trvThirdParty.Nodes[0].Nodes[0];
					}
					else if (entityType == 2)
					{
						trvThirdParty.SelectedNode = trvThirdParty.Nodes[1].Nodes[0];
					}
					else
					{
						trvThirdParty.SelectedNode = trvThirdParty.Nodes[0];
					}
					//SelectTreeNode(selectNodeDetails);
				}
				else
				{
					trvThirdParty.SelectedNode = trvThirdParty.Nodes[0];
				}
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				#endregion

			finally
			{
				#region LogEntry
				LogEntry logEntry = new LogEntry("BindThirdPartyTree", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "BindThirdPartyTree"); 
				Logger.Write(logEntry); 
				#endregion
			}
		}

		/// <summary>
		/// This method saves the Thirdparty details or the vendor details as per the selected node in the tree.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				ThirdParty thirdParty = new  ThirdParty();			
						
				int result = int.MinValue;
				if(uctThirdPartyContactDetails.GetThirdPartyDetailsForSave(thirdParty) != false)
				{
					thirdParty.ThirdPartyID = ((NodeDetails) trvThirdParty.SelectedNode.Tag).NodeID;
					result = ThirdPartyManager.SaveThirdParty(thirdParty);
					if(result == -1)
					{
//						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
//						Nirvana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "ThirdParty already exists.");
					}
					else
					{
//						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
//						Nirvana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "ThirdParty saved.");
						BindThirdPartyTree();

						NodeDetails selectNodeDetails = new NodeDetails(NodeType.PrimeBrokerClearer, result);
						SelectTreeNode(selectNodeDetails);
					}
				}
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				#endregion

			finally
			{
				#region LogEntry
				LogEntry logEntry = new LogEntry("btnSave_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnSave_Click"); 
				Logger.Write(logEntry); 
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
		/// This method shows the details of the selected node on the click event of the tree. It fetches the
		/// details of the selected node from the database by sending the nodeID.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trvThirdParty_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			try
			{
				NodeDetails nodeDetails = (NodeDetails)trvThirdParty.SelectedNode.Tag;
				if(trvThirdParty.SelectedNode == null)
				{
//					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
//					Nirvana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "Please select ThirdParty to be shown with the details!");
				}
				else
				{
					uctThirdPartyContactDetails.SetupControl();
					tbcThirdParty.SelectedTab = tbcThirdParty.Tabs[C_TAB_THIRDPARTY];
					int thirdPartyID = nodeDetails.NodeID;
					Nirvana.Admin.BLL.ThirdParty thirdParty = ThirdPartyManager.GetThirdParty(thirdPartyID);
					uctThirdPartyContactDetails.SetThirdPartyDetails(thirdParty);						
//					tabThirdParty.Show();
				}
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				#endregion

			finally
			{
				#region LogEntry
				LogEntry logEntry = new LogEntry("trvThirdParty_AfterSelect", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "trvThirdParty_AfterSelect"); 
				Logger.Write(logEntry); 
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
					if(trvThirdParty.SelectedNode == null)
					{
//						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
//						Nirvana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "Please select ThirdParty!");
					}
					else
					{
						NodeDetails nodeDetails = (NodeDetails)trvThirdParty.SelectedNode.Tag;
//						stbThirdParty.Text = "Enter details.";					
						uctThirdPartyContactDetails.RefreshThirdPartyDetails();

						tbcThirdParty.SelectedTab = tbcThirdParty.Tabs[C_TAB_THIRDPARTY];
//						tabThirdParty.Show();					

						if(nodeDetails.NodeID != int.MinValue)
						{
							trvThirdParty.SelectedNode = trvThirdParty.SelectedNode.Parent;
						}
					}
				}
					#region Catch
				catch(Exception ex)
				{
					string formattedInfo = ex.StackTrace.ToString();
					Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
						FORM_NAME);
					AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
					appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
				}
					#endregion

				finally
				{
					#region LogEntry
					LogEntry logEntry = new LogEntry("btnAdd_Click", 
						Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
						FORM_NAME + "btnAdd_Click"); 
					Logger.Write(logEntry); 
					#endregion
				}
			}

		/// <summary>
		/// This method selects the node in the tree based on the parameter passed to it in nodedetails. 
		/// </summary>
		/// <param name="nodeDetails"></param>
		private void SelectTreeNode(NodeDetails nodeDetails)
		{
			foreach(TreeNode node in trvThirdParty.Nodes[C_TAB_THIRDPARTY].Nodes)
			{
				if(((NodeDetails) node.Tag).NodeID == nodeDetails.NodeID)
				{
					trvThirdParty.SelectedNode = node;
					break;
				}
			}
		}

		private void ThirdPartyForm_Load(object sender, System.EventArgs e)
		{
		
		}

		#region Highlight Selected Tab
		//To highlight and show the currently selected node in a different color.
		private void tbcThirdParty_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Font f;
			Brush backBrush;
			Brush foreBrush;
			
			if(e.Index == this.tbcThirdParty.SelectedTab.Index)
			{
				f = new Font(e.Font, FontStyle.Regular);
				backBrush = new System.Drawing.SolidBrush(Color.Brown);
				backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
				foreBrush = Brushes.Black;
			}
			else
			{
				f = e.Font;
				backBrush = new SolidBrush(e.BackColor); 
				foreBrush = new SolidBrush(e.ForeColor);
			}

			string tabName = this.tbcThirdParty.Tabs[e.Index].Text;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.FillRectangle(backBrush, e.Bounds);
			Rectangle r = e.Bounds;
			r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
			e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

			sf.Dispose();
			if(e.Index == this.tbcThirdParty.SelectedTab.Index)
			{
				f.Dispose();
				backBrush.Dispose();
			}
			else
			{
				backBrush.Dispose();
				foreBrush.Dispose();
			}
		}	
		#endregion
		
		#region NodeDetails
		//Creating class NodeDetails to be used for the purpose of tree giving it some methods & properties.
		class NodeDetails
		{
			private NodeType _type = NodeType.PrimeBrokerClearer;
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
				get{return _type;}
				set{_type = value;}
			}
			public int NodeID
			{
				get{return _nodeID;}
				set{_nodeID = value;}
			}
		}
		//Creating enumeration to be used to distinguish tree nodetype on the basis of ThirdParty/Vendor
		enum NodeType
		{
			PrimeBrokerClearer = 1,
			Vendor = 2,
			Custodian = 3,
			Administrator = 4
		}
		
		#endregion
	}
}
