namespace Prana.ServiceGateway.Models.RequestDto
{
    public class PowerBIDto
    {
        public string ReportId { get; set; } = string.Empty;
        public string EmbedUrl { get; set; } = string.Empty;
        public string EmbedAccessToken { get; set; } = string.Empty;
        public string TokenExpiration { get; set; } = string.Empty;
    }
}