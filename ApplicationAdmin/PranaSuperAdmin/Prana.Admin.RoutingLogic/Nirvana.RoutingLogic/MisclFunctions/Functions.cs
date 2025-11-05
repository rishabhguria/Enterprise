using Infragistics.Win.UltraWinEditors;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.RoutingLogic.MisclFunctions
{
    /// <summary>
    /// Summary description for Functions.
    /// </summary>
    public class Functions
    {

        private static int iMINVALUE = -1;

        private Functions()
        {
            //
            // TODO: Add constructor logic here
            //
        }



        #region  min value

        public static int MinValue
        {
            get
            {
                return iMINVALUE;
            }
        }



        #endregion


        #region is null
        public static bool IsNull(Object _obj)
        {
            try
            {
                if (_obj == null || _obj.Equals(null) || _obj.Equals(System.DBNull.Value))
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return true;
            }
        }
        #endregion



        #region back color   on/off focus

        public static void object_GotFocus(object sender, System.EventArgs e)
        {
            string[] _strarrayType = sender.GetType().ToString().Split('.');
            string _strType = _strarrayType[_strarrayType.Length - 1].Trim();
            if (_strType.Equals("UltraTextEditor"))
            {
                ((UltraTextEditor)sender).Appearance.BackColor = Color.FromArgb(255, 250, 205);
            }
            else if (_strType.Equals("UltraComboEditor"))
            {
                ((UltraComboEditor)sender).Appearance.BackColor = Color.FromArgb(255, 250, 205);
            }
            else if (_strType.Equals("UltraCheckEditor"))
            {
                ((UltraCheckEditor)sender).Appearance.BackColor = Color.FromArgb(255, 250, 205);
            }
            else if (_strType.Equals("UltraOptionSet"))
            {
                ((UltraOptionSet)sender).Appearance.BackColor = Color.FromArgb(255, 250, 205);
            }
            else if (_strType.Equals("CheckedListBox"))
            {
                ((CheckedListBox)sender).BackColor = Color.FromArgb(255, 250, 205);
            }
            //			else if(_strType.Equals("UltraTree"))
            //			{
            //				((Infragistics.Win.UltraWinTree.UltraTree)sender).Appearance.BackColor = Color.FromArgb(255, 250,205);
            //			}



        }
        public static void object_LostFocus(object sender, System.EventArgs e)
        {
            string[] _strarrayType = sender.GetType().ToString().Split('.');
            string _strType = _strarrayType[_strarrayType.Length - 1].Trim();
            if (_strType.Equals("UltraTextEditor"))
            {
                ((UltraTextEditor)sender).Appearance.BackColor = Color.White;
            }
            else if (_strType.Equals("UltraComboEditor"))
            {
                ((UltraComboEditor)sender).Appearance.BackColor = Color.White;
            }
            else if (_strType.Equals("UltraCheckEditor"))
            {
                ((UltraCheckEditor)sender).Appearance.BackColor = Color.FromArgb(236, 233, 216);
            }
            else if (_strType.Equals("UltraOptionSet"))
            {
                ((UltraOptionSet)sender).Appearance.BackColor = Color.FromArgb(236, 233, 216);
            }
            else if (_strType.Equals("CheckedListBox"))
            {
                ((CheckedListBox)sender).BackColor = Color.White;
            }
            //			else if(_strType.Equals("UltraTree"))
            //			{
            //				((Infragistics.Win.UltraWinTree.UltraTree)sender).Appearance.BackColor = Color.White;
            //			}
        }

        #endregion



        //		#region  node contains key/tag
        //
        //		public static int TreeNodeIndexofTag( System.Windows.Forms.NodeTree _node , string _strTag)
        //		{
        //			int _iIndex = Functions.iMINVALUE ;
        //			for(int i =0; i<_node.Nodes.Count ;i++)
        //			{
        //				if(_node[i].Tag.ToString().Trim().Equals(_strTag.Trim()))
        //				{
        //					_iIndex=i;
        //					break;
        //				}
        //			}
        //
        //			return _iIndex ;
        //		}
        //		
        //		#endregion


    }
}
