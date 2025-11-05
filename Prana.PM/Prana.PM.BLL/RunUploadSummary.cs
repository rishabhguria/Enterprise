using Csla;
//
using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class RunUploadSummary : BusinessBase<RunUploadSummary>
    {
        private CompanyNameID _companyNameID;

        /// <summary>
        /// Gets or sets the company name ID value.
        /// </summary>
        /// <value>The company name ID value.</value>
        public CompanyNameID CompanyNameIDValue
        {
            get
            {
                if (_companyNameID == null)
                {
                    _companyNameID = new CompanyNameID();
                }
                return _companyNameID;
            }
            set { _companyNameID = value; }
        }

        private SortableSearchableList<SetUploadClient> _runUploadList;

        /// <summary>
        /// Gets or sets the run upload .
        /// This is the collection of rows of data of run upload information. 
        /// </summary>
        /// <value>The run upload set UP.</value>
        public SortableSearchableList<SetUploadClient> RunUploadList
        {
            get { return _runUploadList; }
            set { _runUploadList = value; }
        }


        protected override object GetIdValue()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
