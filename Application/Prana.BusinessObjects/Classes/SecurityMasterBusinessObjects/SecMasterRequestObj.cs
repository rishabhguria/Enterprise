using Prana.Global;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterRequestObj
    {
        public SecMasterRequestObj()
        {
            //AddNewRow();
        }

        public void AddNewRow()
        {
            _symbolDataRowCollection.Add(new SymbolDataRow());
        }

        public void AddData(string symbol, ApplicationConstants.SymbologyCodes symbology, long symbol_PK = new long())
        {
            AddNewRow();
            _symbolDataRowCollection[_symbolDataRowCollection.Count - 1].AddData(symbol, symbology, symbol_PK);

        }

        //Added by Omshiv, Jun 2014, we can add BBGID for validation  
        public void AddData(string BBGID)
        {
            AddNewRow();
            _symbolDataRowCollection[_symbolDataRowCollection.Count - 1].AddData(BBGID);

        }

        private List<SymbolDataRow> _symbolDataRowCollection = new List<SymbolDataRow>();

        private int _validatedSymbolCount = 0;

        public int ValidatedSymbolCount
        {
            get { return _validatedSymbolCount; }
            set { _validatedSymbolCount = value; }
        }

        public int Count
        {
            get { return _symbolDataRowCollection.Count; }
        }
        public List<SymbolDataRow> SymbolDataRowCollection
        {
            get { return _symbolDataRowCollection; }
        }
        public string CreateXml()
        {
            DataTable requestDataTable = new DataTable();
            string[] symbologyNames = Enum.GetNames(typeof(ApplicationConstants.SymbologyCodes));
            requestDataTable.TableName = "SecMasterRequest";
            foreach (string name in symbologyNames)
            {
                requestDataTable.Columns.Add(name);
            }

            //Modified by Omshiv, Added BBGID for request
            if (!requestDataTable.Columns.Contains("BBGID"))
                requestDataTable.Columns.Add("BBGID");

            foreach (SymbolDataRow datarow in _symbolDataRowCollection)
            {
                object[] row = new object[ApplicationConstants.SymbologyCodesCount + 1]; // BBGID added - omshiv
                int i = 0;
                bool allempty = true;
                foreach (string symbol in datarow.SymbolData)
                {
                    if (symbol != string.Empty)
                    {
                        allempty = false;
                    }
                    row[i] = symbol;
                    i++;
                }
                //add bbgid to row
                if (!String.IsNullOrWhiteSpace(datarow.BBGID))
                {
                    allempty = false;
                    row[i] = datarow.BBGID;
                }
                //row[i] = _date;
                if (!allempty)
                {
                    requestDataTable.Rows.Add(row);
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(requestDataTable);
            return ds.GetXml();
        }

        public SymbolDataRow GetSymbolRow(SecMasterBaseObj secMasterBaseObj)
        {
            foreach (SymbolDataRow datarow in _symbolDataRowCollection)
            {
                if (datarow.IsSameRequest(secMasterBaseObj))
                {
                    return datarow;
                }

                //modified by omshiv, check for BBGID requested or not
                if (!string.IsNullOrWhiteSpace(datarow.BBGID) && secMasterBaseObj.BBGID.Equals(datarow.BBGID))
                {
                    return datarow;
                }
            }
            return null;
        }

        public List<string> GetPrimarySymbols()
        {
            List<string> list = new List<string>();
            foreach (SymbolDataRow datarow in _symbolDataRowCollection)
            {
                list.Add(datarow.PrimarySymbol);
            }
            return list;
        }
        public void Remove(SymbolDataRow symbolDataRow)
        {
            _symbolDataRowCollection.Remove(symbolDataRow);
        }
        private DateTime _date = DateTime.UtcNow.Date;

        public DateTime RequestedDate
        {
            get { return _date; }
            set { _date = value; }
        }
        private int _hashCode;

        public int HashCode
        {
            get { return _hashCode; }
            set { _hashCode = value; }
        }
        private string _userID;

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        // Added by omshiv, for check security exist in system only. so if value is true then request will not sent to e-signal or BB
        private bool _IsSearchInLocalOnly = false;

        public bool IsSearchInLocalOnly
        {
            get { return _IsSearchInLocalOnly; }
            set { _IsSearchInLocalOnly = value; }
        }

        //Only validate TT request from Nirvana internal option manual validation
        private bool _useOptionManualvalidation = false;

        public bool UseOptionManualvalidation
        {
            get { return _useOptionManualvalidation; }
            set { _useOptionManualvalidation = value; }
        }

        public bool IsRequestValid()
        {
            bool result = false;


            if (Count == 0)
            {
                result = false;
            }
            else if (Count == 1)
            {
                foreach (string requestedSymbol in _symbolDataRowCollection[0].SymbolData)
                {
                    if (requestedSymbol != string.Empty)
                    {
                        result = true;
                        break;
                    }
                }
                //modified by osmhiv
                if (!string.IsNullOrWhiteSpace(_symbolDataRowCollection[0].BBGID))
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }


        public object Clone()
        {
            return Prana.Global.Utilities.DeepCopyHelper.Clone(this);
        }

        //Added to match request/ response from UI
        public string RequestID { get; set; }
    }
    [Serializable]
    public class SymbolDataRow
    {
        bool firstItem = true;
        private ApplicationConstants.SymbologyCodes _primarySymbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
        private string _underlyingSymbol = string.Empty;
        private DateTime _expirationDate = DateTime.MinValue;
        private long _symbol_PK = long.MinValue;

        public long Symbol_PK
        {
            get { return _symbol_PK; }
            set { _symbol_PK = value; }
        }

        private String _BBGID = String.Empty;
        public String BBGID
        {
            get { return _BBGID; }
            set { _BBGID = value; }
        }

        List<string> _symbolData = new List<string>();

        public SymbolDataRow()
        {
            for (int i = 0; i < ApplicationConstants.SymbologyCodesCount; i++)
            {
                _symbolData.Add(string.Empty);
            }
        }
        public void AddData(string symbol, ApplicationConstants.SymbologyCodes symbology, long symbolPK = new long())
        {
            if (firstItem)
            {
                _primarySymbology = symbology;
            }
            _symbolData[(int)symbology] = symbol;
            Symbol_PK = symbolPK;
            firstItem = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BBGId"></param>
        public void AddData(string BBGID)
        {
            _BBGID = BBGID;
        }

        public string PrimarySymbol
        {
            get { return _symbolData[(int)_primarySymbology]; }
        }

        public ApplicationConstants.SymbologyCodes PrimarySymbology
        {
            get
            {
                return _primarySymbology;
            }

            set
            {
                _primarySymbology = value;
            }

        }

        public string UnderlyingSymbol
        {
            get
            {
                return _underlyingSymbol;
            }

            set
            {
                _underlyingSymbol = value;
            }

        }

        public DateTime ExpirationDate
        {
            get
            {
                return _expirationDate;
            }

            set
            {
                _expirationDate = value;
            }

        }

        public bool IsSameRequest(SecMasterBaseObj secMasterBaseObj)
        {
            bool result = true;
            int i = 0;
            //secMasterBaseObj.TickerSymbol
            //ApplicationConstants.SymbologyCodes
            int primarySymbolNumber = (int)ApplicationConstants.PranaSymbology;
            //if(secMasterBaseObj.SymbologyMapping[primarySymbolNumber].Trim().ToUpper() ==primarySymbolNumber.
            //string primarySymbolInReq = secMasterBaseObj.SymbologyMapping[primarySymbolNumber].Trim().ToUpper();
            //string primarySymbol = _symbolData[primarySymbolNumber].Trim().ToUpper();
            int totalnonMatchCount = 0;
            //CHMW-2257 CLONE -[Import] Application error : 'These columns don't currently have unique values' on import
            if (!string.IsNullOrWhiteSpace(secMasterBaseObj.SymbologyMapping[primarySymbolNumber].Trim()) && string.Compare(secMasterBaseObj.SymbologyMapping[primarySymbolNumber].Trim(), _symbolData[primarySymbolNumber].Trim(), StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
            else
            {
                foreach (string symbol in secMasterBaseObj.SymbologyMapping)
                {
                    //string symbol1= _symbolData[i].Trim().ToUpper();
                    //string symbol2 = symbol.Trim().ToUpper();
                    if (_symbolData[i].Trim() != string.Empty)
                    {
                        if (string.Compare(_symbolData[i].Trim(), symbol.Trim(), true) != 0)
                        {
                            result = false;
                            break;
                        }
                    }
                    else
                    {
                        totalnonMatchCount++;
                    }
                    i++;
                }
            }
            if (totalnonMatchCount == _symbolData.Count)
            {
                result = false;
            }
            return result;
        }
        public List<string> SymbolData
        {
            get
            {
                return _symbolData;
            }
        }
    }
}
