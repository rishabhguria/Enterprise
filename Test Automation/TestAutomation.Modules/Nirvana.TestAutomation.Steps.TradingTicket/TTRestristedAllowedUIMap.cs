using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Factory;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.IO;
using System.Reflection;
using System.Globalization;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    [UITestFixture]
    public partial class TTRestristedAllowedUIMap : UIMap
    {
        public TTRestristedAllowedUIMap()
        {
            InitializeComponent();
        }

        public TTRestristedAllowedUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void OpenRestrictedAllowedTab()
        {
            //PranaMain.WaitForVisible();
            if (!PranaMain.IsVisible)
            {
                ExtentionMethods.WaitForVisible(ref PranaMain, 40);
            }
            //Shortcut to open Preferences under Tools (CTRL + ALT + F)
            Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PREF"]);
            //Wait(5000);
            //Tools.Click(MouseButtons.Left);
            //Preferences.Click(MouseButtons.Left);
            ExtentionMethods.WaitForVisible(ref PreferencesMain, 10);
            if (PreferencesMain.IsVisible)
            {
                TradingTicket.Click(MouseButtons.Left);
                RestrictedDivideAllowedSecurities.Click(MouseButtons.Left);
            }
        }

        public DataTable ExportSymbol()
        {
            ExportButton.Click(MouseButtons.Left);
            SaveAs.WaitForVisible();
            TextBoxFilename1.Click(MouseButtons.Left);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +@"\"+ TestDataConstants.Export_SymbolList;
            Keyboard.SendKeys(path);
            ButtonSave.Click(MouseButtons.Left);
            if (ConfirmSaveAs.IsVisible)
            {
                ButtonYes1.Click(MouseButtons.Left);
            }
            Wait(6000);
            ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.Xls);
            DataSet Exportdata = provider.GetTestData(path, 1, 1);
            DataTable dtexportdata = Exportdata.Tables[0];
            return dtexportdata;
        }
    }
}
