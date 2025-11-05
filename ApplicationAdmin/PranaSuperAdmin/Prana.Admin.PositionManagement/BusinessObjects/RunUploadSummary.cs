using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class RunUploadSummary
    {
        private CompanyNameID _companyNameID;

        /// <summary>
        /// Gets or sets the companyNameID.
        /// </summary>
        /// <value>The CompanyNameID.</value>
        public CompanyNameID CompanyNameID
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

        private SortableSearchableList<RunUpload> _runUpload;

        /// <summary>
        /// Gets or sets the run upload .
        /// This is the collection of rows of data of run upload information. 
        /// </summary>
        /// <value>The run upload set UP.</value>
        public SortableSearchableList<RunUpload> RunUpload
        {
            get { return _runUpload; }
            set { _runUpload = value; }
        }
	
    }
}
