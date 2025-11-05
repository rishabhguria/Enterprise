namespace Prana.BusinessObjects
{
    public class PSSymbolMapper
    {
        private int _AUECID;
        public int AUECID
        {
            get { return _AUECID; }
            set { _AUECID = value; }
        }

        private string _ExchangeIdentifier;
        public string ExchangeIdentifier
        {
            get { return _ExchangeIdentifier; }
            set { _ExchangeIdentifier = value; }
        }
        private string _Year;
        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }
        private string _Month;
        public string Month
        {
            get { return _Month; }
            set { _Month = value; }
        }
        private string _Day;
        public string Day
        {
            get { return _Day; }
            set { _Day = value; }
        }
        private string _Type;
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        private string _Strike;
        public string Strike
        {
            get { return _Strike; }
            set { _Strike = value; }
        }
        private string _ExchangeToken;
        public string ExchangeToken
        {
            get { return _ExchangeToken; }
            set { _ExchangeToken = value; }
        }
        private string _PSRootToken;
        public string PSRootToken
        {
            get { return _PSRootToken; }
            set { _PSRootToken = value; }
        }
        private string _PSFormatString;
        public string PSFormatString
        {
            get { return _PSFormatString; }
            set { _PSFormatString = value; }
        }
        private bool _TranslateRoot;
        public bool TranslateRoot
        {
            get { return _TranslateRoot; }
            set { _TranslateRoot = value; }
        }
        private bool _TranslateType;
        public bool TranslateType
        {
            get { return _TranslateType; }
            set { _TranslateType = value; }
        }
        private string _ExerciseStyle;
        public string ExerciseStyle
        {
            get { return _ExerciseStyle; }
            set { _ExerciseStyle = value; }
        }
    }
}