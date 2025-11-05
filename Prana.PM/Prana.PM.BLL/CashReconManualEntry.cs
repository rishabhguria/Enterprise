//
using Prana.BusinessObjects;

namespace Prana.PM.BLL
{
    public class CashReconManualEntry
    {
        private User _user;

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User
        {
            get
            {
                if (_user == null)
                {
                    _user = new User();
                }
                return _user;
            }
            set { _user = value; }
        }

        private CashReconItem _cashReconItemValue;

        /// <summary>
        /// Gets or sets the cash recon item value.
        /// </summary>
        /// <value>The cash recon item value.</value>
        public CashReconItem CashReconItemValue
        {
            get
            {
                if (_cashReconItemValue == null)
                {
                    _cashReconItemValue = new CashReconItem();
                }
                return _cashReconItemValue;
            }
            set { _cashReconItemValue = value; }
        }


        private SortableSearchableList<CashTransactionDetailReconItem> _cashTransactionEntries;

        /// <summary>
        /// Gets or sets the cash transaction entries.
        /// </summary>
        /// <value>The cash transaction entries.</value>
        public SortableSearchableList<CashTransactionDetailReconItem> CashTransactionEntries
        {
            get { return _cashTransactionEntries; }
            set { _cashTransactionEntries = value; }
        }



        private string _summaryComments;

        /// <summary>
        /// Gets or sets the summary comments.
        /// </summary>
        /// <value>The summary comments.</value>
        public string SummaryComments
        {
            get { return _summaryComments; }
            set { _summaryComments = value; }
        }

    }
}
