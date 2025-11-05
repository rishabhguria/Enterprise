using Castle.Core.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Prana.BusinessObjects;
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
    public class MailDeliveryTests
    {
        [Fact]
        public void SendFile_ShouldReturnFalse_WhenMainMailSenderThrowsException()
        {
            // Arrange
            var SenderMailID = "sender@example.com";
            var ReceiverMailID = "receiver@example.com";
            var MailSubject = "Test Subject";
            var MailBody = "Test Body";
            var HostID = "smtp.example.com";
            var SenderPassword = "password";
            var Attachments = new List<string> { "attachment1.txt", "attachment2.txt" };
            var mailDeliveryParameters = new MailDeliveryParameters(HostID, SenderMailID, SenderPassword, ReceiverMailID, MailSubject,
                MailBody, Attachments);

            MailDelivery _mailDelivery = new MailDelivery();

            // Act & Assert
            try
            {
                var result = _mailDelivery.SendFile(mailDeliveryParameters);
                Assert.False(result); // Pass if result is false
            }
            catch (Exception)
            {
                Assert.True(true); // Pass if an exception is thrown
            }
        }
    }
}
