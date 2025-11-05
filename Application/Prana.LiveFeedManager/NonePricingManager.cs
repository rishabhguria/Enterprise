using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prana.LiveFeedManager
{
    /// <summary>
    /// This is a blank ILiveFeedAdapter implementation to be used for None Provider
    /// </summary>
    public sealed class NonePricingManager : ILiveFeedAdapter, IDisposable
    {
        public event EventHandler<EventArgs<bool>> Connected;
        public event EventHandler<EventArgs<bool>> Disconnected;
        public event EventHandler<Data> ContinuousDataResponse;
        public event EventHandler<Data> SnapShotDataResponse;
        public event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;

        public Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols)
        {
            return new Dictionary<string, bool>();
        }

        public void Connect()
        {
            if (Connected != null) { }
        }

        public void DeleteSymbol(string symbol)
        {

        }

        public void Disconnect()
        {

        }

        public void Dispose()
        {
            if (Connected != null)
                Connected = null;
            if (Disconnected != null)
                Disconnected = null;
            if (ContinuousDataResponse != null)
                ContinuousDataResponse = null;
            if (SnapShotDataResponse != null)
                SnapShotDataResponse = null;
            if (OptionChainResponse != null)
                OptionChainResponse = null;
        }

        public List<SymbolData> GetAvailableLiveFeed()
        {
            return new List<SymbolData>();
        }

        public void GetContinuousData(string symbol)
        {

        }

        public Task<object> GetLiveDataDirectlyFromFeed()
        {
            return null;
        }

        public void GetOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter)
        {

        }

        public void GetSnapShotData(string symbol, ApplicationConstants.SymbologyCodes symbologyCode, bool completeInfo)
        {

        }

        public Dictionary<string, int> GetSubscriptionInformation()
        {
            return new Dictionary<string, int>();
        }

        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            return new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetUserInformation()
        {
            return new Dictionary<string, string>();
        }

        public List<Dictionary<string, string>> GetUserPermissionsInformation()
        {
            return new List<Dictionary<string, string>>();
        }

        public void SetDebugEnableDisable(bool isDebugEnable, double pctTolerance)
        {

        }

        public void UpdateSecurityDetails(SecMasterbaseList secMasterList)
        {

        }
    }
}
