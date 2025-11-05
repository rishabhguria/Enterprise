using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class RunUploadManager
    {
        /// <summary>
        /// Gets the run upload set up data for upload client ID.
        /// </summary>
        /// <param name="uploadClientID">The upload client ID.</param>
        /// <returns></returns>
        public static RunUploadSetUpSummary GetRunUploadSetUpDataForUploadClientID(int uploadClientID)
        {
            RunUploadSetUpSummary runUploadSetUpSummary = new RunUploadSetUpSummary();

            // dummy data
            //CompanyNameID company
            //runUploadSetUpSummary.CompanyNameID = new CompanyNameID(uploadClientID, "Nirvana Financial");

            runUploadSetUpSummary.CompanyNameID = new CompanyNameID();

            SortableSearchableList<RunUploadSetup> setUpData = new SortableSearchableList<RunUploadSetup>();

            RunUploadSetup row1 = new RunUploadSetup();
            row1.CompanyNameID.ID = 1;
            row1.CompanyNameID.FullName = "Nirvana Financial Solutions";
            row1.CompanyNameID.ShortName = "AS";
            row1.DataSourceNameID.ID = 1;
            row1.DataSourceNameID.ShortName = "GS";
            row1.DataSourceNameID.FullName = "GoldMan Sachs";
            row1.DirPath = "C:\\UploadData";
            row1.EnableAutoTime = false;
            row1.AutoTime = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
            row1.FileName = "uploadatafile.csv";
            row1.FTPServer = "122.123.232.23";
            row1.Password = "password";
            row1.ServerPort = 5001;
            row1.UserName = "kevinsingh";
            setUpData.Add(row1);            

            RunUploadSetup row2 = new RunUploadSetup();
            row2.CompanyNameID.ID = 1;
            row2.CompanyNameID.FullName = "Nirvana Financial Solutions";
            row2.CompanyNameID.ShortName = "AS";
            row2.DataSourceNameID.ID = 32;
            row2.DataSourceNameID.FullName = "Merrill Lynch";
            row2.DataSourceNameID.ShortName = "ML";
            row2.DirPath = "C:\\UploadData";
            row2.EnableAutoTime = true;
            row2.AutoTime = Convert.ToDateTime(DateTime.Now.AddHours(1.0).ToLongTimeString());
            row2.FileName = "uploadatafile.csv";
            row2.FTPServer = "245.223.132.223";
            row2.Password = "password";
            row2.ServerPort = 8001;
            row2.UserName = "PeterVerma";
            setUpData.Add(row2);

            runUploadSetUpSummary.RunUploadSetUp = setUpData;

            //for (int counter = 0; counter < 2; counter++)
            //{
            //    RunUploadSetup runUploadSetUpRow = new RunUploadSetup();
            //    runUploadSetUpRow.CompanyNameID = new 

            //}


            //runUploadSetUpSummary.runUploadSetUP = new Nirvana.Admin.PositionManagement.Classes.SortableSearchableList<RunUploadSetup>();
            return runUploadSetUpSummary;

            

        }

        public static RunUploadSummary GetRunUploadDataForUploadClientID(int uploadClientID)
        {
            RunUploadSummary runUploadSummary = new RunUploadSummary();

            // dummy data
            //CompanyNameID company
            //runUploadSetUpSummary.CompanyNameID = new CompanyNameID(uploadClientID, "Nirvana Financial");

            runUploadSummary.CompanyNameID = new CompanyNameID();

            SortableSearchableList<RunUpload> runData = new SortableSearchableList<RunUpload>();

            RunUpload row1 = new RunUpload();
            row1.CompanyNameID.ID = 1;
            row1.CompanyNameID.FullName = "Nirvana Financial Solutions";
            row1.CompanyNameID.ShortName = "AS";
            row1.DataSourceNameID.ID = 1;
            row1.DataSourceNameID.ShortName = "GS";
            row1.DataSourceNameID.FullName = "GoldMan Sachs";
            row1.DirPath = "C:\\UploadData";
            row1.EnableAutoTime = false;
            row1.AutoTime = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
            row1.FileName = "uploadatafile.csv";
            row1.Status = RunUploadStatus.Awaiting;
            
            runData.Add(row1);

            RunUpload row2 = new RunUpload();
            row2.CompanyNameID.ID = 1;
            row2.CompanyNameID.FullName = "Nirvana Financial Solutions";
            row2.CompanyNameID.ShortName = "AS";
            row2.DataSourceNameID.ID = 32;
            row2.DataSourceNameID.FullName = "Merrill Lynch";
            row2.DataSourceNameID.ShortName = "ML";
            row2.DirPath = "C:\\UploadData";
            row2.EnableAutoTime = true;
            row2.AutoTime = Convert.ToDateTime(DateTime.Now.AddHours(-1.0).ToLongTimeString());
            row2.FileName = "uploadatafile.csv";
            row2.Status = RunUploadStatus.Successful;
            
            runData.Add(row2);

            runUploadSummary.RunUpload = runData;

            //for (int counter = 0; counter < 2; counter++)
            //{
            //    RunUploadSetup runUploadSetUpRow = new RunUploadSetup();
            //    runUploadSetUpRow.CompanyNameID = new 

            //}


            //runUploadSetUpSummary.runUploadSetUP = new Nirvana.Admin.PositionManagement.Classes.SortableSearchableList<RunUploadSetup>();
            return runUploadSummary;
        }

    }
}
