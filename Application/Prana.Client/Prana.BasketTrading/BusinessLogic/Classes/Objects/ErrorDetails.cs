using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BasketTrading
{
     
    public class ErrorDetails
    {
        private string errorOrderSeqNumbers = "";
        private string errorTypes = "";
        public string ErrorOrderSeqNumbers
        {
            get { return errorOrderSeqNumbers; }
            set { errorOrderSeqNumbers=value ; }
        }
        public string ErrorTypes
        {
            get { return errorTypes; }
            set { errorTypes = value; }
        }

    }
}
