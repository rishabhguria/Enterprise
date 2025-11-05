using System;
using System.Runtime.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    [KnownType(typeof(EquitySwapTradeData))]
    [KnownType(typeof(CFDTradeData))]
    [KnownType(typeof(ConvertibleBondTradeData))]
    public class OTCTradeData
    {

        public static string GetInstrumentType(string swapString)
        {
            try
            {
                string[] externList = swapString.Split(Seperators.SEPERATOR_5);
                var instrumentType = externList[0].Split(Seperators.SEPERATOR_6).GetValue(1).ToString();
                return instrumentType;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;

            }
        }


        private string _uniqueIdentifier;
        public string UniqueIdentifier
        {
            get { return _uniqueIdentifier; }
            set
            {
                _uniqueIdentifier = value;

            }
        }

        /// <summary>
        /// InstrumentType
        /// </summary>
        private string instrumentType;
        public string InstrumentType
        {
            get { return instrumentType; }
            set
            {
                instrumentType = value;

            }
        }

        /// <summary>
        /// InstrumentType
        /// </summary>
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;

            }
        }


        /// <summary>
        /// ISDACounterParty
        /// </summary>
        private int iSDACounterParty;
        public int ISDACounterParty
        {
            get { return iSDACounterParty; }
            set
            {
                iSDACounterParty = value;

            }
        }

        /// <summary>
        /// ISDAContract
        /// </summary>
        private string iSDAContract;
        public string ISDAContract
        {
            get { return iSDAContract; }
            set
            {
                iSDAContract = value;

            }
        }

        private int daysToSettle;
        public int DaysToSettle
        {
            get { return daysToSettle; }
            set { daysToSettle = value; }
        }

        private DateTime tradeDate;
        public DateTime TradeDate
        {
            get { return tradeDate; }
            set { tradeDate = value; }
        }

        private DateTime effectiveDate;
        public DateTime EffectiveDate
        {
            get { return effectiveDate; }
            set { effectiveDate = value; }
        }


        private string groupID;

        public string GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }


        //private string _sedol = string.Empty;
        //public string Sedol
        //{
        //    get { return _sedol; }
        //    set
        //    {
        //        _sedol = value;

        public virtual OTCTradeData Clone()
        {
            throw new NotImplementedException();
        }
    }
}
