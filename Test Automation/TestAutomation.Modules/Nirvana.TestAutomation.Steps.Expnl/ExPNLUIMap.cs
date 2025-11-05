using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Expnl
{
    [UITestFixture]
    public partial class ExPNLUIMap : UIMap
    {
        public ExPNLUIMap()
        {
            InitializeComponent();
        }
        public ExPNLUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
    }
}
