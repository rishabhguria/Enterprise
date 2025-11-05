using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    [UITestFixture]
    public partial class FixedPreferencesUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixedPreferencesUIMap"/> class.
        /// </summary>
        public FixedPreferencesUIMap()
        {
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
                //Wait(15000);
                //Trade.Click(MouseButtons.Left);
                //Allocation2.Click(MouseButtons.Left);
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
                KeyboardUtilities.MinimizeWindow(ref Allocation3);
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
        public void OpenFixedPreferences()
        {
            try
            {
                    OpenAllocation();
                    EditAllocationPreferences.Click(MouseButtons.Left);
                    FixedPreferences1.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Closes the fixed preferences.
        /// </summary>
        public void CloseFixedPreferences()
        {
            try
            {
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
        public void ClearText()
        {
            try
            {
                Keyboard.SendKeys(KeyboardConstants.ENDKEY + KeyboardConstants.SHIFTHOMEKEY + KeyboardConstants.BACKSPACEKEY);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
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
        /// Initializes a new instance of the <see cref="FixedPreferencesUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
      public FixedPreferencesUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

      /// <summary>
      /// Gets the latest grid object.
      /// </summary>
      /// <param name="gridObject">The grid object.</param>
      /// <returns></returns>
      public static UIAutomationElement GetLatestGridObject(UIAutomationElement gridObject)
      {
          UIAutomationElement TempRecords = new UIAutomationElement();
          try
          {
              TempRecords.AutomationName = TestDataConstants.CONST_RECORDS;
              TempRecords.ClassName = TestDataConstants.CONST_VIEWABLERECORDCOLLECTION;
              TempRecords.Comment = null;
              TempRecords.ItemType = "";
              TempRecords.MatchedIndex = 0;
              TempRecords.Name = TestDataConstants.CONST_RECORDS;
              TempRecords.ObjectImage = null;
              TempRecords.Parent = gridObject;
              TempRecords.UIObjectType = UIObjectTypes.Unknown;
              TempRecords.UseCoordinatesOnClick = true;
          }
          catch (Exception ex)
          {
              bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
              if (rethrow)
                  throw;
          }
          return TempRecords;
      }

      /// <summary>
      /// Clicks the on ComboBox item.
      /// </summary>
      /// <param name="pref">The Preference name.</param>
      /// <param name="combo">The combo.</param>
      public void ClickOnComboBoxItem(string pref, UIAutomationElement combo)
      {
          try
          {
              var cacheChildren = combo.AutomationElementWrapper.CachedChildren;
              Dictionary<string, int> nameToIndexMapping = new Dictionary<string, int>();

              for (int i = 1; i < cacheChildren.Count; i++)
              {
                  nameToIndexMapping.Add(cacheChildren[i].CachedChildren[0].Name, i);
              }
              if (nameToIndexMapping.ContainsKey(pref))
                  cacheChildren[nameToIndexMapping[pref]].CachedChildren[0].WpfClick();
              //else
              //    // throw new Exception(MessageConstants.MSG_PREF_NOT_FOUND);
              //    throw;
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
