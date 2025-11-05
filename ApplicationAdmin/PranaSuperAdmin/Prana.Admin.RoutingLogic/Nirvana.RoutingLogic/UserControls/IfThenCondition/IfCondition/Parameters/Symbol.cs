using Prana.Admin.RoutingLogic.MisclFunctions;
using System.IO;
using System.Windows.Forms;

namespace Prana.Admin.RoutingLogic.Controls
{
    /// <summary>
    /// Summary description for Symbol.
    /// </summary>
    public class Symbol : System.Windows.Forms.UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtedt;
        private System.Windows.Forms.Button btnOpenFile;

        private System.Data.DataSet dsData; private BLL.DataRoutingLogicObjects dataRL;
        private string strMemoryID;
        private string strTabID;
        private int iRoutingIndex;
        private const int CONST_MAX_SYMBOL = 50;

        public Symbol(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, string _strIndex, string _strMemoryID, string _strTabID, int _iRoutingIndex)
        {
            this.dsData = _dsData; this.dataRL = _dataRL; this.dataRL = _dataRL;
            this.Tag = _strIndex;
            this.iRoutingIndex = _iRoutingIndex;
            this.strMemoryID = _strMemoryID;
            this.strTabID = _strTabID;
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            this.txtedt.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.txtedt.Enter += new System.EventHandler(Functions.object_GotFocus);

            LoadData();

        }
        public Symbol()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

        }

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
                if (openFileDialog != null)
                {
                    openFileDialog.Dispose();
                }
                if (txtedt != null)
                {
                    txtedt.Dispose();
                }
                if (btnOpenFile != null)
                {
                    btnOpenFile.Dispose();
                }

            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Symbol));
            this.txtedt = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtedt)).BeginInit();
            this.SuspendLayout();
            // 
            // txtedt
            // 
            this.txtedt.AllowDrop = true;
            this.txtedt.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtedt.Location = new System.Drawing.Point(0, 0);
            this.txtedt.Name = "txtedt";
            this.txtedt.Size = new System.Drawing.Size(78, 20);
            this.txtedt.TabIndex = 0;
            this.txtedt.Leave += new System.EventHandler(this.CommitMemory);
            this.txtedt.DragOver += new System.Windows.Forms.DragEventHandler(this.DragFileUpload);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "txt";
            this.openFileDialog.Filter = "Symbol, CSV File | *.txt ";
            this.openFileDialog.Title = "Symbols";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.FileUpload);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenFile.Image")));
            this.btnOpenFile.Location = new System.Drawing.Point(78, 0);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(22, 20);
            this.btnOpenFile.TabIndex = 1;
            this.btnOpenFile.Tag = "btnOpenFile";
            this.btnOpenFile.Click += new System.EventHandler(this.ShowOpenFileDialoge);
            // 
            // Symbol
            // 
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txtedt);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Name = "Symbol";
            this.Size = new System.Drawing.Size(100, 20);
            ((System.ComponentModel.ISupportInitialize)(this.txtedt)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        #region LoadData

        private void LoadData()
        {

            string _strIndex = this.Tag.ToString().Trim();//((System.Windows.Forms.Panel)(this.Parent)).Tag.ToString().Trim().Remove(0,"panel".Length);
            if (int.Parse(_strIndex) >= this.dataRL.ConditionsCount(strTabID, iRoutingIndex))
            {
                this.Hide();
                return;
            }
            else
            {
                this.Show();
            }


            //			object _objMemoryValue = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterValue"+_strIndex];
            //			this.txtedt.Text = IsNull(_objMemoryValue)?"":(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ParameterValue"+_strIndex]).ToString();
            if (!(Functions.IsNull(this.ParentForm)))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
            }
            this.txtedt.Text = this.dataRL.ParameterValue(strTabID, iRoutingIndex, int.Parse(_strIndex));
            if (!(Functions.IsNull(this.ParentForm)))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;
            }
        }

        #endregion

        #region saving to mem

        private void CommitMemory(object sender, System.EventArgs e)
        {
            //			string strParameterValue = "ParameterValue" + this.Tag.ToString().Trim() ;


            string _strValue = this.txtedt.Text.Trim();
            _strValue = _strValue.ToUpper();
            //			_strValue=_strValue.Replace(";",",");
            //			_strValue=_strValue.Replace(":",",");

            string _strValueOld = this.dataRL.ParameterValue(strTabID, iRoutingIndex, int.Parse(this.Tag.ToString().Trim()));


            char[] cCharEntered = _strValue.ToCharArray(0, ((int)_strValue.Length));
            //			ArrayList cCharEntered = (ArrayList)((ICollection)(_strValue.ToCharArray( 0, ((int)_strValue.Length ) )));


            //			Encoding eAscii = Encoding.ASCII;

            foreach (char cChar in cCharEntered)
            {
                string sChar = cChar.ToString();
                //				Byte[] bCharAscii = eAscii.GetBytes(sChar);
                //				int _iAscii = (int)(bCharAscii[0]);

                // Ascii code for 'A' is 65 while that of 'Z' is 90  ,  ' ' 32, ',' 44
                //				bool bValid =  ( (( _iAscii> 64) && (_iAscii < 91)) || (_iAscii == 32 ) || (_iAscii == 44) );
                bool bValid = (char.IsLetter(cChar) || char.IsWhiteSpace(cChar) || cChar.Equals(','));

                if (!bValid)
                {
                    //					MessageBox.Show(" ' "+ sChar + " ' Is not allowed. "+   " Only positive integer numbers " );
                    _strValue = _strValue.Replace(sChar, ",");
                }
            }
            _strValue = _strValue.Trim();
            if (_strValue.Replace(",", "").Trim().Equals(""))
            {
                _strValue = "";
            }

            while (_strValue.StartsWith(","))
            {
                _strValue = _strValue.Remove(0, 1).Trim();
            };

            while (_strValue.EndsWith(","))
            {
                _strValue = _strValue.Remove(_strValue.Length - 1, 1).Trim();
            };

            _strValue = _strValue.Trim();
            //
            //
            //			cCharEntered= _strValue.ToCharArray(0,((int)_strValue.Length ) );
            ////			char[] cCharArray = cCharEntered;
            //////			char[] cCharArray = _strValue.ToCharArray(0,((int)_strValue.Length ) );
            ////			int iTrimBegining=0;
            ////				int iTrimEnd=0;
            ////			for(int i =0;i< cCharEntered.Length;i++)
            ////			{
            ////				if(!(cCharEntered[i].Equals(" ") || cCharEntered[i].Equals(",")))
            ////				{
            ////					iTrimBegining=i;
            ////					break;
            ////				}
            ////				cCharEntered[i]=char.MinValue;				
            ////			}
            ////			for(int i =cCharEntered.Length-1;i> iTrimBegining;i--)
            ////			{
            ////				if(!(cCharEntered[i].Equals(" ") || cCharEntered[i].Equals(",")))
            ////				{
            ////					iTrimEnd=i;
            ////					break;
            ////				}
            ////				cCharEntered[i]=char.MinValue;				
            ////			}
            //
            ////			if(cCharEntered[0].Equals(','))
            ////			{
            ////				cCharEntered[0]=char.MinValue;
            ////				if(cCharEntered.Length>1)
            ////				{
            ////					iTrimBegining=1;
            ////				}
            ////			}
            ////
            ////			if(cCharEntered[cCharEntered.Length-1].Equals(','))
            ////			{
            ////				cCharEntered[cCharEntered.Length-1]=char.MinValue;
            ////				if(cCharEntered.Length>1)
            ////				{
            ////					iTrimEnd=cCharEntered.Length-1;
            ////				}
            ////			}
            //
            //
            //			int iTempstart,iTempEnd;
            //
            //			iTempstart=iTrimBegining;
            //			for(int i =iTrimBegining ; i<=iTrimEnd;)
            //			{
            //				if((cCharEntered[i].Equals(" ") ))
            //				{
            //					iTempEnd=i;
            //					i++;
            //					continue;
            //				}
            //				else if( cCharEntered[i].Equals(","))
            //				{
            //					iTempEnd=i;
            //					i++;
            //					for(int j=iTempstart;j<iTempEnd;j++)
            //					{
            //						cCharEntered[j]=char.MinValue;
            //					}
            //					iTempstart=i;
            //				}
            //				else
            //				{
            //					iTempEnd=i;
            //					i++;
            //					for(int j=iTempEnd;j<iTrimEnd;j++)
            //					{
            //						if(cCharEntered[j].Equals(','))
            //						{
            //
            //
            //						}
            //					}
            //
            //
            //
            //
            //				}	
            //
            //
            //
            //			}
            //
            //
            //			_strValue =_strValue.Trim();
            //			if(_strValue.Replace(",","").Trim().Equals(""))
            //			{
            //				_strValue="";
            //			}
            //
            this.txtedt.Text = _strValue;




            if (!_strValue.Equals(_strValueOld))
            {
                ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, iRoutingIndex, true);

            }

            //            this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)[strParameterValue] = this.txtedt.Text.Trim() ;

            this.dataRL.ParameterValue(strTabID, iRoutingIndex, int.Parse(this.Tag.ToString().Trim()), _strValue);


            //						((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID,iRoutingIndex, true);

        }

        #endregion

        #region file uploading

        private void ShowOpenFileDialoge(object sender, System.EventArgs e)
        {
            this.openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files|*.*";
            // openFileDialog.Title = "Open Line delimited Symbol Text File" ;
            this.openFileDialog.Title = "CSV Symbol Text File";
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.ShowDialog();
            this.SendToBack();
        }

        object _lockerObject = new object();

        private void FileUpload(object sender, System.ComponentModel.CancelEventArgs e)
        {

            //				System.IO.f = this.openFileDialog.OpenFile();

            this.Show();
            System.IO.StreamReader sr = null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();



            //				if(openFileDialog.ShowDialog() == DialogResult.OK)
            //				{
            lock (_lockerObject)
            {
                if (File.Exists(this.openFileDialog.FileName))
                {
                    sr = new System.IO.StreamReader(this.openFileDialog.FileName);
                    //It will pickup only Max symbols from data manager constants symbols from a file
                    this.ReadFileToTextBox(ref sr);

                }
                else
                {
                    sr.Close();
                    MessageBox.Show("File does not exist.");
                    //							return string.Empty;
                }
            }
            //				}
            //				else
            //				{
            //					return string.Empty;
            //				}




        }


        private void DragFileUpload(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //			string s= e.Data.GetData(System.Windows.Forms.DataFormats.Text , true).ToString();
            //			MessageBox.Show("here dd");

            //			System.IO.StreamReader sr = null;
            //			System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //			lock(this.DoDragDrop()
            //			{
            //				if(File.Exists(this.openFileDialog.FileName))
            //				{
            //					sr = new System.IO.StreamReader(this.openFileDialog.FileName);
            //					//It will pickup only Max symbols from data manager constants symbols from a file
            //
            //				}
            //				else
            //				{
            //					sr.Close();
            //					MessageBox.Show("File does not exist.");
            //					//							return string.Empty;
            //				}
            //			}

        }


        private void ReadFileToTextBox(ref System.IO.StreamReader sr)
        {
            int i = 0;
            string _strSymbols = sr.ReadToEnd();

            this.txtedt.Text = "";

            //
            //				_strSymbol="";

            foreach (string _str in _strSymbols.Split(','))
            {
                if (i == CONST_MAX_SYMBOL)
                {
                    break;
                }

                this.txtedt.Text = this.txtedt.Text + _str.Trim() + ", ";

                //				while(sr.Peek() != ',' || sr.Peek() != null)
                //				{
                //					_c = sr.ReadToEnd( sr.Read();
                //						_strSymbol=_strSymbol + _c.ToString();
                //				}
                //				if(sr.Peek() == null)
                //				{
                //					_strSymbol = _strSymbol.Trim();
                //
                //				}
                //				else
                //				{
                //					_strSymbol = _strSymbol.Trim()+ ",";
                //				}
            }
            this.txtedt.Text = this.txtedt.Text.Remove(this.txtedt.Text.Length - 2, ", ".Length);
            sr.Close();

            this.txtedt.Focus();

            //			this.txtedt.Focus();
        }

        #endregion

        //				#region is null
        //		private bool IsNull(Object _obj)
        //		{
        //			if(_obj==null)
        //			{
        //				return true;
        //			}
        //			else if( _obj.Equals(null))
        //			{
        //				return true;
        //			}
        //			else if (_obj.Equals(System.DBNull.Value))
        //			{
        //				return true;
        //			}
        //
        //			return false;
        //		}
        //		#endregion
    }
}