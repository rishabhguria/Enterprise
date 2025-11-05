using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    [UITestFixture]
    public partial class TTPreferencesUIMap : UIMap
    {
        public TTPreferencesUIMap()
        {
            InitializeComponent();
        }

        public TTPreferencesUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }


    }
}
