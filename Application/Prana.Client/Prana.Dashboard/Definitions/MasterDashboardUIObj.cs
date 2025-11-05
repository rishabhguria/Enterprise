using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.Dashboard
{
    class MasterDashboardUIObj
    {
        public int AccountID { get; set; }

        public String AccountName { get; set; }

        public String ThirdPartyName { get; set; }

        public String CompanyName { get; set; }

        private NirvanaTaskStatus _fileUploadStatus = NirvanaTaskStatus.Pending;
        public NirvanaTaskStatus FileUploadStatus
        {
            get
            {
                return _fileUploadStatus;
            }
            set
            {
                _fileUploadStatus = value;
            }
        }

        private NirvanaTaskStatus _importStatus = NirvanaTaskStatus.Pending;
        public NirvanaTaskStatus ImportStatus
        {
            get
            {
                return _importStatus;
            }
            set
            {
                _importStatus = value;
            }
        }

        private NirvanaTaskStatus _reconStatus = NirvanaTaskStatus.Pending;
        public NirvanaTaskStatus ReconStatus
        {
            get
            {
                return _reconStatus;
            }
            set
            {
                _reconStatus = value;
            }
        }
        private NirvanaTaskStatus _sMValidationStatus = NirvanaTaskStatus.Pending;
        public NirvanaTaskStatus SMValidationStatus
        {
            get
            {
                return _sMValidationStatus;
            }
            set
            {
                _sMValidationStatus = value;
            }
        }

        private NirvanaTaskStatus _cnclAmndStatus = NirvanaTaskStatus.Pending;
        public NirvanaTaskStatus CnclAmndStatus
        {
            get
            {
                return _cnclAmndStatus;
            }
            set
            {
                _cnclAmndStatus = value;
            }
        }

        private NirvanaTaskStatus _closingStatus = NirvanaTaskStatus.Pending;
        public NirvanaTaskStatus ClosingStatus
        {
            get
            {
                return _closingStatus;
            }
            set
            {
                _closingStatus = value;
            }
        }

        private NirvanaTaskStatus _markPriceStatus = NirvanaTaskStatus.Pending;
        public NirvanaTaskStatus MarkPriceStatus
        {
            get
            {
                return _markPriceStatus;
            }
            set
            {
                _markPriceStatus = value;
            }
        }

        private NirvanaTaskStatus _reportingStatus = NirvanaTaskStatus.Pending;
        public NirvanaTaskStatus ReportingStatus
        {
            get
            {
                return _reportingStatus;
            }
            set
            {
                _reportingStatus = value;
            }
        }

        private NirvanaTaskStatus _ImportIntoAppStatus = NirvanaTaskStatus.Pending;
        public NirvanaTaskStatus ImportIntoAppStatus
        {
            get
            {
                return _ImportIntoAppStatus;
            }
            set
            {
                _ImportIntoAppStatus = value;
            }
        }


        public DateTime EventTime { get; set; }

        //internal static masterdashboarduiobj getworkflowitem(system.data.datarow dr)
        //{

        //    masterdashboarduiobj uiobject = new masterdashboarduiobj();
        //    try
        //    {
        //        uiobject.accountid = convert.toint32(dr["companyaccountid"]);
        //        uiobject.executiondate = datetime.parse(dr["date"].tostring());
        //        uiobject.accountname = dr["accountname"].tostring();
        //        uiobject.companyname = dr["companyname"].tostring();
        //        uiobject.thirdpartyname = dr["thirdpartyname"].tostring();
        //        uiobject.eventtime = datetime.parse(dr["taskruntime"].tostring()); 

        //        int eventid = convert.toint32(dr["taskid"]);
        //        int stateid = convert.toint32(dr["stateid"]);

        //        uiobject.updatestatus(eventid, stateid);


        //    }
        //    catch (exception ex)
        //    {
        //        // invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionLogger.HandleException(ex,LoggingConstants.policy_logandthrow);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return uiobject;
        //}

        internal void UpdateStatus(int eventID, int stateID)
        {
            NirvanaTaskStatus taskStatus = (NirvanaTaskStatus)Enum.ToObject(typeof(NirvanaTaskStatus), stateID);
            NirvanaWorkFlows workFlow = (NirvanaWorkFlows)Enum.ToObject(typeof(NirvanaWorkFlows), eventID);
            switch (workFlow) //TDOD create enum for Events
            {
                case NirvanaWorkFlows.FileUpload: //Import
                    this.FileUploadStatus = taskStatus;
                    break;
                case NirvanaWorkFlows.Import: //Import
                    this.ImportStatus = taskStatus;
                    break;
                case NirvanaWorkFlows.SMValidation: //SM Validation
                    this.SMValidationStatus = taskStatus;
                    break;
                case NirvanaWorkFlows.Recon: //Recon
                    this.ReconStatus = taskStatus;
                    break;

                case NirvanaWorkFlows.MarkPrice: //Mark Price Status
                    this.MarkPriceStatus = taskStatus;
                    break;

                case NirvanaWorkFlows.CnclAmnd: //CnclAmndStatus
                    this.CnclAmndStatus = taskStatus;
                    break;
                case NirvanaWorkFlows.Closing: //Closing Status
                    this.ClosingStatus = taskStatus;
                    break;

                case NirvanaWorkFlows.Reporting: //Reporting Status
                    this.ReportingStatus = taskStatus;
                    break;
                case NirvanaWorkFlows.ImportIntoAPP: //import into APP
                    this.ImportIntoAppStatus = taskStatus;
                    break;

                default:

                    break;

            }
        }


        public DateTime ExecutionDate { get; set; }
    }
}
