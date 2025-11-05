using System;

namespace Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects
{
    // Create a condition with FieldName, operator and fieldValue - omshiv, Nov 2013
    public class SearchCondition
    {
        private string _fieldName = String.Empty;

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        private object _FieldValue;

        public object FieldValue
        {
            get { return _FieldValue; }
            set { _FieldValue = value; }
        }

        private string _operator;

        public string Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        private string _andOr;

        public string AndOr
        {
            get { return _andOr; }
            set { _andOr = value; }
        }

    }
}
