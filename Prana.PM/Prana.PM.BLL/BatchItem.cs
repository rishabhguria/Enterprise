using System;
using System.Collections.Generic;

namespace Prana.PM.BLL
{
    public class BatchItem
    {
        int batchID;
        public List<object> accountID;
        string formatName;
        int thirdPartyID;
        string thirdPartyType;
        decimal priceCheckTolerance;
        int schedule;
        string cronExpression;
        string execTime;
        DateTime nxtExecTime;
        DateTime lastExecTime;
        int lastScanResult;
        bool autoExec;
        bool enablePriceTolerance;

        /// <summary>
        /// Enum for scan results
        /// </summary>
        enum ExecResult
        {
            NONE = 0,
            PASS = 1,
            FAIL = 2
        }

        /// <summary>
        /// Defualt Constructor
        /// </summary>
        public BatchItem()
        {
            BatchID = 0;
            FormatName = string.Empty;
            accountID = new List<object>();
            PriceCheckTolerance = 0;
            CronExpression = string.Empty;
            Schedule = 0;
            AutoExec = false;
            execTime = string.Empty;
            NxtExecTime = DateTime.Now;
            LastExecTime = DateTime.Now;
            LastScanResult = (int)ExecResult.NONE;
        }

        public string ThirdPartyType
        {
            get { return thirdPartyType; }
            set { thirdPartyType = value; }
        }

        public int BatchID
        {
            get
            { return batchID; }
            set
            { batchID = value; }
        }

        public string CronExpression
        {
            get { return cronExpression; }
            set { cronExpression = value; }
        }

        public string FormatName
        {
            get
            { return formatName; }
            set
            { formatName = value; }
        }
        public decimal PriceCheckTolerance
        {
            get
            { return priceCheckTolerance; }
            set
            { priceCheckTolerance = value; }
        }

        public int Schedule
        {
            get
            { return schedule; }
            set
            { schedule = value; }
        }

        public string ExecTime
        {
            get
            { return execTime; }
            set
            { execTime = value; }
        }

        public int ThirdPartyID
        {
            get { return thirdPartyID; }
            set { thirdPartyID = value; }
        }

        public bool AutoExec
        {
            get
            { return autoExec; }
            set
            { autoExec = value; }
        }

        public DateTime NxtExecTime
        {
            get
            { return nxtExecTime; }
            set
            { nxtExecTime = value; }
        }

        public DateTime LastExecTime
        {
            get { return lastExecTime; }
            set { lastExecTime = value; }
        }

        public int LastScanResult
        {
            get { return lastScanResult; }
            set { lastScanResult = value; }
        }

        public bool EnablePriceTolerance
        {
            get { return enablePriceTolerance; }
            set { enablePriceTolerance = value; }
        }
    }
}
