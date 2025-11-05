using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using System.Collections.Generic;

namespace Prana.PostTrade
{
    public class BuySellBucket
    {
        public BuySellBucket()
        {

        }

        private List<TaxLot> _buytaxlotsandpositions = new List<TaxLot>();

        public List<TaxLot> Buytaxlotsandpositions
        {
            get { return _buytaxlotsandpositions; }
            set { _buytaxlotsandpositions = value; }
        }

        private List<TaxLot> _selltaxlotsandpositions = new List<TaxLot>();


        public List<TaxLot> Selltaxlotsandpositions
        {
            get { return _selltaxlotsandpositions; }
            set { _selltaxlotsandpositions = value; }
        }

        private List<Position> _netpositions = new List<Position>();

        public List<Position> Netpositions
        {
            get { return _netpositions; }
            set { _netpositions = value; }
        }

    }
}