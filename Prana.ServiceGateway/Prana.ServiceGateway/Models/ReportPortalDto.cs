namespace Prana.ServiceGateway.Models
{
    public class EntitySelectDto
    {
        public int selectedID { get; set; }

    }
    public class SetUserPreferencesDto
    {
        public int idx { get; set; }
    }
    public class SetLayoutUserPreferncesDto
    {
        public string eventKey { get; set; }
    }
    public class GetNewlyApprovedReportsDto
    {
        public string theDate { get; set; }
    }
    public class ReportPortalFileDto
    {
        public long Id { get; set; }
        public string Date { get; set; }
        public string Rp { get; set; }
        public string Format { get; set; }
    }
    public class ZipReportFilesDto
    {
        public List<ReportPortalFileDto> files { get; set; }
    }
    public class DownloadReportsZipDto
    {
        public string guid { get; set; }
    }
    public class DownloadExcelFileDto
    {
        public string id { get; set; }
        public string d { get; set; }
        public string rp { get; set; }
        public string rf { get; set; }

    }

    public class SaveDefaultLayoutDto
    {
        public string companyUserId { get; set; }
        public string groupIds { get; set; }
        public string userName { get; set; }
    }

    public class ReportFilesDto
    {
        public List<ReportsSelectionDto> selectedReports { get; set; }
        public DateTime theDate { get; set; }
    }
    public class ReportsSelectionDto
    {
        public int reportId { get; set; }
        public int groupId { get; set; }
        public string reportName { get; set; }
        public bool isExcel { get; set; }
        public bool isPdf { get; set; }

        public bool check { get; set; }

    }

    public class ReportApprovalLog
    {
        public DateTime UpdateDate { get; set; }
        public string ReportsPDF { get; set; }
        public string ReportsExcel { get; set; }
        public string User { get; set; }
        public string Funds { get; set; }
        public string Message { get; set; }
        public string Updated
        {
            get
            {
                return UpdateDate != DateTime.MinValue ? UpdateDate.ToShortDateString() + " " + UpdateDate.ToString("hh:mm:ss tt") : "";
            }
        }

    }
}
