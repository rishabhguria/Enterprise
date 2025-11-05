using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class CorporateAction
    {
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _details;

        public string Details
        {
            get { return _details; }
            set { _details = value; }
        }

        private string _impactOnCash;

        public string ImpactOnCash
        {
            get { return _impactOnCash; }
            set { _impactOnCash = value; }
        }

        private string _impactOnPosition;

        public string ImpactOnPosition
        {
            get { return _impactOnPosition; }
            set { _impactOnPosition = value; }
        }

    }
}
