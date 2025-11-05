using System.Data;

namespace Prana.DatabaseManager
{
    public class DatabaseParameter
    {
        private string _parameterName;
        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        private DbType _parameterType;
        public DbType ParameterType
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        private object _parameterValue;
        public object ParameterValue
        {
            get { return _parameterValue; }
            set { _parameterValue = value; }
        }

        private bool _isOutParameter;
        public bool IsOutParameter
        {
            get { return _isOutParameter; }
            set { _isOutParameter = value; }
        }

        private int _outParameterSize;
        public int OutParameterSize
        {
            get { return _outParameterSize; }
            set { _outParameterSize = value; }
        }
    }
}
