using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Utilities
{
    [UITestFixture]
    public partial class NavLockPopup : UIMap
    {
        public NavLockPopup()
        {
            InitializeComponent();
        }

        public NavLockPopup(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        

        public bool VerifyPopup(string val, string ui)
        {
            bool result = true;
            try
            {
                UIWindow wind = WindowType(ui);
                if (wind.IsEnabled)
                {
                    wind.BringToFront();
                    Wait(2000);
                    string content = wind.MsaaObject.CachedChildren[2].Name.ToString();
                    result = val.Equals(content);
                    // click on popup and close parent ui to run the new scenario
                    ClickAndCloseWin(ui);
                    //ButtonOK.Click();
                    Console.WriteLine("Popup verified");
                    
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            return result;
        }

        private void ClickAndCloseWin(string ui)
        {
            try
            {
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                switch (ui)
                {
                    case "TradingTicket":
                        KeyboardUtilities.CloseWindow(ref TradingTicket_UltraFormManager_Dock_Area_Top);
                        break;
                    case "Allocate":
                        KeyboardUtilities.CloseWindow(ref DockTop);
                        break;
                    case "AddFills":
                        KeyboardUtilities.CloseWindow(ref AddFills_UltraFormManager_Dock_Area_Top);
                        break;
                    case "Blotter":
                        KeyboardUtilities.CloseWindow(ref BlotterMain_UltraFormManager_Dock_Area_Top);
                        break;
                    case "MTT":
                        KeyboardUtilities.CloseWindow(ref MultiTradingTicket_UltraFormManager_Dock_Area_Top);
                        break;
                    case "GL":
                        KeyboardUtilities.CloseWindow(ref FrmCashManagementMain_UltraFormManager_Dock_Area_Top);
                        break;
                    case "Closing":
                        KeyboardUtilities.CloseWindow(ref CloseTrade_UltraFormManager_Dock_Area_Top);
                        break;
                    case "CT":
                        KeyboardUtilities.CloseWindow(ref CreatePosition_UltraFormManager_Dock_Area_Top);
                        break;
                    case "DailyValuation":
                        KeyboardUtilities.CloseWindow(ref MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top);
                        break;
                    default:
                        //do nothing
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private UIWindow WindowType(string ui)
        {
            switch (ui)
            {
                case "TradingTicket":
                    return NAVLock1;
                case "Allocate":
                    return NAVLock2;
                case "AddFills":
                    return NAVLock3;
                case "Blotter":
                    return NAVLock4;
                case "MTT":
                    return NAVLock5;
                case "GL":
                    return NAVLock6;
                case "Closing":
                    return NavLock7;
                case "CT":
                    return NAVLock8;
                case "DailyValuation":
                    return NAVLock9;
                default:
                    return NAVLock;
            }
        }
        
    }
}
