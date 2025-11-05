using System;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;

using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.NavLock
{
    [UITestFixture]
    public partial class NavLockUIMap : UIMap
    {
        public NavLockUIMap()
        {
            InitializeComponent();
        }

        public NavLockUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void OpenNavLock()
        {
            try
            {
                if (PranaMain.IsVisible)
                {
                    ControlPartOfPranaMain.Click(MouseButtons.Left);
                    Wait(2000);
                    Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                    Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                    Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                    Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }
            }
            catch (Exception) { throw; }
        }
    }
}
