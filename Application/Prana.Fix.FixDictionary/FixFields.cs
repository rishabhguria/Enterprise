using System;

namespace Prana.Fix.FixDictionary
{
    [Serializable]
    public class FixFields
    {

        private string _Tag;

        public string Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }

        private string _FieldName;

        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        private string _Type;

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private string _Description;

        public string Desc
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private string _LenRefers;

        public string LenRefers
        {
            get { return _LenRefers; }
            set { _LenRefers = value; }
        }
        private bool _isExternal;

        public bool IsExternal
        {
            get { return _isExternal; }
            set { _isExternal = value; }
        }
        private FieldValidationRule _ValidationRule;

        public FieldValidationRule ValidationRule
        {
            get { return _ValidationRule; }
            set { _ValidationRule = value; }
        }


        private int _position;
        /// <summary>
        /// This field contains the position of the tag in given message type
        /// </summary>
        public int Position
        { get { return _position; } set { _position = value; } }

        public string BasicValidation()
        {
            return string.Empty;
        }

        public string Format
        {
            get
            {
                string format = string.Empty;
                if (_Type == string.Empty)
                    return _Type;
                string[] data = _Type.Split(',');
                if (data.Length > 1)
                {
                    format = data[1];
                }
                return format;
            }
        }

        #region Not doing anything so commented
        //public void SetValue(string value)
        //{

        //}

        //public object GetValue(string value)
        //{
        //    return null;
        //}
        #endregion
    }
}
