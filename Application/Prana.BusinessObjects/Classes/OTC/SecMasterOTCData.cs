using System;
using System.Runtime.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    [KnownType(typeof(SecMasterCFDData))]
    [KnownType(typeof(SecMasterEquitySwap))]
    [KnownType(typeof(SecMasterConvertibleBondData))]
    public class SecMasterOTCData
    {

        public SecMasterOTCData()
        {

        }

        public SecMasterOTCData(string value)
        {
            // TODO: Complete member initialization

        }

        /// <summary>
        /// Id
        /// </summary>
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;

            }
        }


        /// <summary>
        /// Name
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;

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
        /// UnderlyingAssetID
        /// </summary>
        private int underlyingAssetID;
        public int UnderlyingAssetID
        {
            get { return underlyingAssetID; }
            set
            {
                underlyingAssetID = value;

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
        /// CreatedBy
        /// </summary>
        private int createdBy;
        public int CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;

            }
        }

        /// <summary>
        /// CreationDate
        /// </summary>
        private DateTime creationDate;
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;

            }
        }

        /// <summary>
        /// LastModifiedBy
        /// </summary>
        private int lastModifiedBy;
        public int LastModifiedBy
        {
            get { return lastModifiedBy; }
            set
            {
                lastModifiedBy = value;

            }
        }

        /// <summary>
        /// LastModifieDate
        /// </summary>
        private DateTime lastModifieDate;
        public DateTime LastModifieDate
        {
            get { return lastModifieDate; }
            set
            {
                lastModifieDate = value;

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

        private DateTime effectiveDate;

        public DateTime EffectiveDate
        {
            get { return effectiveDate; }
            set { effectiveDate = value; }
        }

        public string View { get; set; }

        public string CustomFieldsString { get; set; }

        public string GroupID { get; set; }
    }
}
