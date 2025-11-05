using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;
using System.Drawing;

namespace Prana.Utilities.UI.UIUtilities
{
    public class UltraComboHelper
    {
        /// <summary>
        /// Returns maximum between largest length of string values of Dropdown and Dropdown width for ultraCombo
        /// ultraCombo must not be misinterpreted as ultraComboEditor.
        /// Ref: "http://www.infragistics.com/community/forums/t/72518.aspx"
        /// </summary>
        /// <param name="ultraCombo1">ultraCombo whose size is to be checked</param>
        /// <returns>Integer value to be set as width of ultraCombo</returns>
        /// <see cref="http://www.infragistics.com/community/forums/t/72518.aspx"/>
        public static int SetMaxWidthforUltraCombo(UltraCombo ultraCombo1, String columnNameToBeChecked)
        {
            Graphics gr = DrawUtility.GetCachedGraphics(ultraCombo1);
            Font fontToDispose = null;

            try
            {
                String largestText = "";
                foreach (UltraGridRow rw in ultraCombo1.Rows.All)
                {
                    if (largestText.Length < rw.Cells[columnNameToBeChecked].Value.ToString().Length)
                        largestText = rw.Cells[columnNameToBeChecked].Value.ToString();
                }

                AppearanceData appData = new AppearanceData();
                AppearancePropFlags requestedProps = AppearancePropFlags.FontData;
                ultraCombo1.ResolveAppearance(ref appData, requestedProps);
                fontToDispose = appData.CreateFont(ultraCombo1.Font);
                Font font = fontToDispose != null ? fontToDispose : ultraCombo1.Font;

                EmbeddableUIElementBase editorElement = ultraCombo1.UIElement.GetDescendant(typeof(EmbeddableUIElementBase)) as EmbeddableUIElementBase;
                if (editorElement != null)
                {
                    Size size = Size.Ceiling(DrawUtility.MeasureString(gr, largestText, font, editorElement.Owner, editorElement.OwnerContext));
                    DrawUtility.ReleaseCachedGraphics(gr);
                    return editorElement.RectInsideBorders.Width > size.Width + 25 ? editorElement.RectInsideBorders.Width : size.Width + 25;
                }
                else
                {
                    return 0;
                }
            }

            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return 0;
            }

        }

    }
}