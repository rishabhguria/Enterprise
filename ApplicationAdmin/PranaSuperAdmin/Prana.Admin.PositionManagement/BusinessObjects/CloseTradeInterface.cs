using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CloseTradeInterface
    {
        private DateTime _dateTime;

        /// <summary>
        /// Gets or sets the date time value.
        /// </summary>
        /// <value>The date time value.</value>
        public DateTime Date
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }

        private CloseTradePreferences _preferences;

        /// <summary>
        /// Gets or sets the preferences.
        /// </summary>
        /// <value>The preferences.</value>
	    public CloseTradePreferences Preferences
	    {
		    get 
            {
                if (_preferences == null)
                {
                    _preferences = new CloseTradePreferences();
                }  
                return _preferences; 
            }
		    set { _preferences = value;}
	    }        

        private SortableSearchableList<AllocatedTrades> _currentAllocatedTradesData;

        /// <summary>
        /// Gets or sets the allocated trades data.
        /// </summary>
        /// <value>The allocated trades data.</value>
        public SortableSearchableList<AllocatedTrades> AllocatedTradesData
        {
            get { return _currentAllocatedTradesData; }
            set { _currentAllocatedTradesData = value; }
        }
	

        

        private SortableSearchableList<PositionEligibleForClosing> _positionsEligibleForClosingList;

        /// <summary>
        /// Gets or sets the positions eligible for closing.
        /// </summary>
        /// <value>The positions eligible for closing.</value>
        public SortableSearchableList<PositionEligibleForClosing> PositionsEligibleForClosing
        {
            get { return _positionsEligibleForClosingList; }
            set { _positionsEligibleForClosingList = value; }
        }
	
    }
}
