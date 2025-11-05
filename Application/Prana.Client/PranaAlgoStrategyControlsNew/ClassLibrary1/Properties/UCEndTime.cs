using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.BusinessObjects;

namespace NirvanaAlgoStrategyControlsUCEndTime
{
    public partial class UCEndTime : UserControl, Nirvana.Interfaces.IAlgoStrategyUserControl
    {
        AlgoStrategyParameters _parameters = new AlgoStrategyParameters();
        public UCEndTime()
        {
            InitializeComponent();
           
        }

        #region IAlgoStrategyUserControl Members

        public string ValidateValues()
        {
            //throw new Exception("The method or operation is not implemented.");
            if ((Convert.ToDateTime(ultraDateTimeEditor1.Value).TimeOfDay) < DateTime.Now.TimeOfDay)
            {
                return "Start Time should be greater than Current Time";
            }
            else
                return string.Empty;
        }

        public AlgoStrategyParameters GetValue()
        {
            //throw new Exception("The method or operation is not implemented.");
            return _parameters;
        }

        public void SetValues(AlgoStrategyParameters parameters )
        {
            _parameters = parameters;
            //this.Name = parameters.Name;
            this.SetBounds(parameters.Xpos, parameters.Ypos, this.Width, this.Height);
            ultraDateTimeEditor1.Name = _parameters.IDs[0];
            this.radioButton1.Text = _parameters.Names[0];
        }
        

        public Dictionary<string, string> GetFixValue()
        {
            //Dictionary<string, string> idValuePair = new Dictionary<string, string>();
            //idValuePair.Add(_parameters.ID, this.ultraDateTimeEditor1.Value.ToString());
            //return idValuePair;

           // string strDec = ValidateValues();
           // if (strDec == string.Empty)
           // {
                Dictionary<string, string> idValuePair = new Dictionary<string, string>();
                idValuePair.Add(ultraDateTimeEditor1.Name, Convert.ToDateTime(ultraDateTimeEditor1.Value).TimeOfDay.ToString());
                return idValuePair;
           // }
            //else
            //{
                //MessageBox.Show(strDec);
                //return null;
           // }
        }

        public void SetUserControls(string type)
        {

        }
        public void SetFixValues(string tag, string value)
        {
            if (this.ultraDateTimeEditor1.Name == tag)
            {
                this.radioButton1.Checked = true;
                this.ultraDateTimeEditor1.Value = value;
            }
        }
        #endregion IAlgoStrategyUserControl Members

        #region IAlgoStrategyUserControl Members


        public Nirvana.Interfaces.IAlgoStrategyUserControl GetUserCtrl()
        {
            UCEndTime ucEndTime = new UCEndTime();
            ucEndTime.SetValues(_parameters);
            ucEndTime.Top = _parameters.Ypos;
            ucEndTime.Left = _parameters.Xpos;
            return ucEndTime;
        }

        public string Validate(OrderSingle order)
        {
            return string.Empty;
        }
        #endregion
    }
}
