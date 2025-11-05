//using Prana.PM.ApplicationConstants;
using Csla;
using System;



namespace Prana.BusinessObjects.PositionManagement
{
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class NetPositionList : BusinessListBase<NetPositionList, Position>
    {
        /// <summary>
        /// Gets or sets the <see cref="Prana.PM.BLL.Position"/> with the specified position id to find.
        /// </summary>
        /// <value></value>
        public Position this[string startTaxLotID]
        {
            get
            {
                Position foundPosition = null;

                foreach (Position position in this)
                {
                    if (position.ID.Equals(startTaxLotID))
                    {
                        foundPosition = position;
                        break;
                    }
                }

                return foundPosition;

            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].ID.Equals(startTaxLotID))
                    {
                        this[i] = value;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Find the existing Position object and returns the same. If position object does not exist then create new and return back.
        /// </summary>
        /// <param name="positionID"></param>
        /// <returns></returns>
        public Position GetOrAddPosition(string startTaxlotID)
        {
            Position position = new Position();


            position.ID = startTaxlotID;
            position.IsClosingSaved = false;
            this.Add(position);


            return position;
        }

        /// <summary>
        /// Find the existing Position object and returns the same. If position object does not exist then create new and return back.
        /// </summary>
        /// <param name="positionID"></param>
        /// <returns></returns>
        public Position GetNewPosition()
        {
            Position position = new Position();
            position.IsExpired_Settled = true;
            position.IsClosingSaved = false;
            //position.SettlementMode = Prana.BusinessObjects.AppConstants.SettlementMode.Physical;
            this.Add(position);
            return position;
        }



        /// <summary>
        /// Gets the position for tax lot ID.
        /// </summary>
        /// <param name="taxLotID">The tax lot ID.</param>
        public Position GetPositionForTaxLotID(string taxLotID)
        {

            foreach (Position position in this)
            {
                if (position.ID.Equals(taxLotID))
                {
                    return position;
                }
            }
            return null;

        }
        /// <summary>
        /// Gets the position for tax lot ID.
        /// </summary>
        /// <param name="taxLotID">The tax lot ID.</param>
        public Position GetPositionForTaxLotClosingID(string taxlotClosingID)
        {

            foreach (Position position in this)
            {
                if (position.TaxLotClosingId.ToString().Equals(taxlotClosingID))
                {
                    return position;
                }
            }
            return null;

        }

        /// <summary>
        /// Gets the position for tax lot ID.
        /// </summary>
        /// <param name="taxLotID">The tax lot ID.</param>
        public NetPositionList GetAllPositionsContaningTaxLotAsClosingMember()
        {
            NetPositionList result = new NetPositionList();
            //foreach (Position position in this)
            //{
            //    foreach (AllocatedTrade taxLot in position.PositionTaxLots)
            //    {
            //        if (taxLot.ID.Equals(taxLotID))
            //        {
            //            result.Add(position);
            //            break;
            //        }
            //    }

            //}
            return result;

        }


        //public void UpdateClosingStatus()
        //{
        //    foreach (object var in collection_to_loop)
        //    {

        //    }
        //}



        /// <summary>
        /// Gets all closed allocated trades for tax lot ID.
        /// </summary>
        /// <param name="taxLotID">The tax lot ID.</param>
        /// <returns></returns>
        public AllocatedTradesList GetAllClosedAllocatedTradesForTaxLotID()
        {
            AllocatedTradesList result = null;
            return result;
        }
    }
}