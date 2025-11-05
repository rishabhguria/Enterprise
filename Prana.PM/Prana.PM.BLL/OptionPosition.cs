using Prana.BusinessObjects.PositionManagement;
using System;


namespace Prana.PM.BLL
{
    [Serializable]
    class OptionPosition : Position
    {
        private bool _callFlag;
        /// <summary>
        /// Gets or sets a value indicating whether [call flag].
        /// </summary>
        /// <value><c>true</c> if [call flag]; otherwise, <c>false</c>.</value>
        public bool CallFlag
        {
            get { return _callFlag; }
            set { _callFlag = value; }
        }

        private double _strikePrice;
        /// <summary>
        /// Gets or sets the strike price.
        /// </summary>
        /// <value>The strike price.</value>
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private string _expirationMonth;
        /// <summary>
        /// Gets or sets the expiration month.
        /// </summary>
        /// <value>The expiration month.</value>
        public string ExpirationMonth
        {
            get { return _expirationMonth; }
            set { _expirationMonth = value; }
        }

        private double _delta;
        /// <summary>
        /// Gets or sets the delta.
        /// </summary>
        /// <value>The delta.</value>
        public double Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        private double _gamma;
        /// <summary>
        /// Gets or sets the gamma.
        /// </summary>
        /// <value>The gamma.</value>
        public double Gamma
        {
            get { return _gamma; }
            set { _gamma = value; }
        }

        private double _rho;
        /// <summary>
        /// Gets or sets the rho.
        /// </summary>
        /// <value>The rho.</value>
        public double Rho
        {
            get { return _rho; }
            set { _rho = value; }
        }

        private double _theta;
        /// <summary>
        /// Gets or sets the theta.
        /// </summary>
        /// <value>The theta.</value>
        public double Theta
        {
            get { return _theta; }
            set { _theta = value; }
        }

        private double _volatility;
        /// <summary>
        /// Gets or sets the implied volatility.
        /// </summary>
        /// <value>The implied volatility.</value>
        public double Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }

        private double _vega;
        /// <summary>
        /// Gets or sets the vega.
        /// </summary>
        /// <value>The vega.</value>
        public double Vega
        {
            get { return _vega; }
            set { _vega = value; }
        }
    }
}
