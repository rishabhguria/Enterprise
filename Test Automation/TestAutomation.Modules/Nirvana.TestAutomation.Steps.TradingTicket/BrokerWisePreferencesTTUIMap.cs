using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    [UITestFixture]
    public partial class BrokerWisePreferencesTTUIMap : UIMap
    {
        public BrokerWisePreferencesTTUIMap()
        {
            InitializeComponent();
        }

        public BrokerWisePreferencesTTUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

    }
}
