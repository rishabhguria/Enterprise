using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;		
using Nirvana.Admin.Controls;
using System.Text;
using Nirvana.Admin.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using System.Data;
using System.IO;


namespace Nirvana.Admin.CommissionRules
{
	/// <summary>
	/// Summary description for CommissionRules.
	/// </summary>
	public class CommissionRules : System.Windows.Forms.Form
	{
		const string C_COMBO_SELECT = "- Select -";
		private const string FORM_NAME = "Commission Rule : ";
		const int C_TAB_COMMISSIONRULE= 0;

		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCommissinRules;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		//private Nirvana.Admin.Controls.CommissionRule uctCommissionRule;
		
		
		private System.Windows.Forms.TreeView trvCommissionRule;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
		//private Nirvana.Admin.Controls.CommissionRule uctCommissionRule;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Panel panel1;
		private Nirvana.Admin.Controls.CommissionRule uctCommissionRule;
		private Nirvana.Admin.Controls.ViewEditCommissionRule uctViewEditRule;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CommissionRules()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CommissionRules));
			this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.uctCommissionRule = new Nirvana.Admin.Controls.CommissionRule();
			this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.uctViewEditRule = new Nirvana.Admin.Controls.ViewEditCommissionRule();
			this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.tabCommissinRules = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.trvCommissionRule = new System.Windows.Forms.TreeView();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ultraTabPageControl2.SuspendLayout();
			this.ultraTabPageControl3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabCommissinRules)).BeginInit();
			this.tabCommissinRules.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ultraTabPageControl2
			// 
			this.ultraTabPageControl2.Controls.Add(this.uctCommissionRule);
			this.ultraTabPageControl2.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl2.Name = "ultraTabPageControl2";
			this.ultraTabPageControl2.Size = new System.Drawing.Size(766, 499);
			// 
			// uctCommissionRule
			// 
			this.uctCommissionRule.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCommissionRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.uctCommissionRule.Location = new System.Drawing.Point(2, 4);
			this.uctCommissionRule.Name = "uctCommissionRule";
			this.uctCommissionRule.Size = new System.Drawing.Size(760, 462);
			this.uctCommissionRule.TabIndex = 0;
			// 
			// ultraTabPageControl3
			// 
			this.ultraTabPageControl3.Controls.Add(this.uctViewEditRule);
			this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl3.Name = "ultraTabPageControl3";
			this.ultraTabPageControl3.Size = new System.Drawing.Size(767, 500);
			// 
			// uctViewEditRule
			// 
			this.uctViewEditRule.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctViewEditRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.uctViewEditRule.Location = new System.Drawing.Point(38, 7);
			this.uctViewEditRule.Name = "uctViewEditRule";
			this.uctViewEditRule.Size = new System.Drawing.Size(710, 300);
			this.uctViewEditRule.TabIndex = 0;
			// 
			// ultraTabPageControl4
			// 
			this.ultraTabPageControl4.Location = new System.Drawing.Point(0, 0);
			this.ultraTabPageControl4.Name = "ultraTabPageControl4";
			// 
			// ultraTabPageControl1
			// 
			this.ultraTabPageControl1.Location = new System.Drawing.Point(0, 0);
			this.ultraTabPageControl1.Name = "ultraTabPageControl1";
			// 
			// tabCommissinRules
			// 
			appearance1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance1.BackColor2 = System.Drawing.Color.White;
			appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tabCommissinRules.ActiveTabAppearance = appearance1;
			this.tabCommissinRules.Controls.Add(this.ultraTabSharedControlsPage2);
			this.tabCommissinRules.Controls.Add(this.ultraTabPageControl2);
			this.tabCommissinRules.Controls.Add(this.ultraTabPageControl3);
			this.tabCommissinRules.Location = new System.Drawing.Point(156, 0);
			this.tabCommissinRules.Name = "tabCommissinRules";
			this.tabCommissinRules.SharedControlsPage = this.ultraTabSharedControlsPage2;
			this.tabCommissinRules.Size = new System.Drawing.Size(768, 520);
			this.tabCommissinRules.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tabCommissinRules.TabIndex = 3;
			ultraTab1.Key = "tbcCommissionRule";
			ultraTab1.TabPage = this.ultraTabPageControl2;
			ultraTab1.Text = "CommissionRule";
			ultraTab2.Key = "tbcView/EditRule";
			ultraTab2.TabPage = this.ultraTabPageControl3;
			ultraTab2.Text = "View/EditRule";
			this.tabCommissinRules.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																										ultraTab1,
																										ultraTab2});
			this.tabCommissinRules.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabCommissinRules_SelectedTabChanged_1);
			// 
			// ultraTabSharedControlsPage2
			// 
			this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
			this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(767, 500);
			// 
			// ultraTabSharedControlsPage1
			// 
			this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(0, 0);
			this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
			// 
			// trvCommissionRule
			// 
			this.trvCommissionRule.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.trvCommissionRule.FullRowSelect = true;
			this.trvCommissionRule.HideSelection = false;
			this.trvCommissionRule.HotTracking = true;
			this.trvCommissionRule.ImageIndex = -1;
			this.trvCommissionRule.Location = new System.Drawing.Point(4, 0);
			this.trvCommissionRule.Name = "trvCommissionRule";
			this.trvCommissionRule.SelectedImageIndex = -1;
			this.trvCommissionRule.ShowLines = false;
			this.trvCommissionRule.Size = new System.Drawing.Size(148, 520);
			this.trvCommissionRule.TabIndex = 2;
			this.trvCommissionRule.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvCommissionRule_AfterSelect);
			// 
			// btnAdd
			// 
			this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAdd.Location = new System.Drawing.Point(0, 2);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 4;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click_1);
			// 
			// btnDelete
			// 
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Location = new System.Drawing.Point(78, 2);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 5;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnSave
			// 
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(438, 4);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 6;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
			// 
			// btnClose
			// 
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Location = new System.Drawing.Point(518, 4);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 8;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnAdd);
			this.panel1.Controls.Add(this.btnDelete);
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Controls.Add(this.btnSave);
			this.panel1.Location = new System.Drawing.Point(4, 528);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(908, 34);
			this.panel1.TabIndex = 9;
			// 
			// CommissionRules
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(1028, 573);
			this.Controls.Add(this.tabCommissinRules);
			this.Controls.Add(this.trvCommissionRule);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CommissionRules";
			this.Text = "CommissionRules";
			this.Load += new System.EventHandler(this.CommissionRules_Load);
			this.ultraTabPageControl2.ResumeLayout(false);
			this.ultraTabPageControl3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tabCommissinRules)).EndInit();
			this.tabCommissinRules.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region BindCommissionRuletree
		private void BindCommissionRuleTree()
		{
			trvCommissionRule.Nodes.Clear();

			Font font = new Font("Tahoma", 11, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			TreeNode treeNodeParent=new TreeNode("CommissionRules");
			treeNodeParent.NodeFont = font;
			treeNodeParent.Tag =new NodeDetails(NodeType.CommissionRule, int.MinValue);
			AUECCommissionRules auecCommissionRules = CommissionRuleManager.GetAUECCommissionRules();
			foreach(AUECCommissionRule auecCommissionRule in auecCommissionRules)
			{	
				TreeNode treeNodeCommission=new TreeNode( auecCommissionRule.RuleName);
				NodeDetails AUECCommissionRuleNode=
					new NodeDetails(NodeType.CommissionRule ,auecCommissionRule.RuleID);
				treeNodeCommission.Tag = AUECCommissionRuleNode;
				treeNodeParent.Nodes.Add(treeNodeCommission);				
			}	
			trvCommissionRule.Nodes.Add(treeNodeParent);
			trvCommissionRule.ExpandAll();		
			//trvCommissionRule.Focus();
			if(treeNodeParent.Nodes.Count > 0)
			{
				trvCommissionRule.SelectedNode = trvCommissionRule.Nodes[0].Nodes[0];
			}
			else
			{
				trvCommissionRule.SelectedNode = trvCommissionRule.Nodes[0];
			}
			
			//trvCommissionRule.SelectedNode=trvCommissionRule.Nodes[C_TAB_COMMISSIONRULE];
		}
		#endregion



		private void SelectTreeNode(NodeDetails nodeDetails)
		{
			foreach(TreeNode node in trvCommissionRule.Nodes[0].Nodes)
			{
				if(((NodeDetails)node.Tag).NodeID==nodeDetails.NodeID)
				{
					trvCommissionRule.SelectedNode=node;
					break;
				}
			}
			
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
		
		}
		#region NodeDetails
		//Creating class NodeDetails to be used for the purpose of tree giving it some methods & properties.
		class NodeDetails
		{
			private NodeType _type = NodeType.CommissionRule;
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
			CommissionRule = 1,
			
		}
		
		#endregion
	

		private void uctCommissionRule_Load(object sender, System.EventArgs e)
		{
			
		}

		private void trvCommissionRule_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			try
			{

				if(trvCommissionRule.SelectedNode!=null)
				{
					if(trvCommissionRule.SelectedNode!= trvCommissionRule.Nodes[0] )
					{
						NodeDetails nodeDetails = (NodeDetails)trvCommissionRule.SelectedNode.Tag;
						int ruleID = nodeDetails.NodeID;
						
						Nirvana.Admin.BLL.AUECCommissionRule auecCommissionRule = CommissionRuleManager.GetAUECCommissionRule(ruleID);
						uctCommissionRule.auecCommissionRuleproperty = auecCommissionRule;
						
						Nirvana.Admin.BLL.CommissionCriteria commissionCriteria=CommissionRuleManager.GetCommissionCriteriaByRuleID(auecCommissionRule.RuleID);
						if(commissionCriteria != null)
						{
							uctCommissionRule.commissionCriteriaproperty = commissionCriteria;

							Nirvana.Admin.BLL.CommissionRuleCriteria commissionRuleCriteria=CommissionRuleManager.GetCommissionRuleCriteria(commissionCriteria.CommissionCriteriaID);
							if(commissionRuleCriteria != null)
							{
								uctCommissionRule.commissionRuleCriteriaproperty = commissionRuleCriteria;
							}
						}
						
						
					}
					else
					{
						uctCommissionRule.RefreshCommissionCriteriaDetails();
						uctCommissionRule.RefreshCommissionRuleCriteriaDetails();
						uctCommissionRule.RefreshCommissionRuleDetails();
					}
				//	trvCommissionRule.SelectedNode = trvCommissionRule.Nodes[0].Nodes[0];
					//tabCommissinRules.Tabs[0].Selected = true;	

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
				LogEntry logEntry = new LogEntry("trvCommissionRule_AfterSelect", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "trvCommissionRule_AfterSelect"); 
				Logger.Write(logEntry); 
				#endregion
			}
	
		}	

		private void CommissionRules_Load(object sender, System.EventArgs e)
		{
			BindCommissionRuleTree();
		}

		
		
				
		public void RefreshCommissionRule()
		{
			uctCommissionRule.RefreshCommissionRuleCriteriaDetails();
			uctCommissionRule.RefreshCommissionCriteriaDetails();
			uctCommissionRule.RefreshCommissionRuleDetails();
		}
		
					

		private void tabCommissinRules_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
		
		}

		
		

		private void ultraTabPageControl4_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		
		private void btnSave_Click_1(object sender, System.EventArgs e)
		{
		try
			{
				//NodeDetails nodeDetails =new NodeDetails();
				if(trvCommissionRule.SelectedNode != null)
				{
					NodeDetails nodeDetails = (NodeDetails) trvCommissionRule.SelectedNode.Tag;
					AUECCommissionRule auecCommissionRule = uctCommissionRule.auecCommissionRuleproperty;

					if(auecCommissionRule != null)
					{
						auecCommissionRule.RuleID = nodeDetails.NodeID;
					}
					int ruleID = nodeDetails.NodeID;
					
					if(auecCommissionRule!=null)
					{
						ruleID=int.Parse(CommissionRuleManager.SaveAUECCommissionRule(auecCommissionRule, ruleID).ToString());

						CommissionCriteria commissionCriteria=uctCommissionRule.commissionCriteriaproperty;
						if(commissionCriteria!= null)
						{
							int CommissionCriteriaID=int.Parse(CommissionRuleManager.SaveCommissionCriteria(commissionCriteria, ruleID).ToString());

							CommissionRuleCriteria commissionRuleCriteria=uctCommissionRule.commissionRuleCriteriaproperty;
							
							if(commissionRuleCriteria!= null)
							{
								int CommissionRuleCriteriaID=int.Parse(CommissionRuleManager.SaveCommissionRuleCriteria(commissionRuleCriteria,CommissionCriteriaID).ToString());

								if(CommissionRuleCriteriaID != int.MinValue)
								{
//									BindCommissionRuleTree();
//									SelectTreeNode(nodeDetails);
//									ruleID = int.Parse(nodeDetails.NodeID.ToString());
//									NodeDetails selectNodeDetails = new NodeDetails(NodeType.CommissionRule,ruleID);
									
								}
							}
							
						}
//						BindCommissionRuleTree();
//						//SelectTreeNode(nodeDetails);
//						ruleID = int.Parse(nodeDetails.NodeID.ToString());
//						NodeDetails selectNodeDetails = new NodeDetails(NodeType.CommissionRule,ruleID);
					}
				
					
					BindCommissionRuleTree();
					 SelectTreeNode(nodeDetails);
					ruleID = int.Parse(nodeDetails.NodeID.ToString());
					NodeDetails selectNodeDetails = new NodeDetails(NodeType.CommissionRule,ruleID);
				
				}
		
		else
			{
				//tabCommissinRules.SelectedTab = tabCommissinRules.Tabs[0];
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

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
		bool result = false;
			if(trvCommissionRule.SelectedNode!=null)
			{
				NodeDetails nodedetails = (NodeDetails) trvCommissionRule.SelectedNode.Tag;
				if(nodedetails.NodeID==int.MinValue)
				{
					MessageBox.Show(this,"Please Select a valid Rule to Delete","Nirvana Alert",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					if(MessageBox.Show(this, "Are you sure you want to delete the selected Rule?", "Nirvana Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						int ruleID=nodedetails.NodeID;
						if(!CommissionRuleManager.DeleteCommissionRule(ruleID,false))
						{
						
								CommissionRuleManager.DeleteCommissionRule(ruleID,true);
								result=true;
							
					}
						else
						{
							result=true;
						}
					}
				}
				if(result == true)
				{
					BindCommissionRuleTree();
				}
			}
		}

		private void btnClose_Click_1(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnAdd_Click_1(object sender, System.EventArgs e)
		{
			
			try
			{
				if(trvCommissionRule.SelectedNode == null)
				{
					
				}
				else
				{
					NodeDetails nodeDetails = (NodeDetails)trvCommissionRule.SelectedNode.Tag;
					
					if(nodeDetails.Type == NodeType.CommissionRule)
					{
						int ruleID = nodeDetails.NodeID;
						
						trvCommissionRule.SelectedNode=trvCommissionRule.SelectedNode.Parent;
						RefreshCommissionRule();
						
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

		private void tabCommissinRules_SelectedTabChanged_1(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
			if (tabCommissinRules.SelectedTab == tabCommissinRules.Tabs[1])
			{
				uctViewEditRule.SetUp();
			}
		}

	

						
	}

		
	
}
