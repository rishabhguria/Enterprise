using Castle.Core.Logging;
using Moq;
using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static System.Collections.Specialized.BitVector32;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class EmailsHelperTests
    {
        [Fact]
        [Trait("Prana.Utilities", "EmailsHelper")]
        public void MailSend_WithNullRecipients_ThrowsArgumentNullException()
        {
            // Arrange
            var subject = "Test Subject";
            var body = "Test Body";
            var sender = "sender@example.com";
            var senderName = "Sender Name";
            var senderPassword = "password";
            string[] recipients = null;
            var port = 587;
            var host = "smtp.example.com";
            var enableSSL = true;
            var authenticationRequired = true;

            // Act
            Action action = () => EmailsHelper.MailSend(subject, body, sender, senderName, senderPassword, recipients, 
                port, host, enableSSL, authenticationRequired);

            //Assert
            Assert.Throws<NullReferenceException>(action);
        }

        [Fact]
        [Trait("Prana.Utilities", "EmailsHelper")]
        public void SendMail_FileDoesNotExist_DoesNotSendEmail()
        {
            // Arrange
            var filePath = MockDataPath.GetFolderPath();
            var fileName = "nonexistentfile.txt";
            var bbgFileImportReceiverIDs = "recipient1@example.com";
            var bbgFileImportSenderID = "sender@example.com";
            var bbgFileImportMailSubject = "Test Subject";
            var bbgFileImportMailBody = "Test Body";
            var bbgFileImportCCIDs = "";
            var bbgFileImportBCCIDs = "";
            var bbgFileImportSenderPWD = "password";
            var bbgFileImportMailServer = "smtp.example.com";
            var bbgFileImportMailServerSMTPPort = 587;
            var bbgFileImportSecureEmail = true;

            // Act
            var exception = Record.Exception(() => EmailsHelper.SendMail(
                filePath, fileName, bbgFileImportReceiverIDs, bbgFileImportSenderID,
                bbgFileImportMailSubject, bbgFileImportMailBody, bbgFileImportCCIDs,
                bbgFileImportBCCIDs, bbgFileImportSenderPWD, bbgFileImportMailServer,
                bbgFileImportMailServerSMTPPort, bbgFileImportSecureEmail));

            // Assert
            Assert.Null(exception);
        }
    }
}
