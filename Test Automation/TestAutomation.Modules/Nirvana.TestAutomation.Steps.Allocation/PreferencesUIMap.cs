using System;
using System.ComponentModel;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    [UITestFixture]
    public partial class PreferencesUIMap : UIMap
    {
        public PreferencesUIMap()
        {
            InitializeComponent();
        }

        public PreferencesUIMap(IContainer container)
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
                //Trade.Click(MouseButtons.Left);
                //Allocation3.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
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
                Allocation4.Click(MouseButtons.Left);
                KeyboardUtilities.MinimizeWindow(ref Allocation4);
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
        /// For minimizing Allocation windows
        /// </summary>
        public void MaximizePreferenceWindow()
        {
            try
            {
                AllocationPreferences2.Click(MouseButtons.Left);
                KeyboardUtilities.MaximizeWindow(ref AllocationPreferences2);
                Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Opens the Attribute Renaming.
        /// </summary>
        public void OpenAttributeRenaming()
        {
            try
            {

                Preferences.Click(MouseButtons.Left);
                AttributeRenaming.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves and closes Attribute Renaming
        /// </summary>
        public void CloseAttributeRenaming()
        {
            try
            {
                Save.Click(MouseButtons.Left);
                if (NirvanaPreferences.IsVisible)
                    ButtonOK.Click(MouseButtons.Left);
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
        /// Clicks the on ComboBox item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="combo">The combo.</param>
        public void ClickOnComboBoxItem(string item, UIAutomationElement combo)
        {
            try
            {
                var cacheChildren = combo.AutomationElementWrapper.CachedChildren;
                Dictionary<string, int> nameToIndexMapping = new Dictionary<string, int>();

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
        /// Creates the dictionary.
        /// </summary>
        /// <param name="ComboElement">The combo element.</param>
        /// <returns></returns>
        public Dictionary<string, int> CreateDictionary(UIAutomationElement ComboElement)
        {
            Dictionary<String, int> NameToIndex = new Dictionary<String, int>();
            try
            {
                int count = ComboElement.AutomationElementWrapper.CachedChildren.Count;
       
                for (int i = 1; i < count; i++)
                {
                    int index = Convert.ToInt32(ComboElement.AutomationElementWrapper.CachedChildren[i].Index);
                    string name = ComboElement.AutomationElementWrapper.CachedChildren[i].CachedChildren[0].Name;
                    NameToIndex.Add(name, index);
                }
                
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return NameToIndex;
           
        }
        /// <summary>
        /// Select Drop Down Button in ProrataAllocationSchemeBasis
        /// </summary>
        public void ProrataSchemeBasisDropButton()
        {
            try
            {
                ControlPartOfCmbboxProrataAllocationSchemeBasis.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
             }
            
        }
        protected Dictionary<string, int> CreateAccountDictionary()
        {
            Dictionary<string, int> AccountNameToId = new Dictionary<string, int>();
            AccountNameToId.Add("OFFSHORE", 1182);
            AccountNameToId.Add("LP C/O", 1183);
            AccountNameToId.Add("Allocation2", 1184);
            AccountNameToId.Add("Allocation3", 1185);
            AccountNameToId.Add("Allocation1", 1186);
            AccountNameToId.Add("rt", 1189);
            AccountNameToId.Add("Allocation4", 1190);

            return AccountNameToId;
        }
        protected Dictionary<string, int> CreateAssetDictionary()
        {
            Dictionary<string, int> AssetNameToId = new Dictionary<string, int>();
            AssetNameToId.Add("Equity", 1);
            AssetNameToId.Add("EquityOption", 2);
            AssetNameToId.Add("Future", 3);
            AssetNameToId.Add("FutureOption", 4);
            AssetNameToId.Add("FX", 5);
            AssetNameToId.Add("Cash", 6);
            AssetNameToId.Add("Indices", 7);
            AssetNameToId.Add("FixedIncome", 8);
            AssetNameToId.Add("PrivateEquity", 9);
            AssetNameToId.Add("FXOption", 10);
            AssetNameToId.Add("FXForward", 11);
            AssetNameToId.Add("Forex", 12);
            AssetNameToId.Add("ConvertibleBond", 13);
            AssetNameToId.Add("CreditDefaultSwap", 14);
            return AssetNameToId;
        }
        protected Dictionary<string, int> CreateCounterPartyDictionary()
        {
            Dictionary<string, int> CounterPartyNameToId = new Dictionary<string, int>();
            CounterPartyNameToId.Add("MS", 1);
            CounterPartyNameToId.Add("GS", 2);
            CounterPartyNameToId.Add("CSFB", 5);
            CounterPartyNameToId.Add("PiperJaffray", 6);
            CounterPartyNameToId.Add("Bernstein", 7);
            CounterPartyNameToId.Add("STN", 9);
            CounterPartyNameToId.Add("FIMAT", 10);
            CounterPartyNameToId.Add("Source", 11);
            CounterPartyNameToId.Add("Lakeshore", 12);
            CounterPartyNameToId.Add("Wolverine", 13);
            CounterPartyNameToId.Add("DC", 14);
            CounterPartyNameToId.Add("DB", 15);
            CounterPartyNameToId.Add("UBS", 16);
            CounterPartyNameToId.Add("dik", 17);
            CounterPartyNameToId.Add("NSEW", 98);
            CounterPartyNameToId.Add("MSCOS", 99);
            return CounterPartyNameToId;
        }
    }
}
