using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.TradeServer
{
    [UITestFixture]
    public partial class TradeServerUIMap : UIMap
    {
        public TradeServerUIMap()
        {
            InitializeComponent();
        }

        public TradeServerUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
