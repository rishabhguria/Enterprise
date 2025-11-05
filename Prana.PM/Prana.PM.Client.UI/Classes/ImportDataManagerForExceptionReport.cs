using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.PM.BLL;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.XMLUtilities;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using Prana.BusinessObjects.AppConstants;
using System.IO;
using Prana.CommonDataCache;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;

namespace Prana.PM.Client.UI
{
    public class ImportDataManagerForExceptionReport
    {

        #region Public Properties

        static Dictionary<string, string> _reconDataSourceXSLT = new Dictionary<string, string>();

        public static Dictionary<string, string> ReconDataSourceXSLT
        {
            get { return _reconDataSourceXSLT; }
            set { _reconDataSourceXSLT = value; }
        }

        static Dictionary<string, List<string> > _dictFromatType = new Dictionary<string, List<string> >();

        public static Dictionary<string, List<string>> DictReconType
        {
            get { return _dictFromatType;  }
            set { _dictFromatType = value; }
        }

        #endregion Public Properties

        #region Select File

        public static List<ReconPosition> GetConvertPositionReconCollectionFromFile(string xSLTName)
        {
            List<ReconPosition> reconPositionColl = null;
            try
            {
                string ReceivedFile = ReceiveFile();
                // if user cancel the operation or received file is null or empty
                if (String.IsNullOrEmpty(ReceivedFile))
                {
                    return reconPositionColl = new List<ReconPosition>();
                }
                DataTable result = null;
                // get the Table for passing .CSV or .xls file
                result = GetDataTableFromDifferentFileFormats(ReceivedFile);
                if (result != null)
                {
                    DataTable resultModified = ArrangeTable(result, "ReconPosition");

                    if (resultModified != null)
                    {
                        // get File path after XSLT Transformation
                        string mappedfilePath = GetFilePathAfterXSLTTransformation(resultModified, xSLTName);
                        bool isXmlValidated = ValidateConvertedXML(mappedfilePath);
                        if (!isXmlValidated)
                        {
                            return null ;
                        }
                        string getMappedXML = string.Empty;
                        DataSet ds = new DataSet();
                        if (!mappedfilePath.Equals(""))
                        {

                            ds.ReadXml(mappedfilePath);
                            // Now we have arranged and updated XML
                            // as above we inserted "*" in the blank columns, but "*" needs extra treatment, so
                            // again we replace the "*" with blank string, the following looping does the same
                            for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                            {
                                for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                                {
                                    string val = ds.Tables[0].Rows[irow].ItemArray[icol].ToString();
                                    if (val.Equals("*"))
                                    {
                                        ds.Tables[0].Rows[irow][icol] = string.Empty;
                                    }
                                }
                            }
                            reconPositionColl = GetReconPositionColl(ds);
                        }

                        reconPositionColl = GetPranaSymbolByDifferentSymbology(reconPositionColl);
                        SortPositionCollection(reconPositionColl);
                        reconPositionColl = GroupPositionCollection(reconPositionColl);
                        SortPositionCollectionFinally(reconPositionColl);

                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return reconPositionColl;
        }

        private static bool ValidateConvertedXML(string convertedXMLPath)
        {
            bool isValidated = false;
            try
            {
                string xsdPath = Application.StartupPath + @"\XSD\Recon.xsd";
                string tmpError;
                isValidated = XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "",out tmpError);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isValidated;
        }

        #region Group Positions by Symbol,Side and Fund

        private static List<ReconPosition> GroupPositionCollection(List<ReconPosition> reconPositionColl)
        {
            double sumofQty = 0;
            double sumofAvgPrice = 0;
            double wtdAvgPrice = 0;
            string preSymbol = string.Empty;
            string preOrderSide = string.Empty;
            string preFundName = string.Empty;

            List<ReconPosition> GroupedColl = new List<ReconPosition>();

            Dictionary<string, ReconPosition> symbolwiseDict = new Dictionary<string, ReconPosition>();

            string key = string.Empty;

            foreach (ReconPosition reconPosition in reconPositionColl)
            {
                if (reconPosition.Symbol.Equals(preSymbol))
                {
                    if (reconPosition.OrderSideTagValue.Equals(preOrderSide))
                    {
                        if (reconPosition.FundName.Equals(preFundName))
                        {
                            if (!string.IsNullOrEmpty(key))
                            {
                                if (symbolwiseDict.ContainsKey(key))
                                {
                                    ReconPosition recPos = symbolwiseDict[key];
                                    sumofQty = reconPosition.Quantity + recPos.Quantity;
                                    sumofAvgPrice = reconPosition.AvgPX * reconPosition.Quantity + recPos.AvgPX * recPos.Quantity;
                                    if (sumofQty > 0)
                                    {
                                        wtdAvgPrice = sumofAvgPrice / sumofQty;
                                    }
                                    recPos.Quantity = sumofQty;
                                    recPos.AvgPX = Math.Round(wtdAvgPrice, 2);
                                }
                            }
                            else
                            {
                                preSymbol = reconPosition.Symbol;
                                preOrderSide = reconPosition.OrderSideTagValue;
                                preFundName = reconPosition.FundName;
                                if (!string.IsNullOrEmpty(preSymbol))
                                {
                                    key = preSymbol + "-" + preOrderSide + "-" + preFundName;

                                    if (!symbolwiseDict.ContainsKey(key))
                                    {
                                        symbolwiseDict.Add(key, reconPosition);
                                    }
                                }
                                else
                                {
                                    GroupedColl.Add(reconPosition);

                                }
                            }

                        }
                        else
                        {
                            preSymbol = reconPosition.Symbol;
                            preOrderSide = reconPosition.OrderSideTagValue;
                            preFundName = reconPosition.FundName;
                            if (!string.IsNullOrEmpty(preSymbol))
                            {
                                key = preSymbol + "-" + preOrderSide + "-" + preFundName;

                                if (!symbolwiseDict.ContainsKey(key))
                                {
                                    symbolwiseDict.Add(key, reconPosition);
                                }
                            }
                            else
                            {
                                GroupedColl.Add(reconPosition);

                            }
                        }
                    }
                    else
                    {
                        preSymbol = reconPosition.Symbol;
                        preOrderSide = reconPosition.OrderSideTagValue;
                        preFundName = reconPosition.FundName;
                        if (!string.IsNullOrEmpty(preSymbol))
                        {
                            key = preSymbol + "-" + preOrderSide + "-" + preFundName;

                            if (!symbolwiseDict.ContainsKey(key))
                            {
                                symbolwiseDict.Add(key, reconPosition);
                            }
                        }
                        else
                        {
                            GroupedColl.Add(reconPosition);

                        }
                    }
                }

                else
                {
                    preSymbol = reconPosition.Symbol;
                    preOrderSide = reconPosition.OrderSideTagValue;
                    preFundName = reconPosition.FundName;
                    if (!string.IsNullOrEmpty(preSymbol))
                    {
                        key = preSymbol + "-" + preOrderSide + "-" + preFundName;

                        if (!symbolwiseDict.ContainsKey(key))
                        {
                            symbolwiseDict.Add(key, reconPosition);
                        }
                    }
                    else
                    {
                        GroupedColl.Add(reconPosition);

                    }
                }

            }

            foreach (KeyValuePair<string, ReconPosition> kvp in symbolwiseDict)
            {
                if (!string.IsNullOrEmpty(kvp.Key.ToString()))
                {
                    ReconPosition reconPos = symbolwiseDict[kvp.Key];
                    GroupedColl.Add(reconPos);
                }

            }

            return GroupedColl;
        }

        #endregion Group Positions by Symbol,Side and Fund

        #region Sort Positions

        private static void SortPositionCollection(List<ReconPosition> reconPositionColl)
        {
            Comparison<ReconPosition> positionComparisonHandler = new Comparison<ReconPosition>(CompareOrders);
            reconPositionColl.Sort(positionComparisonHandler);
        }

        private static int CompareOrders(ReconPosition firstPosition, ReconPosition secondPosition)
        {
            int intComparisionresult = firstPosition.Symbol.CompareTo(secondPosition.Symbol);
            if (intComparisionresult == 0)
            {
                intComparisionresult = firstPosition.OrderSideTagValue.CompareTo(secondPosition.OrderSideTagValue);
                if (intComparisionresult == 0)
                {
                    intComparisionresult = firstPosition.FundName.CompareTo(secondPosition.FundName);

                    if (intComparisionresult == 0)
                    {
                        intComparisionresult = firstPosition.Quantity.CompareTo(secondPosition.Quantity);

                    }
                }

            }

            return intComparisionresult;
        }

        private static void SortPositionCollectionFinally(List<ReconPosition> reconPositionColl)
        {
            Comparison<ReconPosition> positionComparisonHandler = new Comparison<ReconPosition>(CompareOrdersFinally);
            reconPositionColl.Sort(positionComparisonHandler);
        }

        private static int CompareOrdersFinally(ReconPosition firstPosition, ReconPosition secondPosition)
        {
            int intComparisionresult = firstPosition.Symbol.CompareTo(secondPosition.Symbol);
            if (intComparisionresult == 0)
            {
                intComparisionresult = firstPosition.OrderSideTagValue.CompareTo(secondPosition.OrderSideTagValue);
                if (intComparisionresult == 0)
                {
                    intComparisionresult = firstPosition.Quantity.CompareTo(secondPosition.Quantity);

                    if (intComparisionresult == 0)
                    {
                        intComparisionresult = firstPosition.FundName.CompareTo(secondPosition.FundName);
                    }
                }

            }

            return intComparisionresult;
        }

        #endregion
        //const int _hashCode = 3367;
        private static List<ReconPosition> GetPranaSymbolByDifferentSymbology(List<ReconPosition> reconPositionColl)
        {
            try
            {
                if (reconPositionColl.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (ReconPosition reconPosition in reconPositionColl)
                    {
                        if (String.IsNullOrEmpty(reconPosition.Symbol))
                        {

                            if (!string.IsNullOrEmpty(reconPosition.ISIN))
                            {
                                secMasterRequestObj.AddData(reconPosition.ISIN.Trim(), ApplicationConstants.SymbologyCodes.ISINSymbol);
                            }
                            if (!string.IsNullOrEmpty(reconPosition.CUSIP))
                            {
                                secMasterRequestObj.AddData(reconPosition.CUSIP.Trim(), ApplicationConstants.SymbologyCodes.CUSIPSymbol);
                            }
                            if (!string.IsNullOrEmpty(reconPosition.SEDOL))
                            {
                                secMasterRequestObj.AddData(reconPosition.SEDOL.Trim(), ApplicationConstants.SymbologyCodes.SEDOLSymbol);
                            }
                            if (!string.IsNullOrEmpty(reconPosition.RIC))
                            {
                                secMasterRequestObj.AddData(reconPosition.RIC.Trim(), ApplicationConstants.SymbologyCodes.ReutersSymbol);
                            }
                            if (!string.IsNullOrEmpty(reconPosition.Bloomberg))
                            {
                                secMasterRequestObj.AddData(reconPosition.Bloomberg.Trim(), ApplicationConstants.SymbologyCodes.BloombergSymbol);
                            }
                            if (!string.IsNullOrEmpty(reconPosition.OSIOptionSymbol))
                            {
                                secMasterRequestObj.AddData(reconPosition.OSIOptionSymbol.Trim(), ApplicationConstants.SymbologyCodes.OSIOptionSymbol);
                            }
                            if (!string.IsNullOrEmpty(reconPosition.IDCOOptionSymbol))
                            {
                                secMasterRequestObj.AddData(reconPosition.IDCOOptionSymbol.Trim(), ApplicationConstants.SymbologyCodes.IDCOOptionSymbol);
                            }

                           // secMasterRequestObj.AddNewRow();
                        }
                    }

                    //List<SecMasterBaseObj> secMasterCollection = Prana.SecurityMasterNew.SecMasterCacheManager.GetInstance.GetSecMasterDataForList(secMasterRequestObj, _hashCode);

                    //if (secMasterCollection != null && secMasterCollection.Count > 0)
                    //{
                    //    foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                    //    {
                    //        string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                    //        string iSINSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                    //        string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                    //        string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                    //        string ricSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                    //        string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();

                    //        foreach (ReconPosition reconPosition in reconPositionColl)
                    //        {
                    //            if (string.IsNullOrEmpty(reconPosition.Symbol))
                    //            {
                    //                if ( (!string.IsNullOrEmpty(iSINSymbol) && reconPosition.ISIN.ToUpper().Trim().Equals(iSINSymbol.ToUpper())) ||
                    //                     (!string.IsNullOrEmpty(cuspiSymbol) && reconPosition.CUSIP.ToUpper().Trim().Equals(cuspiSymbol.ToUpper())) ||
                    //                     (!string.IsNullOrEmpty(sedolSymbol) && reconPosition.SEDOL.ToUpper().Trim().Equals(sedolSymbol.ToUpper())) ||
                    //                     (!string.IsNullOrEmpty(ricSymbol) && reconPosition.RIC.ToUpper().Trim().Equals(ricSymbol.ToUpper())) ||
                    //                     (!string.IsNullOrEmpty(bloombergSymbol) && reconPosition.Bloomberg.ToUpper().Trim().Equals(bloombergSymbol.ToUpper()))
                    //                    )
                    //                {
                    //                    reconPosition.Symbol = pranaSymbol;
                    //                }
                    //            }
                    //        }

                    //        //string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();

                    //        //foreach (ReconPosition reconPosition in reconPositionColl)
                    //        //{
                    //        //    if (string.IsNullOrEmpty(reconPosition.Symbol))
                    //        //    {
                    //        //        if (reconPosition.CUSIP.Equals(cuspiSymbol))
                    //        //        {
                    //        //            reconPosition.Symbol = pranaSymbol;
                    //        //        }
                    //        //    }
                    //        //}

                    //        //string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();

                    //        //foreach (ReconPosition reconPosition in reconPositionColl)
                    //        //{
                    //        //    if (string.IsNullOrEmpty(reconPosition.Symbol))
                    //        //    {
                    //        //        if (reconPosition.SEDOL.Equals(sedolSymbol))
                    //        //        {
                    //        //            reconPosition.Symbol = pranaSymbol;
                    //        //        }
                    //        //    }
                    //        //}

                    //        //string ricSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();

                    //        //foreach (ReconPosition reconPosition in reconPositionColl)
                    //        //{
                    //        //    if (string.IsNullOrEmpty(reconPosition.Symbol))
                    //        //    {
                    //        //        if (reconPosition.RIC.Equals(ricSymbol))
                    //        //        {
                    //        //            reconPosition.Symbol = pranaSymbol;
                    //        //        }
                    //        //    }
                    //        //}

                    //        //string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();

                    //        //foreach (ReconPosition reconPosition in reconPositionColl)
                    //        //{
                    //        //    if (string.IsNullOrEmpty(reconPosition.Symbol))
                    //        //    {
                    //        //        if (reconPosition.Bloomberg.Equals(bloombergSymbol))
                    //        //        {
                    //        //            reconPosition.Symbol = pranaSymbol;
                    //        //        }
                    //        //    }
                    //        //}
                    //    }
                    }

                    //// get collection after getting prana symbols from ISIN
                    //reconPositionColl = GetPranaSymbolFromISIN(reconPositionColl);

                    //// get collection after getting prana symbols from CUSIP
                    //reconPositionColl = GetPranaSymbolFromCUSIP(reconPositionColl);

                    //// get collection after getting prana symbols from SEDOL
                    //reconPositionColl = GetPranaSymbolFromSEDOL(reconPositionColl);

                    //// get collection after getting prana symbols from RIC
                    //reconPositionColl = GetPranaSymbolFromRIC(reconPositionColl);
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return reconPositionColl;
        }


        private static List<ReconPosition> GetPranaSymbolFromISIN(List<ReconPosition> reconPositionColl)
        {
            try
            {

                List<string> iSINSymbolList = new List<string>();

                // collect ISIN to get the Prana Symbol from Security Master
                foreach (ReconPosition reconPosition in reconPositionColl)
                {
                    if (String.IsNullOrEmpty(reconPosition.Symbol))
                    {
                        if (!String.IsNullOrEmpty(reconPosition.ISIN))
                        {
                            if (!iSINSymbolList.Contains(reconPosition.ISIN))
                            {
                                iSINSymbolList.Add(reconPosition.ISIN);
                            }
                        }
                    }
                }

                if (iSINSymbolList.Count > 0)
                {
                    //List<SecMasterBaseObj> secMasterCollection = Prana.SecurityMasterNew.SecMasterCacheManager.GetInstance.GetSecMasterDataForList(iSINSymbolList, ApplicationConstants.SymbologyCodes.ISINSymbol, _hashCode);

                    //if (secMasterCollection != null && secMasterCollection.Count > 0)
                    //{
                    //    foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                    //    {
                    //        string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                    //        string iSINSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();

                    //        foreach (ReconPosition reconPosition in reconPositionColl)
                    //        {
                    //            if (reconPosition.ISIN.Equals(iSINSymbol))
                    //            {
                    //                reconPosition.Symbol = pranaSymbol;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return reconPositionColl;
        }

        private static List<ReconPosition> GetPranaSymbolFromCUSIP(List<ReconPosition> reconPositionColl)
        {
            try
            {
                List<string> cUSIPSymbolList = new List<string>();

                // collect ISIN to get the Prana Symbol from Security Master
                foreach (ReconPosition reconPosition in reconPositionColl)
                {
                    if (String.IsNullOrEmpty(reconPosition.Symbol))
                    {
                        if (!String.IsNullOrEmpty(reconPosition.CUSIP))
                        {
                            if (!cUSIPSymbolList.Contains(reconPosition.CUSIP))
                            {
                                cUSIPSymbolList.Add(reconPosition.CUSIP);
                            }
                        }
                    }
                }

                if (cUSIPSymbolList.Count > 0)
                {
                    //List<SecMasterBaseObj> secMasterCollection = Prana.SecurityMasterNew.SecMasterCacheManager.GetInstance.GetSecMasterDataForList(cUSIPSymbolList, ApplicationConstants.SymbologyCodes.CUSIPSymbol, _hashCode);

                    //if (secMasterCollection != null && secMasterCollection.Count > 0)
                    //{
                    //    foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                    //    {
                    //        string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                    //        string cUSIPSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();

                    //        foreach (ReconPosition reconPosition in reconPositionColl)
                    //        {
                    //            if (reconPosition.CUSIP.Equals(cUSIPSymbol))
                    //            {
                    //                reconPosition.Symbol = pranaSymbol;

                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return reconPositionColl;
        }

        private static List<ReconPosition> GetPranaSymbolFromSEDOL(List<ReconPosition> reconPositionColl)
        {
            try
            {
                List<string> sEDOLSymbolList = new List<string>();

                // collect SEDOL to get the Prana Symbol from Security Master
                foreach (ReconPosition reconPosition in reconPositionColl)
                {
                    if (String.IsNullOrEmpty(reconPosition.Symbol))
                    {
                        if (!String.IsNullOrEmpty(reconPosition.SEDOL))
                        {
                            if (!sEDOLSymbolList.Contains(reconPosition.SEDOL))
                            {
                                sEDOLSymbolList.Add(reconPosition.SEDOL);
                            }
                        }
                    }
                }

                if (sEDOLSymbolList.Count > 0)
                {
                    //List<SecMasterBaseObj> secMasterCollection = Prana.SecurityMasterNew.SecMasterCacheManager.GetInstance.GetSecMasterDataForList(sEDOLSymbolList, ApplicationConstants.SymbologyCodes.SEDOLSymbol, _hashCode);

                    //if (secMasterCollection != null && secMasterCollection.Count > 0)
                    //{
                    //    foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                    //    {
                    //        string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                    //        string sEDOLSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();

                    //        foreach (ReconPosition reconPosition in reconPositionColl)
                    //        {
                    //            if (reconPosition.SEDOL.Equals(sEDOLSymbol))
                    //            {
                    //                reconPosition.Symbol = pranaSymbol;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return reconPositionColl;
        }

        private static List<ReconPosition> GetPranaSymbolFromRIC(List<ReconPosition> reconPositionColl)
        {
            try
            {
                List<string> rICSymbolList = new List<string>();

                // collect RIC to get the Prana Symbol from Security Master
                foreach (ReconPosition reconPosition in reconPositionColl)
                {
                    if (String.IsNullOrEmpty(reconPosition.Symbol))
                    {
                        if (!String.IsNullOrEmpty(reconPosition.RIC))
                        {
                            if (!rICSymbolList.Contains(reconPosition.RIC))
                            {
                                rICSymbolList.Add(reconPosition.RIC);
                            }
                        }
                    }
                }

                if (rICSymbolList.Count > 0)
                {
                    //List<SecMasterBaseObj> secMasterCollection = Prana.SecurityMasterNew.SecMasterCacheManager.GetInstance.GetSecMasterDataForList(rICSymbolList, ApplicationConstants.SymbologyCodes.ReutersSymbol, _hashCode);

                    //if (secMasterCollection != null && secMasterCollection.Count > 0)
                    //{
                    //    foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                    //    {
                    //        string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                    //        string rICSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();

                    //        foreach (ReconPosition reconPosition in reconPositionColl)
                    //        {
                    //            if (reconPosition.RIC.Equals(rICSymbol))
                    //            {
                    //                reconPosition.Symbol = pranaSymbol;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return reconPositionColl;
        }


        private static string ReceiveFile()
        {
            string strFileName = string.Empty;
            try
            {
                // File open dialog , ask user to select a File
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = "DeskTop";
                openFileDialog1.Title = "Select File to Import";
                openFileDialog1.Filter = openFileDialog1.Filter = "Excel Files (*.xls)|*.xls|CSV Files (*.csv)|*.csv";

                DialogResult importResult = openFileDialog1.ShowDialog();
                if (importResult == DialogResult.OK)
                {
                    strFileName = openFileDialog1.FileName;
                }
                else if (importResult == DialogResult.Cancel)
                {
                    MessageBox.Show("Operation cancelled by User.", "Exception Report");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return strFileName;
        }

        private static DataTable GetDataTableFromDifferentFileFormats(string fileName)
        {
            DataTable dTable = null;
            try
            {
                string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

                switch (fileFormat.ToUpperInvariant())
                {
                    case "CSV":
                        dTable = FileReaderFactory.Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(fileName);
                        //dTable = ParseCSVFile(fileName); 
                        break;
                    case "XLS":
                        dTable = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    default:
                        break;
                }

                ///Modify the table schema and assign in the same table.
                //dTable = ModifyTableSchema(dTable);
            }
            catch (System.IO.IOException)
            {
                //if (ex.TargetSite.MethodHandle.Value.ToString() == "2032573600")
                //{
                MessageBox.Show("File in use! Please close the file and retry.", "Exception Report");
                throw;
                // }
            }
            catch (Exception)
            {
                throw;

            }
            return dTable;
        }

        /// <summary>
        /// Author : rajat
        /// Takes the datatable and modifies the column names for now.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static DataTable ModifyTableSchema(DataTable inputTable)
        {
            DataTable dt = new DataTable();
            try
            {
                string columnName1 = "COL";
                for (int j = 0; j < inputTable.Columns.Count; j++)
                {
                    dt.Columns.Add(new DataColumn(columnName1 + (j + 1)));
                }

                DataRow dr = dt.NewRow();

                for (int i = 0; i < inputTable.Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr.ItemArray = inputTable.Rows[i].ItemArray;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        private static DataTable ParseCSVFile(string path)
        {
            string inputString = "";
            try
            {
                // check that the file exists before opening it
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path);
                    inputString = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ParseCSV(inputString);
        }

        public static DataTable ParseCSV(string inputString)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] lines = inputString.Split('\n');

                string[] elements = lines[1].Split(',');

                string columnName1 = "COL";
                for (int j = 0; j < elements.Length; j++)
                {
                    dt.Columns.Add(new DataColumn(columnName1 + (j + 1)));
                }

                DataRow dr = dt.NewRow();

                for (int i = 0; i < lines.Length; i++)
                {
                    elements = lines[i].Split(',');
                    dr = dt.NewRow();
                    dr.ItemArray = elements;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }


        private static DataTable ArrangeTable(DataTable dt, string tableName)
        {
            try
            {
                int count = dt.Rows.Count;
                dt.Rows.RemoveAt(count - 1);

                // what XML we will generate, all the tagname will be like COL1,COL2...
                string columnName = "COL";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    dt.Columns[j].ColumnName = columnName + (j + 1);

                }
                dt.TableName = tableName;

                // update the Table columns value with "*" where columns value blank in the excel sheet
                // when we generate the XML for that table, the blank coluns do not comes in the generated XML
                // the indexing of the generated XML changed because of blank columns
                // so defalut value of the columns will be  "*"
                for (int irow = 0; irow < dt.Rows.Count; irow++)
                {
                    for (int icol = 0; icol < dt.Columns.Count; icol++)
                    {
                        string val = dt.Rows[irow].ItemArray[icol].ToString();
                        if (String.IsNullOrEmpty(val))
                        {
                            dt.Rows[irow][icol] = "*";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dt;
        }

        private static string GetFilePathAfterXSLTTransformation(DataTable resultModified, string xSLTName)
        {
            string mappedfilePath = string.Empty;
            try
            {

                if (!String.IsNullOrEmpty(xSLTName))
                {
                    // now generate the xml of that table dt
                    if (!Directory.Exists(Application.StartupPath + @"\xmls\Transformation\Temp"))
                        Directory.CreateDirectory(Application.StartupPath + @"\xmls\Transformation\Temp");
                    string serializedPMXml = Application.StartupPath + @"\xmls\Transformation\Temp\serializedXMLforExceptionReport.xml";
                    resultModified.WriteXml(serializedPMXml);
                    // get a new mapped xml
                    string mappedxml = Application.StartupPath + @"\xmls\Transformation\Temp\ConvertedXMLforExceptionReport.xml";
                    // get the Exception Report XSLT 
                    string dirctoryPath = ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString();
                    string strXSLTStartUpPath = Application.StartupPath + "\\" + dirctoryPath + "\\" +  xSLTName;
                    //                                                  serializedXML,MappedserXML, XSLTPath                    
                    mappedfilePath = XMLUtilities.GetTransformed(serializedPMXml, mappedxml, strXSLTStartUpPath);
                }
                else
                {
                    MessageBox.Show("DataSource XSLT not found.", "Exception Report");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }

            return mappedfilePath;
        }

        private static List<ReconPosition> GetReconPositionColl(DataSet ds)
        {
            List<ReconPosition> reconPositionCollection = new List<ReconPosition>();
            try
            {
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//Modules//" + "Prana.PM.BLL.dll");
                Type typeToLoad = assembly.GetType(typeof(ReconPosition).ToString());

                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    ReconPosition exceptionReport = new ReconPosition();

                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        string colName = ds.Tables[0].Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(exceptionReport, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(exceptionReport, ds.Tables[0].Rows[irow][icol].ToString(), null);
                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(exceptionReport, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result1;
                                    blnIsTrue = double.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result1);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(exceptionReport, Convert.ToDouble(ds.Tables[0].Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(exceptionReport, double.MinValue, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(exceptionReport, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result2;
                                    blnIsTrue = int.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result2);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(exceptionReport, Convert.ToInt32(ds.Tables[0].Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(exceptionReport, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(exceptionReport, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result3;
                                    blnIsTrue = Int64.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result3);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(exceptionReport, Convert.ToInt64(ds.Tables[0].Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(exceptionReport, 0, null);
                                    }
                                }
                            }

                        }
                    }
                    if (!String.IsNullOrEmpty(exceptionReport.OrderSideTagValue))
                    {
                        exceptionReport.Side = TagDatabaseManager.GetInstance.GetOrderSideText(exceptionReport.OrderSideTagValue);
                    }
                    reconPositionCollection.Add(exceptionReport);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return reconPositionCollection;
        }

        #endregion Select File

        #region Public Methods

        public static void FillAllXSLTForReconDataSource()
        {
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("GetXSLTForReconDataSource"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        int dataSourceID = Convert.ToInt32(row[0].ToString());
                        int reconType = Convert.ToInt32(row[1].ToString());
                        string formatType = row[2].ToString();
                        string xsltName = row[3].ToString();
                        string key = dataSourceID + Prana.BusinessObjects.Seperators.SEPERATOR_7 + reconType + Prana.BusinessObjects.Seperators.SEPERATOR_7 + formatType;

                        if (!_reconDataSourceXSLT.ContainsKey(key))
                        {
                            _reconDataSourceXSLT.Add(key, xsltName);
                        }

                        string fkey = dataSourceID + Prana.BusinessObjects.Seperators.SEPERATOR_7 + reconType;

                        if (!_dictFromatType.ContainsKey(fkey))
                        {
                            List<string> formatTypes = new List<string>();
                            formatTypes.Add(formatType);
                            _dictFromatType.Add(fkey, formatTypes);
                        }
                        else
                        {
                            if (!_dictFromatType[fkey].Contains(formatType))
                            { _dictFromatType[fkey].Add(formatType); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        public static List<ReconDataSource> GetAllDataSourceNames()
        {
            Database db = DatabaseFactory.CreateDatabase();
            List<ReconDataSource> dataSourceList = new List<ReconDataSource>();

            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "PMGetAllDataSourceNames"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dataSourceList.Add(FillDataSourcesNameID(row, 0));

                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dataSourceList;
        }

        /// <summary>
        /// Fills the data sources.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static ReconDataSource FillDataSourcesNameID(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            ReconDataSource dataSource = null;

            if (row != null)
            {
                dataSource = new ReconDataSource();

                int DATASOURCEID = offset + 0;
                int DATASOURCENAME = offset + 1;
                int DATASOURCESHORTNAME = offset + 2;

                try
                {
                    dataSource.DataSourceID = Convert.ToInt32(row[DATASOURCEID].ToString());
                    dataSource.DataSourceName = Convert.ToString(row[DATASOURCENAME]);
                    dataSource.DataSourceShortName = Convert.ToString(row[DATASOURCESHORTNAME]);
                }
                catch (Exception ex)
                {

                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
            }

            return dataSource;
        }

        public static List<ReconPosition> GetAllInternalPositions(string strAUECDate, int dataSourceID, DateTime localAUECDate, ReconType reconType)
        {
            string strSPName = string.Empty;
            Database db = DatabaseFactory.CreateDatabase();
            List<ReconPosition> reconPositionColl = new List<ReconPosition>();
            Object[] parameter = new object[2];
            if (reconType.Equals(ReconType.Position) || reconType.Equals(ReconType.TaxLot))
            {
                parameter[0] = strAUECDate;
                strSPName = "PMGetFundPositionsForDataSource";
            }
            else if (reconType.Equals(ReconType.Transaction))
            {
                parameter[0] = localAUECDate;
                strSPName = "PMGetAllocatedPositionsForDataSource";
            }

            parameter[1] = dataSourceID;

            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(strSPName, parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        reconPositionColl.Add(FillReconPosition(row, 0));

                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return reconPositionColl;
        }

        private static ReconPosition FillReconPosition(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            ReconPosition reconPosition = null;

            if (row != null)
            {
                reconPosition = new ReconPosition();
                int SideID = offset;
                int Side = offset + 1;
                int Symbol = offset + 2;
                int Quantity = offset + 3;
                int AvgPrice = offset + 4;
                int FundShortName = offset + 5;
                int Fees = offset + 6;
                int Commission = offset + 7;

                try
                {
                    if (row[SideID] != System.DBNull.Value)
                    {
                        reconPosition.OrderSideTagValue = row[SideID].ToString();
                    }
                    if (row[Side] != System.DBNull.Value)
                    {
                        reconPosition.Side = row[Side].ToString();
                    }
                    if (row[Symbol] != System.DBNull.Value)
                    {
                        reconPosition.Symbol = Convert.ToString(row[Symbol]);
                    }
                    if (row[Quantity] != System.DBNull.Value)
                    {
                        reconPosition.Quantity = Convert.ToDouble(row[Quantity].ToString());
                    }
                    if (row[AvgPrice] != System.DBNull.Value)
                    {
                        reconPosition.AvgPX = Convert.ToDouble(row[AvgPrice].ToString());
                    }
                    if (row[FundShortName] != System.DBNull.Value)
                    {
                        reconPosition.FundName = Convert.ToString(row[FundShortName]);
                    }
                    if (row[Fees] != System.DBNull.Value)
                    {
                        reconPosition.Fee = Convert.ToDouble(row[Fees].ToString());
                    }
                    if (row[Commission] != System.DBNull.Value)
                    {
                        reconPosition.Commission = Convert.ToDouble(row[Commission].ToString());
                    }
                }
                catch (Exception ex)
                {

                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
            }

            return reconPosition;
        }

        #region Commented

        // Been Replaced by a generic method in Prana.Utilities
        // Abhilash Katiyar  Date: 23/10/2008

        //public static void GetReconReportXSLTFilesFromDB(string startupPath)
        //{
        //    string directoryPath = "XSLT\\ReconXSLT";
        //    string path = startupPath + "\\" + directoryPath + "\\";
        //    DirectoryInfo dir = new DirectoryInfo(path);
        //    string allFileDetails = string.Empty;
        //    char seperator1 = '^';
        //    char seperator2 = '~';
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    foreach (FileInfo f in dir.GetFiles("*.*"))
        //    {
        //        string fileDetails = f.Name + seperator2 + f.LastWriteTimeUtc.ToString();
        //        if (allFileDetails != string.Empty)
        //            allFileDetails = allFileDetails + seperator1 + fileDetails;
        //        else
        //            allFileDetails = fileDetails;
        //    }

        //    try
        //    {
        //        Database db = DatabaseFactory.CreateDatabase();
        //        object[] parameter = new object[3];
        //        parameter[0] = allFileDetails;
        //        parameter[1] = seperator1;
        //        parameter[2] = seperator2;

        //        using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("PMGetReconReportXSLTFileData", parameter))
        //        {
        //            while (reader.Read())
        //            {

        //                object[] row = new object[reader.FieldCount];
        //                reader.GetValues(row);
        //                string fileName = row[0].ToString();
        //                byte[] data = (byte[])row[1];
        //                FileStream fs = null;
        //                try
        //                {
        //                    fs = new FileStream(path + "\\" + fileName, FileMode.Create);
        //                    fs.Write(data, 0, data.Length);
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message, "Exception Report");
        //                }
        //                finally
        //                {
        //                    if (fs != null)
        //                        fs.Close();
        //                }
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        #endregion

        #endregion Public Methods
    }
}
