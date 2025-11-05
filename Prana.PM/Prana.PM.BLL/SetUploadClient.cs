using Csla;
using Csla.Validation;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using System;
using System.ComponentModel;

namespace Prana.PM.BLL
{
    /// <summary>
    /// Responsible for Running upload using Run Upload setting!
    /// This class is a mix of CompanyUploadsetup and UploadRuns
    /// TODO : Here the meaning of upload is import the file.
    /// The "Upload" term is used to insert data into the respective table in the database.
    /// </summary>
    [Serializable()]
    public class SetUploadClient : BusinessBase<SetUploadClient>
    {
        public SetUploadClient()
        {
            MarkAsChild();
        }
        #region Constants

        const string CONST_FTPServer = "FTPServer";
        const string CONST_FTPFilePath = "FTPFilePath";
        const string CONST_Port = "Port";
        const string CONST_UserName = "UserName";
        const string CONST_Password = "Password";
        const string CONST_DirPath = "DirPath";
        const string CONST_FileName = "FileName";
        const string CONST_ThirdPartyID = "ThirdPartyID";
        const string CONST_TableTypeID = "TableTypeID";
        const string CONST_FileLayoutType = "FileLayoutType";
        const string CONST_DataSourceXSLT = "DataSourceXSLT";
        const string CONST_TableFormatName = "TableFormatName";
        #endregion

        //System.Text.RegularExpressions.Regex regMatchFTPServer = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9.]*$");
        //System.Text.RegularExpressions.Regex regMatchDirPath = new System.Text.RegularExpressions.Regex("[a-zA-Z0-9:/]");
        //System.Text.RegularExpressions.Regex regMatchFileName = new System.Text.RegularExpressions.Regex("[a-zA-Z0-9._]");
        //System.Text.RegularExpressions.Regex regMatchUserName = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9._]*$");

        //private int _id;

        ///// <summary>
        ///// Gets or sets the ID.
        //Replaced by CompanyUploadSetupID, Rajat 25 Nov 2006
        ///// </summary>
        ///// <value>The ID.</value>
        //public int ID
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}



        private int _companyUploadSetupID;

        /// <summary>
        /// Gets or sets the company upload setup ID.
        /// </summary>
        /// <value>The company upload setup ID.</value>
        public int CompanyUploadSetupID
        {
            get { return _companyUploadSetupID; }
            set { _companyUploadSetupID = value; }
        }



        private CompanyNameID _companyNameID;

        /// <summary>
        /// Gets or sets the company name ID value.
        /// </summary>
        /// <value>The company name ID value.</value>
        public CompanyNameID CompanyNameIDValue
        {
            get
            {
                if (_companyNameID == null)
                {
                    _companyNameID = new CompanyNameID();
                }
                return _companyNameID;
            }
            set
            {
                _companyNameID = value;
                PropertyHasChanged();
            }
        }

        private int _companyID;

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        private int _thirdPartyID;

        /// <summary>
        /// Gets or sets the data source ID. Although this is a redundant field with respect to DataSourceNameIDValue
        /// but is required for grid binding !!
        /// </summary>
        /// <value>The data source ID.</value>
        public int ThirdPartyID
        {
            get { return _thirdPartyID; }
            set
            {
                _thirdPartyID = value;
                PropertyHasChanged(CONST_ThirdPartyID);
            }
        }


        private ThirdPartyNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public ThirdPartyNameID DataSourceNameIDValue
        {
            get
            {
                if (_dataSourceNameID == null)
                {
                    _dataSourceNameID = new ThirdPartyNameID();
                }
                return _dataSourceNameID;
            }
            set
            {
                _dataSourceNameID = value;
                //PropertyHasChanged(CONST_DataSourceNameIDValue);
            }
        }

        private bool _autoImport;

        /// <summary>
        /// Gets or sets a value indicating whether [enable auto time].
        /// </summary>
        /// <value><c>true</c> if [enable auto time]; otherwise, <c>false</c>.</value>
        public bool AutoImport
        {
            get { return _autoImport; }
            set
            {
                _autoImport = value;
                PropertyHasChanged();
            }
        }


        private DateTime _autoTime;

