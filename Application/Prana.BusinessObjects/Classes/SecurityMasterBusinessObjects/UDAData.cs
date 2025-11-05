using Prana.Global;
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    // modified by: omshiv, Added UDA ids fields
    [Serializable]
    public class UDAData
    {
        private string _UDAAsset = ApplicationConstants.CONST_UNDEFINED;

        public string UDAAsset
        {
            get { return _UDAAsset; }
            set { _UDAAsset = value; }
        }

        private string _UDASecurityType = ApplicationConstants.CONST_UNDEFINED;

        public string UDASecurityType
        {
            get { return _UDASecurityType; }
            set { _UDASecurityType = value; }
        }

        private string _UDASector = ApplicationConstants.CONST_UNDEFINED;

        public string UDASector
        {
            get { return _UDASector; }
            set { _UDASector = value; }
        }

        private string _UDASubSector = ApplicationConstants.CONST_UNDEFINED;

        public string UDASubSector
        {
            get { return _UDASubSector; }
            set { _UDASubSector = value; }
        }

        private string _UDACountry = ApplicationConstants.CONST_UNDEFINED;

        public string UDACountry
        {
            get { return _UDACountry; }
            set { _UDACountry = value; }
        }
        #region UDA symbol data
        private string _symbol = string.Empty;
        private string _companyName = String.Empty;
        private int _assetID = int.MinValue;
        private int _securityTypeID = int.MinValue;
        private int _subSectorID = int.MinValue;
        private int _sectorID = int.MinValue;
        private int _CountryID = int.MinValue;
        private string _underlyingSymbol = String.Empty;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }
        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }
        public int SecurityTypeID
        {
            get { return _securityTypeID; }
            set { _securityTypeID = value; }
        }
        public int SectorID
        {
            get { return _sectorID; }
            set { _sectorID = value; }
        }
        public int SubSectorID
        {
            get { return _subSectorID; }
            set { _subSectorID = value; }
        }
        public int CountryID
        {
            get { return _CountryID; }
            set { _CountryID = value; }
        }
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }



        ///// <summary>
        ///// fill table row in object
        ///// </summary>
        ///// <param name="row"></param>
        //public void FillSymbolUDAData(object[] row)
        //{

        //    _symbol = row[0].ToString();
        //    if (row[1] != System.DBNull.Value)
        //    {
        //        _assetID = Int32.Parse(row[1].ToString());
        //    }
        //    if (row[2] != System.DBNull.Value)
        //    {
        //        _securityTypeID = Int32.Parse(row[2].ToString());
        //    }
        //    if (row[3] != System.DBNull.Value)
        //    {
        //        _sectorID = Int32.Parse(row[3].ToString());
        //    }
        //    if (row[4] != System.DBNull.Value)
        //    {
        //        _subSectorID = Int32.Parse(row[4].ToString());
        //    }
        //    if (row[5] != System.DBNull.Value)
        //    {
        //        _CountryID = Int32.Parse(row[5].ToString());
        //    }
        //    if (row[6] != System.DBNull.Value)
        //    {
        //        _companyName = row[6].ToString();
        //    }
        //    if (row[7] != System.DBNull.Value)
        //    {
        //        _underlyingSymbol = row[7].ToString();
        //    }

        //}

        #endregion


        public void FillSymbolUDAData(System.Data.DataRow row)
        {

            try
            {
                if (row.Table.Columns.Contains("UDAAssetClassID") && row.Table.Columns.Contains("UDAAssetClass"))
                {
                    this.AssetID = row["UDAAssetClassID"] != DBNull.Value ? Convert.ToInt32(row["UDAAssetClassID"].ToString()) : int.MinValue;
                    this.UDAAsset = row["UDAAssetClass"] != DBNull.Value ? row["UDAAssetClass"].ToString() : string.Empty;
                }
                if (row.Table.Columns.Contains("UDASecurityTypeID") && row.Table.Columns.Contains("UDASecurityType"))
                {
                    this.SecurityTypeID = row["UDASecurityTypeID"] != DBNull.Value ? Convert.ToInt32(row["UDASecurityTypeID"].ToString()) : int.MinValue;
                    this.UDASecurityType = row["UDASecurityType"] != DBNull.Value ? row["UDASecurityType"].ToString() : string.Empty;
                }
                if (row.Table.Columns.Contains("UDASectorID") && row.Table.Columns.Contains("UDASector"))
                {
                    this.SectorID = row["UDASectorID"] != DBNull.Value ? Convert.ToInt32(row["UDASectorID"].ToString()) : int.MinValue;
                    this.UDASector = row["UDASector"] != DBNull.Value ? row["UDASector"].ToString() : string.Empty;
                }
                if (row.Table.Columns.Contains("UDASubSectorID") && row.Table.Columns.Contains("UDASubSector"))
                {
                    this.SubSectorID = row["UDASubSectorID"] != DBNull.Value ? Convert.ToInt32(row["UDASubSectorID"].ToString()) : int.MinValue;
                    this.UDASubSector = row["UDASubSector"] != DBNull.Value ? row["UDASubSector"].ToString() : string.Empty;
                }
                if (row.Table.Columns.Contains("UDACountryID") && row.Table.Columns.Contains("UDACountry"))
                {
                    this.CountryID = row["UDACountryID"] != DBNull.Value ? Convert.ToInt32(row["UDACountryID"].ToString()) : int.MinValue;
                    this.UDACountry = row["UDACountry"] != DBNull.Value ? row["UDACountry"].ToString() : string.Empty;
                }
                if (row.Table.Columns.Contains("PrimarySymbol"))
                {
                    this.Symbol = row["PrimarySymbol"] != DBNull.Value ? row["PrimarySymbol"].ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public int AccountID { get; set; }
    }
}
