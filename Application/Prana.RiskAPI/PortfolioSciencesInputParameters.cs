using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Utilities.MiscUtilities;
using System;

namespace Prana.RiskServer
{
    public class PortfolioSciencesInputParameters
    {
        private string _psSymbol;
        public string PSSymbol
        {
            get { return _psSymbol; }
            set { _psSymbol = value; }
        }

        private string _quantity;
        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private string _startDate;
        public string StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        private string _endDate;
        public string EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        private double _confidenceLevel;
        public double ConfidenceLevel
        {
            get { return _confidenceLevel; }
            set { _confidenceLevel = value; }
        }

        private int _method;
        public int Method
        {
            get { return _method; }
            set { _method = value; }
        }

        private int _volType;
        public int VolType
        {
            get { return _volType; }
            set { _volType = value; }
        }

        private double _lambda;
        public double Lambda
        {
            get { return _lambda; }
            set { _lambda = value; }
        }

        private string _benchMark;
        public string BenchMark
        {
            get { return _benchMark; }
            set { _benchMark = value; }
        }

        private string _benchMarkQuantity;
        public string BenchMarkQuantity
        {
            get { return _benchMarkQuantity; }
            set { _benchMarkQuantity = value; }
        }

        private double _indexStress;
        public double IndexStress
        {
            get { return _indexStress; }
            set { _indexStress = value; }
        }

        private string _pxSelectedFeed;
        public string PxSelectedFeed
        {
            get { return _pxSelectedFeed; }
            set { _pxSelectedFeed = value; }
        }

        private string _underlyingPrice;
        public string UnderlyingPrice
        {
            get { return _underlyingPrice; }
            set { _underlyingPrice = value; }
        }

        private double _symbolCount;
        public double SymbolCount
        {
            get { return _symbolCount; }
            set { _symbolCount = value; }
        }

        public PortfolioSciencesInputParameters()
        {
        }

        public PortfolioSciencesInputParameters(PranaRequestCarrier pranaRequestCarrier, bool forGroup)
        {
            PranaRequestCarrierContainer pranaRequestCarrierContainer;

            if (forGroup)
            {
                pranaRequestCarrierContainer = pranaRequestCarrier.GetGroupPranaRequestCarrierContainer();
            }
            else
            {
                pranaRequestCarrierContainer = pranaRequestCarrier.GetIndividualPranaRequestCarrierContainer();
            }

            _psSymbol = GeneralUtilities.GetStringFromList(pranaRequestCarrierContainer.PSSymbols, ',');
            _quantity = GeneralUtilities.GetStringFromList(pranaRequestCarrierContainer.Quantities, ',');
            _startDate = pranaRequestCarrier.StartDate.ToString("MM/dd/yy");
            _endDate = pranaRequestCarrier.EndDate.ToString("MM/dd/yy");
            _confidenceLevel = Convert.ToDouble(RiskPreferenceManager.RiskPrefernece.ConfidenceLevelPercent.ToString()) / 100;
            _method = Convert.ToInt32(RiskPreferenceManager.RiskPrefernece.Method);
            _volType = Convert.ToInt32(RiskPreferenceManager.RiskPrefernece.VolatilityType);
            _lambda = RiskPreferenceManager.RiskPrefernece.Lambda;
            _benchMark = pranaRequestCarrier.BenchMarkSymbols;
            _benchMarkQuantity = pranaRequestCarrier.BenchMarkQty;
            _indexStress = pranaRequestCarrier.IndexStress / 100;
            _pxSelectedFeed = GeneralUtilities.GetStringFromList(pranaRequestCarrierContainer.PxSelectedFeeds, ',');
            _underlyingPrice = GeneralUtilities.GetStringFromList(pranaRequestCarrierContainer.UnderlyingPrices, ',');
            _symbolCount = pranaRequestCarrierContainer.PSSymbols.Count;
        }
    }
}
