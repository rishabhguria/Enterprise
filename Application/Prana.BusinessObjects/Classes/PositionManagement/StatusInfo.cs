using System.Collections.Generic;


namespace Prana.BusinessObjects
{
    public class StatusInfo
    {
        private PostTradeEnums.Status _status = PostTradeEnums.Status.Closed;
        public PostTradeEnums.Status Status
        {
            get { return _status; }
            set
            {
                _status = value;

            }
        }

        // Sandeep, 11 NOV 2014: This is used to show message details on closing UI i.e. account and symbol is closed in future date
        private string _details = string.Empty;
        public string Details
        {
            get { return _details; }
            set
            {
                _details = value;

            }
        }

        private Dictionary<string, PostTradeEnums.Status> _exercisedUnderlying = new Dictionary<string, PostTradeEnums.Status>();
        public Dictionary<string, PostTradeEnums.Status> ExercisedUnderlying
        {
            get { return _exercisedUnderlying; }
            set
            {
                _exercisedUnderlying = value;

            }
        }
    }
}
