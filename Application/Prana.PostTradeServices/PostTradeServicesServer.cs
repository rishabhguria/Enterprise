using Microsoft.SqlServer.Dts.Runtime;
using Prana.BusinessObjects;
using Prana.BusinessObjects.EventArguments;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Prana.PostTradeServices
{
    public class PostTradeServicesServer
    {
        //Corrected event delegate
        public delegate void GetErrorMsgEventHandler(object sender, PostTradeServicesErrorEventArgs e);
        public event GetErrorMsgEventHandler GetErrorMsgEvent;

        static IQueueProcessor _queueCommMgrOut = null;
        static PostTradeServicesServer _postTradeServicesComponent = null;
        static PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        static PostTradeServicesServer()
        {
            _postTradeServicesComponent = new PostTradeServicesServer();
        }
        public static PostTradeServicesServer GetInstance
        {
            get
            {
                return _postTradeServicesComponent;
            }
        }

        //IAllocationServices _allocationServices = null;

        public void Initilise(IQueueProcessor queueCommMgrIn, IQueueProcessor queueCommMgrOut, ISecMasterServices secServices)
        {
            try
            {
                _secMasterServices = secServices;
                //_allocationServices = allocationServices;
                _queueCommMgrOut = queueCommMgrOut;
                queueCommMgrIn.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(queueCommMgrIn_MessageQueued);
                //new MessageReceivedHandler(queueCommMgrIn_MessageQueued);
                CentralPositionManger.SecMasterServices = secServices;
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void queueCommMgrIn_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage message = e.Value;
                switch (message.MsgType)
                {

                    //case CustomFIXConstants.MSG_Undo_Split:
                    //    {
                    //        CAOnProcessObjects caOnProcessObject = (CAOnProcessObjects)binaryFormatter.DeSerialize(message.Message.ToString());
                    //        string caIds = caOnProcessObject.CorporateActionIDs;
                    //        int affectedPositions1 = SecMasterDataManager.UndoCA(caIds);
                    //        int affectedPositions2 = AllocationDataManager.UndoSplit(caIds);

                    //        caOnProcessObject.IsSaved = (affectedPositions1 > 0) && (affectedPositions2 > 0);
                    //        string responseCopAction = binaryFormatter.Serialize(caOnProcessObject);

                    //        QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_Coprorate_Undo_Response, responseCopAction);
                    //        qMsg.HashCode = message.HashCode;
                    //        _queueCommMgrOut.SendMessage(qMsg);
                    //        break;
                    //    }
                    //case CustomFIXConstants.MSG_Undo_CashDividend:
                    //    {
                    //        CAOnProcessObjects caOnProcessObject = (CAOnProcessObjects)binaryFormatter.DeSerialize(message.Message.ToString());
                    //        string caIds = caOnProcessObject.CorporateActionIDs;
                    //        int affectedPositions1 = SecMasterDataManager.UndoCA(caIds);
                    //        int affectedPositions2 = AllocationDataManager.UndoCashDividend(caIds);

                    //        caOnProcessObject.IsSaved = (affectedPositions1 > 0) && (affectedPositions2 > 0);
                    //        string responseCopAction = binaryFormatter.Serialize(caOnProcessObject);

                    //        QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_Coprorate_Undo_Response, responseCopAction);
                    //        qMsg.HashCode = message.HashCode;
                    //        _queueCommMgrOut.SendMessage(qMsg);
                    //        break;
                    //    }

                    // This was done to convert the FIX based corp action saving into xml type.
                    //case CustomFIXConstants.MSG_ConvertFixToXmlFormatInDb:
                    //    {
                    //        CAOnProcessObjects caOnProcessObject = (CAOnProcessObjects)binaryFormatter.DeSerialize(message.Message.ToString());
                    //        int affectedPositions1 = SecMasterDataManager.ConvertFixToXmlFormatInDb(caOnProcessObject.CorporateActionListString);

                    //        caOnProcessObject.IsSaved = (affectedPositions1 > 0);
                    //        string responseCopAction = binaryFormatter.Serialize(caOnProcessObject);
                    //        QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_ConvertFixToXmlFormatInDb, responseCopAction);
                    //        qMsg.HashCode = message.HashCode;
                    //        _queueCommMgrOut.SendMessage(qMsg);
                    //        break;
                    //    }

                    case CustomFIXConstants.MSG_GetOldCompanyNameForNameChange:
                        {
                            CAOnProcessObjects caOnProcessObject = (CAOnProcessObjects)binaryFormatter.DeSerialize(message.Message.ToString());
                            string oldCompanyName = _secMasterServices.GetOldCompanyNameForNameChange(caOnProcessObject.CorporateActionID);
                            caOnProcessObject.Message = oldCompanyName;
                            string responseCopAction = binaryFormatter.Serialize(caOnProcessObject);
                            QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_GetOldCompanyNameForNameChange, responseCopAction);
                            qMsg.HashCode = message.HashCode;
                            _queueCommMgrOut.SendMessage(qMsg);
                            break;
                        }


                    //case CustomFIXConstants.MSG_SaveCAForSplits:
                    //    {
                    //        CAOnProcessObjects caOnProcessObject = (CAOnProcessObjects)binaryFormatter.DeSerialize(message.Message.ToString());
                    //        bool isApplied = true;
                    //        int affectedPositions1 = SecMasterDataManager.SaveCorporateAction(caOnProcessObject.CorporateActionListString, isApplied);
                    //        int affectedPositions2 = AllocationDataManager.SaveAllTaxLotsPostCorporateAction(caOnProcessObject.Taxlots);

                    //        caOnProcessObject.IsSaved = (affectedPositions1 > 0) && (affectedPositions2 > 0);
                    //        string responseCopAction = binaryFormatter.Serialize(caOnProcessObject);
                    //        QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SaveCAForSplits, responseCopAction);
                    //        qMsg.HashCode = message.HashCode;
                    //        _queueCommMgrOut.SendMessage(qMsg);
                    //        break;
                    //    }
                    //case CustomFIXConstants.MSG_SaveCAForCashDividend:
                    //    {
                    //        CAOnProcessObjects caOnProcessObject = (CAOnProcessObjects)binaryFormatter.DeSerialize(message.Message.ToString());
                    //        bool isApplied = true;
                    //        int affectedPositions1 = SecMasterDataManager.SaveCorporateAction(caOnProcessObject.CorporateActionListString, isApplied);
                    //        int affectedPositions2 = AllocationDataManager.SaveCashDividendForTaxlots(caOnProcessObject.Taxlots);

                    //        caOnProcessObject.IsSaved = (affectedPositions1 > 0) && (affectedPositions2 > 0);
                    //        string responseCopAction = binaryFormatter.Serialize(caOnProcessObject);
                    //        QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SaveCAForCashDividend, responseCopAction);
                    //        qMsg.HashCode = message.HashCode;
                    //        _queueCommMgrOut.SendMessage(qMsg);
                    //        break;
                    //    }


                    //case CustomFIXConstants.MSG_SECMASTER_SymbolREQ:
                    //    {
                    //        SymbolLookupRequestObject lookupObj = (SymbolLookupRequestObject)binaryFormatter.DeSerialize(message.Message.ToString());

                    //        CustomXmlSerializer xml = new CustomXmlSerializer();
                    //        DataSet ds = SecMasterDataManager.GetSymbolLookupRequestedData(xml.WriteString(lookupObj));

                    //        CAOnProcessObjects caOnProcessObject = new CAOnProcessObjects();
                    //        caOnProcessObject.Symbol = lookupObj.TickerSymbol;
                    //        caOnProcessObject.CompanyName = lookupObj.Name;
                    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    //        {
                    //            //Data Found
                    //            caOnProcessObject.IsExist = true;
                    //        }
                    //        else
                    //        {
                    //            //Data Not Found
                    //            caOnProcessObject.IsExist = false;
                    //        }

                    //        string responseStr = binaryFormatter.Serialize(caOnProcessObject);
                    //        QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_SECMASTER_SymbolREQ, responseStr);
                    //        qMsg.HashCode = message.HashCode;
                    //        _queueCommMgrOut.SendMessage(qMsg);
                    //    }
                    //    break;
                    case CustomFIXConstants.MSG_PREPARE_REPORTS:
                        BackgroundWorker bgWorker = new BackgroundWorker();
                        bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                        bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
                        bgWorker.RunWorkerAsync();
                        break;

                    case CustomFIXConstants.MSG_GET_POSITIONS:
                        {
                            List<string> listGrouping = (List<string>)binaryFormatter.DeSerialize(message.Message.ToString());
                            List<PranaPosition> listPos = CentralPositionManger.GetGroupedPositionAsRiskPref(listGrouping);
                            string responsePoslist = binaryFormatter.Serialize(listPos);
                            QueueMessage qMsgResponse = new QueueMessage(CustomFIXConstants.MSG_GET_POSITIONS, responsePoslist);
                            qMsgResponse.RequestID = message.RequestID;
                            qMsgResponse.UserID = message.UserID;
                            _queueCommMgrOut.SendMessage(qMsgResponse);
                        }
                        break;

                    case CustomFIXConstants.MSG_REFRESH_POSITIONS:
                        CentralPositionManger.Refresh();
                        break;
                    //get stuck trades from Fix - omshiv, NOv, 2013
                    case CustomFIXConstants.MSG_GET_MISSING_TRADES:

                        if (GetErrorMsgEvent != null)
                        {
                            GetErrorMsgEvent(this, new PostTradeServicesErrorEventArgs("DropCopy_PostTrade", message.UserID, message.RequestID));

                        }

                        break;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        //private string GetOptionSymbolPKs(string underlyingSymbol,DateTime effectiveDate)
        //{
        //    //We find out the unexpired count of options for a particular underlying symbol. Current use is to generate Symbol_PK 
        //    //for all these options. Only these options would be reinserted with new underlying name
        //    int optionCount = SecMasterDataManager.GetUnExpiredOptionCountForUnderlying(underlyingSymbol, effectiveDate);

        //    StringBuilder sb = new StringBuilder();

        //    for (int i = 0; i < optionCount; i++)
        //    {
        //        sb.Append(SecurityMasterSymbolIDGenerator.GenerateSymbolPK());
        //        sb.Append(",");
        //    }

        //    return sb.ToString().TrimEnd(new char[] { ',' });
        //}

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                string result = e.Result.ToString();
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_REPORTS_PREPARED, result);
                _queueCommMgrOut.SendMessage(qMsg);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataBaseProcessor.MigrateDataFormTransactionDB();
                DataBaseProcessor.SetUp();
                DataBaseProcessor.ProcessPMTaxlotToPMOpenPostionSnapShot();
                DataBaseProcessor.ProcessPMTaxlotClosingToMarkToMarketPNL();
                DataBaseProcessor.ProcessPMTaxlotsToMonthlyIRR();
                DataBaseProcessor.ClearAllDictionaries();
                //DataBaseProcessor.ProcessAdditionalPMTaxlots();

                string packagePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\AnalysisServices\\Package.dtsx";
                Microsoft.SqlServer.Dts.Runtime.Application app = new Microsoft.SqlServer.Dts.Runtime.Application();
                Package package = app.LoadPackage(packagePath, null);
                DTSExecResult result = package.Execute();
                e.Result = result;
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                e.Result = "Unable to Process Please Contact Support Team.";
            }
        }

        private ISecMasterServices _secMasterServices;
        public ISecMasterServices SecMasterServices
        {
            set { _secMasterServices = value; }

        }
        public void SendErrorMsgToClient(List<PranaMessage> msgs)//, String UserID, String RequestID) removed as parameters not used in method, Microsoft Managed Rules
        {
            try
            {
                if (msgs != null)
                {
                    string response = binaryFormatter.Serialize(msgs);
                    QueueMessage qMsgMissingTrades = new QueueMessage(CustomFIXConstants.MSG_GET_MISSING_TRADES, response);
                    //qMsgMissingTrades.RequestID = UserID;
                    //qMsgMissingTrades.UserID = RequestID;
                    _queueCommMgrOut.SendMessage(qMsgMissingTrades);
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
    }
}
