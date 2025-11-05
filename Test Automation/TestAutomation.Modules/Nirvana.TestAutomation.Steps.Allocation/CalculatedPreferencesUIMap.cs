using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;


namespace Nirvana.TestAutomation.Steps.Allocation
{
    [UITestFixture]
    public partial class CalculatedPreferencesUIMap : UIMap
    {
        public CalculatedPreferencesUIMap()
        {
            InitializeComponent();
        }

        public CalculatedPreferencesUIMap(IContainer container)
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
               // Wait(15000);
                //Trade.Click(MouseButtons.Left);
                //Allocation5.Click(MouseButtons.Left);
                ExtentionMethods.WaitForVisible(ref Allocation, 15);
                
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
        /// For minimizing Allocation windows
        /// </summary>
        public void MinimizeAllocation()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref Allocation2);
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
        /// Open calculated Preferences window from Allocation window
        /// </summary>
        public void OpenCalculatedPreferences()
        {
            try
            {
                OpenAllocation();
               // Wait(7000);
              
                EditAllocationPreferences1.Click(MouseButtons.Left);
                if (NirvanaAllocation.IsVisible)
                    ButtonOK3.Click(MouseButtons.Left);
                ExtentionMethods.WaitForVisible(ref EditAllocationPreferences2, 7);
                CalculatedPreferences1.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Closes calculated preferences
        /// </summary>
        public void CloseCalculatedPreferences()
        {
            try
            {
                Allocation.Click(MouseButtons.Left);
                Close.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Clears the text.
        /// </summary>
        /// <param name="TextBoxElement">The text box element.</param>
        public void clearText(UIWindow TextBoxElement)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();
                while (TextBoxElement.Text.Length > 0 && tmr.ElapsedMilliseconds <= 15000)
                {
                    TextBoxElement.Click(MouseButtons.Left);
                    Keyboard.SendKeys("[HOME]");
                    MouseController.DoubleClick();
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                }
                tmr.Stop();
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
        /// clicks on the down button of the combo box
        /// </summary>
        /// <param name="parentAutomationElement"></param>
        public void Clickdropdownbutton(UIAutomationElement parentAutomationElement)
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
        /// Handles the AttachFailing event of the EditAllocationPreferences2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AttachFailingEventArgs"/> instance containing the event data.</param>
        private void EditAllocationPreferences2_AttachFailing(object sender, AttachFailingEventArgs e)
        {
            try
            {
                if (e.CurrentRetryCount >= 1)
                {
                    e.Action = AttachFailingAction.Fail;
                    EditAllocationPreferences2.AttachFailing -= EditAllocationPreferences2_AttachFailing;
                }
                else
                {
                    e.Action = AttachFailingAction.Retry;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public Dictionary<string, int> GetIndexNameMap()
        {
            try
            {
                Dictionary<string, int> indexToName = new Dictionary<string, int>();
                for (int i = 1; i < Records.AutomationElementWrapper.CachedChildren.Count; i++)
                {
                    indexToName.Add(Records.AutomationElementWrapper.CachedChildren[i].Children[1].Name, i);
                }
                return indexToName;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public void DeletePrevSearchedAccount(UIAutomationElement AccountStrategyGrid)
        {
            try
            {
                AccountStrategyGrid.AutomationElementWrapper.Children[0].CachedChildren[0].CachedChildren[0].CachedChildren[0].WpfClick();
                AccountStrategyGrid.AutomationElementWrapper.Children[0].CachedChildren[0].CachedChildren[0].CachedChildren[0].WpfClick();
                Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                DataUtilities.clearTextData(12, true);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                
            }
        }
        public void SendAcountName(UIAutomationElement AccountStrategyGrid,String account)
        {
            try
            {
                AccountStrategyGrid.AutomationElementWrapper.Children[0].CachedChildren[0].CachedChildren[0].CachedChildren[0].WpfClick();
                AccountStrategyGrid.AutomationElementWrapper.Children[0].CachedChildren[0].CachedChildren[0].CachedChildren[0].WpfClick();
                Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                Keyboard.SendKeys(account);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;

            }
        }
      
    }
}
