using System.Collections.Generic;

namespace Prana.PM.BLL
{
    public class ReconInfo
    {
        public ReconInfo()
        {

        }
        private List<ReconPosition> _InternalOrders = new List<ReconPosition>();
        private List<ReconPosition> _unmatchedInternalOrders = new List<ReconPosition>();
        private List<ReconPosition> _MatchedInternalOrders = new List<ReconPosition>();

        private List<ReconPosition> _ExternalOrders = new List<ReconPosition>();
        private List<ReconPosition> _matchedExternalOrders = new List<ReconPosition>();
        private List<ReconPosition> _unmatchedExternalOrders = new List<ReconPosition>();

        public List<ReconPosition> InternalOrders
        {
            get { return _InternalOrders; }
            set { _InternalOrders = value; }
        }

        /// <summary>
        /// This collection contains the full data imported from the external data source (excel file or csv file)
        /// </summary>
        public List<ReconPosition> ExternalOrders
        {
            get { return _ExternalOrders; }
            set { _ExternalOrders = value; }
        }


        public List<ReconPosition> CloneOrders(List<ReconPosition> source)
        {

            ReconPosition[] reconPositionArr = new ReconPosition[source.Count];
            source.CopyTo(reconPositionArr);

            List<ReconPosition> clonedOrderCollection = new List<ReconPosition>((IEnumerable<ReconPosition>)reconPositionArr);

            return clonedOrderCollection;
            //clonedOrderCollection 

        }


        public List<ReconPosition> MatchedInternalOrders
        {
            get { return _MatchedInternalOrders; }
            set { _MatchedInternalOrders = value; }
        }

        public List<ReconPosition> MatchedExternalOrders
        {
            get { return _matchedExternalOrders; }
            set { _matchedExternalOrders = value; }
        }

        public List<ReconPosition> UnmatchedInternalOrders
        {
            get { return _unmatchedInternalOrders; }
            set { _unmatchedInternalOrders = value; }
        }

        public List<ReconPosition> UnmatchedExternalOrders
        {
            get { return _unmatchedExternalOrders; }
            set { _unmatchedExternalOrders = value; }
        }

    }
}
