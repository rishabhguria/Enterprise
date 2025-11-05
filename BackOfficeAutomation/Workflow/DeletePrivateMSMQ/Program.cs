using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Messaging;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace DeletePrivateMSMQ
{
    internal class Program
    {
        public static string logFileName = "";
        public static string sendLogMessage = "";
        static void Main(string[] args)
        {
            DateTime finalTime, initialTime;
            initialTime = DateTime.Now;
            try
            {
                //Console.ForegroundColor = ConsoleColor.Green;

                //Read text file in which user have provided the queue names and read all lines of textfile
                string currentDirectory = Directory.GetCurrentDirectory();
                //string parentDirectory = Directory.GetParent(currentDirectory).FullName;
                //string queueNameFilePath = ConfigurationManager.AppSettings["queueNameTextfilePath"];

                string queueNameFilePath = Path.Combine(currentDirectory, "QueueNamesToBeDeleted.txt");
                //string queueNameFilePath = @"C:\Users\Vipul.Garg\source\repos\DeletePrivateMSMQ\bin\QueueNamesToBeDeleted.txt";
                string[] queueNames = File.ReadAllLines(queueNameFilePath).Select(name => name.Trim()).Where(name => !string.IsNullOrWhiteSpace(name)).ToArray();


                //First Initialize the log file 
                InitializeLogFile(currentDirectory);
                GetIPAddress();

                //Append the log file and print the name of queues that user have provided
                //Console.WriteLine("\nUser provided queue name(s) are :");
                bool cancontinue =ValidAndInvalidQueueNames(queueNames);
                if (cancontinue)
                {
                    //check if all queue are in rest state or in runnoing state 
                    //if find running then break otherwise proceed further
                    bool proceedFurther = CheckAllQueueState_TextFile(queueNames);
                    if (proceedFurther == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please first clear the Message Queue then try again....");
                        Log("Pending message in queue so queue deletion process did not proceed", DateTime.Now);
                        return;
                    }
                    else
                    {
                        //We are checking that if user have provided argument "/all" to delete all queues if yes then we delete all queues
                        //if not then we will ask user want to delete all queues if user provide yes then delete all otherwise take confirmation before deleting each queue
                        if (args.Length > 0)
                        {
                            if (args[0].Trim() == "/all")
                            {
                                Log("User wants to delete all queues without asking for each one", DateTime.Now);
                                foreach (string queueName in queueNames)
                                {
                                    DisplayCountAndDeleteQueue(queueName, true);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nDo you want to delete all queues without asking for each one?[Y/y/N/n] :");
                            string confirmationToDeleteAll = Console.ReadLine().ToUpper();
                            //string demo = " n";
                            //string confirmationToDeleteAll = demo.Trim().ToUpper();
                            if (confirmationToDeleteAll == "Y")
                            {
                                Log("User wants to delete all queues without asking for each one", DateTime.Now);
                                foreach (string queueName in queueNames)
                                {
                                    DisplayCountAndDeleteQueue(queueName, true);
                                }
                            }
                            else
                            {
                                Log("User wants to delete  queue with asking for each one", DateTime.Now);
                                foreach (string queueName in queueNames)
                                {
                                    DisplayCountAndDeleteQueue(queueName, false);
                                }
                            }
                        }
                        Log("\n\n", DateTime.Now);
                    }
                }
                else
                {
                    Log("No valid queue names found.", DateTime.Now);

                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Log($"Exception caught in Main function \n Error: {ex.Message}", DateTime.Now);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            finalTime = DateTime.Now;

            TimeSpan timeDifference = finalTime - initialTime;
            string totalTimeTaken = timeDifference.Hours + " Hours " + timeDifference.Minutes + " Minutes " + timeDifference.Seconds + " Seconds " + timeDifference.Milliseconds + " Milliseconds ";
            Console.WriteLine("Total Time Taken : " + totalTimeTaken);

            string timeLogMessage = "Total Time Taken : " + totalTimeTaken;
            Log(timeLogMessage, DateTime.Now);
        }
        static void InitializeLogFile(string currentDirectory)
        {
            // Get the current date and time
            DateTime currentDate = DateTime.Now;
            // string[] subdirectories = Directory.GetDirectories(currentDirectory);
            //string logFolderPath = Path.Combine(currentDirectory, subdirectories[0] + @"\Logs");
            string logFolderPath = "Logs";

            //string vx = ConfigurationManager.AppSettings.ToString();
            // Create a filename with the current date and time
            logFileName += Path.Combine(logFolderPath, $"Log_{currentDate.ToString("yyyyMMdd")}.txt");
            //Log("\nLog file is created at this Directory" + logFileName);


            try
            {
                if (File.Exists(logFileName))
                {
                    Log("\n\n--Log file created on " + currentDate + "--", DateTime.Now);
                    Log("Log file is created at this Directory " + logFileName, DateTime.Now);

                }
                else
                {
                    // Create the log file
                    using (StreamWriter writer = File.CreateText(logFileName))
                    {
                        writer.WriteLine($"--- Log file created on {currentDate} ---");
                    }

                    // console.writeLine("Log file created: " + logFilename);

                }
            }
            catch (Exception ex)
            {
                // If there is an error, display the error message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error creating log file: " + ex.Message);
            }
        }

        static void Log(string logMessage, DateTime currentDateTime)
        {
            try
            {
                string formattedDateTime = currentDateTime.ToString("[yyyy-MM-dd HH:mm:ss] ");
                string finalLog = formattedDateTime + logMessage;
                File.AppendAllText(logFileName, Environment.NewLine + finalLog);

            }
            catch (Exception ex)
            {
                // If there is an error, display the error message
                // console.writeLine("Error writing to log file: " + ex.Message);
                using (StreamWriter writer = File.AppendText(logFileName))
                {
                    logMessage += "Error while writing this message to log file: " + ex.Message;
                    writer.Write(logMessage);
                }
            }
        }

        static void DisplayCountAndDeleteQueue(string QueueName, bool silent_Confirmation)
        {
            try
            {
                //Console.ForegroundColor = ConsoleColor.Green;
                Log("Execution of  DisplayCountAndDeleteQueue started successfully for queue Name provided by User : " + QueueName, DateTime.Now);

                var privateQueues = MessageQueue.GetPrivateQueuesByMachine(Environment.MachineName);

                //Extracting the queue name 
                // string trimmedqueueName = QueueName.Trim();
                // string queuePath = ".\\private$\\" + trimmedqueueName;
                //string[] parts = queuePath.Split('\\');
                //string queueName = parts[parts.Length - 1];
                //here
                foreach (var queue in privateQueues)
                {
                    if (queue.QueueName.IndexOf(QueueName, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        //Extracting the queue name 
                        string trimmedqueueName = queue.QueueName.Trim();
                        string queuePath = ".\\" + trimmedqueueName.Replace(@"\", "\\");
                        string[] parts = queuePath.Split('\\');
                        string queueName = parts[parts.Length - 1];
                        if (MessageQueue.Exists(queuePath))
                        {
                            //Ask the user to confirmation if already given to all then delete otherwise ask to user 
                            string deleteQueueConfirmation = "";
                            if (silent_Confirmation == true)
                            {
                                deleteQueueConfirmation = "Y";
                            }
                            else
                            {
                                int messageCount = GetMessageCount(queuePath);
                                Console.WriteLine($"Queue {trimmedqueueName} contains {messageCount} messages.");
                                Log($"Queue {trimmedqueueName} contains {messageCount} messages.", DateTime.Now);

                                Console.WriteLine("Do you want to delete queue [Y/y/N/n]: " + trimmedqueueName);
                                deleteQueueConfirmation = Console.ReadLine().Trim().ToUpper();
                            }

                            //now check if user give "Y" to delete queue then delete the queue otherwise not 

                            if (deleteQueueConfirmation == "Y")
                            {
                                int messageCount = GetMessageCount(queuePath);
                                //Console.WriteLine($"Queue {trimmedqueueName} contains {messageCount} messages.");
                                Log($"Queue {trimmedqueueName} contains {messageCount} messages.", DateTime.Now);
                                Log("User give confirmation as Yes to  delete the queue ", DateTime.Now);
                                //Console.WriteLine($"\nQueue {trimmedqueueName} contains {messageCount} messages.", DateTime.Now);
                                MessageQueue.Delete(queuePath);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Queue '{trimmedqueueName}' deleted successfully. ");
                                Log($"Queue '{trimmedqueueName}' deleted successfully. ", DateTime.Now);
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Log("User give confirmation as No to  delete the queue ", DateTime.Now);
                                Console.WriteLine($"Queue '{trimmedqueueName}' not deleted.");
                                Log($"So Queue '{trimmedqueueName}' not deleted. ", DateTime.Now);
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }

                        }
                    }
                }
                //here
            }
            catch (MessageQueueException mqe)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error deleting queues with partial name '{QueueName}': {mqe.Message}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Log($"Catch Exception in DisplayCountAndDeleteQueueWithBackup \n Error deleting queues with partial name '{QueueName}': {mqe.Message}", DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Log($"Catch Exception in DisplayCountAndDeleteQueue \n An error occurred: {ex.Message}", DateTime.Now);
            }
        }
        static int GetMessageCount(string queuePath)
        {
            try
            {
                MessageQueue queue = new MessageQueue(queuePath);
                int countOfMessages = queue.GetAllMessages().Length;
                return countOfMessages;
            }
            catch (Exception)
            {
                return 0; // Queue not found or other error occurred
            }
        }

        static bool CheckAllQueueState_TextFile(string[] queueNames)
        {
            try
            {
                List<KeyValuePair<string, int>> QueueWithMessages = new List<KeyValuePair<string, int>>();
                bool proceedFurther = true;
                var privateQueues = MessageQueue.GetPrivateQueuesByMachine(Environment.MachineName);

                //Console.WriteLine("\nUser provided queue name(s) are :");
                foreach (string queuename in queueNames)
                {
                    foreach (var queue in privateQueues)
                    {
                        if (queue.QueueName.IndexOf(queuename, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            //Extracting the queue name 
                            string trimmedqueueName = queue.QueueName.Trim();
                            string queuePath = ".\\" + trimmedqueueName.Replace(@"\", "\\");
                            string[] parts = queuePath.Split('\\');
                            string queueName = parts[parts.Length - 1];

                            //Extracting the queue name 
                            //  string trimmedqueueName = queueName.Trim();
                            //  string queuePath = ".\\private$\\" + trimmedqueueName;
                            //string queuePath = ".\\" + trimmedqueueName.Replace(@"\", "\\");
                            //string[] parts = queuePath.Split('\\');
                            //string queueName = parts[parts.Length - 1];

                            if (MessageQueue.Exists(queuePath))
                            {

                                int messageCount = GetMessageCount(queuePath);

                                if (messageCount > 0)
                                {
                                    QueueWithMessages.Add(new KeyValuePair<string, int>(trimmedqueueName, messageCount));
                                    proceedFurther = false;
                                }

                            }
                        }
                    }

                }
                if (proceedFurther == false)
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There are some queues in which messages are left those Names are : ");
                    Log("There are some queues in which messages are left those Names are : ", DateTime.Now);
                }
                foreach (var queueName in QueueWithMessages)
                {
                    Console.WriteLine("Name : " + queueName.Key + "Pending Messages Count : " + queueName.Value);
                    Log(queueName.Key + "   " + queueName.Value, DateTime.Now);
                }

                return proceedFurther;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Log($"Catch Exception in CheckAllQueueState_TextFile \n An error occurred: {ex.Message}", DateTime.Now);
                return false;
            }

        }
        static bool ValidAndInvalidQueueNames(string[] queueNames)
        {
            Dictionary<string, int> ValidQueueNames = new Dictionary<string, int>();
            List<string> InValidQueueNames = new List<string>();
            List<string> ProcessedQueueList = new List<string>();

            Console.WriteLine("List Of Queue Names Provided by user:");
            var privateQueues = MessageQueue.GetPrivateQueuesByMachine(Environment.MachineName);

            foreach (string qname in queueNames)
            {
                int validqueuecount=ValidQueueNames.Count;
              //  bool samekey = true;
                foreach (var queue in privateQueues)
                {
                    if (queue.QueueName.IndexOf(qname, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        //Extracting the queue name 
                        string trimmedqueueName = queue.QueueName.Trim();
                        string queuePath = ".\\" + trimmedqueueName.Replace(@"\", "\\");
                        string[] parts = queuePath.Split('\\');
                        string queueName = parts[parts.Length - 1];
                       
                        //if (ProcessedQueueList.Contains(queuePath))
                       // {
                            //string trimmedqueueName = queue.Trim();
                            //string queuePath = ".\\private$\\" + trimmedqueueName;
                            if (MessageQueue.Exists(queuePath))
                            {
                                int messageCount = GetMessageCount(queuePath);
                                if (!ValidQueueNames.ContainsKey(trimmedqueueName))
                                {
                                    ValidQueueNames.Add(trimmedqueueName, messageCount);
                                    //  samekey = false;
                                    ProcessedQueueList.Add(queuePath);
                                }
                            }
                            else
                            {
                               // if (!InValidQueueNames.Contains(qname))
                               // {
                                    InValidQueueNames.Add(qname);
                              //  }
                            }
                      //  }
                        
                    }
                    //else
                    //{
                    //    if (!InValidQueueNames.Contains(qname))
                    //    {
                    //        InValidQueueNames.Add(qname);
                    //    }
                    //}
                    
                }
                //if (ValidQueueNames.Count == validqueuecount && samekey)
                //{
                //    InValidQueueNames.Add(qname);
                    
                //}
            }
            //print Valid queue names
            int validCount = 1;
            if (ValidQueueNames.Count > 0)
            {
                Console.WriteLine("Valid Queue Names are: ");
                Log("Valid Queue Names are : ", DateTime.Now);
                foreach (KeyValuePair<string, int> kvp in ValidQueueNames)
                {

                    Console.WriteLine($"[{validCount}] QueueName: {kvp.Key} contains number of messages: {kvp.Value}");
                    Log($"[{validCount}] QueueName: {kvp.Key} contains number of messages: {kvp.Value}", DateTime.Now);
                    validCount++;
                }
            }
            //print InValid queue names 
            int invalidCount = 1;
            if (InValidQueueNames.Count > 0)
            {
                Console.WriteLine("Invalid Queue Names are: ");
                Log("Invalid Queue Names are : ", DateTime.Now);
                foreach (string invalidqueue in InValidQueueNames)
                {
                    Console.WriteLine($"[{invalidCount}] {invalidqueue}");
                    Log($"[{invalidCount}] {invalidqueue}", DateTime.Now);
                    invalidCount++;
                }
            }
            if (ValidQueueNames.Count == 0)
            {
                Console.WriteLine("No Valid Queue Names Found. Please Check Config File.");
                return false;
            }
            return true;
        }
        static void GetIPAddress()
        {
            // Get the host name of the local machine
            string hostName = Dns.GetHostName();

            // Get the IP addresses associated with the host
            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);

            //Console.WriteLine("IP Addresses for " + hostName + ":");
            Log("IP Addresses for " + hostName + ":", DateTime.Now);
            foreach (IPAddress ipAddress in ipAddresses)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork) // IPv4 address
                {
                    //Console.WriteLine("Current Ip "ipAddress);
                    string ip = ipAddress.ToString();
                    Log(ip, DateTime.Now);
                }
            }
        }
    }
}
