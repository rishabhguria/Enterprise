using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.CreateTransaction
{
    [UITestFixture]
    public partial class CreateTransactionUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTransactionUIMap"/> class.
        /// </summary>
        public CreateTransactionUIMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTransactionUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CreateTransactionUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }


        /// <summary>
        /// Opens the create transaction.
        /// </summary>
        public void OpenCreateTransaction()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open create transaction module(CTRL + ALT + S)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_CT"]);
                ExtentionMethods.WaitForVisible(ref CreatePosition, 15);
                //Wait(5000);
                //PortfolioManagement.Click(MouseButtons.Left);
                //CreateTransaction1.Click(MouseButtons.Left);
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
