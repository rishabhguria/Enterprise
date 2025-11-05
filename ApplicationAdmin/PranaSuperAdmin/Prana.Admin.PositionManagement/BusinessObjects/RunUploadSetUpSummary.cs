using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;
	
namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class RunUploadSetUpSummary
    {
        private CompanyNameID _companyNameID;

        /// <summary>
        /// Gets or sets the datasourcenameID.
        /// </summary>
        /// <value>The data source name ID.</value>
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

        private SortableSearchableList<RunUploadSetup> _runUploadSetUP;

        /// <summary>
        /// Gets or sets the run upload set UP.
        /// This is the collection of rows of data of Set Up run upload information. 
        /// </summary>
        /// <value>The run upload set UP.</value>
        public SortableSearchableList<RunUploadSetup> RunUploadSetUp
        {
            get { return _runUploadSetUP; }
            set { _runUploadSetUP = value; }
        }
	
	
	
    }
}
