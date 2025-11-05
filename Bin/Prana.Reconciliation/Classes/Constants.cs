using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Prana.Global;

namespace Prana.Reconciliation
{
    public class ReconConstants
    {
        public const string MismatchReason = "MismatchDetails";
        public const string CAPTION_MissingData = "Missing Data";

        public const string CAPTION_MismatchReason = "MismatchDetails";
        public const string MismatchType = "MismatchType";
        public const string CAPTION_MismatchType = "MismatchType";
        public const string Matched = "Matched";
        public const string PrimaryKey = "RowID";
        public static string ReconDataDirectoryPath = Application.StartupPath.ToString() + @"\" + ApplicationConstants.RECON_DATA_DIRECTORY.ToString();


        //public const string TABLENAME_NirvanaMasterColumns = "NirvanaMasterColumns";
        //public const string TABLENAME_PBMasterColumns = "PBMasterColumns";

        public const string TABLENAME_NirvanaGridColumns = "NirvanaGridMasterColumns";
        public const string TABLENAME_PBGridMasterColumns = "PBGridMasterColumns";

        public const string COLUMN_Name = "Name";
        public const string COLUMN_ISSelected = "IsSelected";
        public const string COLUMN_GroupType = "GroupType";
        public const string COLUMN_FormulaExpression = "FormulaExpression";
        public const string COLUMN_ReconType = "ReconType";
        public const string COLUMN_SP = "SP";
        public const string COLUMN_DataSourceType = "DataSourceType";
        public const string COLUMN_ReconId = "Recon_Id";
        public const string COLUMN_Summary = "Summary";
        public const string COLUMN_ISIncluded = "IsIncluded";
        public const string NAME_ExceptionReport = "NirvanaExceptionReport";

        //public const string TABLENAME_PBPosition = "PBPositionMasterColumns";
        //public const string TABLENAME_PBTrn = "PBTrnMasterColumns";

    }
}
