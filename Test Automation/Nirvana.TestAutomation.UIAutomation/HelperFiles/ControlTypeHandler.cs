using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using UIAutomationClient;
using System.Text.RegularExpressions;

namespace Nirvana.TestAutomation.UIAutomation
{
    public class ControlTypeHandler
    {
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^\d+$");
        }

        static IUIAutomationElement _currwinglobal = null;
         public static string getValueOfElementAdvance(IUIAutomationElement targetelement, ref bool result, string priority = "Value")
        {
            string valuetouse = string.Empty;
            if (targetelement == null)
            {

                return valuetouse;
            }
            string elementtoget = targetelement.CurrentAutomationId;

            if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ComboBoxControlTypeId)
            {

              string tempValue = string.Empty;
				object tempObj = targetelement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);

				if (tempObj != null)
				{
				    tempValue = tempObj.ToString();
				}

				if (!string.IsNullOrEmpty(tempValue) && !IsNumeric(tempValue))
				{
				    object valueObj = targetelement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
				    if (valueObj != null)
				    {
				        valuetouse = valueObj.ToString();
				    }
				    result = true;
				}

                else
                {
                    IUIAutomation automation = new CUIAutomation8();
                    IUIAutomationCondition lstitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
                    IUIAutomationElementArray lstelement = targetelement.FindAll(
                       TreeScope.TreeScope_Descendants, lstitem);


                    if (lstelement.Length > 0)
                    {

                        if (lstelement.Length == 1)
                        {
                            IUIAutomationElement listitem = lstelement.GetElement(0);
                            string listitemText = listitem.GetCurrentPropertyValue(
                               UIA_PropertyIds.UIA_NamePropertyId).ToString();
                            valuetouse = listitem.CurrentName;
                        }
                        else
                        {
                            for (int i = 0; i < lstelement.Length; i++)
                            {
                                IUIAutomationElement listitem = lstelement.GetElement(i);
                                string listitemText = listitem.GetCurrentPropertyValue(
                                   UIA_PropertyIds.UIA_NamePropertyId).ToString();
                                //WriteToConsole(listitemText);
                                object selectPatternObj;
                                selectPatternObj = listitem.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);

                                IUIAutomationSelectionItemPattern selectPattern = selectPatternObj as IUIAutomationSelectionItemPattern;

                                if (selectPattern != null)
                                {
                                    int _isselected = selectPattern.CurrentIsSelected;
                                    if (_isselected == 1)
                                    {
                                        valuetouse = listitem.CurrentName;
                                    }

                                }
                            }
                        }
                    }
                }

            }
            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_SpinnerControlTypeId)
            {
                try
                {

                    string value = null;
                    IUIAutomation automation = new CUIAutomation();
                    IUIAutomationElement rootElement = automation.GetRootElement();

                    IUIAutomationElementArray childElements = targetelement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    if (childElements.Length > 0)
                    {
                        for (int i = 0; i < childElements.Length; i++)
                        {
                            IUIAutomationElement childElement = childElements.GetElement(i);
                            if (childElement.CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
                            {
                                object valueObj = childElement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                                 value = valueObj != null ? valueObj.ToString() : "";

                                result = true;
                                valuetouse = value;
                                break;
                            }
                        }
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + ex);
                }
            }

            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_CheckBoxControlTypeId)
            {
                try
                {
                    result = true;
                    string value = null;
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                        IUIAutomationTogglePattern selectionpatternprovider = patternprovider as IUIAutomationTogglePattern;
                        value = selectionpatternprovider.CurrentToggleState.ToString();
                        string togglestate = selectionpatternprovider.CurrentToggleState.ToString();
                        valuetouse = togglestate;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(Environment.NewLine + ex);

                }

            }
            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_RadioButtonControlTypeId)
            {
                result = true;
                valuetouse = targetelement.CurrentName;

            }
            else if (targetelement.CurrentAutomationId == "MultiSelectEditor")
            {
                IUIAutomation automation2 = new CUIAutomation8();
                IUIAutomationCondition bttn = automation2.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
                IUIAutomationElement btnelement = targetelement.FindFirst(TreeScope.TreeScope_Descendants, bttn);


                if (btnelement != null)
                {
                    try
                    {
                        result = true;
                        string value = null;
                        object patternprovider;
                        if (btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) != null)
                        {
                            patternprovider = btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                            IUIAutomationExpandCollapsePattern selectionpatternprovider = patternprovider as IUIAutomationExpandCollapsePattern;
                            value = selectionpatternprovider.CurrentExpandCollapseState.ToString();
                            selectionpatternprovider.Expand();
                            IUIAutomation automation1 = new CUIAutomation8();
                            IUIAutomationCondition listt = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "checkedMultipleItems");
                            IUIAutomationElement listelement = _currwinglobal.FindFirst(TreeScope.TreeScope_Descendants, listt);
                            if (listelement != null)
                            {
                                IUIAutomation automation = new CUIAutomation8();

                                IUIAutomationCondition conditiont = automation.CreateTrueCondition();

                                IUIAutomationElementArray checkboxes = listelement.FindAll(TreeScope.TreeScope_Descendants, conditiont);

                                if (checkboxes != null)
                                {
                                    for (int i = 0; i < checkboxes.Length; i++)
                                    {
                                        IUIAutomationElement checkbox = checkboxes.GetElement(i);
                                        try
                                        {

                                            object patternproviderrr;
                                            if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) != null)
                                            {
                                                patternproviderrr = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                                                IUIAutomationTogglePattern chkboxpatternprovider = patternproviderrr as IUIAutomationTogglePattern;
 
                                                if (chkboxpatternprovider.CurrentToggleState.ToString() == "ToggleState_On")
                                                {
                                                    valuetouse += ("," + checkbox.CurrentName);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(Environment.NewLine + ex);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(Environment.NewLine + ex);

                    }

                }
            }
            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
            {
                try
                {
                    result = true;
                    IUIAutomationValuePattern valuePattern = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                    if (valuePattern != null)
                    {
                        valuetouse = valuePattern.CurrentValue;
                        // WriteToConsole(valuetouse);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + ex);

                }

            }

            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_CustomControlTypeId)
            {
                try
                {

                    string cellValue = targetelement.CurrentName;
                    if (!string.IsNullOrEmpty(cellValue) && string.Equals(priority, "Name", StringComparison.OrdinalIgnoreCase))
                    {
                        return cellValue;
                    }

                    IUIAutomation automation = new CUIAutomation8();

                    IUIAutomationElementArray subChildElements = targetelement.FindAll(TreeScope.TreeScope_Children, automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId));

                    for (int k = 0; k < subChildElements.Length; k++)
                    {
                        IUIAutomationElement editElement = subChildElements.GetElement(k);

                        if ((bool)editElement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_IsValuePatternAvailablePropertyId))
                        {
                            var valuePattern = editElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;

                            if (valuePattern != null)
                            {
                                cellValue = valuePattern.CurrentValue;
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    return cellValue;
                                }
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(cellValue))
                    {
                        var valuePattern = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                        if (valuePattern != null)
                        {
                            cellValue = valuePattern.CurrentValue;
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                return cellValue;
                            }
                        }
                    }
                    return cellValue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + ex);
                }
            }
            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_TextControlTypeId)
            {
                try 
                {
                    return targetelement.CurrentName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + ex);
                }
            }

            return valuetouse;
        }
        public static string getValueOfElement(IUIAutomationElement targetelement, ref bool result)
        {
            string valuetouse = string.Empty;
            if (targetelement == null)
            {

                return valuetouse;
            }
            string elementtoget = targetelement.CurrentAutomationId;

            if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ComboBoxControlTypeId)
            {

            string tempValue = targetelement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId) != null
    ? targetelement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId).ToString()
    : "";

