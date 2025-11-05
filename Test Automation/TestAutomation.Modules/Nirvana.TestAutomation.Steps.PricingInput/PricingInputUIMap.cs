using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.PricingInput
{
    [UITestFixture]
    public partial class PricingInputUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PricingInputUIMap"/> class.
        /// </summary>
        public PricingInputUIMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PricingInputUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public PricingInputUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
    }
}
