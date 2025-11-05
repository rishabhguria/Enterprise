namespace Prana.Utilities.UI
{
    public class MessageBoxInfo
    {
        private string _Caption;

        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }

        private string _MessageText;

        public string MessageText
        {
            get { return _MessageText; }
            set { _MessageText = value; }
        }

        private CustomMessageBoxButtons _MessageBoxButtons;

        public CustomMessageBoxButtons MessageBoxButtons
        {
            get { return _MessageBoxButtons; }
            set { _MessageBoxButtons = value; }
        }

        private ErrorIcon _ErrorIcons;

        public ErrorIcon ErrorIcons
        {
            get { return _ErrorIcons; }
            set { _ErrorIcons = value; }
        }

        public MessageBoxInfo(string MessageText, ErrorIcon ErrorIcons, CustomMessageBoxButtons MessageBoxButtons, string Caption)
        {
            this._Caption = Caption;
            this._MessageText = MessageText;
            this._MessageBoxButtons = MessageBoxButtons;
            this._ErrorIcons = ErrorIcons;
        }

        //public MessageBoxInfo(string MessageText, ErrorIcon ErrorIcons, CustomMessageBoxButtons MessageBoxButtons)
        //{
        //    this.Caption = "Error";
        //    this.MessageText = MessageText;
        //    this.MessageBoxButtons = MessageBoxButtons;
        //    this.ErrorIcons = ErrorIcons;
        //}

        public MessageBoxInfo(string MessageText, MessageType messageType)
        {
            if (messageType == MessageType.Information)
            {
                this._Caption = "Information";
                this._MessageBoxButtons = CustomMessageBoxButtons.OK;
                this._ErrorIcons = ErrorIcon.Information;
            }

            else if (messageType == MessageType.Warning)
            {
                this._Caption = "Warning!";
                this._MessageBoxButtons = CustomMessageBoxButtons.OKCancel;
                this._ErrorIcons = ErrorIcon.Error;
            }

            else if (messageType == MessageType.Question)
            {
                this._Caption = "Question";
                this._MessageBoxButtons = CustomMessageBoxButtons.YesNo;
                this._ErrorIcons = ErrorIcon.Question;
            }

            this._MessageText = MessageText;

        }

        public MessageBoxInfo(string MessageText)
        {
            this._Caption = "Information";
            this._MessageText = MessageText;
            this._MessageBoxButtons = CustomMessageBoxButtons.OK;
            this._ErrorIcons = ErrorIcon.None;
        }

    }

    public enum CustomMessageBoxButtons
    {
        OK = 0,
        OKCancel = 1,
        AbortRetryIgnore = 2,
        YesNoCancel = 3,
        YesNo = 4,
        RetryCancel = 5,
    }

    public enum ErrorIcon
    {
        None = 0,
        Error = 16,
        Question = 32,
        Exclamation = 48,
        Information = 64,

    }
    public enum MessageType
    {
        Warning,
        Information,
        Question
    }

}
