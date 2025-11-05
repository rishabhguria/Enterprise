using System;
using System.Collections.Generic;
using System.Text;
using Prana.PM.BLL;
using System.Data;
//using Prana.PM.MultiLayoutParse.Components;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.AppConstants;


namespace Prana.PM.MultiLayoutParse
{
    [ParsingAttribute(DataSourceFileLayout.HeaderData)]
    public class HeaderDataStrategy : FileParsingStrategy
    {
        public override int ParseFileAndStoreData(RunUpload currentFtpProgress)
        {
            return Convert.ToInt16(DataSourceFileLayout.HeaderData);
        }

        public override bool ParseFileAndStoreData(RunUpload currentFtpProgress, DataTable datasourceData)
        {
            return false;
            //return Convert.ToInt16(DataSourceFileLayout.HeaderData);
        }
    }
}
