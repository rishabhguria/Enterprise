using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.Tools
{
    partial class SymbolLookUp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            UnwireEvents();
            _secMasterSyncService.Dispose();
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (templateViewUI != null)
                {
                    templateViewUI.Dispose();
                }
                if (_secMasterSyncService != null)
                {
                    _secMasterSyncService.Dispose();
                }
                if (frmAccountWiseUDA != null)
                {
                    frmAccountWiseUDA.Dispose();
                }
                if(_customColumnChooserDialog != null)
                {
                    _customColumnChooserDialog.Dispose();
                }
                if(accountWiseUDA != null)
                {
                    accountWiseUDA.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance118 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance108 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance109 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance110 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance111 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance112 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance113 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance114 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance115 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance116 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance117 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance119 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance120 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance121 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance122 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance123 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance124 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance125 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance126 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance127 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance128 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance129 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance130 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance131 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance132 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance133 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance134 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance135 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance136 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance137 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance138 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance139 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance140 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance141 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance142 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance143 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance144 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance145 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance146 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance147 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance148 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance149 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance150 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance151 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance152 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance153 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance154 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance155 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance156 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance157 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance158 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance159 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance160 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance161 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance162 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance163 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance164 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance165 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance166 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance167 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance168 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance169 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance170 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance171 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Advanced Search");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Advanced Search", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add Security");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool55 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OTC Template");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool56 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Instrument Type Fields");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Export To Excel");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Layout");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Auto Clear Data On Search");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Validate Symbol From Live Feed");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Edit UDA");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Future Root Data");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AUEC Mappings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Account Wise UDA");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DYNAMIC_UDA");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add Security");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool65 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OTC Template");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool66 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Instrument Type Fields");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Advanced Search", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Export To Excel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Validate Symbol From Live Feed");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Edit UDA");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Future Root Data");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool1");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Auto Clear Data On Search");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Clear", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Clear", "");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Layout");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save Layout");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Clear Layout");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Clear Layout");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save Layout");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AUEC Mappings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Account Wise UDA");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DYNAMIC_UDA");
            Infragistics.Win.Appearance appearance172 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance173 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance174 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance175 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance176 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance177 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance178 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance179 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance180 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance181 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance182 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance183 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolLookUp));
            this.tabBasicDetails = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.gbOptions = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblStrickPrices = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcStrikePrice = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.ugpcPutOrCal = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblPutorCall = new Infragistics.Win.Misc.UltraLabel();
            this.gbSymbology = new Infragistics.Win.Misc.UltraGroupBox();
            this.ugpcMultiplier = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.ultraMultiplier = new Infragistics.Win.Misc.UltraLabel();
            this.lblCurrency = new Infragistics.Win.Misc.UltraLabel();
            this.ugcpCurrencyID = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblExpirationDateTemplate = new Infragistics.Win.Misc.UltraLabel();
            this.ugcpExpirationDate = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblProxySymbol = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcLongName = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblDescription = new Infragistics.Win.Misc.UltraLabel();
            this.ugcpUnderLyingSymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.ugpcProxySymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblBloombergSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcBloombergSymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblBloombergSymbolExCode = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcBloombergSymbolExCode = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblUnderlyingSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.lblTickerSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.ugcpTickerSymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblFixedDateTemplate = new Infragistics.Win.Misc.UltraLabel();
            this.ugcpFixedDate = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.gbBasicDetails = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblAssetId = new Infragistics.Win.Misc.UltraLabel();
            this.ugcpAssetClass = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblUnderlying = new Infragistics.Win.Misc.UltraLabel();
            this.ugcpExchangeID = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblExchange = new Infragistics.Win.Misc.UltraLabel();
            this.ugcpUnderLyingID = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.btnSelectAUEC = new Infragistics.Win.Misc.UltraButton();
            this.gbFuture = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnEditRootData = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcCutOffTime = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.gbfx = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblVSCurrencyValue = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblLeadCurrencyValue = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGridCellProxy4 = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.gbFixedIncome = new Infragistics.Win.Misc.UltraGroupBox();
            this.ugpcCollateralType = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblCollateralType = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcSecurityTypeID = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblSecurityTypeID = new Infragistics.Win.Misc.UltraLabel();
            this.lblDaysToSettlement = new Infragistics.Win.Misc.UltraLabel();
            this.lblCoupon = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcDaysToSettlement = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.ugpcIsZero = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblIsZero = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcCoupon = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblCouponFrequencyID = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcCouponFrequencyID = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblFirstCouponDate = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcFirstCouponDate = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblIssueDate = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcIssueDate = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblAccrualBasisID = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcAccrualBasisID = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.tabAssetSpecific = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.gbPriceInfo = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblStrikePriceMultiplier = new Infragistics.Win.Misc.UltraLabel();
            this.ugcpStrikePriceMultiplier = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblDelta = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcDelta = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblRoundLot = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcRoundLot = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.gbSecDates = new Infragistics.Win.Misc.UltraGroupBox();
            this.ugpcCreationDate = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblCreationDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblModifiedDate = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcModifiedDate = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcApprovalDate = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.gbSecurityInfo = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraGridCellProxy5 = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblBloombergOptionRoot = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcEsignalOptionRoot = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblESignalOptionRoot = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcIsSecApproved = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblApprovalStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcISINSymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblISINSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcIDCOOptionSymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblIDCOOptionSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcComments = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblComments = new Infragistics.Win.Misc.UltraLabel();
            this.lblFactSetSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcFactSetSymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblActivSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcActivSymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblSedolSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcSedolSymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblCusipSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcOSIOptionSymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblOSIOptionSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcCusipSymbol = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.tabUDADetails = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.gbUDADetails = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblUseDefaultUDA = new Infragistics.Win.Misc.UltraLabel();
            this.gcpcUseDefaultUDA = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.ugpcUDACountry = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.btnUDAUI = new Infragistics.Win.Misc.UltraButton();
            this.ugpcUDASubSector = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblUDAAssetClass = new Infragistics.Win.Misc.UltraLabel();
            this.lblSubSector = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcAssetID = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.ugpcUDASector = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblUDACountry = new Infragistics.Win.Misc.UltraLabel();
            this.lblUDASector = new Infragistics.Win.Misc.UltraLabel();
            this.ugcpUDASecurity = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblSecType = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.btnPreTab = new Infragistics.Win.Misc.UltraButton();
            this.btnEditSecurity = new Infragistics.Win.Misc.UltraButton();
            this.btnNextTab = new Infragistics.Win.Misc.UltraButton();
            this.lblPageDetail = new Infragistics.Win.Misc.UltraLabel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.tabDynamicUDA = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.gbDynamicUDA = new Infragistics.Win.Misc.UltraGroupBox();
            this.ctrlDynamicUDASymbol1 = new Prana.Tools.PL.SecMaster.ctrlDynamicUDASymbol();
            this.ultraGridCellProxy1 = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.ultraGridCellProxy2 = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblStatus = new Infragistics.Win.Misc.UltraLabel();
            this.SymbolLookUp_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTraded = new Infragistics.Win.Misc.UltraLabel();
            this.lblSearch = new Infragistics.Win.Misc.UltraLabel();
            this.cmbbxSMView = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.rdBtnHistSymbols = new System.Windows.Forms.RadioButton();
            this.rdBtnOpenSymbols = new System.Windows.Forms.RadioButton();
            this.rdBtnSearchSymbols = new System.Windows.Forms.RadioButton();
            this.cmbbxSearchCriteria = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.chkbxUnApprovedSec = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.cmbbxMatchOn = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtbxInput = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnGetData = new Infragistics.Win.Misc.UltraButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.grdRowEditTemplate = new Infragistics.Win.UltraWinGrid.UltraGridRowEditTemplate();
            this.tabCntrlSecurity = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.grdData = new PranaUltraGrid();
            this.mnuSymbolLookup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.approveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAUECToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tradeSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPricingInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPrev = new Infragistics.Win.Misc.UltraButton();
            this.btnNext = new Infragistics.Win.Misc.UltraButton();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.toolBarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.symbolLookupUltraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.ugpcVsCurrency = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblVsCurrency = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcLeadCurrencyID = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblLeadCurrencyID = new Infragistics.Win.Misc.UltraLabel();
            this.lblIsNDF = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcIsNDF = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblStrickPrice = new Infragistics.Win.Misc.UltraLabel();
            this.upgcStrikePrice = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.ugpcPutOrCall = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.lblPutCall = new Infragistics.Win.Misc.UltraLabel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.chkBxIsFullSearch = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._SymbolLookUp_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SymbolLookUp_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SymbolLookUp_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.lblShares = new Infragistics.Win.Misc.UltraLabel();
            this.ugpcShares = new Infragistics.Win.UltraWinGrid.UltraGridCellProxy();
            this.cbActivSymbolCamelCase = new System.Windows.Forms.CheckBox();
            this.tabBasicDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbOptions)).BeginInit();
            this.gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbSymbology)).BeginInit();
            this.gbSymbology.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbBasicDetails)).BeginInit();
            this.gbBasicDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFuture)).BeginInit();
            this.gbFuture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbfx)).BeginInit();
            this.gbfx.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFixedIncome)).BeginInit();
            this.gbFixedIncome.SuspendLayout();
            this.tabAssetSpecific.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbPriceInfo)).BeginInit();
            this.gbPriceInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbSecDates)).BeginInit();
            this.gbSecDates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbSecurityInfo)).BeginInit();
            this.gbSecurityInfo.SuspendLayout();
            this.tabUDADetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbUDADetails)).BeginInit();
            this.gbUDADetails.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.tabDynamicUDA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbDynamicUDA)).BeginInit();
            this.gbDynamicUDA.SuspendLayout();
            this.SymbolLookUp_Fill_Panel.ClientArea.SuspendLayout();
            this.SymbolLookUp_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxSMView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxSearchCriteria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxUnApprovedSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxMatchOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtbxInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRowEditTemplate)).BeginInit();
            this.grdRowEditTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCntrlSecurity)).BeginInit();
            this.tabCntrlSecurity.SuspendLayout();
            this.ultraTabSharedControlsPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.mnuSymbolLookup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolBarManager)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBxIsFullSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabBasicDetails
            // 
            this.tabBasicDetails.Controls.Add(this.gbOptions);
            this.tabBasicDetails.Controls.Add(this.gbSymbology);
            this.tabBasicDetails.Controls.Add(this.gbBasicDetails);
            this.tabBasicDetails.Controls.Add(this.gbFuture);
            this.tabBasicDetails.Controls.Add(this.gbfx);
            this.tabBasicDetails.Controls.Add(this.gbFixedIncome);
            this.tabBasicDetails.Controls.Add(this.ultraPanel1);
            this.tabBasicDetails.Location = new System.Drawing.Point(0, 0);
            this.tabBasicDetails.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabBasicDetails.Name = "tabBasicDetails";
            this.tabBasicDetails.Size = new System.Drawing.Size(877, 345);
            // 
            // gbOptions
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.gbOptions.ContentAreaAppearance = appearance1;
            this.gbOptions.Controls.Add(this.lblStrickPrices);
            this.gbOptions.Controls.Add(this.ugpcStrikePrice);
            this.gbOptions.Controls.Add(this.ugpcPutOrCal);
            this.gbOptions.Controls.Add(this.lblPutorCall);
            this.gbOptions.ForeColor = System.Drawing.Color.White;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.Gainsboro;
            this.gbOptions.HeaderAppearance = appearance6;
            this.gbOptions.Location = new System.Drawing.Point(5, 207);
            this.gbOptions.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(853, 93);
            this.gbOptions.TabIndex = 12;
            this.gbOptions.Tag = "";
            this.gbOptions.Text = "Options";
            this.gbOptions.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // lblStrickPrices
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.lblStrickPrices.Appearance = appearance2;
            this.lblStrickPrices.AutoSize = true;
            this.lblStrickPrices.Location = new System.Drawing.Point(191, 33);
            this.lblStrickPrices.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblStrickPrices.Name = "lblStrickPrices";
            this.lblStrickPrices.Size = new System.Drawing.Size(73, 18);
            this.lblStrickPrices.TabIndex = 64;
            this.lblStrickPrices.Tag = "Strike Price";
            this.lblStrickPrices.Text = "Strike Price*:";
            // 
            // ugpcStrikePrice
            // 
            this.ugpcStrikePrice.ColumnKey = "StrikePrice";
            appearance3.ForeColorDisabled = System.Drawing.Color.Black;
            this.ugpcStrikePrice.EditAppearance = appearance3;
            this.ugpcStrikePrice.Location = new System.Drawing.Point(287, 30);
            this.ugpcStrikePrice.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcStrikePrice.Name = "ugpcStrikePrice";
            this.ugpcStrikePrice.Size = new System.Drawing.Size(87, 24);
            this.ugpcStrikePrice.TabIndex = 13;
            this.ugpcStrikePrice.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcStrikePrice.Text = "Band or column does not exist.";
            // 
            // ugpcPutOrCal
            // 
            this.ugpcPutOrCal.ColumnKey = "PutOrCall";
            appearance4.ForeColorDisabled = System.Drawing.Color.Black;
            this.ugpcPutOrCal.EditAppearance = appearance4;
            this.ugpcPutOrCal.Location = new System.Drawing.Point(85, 29);
            this.ugpcPutOrCal.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcPutOrCal.Name = "ugpcPutOrCal";
            this.ugpcPutOrCal.Size = new System.Drawing.Size(87, 24);
            this.ugpcPutOrCal.TabIndex = 12;
            this.ugpcPutOrCal.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcPutOrCal.Text = "Band or column does not exist.";
            // 
            // lblPutorCall
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            this.lblPutorCall.Appearance = appearance5;
            this.lblPutorCall.AutoSize = true;
            this.lblPutorCall.Location = new System.Drawing.Point(10, 33);
            this.lblPutorCall.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblPutorCall.Name = "lblPutorCall";
            this.lblPutorCall.Size = new System.Drawing.Size(55, 18);
            this.lblPutorCall.TabIndex = 63;
            this.lblPutorCall.Tag = "Put/Call";
            this.lblPutorCall.Text = "Put/Call*:";
            // 
            // gbSymbology
            // 
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.gbSymbology.ContentAreaAppearance = appearance7;
            this.gbSymbology.Controls.Add(this.ugpcMultiplier);
            this.gbSymbology.Controls.Add(this.ultraMultiplier);
            this.gbSymbology.Controls.Add(this.lblCurrency);
            this.gbSymbology.Controls.Add(this.ugcpCurrencyID);
            this.gbSymbology.Controls.Add(this.lblExpirationDateTemplate);
            this.gbSymbology.Controls.Add(this.ugcpExpirationDate);
            this.gbSymbology.Controls.Add(this.lblProxySymbol);
            this.gbSymbology.Controls.Add(this.ugpcLongName);
            this.gbSymbology.Controls.Add(this.lblDescription);
            this.gbSymbology.Controls.Add(this.ugcpUnderLyingSymbol);
            this.gbSymbology.Controls.Add(this.ugpcProxySymbol);
            this.gbSymbology.Controls.Add(this.lblBloombergSymbol);
            this.gbSymbology.Controls.Add(this.ugpcBloombergSymbol);
            this.gbSymbology.Controls.Add(this.lblUnderlyingSymbol);
            this.gbSymbology.Controls.Add(this.lblTickerSymbol);
            this.gbSymbology.Controls.Add(this.ugcpTickerSymbol);
            this.gbSymbology.Controls.Add(this.lblFixedDateTemplate);
            this.gbSymbology.Controls.Add(this.ugcpFixedDate);
            this.gbSymbology.ForeColor = System.Drawing.Color.White;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.Color.Gainsboro;
            this.gbSymbology.HeaderAppearance = appearance26;
            this.gbSymbology.Location = new System.Drawing.Point(5, 78);
            this.gbSymbology.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbSymbology.Name = "gbSymbology";
            this.gbSymbology.Size = new System.Drawing.Size(853, 130);
            this.gbSymbology.TabIndex = 102;
            this.gbSymbology.Text = "Basic Details";
            this.gbSymbology.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // ugpcMultiplier
            // 
            this.ugpcMultiplier.ColumnKey = "Multiplier";
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.ugpcMultiplier.EditAppearance = appearance8;
            this.ugpcMultiplier.Location = new System.Drawing.Point(722, 24);
            this.ugpcMultiplier.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcMultiplier.Name = "ugpcMultiplier";
            this.ugpcMultiplier.Size = new System.Drawing.Size(122, 24);
            this.ugpcMultiplier.TabIndex = 6;
            this.ugpcMultiplier.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcMultiplier.Text = "Band or column does not exist.";
            // 
            // ultraMultiplier
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            this.ultraMultiplier.Appearance = appearance9;
            this.ultraMultiplier.AutoSize = true;
            this.ultraMultiplier.Location = new System.Drawing.Point(597, 30);
            this.ultraMultiplier.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ultraMultiplier.Name = "ultraMultiplier";
            this.ultraMultiplier.Size = new System.Drawing.Size(65, 18);
            this.ultraMultiplier.TabIndex = 122;
            this.ultraMultiplier.Tag = "Multiplier";
            this.ultraMultiplier.Text = "Multiplier*:";
            // 
            // lblCurrency
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrency.Appearance = appearance10;
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Location = new System.Drawing.Point(315, 30);
            this.lblCurrency.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(61, 18);
            this.lblCurrency.TabIndex = 111;
            this.lblCurrency.Tag = "Currency";
            this.lblCurrency.Text = "Currency*:";
            // 
            // ugcpCurrencyID
            // 
            this.ugcpCurrencyID.ColumnKey = "CurrencyID";
            appearance11.ForeColor = System.Drawing.Color.Black;
            this.ugcpCurrencyID.EditAppearance = appearance11;
            this.ugcpCurrencyID.Location = new System.Drawing.Point(445, 24);
            this.ugcpCurrencyID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugcpCurrencyID.Name = "ugcpCurrencyID";
            this.ugcpCurrencyID.Size = new System.Drawing.Size(125, 24);
            this.ugcpCurrencyID.TabIndex = 5;
            this.ugcpCurrencyID.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugcpCurrencyID.Text = "Band or column does not exist.";
            // 
            // lblExpirationDateTemplate
            // 
            appearance12.BackColor = System.Drawing.Color.Transparent;
            this.lblExpirationDateTemplate.Appearance = appearance12;
            this.lblExpirationDateTemplate.AutoSize = true;
            this.lblExpirationDateTemplate.Location = new System.Drawing.Point(595, 100);
            this.lblExpirationDateTemplate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblExpirationDateTemplate.Name = "lblExpirationDateTemplate";
            this.lblExpirationDateTemplate.Size = new System.Drawing.Size(96, 18);
            this.lblExpirationDateTemplate.TabIndex = 109;
            this.lblExpirationDateTemplate.Tag = "Expiration Date";
            this.lblExpirationDateTemplate.Text = "Expiration Date*:";
            // 
            // ugcpExpirationDate
            // 
            this.ugcpExpirationDate.ColumnKey = "ExpirationDate";
            appearance13.ForeColor = System.Drawing.Color.Black;
            this.ugcpExpirationDate.EditAppearance = appearance13;
            this.ugcpExpirationDate.Location = new System.Drawing.Point(722, 95);
            this.ugcpExpirationDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugcpExpirationDate.Name = "ugcpExpirationDate";
            this.ugcpExpirationDate.Size = new System.Drawing.Size(122, 24);
            this.ugcpExpirationDate.TabIndex = 12;
            this.ugcpExpirationDate.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugcpExpirationDate.Text = "Band or column does not exist.";
            // 
            // lblProxySymbol
            // 
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.lblProxySymbol.Appearance = appearance14;
            this.lblProxySymbol.AutoSize = true;
            this.lblProxySymbol.Location = new System.Drawing.Point(595, 63);
            this.lblProxySymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblProxySymbol.Name = "lblProxySymbol";
            this.lblProxySymbol.Size = new System.Drawing.Size(81, 18);
            this.lblProxySymbol.TabIndex = 108;
            this.lblProxySymbol.Tag = "ProxySymbol";
            this.lblProxySymbol.Text = "Proxy Symbol:";
            // 
            // ugpcLongName
            // 
            this.ugpcLongName.ColumnKey = "LongName";
            appearance15.ForeColor = System.Drawing.Color.Black;
            this.ugpcLongName.EditAppearance = appearance15;
            this.ugpcLongName.Location = new System.Drawing.Point(155, 97);
            this.ugpcLongName.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcLongName.Name = "ugpcLongName";
            this.ugpcLongName.Size = new System.Drawing.Size(416, 24);
            this.ugpcLongName.TabIndex = 11;
            this.ugpcLongName.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcLongName.Text = "Band or column does not exist.";
            // 
            // lblDescription
            // 
            appearance16.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Appearance = appearance16;
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(10, 100);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(74, 18);
            this.lblDescription.TabIndex = 107;
            this.lblDescription.Tag = "Description";
            this.lblDescription.Text = "Description*:";
            // 
            // ugcpUnderLyingSymbol
            // 
            this.ugcpUnderLyingSymbol.ColumnKey = "UnderLyingSymbol";
            appearance17.ForeColor = System.Drawing.Color.Black;
            this.ugcpUnderLyingSymbol.EditAppearance = appearance17;
            this.ugcpUnderLyingSymbol.Location = new System.Drawing.Point(155, 60);
            this.ugcpUnderLyingSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugcpUnderLyingSymbol.Name = "ugcpUnderLyingSymbol";
            this.ugcpUnderLyingSymbol.Size = new System.Drawing.Size(125, 24);
            this.ugcpUnderLyingSymbol.TabIndex = 8;
            this.ugcpUnderLyingSymbol.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugcpUnderLyingSymbol.Text = "Band or column does not exist.";
            // 
            // ugpcProxySymbol
            // 
            this.ugpcProxySymbol.ColumnKey = "ProxySymbol";
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.ugpcProxySymbol.EditAppearance = appearance18;
            this.ugpcProxySymbol.Location = new System.Drawing.Point(722, 60);
            this.ugpcProxySymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcProxySymbol.Name = "ugpcProxySymbol";
            this.ugpcProxySymbol.Size = new System.Drawing.Size(122, 24);
            this.ugpcProxySymbol.TabIndex = 10;
            this.ugpcProxySymbol.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcProxySymbol.Text = "Band or column does not exist.";
            // 
            // lblBloombergSymbol
            // 
            appearance19.BackColor = System.Drawing.Color.Transparent;
            this.lblBloombergSymbol.Appearance = appearance19;
            this.lblBloombergSymbol.AutoSize = true;
            this.lblBloombergSymbol.Location = new System.Drawing.Point(315, 59);
            this.lblBloombergSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblBloombergSymbol.Name = "lblBloombergSymbol";
            this.lblBloombergSymbol.Size = new System.Drawing.Size(68, 18);
            this.lblBloombergSymbol.TabIndex = 91;
            this.lblBloombergSymbol.Tag = "BloombergSymbol";
            this.lblBloombergSymbol.Text = "Bloomberg Sym :\n(with Composite Code)";
            // 
            // ugpcBloombergSymbol
            // 
            this.ugpcBloombergSymbol.ColumnKey = "BloombergSymbol";
            appearance20.ForeColor = System.Drawing.Color.Black;
            this.ugpcBloombergSymbol.EditAppearance = appearance20;
            this.ugpcBloombergSymbol.Location = new System.Drawing.Point(445, 60);
            this.ugpcBloombergSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcBloombergSymbol.Name = "ugpcBloombergSymbol";
            this.ugpcBloombergSymbol.Size = new System.Drawing.Size(125, 24);
            this.ugpcBloombergSymbol.TabIndex = 9;
            this.ugpcBloombergSymbol.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcBloombergSymbol.Text = "Band or column does not exist.";
            // 
            // lblBloombergSymbolExCode
            // 
            appearance182.BackColor = System.Drawing.Color.Transparent;
            this.lblBloombergSymbolExCode.Appearance = appearance182;
            this.lblBloombergSymbolExCode.AutoSize = true;
            this.lblBloombergSymbolExCode.Location = new System.Drawing.Point(3, 172);
            this.lblBloombergSymbolExCode.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblBloombergSymbolExCode.Name = "lblBloombergSymbolWithExchangeCode";
            this.lblBloombergSymbolExCode.Size = new System.Drawing.Size(68, 18);
            this.lblBloombergSymbolExCode.TabIndex = 91;
            this.lblBloombergSymbolExCode.Tag = "BloombergSymbolWithExchangeCode";
            this.lblBloombergSymbolExCode.Text = "Bloomberg Sym  :\n(with Exchange Code)";
            // 
            // ugpcBloombergSymbolExCode
            // 
            this.ugpcBloombergSymbolExCode.ColumnKey = "BloombergSymbolWithExchangeCode";
            this.ugpcBloombergSymbolExCode.EditAppearance = appearance20;
            this.ugpcBloombergSymbolExCode.Location = new System.Drawing.Point(124, 172);
            this.ugpcBloombergSymbolExCode.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcBloombergSymbolExCode.Name = "ugpcBloombergSymbolExCode";
            this.ugpcBloombergSymbolExCode.Size = new System.Drawing.Size(135, 24);
            this.ugpcBloombergSymbolExCode.TabIndex = 9;
            this.ugpcBloombergSymbolExCode.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcBloombergSymbolExCode.Text = "Band or column does not exist.";
            // 
            // lblUnderlyingSymbol
            // 
            appearance21.BackColor = System.Drawing.Color.Transparent;
            this.lblUnderlyingSymbol.Appearance = appearance21;
            this.lblUnderlyingSymbol.AutoSize = true;
            this.lblUnderlyingSymbol.Location = new System.Drawing.Point(10, 63);
            this.lblUnderlyingSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblUnderlyingSymbol.Name = "lblUnderlyingSymbol";
            this.lblUnderlyingSymbol.Size = new System.Drawing.Size(116, 18);
            this.lblUnderlyingSymbol.TabIndex = 90;
            this.lblUnderlyingSymbol.Tag = "Underlying Symbol";
            this.lblUnderlyingSymbol.Text = "Underlying Symbol*:";
            // 
            // lblTickerSymbol
            // 
            appearance22.BackColor = System.Drawing.Color.Transparent;
            this.lblTickerSymbol.Appearance = appearance22;
            this.lblTickerSymbol.AutoSize = true;
            this.lblTickerSymbol.Location = new System.Drawing.Point(10, 30);
            this.lblTickerSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblTickerSymbol.Name = "lblTickerSymbol";
            this.lblTickerSymbol.Size = new System.Drawing.Size(88, 18);
            this.lblTickerSymbol.TabIndex = 87;
            this.lblTickerSymbol.Tag = "Ticker Symbol";
            this.lblTickerSymbol.Text = "Ticker Symbol*:";
            // 
            // ugcpTickerSymbol
            // 
            this.ugcpTickerSymbol.ColumnKey = "TickerSymbol";
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.ForeColor = System.Drawing.Color.Black;
            this.ugcpTickerSymbol.EditAppearance = appearance23;
            this.ugcpTickerSymbol.Location = new System.Drawing.Point(155, 24);
            this.ugcpTickerSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugcpTickerSymbol.Name = "ugcpTickerSymbol";
            this.ugcpTickerSymbol.Size = new System.Drawing.Size(125, 24);
            this.ugcpTickerSymbol.TabIndex = 4;
            this.ugcpTickerSymbol.Text = "Band or column does not exist.";
            // 
            // lblFixedDateTemplate
            // 
            appearance24.BackColor = System.Drawing.Color.Transparent;
            this.lblFixedDateTemplate.Appearance = appearance24;
            this.lblFixedDateTemplate.AutoSize = true;
            this.lblFixedDateTemplate.Location = new System.Drawing.Point(595, 93);
            this.lblFixedDateTemplate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblFixedDateTemplate.Name = "lblFixedDateTemplate";
            this.lblFixedDateTemplate.Size = new System.Drawing.Size(68, 18);
            this.lblFixedDateTemplate.TabIndex = 90;
            this.lblFixedDateTemplate.Tag = "Fixing Date";
            this.lblFixedDateTemplate.Text = "Fixing Date:";
            // 
            // ugcpFixedDate
            // 
            this.ugcpFixedDate.ColumnKey = "FixingDate";
            appearance25.ForeColorDisabled = System.Drawing.Color.Black;
            this.ugcpFixedDate.EditAppearance = appearance25;
            this.ugcpFixedDate.Location = new System.Drawing.Point(722, 88);
            this.ugcpFixedDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugcpFixedDate.Name = "ugcpFixedDate";
            this.ugcpFixedDate.Size = new System.Drawing.Size(135, 24);
            this.ugcpFixedDate.TabIndex = 16;
            this.ugcpFixedDate.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugcpFixedDate.Text = "Band or column does not exist.";
            // 
            // gbBasicDetails
            // 
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance27.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.gbBasicDetails.ContentAreaAppearance = appearance27;
            this.gbBasicDetails.Controls.Add(this.lblAssetId);
            this.gbBasicDetails.Controls.Add(this.ugcpAssetClass);
            this.gbBasicDetails.Controls.Add(this.lblUnderlying);
            this.gbBasicDetails.Controls.Add(this.ugcpExchangeID);
            this.gbBasicDetails.Controls.Add(this.lblExchange);
            this.gbBasicDetails.Controls.Add(this.ugcpUnderLyingID);
            this.gbBasicDetails.Controls.Add(this.btnSelectAUEC);
            this.gbBasicDetails.ForeColor = System.Drawing.Color.White;
            appearance35.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance35.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.gbBasicDetails.HeaderAppearance = appearance35;
            this.gbBasicDetails.Location = new System.Drawing.Point(3, 10);
            this.gbBasicDetails.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbBasicDetails.Name = "gbBasicDetails";
            this.gbBasicDetails.Size = new System.Drawing.Size(853, 61);
            this.gbBasicDetails.TabIndex = 36;
            this.gbBasicDetails.Text = "Security Type";
            this.gbBasicDetails.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // lblAssetId
            // 
            appearance28.BackColor = System.Drawing.Color.Transparent;
            this.lblAssetId.Appearance = appearance28;
            this.lblAssetId.AutoSize = true;
            this.lblAssetId.Location = new System.Drawing.Point(10, 32);
            this.lblAssetId.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblAssetId.Name = "lblAssetId";
            this.lblAssetId.Size = new System.Drawing.Size(72, 18);
            this.lblAssetId.TabIndex = 10;
            this.lblAssetId.Tag = "AssetClass";
            this.lblAssetId.Text = "Asset Class*:";
            // 
            // ugcpAssetClass
            // 
            this.ugcpAssetClass.ColumnKey = "AssetID";
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.ForeColor = System.Drawing.Color.Black;
            this.ugcpAssetClass.EditAppearance = appearance29;
            this.ugcpAssetClass.Location = new System.Drawing.Point(108, 27);
            this.ugcpAssetClass.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugcpAssetClass.Name = "ugcpAssetClass";
            this.ugcpAssetClass.Size = new System.Drawing.Size(125, 24);
            this.ugcpAssetClass.TabIndex = 1;
            this.ugcpAssetClass.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugcpAssetClass.Text = "Band or column does not exist.";
            // 
            // lblUnderlying
            // 
            appearance30.BackColor = System.Drawing.Color.Transparent;
            this.lblUnderlying.Appearance = appearance30;
            this.lblUnderlying.AutoSize = true;
            this.lblUnderlying.Location = new System.Drawing.Point(262, 32);
            this.lblUnderlying.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblUnderlying.Name = "lblUnderlying";
            this.lblUnderlying.Size = new System.Drawing.Size(72, 18);
            this.lblUnderlying.TabIndex = 12;
            this.lblUnderlying.Tag = "Underlying";
            this.lblUnderlying.Text = "Underlying*:";
            // 
            // ugcpExchangeID
            // 
            this.ugcpExchangeID.ColumnKey = "ExchangeID";
            appearance31.ForeColor = System.Drawing.Color.Black;
            this.ugcpExchangeID.EditAppearance = appearance31;
            this.ugcpExchangeID.Location = new System.Drawing.Point(591, 27);
            this.ugcpExchangeID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugcpExchangeID.Name = "ugcpExchangeID";
            this.ugcpExchangeID.Size = new System.Drawing.Size(122, 24);
            this.ugcpExchangeID.TabIndex = 3;
            this.ugcpExchangeID.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugcpExchangeID.Text = "Band or column does not exist.";
            // 
            // lblExchange
            // 
            appearance32.BackColor = System.Drawing.Color.Transparent;
            this.lblExchange.Appearance = appearance32;
            this.lblExchange.AutoSize = true;
            this.lblExchange.Location = new System.Drawing.Point(511, 32);
            this.lblExchange.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblExchange.Name = "lblExchange";
            this.lblExchange.Size = new System.Drawing.Size(64, 18);
            this.lblExchange.TabIndex = 14;
            this.lblExchange.Tag = "Exchange";
            this.lblExchange.Text = "Exchange*:";
            // 
            // ugcpUnderLyingID
            // 
            this.ugcpUnderLyingID.ColumnKey = "UnderLyingID";
            appearance33.ForeColor = System.Drawing.Color.Black;
            this.ugcpUnderLyingID.EditAppearance = appearance33;
            this.ugcpUnderLyingID.Location = new System.Drawing.Point(355, 27);
            this.ugcpUnderLyingID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugcpUnderLyingID.Name = "ugcpUnderLyingID";
            this.ugcpUnderLyingID.Size = new System.Drawing.Size(128, 24);
            this.ugcpUnderLyingID.TabIndex = 2;
            this.ugcpUnderLyingID.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugcpUnderLyingID.Text = "Band or column does not exist.";
            // 
            // btnSelectAUEC
            // 
            appearance34.BackColor = System.Drawing.Color.Gray;
            appearance34.ForeColor = System.Drawing.Color.White;
            this.btnSelectAUEC.Appearance = appearance34;
            this.btnSelectAUEC.Location = new System.Drawing.Point(738, 28);
            this.btnSelectAUEC.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnSelectAUEC.Name = "btnSelectAUEC";
            this.btnSelectAUEC.Size = new System.Drawing.Size(107, 24);
            this.btnSelectAUEC.TabIndex = 43;
            this.btnSelectAUEC.Text = "&Edit AUEC";
            this.btnSelectAUEC.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSelectAUEC.Visible = false;
            this.btnSelectAUEC.Click += new System.EventHandler(this.btnAuecs_Click);
            // 
            // gbFuture
            // 
            appearance36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance36.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance36.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.gbFuture.ContentAreaAppearance = appearance36;
            this.gbFuture.Controls.Add(this.btnEditRootData);
            this.gbFuture.Controls.Add(this.ultraLabel4);
            this.gbFuture.Controls.Add(this.ugpcCutOffTime);
            this.gbFuture.ForeColor = System.Drawing.Color.White;
            appearance40.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance40.BackColor2 = System.Drawing.Color.Gainsboro;
            this.gbFuture.HeaderAppearance = appearance40;
            this.gbFuture.Location = new System.Drawing.Point(5, 207);
            this.gbFuture.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbFuture.Name = "gbFuture";
            this.gbFuture.Size = new System.Drawing.Size(853, 93);
            this.gbFuture.TabIndex = 100;
            this.gbFuture.Tag = "";
            this.gbFuture.Text = "Future";
            this.gbFuture.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // btnEditRootData
            // 
            appearance37.BackColor = System.Drawing.Color.Gray;
            appearance37.ForeColor = System.Drawing.Color.White;
            this.btnEditRootData.Appearance = appearance37;
            this.btnEditRootData.Location = new System.Drawing.Point(345, 30);
            this.btnEditRootData.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnEditRootData.Name = "btnEditRootData";
            this.btnEditRootData.Size = new System.Drawing.Size(121, 24);
            this.btnEditRootData.TabIndex = 98;
            this.btnEditRootData.Text = "&Edit Root Data";
            this.btnEditRootData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEditRootData.Click += new System.EventHandler(this.btnRootData_Click);
            // 
            // ultraLabel4
            // 
            appearance38.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel4.Appearance = appearance38;
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Location = new System.Drawing.Point(23, 33);
            this.ultraLabel4.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(76, 18);
            this.ultraLabel4.TabIndex = 97;
            this.ultraLabel4.Tag = "Cut Off Time";
            this.ultraLabel4.Text = "Cut Off Time:";
            // 
            // ugpcCutOffTime
            // 
            this.ugpcCutOffTime.ColumnKey = "CutOffTime";
            appearance39.ForeColor = System.Drawing.Color.Black;
            this.ugpcCutOffTime.EditAppearance = appearance39;
            this.ugpcCutOffTime.Location = new System.Drawing.Point(122, 30);
            this.ugpcCutOffTime.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcCutOffTime.Name = "ugpcCutOffTime";
            this.ugpcCutOffTime.Size = new System.Drawing.Size(206, 24);
            this.ugpcCutOffTime.TabIndex = 14;
            this.ugpcCutOffTime.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcCutOffTime.Text = "Band or column does not exist.";
            // 
            // gbfx
            // 
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance41.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.gbfx.ContentAreaAppearance = appearance41;
            this.gbfx.Controls.Add(this.lblVSCurrencyValue);
            this.gbfx.Controls.Add(this.ultraLabel1);
            this.gbfx.Controls.Add(this.lblLeadCurrencyValue);
            this.gbfx.Controls.Add(this.ultraLabel2);
            this.gbfx.Controls.Add(this.ultraLabel3);
            this.gbfx.Controls.Add(this.ultraGridCellProxy4);
            this.gbfx.ForeColor = System.Drawing.Color.White;
            appearance48.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance48.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.gbfx.HeaderAppearance = appearance48;
            this.gbfx.Location = new System.Drawing.Point(7, 202);
            this.gbfx.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbfx.Name = "gbfx";
            this.gbfx.Size = new System.Drawing.Size(850, 66);
            this.gbfx.TabIndex = 87;
            this.gbfx.Tag = "";
            this.gbfx.Text = "FX  / FX Forward";
            this.gbfx.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // lblVSCurrencyValue
            // 
            appearance43.BackColor = System.Drawing.Color.Transparent;
            this.lblVSCurrencyValue.Appearance = appearance43;
            this.lblVSCurrencyValue.AutoSize = true;
            this.lblVSCurrencyValue.Location = new System.Drawing.Point(282, 35);
            this.lblVSCurrencyValue.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblVSCurrencyValue.Name = "lblVSCurrencyValue";
            this.lblVSCurrencyValue.Size = new System.Drawing.Size(0, 0);
            this.lblVSCurrencyValue.TabIndex = 93;
            this.lblVSCurrencyValue.Text = "";
            // 
            // ultraLabel1
            // 
            appearance43.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel1.Appearance = appearance43;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(200, 35);
            this.ultraLabel1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(72, 18);
            this.ultraLabel1.TabIndex = 93;
            this.ultraLabel1.Tag = "Vs Currency";
            this.ultraLabel1.Text = "Vs Currency:";
            // 
            // lblLeadCurrencyValue
            // 
            appearance43.BackColor = System.Drawing.Color.Transparent;
            this.lblLeadCurrencyValue.Appearance = appearance43;
            this.lblLeadCurrencyValue.AutoSize = true;
            this.lblLeadCurrencyValue.Location = new System.Drawing.Point(100, 35);
            this.lblLeadCurrencyValue.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblLeadCurrencyValue.Name = "lblLeadCurrencyValue";
            this.lblLeadCurrencyValue.Size = new System.Drawing.Size(0, 0);
            this.lblLeadCurrencyValue.TabIndex = 93;
            this.lblLeadCurrencyValue.Text = "";

            // 
            // ultraLabel2
            // 
            appearance45.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel2.Appearance = appearance45;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(5, 35);
            this.ultraLabel2.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(85, 18);
            this.ultraLabel2.TabIndex = 86;
            this.ultraLabel2.Tag = "Lead Currency";
            this.ultraLabel2.Text = "Lead Currency:";
            // 
            // ultraLabel3
            // 
            appearance46.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel3.Appearance = appearance46;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(713, 33);
            this.ultraLabel3.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(43, 18);
            this.ultraLabel3.TabIndex = 89;
            this.ultraLabel3.Tag = "Is NDF";
            this.ultraLabel3.Text = "Is NDF:";
            // 
            // ultraGridCellProxy4
            // 
            this.ultraGridCellProxy4.ColumnKey = "IsNDF";
            appearance47.ForeColor = System.Drawing.Color.Black;
            this.ultraGridCellProxy4.EditAppearance = appearance47;
            this.ultraGridCellProxy4.Location = new System.Drawing.Point(775, 29);
            this.ultraGridCellProxy4.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ultraGridCellProxy4.Name = "ultraGridCellProxy4";
            this.ultraGridCellProxy4.Size = new System.Drawing.Size(47, 24);
            this.ultraGridCellProxy4.TabIndex = 17;
            this.ultraGridCellProxy4.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ultraGridCellProxy4.Text = "Band or column does not exist.";
            // 
            // gbFixedIncome
            // 
            appearance49.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance49.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance49.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.gbFixedIncome.ContentAreaAppearance = appearance49;
            this.gbFixedIncome.Controls.Add(this.ugpcCollateralType);
            this.gbFixedIncome.Controls.Add(this.lblCollateralType);
            this.gbFixedIncome.Controls.Add(this.ugpcSecurityTypeID);
            this.gbFixedIncome.Controls.Add(this.lblSecurityTypeID);
            this.gbFixedIncome.Controls.Add(this.lblDaysToSettlement);
            this.gbFixedIncome.Controls.Add(this.lblCoupon);
            this.gbFixedIncome.Controls.Add(this.ugpcDaysToSettlement);
            this.gbFixedIncome.Controls.Add(this.ugpcIsZero);
            this.gbFixedIncome.Controls.Add(this.lblIsZero);
            this.gbFixedIncome.Controls.Add(this.ugpcCoupon);
            this.gbFixedIncome.Controls.Add(this.lblCouponFrequencyID);
            this.gbFixedIncome.Controls.Add(this.ugpcCouponFrequencyID);
            this.gbFixedIncome.Controls.Add(this.lblFirstCouponDate);
            this.gbFixedIncome.Controls.Add(this.ugpcFirstCouponDate);
            this.gbFixedIncome.Controls.Add(this.lblIssueDate);
            this.gbFixedIncome.Controls.Add(this.ugpcIssueDate);
            this.gbFixedIncome.Controls.Add(this.lblAccrualBasisID);
            this.gbFixedIncome.Controls.Add(this.ugpcAccrualBasisID);
            this.gbFixedIncome.ForeColor = System.Drawing.Color.White;
            appearance66.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance66.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.gbFixedIncome.HeaderAppearance = appearance66;
            this.gbFixedIncome.Location = new System.Drawing.Point(5, 205);
            this.gbFixedIncome.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbFixedIncome.Name = "gbFixedIncome";
            this.gbFixedIncome.Size = new System.Drawing.Size(850, 122);
            this.gbFixedIncome.TabIndex = 86;
            this.gbFixedIncome.Tag = "Fixed Income";
            this.gbFixedIncome.Text = "Fixed Income";
            this.gbFixedIncome.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // ugpcCollateralType
            // 
            this.ugpcCollateralType.ColumnKey = "CollateralTypeID";
            appearance48.ForeColor = System.Drawing.Color.Black;
            this.ugpcCollateralType.EditAppearance = appearance48;
            this.ugpcCollateralType.Location = new System.Drawing.Point(120, 87);
            this.ugpcCollateralType.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcCollateralType.Name = "ugpcCollateralType";
            this.ugpcCollateralType.Size = new System.Drawing.Size(117, 24);
            this.ugpcCollateralType.TabIndex = 132;
            this.ugpcCollateralType.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcCollateralType.Text = "Band or column does not exist.";
            // 
            // lblCollateralType
            // 
            this.lblCollateralType.AutoSize = true;
            this.lblCollateralType.Location = new System.Drawing.Point(9, 91);
            this.lblCollateralType.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblCollateralType.Name = "lblCollateralType";
            this.lblCollateralType.Size = new System.Drawing.Size(88, 18);
            this.lblCollateralType.TabIndex = 131;
            this.lblCollateralType.Tag = "Collateral Type";
            this.lblCollateralType.Text = "Collateral Type:";
            // 
            // ugpcSecurityTypeID
            // 
            this.ugpcSecurityTypeID.ColumnKey = "BondTypeID";
            appearance50.ForeColor = System.Drawing.Color.Black;
            this.ugpcSecurityTypeID.EditAppearance = appearance50;
            this.ugpcSecurityTypeID.Location = new System.Drawing.Point(120, 57);
            this.ugpcSecurityTypeID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcSecurityTypeID.Name = "ugpcSecurityTypeID";
            this.ugpcSecurityTypeID.Size = new System.Drawing.Size(99, 24);
            this.ugpcSecurityTypeID.TabIndex = 25;
            this.ugpcSecurityTypeID.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcSecurityTypeID.Text = "Band or column does not exist.";
            // 
            // lblSecurityTypeID
            // 
            appearance51.BackColor = System.Drawing.Color.Transparent;
            this.lblSecurityTypeID.Appearance = appearance51;
            this.lblSecurityTypeID.AutoSize = true;
            this.lblSecurityTypeID.Location = new System.Drawing.Point(9, 61);
            this.lblSecurityTypeID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblSecurityTypeID.Name = "lblSecurityTypeID";
            this.lblSecurityTypeID.Size = new System.Drawing.Size(65, 18);
            this.lblSecurityTypeID.TabIndex = 67;
            this.lblSecurityTypeID.Tag = "Bond Type";
            this.lblSecurityTypeID.Text = "Bond Type:";
            // 
            // lblDaysToSettlement
            // 
            appearance52.BackColor = System.Drawing.Color.Transparent;
            this.lblDaysToSettlement.Appearance = appearance52;
            this.lblDaysToSettlement.AutoSize = true;
            this.lblDaysToSettlement.Location = new System.Drawing.Point(514, 61);
            this.lblDaysToSettlement.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblDaysToSettlement.Name = "lblDaysToSettlement";
            this.lblDaysToSettlement.Size = new System.Drawing.Size(113, 18);
            this.lblDaysToSettlement.TabIndex = 45;
            this.lblDaysToSettlement.Tag = "Days To Settlement";
            this.lblDaysToSettlement.Text = "Days To Settlement:";
            // 
            // lblCoupon
            // 
            appearance53.BackColor = System.Drawing.Color.Transparent;
            this.lblCoupon.Appearance = appearance53;
            this.lblCoupon.AutoSize = true;
            this.lblCoupon.Location = new System.Drawing.Point(703, 30);
            this.lblCoupon.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblCoupon.Name = "lblCoupon";
            this.lblCoupon.Size = new System.Drawing.Size(51, 18);
            this.lblCoupon.TabIndex = 62;
            this.lblCoupon.Tag = "Coupon";
            this.lblCoupon.Text = "Coupon:";
            // 
            // ugpcDaysToSettlement
            // 
            this.ugpcDaysToSettlement.ColumnKey = "DaysToSettlement";
            appearance54.ForeColor = System.Drawing.Color.Black;
            this.ugpcDaysToSettlement.EditAppearance = appearance54;
            this.ugpcDaysToSettlement.Location = new System.Drawing.Point(664, 57);
            this.ugpcDaysToSettlement.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcDaysToSettlement.Name = "ugpcDaysToSettlement";
            this.ugpcDaysToSettlement.Size = new System.Drawing.Size(52, 24);
            this.ugpcDaysToSettlement.TabIndex = 27;
            this.ugpcDaysToSettlement.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcDaysToSettlement.Text = "Band or column does not exist.";
            // 
            // ugpcIsZero
            // 
            this.ugpcIsZero.ColumnKey = "IsZero";
            appearance55.ForeColor = System.Drawing.Color.Black;
            this.ugpcIsZero.EditAppearance = appearance55;
            this.ugpcIsZero.Location = new System.Drawing.Point(789, 57);
            this.ugpcIsZero.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcIsZero.Name = "ugpcIsZero";
            this.ugpcIsZero.Size = new System.Drawing.Size(45, 24);
            this.ugpcIsZero.TabIndex = 28;
            this.ugpcIsZero.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcIsZero.Text = "Band or column does not exist.";
            // 
            // lblIsZero
            // 
            appearance56.BackColor = System.Drawing.Color.Transparent;
            this.lblIsZero.Appearance = appearance56;
            this.lblIsZero.AutoSize = true;
            this.lblIsZero.Location = new System.Drawing.Point(724, 61);
            this.lblIsZero.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblIsZero.Name = "lblIsZero";
            this.lblIsZero.Size = new System.Drawing.Size(44, 18);
            this.lblIsZero.TabIndex = 63;
            this.lblIsZero.Tag = "IsZero";
            this.lblIsZero.Text = "Is Zero:";
            // 
            // ugpcCoupon
            // 
            this.ugpcCoupon.ColumnKey = "Coupon";
            appearance57.ForeColor = System.Drawing.Color.Black;
            this.ugpcCoupon.EditAppearance = appearance57;
            this.ugpcCoupon.Location = new System.Drawing.Point(769, 26);
            this.ugpcCoupon.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcCoupon.Name = "ugpcCoupon";
            this.ugpcCoupon.Size = new System.Drawing.Size(63, 24);
            this.ugpcCoupon.TabIndex = 23;
            this.ugpcCoupon.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcCoupon.Text = "Band or column does not exist.";
            // 
            // lblCouponFrequencyID
            // 
            appearance58.BackColor = System.Drawing.Color.Transparent;
            this.lblCouponFrequencyID.Appearance = appearance58;
            this.lblCouponFrequencyID.AutoSize = true;
            this.lblCouponFrequencyID.Location = new System.Drawing.Point(483, 30);
            this.lblCouponFrequencyID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblCouponFrequencyID.Name = "lblCouponFrequencyID";
            this.lblCouponFrequencyID.Size = new System.Drawing.Size(115, 18);
            this.lblCouponFrequencyID.TabIndex = 61;
            this.lblCouponFrequencyID.Tag = "Coupon Frequency ID";
            this.lblCouponFrequencyID.Text = "Coupon Frequency*:";
            // 
            // ugpcCouponFrequencyID
            // 
            this.ugpcCouponFrequencyID.ColumnKey = "CouponFrequencyID";
            appearance59.ForeColor = System.Drawing.Color.Black;
            this.ugpcCouponFrequencyID.EditAppearance = appearance59;
            this.ugpcCouponFrequencyID.Location = new System.Drawing.Point(626, 26);
            this.ugpcCouponFrequencyID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcCouponFrequencyID.Name = "ugpcCouponFrequencyID";
            this.ugpcCouponFrequencyID.Size = new System.Drawing.Size(64, 24);
            this.ugpcCouponFrequencyID.TabIndex = 22;
            this.ugpcCouponFrequencyID.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcCouponFrequencyID.Text = "Band or column does not exist.";
            // 
            // lblFirstCouponDate
            // 
            appearance60.BackColor = System.Drawing.Color.Transparent;
            this.lblFirstCouponDate.Appearance = appearance60;
            this.lblFirstCouponDate.AutoSize = true;
            this.lblFirstCouponDate.Location = new System.Drawing.Point(240, 61);
            this.lblFirstCouponDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblFirstCouponDate.Name = "lblFirstCouponDate";
            this.lblFirstCouponDate.Size = new System.Drawing.Size(106, 18);
            this.lblFirstCouponDate.TabIndex = 57;
            this.lblFirstCouponDate.Tag = "FirstCouponDate";
            this.lblFirstCouponDate.Text = "First Coupon Date:";
            // 
            // ugpcFirstCouponDate
            // 
            this.ugpcFirstCouponDate.ColumnKey = "FirstCouponDate";
            appearance61.ForeColor = System.Drawing.Color.Black;
            this.ugpcFirstCouponDate.EditAppearance = appearance61;
            this.ugpcFirstCouponDate.Location = new System.Drawing.Point(385, 57);
            this.ugpcFirstCouponDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcFirstCouponDate.Name = "ugpcFirstCouponDate";
            this.ugpcFirstCouponDate.Size = new System.Drawing.Size(113, 24);
            this.ugpcFirstCouponDate.TabIndex = 26;
            this.ugpcFirstCouponDate.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcFirstCouponDate.Text = "Band or column does not exist.";
            // 
            // lblIssueDate
            // 
            appearance62.BackColor = System.Drawing.Color.Transparent;
            this.lblIssueDate.Appearance = appearance62;
            this.lblIssueDate.AutoSize = true;
            this.lblIssueDate.Location = new System.Drawing.Point(9, 30);
            this.lblIssueDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblIssueDate.Name = "lblIssueDate";
            this.lblIssueDate.Size = new System.Drawing.Size(69, 18);
            this.lblIssueDate.TabIndex = 56;
            this.lblIssueDate.Tag = "IssueDate";
            this.lblIssueDate.Text = "Issue Date*:";
            // 
            // ugpcIssueDate
            // 
            this.ugpcIssueDate.ColumnKey = "IssueDate";
            appearance63.ForeColor = System.Drawing.Color.Black;
            this.ugpcIssueDate.EditAppearance = appearance63;
            this.ugpcIssueDate.Location = new System.Drawing.Point(99, 26);
            this.ugpcIssueDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcIssueDate.Name = "ugpcIssueDate";
            this.ugpcIssueDate.Size = new System.Drawing.Size(120, 24);
            this.ugpcIssueDate.TabIndex = 20;
            this.ugpcIssueDate.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcIssueDate.Text = "Band or column does not exist.";
            // 
            // lblAccrualBasisID
            // 
            appearance64.BackColor = System.Drawing.Color.Transparent;
            this.lblAccrualBasisID.Appearance = appearance64;
            this.lblAccrualBasisID.AutoSize = true;
            this.lblAccrualBasisID.Location = new System.Drawing.Point(240, 30);
            this.lblAccrualBasisID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblAccrualBasisID.Name = "lblAccrualBasisID";
            this.lblAccrualBasisID.Size = new System.Drawing.Size(83, 18);
            this.lblAccrualBasisID.TabIndex = 55;
            this.lblAccrualBasisID.Tag = "Accrual Basis";
            this.lblAccrualBasisID.Text = "Accrual Basis*:";
            // 
            // ugpcAccrualBasisID
            // 
            this.ugpcAccrualBasisID.ColumnKey = "AccrualBasisID";
            appearance65.ForeColor = System.Drawing.Color.Black;
            this.ugpcAccrualBasisID.EditAppearance = appearance65;
            this.ugpcAccrualBasisID.Location = new System.Drawing.Point(349, 26);
            this.ugpcAccrualBasisID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcAccrualBasisID.Name = "ugpcAccrualBasisID";
            this.ugpcAccrualBasisID.Size = new System.Drawing.Size(117, 24);
            this.ugpcAccrualBasisID.TabIndex = 21;
            this.ugpcAccrualBasisID.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcAccrualBasisID.Text = "Band or column does not exist.";
            // 
            // tabAssetSpecific
            // 
            this.tabAssetSpecific.Controls.Add(this.gbPriceInfo);
            this.tabAssetSpecific.Controls.Add(this.gbSecDates);
            this.tabAssetSpecific.Controls.Add(this.gbSecurityInfo);
            this.tabAssetSpecific.Location = new System.Drawing.Point(-10000, -10000);
            this.tabAssetSpecific.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabAssetSpecific.Name = "tabAssetSpecific";
            this.tabAssetSpecific.Size = new System.Drawing.Size(877, 345);
            // 
            // gbPriceInfo
            // 
            appearance68.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance68.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance68.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.gbPriceInfo.ContentAreaAppearance = appearance68;
            this.gbPriceInfo.Controls.Add(this.lblStrikePriceMultiplier);
            this.gbPriceInfo.Controls.Add(this.ugcpStrikePriceMultiplier);
            this.gbPriceInfo.Controls.Add(this.lblDelta);
            this.gbPriceInfo.Controls.Add(this.ugpcDelta);
            this.gbPriceInfo.Controls.Add(this.lblRoundLot);
            this.gbPriceInfo.Controls.Add(this.ugpcRoundLot);
            this.gbPriceInfo.ForeColor = System.Drawing.Color.White;
            appearance75.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance75.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.gbPriceInfo.HeaderAppearance = appearance75;
            this.gbPriceInfo.Location = new System.Drawing.Point(5, 9);
            this.gbPriceInfo.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbPriceInfo.Name = "gbPriceInfo";
            this.gbPriceInfo.Size = new System.Drawing.Size(859, 61);
            this.gbPriceInfo.TabIndex = 86;
            this.gbPriceInfo.Text = "Price Info";
            this.gbPriceInfo.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // lblStrikePriceMultiplier
            // 
            appearance69.BackColor = System.Drawing.Color.Transparent;
            this.lblStrikePriceMultiplier.Appearance = appearance69;
            this.lblStrikePriceMultiplier.AutoSize = true;
            this.lblStrikePriceMultiplier.Location = new System.Drawing.Point(565, 35);
            this.lblStrikePriceMultiplier.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblStrikePriceMultiplier.Name = "lblStrikePriceMultiplier";
            this.lblStrikePriceMultiplier.Size = new System.Drawing.Size(123, 18);
            this.lblStrikePriceMultiplier.TabIndex = 126;
            this.lblStrikePriceMultiplier.Tag = "StrikePriceMultiplier";
            this.lblStrikePriceMultiplier.Text = "Strike Price Multiplier:";
            // 
            // ugcpStrikePriceMultiplier
            // 
            this.ugcpStrikePriceMultiplier.ColumnKey = "StrikePriceMultiplier";
            appearance70.ForeColor = System.Drawing.Color.Black;
            this.ugcpStrikePriceMultiplier.EditAppearance = appearance70;
            this.ugcpStrikePriceMultiplier.Location = new System.Drawing.Point(720, 29);
            this.ugcpStrikePriceMultiplier.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugcpStrikePriceMultiplier.Name = "ugcpStrikePriceMultiplier";
            this.ugcpStrikePriceMultiplier.Size = new System.Drawing.Size(135, 24);
            this.ugcpStrikePriceMultiplier.TabIndex = 125;
            this.ugcpStrikePriceMultiplier.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugcpStrikePriceMultiplier.Text = "Band or column does not exist.";
            // 
            // lblDelta
            // 
            appearance71.BackColor = System.Drawing.Color.Transparent;
            this.lblDelta.Appearance = appearance71;
            this.lblDelta.AutoSize = true;
            this.lblDelta.Location = new System.Drawing.Point(276, 35);
            this.lblDelta.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblDelta.Name = "lblDelta";
            this.lblDelta.Size = new System.Drawing.Size(101, 18);
            this.lblDelta.TabIndex = 124;
            this.lblDelta.Tag = "Delta";
            this.lblDelta.Text = "Leveraged Factor:";
            // 
            // ugpcDelta
            // 
            this.ugpcDelta.ColumnKey = "Delta";
            appearance72.ForeColor = System.Drawing.Color.Black;
            this.ugpcDelta.EditAppearance = appearance72;
            this.ugpcDelta.Location = new System.Drawing.Point(411, 29);
            this.ugpcDelta.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcDelta.Name = "ugpcDelta";
            this.ugpcDelta.Size = new System.Drawing.Size(135, 24);
            this.ugpcDelta.TabIndex = 17;
            this.ugpcDelta.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcDelta.Text = "Band or column does not exist.";
            // 
            // lblRoundLot
            // 
            appearance73.BackColor = System.Drawing.Color.Transparent;
            this.lblRoundLot.Appearance = appearance73;
            this.lblRoundLot.AutoSize = true;
            this.lblRoundLot.Location = new System.Drawing.Point(3, 35);
            this.lblRoundLot.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblRoundLot.Name = "lblRoundLot";
            this.lblRoundLot.Size = new System.Drawing.Size(64, 18);
            this.lblRoundLot.TabIndex = 122;
            this.lblRoundLot.Tag = "RoundLot";
            this.lblRoundLot.Text = "Round Lot:";
            // 
            // ugpcRoundLot
            // 
            this.ugpcRoundLot.ColumnKey = "RoundLot";
            appearance74.ForeColor = System.Drawing.Color.Black;
            this.ugpcRoundLot.EditAppearance = appearance74;
            this.ugpcRoundLot.Location = new System.Drawing.Point(122, 29);
            this.ugpcRoundLot.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcRoundLot.Name = "ugpcRoundLot";
            this.ugpcRoundLot.Size = new System.Drawing.Size(135, 24);
            this.ugpcRoundLot.TabIndex = 16;
            this.ugpcRoundLot.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcRoundLot.Text = "Band or column does not exist.";
            // 
            // gbSecDates
            // 
            appearance76.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance76.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance76.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.gbSecDates.ContentAreaAppearance = appearance76;
            this.gbSecDates.Controls.Add(this.ugpcCreationDate);
            this.gbSecDates.Controls.Add(this.lblCreationDate);
            this.gbSecDates.Controls.Add(this.lblModifiedDate);
            this.gbSecDates.Controls.Add(this.ugpcModifiedDate);
            this.gbSecDates.Controls.Add(this.ultraLabel8);
            this.gbSecDates.Controls.Add(this.ugpcApprovalDate);
            this.gbSecDates.ForeColor = System.Drawing.Color.White;
            appearance83.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance83.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.gbSecDates.HeaderAppearance = appearance83;
            this.gbSecDates.Location = new System.Drawing.Point(5, 290);
            this.gbSecDates.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbSecDates.Name = "gbSecDates";
            this.gbSecDates.Size = new System.Drawing.Size(859, 62);
            this.gbSecDates.TabIndex = 85;
            this.gbSecDates.Text = "Security Dates";
            this.gbSecDates.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // ugpcCreationDate
            // 
            this.ugpcCreationDate.ColumnKey = "CreationDate";
            appearance77.ForeColor = System.Drawing.Color.Black;
            this.ugpcCreationDate.EditAppearance = appearance77;
            this.ugpcCreationDate.Location = new System.Drawing.Point(122, 30);
            this.ugpcCreationDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcCreationDate.Name = "ugpcCreationDate";
            this.ugpcCreationDate.Size = new System.Drawing.Size(135, 24);
            this.ugpcCreationDate.TabIndex = 25;
            this.ugpcCreationDate.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcCreationDate.Text = "Band or column does not exist.";
            // 
            // lblCreationDate
            // 
            appearance78.BackColor = System.Drawing.Color.Transparent;
            this.lblCreationDate.Appearance = appearance78;
            this.lblCreationDate.AutoSize = true;
            this.lblCreationDate.Location = new System.Drawing.Point(3, 33);
            this.lblCreationDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblCreationDate.Name = "lblCreationDate";
            this.lblCreationDate.Size = new System.Drawing.Size(82, 18);
            this.lblCreationDate.TabIndex = 71;
            this.lblCreationDate.Tag = "Creation Date";
            this.lblCreationDate.Text = "Creation Date:";
            // 
            // lblModifiedDate
            // 
            appearance77.BackColor = System.Drawing.Color.Transparent;
            this.lblModifiedDate.Appearance = appearance77;
            this.lblModifiedDate.AutoSize = true;
            this.lblModifiedDate.Location = new System.Drawing.Point(276, 33);
            this.lblModifiedDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblModifiedDate.Name = "lblModifiedDate";
            this.lblModifiedDate.Size = new System.Drawing.Size(85, 18);
            this.lblModifiedDate.TabIndex = 66;
            this.lblModifiedDate.Tag = "Modified Date";
            this.lblModifiedDate.Text = "Modified Date:";
            // 
            // ugpcModifiedDate
            // 
            this.ugpcModifiedDate.ColumnKey = "ModifiedDate";
            appearance80.ForeColor = System.Drawing.Color.Black;
            this.ugpcModifiedDate.EditAppearance = appearance80;
            this.ugpcModifiedDate.Location = new System.Drawing.Point(411, 30);
            this.ugpcModifiedDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcModifiedDate.Name = "ugpcModifiedDate";
            this.ugpcModifiedDate.Size = new System.Drawing.Size(135, 24);
            this.ugpcModifiedDate.TabIndex = 26;
            this.ugpcModifiedDate.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcModifiedDate.Text = "Band or column does not exist.";
            // 
            // ultraLabel8
            // 
            appearance81.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel8.Appearance = appearance81;
            this.ultraLabel8.AutoSize = true;
            this.ultraLabel8.Location = new System.Drawing.Point(565, 33);
            this.ultraLabel8.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(85, 18);
            this.ultraLabel8.TabIndex = 67;
            this.ultraLabel8.Tag = "Approval Date";
            this.ultraLabel8.Text = "Approval Date:";
            // 
            // ugpcApprovalDate
            // 
            this.ugpcApprovalDate.ColumnKey = "ApprovalDate";
            appearance82.ForeColor = System.Drawing.Color.Black;
            this.ugpcApprovalDate.EditAppearance = appearance82;
            this.ugpcApprovalDate.Location = new System.Drawing.Point(720, 30);
            this.ugpcApprovalDate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcApprovalDate.Name = "ugpcApprovalDate";
            this.ugpcApprovalDate.Size = new System.Drawing.Size(135, 24);
            this.ugpcApprovalDate.TabIndex = 27;
            this.ugpcApprovalDate.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcApprovalDate.Text = "Band or column does not exist.";
            // 
            // gbSecurityInfo
            // 
            appearance84.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance84.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance84.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.gbSecurityInfo.ContentAreaAppearance = appearance84;
            this.gbSecurityInfo.Controls.Add(this.cbActivSymbolCamelCase);
            this.gbSecurityInfo.Controls.Add(this.ugpcShares);
            this.gbSecurityInfo.Controls.Add(this.lblShares);
            this.gbSecurityInfo.Controls.Add(this.ultraGridCellProxy5);
            this.gbSecurityInfo.Controls.Add(this.lblBloombergOptionRoot);
            this.gbSecurityInfo.Controls.Add(this.ugpcEsignalOptionRoot);
            this.gbSecurityInfo.Controls.Add(this.lblESignalOptionRoot);
            this.gbSecurityInfo.Controls.Add(this.ugpcIsSecApproved);
            this.gbSecurityInfo.Controls.Add(this.lblApprovalStatus);
            this.gbSecurityInfo.Controls.Add(this.ugpcISINSymbol);
            this.gbSecurityInfo.Controls.Add(this.lblISINSymbol);
            this.gbSecurityInfo.Controls.Add(this.ugpcIDCOOptionSymbol);
            this.gbSecurityInfo.Controls.Add(this.lblIDCOOptionSymbol);
            this.gbSecurityInfo.Controls.Add(this.ugpcComments);
            this.gbSecurityInfo.Controls.Add(this.lblComments);
            this.gbSecurityInfo.Controls.Add(this.lblFactSetSymbol);
            this.gbSecurityInfo.Controls.Add(this.ugpcFactSetSymbol);
            this.gbSecurityInfo.Controls.Add(this.lblActivSymbol);
            this.gbSecurityInfo.Controls.Add(this.ugpcActivSymbol);
            this.gbSecurityInfo.Controls.Add(this.lblSedolSymbol);
            this.gbSecurityInfo.Controls.Add(this.ugpcSedolSymbol);
            this.gbSecurityInfo.Controls.Add(this.lblCusipSymbol);
            this.gbSecurityInfo.Controls.Add(this.ugpcOSIOptionSymbol);
            this.gbSecurityInfo.Controls.Add(this.lblOSIOptionSymbol);
            this.gbSecurityInfo.Controls.Add(this.ugpcCusipSymbol);
            this.gbSecurityInfo.Controls.Add(this.lblBloombergSymbolExCode);
            this.gbSecurityInfo.Controls.Add(this.ugpcBloombergSymbolExCode);
            this.gbSecurityInfo.ForeColor = System.Drawing.Color.White;
            appearance103.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance103.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.gbSecurityInfo.HeaderAppearance = appearance103;
            this.gbSecurityInfo.Location = new System.Drawing.Point(5, 72);
            this.gbSecurityInfo.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbSecurityInfo.Name = "gbSecurityInfo";
            this.gbSecurityInfo.Size = new System.Drawing.Size(859, 216);
            this.gbSecurityInfo.TabIndex = 84;
            this.gbSecurityInfo.Tag = "";
            this.gbSecurityInfo.Text = "Security Info";
            this.gbSecurityInfo.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // ultraGridCellProxy5
            // 
            this.ultraGridCellProxy5.ColumnKey = "BloombergOptionRoot";
            appearance85.ForeColor = System.Drawing.Color.Black;
            this.ultraGridCellProxy5.EditAppearance = appearance85;
            this.ultraGridCellProxy5.Location = new System.Drawing.Point(720, 125);
            this.ultraGridCellProxy5.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ultraGridCellProxy5.Name = "ultraGridCellProxy5";
            this.ultraGridCellProxy5.Size = new System.Drawing.Size(125, 24);
            this.ultraGridCellProxy5.TabIndex = 125;
            this.ultraGridCellProxy5.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ultraGridCellProxy5.Text = "Band or column does not exist.";
            // 
            // lblBloombergOptionRoot
            // 
            appearance86.BackColor = System.Drawing.Color.Transparent;
            this.lblBloombergOptionRoot.Appearance = appearance86;
            this.lblBloombergOptionRoot.AutoSize = true;
            this.lblBloombergOptionRoot.Location = new System.Drawing.Point(565, 130);
            this.lblBloombergOptionRoot.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblBloombergOptionRoot.Name = "lblBloombergOptionRoot";
            this.lblBloombergOptionRoot.Size = new System.Drawing.Size(139, 18);
            this.lblBloombergOptionRoot.TabIndex = 126;
            this.lblBloombergOptionRoot.Tag = "BloombergOptionRoot";
            this.lblBloombergOptionRoot.Text = "Bloomberg Option Root:";
            // 
            // ugpcEsignalOptionRoot
            // 
            this.ugpcEsignalOptionRoot.ColumnKey = "EsignalOptionRoot";
            appearance87.ForeColor = System.Drawing.Color.Black;
            this.ugpcEsignalOptionRoot.EditAppearance = appearance87;
            this.ugpcEsignalOptionRoot.Location = new System.Drawing.Point(411, 125);
            this.ugpcEsignalOptionRoot.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcEsignalOptionRoot.Name = "ugpcEsignalOptionRoot";
            this.ugpcEsignalOptionRoot.Size = new System.Drawing.Size(135, 24);
            this.ugpcEsignalOptionRoot.TabIndex = 123;
            this.ugpcEsignalOptionRoot.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcEsignalOptionRoot.Text = "Band or column does not exist.";
            // 
            // lblESignalOptionRoot
            // 
            appearance88.BackColor = System.Drawing.Color.Transparent;
            this.lblESignalOptionRoot.Appearance = appearance88;
            this.lblESignalOptionRoot.AutoSize = true;
            this.lblESignalOptionRoot.Location = new System.Drawing.Point(276, 130);
            this.lblESignalOptionRoot.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblESignalOptionRoot.Name = "lblESignalOptionRoot";
            this.lblESignalOptionRoot.Size = new System.Drawing.Size(116, 18);
            this.lblESignalOptionRoot.TabIndex = 124;
            this.lblESignalOptionRoot.Tag = "EsignalOptionRoot";
            this.lblESignalOptionRoot.Text = "Esignal Option Root:";
            // 
            // ugpcIsSecApproved
            // 
            this.ugpcIsSecApproved.ColumnKey = "SecApprovalStatus";
            appearance89.ForeColor = System.Drawing.Color.Black;
            this.ugpcIsSecApproved.EditAppearance = appearance89;
            this.ugpcIsSecApproved.Location = new System.Drawing.Point(720, 60);
            this.ugpcIsSecApproved.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcIsSecApproved.Name = "ugpcIsSecApproved";
            this.ugpcIsSecApproved.Size = new System.Drawing.Size(135, 24);
            this.ugpcIsSecApproved.TabIndex = 23;
            this.ugpcIsSecApproved.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcIsSecApproved.Text = "Band or column does not exist.";
            // 
            // lblApprovalStatus
            // 
            appearance90.BackColor = System.Drawing.Color.Transparent;
            this.lblApprovalStatus.Appearance = appearance90;
            this.lblApprovalStatus.AutoSize = true;
            this.lblApprovalStatus.Location = new System.Drawing.Point(565, 65);
            this.lblApprovalStatus.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblApprovalStatus.Name = "lblApprovalStatus";
            this.lblApprovalStatus.Size = new System.Drawing.Size(93, 18);
            this.lblApprovalStatus.TabIndex = 122;
            this.lblApprovalStatus.Tag = "Approved";
            this.lblApprovalStatus.Text = "Approval Status:";
            // 
            // ugpcISINSymbol
            // 
            this.ugpcISINSymbol.ColumnKey = "ISINSymbol";
            appearance91.ForeColor = System.Drawing.Color.Black;
            this.ugpcISINSymbol.EditAppearance = appearance91;
            this.ugpcISINSymbol.Location = new System.Drawing.Point(720, 28);
            this.ugpcISINSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcISINSymbol.Name = "ugpcISINSymbol";
            this.ugpcISINSymbol.Size = new System.Drawing.Size(135, 24);
            this.ugpcISINSymbol.TabIndex = 20;
            this.ugpcISINSymbol.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcISINSymbol.Text = "Band or column does not exist.";
            // 
            // lblISINSymbol
            // 
            appearance92.BackColor = System.Drawing.Color.Transparent;
            this.lblISINSymbol.Appearance = appearance92;
            this.lblISINSymbol.AutoSize = true;
            this.lblISINSymbol.Location = new System.Drawing.Point(565, 32);
            this.lblISINSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblISINSymbol.Name = "lblISINSymbol";
            this.lblISINSymbol.Size = new System.Drawing.Size(74, 18);
            this.lblISINSymbol.TabIndex = 120;
            this.lblISINSymbol.Tag = "ISINSymbol";
            this.lblISINSymbol.Text = "ISIN Symbol:";
            // 
            // ugpcIDCOOptionSymbol
            // 
            this.ugpcIDCOOptionSymbol.ColumnKey = "IDCOOptionSymbol";
            appearance93.ForeColor = System.Drawing.Color.Black;
            this.ugpcIDCOOptionSymbol.EditAppearance = appearance93;
            this.ugpcIDCOOptionSymbol.Location = new System.Drawing.Point(124, 60);
            this.ugpcIDCOOptionSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcIDCOOptionSymbol.Name = "ugpcIDCOOptionSymbol";
            this.ugpcIDCOOptionSymbol.Size = new System.Drawing.Size(135, 24);
            this.ugpcIDCOOptionSymbol.TabIndex = 21;
            this.ugpcIDCOOptionSymbol.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcIDCOOptionSymbol.Text = "Band or column does not exist.";
            // 
            // lblIDCOOptionSymbol
            // 
            appearance94.BackColor = System.Drawing.Color.Transparent;
            this.lblIDCOOptionSymbol.Appearance = appearance94;
            this.lblIDCOOptionSymbol.AutoSize = true;
            this.lblIDCOOptionSymbol.Location = new System.Drawing.Point(3, 65);
            this.lblIDCOOptionSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblIDCOOptionSymbol.Name = "lblIDCOOptionSymbol";
            this.lblIDCOOptionSymbol.Size = new System.Drawing.Size(92, 18);
            this.lblIDCOOptionSymbol.TabIndex = 112;
            this.lblIDCOOptionSymbol.Tag = "IDCOOptionSymbol";
            this.lblIDCOOptionSymbol.Text = "IDCOOption-22:";
            // 
            // ugpcComments
            // 
            this.ugpcComments.ColumnKey = "Comments";
            appearance95.ForeColor = System.Drawing.Color.Black;
            this.ugpcComments.EditAppearance = appearance95;
            this.ugpcComments.Location = new System.Drawing.Point(124, 93);
            this.ugpcComments.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcComments.Name = "ugpcComments";
            this.ugpcComments.Size = new System.Drawing.Size(135, 24);
            this.ugpcComments.TabIndex = 24;
            this.ugpcComments.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcComments.Text = "Band or column does not exist.";
            // 
            // lblComments
            // 
            appearance96.BackColor = System.Drawing.Color.Transparent;
            this.lblComments.Appearance = appearance96;
            this.lblComments.AutoSize = true;
            this.lblComments.Location = new System.Drawing.Point(3, 97);
            this.lblComments.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(66, 18);
            this.lblComments.TabIndex = 111;
            this.lblComments.Tag = "Comments";
            this.lblComments.Text = "Comments:";
            // 
            // lblFactSetSymbol
            // 
            this.lblFactSetSymbol.Appearance = appearance19;
            this.lblFactSetSymbol.AutoSize = true;
            this.lblFactSetSymbol.Location = new System.Drawing.Point(565, 97);
            this.lblFactSetSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblFactSetSymbol.Name = "lblFactSetSymbol";
            this.lblFactSetSymbol.Size = new System.Drawing.Size(91, 18);
            this.lblFactSetSymbol.TabIndex = 91;
            this.lblFactSetSymbol.Tag = "FactSetSymbol";
            this.lblFactSetSymbol.Text = "FactSet Symbol:";
            // 
            // ugpcFactSetSymbol
            // 
            this.ugpcFactSetSymbol.ColumnKey = "FactSetSymbol";
            this.ugpcFactSetSymbol.EditAppearance = appearance20;
            this.ugpcFactSetSymbol.Location = new System.Drawing.Point(720, 93);
            this.ugpcFactSetSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcFactSetSymbol.Name = "ugpcFactSetSymbol";
            this.ugpcFactSetSymbol.Size = new System.Drawing.Size(125, 24);
            this.ugpcFactSetSymbol.TabIndex = 10;
            this.ugpcFactSetSymbol.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcFactSetSymbol.Text = "Band or column does not exist.";
            // 
            // lblActivSymbol
            // 
            this.lblActivSymbol.Appearance = appearance19;
            this.lblActivSymbol.AutoSize = true;
            this.lblActivSymbol.Location = new System.Drawing.Point(3, 130);
            this.lblActivSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblActivSymbol.Name = "lblActivSymbol";
            this.lblActivSymbol.Size = new System.Drawing.Size(84, 18);
            this.lblActivSymbol.TabIndex = 91;
            this.lblActivSymbol.Tag = "ActivSymbol";
            this.lblActivSymbol.Text = "ACTIV Symbol:";
            // 
            // ugpcActivSymbol
            // 
            this.ugpcActivSymbol.ColumnKey = "ActivSymbol";
            this.ugpcActivSymbol.EditAppearance = appearance20;
            this.ugpcActivSymbol.Location = new System.Drawing.Point(124, 125);
            this.ugpcActivSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcActivSymbol.Name = "ugpcActivSymbol";
            this.ugpcActivSymbol.Size = new System.Drawing.Size(135, 24);
            this.ugpcActivSymbol.TabIndex = 10;
            this.ugpcActivSymbol.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcActivSymbol.Text = "Band or column does not exist.";
            // 
            // lblSedolSymbol
            // 
            appearance97.BackColor = System.Drawing.Color.Transparent;
            this.lblSedolSymbol.Appearance = appearance97;
            this.lblSedolSymbol.AutoSize = true;
            this.lblSedolSymbol.Location = new System.Drawing.Point(3, 32);
            this.lblSedolSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblSedolSymbol.Name = "lblSedolSymbol";
            this.lblSedolSymbol.Size = new System.Drawing.Size(82, 18);
            this.lblSedolSymbol.TabIndex = 108;
            this.lblSedolSymbol.Tag = "SedolSymbol";
            this.lblSedolSymbol.Text = "Sedol Symbol:";
            // 
            // ugpcSedolSymbol
            // 
            this.ugpcSedolSymbol.ColumnKey = "SedolSymbol";
            appearance98.ForeColor = System.Drawing.Color.Black;
            this.ugpcSedolSymbol.EditAppearance = appearance98;
            this.ugpcSedolSymbol.Location = new System.Drawing.Point(124, 28);
            this.ugpcSedolSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcSedolSymbol.Name = "ugpcSedolSymbol";
            this.ugpcSedolSymbol.Size = new System.Drawing.Size(135, 24);
            this.ugpcSedolSymbol.TabIndex = 18;
            this.ugpcSedolSymbol.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcSedolSymbol.Text = "Band or column does not exist.";
            // 
            // lblCusipSymbol
            // 
            appearance99.BackColor = System.Drawing.Color.Transparent;
            this.lblCusipSymbol.Appearance = appearance99;
            this.lblCusipSymbol.AutoSize = true;
            this.lblCusipSymbol.Location = new System.Drawing.Point(276, 32);
            this.lblCusipSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblCusipSymbol.Name = "lblCusipSymbol";
            this.lblCusipSymbol.Size = new System.Drawing.Size(81, 18);
            this.lblCusipSymbol.TabIndex = 109;
            this.lblCusipSymbol.Tag = "Cusip Symbol";
            this.lblCusipSymbol.Text = "Cusip Symbol:";
            // 
            // ugpcOSIOptionSymbol
            // 
            this.ugpcOSIOptionSymbol.ColumnKey = "OSIOptionSymbol";
            appearance100.ForeColor = System.Drawing.Color.Black;
            this.ugpcOSIOptionSymbol.EditAppearance = appearance100;
            this.ugpcOSIOptionSymbol.Location = new System.Drawing.Point(411, 60);
            this.ugpcOSIOptionSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcOSIOptionSymbol.Name = "ugpcOSIOptionSymbol";
            this.ugpcOSIOptionSymbol.Size = new System.Drawing.Size(135, 24);
            this.ugpcOSIOptionSymbol.TabIndex = 22;
            this.ugpcOSIOptionSymbol.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcOSIOptionSymbol.Text = "Band or column does not exist.";
            // 
            // lblOSIOptionSymbol
            // 
            appearance101.BackColor = System.Drawing.Color.Transparent;
            this.lblOSIOptionSymbol.Appearance = appearance101;
            this.lblOSIOptionSymbol.AutoSize = true;
            this.lblOSIOptionSymbol.Location = new System.Drawing.Point(276, 65);
            this.lblOSIOptionSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblOSIOptionSymbol.Name = "lblOSIOptionSymbol";
            this.lblOSIOptionSymbol.Size = new System.Drawing.Size(82, 15);
            this.lblOSIOptionSymbol.TabIndex = 110;
            this.lblOSIOptionSymbol.Tag = "OSIOptionSymbol";
            this.lblOSIOptionSymbol.Text = "OSIOption-21:";
            this.lblOSIOptionSymbol.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ugpcCusipSymbol
            // 
            this.ugpcCusipSymbol.ColumnKey = "CusipSymbol";
            appearance102.ForeColor = System.Drawing.Color.Black;
            this.ugpcCusipSymbol.EditAppearance = appearance102;
            this.ugpcCusipSymbol.Location = new System.Drawing.Point(411, 28);
            this.ugpcCusipSymbol.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcCusipSymbol.Name = "ugpcCusipSymbol";
            this.ugpcCusipSymbol.Size = new System.Drawing.Size(135, 24);
            this.ugpcCusipSymbol.TabIndex = 19;
            this.ugpcCusipSymbol.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcCusipSymbol.Text = "Band or column does not exist.";
            // 
            // tabUDADetails
            // 
            this.tabUDADetails.Controls.Add(this.gbUDADetails);
            this.tabUDADetails.Location = new System.Drawing.Point(-10000, -10000);
            this.tabUDADetails.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabUDADetails.Name = "tabUDADetails";
            this.tabUDADetails.Size = new System.Drawing.Size(877, 345);
            // 
            // gbUDADetails
            // 
            appearance104.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance104.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance104.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            this.gbUDADetails.ContentAreaAppearance = appearance104;
            this.gbUDADetails.Controls.Add(this.lblUseDefaultUDA);
            this.gbUDADetails.Controls.Add(this.gcpcUseDefaultUDA);
            this.gbUDADetails.Controls.Add(this.ugpcUDACountry);
            this.gbUDADetails.Controls.Add(this.btnUDAUI);
            this.gbUDADetails.Controls.Add(this.ugpcUDASubSector);
            this.gbUDADetails.Controls.Add(this.lblUDAAssetClass);
            this.gbUDADetails.Controls.Add(this.lblSubSector);
            this.gbUDADetails.Controls.Add(this.ugpcAssetID);
            this.gbUDADetails.Controls.Add(this.ugpcUDASector);
            this.gbUDADetails.Controls.Add(this.lblUDACountry);
            this.gbUDADetails.Controls.Add(this.lblUDASector);
            this.gbUDADetails.Controls.Add(this.ugcpUDASecurity);
            this.gbUDADetails.Controls.Add(this.lblSecType);
            this.gbUDADetails.ForeColor = System.Drawing.Color.White;
            appearance117.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance117.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance117.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance117.ForeColor = System.Drawing.Color.Black;
            this.gbUDADetails.HeaderAppearance = appearance117;
            this.gbUDADetails.Location = new System.Drawing.Point(9, 18);
            this.gbUDADetails.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbUDADetails.Name = "gbUDADetails";
            this.gbUDADetails.Size = new System.Drawing.Size(864, 284);
            this.gbUDADetails.TabIndex = 81;
            this.gbUDADetails.Text = "UDA Details";
            this.gbUDADetails.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // lblUseDefaultUDA
            // 
            appearance105.BackColor = System.Drawing.Color.Transparent;
            this.lblUseDefaultUDA.Appearance = appearance105;
            this.lblUseDefaultUDA.AutoSize = true;
            this.lblUseDefaultUDA.Location = new System.Drawing.Point(85, 38);
            this.lblUseDefaultUDA.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblUseDefaultUDA.Name = "lblUseDefaultUDA";
            this.lblUseDefaultUDA.Size = new System.Drawing.Size(414, 18);
            this.lblUseDefaultUDA.TabIndex = 77;
            this.lblUseDefaultUDA.Tag = "SecurityType";
            this.lblUseDefaultUDA.Text = "Use UDA information from Underlying Symbol or Root Symbol (if available)";
            // 
            // gcpcUseDefaultUDA
            // 
            this.gcpcUseDefaultUDA.ColumnKey = "UseUDAFromUnderlyingOrRoot";
            appearance106.ForeColor = System.Drawing.Color.Black;
            this.gcpcUseDefaultUDA.EditAppearance = appearance106;
            this.gcpcUseDefaultUDA.Location = new System.Drawing.Point(31, 36);
            this.gcpcUseDefaultUDA.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gcpcUseDefaultUDA.Name = "gcpcUseDefaultUDA";
            this.gcpcUseDefaultUDA.Size = new System.Drawing.Size(50, 22);
            this.gcpcUseDefaultUDA.TabIndex = 76;
            this.gcpcUseDefaultUDA.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.gcpcUseDefaultUDA.Text = "Band or column does not exist.";
            // 
            // ugpcUDACountry
            // 
            this.ugpcUDACountry.ColumnKey = "UDACountryID";
            appearance107.ForeColor = System.Drawing.Color.Black;
            this.ugpcUDACountry.EditAppearance = appearance107;
            this.ugpcUDACountry.Location = new System.Drawing.Point(589, 74);
            this.ugpcUDACountry.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcUDACountry.Name = "ugpcUDACountry";
            this.ugpcUDACountry.Size = new System.Drawing.Size(167, 24);
            this.ugpcUDACountry.TabIndex = 31;
            this.ugpcUDACountry.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcUDACountry.Text = "Band or column does not exist.";
            // 
            // btnUDAUI
            // 
            this.btnUDAUI.BackColorInternal = System.Drawing.Color.Silver;
            this.btnUDAUI.Location = new System.Drawing.Point(589, 160);
            this.btnUDAUI.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnUDAUI.Name = "btnUDAUI";
            this.btnUDAUI.Size = new System.Drawing.Size(117, 24);
            this.btnUDAUI.TabIndex = 35;
            this.btnUDAUI.Text = "Edit UDA ";
            this.btnUDAUI.Click += new System.EventHandler(this.menuUDAData_Click);
            // 
            // ugpcUDASubSector
            // 
            this.ugpcUDASubSector.ColumnKey = "UDASubSectorID";
            appearance108.ForeColor = System.Drawing.Color.Black;
            this.ugpcUDASubSector.EditAppearance = appearance108;
            this.ugpcUDASubSector.Location = new System.Drawing.Point(168, 155);
            this.ugpcUDASubSector.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcUDASubSector.Name = "ugpcUDASubSector";
            this.ugpcUDASubSector.Size = new System.Drawing.Size(167, 24);
            this.ugpcUDASubSector.TabIndex = 34;
            this.ugpcUDASubSector.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcUDASubSector.Text = "Band or column does not exist.";
            // 
            // lblUDAAssetClass
            // 
            appearance109.BackColor = System.Drawing.Color.Transparent;
            this.lblUDAAssetClass.Appearance = appearance109;
            this.lblUDAAssetClass.AutoSize = true;
            this.lblUDAAssetClass.Location = new System.Drawing.Point(24, 117);
            this.lblUDAAssetClass.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblUDAAssetClass.Name = "lblUDAAssetClass";
            this.lblUDAAssetClass.Size = new System.Drawing.Size(96, 18);
            this.lblUDAAssetClass.TabIndex = 72;
            this.lblUDAAssetClass.Tag = "AssetClass";
            this.lblUDAAssetClass.Text = "UDA Asset Class:";
            // 
            // lblSubSector
            // 
            appearance110.BackColor = System.Drawing.Color.Transparent;
            this.lblSubSector.Appearance = appearance110;
            this.lblSubSector.AutoSize = true;
            this.lblSubSector.Location = new System.Drawing.Point(24, 160);
            this.lblSubSector.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblSubSector.Name = "lblSubSector";
            this.lblSubSector.Size = new System.Drawing.Size(94, 18);
            this.lblSubSector.TabIndex = 71;
            this.lblSubSector.Tag = "SubSector";
            this.lblSubSector.Text = "UDA Sub Sector:";
            // 
            // ugpcAssetID
            // 
            this.ugpcAssetID.ColumnKey = "UDAAssetClassID";
            appearance111.ForeColorDisabled = System.Drawing.Color.Black;
            this.ugpcAssetID.EditAppearance = appearance111;
            this.ugpcAssetID.Location = new System.Drawing.Point(168, 112);
            this.ugpcAssetID.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcAssetID.Name = "ugpcAssetID";
            this.ugpcAssetID.Size = new System.Drawing.Size(167, 24);
            this.ugpcAssetID.TabIndex = 32;
            this.ugpcAssetID.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcAssetID.Text = "Band or column does not exist.";
            // 
            // ugpcUDASector
            // 
            this.ugpcUDASector.ColumnKey = "UDASectorID";
            appearance112.ForeColor = System.Drawing.Color.Black;
            this.ugpcUDASector.EditAppearance = appearance112;
            this.ugpcUDASector.Location = new System.Drawing.Point(589, 113);
            this.ugpcUDASector.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcUDASector.Name = "ugpcUDASector";
            this.ugpcUDASector.Size = new System.Drawing.Size(167, 24);
            this.ugpcUDASector.TabIndex = 33;
            this.ugpcUDASector.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcUDASector.Text = "Band or column does not exist.";
            // 
            // lblUDACountry
            // 
            appearance113.BackColor = System.Drawing.Color.Transparent;
            this.lblUDACountry.Appearance = appearance113;
            this.lblUDACountry.AutoSize = true;
            this.lblUDACountry.Location = new System.Drawing.Point(471, 76);
            this.lblUDACountry.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblUDACountry.Name = "lblUDACountry";
            this.lblUDACountry.Size = new System.Drawing.Size(79, 18);
            this.lblUDACountry.TabIndex = 73;
            this.lblUDACountry.Tag = "Country";
            this.lblUDACountry.Text = "UDA Country:";
            // 
            // lblUDASector
            // 
            appearance114.BackColor = System.Drawing.Color.Transparent;
            this.lblUDASector.Appearance = appearance114;
            this.lblUDASector.AutoSize = true;
            this.lblUDASector.Location = new System.Drawing.Point(471, 117);
            this.lblUDASector.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblUDASector.Name = "lblUDASector";
            this.lblUDASector.Size = new System.Drawing.Size(70, 18);
            this.lblUDASector.TabIndex = 70;
            this.lblUDASector.Tag = "Sector";
            this.lblUDASector.Text = "UDA Sector:";
            // 
            // ugcpUDASecurity
            // 
            this.ugcpUDASecurity.ColumnKey = "UDASecurityTypeID";
            appearance115.ForeColor = System.Drawing.Color.Black;
            this.ugcpUDASecurity.EditAppearance = appearance115;
            this.ugcpUDASecurity.Location = new System.Drawing.Point(168, 74);
            this.ugcpUDASecurity.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugcpUDASecurity.Name = "ugcpUDASecurity";
            this.ugcpUDASecurity.Size = new System.Drawing.Size(167, 24);
            this.ugcpUDASecurity.TabIndex = 29;
            this.ugcpUDASecurity.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugcpUDASecurity.Text = "Band or column does not exist.";
            // 
            // lblSecType
            // 
            appearance116.BackColor = System.Drawing.Color.Transparent;
            this.lblSecType.Appearance = appearance116;
            this.lblSecType.AutoSize = true;
            this.lblSecType.Location = new System.Drawing.Point(24, 74);
            this.lblSecType.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblSecType.Name = "lblSecType";
            this.lblSecType.Size = new System.Drawing.Size(108, 18);
            this.lblSecType.TabIndex = 74;
            this.lblSecType.Tag = "SecurityType";
            this.lblSecType.Text = "UDA Security Type:";
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnPreTab);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnEditSecurity);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnNextTab);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblPageDetail);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnCancel);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnOK);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(877, 345);
            this.ultraPanel1.TabIndex = 106;
            // 
            // btnPreTab
            // 
            this.btnPreTab.Location = new System.Drawing.Point(213, 362);
            this.btnPreTab.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnPreTab.Name = "btnPreTab";
            this.btnPreTab.Size = new System.Drawing.Size(107, 24);
            this.btnPreTab.TabIndex = 40;
            this.btnPreTab.Text = "&Back";
            this.btnPreTab.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPreTab.Click += new System.EventHandler(this.btnPreTab_Click);
            // 
            // btnEditSecurity
            // 
            this.btnEditSecurity.Location = new System.Drawing.Point(451, 362);
            this.btnEditSecurity.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnEditSecurity.Name = "btnEditSecurity";
            this.btnEditSecurity.Size = new System.Drawing.Size(101, 24);
            this.btnEditSecurity.TabIndex = 42;
            this.btnEditSecurity.Text = "&Edit";
            this.btnEditSecurity.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEditSecurity.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // btnNextTab
            // 
            this.btnNextTab.Location = new System.Drawing.Point(341, 362);
            this.btnNextTab.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnNextTab.Name = "btnNextTab";
            this.btnNextTab.Size = new System.Drawing.Size(101, 24);
            this.btnNextTab.TabIndex = 41;
            this.btnNextTab.Text = "&Next";
            this.btnNextTab.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNextTab.Click += new System.EventHandler(this.btnNextTab_Click);
            // 
            // lblPageDetail
            // 
            appearance104.BackColor = System.Drawing.Color.Transparent;
            appearance104.ForeColor = System.Drawing.Color.White;
            this.lblPageDetail.Appearance = appearance104;
            this.lblPageDetail.AutoSize = true;
            this.lblPageDetail.Location = new System.Drawing.Point(87, 366);
            this.lblPageDetail.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblPageDetail.Name = "lblPageDetail";
            this.lblPageDetail.Size = new System.Drawing.Size(69, 18);
            this.lblPageDetail.TabIndex = 103;
            this.lblPageDetail.Tag = "";
            this.lblPageDetail.Text = "Page: 1 of 3";
            // 
            // btnCancel
            // 
            this.btnCancel.AcceptsFocus = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(672, 362);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 24);
            this.btnCancel.TabIndex = 45;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(565, 362);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(101, 24);
            this.btnOK.TabIndex = 44;
            this.btnOK.Text = "&Save";
            this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOK.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tabDynamicUDA
            // 
            this.tabDynamicUDA.Controls.Add(this.gbDynamicUDA);
            this.tabDynamicUDA.Location = new System.Drawing.Point(-10000, -10000);
            this.tabDynamicUDA.Name = "tabDynamicUDA";
            this.tabDynamicUDA.Size = new System.Drawing.Size(877, 345);
            // 
            // gbDynamicUDA
            // 
            this.gbDynamicUDA.ContentAreaAppearance = appearance117;
            this.gbDynamicUDA.Controls.Add(this.ctrlDynamicUDASymbol1);
            this.gbDynamicUDA.ForeColor = System.Drawing.Color.White;
            appearance118.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance118.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance118.ForeColor = System.Drawing.Color.White;
            this.gbDynamicUDA.HeaderAppearance = appearance118;
            this.gbDynamicUDA.Location = new System.Drawing.Point(6, 30);
            this.gbDynamicUDA.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.gbDynamicUDA.Name = "gbDynamicUDA";
            this.gbDynamicUDA.Size = new System.Drawing.Size(864, 284);
            this.gbDynamicUDA.TabIndex = 107;
            this.gbDynamicUDA.Text = "UDA Details";
            this.gbDynamicUDA.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // ctrlDynamicUDASymbol1
            // 
            this.ctrlDynamicUDASymbol1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlDynamicUDASymbol1.Location = new System.Drawing.Point(2, 22);
            this.ctrlDynamicUDASymbol1.Name = "ctrlDynamicUDASymbol1";
            this.ctrlDynamicUDASymbol1.Size = new System.Drawing.Size(860, 260);
            this.ctrlDynamicUDASymbol1.TabIndex = 0;
            this.ctrlDynamicUDASymbol1.Tag = "ctrlDynamicUDASymbol1";
            // 
            // lblStatus
            // 
            this.lblStatus.Appearance = appearance118;
            this.lblStatus.Location = new System.Drawing.Point(16, 340);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(829, 24);
            this.lblStatus.TabIndex = 105;
            // 
            // SymbolLookUp_Fill_Panel
            // 
            // 
            // SymbolLookUp_Fill_Panel.ClientArea
            // 
            this.SymbolLookUp_Fill_Panel.ClientArea.Controls.Add(this.splitContainer1);
            this.SymbolLookUp_Fill_Panel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Left);
            this.SymbolLookUp_Fill_Panel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Right);
            this.SymbolLookUp_Fill_Panel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Bottom);
            this.SymbolLookUp_Fill_Panel.ClientArea.Controls.Add(this.statusStrip1);
            this.SymbolLookUp_Fill_Panel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Top);
            this.SymbolLookUp_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.SymbolLookUp_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SymbolLookUp_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.SymbolLookUp_Fill_Panel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.SymbolLookUp_Fill_Panel.Name = "SymbolLookUp_Fill_Panel";
            this.SymbolLookUp_Fill_Panel.Size = new System.Drawing.Size(1288, 553);
            this.SymbolLookUp_Fill_Panel.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1288, 506);
            this.splitContainer1.SplitterDistance = 30;
            this.splitContainer1.SplitterWidth = 3;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer1.TabIndex = 87;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTraded);
            this.panel1.Controls.Add(this.lblSearch);
            this.panel1.Controls.Add(this.cmbbxSMView);
            this.panel1.Controls.Add(this.rdBtnHistSymbols);
            this.panel1.Controls.Add(this.rdBtnOpenSymbols);
            this.panel1.Controls.Add(this.rdBtnSearchSymbols);
            this.panel1.Controls.Add(this.cmbbxSearchCriteria);
            this.panel1.Controls.Add(this.chkbxUnApprovedSec);
            this.panel1.Controls.Add(this.cmbbxMatchOn);
            this.panel1.Controls.Add(this.txtbxInput);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnGetData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1288, 30);
            this.inboxControlStyler1.SetStyleSettings(this.panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.panel1.TabIndex = 85;
            // 
            // lblTraded
            // 
            appearance119.TextHAlignAsString = "Left";
            appearance119.TextVAlignAsString = "Middle";
            this.lblTraded.Appearance = appearance119;
            this.lblTraded.Location = new System.Drawing.Point(747, 3);
            this.lblTraded.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblTraded.Name = "lblTraded";
            this.lblTraded.Size = new System.Drawing.Size(101, 23);
            this.lblTraded.TabIndex = 135;
            this.lblTraded.Text = "Traded Symbols:";
            // 
            // lblSearch
            // 
            appearance120.TextHAlignAsString = "Left";
            appearance120.TextVAlignAsString = "Middle";
            this.lblSearch.Appearance = appearance120;
            this.lblSearch.Location = new System.Drawing.Point(5, 3);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(49, 23);
            this.lblSearch.TabIndex = 134;
            this.lblSearch.Text = "Search:";
            // 
            // cmbbxSMView
            // 
            this.cmbbxSMView.AutoSize = false;
            appearance121.BackColor = System.Drawing.SystemColors.Window;
            appearance121.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxSMView.DisplayLayout.Appearance = appearance121;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbbxSMView.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbbxSMView.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxSMView.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance122.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance122.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance122.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance122.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxSMView.DisplayLayout.GroupByBox.Appearance = appearance122;
            appearance123.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxSMView.DisplayLayout.GroupByBox.BandLabelAppearance = appearance123;
            this.cmbbxSMView.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance124.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance124.BackColor2 = System.Drawing.SystemColors.Control;
            appearance124.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance124.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxSMView.DisplayLayout.GroupByBox.PromptAppearance = appearance124;
            this.cmbbxSMView.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxSMView.DisplayLayout.MaxRowScrollRegions = 1;
            appearance125.BackColor = System.Drawing.SystemColors.Window;
            appearance125.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxSMView.DisplayLayout.Override.ActiveCellAppearance = appearance125;
            appearance126.BackColor = System.Drawing.SystemColors.Highlight;
            appearance126.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxSMView.DisplayLayout.Override.ActiveRowAppearance = appearance126;
            this.cmbbxSMView.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxSMView.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance127.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxSMView.DisplayLayout.Override.CardAreaAppearance = appearance127;
            appearance128.BorderColor = System.Drawing.Color.Silver;
            appearance128.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxSMView.DisplayLayout.Override.CellAppearance = appearance128;
            this.cmbbxSMView.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxSMView.DisplayLayout.Override.CellPadding = 0;
            appearance129.BackColor = System.Drawing.SystemColors.Control;
            appearance129.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance129.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance129.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance129.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxSMView.DisplayLayout.Override.GroupByRowAppearance = appearance129;
            appearance130.TextHAlignAsString = "Left";
            this.cmbbxSMView.DisplayLayout.Override.HeaderAppearance = appearance130;
            this.cmbbxSMView.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxSMView.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance131.BackColor = System.Drawing.SystemColors.Window;
            appearance131.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxSMView.DisplayLayout.Override.RowAppearance = appearance131;
            this.cmbbxSMView.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance132.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxSMView.DisplayLayout.Override.TemplateAddRowAppearance = appearance132;
            this.cmbbxSMView.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxSMView.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxSMView.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxSMView.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxSMView.Location = new System.Drawing.Point(1146, 3);
            this.cmbbxSMView.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.cmbbxSMView.Name = "cmbbxSMView";
            this.cmbbxSMView.Size = new System.Drawing.Size(143, 23);
            this.cmbbxSMView.TabIndex = 133;
            this.cmbbxSMView.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxSMView.ValueChanged += new System.EventHandler(this.cmbbxSMView_ValueChanged);
            // 
            // rdBtnHistSymbols
            // 
            this.rdBtnHistSymbols.Location = new System.Drawing.Point(908, 3);
            this.rdBtnHistSymbols.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.rdBtnHistSymbols.Name = "rdBtnHistSymbols";
            this.rdBtnHistSymbols.Size = new System.Drawing.Size(82, 23);
            this.inboxControlStyler1.SetStyleSettings(this.rdBtnHistSymbols, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rdBtnHistSymbols.TabIndex = 132;
            this.rdBtnHistSymbols.TabStop = true;
            this.rdBtnHistSymbols.Text = "Historical";
            this.rdBtnHistSymbols.UseVisualStyleBackColor = true;
            this.rdBtnHistSymbols.CheckedChanged += new System.EventHandler(this.rdBtnSearchSymbols_CheckedChanged);
            // 
            // rdBtnOpenSymbols
            // 
            this.rdBtnOpenSymbols.Location = new System.Drawing.Point(848, 3);
            this.rdBtnOpenSymbols.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.rdBtnOpenSymbols.Name = "rdBtnOpenSymbols";
            this.rdBtnOpenSymbols.Size = new System.Drawing.Size(59, 23);
            this.inboxControlStyler1.SetStyleSettings(this.rdBtnOpenSymbols, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rdBtnOpenSymbols.TabIndex = 131;
            this.rdBtnOpenSymbols.TabStop = true;
            this.rdBtnOpenSymbols.Text = "Open";
            this.rdBtnOpenSymbols.UseVisualStyleBackColor = true;
            this.rdBtnOpenSymbols.CheckedChanged += new System.EventHandler(this.rdBtnSearchSymbols_CheckedChanged);
            // 
            // rdBtnSearchSymbols
            // 
            this.rdBtnSearchSymbols.Location = new System.Drawing.Point(54, 3);
            this.rdBtnSearchSymbols.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.rdBtnSearchSymbols.Name = "rdBtnSearchSymbols";
            this.rdBtnSearchSymbols.Size = new System.Drawing.Size(73, 23);
            this.inboxControlStyler1.SetStyleSettings(this.rdBtnSearchSymbols, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rdBtnSearchSymbols.TabIndex = 130;
            this.rdBtnSearchSymbols.TabStop = true;
            this.rdBtnSearchSymbols.Text = "Symbol";
            this.rdBtnSearchSymbols.UseVisualStyleBackColor = true;
            this.rdBtnSearchSymbols.CheckedChanged += new System.EventHandler(this.rdBtnSearchSymbols_CheckedChanged);
            // 
            // cmbbxSearchCriteria
            // 
            this.cmbbxSearchCriteria.AutoSize = false;
            appearance133.BackColor = System.Drawing.SystemColors.Window;
            appearance133.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxSearchCriteria.DisplayLayout.Appearance = appearance133;
            ultraGridBand2.ColHeadersVisible = false;
            this.cmbbxSearchCriteria.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbbxSearchCriteria.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxSearchCriteria.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance134.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance134.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance134.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance134.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxSearchCriteria.DisplayLayout.GroupByBox.Appearance = appearance134;
            appearance135.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxSearchCriteria.DisplayLayout.GroupByBox.BandLabelAppearance = appearance135;
            this.cmbbxSearchCriteria.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance136.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance136.BackColor2 = System.Drawing.SystemColors.Control;
            appearance136.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance136.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxSearchCriteria.DisplayLayout.GroupByBox.PromptAppearance = appearance136;
            this.cmbbxSearchCriteria.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxSearchCriteria.DisplayLayout.MaxRowScrollRegions = 1;
            appearance137.BackColor = System.Drawing.SystemColors.Window;
            appearance137.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxSearchCriteria.DisplayLayout.Override.ActiveCellAppearance = appearance137;
            appearance138.BackColor = System.Drawing.SystemColors.Highlight;
            appearance138.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxSearchCriteria.DisplayLayout.Override.ActiveRowAppearance = appearance138;
            this.cmbbxSearchCriteria.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxSearchCriteria.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance139.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxSearchCriteria.DisplayLayout.Override.CardAreaAppearance = appearance139;
            appearance140.BorderColor = System.Drawing.Color.Silver;
            appearance140.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxSearchCriteria.DisplayLayout.Override.CellAppearance = appearance140;
            this.cmbbxSearchCriteria.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxSearchCriteria.DisplayLayout.Override.CellPadding = 0;
            appearance141.BackColor = System.Drawing.SystemColors.Control;
            appearance141.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance141.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance141.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance141.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxSearchCriteria.DisplayLayout.Override.GroupByRowAppearance = appearance141;
            appearance142.TextHAlignAsString = "Left";
            this.cmbbxSearchCriteria.DisplayLayout.Override.HeaderAppearance = appearance142;
            this.cmbbxSearchCriteria.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxSearchCriteria.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance143.BackColor = System.Drawing.SystemColors.Window;
            appearance143.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxSearchCriteria.DisplayLayout.Override.RowAppearance = appearance143;
            this.cmbbxSearchCriteria.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance144.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxSearchCriteria.DisplayLayout.Override.TemplateAddRowAppearance = appearance144;
            this.cmbbxSearchCriteria.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxSearchCriteria.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxSearchCriteria.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxSearchCriteria.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxSearchCriteria.Location = new System.Drawing.Point(136, 3);
            this.cmbbxSearchCriteria.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.cmbbxSearchCriteria.Name = "cmbbxSearchCriteria";
            this.cmbbxSearchCriteria.Size = new System.Drawing.Size(163, 23);
            this.cmbbxSearchCriteria.TabIndex = 1;
            this.cmbbxSearchCriteria.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxSearchCriteria.ValueChanged += new System.EventHandler(this.cmbbxSearchCriteria_ValueChanged);
            // 
            // chkbxUnApprovedSec
            // 
            appearance145.FontData.Name = "Segoe UI";
            appearance145.FontData.SizeInPoints = 9F;
            this.chkbxUnApprovedSec.Appearance = appearance145;
            this.chkbxUnApprovedSec.Location = new System.Drawing.Point(608, 3);
            this.chkbxUnApprovedSec.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.chkbxUnApprovedSec.Name = "chkbxUnApprovedSec";
            this.chkbxUnApprovedSec.Size = new System.Drawing.Size(145, 23);
            this.chkbxUnApprovedSec.TabIndex = 4;
            this.chkbxUnApprovedSec.Text = "Un Approved Only";
            // 
            // cmbbxMatchOn
            // 
            this.cmbbxMatchOn.AutoSize = false;
            appearance146.BackColor = System.Drawing.SystemColors.Window;
            appearance146.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxMatchOn.DisplayLayout.Appearance = appearance146;
            ultraGridBand3.ColHeadersVisible = false;
            this.cmbbxMatchOn.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbbxMatchOn.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxMatchOn.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance147.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance147.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance147.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance147.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxMatchOn.DisplayLayout.GroupByBox.Appearance = appearance147;
            appearance148.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxMatchOn.DisplayLayout.GroupByBox.BandLabelAppearance = appearance148;
            this.cmbbxMatchOn.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance149.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance149.BackColor2 = System.Drawing.SystemColors.Control;
            appearance149.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance149.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxMatchOn.DisplayLayout.GroupByBox.PromptAppearance = appearance149;
            this.cmbbxMatchOn.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxMatchOn.DisplayLayout.MaxRowScrollRegions = 1;
            appearance150.BackColor = System.Drawing.SystemColors.Window;
            appearance150.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxMatchOn.DisplayLayout.Override.ActiveCellAppearance = appearance150;
            appearance151.BackColor = System.Drawing.SystemColors.Highlight;
            appearance151.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxMatchOn.DisplayLayout.Override.ActiveRowAppearance = appearance151;
            this.cmbbxMatchOn.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxMatchOn.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance152.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxMatchOn.DisplayLayout.Override.CardAreaAppearance = appearance152;
            appearance153.BorderColor = System.Drawing.Color.Silver;
            appearance153.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxMatchOn.DisplayLayout.Override.CellAppearance = appearance153;
            this.cmbbxMatchOn.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxMatchOn.DisplayLayout.Override.CellPadding = 0;
            appearance154.BackColor = System.Drawing.SystemColors.Control;
            appearance154.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance154.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance154.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance154.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxMatchOn.DisplayLayout.Override.GroupByRowAppearance = appearance154;
            appearance155.TextHAlignAsString = "Left";
            this.cmbbxMatchOn.DisplayLayout.Override.HeaderAppearance = appearance155;
            this.cmbbxMatchOn.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxMatchOn.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance156.BackColor = System.Drawing.SystemColors.Window;
            appearance156.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxMatchOn.DisplayLayout.Override.RowAppearance = appearance156;
            this.cmbbxMatchOn.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance157.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxMatchOn.DisplayLayout.Override.TemplateAddRowAppearance = appearance157;
            this.cmbbxMatchOn.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxMatchOn.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxMatchOn.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxMatchOn.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxMatchOn.Location = new System.Drawing.Point(307, 3);
            this.cmbbxMatchOn.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.cmbbxMatchOn.Name = "cmbbxMatchOn";
            this.cmbbxMatchOn.Size = new System.Drawing.Size(112, 23);
            this.cmbbxMatchOn.TabIndex = 2;
            this.cmbbxMatchOn.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxMatchOn.ValueChanged += new System.EventHandler(this.cmbbxSearchCriteria_ValueChanged);
            // 
            // txtbxInput
            // 
            this.txtbxInput.AutoSize = false;
            this.txtbxInput.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbxInput.Location = new System.Drawing.Point(427, 3);
            this.txtbxInput.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtbxInput.MaxLength = 100;
            this.txtbxInput.Name = "txtbxInput";
            this.txtbxInput.Size = new System.Drawing.Size(171, 23);
            this.txtbxInput.TabIndex = 3;
            this.txtbxInput.TextChanged += new System.EventHandler(this.txtbxInput_TextChanged);
            this.txtbxInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbxSymbol_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.BackColorInternal = System.Drawing.Color.Silver;
            this.btnSave.Location = new System.Drawing.Point(1079, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(59, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGetData
            // 
            this.btnGetData.BackColorInternal = System.Drawing.Color.Silver;
            this.btnGetData.Location = new System.Drawing.Point(990, 3);
            this.btnGetData.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(80, 23);
            this.btnGetData.TabIndex = 5;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.grdRowEditTemplate);
            this.splitContainer2.Panel1.Controls.Add(this.grdData);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnPrev);
            this.splitContainer2.Panel2.Controls.Add(this.btnNext);
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer2.Panel2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer2.Size = new System.Drawing.Size(1288, 473);
            this.splitContainer2.SplitterDistance = 428;
            this.splitContainer2.SplitterWidth = 3;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer2.TabIndex = 87;
            // 
            // grdRowEditTemplate
            // 
            appearance158.ForeColor = System.Drawing.Color.Black;
            this.grdRowEditTemplate.Appearance = appearance158;
            this.grdRowEditTemplate.AutoScroll = true;
            this.grdRowEditTemplate.Controls.Add(this.tabCntrlSecurity);
            this.grdRowEditTemplate.DialogSettings.AcceptButton = this.btnOK;
            this.grdRowEditTemplate.DialogSettings.CancelButton = this.btnCancel;
            this.grdRowEditTemplate.ForeColor = System.Drawing.Color.Black;
            this.grdRowEditTemplate.Location = new System.Drawing.Point(153, 40);
            this.grdRowEditTemplate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.grdRowEditTemplate.Name = "grdRowEditTemplate";
            this.grdRowEditTemplate.Size = new System.Drawing.Size(877, 345);
            this.grdRowEditTemplate.TabIndex = 86;
            this.grdRowEditTemplate.Visible = false;
            // 
            // tabCntrlSecurity
            // 
            appearance159.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance159.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance159.BorderColor = System.Drawing.Color.Salmon;
            this.tabCntrlSecurity.Appearance = appearance159;
            this.tabCntrlSecurity.Controls.Add(this.tabBasicDetails);
            this.tabCntrlSecurity.Controls.Add(this.tabUDADetails);
            this.tabCntrlSecurity.Controls.Add(this.ultraTabSharedControlsPage2);
            this.tabCntrlSecurity.Controls.Add(this.tabAssetSpecific);
            this.tabCntrlSecurity.Controls.Add(this.tabDynamicUDA);
            this.tabCntrlSecurity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCntrlSecurity.Location = new System.Drawing.Point(0, 0);
            this.tabCntrlSecurity.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tabCntrlSecurity.Name = "tabCntrlSecurity";
            this.tabCntrlSecurity.SharedControls.AddRange(new System.Windows.Forms.Control[] {
            this.ultraPanel1});
            this.tabCntrlSecurity.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.tabCntrlSecurity.Size = new System.Drawing.Size(877, 345);
            this.tabCntrlSecurity.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Wizard;
            this.tabCntrlSecurity.TabIndex = 0;
            ultraTab3.TabPage = this.tabBasicDetails;
            ultraTab3.Text = "Basic Details";
            ultraTab1.TabPage = this.tabAssetSpecific;
            ultraTab1.Text = "Asset Specific Details";
            ultraTab4.TabPage = this.tabUDADetails;
            ultraTab4.Text = "UDA Details";
            ultraTab2.Key = "DynamicUDA";
            ultraTab2.TabPage = this.tabDynamicUDA;
            ultraTab2.Tag = "DynamicUDA";
            ultraTab2.Text = "UDA Details";
            this.tabCntrlSecurity.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab3,
            ultraTab1,
            ultraTab4,
            ultraTab2});
            this.tabCntrlSecurity.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2007;
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Controls.Add(this.ultraPanel1);
            this.ultraTabSharedControlsPage2.Controls.Add(this.lblStatus);
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(877, 345);
            // 
            // grdData
            // 
            this.grdData.ContextMenuStrip = this.mnuSymbolLookup;
            appearance160.BackColor = System.Drawing.Color.Black;
            this.grdData.DisplayLayout.Appearance = appearance160;
            this.grdData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdData.DisplayLayout.GroupByBox.Hidden = true;
            this.grdData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdData.DisplayLayout.MaxRowScrollRegions = 1;
            appearance161.ForeColor = System.Drawing.Color.Black;
            this.grdData.DisplayLayout.Override.ActiveCellAppearance = appearance161;
            appearance162.BackColor = System.Drawing.Color.LightSlateGray;
            appearance162.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance162.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance162.BorderColor = System.Drawing.Color.DimGray;
            appearance162.FontData.BoldAsString = "True";
            appearance162.ForeColor = System.Drawing.Color.White;
            this.grdData.DisplayLayout.Override.ActiveRowAppearance = appearance162;
            this.grdData.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdData.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdData.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.grdData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdData.DisplayLayout.Override.CellPadding = 0;
            this.grdData.DisplayLayout.Override.CellSpacing = 0;
            this.grdData.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance163.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance163.FontData.Name = "Segoe UI";
            appearance163.FontData.SizeInPoints = 9F;
            appearance163.TextHAlignAsString = "Center";
            this.grdData.DisplayLayout.Override.HeaderAppearance = appearance163;
            this.grdData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance164.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance164.ForeColor = System.Drawing.Color.Orange;
            appearance164.TextHAlignAsString = "Right";
            appearance164.TextVAlignAsString = "Middle";
            this.grdData.DisplayLayout.Override.RowAlternateAppearance = appearance164;
            appearance165.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            appearance165.ForeColor = System.Drawing.Color.Orange;
            appearance165.TextHAlignAsString = "Right";
            appearance165.TextVAlignAsString = "Middle";
            this.grdData.DisplayLayout.Override.RowAppearance = appearance165;
            appearance166.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdData.DisplayLayout.Override.RowSelectorAppearance = appearance166;
            this.grdData.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance167.BackColor = System.Drawing.Color.Transparent;
            appearance167.FontData.BoldAsString = "True";
            this.grdData.DisplayLayout.Override.SelectedRowAppearance = appearance167;
            this.grdData.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdData.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdData.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdData.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdData.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            appearance168.BackColor = System.Drawing.SystemColors.Info;
            this.grdData.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance168;
            this.grdData.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdData.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance169.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdData.DisplayLayout.Override.TemplateAddRowAppearance = appearance169;
            this.grdData.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdData.DisplayLayout.UseFixedHeaders = true;
            this.grdData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdData.Location = new System.Drawing.Point(0, 0);
            this.grdData.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(1288, 428);
            this.grdData.TabIndex = 15;
            this.grdData.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdData_AfterCellUpdate);
            this.grdData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdData_InitializeLayout);
            this.grdData.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdData_InitializeRow);
            this.grdData.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdData_CellChange);
            this.grdData.AfterPerformAction += new Infragistics.Win.UltraWinGrid.AfterUltraGridPerformActionEventHandler(this.grdData_AfterPerformAction);
            this.grdData.BeforeRowEditTemplateDisplayed += new Infragistics.Win.UltraWinGrid.BeforeRowEditTemplateDisplayedEventHandler(this.grdData_BeforeRowEditTemplateDisplayed);
            this.grdData.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.grdData_DoubleClickRow);
            this.grdData.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.grdData_AfterColPosChanged);
            this.grdData.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdData_BeforeCustomRowFilterDialog);
            this.grdData.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdData_BeforeColumnChooserDisplayed);
            this.grdData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdData_MouseDown);
            // 
            // mnuSymbolLookup
            // 
            this.mnuSymbolLookup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.approveToolStripMenuItem,
            this.editToolStripMenuItem,
            this.addSymbolToolStripMenuItem,
            this.selectAUECToolStripMenuItem,
            this.saveLayoutToolStripMenuItem,
            this.tradeSymbolToolStripMenuItem,
            this.addPricingInputToolStripMenuItem});
            this.mnuSymbolLookup.Name = "contextMenuStrip1";
            this.mnuSymbolLookup.Size = new System.Drawing.Size(168, 158);
            this.inboxControlStyler1.SetStyleSettings(this.mnuSymbolLookup, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            //this.mnuSymbolLookup.Opening += new System.ComponentModel.CancelEventHandler(this.mnuSymbolLookup_Opening);
            // 
            // approveToolStripMenuItem
            // 
            this.approveToolStripMenuItem.Name = "approveToolStripMenuItem";
            this.approveToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.approveToolStripMenuItem.Text = "Approve";
            this.approveToolStripMenuItem.Click += new System.EventHandler(this.approveToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // addSymbolToolStripMenuItem
            // 
            this.addSymbolToolStripMenuItem.Name = "addSymbolToolStripMenuItem";
            this.addSymbolToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.addSymbolToolStripMenuItem.Text = "Add Symbol";
            this.addSymbolToolStripMenuItem.Click += new System.EventHandler(this.addSymbolToolStripMenuItem_Click);
            // 
            // selectAUECToolStripMenuItem
            // 
            this.selectAUECToolStripMenuItem.Name = "selectAUECToolStripMenuItem";
            this.selectAUECToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.selectAUECToolStripMenuItem.Text = "Select AUEC";
            this.selectAUECToolStripMenuItem.Click += new System.EventHandler(this.btnAuecs_Click);
            // 
            // addpricinginputToolStripMenuItem
            //
            this.addPricingInputToolStripMenuItem.Name = "addPricingInputToolStripMenuItem";
            this.addPricingInputToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.addPricingInputToolStripMenuItem.Text = "Add Pricing Input";
            this.addPricingInputToolStripMenuItem.Click += new System.EventHandler(this.addPricingInputToolStripMenuItem_Click);

            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // tradeSymbolToolStripMenuItem
            // 
            this.tradeSymbolToolStripMenuItem.Name = "tradeSymbolToolStripMenuItem";
            this.tradeSymbolToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.tradeSymbolToolStripMenuItem.Text = "Trade Symbol";
            this.tradeSymbolToolStripMenuItem.Click += new System.EventHandler(this.tradeSymbolToolStripMenuItem_Click);
            // 
            // addPricingInputToolStripMenuItem
            // 
            this.addPricingInputToolStripMenuItem.Name = "addPricingInputToolStripMenuItem";
            this.addPricingInputToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.addPricingInputToolStripMenuItem.Text = "Add Pricing Input";
            this.addPricingInputToolStripMenuItem.Click += new System.EventHandler(this.addPricingInputToolStripMenuItem_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPrev.BackColorInternal = System.Drawing.Color.Silver;
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(592, 22);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(43, 24);
            this.btnPrev.TabIndex = 16;
            this.btnPrev.Text = "<<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNext.BackColorInternal = System.Drawing.Color.Silver;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(655, 22);
            this.btnNext.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(43, 24);
            this.btnNext.TabIndex = 17;
            this.btnNext.Text = ">>";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // _ClientArea_Toolbars_Dock_Area_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ClientArea_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._ClientArea_Toolbars_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this._ClientArea_Toolbars_Dock_Area_Left.Name = "_ClientArea_Toolbars_Dock_Area_Left";
            this._ClientArea_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 506);
            this._ClientArea_Toolbars_Dock_Area_Left.ToolbarsManager = this.toolBarManager;
            // 
            // toolBarManager
            // 
            appearance170.FontData.SizeInPoints = 9F;
            this.toolBarManager.Appearance = appearance170;
            this.toolBarManager.DesignerFlags = 1;
            this.toolBarManager.DockWithinContainer = this.SymbolLookUp_Fill_Panel.ClientArea;
            this.toolBarManager.LockToolbars = true;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            stateButtonTool1.InstanceProps.RecentlyUsed = false;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool1,
            buttonTool5,
            buttonTool55,
            buttonTool56,
            buttonTool1,
            popupMenuTool3,
            popupMenuTool1,
            buttonTool8,
            buttonTool11,
            buttonTool12,
            buttonTool3,
            buttonTool7,
            buttonTool15});
            ultraToolbar1.Text = "Advanced Search";
            this.toolBarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            buttonTool6.SharedPropsInternal.Caption = "Add Security";
            buttonTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool65.SharedPropsInternal.Caption = "OTC Template";
            buttonTool65.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool66.SharedPropsInternal.Caption = "Instrument Type Fields";
            buttonTool66.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            stateButtonTool2.SharedPropsInternal.Caption = "Advanced Search";
            stateButtonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool2.SharedPropsInternal.Caption = "Export To Excel";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool10.SharedPropsInternal.Caption = "Validate Symbol From Live Feed";
            buttonTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool13.SharedPropsInternal.Caption = "Edit UDA";
            buttonTool13.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool14.SharedPropsInternal.Caption = "Future Root Data";
            buttonTool14.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool16.SharedPropsInternal.Caption = "ButtonTool1";
            popupMenuTool2.SharedPropsInternal.Caption = "Auto Clear Data On Search";
            popupMenuTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            popupMenuTool2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool3});
            stateButtonTool4.SharedPropsInternal.Caption = "Clear";
            popupMenuTool4.SharedPropsInternal.Caption = "Save Layout";
            popupMenuTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            popupMenuTool4.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool19,
            buttonTool17});
            buttonTool18.SharedPropsInternal.Caption = "Clear Layout";
            buttonTool20.SharedPropsInternal.Caption = "Save Layout";
            buttonTool4.SharedPropsInternal.Caption = "AUEC Mappings";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool9.SharedPropsInternal.Caption = "Account Wise UDA";
            buttonTool9.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool21.SharedPropsInternal.Caption = "Dynamic UDA";
            buttonTool21.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            this.toolBarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool6,
            buttonTool65,
            buttonTool66,
            stateButtonTool2,
            buttonTool2,
            buttonTool10,
            buttonTool13,
            buttonTool14,
            buttonTool16,
            popupMenuTool2,
            stateButtonTool4,
            popupMenuTool4,
            buttonTool18,
            buttonTool20,
            buttonTool4,
            buttonTool9,
            buttonTool21});
            this.toolBarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.toolBarManager_BeforeToolbarListDropdown);
            this.toolBarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.toolBarManager_ToolClick);
            // 
            // _ClientArea_Toolbars_Dock_Area_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ClientArea_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1288, 25);
            this._ClientArea_Toolbars_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this._ClientArea_Toolbars_Dock_Area_Right.Name = "_ClientArea_Toolbars_Dock_Area_Right";
            this._ClientArea_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 506);
            this._ClientArea_Toolbars_Dock_Area_Right.ToolbarsManager = this.toolBarManager;
            // 
            // _ClientArea_Toolbars_Dock_Area_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ClientArea_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 531);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_Bottom";
            this._ClientArea_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1288, 0);
            this._ClientArea_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.toolBarManager;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 531);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1288, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 84;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabel1.MouseHover += new System.EventHandler(this.toolStripStatusLabel1_MouseHover);
            // 
            // _ClientArea_Toolbars_Dock_Area_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ClientArea_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this._ClientArea_Toolbars_Dock_Area_Top.Name = "_ClientArea_Toolbars_Dock_Area_Top";
            this._ClientArea_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1288, 25);
            this._ClientArea_Toolbars_Dock_Area_Top.ToolbarsManager = this.toolBarManager;
            // 
            // ugpcVsCurrency
            // 
            this.ugpcVsCurrency.ColumnKey = "VsCurrency";
            this.ugpcVsCurrency.Location = new System.Drawing.Point(256, 29);
            this.ugpcVsCurrency.Name = "ugpcVsCurrency";
            this.ugpcVsCurrency.Size = new System.Drawing.Size(69, 23);
            this.ugpcVsCurrency.TabIndex = 88;
            this.ugpcVsCurrency.Text = "Band or column does not exist.";
            // 
            // lblVsCurrency
            // 
            appearance171.BackColor = System.Drawing.Color.Transparent;
            this.lblVsCurrency.Appearance = appearance171;
            this.lblVsCurrency.AutoSize = true;
            this.lblVsCurrency.Location = new System.Drawing.Point(179, 34);
            this.lblVsCurrency.Name = "lblVsCurrency";
            this.lblVsCurrency.Size = new System.Drawing.Size(70, 14);
            this.lblVsCurrency.TabIndex = 93;
            this.lblVsCurrency.Tag = "Vs Currency";
            this.lblVsCurrency.Text = "Vs Currency:";
            // 
            // ugpcLeadCurrencyID
            // 
            this.ugpcLeadCurrencyID.ColumnKey = "LeadCurrencyID";
            this.ugpcLeadCurrencyID.Location = new System.Drawing.Point(88, 29);
            this.ugpcLeadCurrencyID.Name = "ugpcLeadCurrencyID";
            this.ugpcLeadCurrencyID.Size = new System.Drawing.Size(66, 23);
            this.ugpcLeadCurrencyID.TabIndex = 87;
            this.ugpcLeadCurrencyID.Text = "Band or column does not exist.";
            // 
            // lblLeadCurrencyID
            // 
            appearance172.BackColor = System.Drawing.Color.Transparent;
            this.lblLeadCurrencyID.Appearance = appearance172;
            this.lblLeadCurrencyID.AutoSize = true;
            this.lblLeadCurrencyID.Location = new System.Drawing.Point(6, 34);
            this.lblLeadCurrencyID.Name = "lblLeadCurrencyID";
            this.lblLeadCurrencyID.Size = new System.Drawing.Size(78, 14);
            this.lblLeadCurrencyID.TabIndex = 86;
            this.lblLeadCurrencyID.Tag = "LeadCurrency";
            this.lblLeadCurrencyID.Text = "LeadCurrency:";
            // 
            // lblIsNDF
            // 
            appearance173.BackColor = System.Drawing.Color.Transparent;
            this.lblIsNDF.Appearance = appearance173;
            this.lblIsNDF.AutoSize = true;
            this.lblIsNDF.Location = new System.Drawing.Point(535, 32);
            this.lblIsNDF.Name = "lblIsNDF";
            this.lblIsNDF.Size = new System.Drawing.Size(42, 14);
            this.lblIsNDF.TabIndex = 89;
            this.lblIsNDF.Tag = "Is NDF";
            this.lblIsNDF.Text = "Is NDF:";
            // 
            // ugpcIsNDF
            // 
            this.ugpcIsNDF.ColumnKey = "IsNDF";
            this.ugpcIsNDF.Location = new System.Drawing.Point(581, 27);
            this.ugpcIsNDF.Name = "ugpcIsNDF";
            this.ugpcIsNDF.Size = new System.Drawing.Size(35, 23);
            this.ugpcIsNDF.TabIndex = 91;
            this.ugpcIsNDF.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcIsNDF.Text = "Band or column does not exist.";
            // 
            // lblStrickPrice
            // 
            appearance174.BackColor = System.Drawing.Color.Transparent;
            this.lblStrickPrice.Appearance = appearance174;
            this.lblStrickPrice.AutoSize = true;
            this.lblStrickPrice.Location = new System.Drawing.Point(143, 32);
            this.lblStrickPrice.Name = "lblStrickPrice";
            this.lblStrickPrice.Size = new System.Drawing.Size(65, 14);
            this.lblStrickPrice.TabIndex = 64;
            this.lblStrickPrice.Tag = "Strick Price";
            this.lblStrickPrice.Text = "Strick Price:";
            // 
            // upgcStrikePrice
            // 
            this.upgcStrikePrice.ColumnKey = "StrikePrice";
            this.upgcStrikePrice.Location = new System.Drawing.Point(215, 28);
            this.upgcStrikePrice.Name = "upgcStrikePrice";
            this.upgcStrikePrice.Size = new System.Drawing.Size(66, 23);
            this.upgcStrikePrice.TabIndex = 62;
            this.ugpcStrikePrice.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.upgcStrikePrice.Text = "Band or column does not exist.";
            // 
            // ugpcPutOrCall
            // 
            this.ugpcPutOrCall.ColumnKey = "PutOrCall";
            this.ugpcPutOrCall.Location = new System.Drawing.Point(64, 27);
            this.ugpcPutOrCall.Name = "ugpcPutOrCall";
            this.ugpcPutOrCall.Size = new System.Drawing.Size(66, 23);
            this.ugpcPutOrCall.TabIndex = 61;
            this.ugpcPutOrCall.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcPutOrCall.Text = "Band or column does not exist.";
            // 
            // lblPutCall
            // 
            appearance175.BackColor = System.Drawing.Color.Transparent;
            this.lblPutCall.Appearance = appearance175;
            this.lblPutCall.AutoSize = true;
            this.lblPutCall.Location = new System.Drawing.Point(9, 32);
            this.lblPutCall.Name = "lblPutCall";
            this.lblPutCall.Size = new System.Drawing.Size(47, 14);
            this.lblPutCall.TabIndex = 63;
            this.lblPutCall.Tag = "Put/Call";
            this.lblPutCall.Text = "Put/Call:";
            // 
            // chkBxIsFullSearch
            // 
            this.chkBxIsFullSearch.Location = new System.Drawing.Point(1129, 5);
            this.chkBxIsFullSearch.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.chkBxIsFullSearch.Name = "chkBxIsFullSearch";
            this.chkBxIsFullSearch.Size = new System.Drawing.Size(87, 22);
            this.chkBxIsFullSearch.Style = Infragistics.Win.EditCheckStyle.Button;
            this.chkBxIsFullSearch.TabIndex = 5;
            this.chkBxIsFullSearch.Text = "Quick Scan";
            this.chkBxIsFullSearch.Visible = false;
            this.chkBxIsFullSearch.CheckedChanged += new System.EventHandler(this.chkBxIsFullSearch_CheckedChanged);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _SymbolLookUp_UltraFormManager_Dock_Area_Left
            // 
            this._SymbolLookUp_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SymbolLookUp_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._SymbolLookUp_UltraFormManager_Dock_Area_Left.Name = "_SymbolLookUp_UltraFormManager_Dock_Area_Left";
            this._SymbolLookUp_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 553);
            // 
            // _SymbolLookUp_UltraFormManager_Dock_Area_Right
            // 
            this._SymbolLookUp_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SymbolLookUp_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1296, 32);
            this._SymbolLookUp_UltraFormManager_Dock_Area_Right.Name = "_SymbolLookUp_UltraFormManager_Dock_Area_Right";
            this._SymbolLookUp_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 553);
            // 
            // _SymbolLookUp_UltraFormManager_Dock_Area_Top
            // 
            this._SymbolLookUp_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SymbolLookUp_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._SymbolLookUp_UltraFormManager_Dock_Area_Top.Name = "_SymbolLookUp_UltraFormManager_Dock_Area_Top";
            this._SymbolLookUp_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1304, 32);
            // 
            // _SymbolLookUp_UltraFormManager_Dock_Area_Bottom
            // 
            this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 585);
            this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom.Name = "_SymbolLookUp_UltraFormManager_Dock_Area_Bottom";
            this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1304, 8);
            // 
            // ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;
            // 
            // lblShares
            // 
            appearance84.BackColor = System.Drawing.Color.Transparent;
            this.lblShares.Appearance = appearance84;
            this.lblShares.AutoSize = true;
            this.lblShares.Location = new System.Drawing.Point(269, 93);
            this.lblShares.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblShares.Name = "lblShares";
            this.lblShares.Size = new System.Drawing.Size(144, 18);
            this.lblShares.TabIndex = 129;
            this.lblShares.Tag = "SharesOutstanding";
            this.lblShares.Text = "Shares Outstanding:";
            // 
            // ugpcShares
            // 
            this.ugpcShares.ColumnKey = "SharesOutstanding";
            appearance83.ForeColor = System.Drawing.Color.Black;
            this.ugpcShares.EditAppearance = appearance83;
            this.ugpcShares.Location = new System.Drawing.Point(411, 90);
            this.ugpcShares.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ugpcShares.Name = "ugpcShares";
            this.ugpcShares.Size = new System.Drawing.Size(135, 24);
            this.ugpcShares.TabIndex = 130;
            this.ugpcShares.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugpcShares.Text = "Band or column does not exist.";

            this.cbActivSymbolCamelCase.AutoSize = true;
            this.cbActivSymbolCamelCase.Location = new System.Drawing.Point(124, 149);
            this.cbActivSymbolCamelCase.Name = "cbActivSymbolCamelCase";
            this.cbActivSymbolCamelCase.Size = new System.Drawing.Size(83, 14);
            this.inboxControlStyler1.SetStyleSettings(this.cbActivSymbolCamelCase, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cbActivSymbolCamelCase.TabIndex = 131;
            this.cbActivSymbolCamelCase.Text = "Allow camelCase";
            this.cbActivSymbolCamelCase.ForeColor = System.Drawing.Color.Black;
            this.cbActivSymbolCamelCase.UseVisualStyleBackColor = true;
            this.cbActivSymbolCamelCase.CheckedChanged += new System.EventHandler(this.cbActivSymbolCamelCase_CheckedChanged);

            // 
            // SymbolLookUp
            // 
            this.AcceptButton = this.btnGetData;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1304, 593);
            this.Controls.Add(this.chkBxIsFullSearch);
            this.Controls.Add(this.SymbolLookUp_Fill_Panel);
            this.Controls.Add(this._SymbolLookUp_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._SymbolLookUp_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._SymbolLookUp_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._SymbolLookUp_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "SymbolLookUp";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Security Master";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SymbolLookUp_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SymbolLookUp_FormClosed);
            this.Load += new System.EventHandler(this.SymbolLookUp_Load);
            this.tabBasicDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbOptions)).EndInit();
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbSymbology)).EndInit();
            this.gbSymbology.ResumeLayout(false);
            this.gbSymbology.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbBasicDetails)).EndInit();
            this.gbBasicDetails.ResumeLayout(false);
            this.gbBasicDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFuture)).EndInit();
            this.gbFuture.ResumeLayout(false);
            this.gbFuture.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbfx)).EndInit();
            this.gbfx.ResumeLayout(false);
            this.gbfx.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFixedIncome)).EndInit();
            this.gbFixedIncome.ResumeLayout(false);
            this.gbFixedIncome.PerformLayout();
            this.tabAssetSpecific.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbPriceInfo)).EndInit();
            this.gbPriceInfo.ResumeLayout(false);
            this.gbPriceInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbSecDates)).EndInit();
            this.gbSecDates.ResumeLayout(false);
            this.gbSecDates.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbSecurityInfo)).EndInit();
            this.gbSecurityInfo.ResumeLayout(false);
            this.gbSecurityInfo.PerformLayout();
            this.tabUDADetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbUDADetails)).EndInit();
            this.gbUDADetails.ResumeLayout(false);
            this.gbUDADetails.PerformLayout();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            this.tabDynamicUDA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbDynamicUDA)).EndInit();
            this.gbDynamicUDA.ResumeLayout(false);
            this.SymbolLookUp_Fill_Panel.ClientArea.ResumeLayout(false);
            this.SymbolLookUp_Fill_Panel.ClientArea.PerformLayout();
            this.SymbolLookUp_Fill_Panel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxSMView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxSearchCriteria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxUnApprovedSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxMatchOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtbxInput)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRowEditTemplate)).EndInit();
            this.grdRowEditTemplate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabCntrlSecurity)).EndInit();
            this.tabCntrlSecurity.ResumeLayout(false);
            this.ultraTabSharedControlsPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.mnuSymbolLookup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolBarManager)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBxIsFullSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }



        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager toolBarManager;
        private Infragistics.Win.Misc.UltraPanel SymbolLookUp_Fill_Panel;
        private Infragistics.Win.Misc.UltraButton btnGetData;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtbxInput;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.ContextMenuStrip mnuSymbolLookup;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSymbolToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraButton btnNext;
        private Infragistics.Win.Misc.UltraButton btnPrev;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter symbolLookupUltraGridExcelExporter;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxMatchOn;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxSearchCriteria;
        private Infragistics.Win.UltraWinGrid.UltraGridRowEditTemplate grdRowEditTemplate;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCntrlSecurity;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabBasicDetails;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabUDADetails;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxUnApprovedSec;
        private Infragistics.Win.Misc.UltraGroupBox gbBasicDetails;
        private Infragistics.Win.Misc.UltraLabel lblAssetId;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugcpAssetClass;
        private Infragistics.Win.Misc.UltraLabel lblUnderlying;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugcpExchangeID;
        private Infragistics.Win.Misc.UltraLabel lblExchange;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugcpUnderLyingID;
        private System.Windows.Forms.ToolStripMenuItem selectAUECToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPricingInputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem approveToolStripMenuItem;
        private Infragistics.Win.Misc.UltraButton btnUDAUI;
        private Infragistics.Win.Misc.UltraLabel lblUDAAssetClass;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcAssetID;
        private Infragistics.Win.Misc.UltraLabel lblUDACountry;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugcpUDASecurity;
        private Infragistics.Win.Misc.UltraLabel lblSecType;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcUDACountry;
        private Infragistics.Win.Misc.UltraLabel lblUDASector;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcUDASector;
        private Infragistics.Win.Misc.UltraLabel lblSubSector;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcUDASubSector;
        private Infragistics.Win.Misc.UltraGroupBox gbUDADetails;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabAssetSpecific;
        private Infragistics.Win.Misc.UltraButton btnEditSecurity;
        private Infragistics.Win.Misc.UltraButton btnOK;
        private Infragistics.Win.Misc.UltraButton btnNextTab;
        private Infragistics.Win.Misc.UltraButton btnSelectAUEC;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnPreTab;
        private Infragistics.Win.Misc.UltraGroupBox gbFixedIncome;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcSecurityTypeID;
        private Infragistics.Win.Misc.UltraLabel lblSecurityTypeID;
        private Infragistics.Win.Misc.UltraLabel lblDaysToSettlement;
        private Infragistics.Win.Misc.UltraLabel lblCoupon;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcDaysToSettlement;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcIsZero;
        private Infragistics.Win.Misc.UltraLabel lblIsZero;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcCoupon;
        private Infragistics.Win.Misc.UltraLabel lblCouponFrequencyID;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcCouponFrequencyID;
        private Infragistics.Win.Misc.UltraLabel lblFirstCouponDate;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcFirstCouponDate;
        private Infragistics.Win.Misc.UltraLabel lblIssueDate;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcIssueDate;
        private Infragistics.Win.Misc.UltraLabel lblAccrualBasisID;
        private Infragistics.Win.Misc.UltraLabel lblCollateralType;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcAccrualBasisID;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcCollateralType;
        private Infragistics.Win.Misc.UltraGroupBox gbSecurityInfo;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcIDCOOptionSymbol;
        private Infragistics.Win.Misc.UltraLabel lblIDCOOptionSymbol;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcComments;
        private Infragistics.Win.Misc.UltraLabel lblComments;
        private Infragistics.Win.Misc.UltraLabel lblFactSetSymbol;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcFactSetSymbol;
        private Infragistics.Win.Misc.UltraLabel lblActivSymbol;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcActivSymbol;
        private Infragistics.Win.Misc.UltraLabel lblSedolSymbol;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcSedolSymbol;
        private Infragistics.Win.Misc.UltraLabel lblCusipSymbol;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcOSIOptionSymbol;
        private Infragistics.Win.Misc.UltraLabel lblOSIOptionSymbol;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcCusipSymbol;
        private Infragistics.Win.Misc.UltraGroupBox gbfx;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ultraGridCellProxy1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ultraGridCellProxy2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugcpFixedDate;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ultraGridCellProxy4;
        private Infragistics.Win.Misc.UltraLabel lblFixedDateTemplate;
        private Infragistics.Win.Misc.UltraGroupBox gbSecDates;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcCreationDate;
        private Infragistics.Win.Misc.UltraLabel lblCreationDate;
        private Infragistics.Win.Misc.UltraLabel lblModifiedDate;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcModifiedDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcApprovalDate;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcVsCurrency;
        private Infragistics.Win.Misc.UltraLabel lblVsCurrency;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcLeadCurrencyID;
        private Infragistics.Win.Misc.UltraLabel lblLeadCurrencyID;
        private Infragistics.Win.Misc.UltraLabel lblLeadCurrencyValue;
        private Infragistics.Win.Misc.UltraLabel lblVSCurrencyValue;
        private Infragistics.Win.Misc.UltraLabel lblIsNDF;
        //private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcFixingDate;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcIsNDF;
        //private Infragistics.Win.Misc.UltraLabel lblFixingDate;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcISINSymbol;
        private Infragistics.Win.Misc.UltraLabel lblISINSymbol;
        private Infragistics.Win.Misc.UltraGroupBox gbPriceInfo;
        private Infragistics.Win.Misc.UltraLabel lblDelta;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcDelta;
        private Infragistics.Win.Misc.UltraLabel lblRoundLot;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcRoundLot;
        private Infragistics.Win.Misc.UltraGroupBox gbFuture;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcCutOffTime;
        //private Infragistics.Win.Misc.UltraLabel lblExpirationDate;
        //private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcExpirationDate;
        private Infragistics.Win.Misc.UltraLabel lblStrickPrice;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy upgcStrikePrice;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcPutOrCall;
        private Infragistics.Win.Misc.UltraLabel lblPutCall;
        private Infragistics.Win.Misc.UltraGroupBox gbOptions;
        private Infragistics.Win.Misc.UltraLabel lblStrickPrices;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcStrikePrice;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcPutOrCal;
        private Infragistics.Win.Misc.UltraLabel lblPutorCall;
        private Infragistics.Win.Misc.UltraGroupBox gbSymbology;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcProxySymbol;
        private Infragistics.Win.Misc.UltraLabel lblBloombergSymbol;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcBloombergSymbol;
        private Infragistics.Win.Misc.UltraLabel lblBloombergSymbolExCode;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcBloombergSymbolExCode;
        private Infragistics.Win.Misc.UltraLabel lblUnderlyingSymbol;
        private Infragistics.Win.Misc.UltraLabel lblTickerSymbol;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugcpTickerSymbol;
        private Infragistics.Win.Misc.UltraLabel lblExpirationDateTemplate;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugcpExpirationDate;
        private Infragistics.Win.Misc.UltraLabel lblProxySymbol;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcLongName;
        private Infragistics.Win.Misc.UltraLabel lblDescription;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugcpUnderLyingSymbol;
        private System.Windows.Forms.ToolStripMenuItem tradeSymbolToolStripMenuItem;
        private Infragistics.Win.Misc.UltraLabel lblPageDetail;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcMultiplier;
        private Infragistics.Win.Misc.UltraLabel ultraMultiplier;
        private Infragistics.Win.Misc.UltraLabel lblCurrency;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugcpCurrencyID;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcIsSecApproved;
        private Infragistics.Win.Misc.UltraLabel lblApprovalStatus;
        private PranaUltraGrid grdData;
        private Infragistics.Win.Misc.UltraLabel lblUseDefaultUDA;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy gcpcUseDefaultUDA;
        private System.Windows.Forms.RadioButton rdBtnHistSymbols;
        private System.Windows.Forms.RadioButton rdBtnOpenSymbols;
        private System.Windows.Forms.RadioButton rdBtnSearchSymbols;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxSMView;
        private Infragistics.Win.Misc.UltraLabel lblTraded;
        private Infragistics.Win.Misc.UltraLabel lblSearch;
        private Infragistics.Win.Misc.UltraButton btnEditRootData;
        private Infragistics.Win.Misc.UltraLabel lblStatus;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBxIsFullSearch;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcEsignalOptionRoot;
        private Infragistics.Win.Misc.UltraLabel lblESignalOptionRoot;
        private Infragistics.Win.Misc.UltraLabel lblStrikePriceMultiplier;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugcpStrikePriceMultiplier;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ultraGridCellProxy5;
        private Infragistics.Win.Misc.UltraLabel lblBloombergOptionRoot;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SymbolLookUp_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SymbolLookUp_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SymbolLookUp_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SymbolLookUp_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabDynamicUDA;
        private Infragistics.Win.Misc.UltraGroupBox gbDynamicUDA;
        private PL.SecMaster.ctrlDynamicUDASymbol ctrlDynamicUDASymbol1;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
        private Infragistics.Win.UltraWinGrid.UltraGridCellProxy ugpcShares;
        private Infragistics.Win.Misc.UltraLabel lblShares;
        private System.Windows.Forms.CheckBox cbActivSymbolCamelCase;
    }
}