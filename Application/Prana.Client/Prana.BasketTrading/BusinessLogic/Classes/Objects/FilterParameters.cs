using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;
namespace Prana.BasketTrading
{
   public  class FilterParameters
    {
        int _assetID=int.MinValue ;
        int _underLyingID=int.MinValue;
        string _sideTagValue = ApplicationConstants.C_COMBO_SELECT;
        string _orderTypeTagValue=ApplicationConstants.C_COMBO_SELECT;
        string _venueName = ApplicationConstants.C_COMBO_SELECT;
        string _counterPartyName = ApplicationConstants.C_COMBO_SELECT;
        string _symbol=string.Empty;
       public FilterParameters()
       {
       }

       public FilterParameters(int assetID, int underLyingID, string sideTagValue, string orderTypeTagValue, string venueName, string counterPartyName, string symbol)
        {
            _assetID = assetID;
            _underLyingID = underLyingID;
            _sideTagValue = sideTagValue;
            _orderTypeTagValue = orderTypeTagValue;
            _venueName = venueName ;
            _counterPartyName = counterPartyName;
            _symbol = symbol;

        }
        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }
        public int UnderLyingID
        {
           get { return _underLyingID; }
           set { _underLyingID = value; }
        }
        public string  SideTagValue
        {
            get { return _sideTagValue; }
            set { _sideTagValue = value; }
        }
        public string OrderTypeTagValue
       {
           get { return _orderTypeTagValue; }
           set { _orderTypeTagValue = value; }
       }
       public string VenueName
        {
            get { return _venueName; }
            set { _venueName = value; }
        }
       public string CounterPartyName
        {
            get { return _counterPartyName; }
            set { _counterPartyName = value; }
        }
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
    }
}
