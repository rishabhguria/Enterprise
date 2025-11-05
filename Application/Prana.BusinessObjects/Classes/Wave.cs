using System;



namespace Prana.BusinessObjects
{
    [Serializable]
    public class Wave
    {
        string _waveID = string.Empty;
        //string _tradedWaveID = string.Empty;
        string _basketID = string.Empty;
        string _waveName = string.Empty;
        float _percentage = float.MinValue;
        OrderCollection _waveOrders = null;
        bool _canBeTraded = true;
        public Wave()
        {
            _waveOrders = new OrderCollection();
            _waveID = IDGenerator.GenerateClientWaveID();
        }



        public float Percentage
        {
            get { return _percentage; }
            set { _percentage = value; }
        }
        public string Name
        {
            get { return _waveName; }
            set { _waveName = value; }
        }
        public string WaveID
        {
            get { return _waveID; }
            set { _waveID = value; }
        }
        //public string  TradedWaveID
        //{
        //    get { return _tradedWaveID; }
        //    set { _tradedWaveID = value; }
        //}
        public OrderCollection WaveOrders
        {
            get { return _waveOrders; }
            set { _waveOrders = value; }
        }
        public string BasketID
        {
            get { return _basketID; }
            set { _basketID = value; }
        }
        public bool CanBeTraded
        {
            get { return _canBeTraded; }
            set { _canBeTraded = value; }
        }
    }
}
