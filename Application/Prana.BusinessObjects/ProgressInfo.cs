using System;

namespace Prana.BusinessObjects
{

    [Serializable]
    public class ProgressInfo
    {
        /// <summary>
        /// To be used to display status on form header..
        /// </summary>
        private string _headerText;

        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }


        /// <summary>
        /// can be displayed on a tool strip and be used as sub status depicting the current progress of a long running task..
        /// </summary>
        private string _progressStatus;

        public string ProgressStatus
        {
            get { return _progressStatus; }
            set { _progressStatus = value; }
        }


        private bool _isTaskCompleted = false;

        public bool IsTaskCompleted
        {
            get { return _isTaskCompleted; }
            set { _isTaskCompleted = value; }
        }

    }
}
