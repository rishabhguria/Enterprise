
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSBatch
{
    /// <summary>
    /// Invoke Builder
    /// </summary>
    /// <remarks></remarks>
    public class InvokeBuilder : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static List<InvokeCommand> cmds = new List<InvokeCommand>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public InvokeBuilder()
        {

            cmds.Add(new InvokeCommand()
            {
                DataType = "# Down Days",
                CSFunction = "Functions.CountUpDownDays",
                Direction = "Down",
                Constant = 0,
                SQLFunction = "db.F_UpDownDays",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "# Up Days",
                CSFunction = "Functions.CountUpDownDays",
                Direction = "Up",
                Constant = 0,
                SQLFunction = "db.F_UpDownDays",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "# Long Symbols",
                CSFunction = "Functions.CountLongShortSymbol",
                Direction = "Long",
                Constant = 0,
                SQLFunction = "db.F_D_countLongShortSymbol",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "# Short Symbols",
                CSFunction = "Functions.CountLongShortSymbol",
                Direction = "Short",
                Constant = 0,
                SQLFunction = "db.F_D_countLongShortSymbol",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Average Daily long P&L",
                CSFunction = "Functions.GetLongShortDailyPnlAverage",
                Direction = "Long",
                Constant = 0,
                SQLFunction = "db.F_LongShortDailyPnlAvg",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Average Daily P&L",
                CSFunction = "Functions.GetLongShortDailyPnlAverage",
                Direction = null,
                Constant = 0,
                SQLFunction = "db.F_LongShortDailyPnlAvg",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Average Daily short P&L",
                CSFunction = "Functions.GetLongShortDailyPnlAverage",
                Direction = "Short",
                Constant = 0,
                SQLFunction = "db.F_LongShortDailyPnlAvg",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Cash",
                CSFunction = "Functions.GetCashValue",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_D_getCash",
                ParamFlag = 14
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Contribution",
                CSFunction = "Functions.GetContribution",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_getContribution",
                ParamFlag = 15
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Gross Beta Exposure",
                CSFunction = "Functions.GetLongShortBetaExposure",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_D_LongShortBetaExposure",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Gross Delta Exposure",
                CSFunction = "Functions.GetLongShortDeltaExposure",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_D_LongShortDeltaExposure",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Gross Market Value",
                CSFunction = "Functions.GetLongShortEndingMarketValue",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_D_getLongShortEMV",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Historical VaR at 95 CI",
                CSFunction = "Functions.GetHistoricalValueAtRisk",
                Direction = "",
                Constant = 95,
                SQLFunction = "db.F_historicalVaR",
                ParamFlag = 47
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Largest Day Gain",
                CSFunction = "Functions.GetLargestDayGainLoss",
                Direction = "Gain",
                Constant = 0,
                SQLFunction = "db.F_getLargestDayGainLoss",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Largest Day Loss",
                CSFunction = "Functions.GetLargestDayGainLoss",
                Direction = "Loss",
                Constant = 0,
                SQLFunction = "db.F_getLargestDayGainLoss",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Long Beta Exposure",
                CSFunction = "Functions.GetLongShortBetaExposure",
                Direction = "Long",
                Constant = 0,
                SQLFunction = "db.F_D_LongShortBetaExposure",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Short Beta Exposure",
                CSFunction = "Functions.GetLongShortBetaExposure",
                Direction = "Short",
                Constant = 0,
                SQLFunction = "db.F_D_LongShortBetaExposure",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Long Delta Exposure",
                CSFunction = "Functions.GetLongShortDeltaExposure",
                Direction = "Long",
                Constant = 0,
                SQLFunction = "db.F_D_LongShortDeltaExposure",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Short Delta Exposure",
                CSFunction = "Functions.GetLongShortDeltaExposure",
                Direction = "Short",
                Constant = 0,
                SQLFunction = "db.F_D_LongShortDeltaExposure",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Long Market Value",
                CSFunction = "Functions.GetLongShortEndingMarketValue",
                Direction = "Long",
                Constant = 0,
                SQLFunction = "db.F_D_getLongShortEMV",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Short Market Value",
                CSFunction = "Functions.GetLongShortEndingMarketValue",
                Direction = "Short",
                Constant = 0,
                SQLFunction = "db.F_D_getLongShortEMV",
                ParamFlag = 30
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Long P&L",
                CSFunction = "Functions.GetLongShortProfitLoss",
                Direction = "Long",
                Constant = 0,
                SQLFunction = "db.F_getLongShortPNL",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Short P&L",
                CSFunction = "Functions.GetLongShortProfitLoss",
                Direction = "Short",
                Constant = 0,
                SQLFunction = "db.F_getLongShortPNL",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Linked Return",
                CSFunction = "Functions.GetLinkedModDietz",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_getLinkedMDReturn",
                ParamFlag = 15
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Modified Dietz Return",
                CSFunction = "Functions.GetModDietzReturn",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_getMDReturn",
                ParamFlag = 15
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "NAV",
                CSFunction = "Functions.GetNetAssetValue",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_D_getNAV",
                ParamFlag = 14
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Net Beta Exposure",
                CSFunction = "Functions.GetNetBetaExposure",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_D_BetaExposure",
                ParamFlag = 14
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Net Delta Exposure",
                CSFunction = "Functions.GetNetDeltaExposure",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_D_DeltaExposure",
                ParamFlag = 14
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Net Market Value",
                CSFunction = "Functions.GetNetEndingMarketValue",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_D_getEMV",
                ParamFlag = 14
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "P&L",
                CSFunction = "Functions.GetProfitLoss",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_getPNL",
                ParamFlag = 15
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Sample Standard Deviation of Daily Long P&L",
                CSFunction = "Functions.GetProfitLossStdDev",
                Direction = "Long",
                Constant = 0,
                SQLFunction = "db.F_LongShortDailyPnlStdDev",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Sample Standard Deviation of Daily P&L",
                CSFunction = "Functions.GetProfitLossStdDev",
                Direction = null,
                Constant = 0,
                SQLFunction = "db.F_LongShortDailyPnlStdDev",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Sample Standard Deviation of Daily Short P&L",
                CSFunction = "Functions.GetProfitLossStdDev",
                Direction = "Short",
                Constant = 0,
                SQLFunction = "db.F_LongShortDailyPnlStdDev",
                ParamFlag = 31
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Top 10 Long Symbols MV",
                CSFunction = "Functions.GetTopNLongShortValueBySymbol",
                Direction = "Long",
                Constant = 10,
                SQLFunction = "db.F_D_TopNLongShortSymbolsMV",
                ParamFlag = 94
            });

            cmds.Add(new InvokeCommand()
            {
                DataType = "Top 10 Losing Symbols P&L",
                CSFunction = "Functions.GetTopNProfitLossBySymbol",
                Direction = "Loser",
                Constant = 10,
                SQLFunction = "db.F_TopNSymbolPNL",
                ParamFlag = 95
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Top 10 Short Symbols MV",
                CSFunction = "Functions.GetTopNLongShortValueBySymbol",
                Direction = "Short",
                Constant = 10,
                SQLFunction = "db.F_D_TopNLongShortSymbolsMV",
                ParamFlag = 94
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Top 10 Winning Symbols P&L",
                CSFunction = "Functions.GetTopNProfitLossBySymbol",
                Direction = "Winner",
                Constant = 10,
                SQLFunction = "db.F_TopNSymbolPNL",
                ParamFlag = 95
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Top 5 Long Symbols MV",
                CSFunction = "Functions.GetTopNLongShortValueBySymbol",
                Direction = "Long",
                Constant = 5,
                SQLFunction = "db.F_D_TopNLongShortSymbolsMV",
                ParamFlag = 94
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Top 5 Short Symbols MV",
                CSFunction = "Functions.GetTopNLongShortValueBySymbol",
                Direction = "Short",
                Constant = 5,
                SQLFunction = "db.F_D_TopNLongShortSymbolsMV",
                ParamFlag = 94
            });
            cmds.Add(new InvokeCommand()
            {
                DataType = "Win/Loss Ratio",
                CSFunction = "Functions.GetWinLossRatio",
                Direction = "",
                Constant = 0,
                SQLFunction = "db.F_winLossRatio",
                ParamFlag = 15
            });



        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            cmds.Clear();
        }
    }
}
