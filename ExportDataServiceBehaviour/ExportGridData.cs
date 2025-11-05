using ExportGridsData;
using Prana.Blotter;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prana.Rebalancer;

namespace ExportDataServiceBehaviour
{
    public class ExportGridData : IExportGridData
    {
        public const string Const_BlotterMain = "BlotterMain";
        private const string Const_Rebalancer = "RebalancerWindow";
        private const string Const_AllocationClientWindow = "AllocationClientWindow";
        private const string Const_UpdatePreferenceWindow = "UpdatePreferenceWindow";
        private const string Const_EditAllocationPreferenceWindow = "EditAllocationPreferenceWindow";
        private const string Const_ModelPortfolioWindow = "ModelPortfolioGrid";
        private const string Const_RASImportWindow = "RASImportWindow";
        private const string Const_TradeBuySellListView = "TradeBuySellListView";
        private const string Const_CloseTrade = "CloseTrade";
        private const string Const_TT = "AccountQty";
        private const string Const_MTT = "MultiTradingTicket";
        private const string Const_PM = "PM";
        private const string Const_BlotterReports = "BlotterReports";
        private const string Const_ComplianceEngine = "ComplianceEngine";
        private const string Const_DataGrid = "DataGrid";
        private const string Const_SymbolLookUp = "SymbolLookUp";
        private const string Const_ValidAUECs = "ValidAUECs";
        private const string Const_AUECMappingUI = "AUECMappingUI";
        private const string Const_MarkPriceAndForexConversion = "MarkPriceAndForexConversion";
        private const string Const_WatchListMain = "WatchListMain";
        private const string Const_TradingRulesViolatedPopUp = "TradingRulesViolatedPopUp";
        private const string Const_CreatePosition = "CreatePosition";
        private const string Const_ShortLocate = "ShortLocate";
        private const string Const_ImportData = "ImportData";
        private const string Const_ImportPositionsDisplayForm = "ImportPositionsDisplayForm";
        private const string Const_OptionModelInputs = "OptionModelInputs";
        private const string Const_frmThirdParty = "frmThirdParty";
        private const string Const_CalculatedValues = "CalculatedValues";
        private const string Const_AllocationDetailsGrid = "AllocationDetailsGrid";
        private const string Const_AccountStrategyGrid = "AccountStrategyGrid";
        private const string Const_GrdClosingMethod = "grdClosingMethod";
        private const string Const_grdBrokerWiseSettings = "grdBrokerWiseSettings";
        private const string Const_grdAssetSide = "grdAssetSide";
        // private const string Const_ShortLocate = "";
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                if (gridName == Const_AccountStrategyGrid)
                {
                    Type typeOfInstance = typeof(Prana.Allocation.Client.Controls.Preferences.ViewModels.PreferenceAccountStrategyControlViewModel);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_BlotterMain)
                {
                    Type typeOfInstance = typeof(BlotterMain);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (gridName == Const_DataGrid)
                {
                    Type typeOfInstance = typeof(Prana.Rebalancer.RebalancerNew.ViewModels.DataPreferencesViewModel);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (gridName == Const_ModelPortfolioWindow)
                {
                    Type typeOfInstance = typeof(Prana.Rebalancer.RebalancerNew.ViewModels.ModelPortfolioViewModel);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_Rebalancer || WindowName == Const_RASImportWindow || WindowName == Const_TradeBuySellListView)
                {
                    Type typeOfInstance = typeof(Prana.Rebalancer.RebalancerNew.ViewModels.RebalancerViewModel);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_EditAllocationPreferenceWindow || WindowName == Const_AllocationClientWindow || WindowName == Const_UpdatePreferenceWindow)
                {
                    Type typeOfInstance = typeof(Prana.Allocation.Client.Forms.ViewModels.AllocationClientViewModel);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_PM)
                {
                    Type typeOfInstance = typeof(Prana.PM.Client.UI.Controls.CtrlMainConsolidationView);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_CloseTrade)
                {
                    Type typeOfInstance = typeof(Prana.PM.Client.UI.Forms.CloseTrade);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_MTT)
                {
                    Type typeOfInstance = typeof(Prana.TradingTicket.Forms.MultiTradingTicket);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_TT)
                {
                    Type typeOfInstance = typeof(Prana.TradingTicket.Forms.AccountQty);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_BlotterReports)
                {
                    Type typeOfInstance = typeof(Prana.Blotter.BlotterReports);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_ComplianceEngine)
                {
                    Type typeOfInstance = typeof(Prana.ComplianceEngine.ComplianceEngine);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_SymbolLookUp || WindowName == Const_AUECMappingUI)
                {
                    Type typeOfInstance = typeof(Prana.Tools.SymbolLookUp);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_ValidAUECs)
                {
                    Type typeOfInstance = typeof(Prana.Tools.ValidAUECs);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_MarkPriceAndForexConversion)
                {
                    Type typeOfInstance = typeof(Prana.PM.Client.UI.Forms.MarkPriceAndForexConversion);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_WatchListMain)
                {
                    Type typeOfInstance = typeof(Prana.WatchList.Forms.WatchListMain);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_TradingRulesViolatedPopUp)
                {
                    Type typeOfInstance = typeof(Prana.TradeManager.Forms.TradingRulesViolatedPopUp);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_CreatePosition)
                {
                    Type typeOfInstance = typeof(Prana.PM.Client.UI.Forms.CreatePosition);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_ShortLocate)
                {
                    Type typeOfInstance = typeof(Prana.ShortLocate.ShortLocate);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_ImportData || WindowName == Const_ImportPositionsDisplayForm)
                {
                    Type typeOfInstance = typeof(Prana.PM.Client.UI.Forms.ImportData);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_OptionModelInputs)
                {
                    Type typeOfInstance = typeof(Prana.Tools.OptionModelInputs);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (WindowName == Const_frmThirdParty)
                {
                    Type typeOfInstance = typeof(Prana.ThirdPartyUI.frmThirdParty);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (gridName == Const_CalculatedValues)
                {
                    Type typeOfInstance = typeof(Prana.Rebalancer.PercentTradingTool.ViewModel.PercentTradingToolViewModel);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (gridName == Const_AllocationDetailsGrid)
                {
                    Type typeOfInstance = typeof(Prana.Rebalancer.PercentTradingTool.ViewModel.ViewAllocationDetailsViewModel);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (gridName == Const_GrdClosingMethod)
                {
                    Type typeOfInstance = typeof(Prana.ClientCommon.ClosingPreferencesUsrCtrl);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
                else if (gridName == Const_grdBrokerWiseSettings || gridName == Const_grdAssetSide)
                {
                    Type typeOfInstance = typeof(Prana.TradingTicket.TicketPreferenceControl);
                    var registeredIns = InstanceManager.GetInstance<IExportGridData>(typeOfInstance);
                    if (registeredIns != null)
                        registeredIns.ExportData(gridName, WindowName, tabName, filePath);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
    }
}
