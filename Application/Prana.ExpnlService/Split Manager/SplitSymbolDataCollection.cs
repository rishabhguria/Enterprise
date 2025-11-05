using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System.Collections.Concurrent;

namespace Prana.ExpnlService
{
    public class SplitSymbolDataCollection
    {
        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private AssetCategory _assetCategory;
        public AssetCategory AssetCategory
        {
            get { return _assetCategory; }
            set { _assetCategory = value; }
        }

        private string _contractType;
        public string ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
        }

        private ConcurrentDictionary<string, ExposureAndPnlOrderCollection> _sideWiseOrderCollection = new ConcurrentDictionary<string, ExposureAndPnlOrderCollection>();
        public ConcurrentDictionary<string, ExposureAndPnlOrderCollection> SideWiseOrderCollection
        {
            get { return _sideWiseOrderCollection; }
            set { _sideWiseOrderCollection = value; }
        }
    }
}
