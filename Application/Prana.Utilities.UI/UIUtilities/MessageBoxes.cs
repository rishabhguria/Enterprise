using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public class InformationMessageBox
    {

        /// <summary>
        /// Displays the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static DialogResult Display(string text)
        {
            return MessageBox.Show(text, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Displays the specified text along with the title bar text.
        /// </summary>
        /// <param name="displayText">The text message to be displayed.</param>
        /// <param name="titleText">The title bar text to be displayed.</param>
        /// <returns>The result of dialog box action.</returns>
        public static DialogResult Display(string displayText, string titleText)
        {
            return MessageBox.Show(displayText, titleText, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class ErrorMessageBox
    {

        /// <summary>
        /// Displays the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static DialogResult Display(string text)
        {
            return MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Displays the specified text along with the title bar text.
        /// </summary>
        /// <param name="displayText">The text message to be displayed.</param>
        /// <param name="titleText">The title bar text to be displayed.</param>
        /// <returns>The result of dialog box action.</returns>
        public static DialogResult Display(string displayText, string titleText)
        {
            return MessageBox.Show(displayText, titleText, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public class ConfirmationMessageBox
    {

        /// <summary>
        /// Displays the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static DialogResult Display(string text)
        {
            return MessageBox.Show(text, "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Displays the specified text along with the title bar text and show Yes No Cancel Button
        /// </summary>
        /// <param name="displayText">The text message to be displayed.</param>
        /// <param name="titleText">The title bar text to be displayed.</param>
        /// <returns>The result of dialog box action.</returns>
        public static DialogResult Display(string displayText, string titleText)
        {
            return MessageBox.Show(displayText, titleText, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Displays the specified text along with the title bar text and show Yes No Button
        /// </summary>
        /// <param name="displayText">The text message to be displayed.</param>
        /// <param name="titleText">The title bar text to be displayed.</param>
        /// <returns>The result of dialog box action.</returns>
        public static DialogResult DisplayYesNo(string displayText, string titleText)
        {
            return MessageBox.Show(displayText, titleText, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }
    }

    public class WarningMessageBox
    {

        /// <summary>
        /// Displays the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static DialogResult Display(string text)
        {
            return MessageBox.Show(text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Displays the specified text along with the title bar text.
        /// </summary>
        /// <param name="displayText">The text message to be displayed.</param>
        /// <param name="titleText">The title bar text to be displayed.</param>
        /// <returns>The result of dialog box action.</returns>
        public static DialogResult Display(string displayText, string titleText)
        {
            return MessageBox.Show(displayText, titleText, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
