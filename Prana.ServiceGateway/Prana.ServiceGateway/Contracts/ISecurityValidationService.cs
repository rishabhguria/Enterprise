using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.Contracts
{
    public interface ISecurityValidationService
    {
        void Initialize();

        Task SymbolSearch(RequestResponseModel requestResponseObj);

        Task SMSymbolSearch(RequestResponseModel requestResponseObj);

        void SMSaveNewSymbol(RequestResponseModel requestResponseObj);

        Task ValidateSymbolUnifiedAsync(List<string> symbols, int companyUserID, string correlationId, string requestId = "", bool isOptionSymbol = false, string underLyingSymbol = "",int symbology = 0);
    }
}