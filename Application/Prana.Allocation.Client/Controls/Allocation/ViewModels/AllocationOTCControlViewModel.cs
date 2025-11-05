using Prana.BusinessObjects;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.SM.OTC;
using Prana.Utilities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{



    public class AllocationOTCControlViewModel : BindableBase, IDisposable
    {
        #region Events

        public event EventHandler<EventArgs<OTCTradeData>> SaveOTCClickedEvent;
        #endregion Events

        #region Properties

        /// <summary>
        /// The _enable control
        /// </summary>
        private bool _enableControl;
        /// <summary>
        /// Gets or sets a value indicating whether [enable control].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable control]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableControl
        {
            get { return _enableControl; }
            set
            {
                _enableControl = value;
                RaisePropertyChangedEvent("EnableControl");
            }
        }

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


        private string textPageSwitch = "Page 1 of 2";
        public string TextPageSwitch
        {
            get { return textPageSwitch; }
            set { textPageSwitch = value; OnPropertyChanged(); }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _groupId;

        public string GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }


        ObservableDictionary<int, string> currencyList = new ObservableDictionary<int, string>();
        public ObservableDictionary<int, string> CurrencyList
        {
            get { return currencyList; }
            set { currencyList = value; }
        }

        private KeyValuePair<int, string> currency;
        public KeyValuePair<int, string> Currency
        {
            get { return currency; }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }
        private Visibility isShowHideOnnextButton = Visibility.Visible;

        public Visibility IsShowHideOnnextButton
        {
            get { return isShowHideOnnextButton; }
            set { isShowHideOnnextButton = value; OnPropertyChanged(); }
        }

        private Visibility isShowHideOnBackButton = Visibility.Collapsed;

        public Visibility IsShowHideOnBackButton
        {
            get { return isShowHideOnBackButton; }
            set { isShowHideOnBackButton = value; OnPropertyChanged(); }
        }

        #endregion

        public DelegateCommand SaveOTCTemplateButtonClickedCommand { get; set; }
        public DelegateCommand NextOTCTemplateButtonClickedCommand { get; set; }
        public DelegateCommand BackOTCTemplateButtonClickedCommand { get; set; }


        ///// <summary>
        ///// Occurs when [on OTC Params].
        ///// </summary>
        //public event EventHandler<SecMasterOTCData> OnSaveOTCParamsEvent;

        public AllocationOTCControlViewModel()
        {
            try
            {
                InitializeMembers();
            }
            catch (Exception)
            {


            }
        }
        Dictionary<int, string> CounterParties = new Dictionary<int, string>();
        /// <summary>
        /// Initialize Members
        /// </summary>
        private void InitializeMembers()
        {
            try
            {

                SaveOTCTemplateButtonClickedCommand = new DelegateCommand(() => SaveTemplateDetailsAsync());
                NextOTCTemplateButtonClickedCommand = new DelegateCommand(() => ChangeControlVisibility(true));
                BackOTCTemplateButtonClickedCommand = new DelegateCommand(() => ChangeControlVisibility(false));
                _equitySwapViewModel = EquitySwapViewModel ?? new EquitySwapViewModel();
                _cfdViewModel = CFDViewModel ?? new CFDViewModel();
                _convertibleBondViewModel = ConvertibleBondViewModel ?? new ConvertibleBondViewModel();
                _showCustomTemplateViewModel = ShowCustomTemplateViewModel ?? new ShowCustomTemplateViewModel();
                EquitySwapViewModel.SetTradeView(true);
                ConvertibleBondViewModel.SetTradeView(true);
                CounterParties = CommonDataCache.CachedDataManager.GetInstance.GetAllCurrencies();
                foreach (var item in CounterParties)
                {
                    if (!CurrencyList.ContainsKey(item.Key))
                    {
                        CurrencyList.Add(item.Key, item.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
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
                IsShowHideOnBackButton = Visibility.Visible;
                IsShowHideOnnextButton = Visibility.Collapsed;
                TextPageSwitch = "Page 2 of 2";
            }
            else
            {
                IsShowHideOnnextButton = Visibility.Visible;
                IsShowHideOnBackButton = Visibility.Collapsed;
                TextPageSwitch = "Page 1 of 2";
            }

        }


        /// <summary>
        /// Sets the preview to swap UI.
        /// </summary>
        /// <param name="group">The group.</param>
        internal void SetPreviewToSwapUI(AllocationGroup group)
        {
            try
            {

                OTCTradeData otcParams = group.OTCParameters;
                if (otcParams != null)
                {
                    TemplateData = new SecMasterOTCDataModel();
                    DateTime effectiveDate = otcParams.EffectiveDate;// BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(group.AUECLocalDate, otcParams.DaysToSettle, 1);
                    TemplateData.EffectiveDate = effectiveDate;
                    TemplateData.TradeDate = otcParams.TradeDate;


                    var instrumentTypeId = otcParams.InstrumentType;
                    InstrumentType instrumentType = (InstrumentType)Enum.Parse(typeof(InstrumentType), instrumentTypeId);


                    switch (instrumentType)
                    {
                        case InstrumentType.EquitySwap:
                            TemplateData.IsEquitySwapVisible = Visibility.Visible;
                            TemplateData.IsEquityCFDVisible = Visibility.Collapsed;
                            TemplateData.IsConvertibleBondVisible = Visibility.Collapsed;
                            var equitySwapData = otcParams as EquitySwapTradeData;
                            EquitySwapViewModel.SetData(equitySwapData);
                            ShowCustomTemplateViewModel.SetData(equitySwapData.CustomFields);
                            break;
                        case InstrumentType.CFD:
                            TemplateData.IsEquitySwapVisible = Visibility.Collapsed;
                            TemplateData.IsEquityCFDVisible = Visibility.Visible;
                            TemplateData.IsConvertibleBondVisible = Visibility.Collapsed;
                            var cfdData = otcParams as CFDTradeData;
                            CFDViewModel.SetData(cfdData);
                            ShowCustomTemplateViewModel.SetData(cfdData.CustomFields);
                            break;
                        case InstrumentType.ConvertibleBond:
                            TemplateData.IsEquitySwapVisible = Visibility.Collapsed;
                            TemplateData.IsEquityCFDVisible = Visibility.Collapsed;
                            TemplateData.IsConvertibleBondVisible = Visibility.Visible;
                            var otcData = otcParams as ConvertibleBondTradeData;
                            ConvertibleBondViewModel.SetData(otcData);
                            TemplateData.Sedol = otcData.Sedol;
                            TemplateData.Cusip = otcData.Cusip;
                            TemplateData.Isin = otcData.Isin;
                            var values = CounterParties.Where(x => otcData.Currency.Equals(x.Key.ToString())).FirstOrDefault();
                            TemplateData.Currency = new KeyValuePair<int, string>(values.Key, values.Value);
                            ShowCustomTemplateViewModel.SetData(otcData.CustomFields);
                            break;
                        default:
                            TemplateData.IsEquitySwapVisible = Visibility.Visible;
                            TemplateData.IsEquityCFDVisible = Visibility.Collapsed;
                            TemplateData.IsConvertibleBondVisible = Visibility.Collapsed;
                            _equitySwapViewModel = EquitySwapViewModel ?? new EquitySwapViewModel();
                            _cfdViewModel = CFDViewModel ?? new CFDViewModel();
                            _convertibleBondViewModel = ConvertibleBondViewModel ?? new ConvertibleBondViewModel();
                            _showCustomTemplateViewModel = ShowCustomTemplateViewModel ?? new ShowCustomTemplateViewModel();
                            TemplateData = null;
                            break;

                    }

                    TemplateData.InstrumentType = instrumentType;
                    TemplateData.DaysToSettle = otcParams.DaysToSettle;
                    TemplateData.Symbol = group.Symbol;
                    TemplateData.UniqueIdentifier = otcParams.UniqueIdentifier;
                    OnPropertyChanged("TemplateData");
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Save Template Details Async
        /// </summary>
        private void SaveTemplateDetailsAsync()
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
                        equitySwapDataFinal.EquityLeg_BulletSwap = equityData.EquityLeg_BulletSwap;
                        equitySwapDataFinal.EquityLeg_Frequency = ((int)equityData.SelectedEquityLeg_Frequency).ToString();
                        equitySwapDataFinal.EquityLeg_ExcludeDividends = equityData.EquityLeg_ExcludeDividends;
                        equitySwapDataFinal.EquityLeg_ImpliedCommission = equityData.EquityLeg_ImpliedCommission;
                        equitySwapDataFinal.EquityLeg_FirstPaymentDate = equityData.EquityLeg_FirstPaymentDate;
                        equitySwapDataFinal.EquityLeg_ExpirationDate = equityData.EquityLeg_ExpirationDate;
                        equitySwapDataFinal.TradeDate = TemplateData.TradeDate;

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
                            equitySwapDataFinal.CustomFieldJsonString = JsonHelper.SerializeObject(equitySwapDataFinal.CustomFields);
                        }


                        if (SaveOTCClickedEvent != null)
                            SaveOTCClickedEvent(this, new EventArgs<OTCTradeData>(equitySwapDataFinal));

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
                        cfdDataFinal.FinanceLeg_DayCount = (int)cfdTradeData.Finance_DayCount;
                        cfdDataFinal.FinanceLeg_FixedRate = cfdTradeData.Finance_Fixedrate;
                        cfdDataFinal.FinanceLeg_InterestRate = (int)cfdTradeData.Finance_InteresrRatebenchmark;
                        cfdDataFinal.FinanceLeg_SpreadBasisPoint = cfdTradeData.Finance_SpreadBP;
                        cfdDataFinal.CommissionBasis = ((int)cfdTradeData.CFD_Commissionbasis).ToString();
                        cfdDataFinal.HardCommissionRate = cfdTradeData.CFD_HardCommRate;
                        cfdDataFinal.SoftCommissionRate = cfdTradeData.CFD_SoftCommRate;
                        cfdDataFinal.TradeDate = TemplateData.TradeDate;
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
                            cfdDataFinal.CustomFieldJsonString = JsonHelper.SerializeObject(cfdDataFinal.CustomFields);
                        }
                        if (SaveOTCClickedEvent != null)
                            SaveOTCClickedEvent(this, new EventArgs<OTCTradeData>(cfdDataFinal));

                    }
                    else if (TemplateData.InstrumentType.Equals(InstrumentType.ConvertibleBond))
                    {

                        ConvertibleBondTradeData convertibleBondFinalData = new ConvertibleBondTradeData();
                        var convertibleBondData = ConvertibleBondViewModel.ConvertibleBondView;

                        convertibleBondFinalData.InstrumentType = ((int)TemplateData.InstrumentType).ToString();
                        convertibleBondFinalData.ISDACounterParty = 1;// TemplateData.ISDACounterParty.Key;
                        convertibleBondFinalData.Symbol = TemplateData.Symbol;
                        UpdateConvertibleBondUniqueIdentifierSymbol();
                        convertibleBondFinalData.UniqueIdentifier = TemplateData.UniqueIdentifier;
                        convertibleBondFinalData.DaysToSettle = TemplateData.DaysToSettle;
                        convertibleBondFinalData.EffectiveDate = TemplateData.EffectiveDate;
                        convertibleBondFinalData.TradeDate = TemplateData.TradeDate;
                        convertibleBondFinalData.EquityLeg_ConversionPrice = convertibleBondData.EquityLeg_ConversionPrice;
                        convertibleBondFinalData.EquityLeg_ConversionRatio = convertibleBondData.EquityLeg_ConversionRatio;
                        convertibleBondFinalData.EquityLeg_ConversionDate = TemplateData.EffectiveDate;//equityData.EquityLeg_ConversionDate;
                        convertibleBondFinalData.FinanceLeg_ZeroCoupon = convertibleBondData.FinanceLeg_ZeroCoupon;
                        convertibleBondFinalData.FinanceLeg_IRBenchMark = (int)convertibleBondData.FinanceLeg_IRBenchMark;
                        convertibleBondFinalData.FinanceLeg_FXRate = convertibleBondData.FinanceLeg_FXRate;
                        convertibleBondFinalData.FinanceLeg_DayCount = (int)convertibleBondData.FinanceLeg_DayCount;
                        convertibleBondFinalData.FinanceLeg_FXRate = convertibleBondData.FinanceLeg_FXRate;
                        convertibleBondFinalData.FinanceLeg_SBPoint = convertibleBondData.FinanceLeg_SBPoint;
                        convertibleBondFinalData.FinanceLeg_CouponFreq = (int)convertibleBondData.FinanceLeg_CouponFreq;
                        convertibleBondFinalData.FinanceLeg_ParValue = convertibleBondData.FinanceLeg_ParValue;
                        convertibleBondFinalData.FinanceLeg_FirstPaymentDate = convertibleBondData.FinanceLeg_FirstPaymentDate;
                        convertibleBondFinalData.FinanceLeg_FirstResetDate = convertibleBondData.FinanceLeg_FirstResetDate;
                        convertibleBondFinalData.Commission_Basis = (int)convertibleBondData.Commission_Basis;
                        convertibleBondFinalData.Commission_HardCommRate = convertibleBondData.Commission_HardCommRate;
                        convertibleBondFinalData.Commission_SoftCommRate = convertibleBondData.Commission_SoftCommRate;
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
                            convertibleBondFinalData.CustomFieldJsonString = JsonHelper.SerializeObject(convertibleBondFinalData.CustomFields);
                        }
                        if (SaveOTCClickedEvent != null)
                            SaveOTCClickedEvent(this, new EventArgs<OTCTradeData>(convertibleBondFinalData));

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
            }

        }

        public void SetDataFromAllocation(string groupId)
        {
            GroupId = groupId;
            EquitySwapViewModel.SetTradeView(true);
            ConvertibleBondViewModel.SetTradeView(true);

        }

        public void SetData(string symbol)
        {
            Symbol = symbol;
            EquitySwapViewModel.SetTradeView(true);
            ConvertibleBondViewModel.SetTradeView(true);
        }



    }
}