if (!string.IsNullOrEmpty(tempValue) && !IsNumeric(tempValue))
{
    valuetouse = targetelement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId) != null
        ? targetelement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId).ToString()
        : "";
    result = true;
}

                else
                {
                    IUIAutomation automation = new CUIAutomation8();
                    IUIAutomationCondition lstitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
                    IUIAutomationElementArray lstelement = targetelement.FindAll(
                       TreeScope.TreeScope_Descendants, lstitem);


                    if (lstelement.Length > 0)
                    {

                        /* for (int i = 0; i < childElements.Length; i++)
                         {
                             IUIAutomationElement childElement = childElements.GetElement(i);
                             if (childElement.CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
                             {
                                 valuetouse = childElement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId)?.ToString() ?? "";
                                 result = true;
                                 break;
                             }
                         }*/

                        if (lstelement.Length == 1)
                        {
                            IUIAutomationElement listitem = lstelement.GetElement(0);
                            string listitemText = listitem.GetCurrentPropertyValue(
                               UIA_PropertyIds.UIA_NamePropertyId).ToString();
                            valuetouse = listitem.CurrentName;
                        }
                        else
                        {
                            for (int i = 0; i < lstelement.Length; i++)
                            {
                                IUIAutomationElement listitem = lstelement.GetElement(i);
                                string listitemText = listitem.GetCurrentPropertyValue(
                                   UIA_PropertyIds.UIA_NamePropertyId).ToString();
                                //WriteToConsole(listitemText);
                                object selectPatternObj;
                                selectPatternObj = listitem.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);

                                IUIAutomationSelectionItemPattern selectPattern = selectPatternObj as IUIAutomationSelectionItemPattern;

                                if (selectPattern != null)
                                {
                                    // Extract the value from the ComboBox
                                    int _isselected = selectPattern.CurrentIsSelected;
                                    if (_isselected == 1)
                                    {
                                        valuetouse = listitem.CurrentName;
                                    }
                                    //WriteToConsole($"ComboBox value: {comboBoxValue}");

                                }
                            }
                        }
                    }
                }

            }
            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_SpinnerControlTypeId)
            {
                try
                {

                    string value = null;
                    IUIAutomation automation = new CUIAutomation();
                    IUIAutomationElement rootElement = automation.GetRootElement();

                    IUIAutomationElementArray childElements = targetelement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    if (childElements.Length > 0)
                    {
                        for (int i = 0; i < childElements.Length; i++)
                        {
                            IUIAutomationElement childElement = childElements.GetElement(i);
                            if (childElement.CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
                            {
                                var valueProperty = childElement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                                value = valueProperty != null ? valueProperty.ToString() : "";

                                result = true;
                                valuetouse = value;
                                break;
                            }
                        }
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + ex);
                }
            }

            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_CheckBoxControlTypeId)
            {
                try
                {
                    result = true;
                    string value = null;
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                        IUIAutomationTogglePattern selectionpatternprovider = patternprovider as IUIAutomationTogglePattern;
                        value = selectionpatternprovider.CurrentToggleState.ToString();
                        string togglestate = selectionpatternprovider.CurrentToggleState.ToString();
                        //WriteToConsole(selectionpatternprovider.CurrentToggleState.ToString());
                        valuetouse = togglestate;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(Environment.NewLine + ex);

                }

            }
            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_RadioButtonControlTypeId)
            {
                result = true;
                valuetouse = targetelement.CurrentName;

            }
            else if (targetelement.CurrentAutomationId == "MultiSelectEditor")
            {
                //  WriteToConsole("inside multi select ");

                IUIAutomation automation2 = new CUIAutomation8();
                IUIAutomationCondition bttn = automation2.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
                IUIAutomationElement btnelement = targetelement.FindFirst(TreeScope.TreeScope_Descendants, bttn);


                if (btnelement != null)
                {
                    // WriteToConsole("Found Button element ");
                    //expand pattern invoke
                    try
                    {
                        result = true;
                        string value = null;
                        object patternprovider;
                        if (btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) != null)
                        {
                            patternprovider = btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                            // WriteToConsole("inside expand pattern");
                            IUIAutomationExpandCollapsePattern selectionpatternprovider = patternprovider as IUIAutomationExpandCollapsePattern;
                            value = selectionpatternprovider.CurrentExpandCollapseState.ToString();
                            //WriteToConsole("......////...........");
                            //   WriteToConsole(selectionpatternprovider.CurrentExpandCollapseState.ToString());
                            selectionpatternprovider.Expand();
                            //  WriteToConsole(selectionpatternprovider.CurrentExpandCollapseState.ToString());
                            IUIAutomation automation1 = new CUIAutomation8();
                            IUIAutomationCondition listt = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "checkedMultipleItems");
                            IUIAutomationElement listelement = _currwinglobal.FindFirst(TreeScope.TreeScope_Descendants, listt);
                            if (listelement != null)
                            {
                                // Condition checkboxitem = new PropertyCondition(AutomationElement.ControlTypeProperty,listelement);
                                // AutomationElementCollection checkboxes = listelement.FindAll(TreeScope.Children, checkboxitem);
                                IUIAutomation automation = new CUIAutomation8();

                                IUIAutomationCondition conditiont = automation.CreateTrueCondition();

                                IUIAutomationElementArray checkboxes = listelement.FindAll(TreeScope.TreeScope_Descendants, conditiont);
                                //WriteToConsole(checkboxes[0].Current.Name);
                                //   WriteToConsole("SIZE OF List" + checkboxes.Length);

                                if (checkboxes != null)
                                {
                                    // Console.Write("inside checkboxes ");
                                    for (int i = 0; i < checkboxes.Length; i++)
                                    {
                                        IUIAutomationElement checkbox = checkboxes.GetElement(i);
                                        //  WriteToConsole(checkbox.CurrentName + "..");
                                        try
                                        {
                                            // IUIAutomationValuePattern valuePattern = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                                            // IUIAutomationTogglePattern togglePattern = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;
                                            //string value = null;
                                            object patternproviderrr;
                                            if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) != null)
                                            {
                                                patternproviderrr = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                                                IUIAutomationTogglePattern chkboxpatternprovider = patternproviderrr as IUIAutomationTogglePattern;
                                                // value = selectionpatternprovider.Current.ToString();
                                                // WriteToConsole("......////...........");
                                                //  WriteToConsole(chkboxpatternprovider.ToString());
                                                //   WriteToConsole(chkboxpatternprovider.CurrentToggleState.ToString());
                                                if (chkboxpatternprovider.CurrentToggleState.ToString() == "ToggleState_On")
                                                {
                                                    valuetouse += ("," + checkbox.CurrentName);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(Environment.NewLine + ex);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(Environment.NewLine + ex);

                    }

                }
            }
            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
            {
                try
                {
                    result = true;
                    IUIAutomationValuePattern valuePattern = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                    if (valuePattern != null)
                    {
                        valuetouse = valuePattern.CurrentValue;
                        // WriteToConsole(valuetouse);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + ex);

                }

            }

            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_CustomControlTypeId)
            {
                try
                {

                    string cellValue = targetelement.CurrentName;
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        return cellValue;
                    }

                    IUIAutomation automation = new CUIAutomation8();
                    IUIAutomationElementArray subChildElements = targetelement.FindAll(TreeScope.TreeScope_Children, automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId));

                    for (int k = 0; k < subChildElements.Length; k++)
                    {
                        IUIAutomationElement editElement = subChildElements.GetElement(k);

                        if ((bool)editElement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_IsValuePatternAvailablePropertyId))
                        {
                            var valuePattern = editElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;

                            if (valuePattern != null)
                            {
                                cellValue = valuePattern.CurrentValue;
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    return cellValue;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + ex);
                }
            }

            return valuetouse;
        }
        public static IUIAutomationElement FindCheckBoxChildElement(IUIAutomationElement targetElement)
        {
            IUIAutomationElement subchildElement = null;
            try
            {
                if (targetElement != null)
                {
                    var uiAutomation = new CUIAutomation();
                    var condition = uiAutomation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_ControlTypePropertyId,
                        UIA_ControlTypeIds.UIA_CheckBoxControlTypeId
                    );

                    IUIAutomationElementArray childElements = targetElement.FindAll(
                        TreeScope.TreeScope_Children,
                        condition
                    );

                    if (childElements != null && childElements.Length > 0)
                    {
                        subchildElement = childElements.GetElement(0);
                        return subchildElement;
                    }
                    else
                    {
                        Console.WriteLine("No CheckBox found. Listing control types of all child elements:");


                    }
                }
                return subchildElement;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine + ex);
                return null;
            }
        }
        public static bool ImageControlTypeChildExist(IUIAutomationElement targetElement)
        {
            IUIAutomationElement subchildElement = null;
            try
            {
                if (targetElement != null)
                {
                    var uiAutomation = new CUIAutomation();
                    var condition = uiAutomation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_ControlTypePropertyId,
                        UIA_ControlTypeIds.UIA_ImageControlTypeId
                    );

                    IUIAutomationElementArray childElements = targetElement.FindAll(
                        TreeScope.TreeScope_Children,
                        condition
                    );

                    if (childElements != null && childElements.Length > 0)
                    {
                        subchildElement = childElements.GetElement(0);
                        return true;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine + ex);
                return false;
            }
        }
    }

}
