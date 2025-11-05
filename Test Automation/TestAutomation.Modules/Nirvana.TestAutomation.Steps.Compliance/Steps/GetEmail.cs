using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Net.Mail;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.BussinessObjects;
using System.Data;
using Nirvana.TestAutomation.Interfaces;
using System.Text.RegularExpressions;

namespace Nirvana.TestAutomation.Steps.Compliance
{
    public partial class GetEmail : ITestStep
    {
       static string[] Scopes = {
                             GmailService.Scope.MailGoogleCom
               };
       static string ApplicationName = "Email Verification";
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                DataTable table = testData.Tables[0];
                GmailService ser = GetGmailService();
                // Define the user's email address
                string userEmail = "TestAutomation@nirvanasolutions.com";

                // Get the list of messages
                ListMessages(ser, userEmail, table);
                List<string> resultofEmailVerification = CompareTextFiles(table.Rows[0][TestDataConstants.COL_PATH].ToString());
                if (resultofEmailVerification.Count > 0)
                {
                    _result.ErrorMessage = String.Join("\n\r", resultofEmailVerification);
                }
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }


        public static GmailService GetGmailService()
        {
            
            try
            {
                UserCredential credential;
                using (var stream =
                      new FileStream("credential.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";


                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
                }


                // Create Gmail API service.   
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                return service;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        // Function to list messages
        static void ListMessages(GmailService service, String userId, DataTable table)
        {
            try
            {
                // Define parameters for the request
                UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(userId);
                request.LabelIds = "INBOX"; // Only retrieve messages from the inbox
                request.MaxResults = 150; // Limit the number of messages to retrieve

                // Get the response
                ListMessagesResponse response = request.Execute();

                string filePath = "emails.txt";
                if (File.Exists(filePath))
                {
                    // If the file exists, delete it
                    File.Delete(filePath);
                }

                // Process each message
                foreach (Message msg in response.Messages)
                {
                    // Get the message details
                    Message message = service.Users.Messages.Get(userId, msg.Id).Execute();

                    // Extract subject and body
                    string subject = "";
                    string body = "";
                    string contentBetweenDelimiters = "";
                    foreach (var header in message.Payload.Headers)
                    {
                        if (header.Name == "Subject")
                            subject = header.Value;
                        else if (header.Name == "From")
                            body = header.Value;
                    }
                    if (subject.ToString().Equals(table.Rows[0][TestDataConstants.COL_SUBJECT]) || body.ToString().Equals(table.Rows[0][TestDataConstants.COL_FROM]))
                    {

                        //string content = message.Snippet.ToString();
                        var payload = message.Payload;
                        string decodedString = GetBody(payload);

                        string content = RemoveHtmlTags(decodedString);
                        if (content.Contains("------------------------------------------"))
                        {
                            contentBetweenDelimiters = ExtractContentBetweenDelimiters(content);
                        }

                        // Write Summary of mail to a file
                        using (StreamWriter outputFile = new StreamWriter("emails.txt", true))
                        {
                            outputFile.WriteLine(contentBetweenDelimiters);
                            outputFile.WriteLine();
                        }
                    }
                    else
                        continue;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }

        static string GetBody(Google.Apis.Gmail.v1.Data.MessagePart messagePart)
        {
            if (messagePart.Parts == null)
            {
                return DecodeBase64(messagePart.Body.Data);
            }
            else
            {
                foreach (var part in messagePart.Parts)
                {
                    if (part.MimeType == "text/plain" || part.MimeType == "text/html")
                    {
                        return DecodeBase64(part.Body.Data);
                    }
                }
                return string.Empty;
            }
        }

        static string DecodeBase64(string base64String)
        {
            int padChars = (base64String.Length % 4) == 0 ? 0 : (4 - (base64String.Length % 4));
            StringBuilder result = new StringBuilder(base64String, base64String.Length + padChars);
            result.Append(String.Empty.PadRight(padChars, '='));
            result.Replace('-', '+');
            result.Replace('_', '/');
            byte[] bytes =  Convert.FromBase64String(result.ToString());
            return Encoding.UTF8.GetString(bytes);
        }

        static string RemoveHtmlTags(string input)
        {
            // Remove <p> tags and replace with line breaks
            string textWithLineBreaks = Regex.Replace(input, "<p>", Environment.NewLine);
            // Remove <br> tags and replace with line breaks
            textWithLineBreaks = Regex.Replace(textWithLineBreaks, "<br>", Environment.NewLine);
            // Remove <a> tags and keep the anchor text
            textWithLineBreaks = Regex.Replace(textWithLineBreaks, "<a[^>]*>(.*?)</a>", "$1");
            // Remove other HTML tags
            return Regex.Replace(textWithLineBreaks, "<.*?>", "");
        }

        static string ExtractContentBetweenDelimiters(string input)
        {
            string result = "";
            // Define pattern to extract content between delimiter lines
            string pattern = @"----------------------------------------------------\s*(.+?)\s*----------------------------------------------------";
            // Match the pattern in the input string
            Match match = Regex.Match(input, pattern, RegexOptions.Singleline);
            // Return the content between delimiter lines if a match is found, otherwise return empty string
            string value =  match.Success ? match.Groups[1].Value.Trim() : "";
            if (value.Contains("-----"))
            {
                // Find the index of the last occurrence of "----"
                int lastIndex = value.LastIndexOf("----");

                if (lastIndex != -1)
                {
                    // Extract the substring after the last occurrence of "----"
                     result = value.Substring(lastIndex + "----".Length).Trim();
                 
                }
            }
            return result;
        }

        static List<string> CompareTextFiles(string file)
        {
            List<string> missingLines = new List<string>();
            // Read the expected lines from the file
            string[] expectedLines = File.ReadAllLines(file);

            // Read the content of the email file line by line
            string[] emailLines = File.ReadAllLines("emails.txt");
            // Flag to track if all expected lines are found
            

        // Check if each expected line is present in the email file
            foreach (string expectedLine in expectedLines)
            {
                bool lineFound = false;
                foreach (string emailLine in emailLines)
                {
                    if (emailLine.Contains(expectedLine))
                    {
                        lineFound = true;
                        break;
                    }
                }

                if (!lineFound)
                {
                    missingLines.Add(expectedLine);
                    break;
                }
            }
            return missingLines;
        }
    }
}
