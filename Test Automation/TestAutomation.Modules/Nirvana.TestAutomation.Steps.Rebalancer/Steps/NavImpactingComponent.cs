using System.Linq;
using System.Text;
using TestAutomationFX.UI;
using System.Threading.Tasks;
using System.Data;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class NavImpactingComponent : RebalancerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();

                Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);
                NAVImpactingComponents1.Click(MouseButtons.Left);
                if (PageRight.IsVisible)
                {
                    PageRight.Click(MouseButtons.Left);
                }
                if (testData != null)
                {
                    AddNav(testData);
                }
                Wait(2000);
                Recalculate.Click(MouseButtons.Left);
                Wait(2000);
                NAVImpactingComponents1.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }

        private void AddNav(DataSet data)
        {
            try
            {
                int count = 1;
                string value = string.Empty;

                //var msobject = 
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    if (!dr[TestDataConstants.COL_USERADJUSTEDNAV].ToString().Equals(string.Empty))
                    {

                        switch (count)
                        {
                            case 1:
                                value = dr[TestDataConstants.COL_USERADJUSTEDNAV].ToString();
                                //0CheckBox11.Click(MouseButtons.Left);

                                XamMaskedEditor3.DoubleClick(MouseButtons.Left);
                                Keyboard.SendKeys(value);
                                ControlPartOfCheckBox11.Click(MouseButtons.Left);
                                //CheckBox11.Click(MouseButtons.Left);
                                //Keyboard.SendKeys(KeyboardConstants.SPACE);
                                count++;
                                break;
                            case 2:
                                value = dr[TestDataConstants.COL_USERADJUSTEDNAV].ToString();
                                try
                                {
                                    XamMaskedEditor4.DoubleClick(MouseButtons.Left);
                                    Keyboard.SendKeys(value);
                                    ControlPartOfCheckBox12.Click(MouseButtons.Left);
                                    count++;
                                    break;

                                }
                                catch (Exception) { }


                                try
                                {

                                    XamMaskedEditor8.DoubleClick(MouseButtons.Left);
                                    Keyboard.SendKeys(value);
                                    ControlPartOfCheckBox16.Click(MouseButtons.Left);
                                    count++;
                                    break;


                                }
                                catch (Exception) { }

                                try
                                {
                                    XamCheckEditor7.DoubleClick(MouseButtons.Left);
                                    Keyboard.SendKeys(value);
                                    ControlPartOfCheckBox15.Click(MouseButtons.Left);
                                    count++;
                                    break;

                                }
                                catch (Exception) { }

                                break;
                            case 3:
                                value = dr[TestDataConstants.COL_USERADJUSTEDNAV].ToString();
                                XamMaskedEditor5.Click(MouseButtons.Left);
                                Keyboard.SendKeys(value);
                                ControlPartOfCheckBox13.Click(MouseButtons.Left);
                                count++;
                                break;
                            case 4:
                                value = dr[TestDataConstants.COL_USERADJUSTEDNAV].ToString();
                                XamMaskedEditor6.Click(MouseButtons.Left);
                                Keyboard.SendKeys(value);
                                ControlPartOfCheckBox14.Click(MouseButtons.Left);
                                count++;
                                break;

                        }
                    }
                }
            }
            catch (Exception) { throw; }
        }
    }
}
