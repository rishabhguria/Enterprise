using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public class PranaMessageBox
    {
        public static DialogResult ShowDialog(MessageBoxInfo messagebxinfo)
        {
            DialogResult dialogresult = MessageBox.Show(messagebxinfo.MessageText, messagebxinfo.Caption, (MessageBoxButtons)messagebxinfo.MessageBoxButtons, (MessageBoxIcon)messagebxinfo.ErrorIcons, MessageBoxDefaultButton.Button1);
            return dialogresult;
        }
    }
}
