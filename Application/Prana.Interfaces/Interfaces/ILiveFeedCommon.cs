using System;

namespace Prana.Interfaces
{
    public interface ILiveFeedApplicationConstants
    {
        /// <summary>
        /// It requests the continuous data if possible
        /// </summary>
        /// <param name="Symbol"></param>
        void RequestData(string Symbol);

        void Connect();
        event EventHandler Connected;
        void Disconnect();
        event EventHandler DisConnected;
        bool IsConnected
        {
            get;
        }
        void RemoveAllSymbols();
        void RemoveSymbol(string Symbol);
        event EventHandler UpdateData;
        // purpose for     re-requesting teh data in case dat is not cvomming, after once calling requestdata()
        //void UpdateRequest(string Symbol);

    }
}
