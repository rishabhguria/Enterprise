using Moq;
using Prana.BusinessObjects;
using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class EmailSenderTests
    {
        [Fact]
        [Trait("Prana.Utilities", "EmailSender")]
        public void SendEmail_ShouldReturnTrue_WhenSettingsIsNull()
        {
            // Arrange
            ThirdPartyEmail settings = null;
            List<string> mailAttachments = null;
            string status = "TestStatus";
            string body = "TestBody";

            // Act
            var result = EmailSender.SendEmail(settings, mailAttachments, status, body);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "EmailSender")]
        public void SendEmail_ShouldReturnFalse_WhenMailToIsInvalid()
        {
            // Arrange
            var settings = new ThirdPartyEmail
            {
                Enabled = true,
                MailTo = "invalid,email",
                MailFrom = "test@test.com",
                Smtp = "smtp.test.com",
                Port = 587,
                SSLEnabled = true,
                UserName = "username",
                Password = "password",
                Priority = MailPriority.Normal
            };
            List<string> mailAttachments = null;
            string status = "TestStatus";
            string body = "TestBody";

            // Act
            var result = EmailSender.SendEmail(settings, mailAttachments, status, body);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "EmailSender")]
        public void SendEmail_ShouldReturnFalse_WhenMailFromIsInvalid()
        {
            // Arrange
            var settings = new ThirdPartyEmail
            {
                Enabled = true,
                MailTo = "test@test.com",
                MailFrom = "",
                Smtp = "smtp.test.com",
                Port = 587,
                SSLEnabled = true,
                UserName = "username",
                Password = "password",
                Priority = MailPriority.Normal
            };
            List<string> mailAttachments = null;
            string status = "TestStatus";
            string body = "TestBody";

            // Act
            var result = EmailSender.SendEmail(settings, mailAttachments, status, body);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "EmailSender")]
        public void SendEmail_ShouldReturnTrue_WhenSettingsAreValid()
        {
            // Arrange
            var settings = new ThirdPartyEmail
            {
                Enabled = true,
                MailTo = "test@test.com",
                MailFrom = "from@test.com",
                Smtp = "smtp.test.com",
                Port = 587,
                SSLEnabled = true,
                UserName = "username",
                Password = "password",
                Priority = MailPriority.Normal
            };
            List<string> mailAttachments = null;
            string status = "TestStatus";
            string body = "TestBody";

            // Act
            var result = EmailSender.SendEmail(settings, mailAttachments, status, body);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "EmailSender")]
        public void SendEmail_ShouldReturnFalse_WhenAttachmentsPathWrong()
        {
            // Arrange
            var settings = new ThirdPartyEmail
            {
                Enabled = true,
                MailTo = "test@test.com",
                MailFrom = "from@test.com",
                Smtp = "smtp.test.com",
                Port = 587,
                SSLEnabled = true,
                UserName = "username",
                Password = "password",
                Priority = MailPriority.Normal
            };
            List<string> mailAttachments = new List<string> { "attachment1", "attachment2" };
            string status = "TestStatus";
            string body = "TestBody";

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => EmailSender.SendEmail(settings, mailAttachments, status, body));
        }

        [Fact]
        [Trait("Prana.Utilities", "EmailSender")]
        public void SendEmail_ShouldReturnTrue_WhenAttachmentsPathCorrect()
        {
            // Arrange
            var settings = new ThirdPartyEmail
            {
                Enabled = true,
                MailTo = "test@test.com",
                MailFrom = "from@test.com",
                Smtp = "smtp.test.com",
                Port = 587,
                SSLEnabled = true,
                UserName = "username",
                Password = "password",
                Priority = MailPriority.Normal
            };
            string filePath = String.Format("{0}CreateDataTable.cs",MockDataPath.GetFolderPath());
            List<string> mailAttachments = new List<string> { filePath };
            string status = "TestStatus";
            string body = "TestBody";

            // Act
            var result = EmailSender.SendEmail(settings, mailAttachments, status, body);

            // Assert
            Assert.True(result);
        }
    }
}
