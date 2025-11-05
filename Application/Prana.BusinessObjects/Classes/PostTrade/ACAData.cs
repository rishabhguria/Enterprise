using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ACAData
    {

        public ACAData(string symbol, int FundID, string orderSideTagValue)
        {
            _symbol = symbol;
            _fundID = FundID;
            _positionType = GetPositionType(orderSideTagValue);

        }

        public ACAData()
        {

        }

        public object Clone()
        {
            Type type = this.GetType();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }

        private string GetPositionType(string OrderSideTagValue)
        {
            switch (OrderSideTagValue)
            {

                case FIXConstants.SIDE_Buy:
                case FIXConstants.SIDE_Buy_Open:
                case FIXConstants.SIDE_Sell:
                case FIXConstants.SIDE_Sell_Closed:

                    return Prana.BusinessObjects.AppConstants.PositionType.Long.ToString();


                case FIXConstants.SIDE_SellShort:
                case FIXConstants.SIDE_Sell_Open:
                case FIXConstants.SIDE_Buy_Closed:

                    return Prana.BusinessObjects.AppConstants.PositionType.Short.ToString();

                default:
                    return Prana.BusinessObjects.AppConstants.PositionType.Long.ToString();
            }
        }

        private string _symbol = string.Empty;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private double _positionQty = 0.0;

        public double PositionQty
        {
            get { return _positionQty; }
            set { _positionQty = value; }
        }

        private double _acaAvgPrice = 0.0;

        public double ACAAvgPrice
        {
            get { return _acaAvgPrice; }
            set { _acaAvgPrice = value; }
        }


        private int _fundID = int.MinValue;

        public int FundID
        {
            get { return _fundID; }
            set { _fundID = value; }
        }

        private DateTime _date = DateTime.MinValue;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private string _positionType = string.Empty;

        public string PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }

        private double _acaUnitCost = 0.0;

        public double ACAUnitCost
        {
            get { return _acaUnitCost; }
            set { _acaUnitCost = value; }
        }

    }
}
