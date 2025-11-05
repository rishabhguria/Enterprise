using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;


namespace Nirvana.TestAutomation.Steps.Allocation
{
    [UITestFixture]
    public partial class MasterFundPreferencesUIMap : UIMap
    {
        public MasterFundPreferencesUIMap()
        {
            InitializeComponent();
        }

        public MasterFundPreferencesUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        /// <summary>
        /// Open Allocation window from Prana Main window
        /// </summary>
        public void OpenAllocation()
        {
            try
            {
                //  Shortcut to open allocation module(CTRL + SHIFT + A)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_ALLOCATION"]);
                ExtentionMethods.WaitForVisible(ref Allocation, 15);
               // Wait(15000);
                //Trade.Click(MouseButtons.Left);
                //Allocation1.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Opens the mf preferences.
        /// </summary>
        public void OpenMFPreferences()
        {
            try
            {
                OpenAllocation();
                //Wait(3000);
                ExtentionMethods.WaitForVisible(ref Allocation, 15);
                EditAllocationPreferences3.Click(MouseButtons.Left);
                CalculatedPreferences.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// For minimizing Allocation windows
        /// </summary>
        public void MinimizeAllocation()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref Allocation3);
             //   Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// For minimizing Edit Allocation Preferences windows
        /// </summary>
        public void MinimizeEditAllocationPreference()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref EditAllocationPreferences2);
               // Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// clicks on the down button of the combo box
        /// </summary>
        /// <param name="parentAutomationElement"></param>
        public void Clickdropdownbutton(UIAutomationElement parentAutomationElement)
        {
            try
            {
                UIControlPart dropdownButton = new UIControlPart();
                dropdownButton.BoundsInParent = new System.Drawing.Rectangle(80, 0, 25, 20);
                dropdownButton.Comment = null;
                dropdownButton.ControlPartProvider = null;
                dropdownButton.Name = "dropdownButton";
                dropdownButton.ObjectImage = null;
                dropdownButton.Parent = parentAutomationElement;
                dropdownButton.Path = null;
                dropdownButton.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
                dropdownButton.UseCoordinatesOnClick = false;
                dropdownButton.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }

        /// <summary>
        /// clicks on the down button of the  big combo box 
        /// </summary>
        /// <param name="parentAutomationElement"></param>
        public void ClickdropdownbuttonLongbox(UIAutomationElement parentAutomationElement)
        {
            try
            {
                UIControlPart dropdownButton = new UIControlPart();
                dropdownButton.BoundsInParent = new System.Drawing.Rectangle(120, 0, 25, 20);
                dropdownButton.Comment = null;
                dropdownButton.ControlPartProvider = null;
                dropdownButton.Name = "dropdownButton";
                dropdownButton.ObjectImage = null;
                dropdownButton.Parent = parentAutomationElement;
                dropdownButton.Path = null;
                dropdownButton.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
                dropdownButton.UseCoordinatesOnClick = false;
                dropdownButton.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }
        /// <summary>
        /// clicks on the item from the list of items
        /// </summary>
        /// <param name="item">item to be clicked on</param>
        /// <param name="combo"></param>
        public void ClickOnComboBoxItem(string item, UIAutomationElement combo)
        {
            try
            {
                var cacheChildren = combo.AutomationElementWrapper.CachedChildren;
                Dictionary<string, int> nameToIndexMapping = new Dictionary<string, int>(); //ControlPartOfCmbboxAllocationMethod.Click(MouseButtons.Left);
                for (int i = 1; i < cacheChildren.Count; i++)
                {
                    nameToIndexMapping.Add(cacheChildren[i].CachedChildren[0].Name, i);

                }
                if (nameToIndexMapping[item.ToString()] > 1 && TestDataConstants.COL_MFPREF.ToString().Contains(item.ToString()))
                {
                    Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }
                else
                    cacheChildren[nameToIndexMapping[item]].CachedChildren[0].WpfClick();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// get preference index map
        /// </summary>
        /// <returns>map of Calculated Preference to index</returns>
        public Dictionary<string, int> GetPreferenceIndexMap()
        {
            try
            {
                Dictionary<string, int> preferenceIndexMap = new Dictionary<string, int>();
                for (int i = 1; i < Records.AutomationElementWrapper.CachedChildren.Count; i++)
                {
                    preferenceIndexMap.Add(Records.AutomationElementWrapper.CachedChildren[i].CachedChildren[1].Name, i);
                }
                return preferenceIndexMap;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return null;
            }
        }
        /// <summary>
        /// get account index map
        /// </summary>
        /// <returns>map of account name to index from the account strategy grid</returns>
        public Dictionary<string, int> GetAccountIndexMap(UIAutomationElement element)
        {
            try
            {
                Dictionary<string, int> accountIndexMap = new Dictionary<string, int>();
                for (int i = 0; i < element.AutomationElementWrapper.Children.Count; i++)
                {
                    accountIndexMap.Add(element.AutomationElementWrapper.Children[i].Children[0].Children[1].Name, i);
                }
                return accountIndexMap;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// get strategy name index map
        /// </summary>
        /// <returns>map of strategy name to index</returns>
        public Dictionary<string, int> GetStrategyIndexMap(UIAutomationElement element)
        {
            try
            {
                Dictionary<string, int> strategyIndexMap = new Dictionary<string, int>();
                for (int i = 0; i < element.AutomationElementWrapper.Children[0].Children.Count; i++)
                {
                    strategyIndexMap.Add(element.AutomationElementWrapper.Children[0].Children[i].Name, i);
                }
                return strategyIndexMap;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return null;
            }
        }

    }
}
