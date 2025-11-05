using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.ServiceConnector;
using Prana.Utilities.DateTimeUtilities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Prana.SM.OTC
{



    public class OTCTradeDetailsViewModel : BindableBase, IDisposable
    {


        #region Properties

        private CFDViewModel _cfdViewModel;
        public CFDViewModel CFDViewModel
        {
            get { return _cfdViewModel; }
            set
            {
                _cfdViewModel = value;
                OnPropertyChanged();
            }
        }

        private EquitySwapViewModel _equitySwapViewModel;
        public EquitySwapViewModel EquitySwapViewModel
        {
            get { return _equitySwapViewModel; }
            set
            {
                _equitySwapViewModel = value;
                OnPropertyChanged();
            }
        }

        private ConvertibleBondViewModel _convertibleBondViewModel;
        public ConvertibleBondViewModel ConvertibleBondViewModel
        {
            get { return _convertibleBondViewModel; }
            set
            {
                _convertibleBondViewModel = value;
                OnPropertyChanged();
            }
        }

        private ShowCustomTemplateViewModel _showCustomTemplateViewModel;
        public ShowCustomTemplateViewModel ShowCustomTemplateViewModel
        {
            get { return _showCustomTemplateViewModel; }
            set
            {
                _showCustomTemplateViewModel = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SecMasterOTCData> _Templates;
        public ObservableCollection<SecMasterOTCData> Templates
        {
            get
            {
                return this._Templates;
            }
            set
            {
                if (this._Templates != value)
                {
                    this._Templates = value;
                    this.OnPropertyChanged("Templates");
                }
            }
        }

        private SecMasterOTCData _SelectedTemplate;
        public SecMasterOTCData SelectedTemplate
        {
            get { return _SelectedTemplate; }
            set
            {
                _SelectedTemplate = value;
                OnPropertyChanged("SelectedTemplate");
            }
        }

        private InstrumentType otcInstrumentType;
        public InstrumentType OTCInstrumentType
        {
            get { return otcInstrumentType; }
            set
            {
                otcInstrumentType = value;
                OnPropertyChanged();
            }
        }

        private int templateID;
        public int TemplateID
        {
            get { return templateID; }
            set
            {
                templateID = value;
                OnPropertyChanged();
            }
        }


        private SecMasterOTCDataModel templateData;
        public SecMasterOTCDataModel TemplateData
        {
            get { return templateData; }
            set
            {
                templateData = value;
                OnPropertyChanged("TemplateData");
            }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private DateTime _tradeDate;

        public DateTime TradeDate
        {
            get { return _tradeDate; }
            set { _tradeDate = value; }
        }


        private string isShowHideOnnextButton = "Visible";

        public string IsShowHideOnnextButton
        {
            get { return isShowHideOnnextButton; }
            set { isShowHideOnnextButton = value; OnPropertyChanged(); }
        }

        private string isShowHideOnBackButton = "Collapsed";

        public string IsShowHideOnBackButton
        {
            get { return isShowHideOnBackButton; }
            set { isShowHideOnBackButton = value; OnPropertyChanged(); }
        }

        private string textPageSwitch = "Page 1 of 2";
        public string TextPageSwitch
        {
            get { return textPageSwitch; }
            set { textPageSwitch = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The bring to front
        /// </summary>
        private WindowState _bringToFront;

        /// <summary>
        /// Gets or sets the bring to front.
        /// </summary>
        /// <value>
        /// The bring to front.
        /// </value>
        public WindowState BringToFront
        {
            get { return _bringToFront; }
            set
            {
                if (_bringToFront == WindowState.Minimized)
                    _bringToFront = value;
                else
                {
                    if (value == WindowState.Minimized)
                        _bringToFront = value;
                    else
                    {
                        WindowState currentState = _bringToFront;
                        _bringToFront = WindowState.Minimized;
                        RaisePropertyChangedEvent("BringToFront");
                        _bringToFront = currentState;
                    }
                }
                RaisePropertyChangedEvent("BringToFront");
            }
        }

        #endregion

        public DelegateCommand<Window> SaveOTCTemplateButtonClickedCommand { get; set; }
        public DelegateCommand<Window> CancelOTCTemplateButtonClickedCommand { get; set; }
        public DelegateCommand NextOTCTemplateButtonClickedCommand { get; set; }
        public DelegateCommand BackOTCTemplateButtonClickedCommand { get; set; }
        public DelegateCommand<SecMasterOTCData> TemplateSelectionChangedCommand { get; set; }

        /// <summary>
        /// Occurs when [on OTC Params].
        /// </summary>
        public event EventHandler<OTCTradeData> OnSaveOTCParamsEvent;

        public OTCTradeDetailsViewModel()
        {
            try
            {
                InitializeMembers();
            }
            catch (Exception)
            {


            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateID"></param>
        public OTCTradeDetailsViewModel(int templateID)
        {
            this.TemplateID = templateID;
            InitializeMembers();
            GetTemplateDetailsAsync(templateID);
        }

        /// <summary>
        /// Initialize Members
        /// </summary>
        private void InitializeMembers()
        {
            TemplateSelectionChangedCommand = new DelegateCommand<SecMasterOTCData>(obj => TemplateSelectionChangedCommandAction(obj));

            CancelOTCTemplateButtonClickedCommand = new DelegateCommand<Window>((WindowObj) => CancleButtonClickedCommandAction(WindowObj));
            SaveOTCTemplateButtonClickedCommand = new DelegateCommand<Window>((WindowObj) => SaveTemplateDetailsAsync(WindowObj));
            NextOTCTemplateButtonClickedCommand = new DelegateCommand(() => ChangeControlVisibility(true));
            BackOTCTemplateButtonClickedCommand = new DelegateCommand(() => ChangeControlVisibility(false));
            _equitySwapViewModel = EquitySwapViewModel ?? new EquitySwapViewModel();
            _showCustomTemplateViewModel = ShowCustomTemplateViewModel ?? new ShowCustomTemplateViewModel();
            _cfdViewModel = CFDViewModel ?? new CFDViewModel();
            _convertibleBondViewModel = ConvertibleBondViewModel ?? new ConvertibleBondViewModel();

            Templates = new ObservableCollection<SecMasterOTCData>();
            TemplateData = new SecMasterOTCDataModel();

            Dictionary<int, string> CounterParties = CommonDataCache.CachedDataManager.GetInstance.GetAllCurrencies();
            foreach (var item in CounterParties)
            {
                if (!CurrencyList.ContainsKey(item.Key))
                {
                    CurrencyList.Add(item.Key, item.Value);
                }
            }

        }



        ObservableDictionary<int, string> currencyList = new ObservableDictionary<int, string>();
        public ObservableDictionary<int, string> CurrencyList
        {
            get { return currencyList; }
            set { currencyList = value; }
        }


        private async void GetOTCTemplatesAsync()
        {
            try
            {
                int otcInstrumentTypeId = (int)otcInstrumentType;
                var secMasterOTCData = await SecMasterOTCServiceApi.GetInstance().GetOTCTemplatesAsync(otcInstrumentTypeId);
                Templates = new ObservableCollection<SecMasterOTCData>(secMasterOTCData as List<SecMasterOTCData>);

            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void CancleButtonClickedCommandAction(Window windowObj)
        {

            if (windowObj != null)
                windowObj.Close();
        }

        private void TemplateSelectionChangedCommandAction(SecMasterOTCData template)
        {
            try
            {
                GetTemplateDetailsAsync(template.Id);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void ChangeControlVisibility(bool isVisible)
        {
            if (isVisible)
            {
                IsShowHideOnBackButton = "Visible";
                IsShowHideOnnextButton = "Collapsed";
                TextPageSwitch = "Page 2 of 2";
            }
            else
            {
                IsShowHideOnnextButton = "Visible";
                IsShowHideOnBackButton = "Collapsed";
                TextPageSwitch = "Page 1 of 2";
            }

        }


        /// <summary>
        /// 
        /// </summary>
        private async void GetTemplateDetailsAsync(int templateID)
        {
            try
            {
                SecMasterOTCData templateData = await SecMasterOTCServiceApi.GetInstance().GetOTCTemplatesDetailsAsync(templateID);
                //SecMasterOTCData  _OTCtemplateData = await SecMasterOTCServiceApi.GetInstance().GetOTCTempDataTemplatesDetailsAsync(TemplateID);
                if (templateData != null)
                {
                    DateTime effectiveDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(_tradeDate, templateData.DaysToSettle, 1);
                    TemplateData.EffectiveDate = effectiveDate;
                    switch (otcInstrumentType)
                    {
                        case InstrumentType.EquitySwap:
                            var equitySwapData = templateData as SecMasterEquitySwap;
                            EquitySwapViewModel.SetData(equitySwapData, effectiveDate);
                            ShowCustomTemplateViewModel.SetData(equitySwapData.CustomFields);
                            TemplateData.UniqueIdentifier = "Swap: " + Symbol + " R" + EquitySwapViewModel.EquitySwap.FinanceLeg_FixedRate + "% BP" + EquitySwapViewModel.EquitySwap.FinanceLeg_SpreadBasisPoint + " " + (EquitySwapViewModel.EquitySwap.EquityLeg_BulletSwap ? "Bullet Swap" : EquitySwapViewModel.EquitySwap.EquityLeg_ExpirationDate.ToString("MM/dd/yyyy"));
                            break;

                        case InstrumentType.CFD:
                            var cfdData = templateData as SecMasterCFDData;
                            CFDViewModel.SetData(cfdData);
                            ShowCustomTemplateViewModel.SetData(cfdData.CustomFields);
                            TemplateData.UniqueIdentifier = "CFD: " + Symbol + " R" + CFDViewModel.CFDData.Finance_Fixedrate + "% BP" + CFDViewModel.CFDData.Finance_SpreadBP + " CFD";
                            break;
                        case InstrumentType.ConvertibleBond:
                            var CbondData = templateData as SecMasterConvertibleBondData;
                            ConvertibleBondViewModel.SetData(CbondData, effectiveDate);
                            ShowCustomTemplateViewModel.SetData(CbondData.CustomFields);
                            TemplateData.UniqueIdentifier = "ConvertibleBond: " + Symbol + " R" + ConvertibleBondViewModel.ConvertibleBondView.FinanceLeg_FXRate + "% BP" + ConvertibleBondViewModel.ConvertibleBondView.FinanceLeg_SBPoint + " ConvertibleBond";
                            break;
                    }

                    TemplateData.InstrumentType = OTCInstrumentType;
                    TemplateData.DaysToSettle = templateData.DaysToSettle;
                    TemplateData.Symbol = Symbol;
                    OnPropertyChanged("TemplateData");
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

        }

        /// <summary>
        /// Save Template Details Async
        /// </summary>
        private void SaveTemplateDetailsAsync(Window windowObj)
        {
            try
            {
                if (TemplateData != null)
                {


                    if (TemplateData.InstrumentType.Equals(InstrumentType.EquitySwap))
                    {

                        EquitySwapTradeData equitySwapDataFinal = new EquitySwapTradeData();
                        var equityData = EquitySwapViewModel.EquitySwap;

                        equitySwapDataFinal.InstrumentType = ((int)TemplateData.InstrumentType).ToString();
                        equitySwapDataFinal.ISDACounterParty = 1;// TemplateData.ISDACounterParty.Key;
                        equitySwapDataFinal.Symbol = TemplateData.Symbol;
                        UpdateSwapUniqueIdentifierSymbol();
                        equitySwapDataFinal.UniqueIdentifier = TemplateData.UniqueIdentifier;
                        equitySwapDataFinal.DaysToSettle = TemplateData.DaysToSettle;
                        equitySwapDataFinal.EffectiveDate = TemplateData.EffectiveDate;
                        equitySwapDataFinal.TradeDate = TradeDate;
                        equitySwapDataFinal.EquityLeg_BulletSwap = equityData.EquityLeg_BulletSwap;
                        equitySwapDataFinal.EquityLeg_Frequency = ((int)equityData.SelectedEquityLeg_Frequency).ToString();
                        equitySwapDataFinal.EquityLeg_ExcludeDividends = equityData.EquityLeg_ExcludeDividends;
                        equitySwapDataFinal.EquityLeg_ImpliedCommission = equityData.EquityLeg_ImpliedCommission;
                        equitySwapDataFinal.EquityLeg_FirstPaymentDate = equityData.EquityLeg_FirstPaymentDate;
                        equitySwapDataFinal.EquityLeg_ExpirationDate = equityData.EquityLeg_ExpirationDate;


                        equitySwapDataFinal.FinanceLeg_DayCount = (int)equityData.FinanceLeg_DayCount;
                        equitySwapDataFinal.FinanceLeg_FixedRate = equityData.FinanceLeg_FixedRate;
                        equitySwapDataFinal.FinanceLeg_Frequency = ((int)equityData.FinanceLeg_Frequency).ToString();
                        equitySwapDataFinal.FinanceLeg_InterestRate = (int)equityData.FinanceLeg_InterestRate;
                        equitySwapDataFinal.FinanceLeg_SpreadBasisPoint = equityData.FinanceLeg_SpreadBasisPoint;
                        equitySwapDataFinal.FinanceLeg_FirstPaymentDate = equityData.FinanceLeg_FirstPaymentDate;
                        equitySwapDataFinal.FinanceLeg_FirstResetDate = equityData.FinanceLeg_FirstResetDate;

                        equitySwapDataFinal.CommissionBasis = ((int)equityData.CommissionBasis).ToString();
                        equitySwapDataFinal.HardCommissionRate = equityData.HardCommissionRate;
                        equitySwapDataFinal.SoftCommissionRate = equityData.SoftCommissionRate;


                        if (ShowCustomTemplateViewModel.CustomFields != null)
                        {
                            foreach (var field in ShowCustomTemplateViewModel.CustomFields)
                            {
                                OTCCustomFields model = new OTCCustomFields()
                                {
                                    DataType = ((int)(field.DataType)).ToString(),
                                    DefaultValue = field.DefaultValue,
                                    ID = field.ID,
                                    InstrumentType = ((int)field.InstrumentType).ToString(),
                                    Name = field.Name
                                };
                                equitySwapDataFinal.CustomFields.Add(model);

                            }

                        }

                        if (OnSaveOTCParamsEvent != null)
                        {
                            OnSaveOTCParamsEvent(this, equitySwapDataFinal);
                        }
                        if (windowObj != null)
                            windowObj.Hide();
                    }
                    else if (TemplateData.InstrumentType.Equals(InstrumentType.CFD))
                    {
                        CFDTradeData cfdDataFinal = new CFDTradeData();
                        var cfdTradeData = CFDViewModel.CFDData;

                        cfdDataFinal.InstrumentType = ((int)TemplateData.InstrumentType).ToString();
                        cfdDataFinal.ISDACounterParty = 1;// TemplateData.ISDACounterParty.Key;
                        cfdDataFinal.Symbol = TemplateData.Symbol;
                        UpdateCFDUniqueIdentifierSymbol();
                        cfdDataFinal.UniqueIdentifier = TemplateData.UniqueIdentifier;
                        cfdDataFinal.DaysToSettle = TemplateData.DaysToSettle;
                        cfdDataFinal.EffectiveDate = TemplateData.EffectiveDate;
                        cfdDataFinal.TradeDate = TradeDate;
                        cfdDataFinal.FinanceLeg_DayCount = (int)cfdTradeData.Finance_DayCount;
                        cfdDataFinal.FinanceLeg_FixedRate = cfdTradeData.Finance_Fixedrate;
                        cfdDataFinal.FinanceLeg_InterestRate = (int)cfdTradeData.Finance_InteresrRatebenchmark;
                        cfdDataFinal.FinanceLeg_SpreadBasisPoint = cfdTradeData.Finance_SpreadBP;
                        cfdDataFinal.CommissionBasis = ((int)cfdTradeData.CFD_Commissionbasis).ToString();
                        cfdDataFinal.HardCommissionRate = cfdTradeData.CFD_HardCommRate;
                        cfdDataFinal.SoftCommissionRate = cfdTradeData.CFD_SoftCommRate;

                        cfdDataFinal.Collateral_DayCount = (int)cfdTradeData.Collateral_DayCount;
                        cfdDataFinal.Collateral_Margin = cfdTradeData.Collateral_Margin;
                        cfdDataFinal.Collateral_Rate = cfdTradeData.Collateral_Rate;


                        if (ShowCustomTemplateViewModel.CustomFields != null)
                        {
                            foreach (var field in ShowCustomTemplateViewModel.CustomFields)
                            {
                                OTCCustomFields model = new OTCCustomFields()
                                {
                                    DataType = ((int)(field.DataType)).ToString(),
                                    DefaultValue = field.DefaultValue,
                                    ID = field.ID,
                                    InstrumentType = ((int)field.InstrumentType).ToString(),
                                    Name = field.Name
                                };
                                cfdDataFinal.CustomFields.Add(model);

                            }

                        }

                        if (OnSaveOTCParamsEvent != null)
                        {
                            OnSaveOTCParamsEvent(this, cfdDataFinal);
                        }
                        if (windowObj != null)
                            windowObj.Hide();

                    }
                    else if (TemplateData.InstrumentType.Equals(InstrumentType.ConvertibleBond))
                    {

                        ConvertibleBondTradeData convertibleBondFinalData = new ConvertibleBondTradeData();
                        var equityData = ConvertibleBondViewModel.ConvertibleBondView;

                        convertibleBondFinalData.InstrumentType = ((int)TemplateData.InstrumentType).ToString();
                        convertibleBondFinalData.ISDACounterParty = 1;// TemplateData.ISDACounterParty.Key;
                        convertibleBondFinalData.Symbol = TemplateData.Symbol;
                        UpdateConvertibleBondUniqueIdentifierSymbol();
                        convertibleBondFinalData.UniqueIdentifier = TemplateData.UniqueIdentifier;
                        convertibleBondFinalData.DaysToSettle = TemplateData.DaysToSettle;
                        convertibleBondFinalData.EffectiveDate = TemplateData.EffectiveDate;
                        convertibleBondFinalData.TradeDate = TradeDate;
                        convertibleBondFinalData.EquityLeg_ConversionPrice = equityData.EquityLeg_ConversionPrice;
                        convertibleBondFinalData.EquityLeg_ConversionRatio = equityData.EquityLeg_ConversionRatio;
                        convertibleBondFinalData.EquityLeg_ConversionDate = TemplateData.EffectiveDate;//equityData.EquityLeg_ConversionDate;
                        convertibleBondFinalData.FinanceLeg_ZeroCoupon = equityData.FinanceLeg_ZeroCoupon;
                        convertibleBondFinalData.FinanceLeg_IRBenchMark = (int)equityData.FinanceLeg_IRBenchMark;
                        convertibleBondFinalData.FinanceLeg_FXRate = equityData.FinanceLeg_FXRate;
                        convertibleBondFinalData.FinanceLeg_DayCount = (int)equityData.FinanceLeg_DayCount;
                        convertibleBondFinalData.FinanceLeg_FXRate = equityData.FinanceLeg_FXRate;
                        convertibleBondFinalData.FinanceLeg_SBPoint = equityData.FinanceLeg_SBPoint;
                        convertibleBondFinalData.FinanceLeg_CouponFreq = (int)equityData.FinanceLeg_CouponFreq;
                        convertibleBondFinalData.FinanceLeg_ParValue = equityData.FinanceLeg_ParValue;
                        convertibleBondFinalData.FinanceLeg_FirstPaymentDate = equityData.FinanceLeg_FirstPaymentDate;
                        convertibleBondFinalData.FinanceLeg_FirstResetDate = equityData.FinanceLeg_FirstResetDate;
                        convertibleBondFinalData.Commission_Basis = (int)equityData.Commission_Basis;
                        convertibleBondFinalData.Commission_HardCommRate = equityData.Commission_HardCommRate;
                        convertibleBondFinalData.Commission_SoftCommRate = equityData.Commission_SoftCommRate;
                        convertibleBondFinalData.Sedol = TemplateData.Sedol;
                        convertibleBondFinalData.Cusip = TemplateData.Cusip;
                        convertibleBondFinalData.Isin = TemplateData.Isin;
                        convertibleBondFinalData.Currency = TemplateData.Currency.Key.ToString();

                        if (ShowCustomTemplateViewModel.CustomFields != null)
                        {
                            foreach (var field in ShowCustomTemplateViewModel.CustomFields)
                            {
                                OTCCustomFields model = new OTCCustomFields()
                                {
                                    DataType = ((int)(field.DataType)).ToString(),
                                    DefaultValue = field.DefaultValue,
                                    ID = field.ID,
                                    InstrumentType = ((int)field.InstrumentType).ToString(),
                                    Name = field.Name
                                };
                                convertibleBondFinalData.CustomFields.Add(model);
                            }
                        }
                        if (OnSaveOTCParamsEvent != null)
                        {
                            OnSaveOTCParamsEvent(this, convertibleBondFinalData);
                        }
                        if (windowObj != null)
                            windowObj.Hide();
                    }
                }

            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

        }

        private void UpdateCFDUniqueIdentifierSymbol()
        {
            TemplateData.UniqueIdentifier = "CFD: " + Symbol + " R" + CFDViewModel.CFDData.Finance_Fixedrate + "% BP" + CFDViewModel.CFDData.Finance_SpreadBP + " CFD";
        }

        private void UpdateSwapUniqueIdentifierSymbol()
        {
            TemplateData.UniqueIdentifier = "Swap: " + Symbol + " R" + EquitySwapViewModel.EquitySwap.FinanceLeg_FixedRate + "% BP" + EquitySwapViewModel.EquitySwap.FinanceLeg_SpreadBasisPoint + " " + (EquitySwapViewModel.EquitySwap.EquityLeg_BulletSwap ? "Bullet Swap" : EquitySwapViewModel.EquitySwap.EquityLeg_ExpirationDate.ToString("MM/dd/yyyy"));
        }

        private void UpdateConvertibleBondUniqueIdentifierSymbol()
        {
            TemplateData.UniqueIdentifier = "ConvertibleBond: " + Symbol + " R" + ConvertibleBondViewModel.ConvertibleBondView.FinanceLeg_FXRate + "% BP" + ConvertibleBondViewModel.ConvertibleBondView.FinanceLeg_SBPoint + " ConvertibleBond";
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _equitySwapViewModel = null;
                _showCustomTemplateViewModel = null;
                _cfdViewModel = null;
                _convertibleBondViewModel = null;
            }
        }

        public void SetData(string symbol)
        {
            Symbol = symbol;
            EquitySwapViewModel.SetTradeView(true);
            ConvertibleBondViewModel.SetTradeView(true);
        }


        /// <summary>
        /// Set Data for Trade 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="instrumentType"></param>
        public void SetData(string symbol, DateTime tradeDate, InstrumentType instrumentType)
        {
            try
            {
                _symbol = symbol;
                _tradeDate = tradeDate;
                otcInstrumentType = instrumentType;
                EquitySwapViewModel.SetTradeView(true);
                ConvertibleBondViewModel.SetTradeView(true);
                GetOTCTemplatesAsync();

                TemplateData.Symbol = symbol;

                if (instrumentType.Equals(InstrumentType.CFD))
                {
                    TemplateData.IsEquitySwapVisible = Visibility.Collapsed;
                    TemplateData.IsEquityCFDVisible = Visibility.Visible;
                    TemplateData.IsConvertibleBondVisible = Visibility.Collapsed;

                }
                else if (instrumentType.Equals(InstrumentType.EquitySwap))
                {
                    TemplateData.IsEquitySwapVisible = Visibility.Visible;
                    TemplateData.IsEquityCFDVisible = Visibility.Collapsed;
                    TemplateData.IsConvertibleBondVisible = Visibility.Collapsed;
                }
                else
                {
                    TemplateData.IsEquitySwapVisible = Visibility.Collapsed;
                    TemplateData.IsEquityCFDVisible = Visibility.Collapsed;
                    TemplateData.IsConvertibleBondVisible = Visibility.Visible;
                }


            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }




    }
}
