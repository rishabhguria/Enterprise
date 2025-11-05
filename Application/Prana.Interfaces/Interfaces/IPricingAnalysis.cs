using Prana.BusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;

namespace Prana.Interfaces
{
    public interface IPricingAnalysis
    {

        event EventHandler Disconnected;
        event EventHandler Connected;
        //event GreeksCalculaterHandler GreeksCalculated;
        event EventHandler<EventArgs<ResponseObj>> GreeksCalculated;
        //event StepCalculaterHandler StepAnalysisCompleted;
        event EventHandler<EventArgs<List<StepAnalysisResponse>>> StepAnalysisCompleted;
        //event Level1SnapShotReceivedHandler Level1SnapShotReceived;
        //event CompletedReceivedDelegate OptionResponseCompleted;
        //event PricingDataSnapShotHandler UpdateOptionGreeks;
        PranaInternalConstants.ConnectionStatus ConnectionStatus { get; }
        void ConnectToServer(ConnectionProperties connProp);
        void DisConnect();
        //event PricingDataSnapShotHandler PricingDataSnapShotReceived;
        //void SendRequest(InputParametersCollection inputParametersCollection, string requestID);
        //void SendRequest(InputParametersCollection inputParametersCollection,int hashcode);
        //void SendRequest(InputParametersCollection inputParametersCollection, string requestID, int userID);
        void SendSnapShotRequest(InputParametersCollection inputParametersCollection, int userID);
        void SendStepAnalRequest(InputParametersCollection inputParametersCollection, int userID);
        //void SendRequest(VolSkewParametersCollection VolSkewCollection, string requestID, int hashcode, int userID);
        //void SendRequest(StepAnalysisResponse stepAnalysisReq,string requestID, int userID);
        //void SendRequest(List<string> symbolList, string requestID, int userID);
        //void SendRequest(List<string> symbolList,string requestID,int hashcode, int userID);
        void SendRequest(string msgType);
        void SendRequest(string underlyingSymbol, string optSymbol, CompanyUser currentUser);

        ICommunicationManager ClientCommunicationManager
        {
            get;
            set;
        }


    }

    public delegate void PricingDataSnapShotHandler(Dictionary<string, SymbolData> pricingDataDict);
    //public delegate void GreeksCalculaterHandler(ResponseObj SimulationResponse);
    //public delegate void StepCalculaterHandler(List<StepAnalysisResponse> stepRes);
    public delegate void Level1SnapShotReceivedHandler(SymbolData l1Data);
    public delegate void VolSkewHandler(VolSkewObject VolSkewResponse);

}