        /// <summary>
        /// Gets or sets the auto time.
        /// </summary>
        /// <value>The auto time.</value>
        public DateTime AutoTime
        {
            get { return _autoTime; }
            set
            {
                _autoTime = value;
                PropertyHasChanged();
            }
        }

        private string _ftpServer;

        /// <summary>
        /// Gets or sets the FTP server.
        /// </summary>
        /// <value>The FTP server.</value>
        public string FTPServer
        {
            get { return _ftpServer; }
            set
            {
                _ftpServer = value;
                PropertyHasChanged(CONST_FTPServer);
            }
        }

        private string _ftpFilePath = string.Empty;

        /// <summary>
        /// Gets or sets the FTP file path.
        /// </summary>
        /// <value>The FTP file path.</value>
        public string FTPFilePath
        {
            get { return _ftpFilePath; }
            set
            {
                _ftpFilePath = value;
                PropertyHasChanged(CONST_FTPFilePath);
            }
        }

        private int _serverPort = 21;

        /// <summary>
        /// Gets or sets the server port.
        /// 21 is default FTP port
        /// </summary>
        /// <value>The server port.</value>
        public int Port
        {
            get { return _serverPort; }
            set
            {
                _serverPort = value;
                PropertyHasChanged(CONST_Port);
            }
        }


