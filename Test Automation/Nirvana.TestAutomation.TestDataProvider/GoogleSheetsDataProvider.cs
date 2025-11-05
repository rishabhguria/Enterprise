using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.TestDataProvider
{
    public class GoogleSheetsDataProvider : ITestDataProvider
    {
        /// <summary>
        /// Connects to google sheets service.
        /// </summary>
        /// <returns></returns>
        private static SheetsService ConnectToGoogleSheetsService()
        {
            SheetsService service = null;
            try
            {
                // If modifying these scopes, delete your previously saved credentials at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
                string[] Scopes = { SheetsService.Scope.Spreadsheets };
                string ApplicationName = "Google Sheets API Test Data";

                UserCredential credential;

                using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
                }

                // Create Google Sheets API service.
                service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
            return service;
        }

        /// <summary>
        /// Gets the data set from value response.
        /// </summary>
        /// <param name="response">The response.</param>
        private static DataSet GetDataSetFromValueResponse(ValueRange response)
        {
            DataSet ds = new DataSet();
            try
            {
                IList<IList<Object>> values = response.Values;
                ds.Tables.Add(new DataTable());
                if (values.Count > 0)
                {
                    for (int i = 0; i < values.First().Count; i++)
                        ds.Tables[0].Columns.Add();
                }
                foreach (IList<Object> obj in values)
                {
                    ds.Tables[0].Rows.Add(obj.ToArray());
                }
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
            return ds;
        }

        /// <summary>
        /// Gets the test data.
        /// </summary>
        /// <param name="spreadsheetId">The spreadsheet identifier.</param>
        /// <returns></returns>
        public DataSet GetTestData(string spreadsheetId, int rowHeaderIndex = 1, int startColumnFrom = 1, string fileType = "")
        {
            DataSet dataSet = new DataSet();
            try
            {
                SheetsService service = ConnectToGoogleSheetsService();
                if (service != null)
                {
                    String range = "Sheet1!A1:Z1000";
                    SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

                    ValueRange response = request.Execute();
                    dataSet = GetDataSetFromValueResponse(response);
                }
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
            return dataSet;
        }
    }
}
