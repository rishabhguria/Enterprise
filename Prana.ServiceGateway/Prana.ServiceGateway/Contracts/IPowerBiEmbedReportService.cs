using Prana.ServiceGateway.Models.RequestDto;

namespace Prana.ServiceGateway.Contracts
{
    public interface IPowerBiEmbedReportService
    {
        Task<PowerBIDto> GetPowerBIReportEmbedInfo(string workspaceId, string reportId);
        Task<string> GetAccessToken(string clientId, string clientSecret);
    }
}