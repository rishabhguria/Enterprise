namespace Prana.ServiceGateway.Contracts
{
    public interface IWatchlistDataService
    {
        Task AddSymbolInTab(int userId, string symbol, string tabName);

        Task AddTab(int userId, string tabName);

        Task DeleteTab(int userId, string tabName);

        Task<Dictionary<string, int>> GetTabNames(int userId);

        Task<Dictionary<string, HashSet<string>>> GetTabWiseSymbols(int userId);

        Task RemoveSymbolFromTab(int userId, string symbol, string tabName);

        Task RenameTab(int userId, string oldTabName, string newTabName);
    }
}