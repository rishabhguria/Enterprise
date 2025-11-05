using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class PositionEligibleForClosing : Position
    {
        private int _closingQuantity;

        /// <summary>
        /// Gets or sets my property.
        /// </summary>
        /// <value>My property.</value>
        public int ClosingQuantity
        {
            get { return _closingQuantity; }
            set { _closingQuantity = value; }
        }

        
    }
}
