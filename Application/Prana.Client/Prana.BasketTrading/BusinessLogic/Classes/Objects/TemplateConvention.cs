using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BasketTrading
{
    public class TemplateConvention
    {

        private string _conventionMappingID = string.Empty;
        private string _percentage = string.Empty;
        private string _roundLot = string.Empty;


        public string ConventionMappingID
        {
            set { _conventionMappingID = value; }
            get { return _conventionMappingID; }
        }

        public string Percentage
        {
            set { _percentage = value; }
            get { return _percentage; }
        }

        public string RoundLot
        {
            set { _roundLot = value; }
            get { return _roundLot; }
        }
        
    }
}
