
using Csla;
using Prana.BusinessObjects.PositionManagement;

namespace Prana.PM.BLL
{
    public class DataSourceReconColumnsInfo : BusinessBase<DataSourceReconColumnsInfo>
    {

        private ThirdPartyNameID _datasourceNameId;

        public ThirdPartyNameID DataSourceNameIDValue
        {
            get { return _datasourceNameId; }
            set
            {
                _datasourceNameId = value;
                PropertyHasChanged();
            }
        }

        private AppReconciliedColumnList _appReconciliedColumnList;

        public AppReconciliedColumnList AppReconciliedColumnList
        {
            get { return _appReconciliedColumnList; }
            set
            {
                _appReconciliedColumnList = value;
                PropertyHasChanged();
            }
        }

        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        //private int _id;
        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return 0;
        }
    }
}
