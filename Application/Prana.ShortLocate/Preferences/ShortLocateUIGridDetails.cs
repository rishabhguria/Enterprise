using Prana.LogManager;
using System;

namespace Prana.ShortLocate.Preferences
{
    public class ShortLocateUIGridDetails
    {
        private string _field;

        private double _decimalplaces;


        public string FieldName
        {
            get { return _field; }
            set { _field = value; }
        }

        public double DecimalPlaces
        {
            get { return _decimalplaces; }
            set { _decimalplaces = value; }
        }

        public ShortLocateUIGridDetails()
        {
            try
            {
                this._field = string.Empty;
                this._decimalplaces = 0;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        public void ShortLocateDecimalPreference(string field, double noOfdecimal)
        {
            try
            {
                this._field = field;
                this._decimalplaces = noOfdecimal;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

    }
}
