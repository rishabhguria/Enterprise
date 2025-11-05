using System;
using System.ComponentModel;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Dropcopy
{
    [UITestFixture]
    public partial class Dropcopy : UIMap
    {
        public Dropcopy()
        {
            InitializeComponent();
        }

        public Dropcopy(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
