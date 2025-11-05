using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public interface ILiveFeedAdapter
    {
        void Connect();
        void Disconnect();
        void GetContinuousData(string symbol);
        void GetSnapShotData(string symbol, ApplicationConstants.SymbologyCodes symbologyCode, bool completeInfo);
        void GetOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter);
        void DeleteSymbol(string symbol);
        List<SymbolData> GetAvailableLiveFeed();
        Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols);
        Dictionary<string, string> GetUserInformation();
        List<Dictionary<string, string>> GetUserPermissionsInformation();
        Dictionary<string, int> GetSubscriptionInformation();
        Dictionary<string, string> GetTickersLastStatusCode();

        event EventHandler<EventArgs<bool>> Connected;
        event EventHandler<EventArgs<bool>> Disconnected;
        event EventHandler<Data> ContinuousDataResponse;
        event EventHandler<Data> SnapShotDataResponse;
        event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;

        /// <summary>
        /// Set Debug Enable Disable
        /// </summary>
        /// <param name="isDebugEnable"></param>
        /// <param name="pctTolerance"></param>
        void SetDebugEnableDisable(bool isDebugEnable, double pctTolerance);


        /// <summary>
        /// Update Security Details
        /// </summary>
        /// <param name="secMasterList"></param>
        void UpdateSecurityDetails(SecMasterbaseList secMasterList);


        /// <summary>
        /// Get Live Data Directly From Feed
        /// </summary>
        /// <returns></returns>
        System.Threading.Tasks.Task<object> GetLiveDataDirectlyFromFeed();
    }
}
