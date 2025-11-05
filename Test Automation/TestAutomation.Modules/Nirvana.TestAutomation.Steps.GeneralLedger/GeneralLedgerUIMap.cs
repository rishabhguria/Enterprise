using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    [UITestFixture]
    public partial class GeneralLedgerUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralLedgerUIMap"/> class.
        /// </summary>
        public GeneralLedgerUIMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralLedgerUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public GeneralLedgerUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
    }
}
