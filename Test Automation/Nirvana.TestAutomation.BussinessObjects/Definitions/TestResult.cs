using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.BussinessObjects
{
    public class TestResult
    {
        /// <summary>
        /// The is error
        /// </summary>
        private bool _isPassed = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c>.
        /// </value>
        public bool IsPassed
        {
            get { return _isPassed; }
            set { _isPassed = value; }
        }

        /// <summary>
        /// The error message
        /// </summary>
        private String _errorMessage = null;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public String ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                if (!string.IsNullOrWhiteSpace(_errorMessage))
                    IsPassed = false;
            }
        }

        public TestResult()
        {
            IsPassed = true;
            ErrorMessage = string.Empty;
        }

        public void AddResult(bool isPass, string error)
        {
            IsPassed = isPass;
            ErrorMessage = error;
        }
    }
}
