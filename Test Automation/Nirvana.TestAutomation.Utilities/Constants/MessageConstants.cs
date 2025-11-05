using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities
{
    public static class MessageConstants
    {
        /// <summary>
        /// The MSG preference not found
        /// </summary>
        public const string MSG_PREF_NOT_FOUND = "Preference not found!";

        /// <summary>
        /// The MSG object not found
        /// </summary>
        public const string MSG_OBJ_NOT_FOUND = "Object not found!";

        /// <summary>
        /// The MSG master fund not added
        /// </summary>
        public const string MSG_MASTER_FUND_NOT_ADDED = "Master fund not found!";

        /// <summary>
        /// The MSG Application start up.
        /// </summary>
        public const string APPLICATION_START_UP = "Application Start Up";

        /// <summary>
        /// The MSG Login to application
        /// </summary>
        public const string LOGIN_TO_APPLICATION = "Login to the application";

        /// <summary>
        /// The Exception occured.
        /// </summary>
        public const string EXCEPTION_OCCURED = "Exception occured";

        /// <summary>
        /// Screen shot not captuted.
        /// </summary>
        public const string SCREENSHOT_NOT_CAPUTRED = "Screenshot not captured";

        /// <summary>
        /// Email Message
        /// </summary>
        public const string Email_Message = "<html> <body style ='border: 2px solid black; padding:20px;'><p>This is an automatically generated notification email from an unattended mailbox. Please do not directly reply. Open attached document(s) for your reference. For questions and concerns please contact support@nirvanasolutions.com or call 212-768-3410. </p><p>NOTICE - This message contains privileged and confidential information intended only for the use of the addressee named above. If you are not the intended recipient of this message, you are hereby notified that you must not disseminate, copy or take any action in reliance on it. If you have received this message in error, please immediately notify Nirvana Financial Solutions, INC, its subsidiaries or associates. Any views expressed in this message are those of the individual sender, except where the sender specifically states them to be the view of Nirvana Financial Solutions, INC , its subsidiaries and associates.</p></br>Thank you.</p> <h2>Nirvana Team </h2> </body></html> ";

        /// <summary>
        /// The module application
        /// </summary>
        public const string MODULE_APPLICATION = "Prana Application";

        /// <summary>
        /// Error message in case of application start up failure.
        /// </summary>
        public const string STARTUP_ERROR = @"Not able to start application.!";

        /// <summary>
        /// Error message in case of application login failure.
        /// </summary>
        public const string LOGIN_ERROR = @"Not able to login to application.!";

        /// <summary>
        /// Error message in case of MachineSpecificIssue.
        /// </summary>
        public const string MACHINE_ERROR = @"Machine Specific issue!";
    }
}