        private string _userName = string.Empty;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                PropertyHasChanged(CONST_UserName);
            }
        }

        private string _password = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PropertyHasChanged(CONST_Password);
            }
        }

        private string _dirPath = string.Empty;

        /// <summary>
        /// Gets or sets the directory path.
        /// </summary>
        /// <value>The dir path.</value>
        public string DirPath
        {
            get { return _dirPath; }
            set
            {
                //if (value is System.DBNull)
                //{
                //    value = string.Empty;
                //}
                _dirPath = value;
                PropertyHasChanged(CONST_DirPath);
            }
        }

        private string _fileName = string.Empty;

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                PropertyHasChanged(CONST_FileName);
            }
        }


        private string _dataSourceXSLT = string.Empty;

        /// <summary>
        /// Gets or sets DataSource XSLT with Path.
        /// </summary>
        /// <value>XSLT Full Path.</value>
        public string DataSourceXSLT
        {
            get { return _dataSourceXSLT; }
            set
            {
                _dataSourceXSLT = value;
                PropertyHasChanged(CONST_DataSourceXSLT);
            }
        }
        string _tableFormatName = string.Empty;
        public string TableFormatName
        {
            get { return _tableFormatName; }
            set
            {
                _tableFormatName = value;
                PropertyHasChanged(CONST_TableFormatName);
            }
        }


        private int _tableTypeID = 1;
        /// <summary>        
        /// 1 is for Table Type Transaction, by default setting it to one, user will have to see, if he wants transaction
        /// or Net Postion types... Gets or sets the table type ID.
        /// </summary>
        /// <value>The data table type ID.</value>
        public int TableTypeID
        {
            get
            {
                return _tableTypeID;
            }
            set
            {
                _tableTypeID = value;
                PropertyHasChanged(CONST_TableTypeID);
                PropertyHasChanged(CONST_ThirdPartyID);
            }
        }

        private DataSourceFileLayout _fileLayoutType;

        /// <summary>
        /// Gets or sets the type of the file layout.
        /// </summary>
        /// <value>The type of the file layout.</value>
        public DataSourceFileLayout FileLayoutType
        {
            get { return _fileLayoutType; }
            set
            {
                _fileLayoutType = value;
                PropertyHasChanged(CONST_FileLayoutType);
            }
        }

        private byte[] _dataSourceXSLTBinaryData;

        [Browsable(false)]
        public byte[] DataSourceXSLTBinaryData
        {
            get { return _dataSourceXSLTBinaryData; }
            set { _dataSourceXSLTBinaryData = value; }
        }

        private DateTime _lastSaveTime;

        [Browsable(false)]
        public DateTime LastSaveTime
        {
            get { return _lastSaveTime; }
            set { _lastSaveTime = value; }
        }

        private int _dataSourceXSLTFileID;

        [Browsable(false)]
        public int DataSourceXSLTFileID
        {
            get { return _dataSourceXSLTFileID; }
            set { _dataSourceXSLTFileID = value; }
        }

        private int mappingFileType;

        [Browsable(false)]
        public int MappingFileType
        {
            get { return mappingFileType; }
            set { mappingFileType = value; }
        }



        protected override object GetIdValue()
        {
            return _companyUploadSetupID;
        }

        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        #region Validation Rules

        protected override void AddBusinessRules()
        {
            //ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_DataSourceNameIDValue, 1));
            ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_ThirdPartyID, 1));
            ValidationRules.AddRule(CommonRules.MinValue<int>, new CommonRules.MinValueRuleArgs<int>(CONST_Port, 1));
            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_FTPServer);
            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_FileName);
            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_UserName);
            ValidationRules.AddRule(new RuleHandler(CommonRules.StringRequired), CONST_Password);

            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_FTPServer, 100));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_FTPFilePath, 250));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_UserName, 100));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_Password, 50));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_DirPath, 500));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_FileName, 500));

            ////ValidationRules.AddRule(CommonRules.RegExMatch, new Csla.Validation.CommonRules.RegExRuleArgs(CONST_DirPath, regMatchDirPath));
            //ValidationRules.AddRule(CommonRules.RegExMatch, new Csla.Validation.CommonRules.RegExRuleArgs(CONST_FileName, regMatchFileName));
            //ValidationRules.AddRule(CommonRules.RegExMatch, new Csla.Validation.CommonRules.RegExRuleArgs(CONST_FTPServer, regMatchFTPServer));
            //ValidationRules.AddRule(CommonRules.RegExMatch, new Csla.Validation.CommonRules.RegExRuleArgs(CONST_UserName, regMatchUserName));

            //To Do: add more validations like checking for proper URI, User Name, Password etc !!

            //ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_SourceColumnName, 50));
            //ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_SampleValue, 100));
            //ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_Description, 200));
            //ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_Notes, 200));

            ValidationRules.AddRule(CustomClass.DataSourceRequired, CONST_ThirdPartyID);

            //ValidationRules.AddRule(CustomClass.DirPathCheck, CONST_DirPath);
            ValidationRules.AddRule(CustomClass.FileNameCheck, CONST_FileName);
            ValidationRules.AddRule(CustomClass.FTPServerCheck, CONST_FTPServer);
            ValidationRules.AddRule(CustomClass.UserNameCheck, CONST_UserName);

            ValidationRules.AddRule(CustomClass.DataSourceColumnTypeRepeatCheck, CONST_ThirdPartyID);
            ValidationRules.AddRule(CustomClass.DataSourceColumnTypeRepeatCheck, CONST_TableTypeID);
            ValidationRules.AddRule(CustomClass.DataSourceColumnTypeRepeatCheck, CONST_TableFormatName);
            ValidationRules.AddRule(CustomClass.DataSourceXSLTRequired, CONST_DataSourceXSLT);
            ValidationRules.AddRule(CustomClass.TableFormatNameRequired, CONST_TableFormatName);
            ValidationRules.AddRule(CommonRules.StringMaxLength, new Csla.Validation.CommonRules.MaxLengthRuleArgs(CONST_TableFormatName, 100));
        }

        public class CustomClass : RuleArgs
        {
            static System.Text.RegularExpressions.Regex regMatchFTPServer = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9.]*$");
            //static System.Text.RegularExpressions.Regex regMatchDirPath = new System.Text.RegularExpressions.Regex("[a-zA-Z0-9:/]");
            static System.Text.RegularExpressions.Regex regMatchFileName = new System.Text.RegularExpressions.Regex("[a-zA-Z0-9._]");
            static System.Text.RegularExpressions.Regex regMatchUserName = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9._]*$");

            public CustomClass(string validation)
                : base(validation)
            {
            }

            public static bool DataSourceRequired(object target, RuleArgs e)
            {
                SetUploadClient finalTarget = target as SetUploadClient;
                if (finalTarget != null)
                {
                    if (finalTarget.ThirdPartyID <= 0)
                    {
                        e.Description = "Data Source required";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            //public static bool DirPathCheck(object target, RuleArgs e)
            //{
            //    SetUploadClient finalTarget = target as SetUploadClient;
            //    if (finalTarget != null)
            //    {
            //        if (!regMatchDirPath.IsMatch(finalTarget.DirPath))
            //        {
            //            e.Description = "Directory path not valid";
            //            return false;
            //        }
            //        else
            //        {
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            public static bool FileNameCheck(object target, RuleArgs e)
            {
                SetUploadClient finalTarget = target as SetUploadClient;
                if (finalTarget != null)
                {
                    if (!regMatchFileName.IsMatch(finalTarget.FileName))
                    {
                        e.Description = "File name not valid";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool FTPServerCheck(object target, RuleArgs e)
            {
                SetUploadClient finalTarget = target as SetUploadClient;
                if (finalTarget != null)
                {
                    if (!regMatchFTPServer.IsMatch(finalTarget.FTPServer))
                    {
                        e.Description = "FTP Server not valid";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool UserNameCheck(object target, RuleArgs e)
            {
                SetUploadClient finalTarget = target as SetUploadClient;
                if (finalTarget != null)
                {
                    if (!regMatchUserName.IsMatch(finalTarget.UserName))
                    {
                        e.Description = "User name not valid";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool DataSourceColumnTypeRepeatCheck(object target, RuleArgs e)
            {
                SetUploadClient sourceTarget = target as SetUploadClient;
                UploadClientSetUpList uploadClientSetUpList = (UploadClientSetUpList)sourceTarget.Parent;
                UploadClientSetUpList uploadClientSetUpListClone = new UploadClientSetUpList(); ;

                if (uploadClientSetUpList != null)
                {
                    uploadClientSetUpListClone = uploadClientSetUpList.Clone();
                    int index = 0;
                    //Have to use the follwoing link as the "uploadClientSetUpListClone.Remove(sourceTarget)" was not working
                    //as the reason I think is that the indexes in the clone list gets reverse or changed from the orignal list.
                    foreach (SetUploadClient setUploadClient in uploadClientSetUpListClone)
                    {
                        if (setUploadClient.ThirdPartyID == sourceTarget.ThirdPartyID && setUploadClient.TableTypeID == sourceTarget.TableTypeID && setUploadClient.TableFormatName == sourceTarget.TableFormatName)
                        {
                            break;
                        }
                        index++;
                    }
                    uploadClientSetUpListClone.RemoveAt(index);
                }
                if (sourceTarget != null && uploadClientSetUpList != null)
                {
                    foreach (SetUploadClient setUploadClient in uploadClientSetUpListClone)
                    {
                        if (setUploadClient.ThirdPartyID == sourceTarget.ThirdPartyID && setUploadClient.TableTypeID == sourceTarget.TableTypeID && setUploadClient.TableFormatName == sourceTarget.TableFormatName)
                        {
                            e.Description = "Data Source,table type and Table Type Format Name combination has already been choosen, please select some different combination.";
                            return false;
                        }
                    }
                    Prana.PM.DAL.DataSourceManager.GetDataSourceTable(sourceTarget.ThirdPartyID, sourceTarget.TableTypeID);

                    //Commented on 10th Dec as per the new requirements.
                    //if (dataSourceTable.TableName == "" || dataSourceTable.TableName == null)
                    //{
                    //    e.Description = "The mapping for the data source columns has not been done with application columns, please map the columns first.";
                    //    return false;
                    //}

                    return true;
                }
                else
                {
                    return true;
                }
            }

            public static bool DataSourceXSLTRequired(object target, RuleArgs e)
            {
                SetUploadClient finalTarget = target as SetUploadClient;
                if (finalTarget != null)
                {
                    if (String.IsNullOrEmpty(finalTarget.DataSourceXSLT))
                    {
                        e.Description = "Data Source XSLT required";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public static bool TableFormatNameRequired(object target, RuleArgs e)
            {
                SetUploadClient finalTarget = target as SetUploadClient;
                if (finalTarget != null)
                {
                    if (String.IsNullOrEmpty(finalTarget.TableFormatName))
                    {
                        e.Description = "Table Format Name required";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }


        }

        #endregion
    }
}
