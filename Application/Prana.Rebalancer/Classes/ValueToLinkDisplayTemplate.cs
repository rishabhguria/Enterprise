using Infragistics.Windows.Editors;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using System.Windows;
using System.Windows.Controls;

namespace Prana.Rebalancer.Classes
{
    internal class ValueToLinkDisplayTemplate : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate dataTemplate = null;
            var editor = TemplateEditor.GetEditor(container);

            // Clearing the Tag property which in this case is used to track the template returned in edit mode.
            editor.Tag = null;
            if (item != null)
            {
                string value = editor.Value.ToString();
                if (value.Contains(ComplainceConstants.CONST_MULTIPLE))
                {
                    dataTemplate = editor.FindResource("MultipleValueTemplate") as DataTemplate;
                }
                else
                {
                    dataTemplate = editor.FindResource("StringFieldDisplayTemplate") as DataTemplate;
                }
            }
            if (dataTemplate != null) return dataTemplate;
            return base.SelectTemplate(item, container);
        }
    }
}
