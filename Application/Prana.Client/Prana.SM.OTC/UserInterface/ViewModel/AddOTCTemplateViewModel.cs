using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.ServiceConnector;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Prana.SM.OTC
{

    public enum InstrumentType
    {
        EquitySwap = 1,
        CFD = 2,
        ConvertibleBond = 3
    }

    public enum BooleanValue
    {
        Yes = 1,
        No = 0
    }
    public enum AssetClass
    {
        Equity = 1,
        EquityOption = 2,
        Future = 3,
        FutureOption = 4
    }


    public class AddOTCTemplateViewModel : BindableBase, IDisposable
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

        private InstrumentType selectedInstrumentType;
        public InstrumentType SelectedInstrumentType
        {
            get { return selectedInstrumentType; }
            set { selectedInstrumentType = value; }
        }

        private SecMasterOTCDataModel templateData;
        public SecMasterOTCDataModel TemplateData
        {
            get { return templateData; }
            set
            {
                templateData = value;
                OnPropertyChanged();
            }
        }

        private Visibility isShowHideOnnextButton = Visibility.Visible;
        public Visibility IsShowHideOnnextButton
        {
            get { return isShowHideOnnextButton; }
            set { isShowHideOnnextButton = value; OnPropertyChanged(); }
        }

        private string textPageSwitch = "Page 1 of 2";
        public string TextPageSwitch
        {
            get { return textPageSwitch; }
            set { textPageSwitch = value; OnPropertyChanged(); }
        }

        private Visibility isShowHideOnBackButton = Visibility.Collapsed;

        public Visibility IsShowHideOnBackButton
        {
            get { return isShowHideOnBackButton; }
            set { isShowHideOnBackButton = value; OnPropertyChanged(); }
        }

        private bool isEnableInstrumentType = true;
        public bool IsEnableInstrumentType
        {
            get { return isEnableInstrumentType; }
            set
            {
                isEnableInstrumentType = value;
                OnPropertyChanged("IsEnableInstrumentType");
            }
        }


        Visibility isVisibleLblNotificationBar = Visibility.Hidden;
        public Visibility IsVisibleLblNotificationBar
        {
            get { return isVisibleLblNotificationBar; }
            set { isVisibleLblNotificationBar = value; OnPropertyChanged(); }
        }

        Color notificationBarColor = Color.Green;
        public Color NotificationBarColor
        {
            get { return notificationBarColor; }
            set { notificationBarColor = value; OnPropertyChanged(); }
        }

        private string notificationBarContent;
        public string NotificationBarContent
        {
            get { return notificationBarContent; }
            set
            {
                notificationBarContent = value;
                OnPropertyChanged();
            }
        }


        ObservableDictionary<int, string> counterPartiesList = new ObservableDictionary<int, string>();
        public ObservableDictionary<int, string> CounterPartiesList
        {
            get { return counterPartiesList; }
            set { counterPartiesList = value; }
        }

        /// <summary>
        /// Gets or sets the form close button.
        /// </summary>
        /// <value>
        /// The form close button.
        /// </value>
        public DelegateCommand<object> FormCloseButton { get; set; }

        /// <summary>
        /// Gets or sets the form closed button.
        /// </summary>
        /// <value>
        /// The form closed button.
        /// </value>
        public DelegateCommand<object> FormClosed { get; set; }

        /// <summary>
        /// Occurs when [on form close button event].
        /// </summary>
        internal event EventHandler OnFormCloseButtonEvent;



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

        #region Commands

        public DelegateCommand SaveOTCTemplateButtonClickedCommand { get; set; }
        public DelegateCommand NextOTCTemplateButtonClickedCommand { get; set; }
        public DelegateCommand BackOTCTemplateButtonClickedCommand { get; set; }
        public DelegateCommand InstrumentSelectionChangedCommand { get; set; }

        #endregion


        SecMasterOTCData _EquitySwapTemplateData = new SecMasterOTCData();
        SecMasterOTCData _CFDtemplateData = new SecMasterOTCData();
        SecMasterOTCData _OTCtemplateData = new SecMasterOTCData();
        string _createdBy = string.Empty;
        List<SecMasterOTCData> AllSavedTemplates = new List<SecMasterOTCData>();



        /// <summary>
        /// Add OTC TemplateViewModel
        /// </summary>
        public AddOTCTemplateViewModel()
        {
            try
            {
                InitializeMembers();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add OTC TemplateViewModel
        /// </summary>
        /// <param name="templateID"></param>
        public AddOTCTemplateViewModel(int templateID, InstrumentType Itype)
        {
            try
            {
                this.TemplateID = templateID;
                this.SelectedInstrumentType = Itype;
                IsEnableInstrumentType = false;
                InitializeMembers();
                GetTemplateDetailsAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Initialize Members
        /// </summary>
        private void InitializeMembers()
        {
            SaveOTCTemplateButtonClickedCommand = new DelegateCommand(() => SaveTemplateDetailsAsync());
            NextOTCTemplateButtonClickedCommand = new DelegateCommand(() => ChangeControlVisibility(true));
            BackOTCTemplateButtonClickedCommand = new DelegateCommand(() => ChangeControlVisibility(false));
            InstrumentSelectionChangedCommand = new DelegateCommand(() => InstrumentSelectionChangedCommandAction());
            FormCloseButton = new DelegateCommand<object>((parameter) => OnCloseButton(parameter));
            FormClosed = new DelegateCommand<object>((parameter) => OnFormClosed(parameter));
            _equitySwapViewModel = EquitySwapViewModel ?? new EquitySwapViewModel();
            _cfdViewModel = CFDViewModel ?? new CFDViewModel();
            _convertibleBondViewModel = ConvertibleBondViewModel ?? new ConvertibleBondViewModel();
            _showCustomTemplateViewModel = ShowCustomTemplateViewModel ?? new ShowCustomTemplateViewModel();
            TemplateData = new SecMasterOTCDataModel();

            Dictionary<int, string> CounterParties = CommonDataCache.CachedDataManager.GetInstance.GetAllCounterParties();
            foreach (var item in CounterParties)
            {
                if (!CounterPartiesList.ContainsKey(item.Key))
                {
                    CounterPartiesList.Add(item.Key, item.Value);
                }
            }
        }

        private async System.Threading.Tasks.Task GetAllTheTemplates()
        {
            try
            {
                AllSavedTemplates = await SecMasterOTCServiceApi.GetInstance().GetOTCTemplatesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Set Default TemplatesValue
        /// </summary>
        private void SetDefaultTemplatesValue()
        {
            GetTemplateDetailsAsync();
            TemplateData.LastModifiedBy = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.FirstName;
            TemplateData.CreatedBy = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.FirstName;
        }


        private void InstrumentSelectionChangedCommandAction()
        {
            if (TemplateData.InstrumentType == InstrumentType.CFD)
            {
                TemplateData.IsEquitySwapVisible = Visibility.Collapsed;
                TemplateData.IsEquityCFDVisible = Visibility.Visible;
                TemplateData.IsConvertibleBondVisible = Visibility.Collapsed;
                _showCustomTemplateViewModel.SelectedInstrumentType = TemplateData.InstrumentType;

                if (_CFDtemplateData != null && _CFDtemplateData.InstrumentType != null)
                {
                    SetCFDTemplateData(_CFDtemplateData);
                }
            }
            else if (TemplateData.InstrumentType == InstrumentType.EquitySwap)
            {
                TemplateData.IsEquitySwapVisible = Visibility.Visible;
                TemplateData.IsEquityCFDVisible = Visibility.Collapsed;
                TemplateData.IsConvertibleBondVisible = Visibility.Collapsed;
                _showCustomTemplateViewModel.SelectedInstrumentType = TemplateData.InstrumentType;

                if (_EquitySwapTemplateData != null && _EquitySwapTemplateData.InstrumentType != null)
                {
                    SetEquitySwapTemplateData(_EquitySwapTemplateData);
                }
            }
            else if (TemplateData.InstrumentType == InstrumentType.ConvertibleBond)
            {
                TemplateData.IsEquitySwapVisible = Visibility.Collapsed;
                TemplateData.IsEquityCFDVisible = Visibility.Collapsed;
                TemplateData.IsConvertibleBondVisible = Visibility.Visible;
                _showCustomTemplateViewModel.SelectedInstrumentType = TemplateData.InstrumentType;

                if (_OTCtemplateData != null && _OTCtemplateData.InstrumentType != null)
                {
                    SetOTCTemplateData(_OTCtemplateData);
                }
            }
        }
        private string titleBarText = "Template";

        public string TitleBarText
        {
            get { return titleBarText; }
            set { titleBarText = value; OnPropertyChanged("TitlebarText"); }
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
        /// Get Template DetailsAsync
        /// </summary>
        private async void GetTemplateDetailsAsync()
        {
            try
            {
                ShowNotificationMessage("Please wait, Getting data...", 1);

                SecMasterOTCData templateData = new SecMasterOTCData();
                if (SelectedInstrumentType.Equals(InstrumentType.EquitySwap))
                {
                    _EquitySwapTemplateData = await SecMasterOTCServiceApi.GetInstance().GetOTCTemplatesDetailsAsync(TemplateID);
                    if (_EquitySwapTemplateData != null)
                    {
                        _createdBy = CommonDataCache.CachedDataManager.GetInstance.GetUserText(_EquitySwapTemplateData.CreatedBy);
                        TemplateData.InstrumentType = SelectedInstrumentType;
                    }
                }
                else if (SelectedInstrumentType.Equals(InstrumentType.CFD))
                {
                    _CFDtemplateData = await SecMasterOTCServiceApi.GetInstance().GetCFDOTCTemplatesDetailsAsync(TemplateID);
                    if (_CFDtemplateData != null)
                    {
                        _createdBy = CommonDataCache.CachedDataManager.GetInstance.GetUserText(_CFDtemplateData.CreatedBy);
                        TemplateData.InstrumentType = SelectedInstrumentType;
                    }
                }
                else if (SelectedInstrumentType.Equals(InstrumentType.ConvertibleBond))
                {
                    _OTCtemplateData = await SecMasterOTCServiceApi.GetInstance().GetOTCTempDataTemplatesDetailsAsync(TemplateID);
                    if (_OTCtemplateData != null)
                    {
                        _createdBy = CommonDataCache.CachedDataManager.GetInstance.GetUserText(_OTCtemplateData.CreatedBy);
                        TemplateData.InstrumentType = SelectedInstrumentType;
                    }
                }
                else if (SelectedInstrumentType == 0)
                {
                    _createdBy = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.FirstName;
                    _CFDtemplateData = await SecMasterOTCServiceApi.GetInstance().GetCFDOTCTemplatesDetailsAsync((int)InstrumentType.CFD);
                    _EquitySwapTemplateData = await SecMasterOTCServiceApi.GetInstance().GetOTCTemplatesDetailsAsync((int)InstrumentType.EquitySwap);
                    _OTCtemplateData = await SecMasterOTCServiceApi.GetInstance().GetOTCTempDataTemplatesDetailsAsync((int)InstrumentType.ConvertibleBond);
                    TemplateData.InstrumentType = InstrumentType.EquitySwap;
                }

                HideNotificationMessage();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

        }


        /// <summary>
        /// Set OTC TemplateData
        /// </summary>
        /// <param name="createdBy"></param>
        /// <param name="templateData"></param>
        private void SetOTCTemplateData(string createdBy, SecMasterOTCData templateData)
        {
            var instrumentType = InstrumentType.EquitySwap;
            Enum.TryParse(templateData.InstrumentType, out instrumentType);
            var UnderlyingAssetID = AssetClass.Equity;
            Enum.TryParse(templateData.UnderlyingAssetID.ToString(), out UnderlyingAssetID);

            TemplateData.Name = templateData.Name;
            TemplateData.Description = templateData.Description;
            TemplateData.UnderlyingAssetID = UnderlyingAssetID;
            TemplateData.CreationDate = templateData.CreationDate;
            TemplateData.LastModifieDate = templateData.LastModifieDate;
            TemplateData.LastModifiedBy = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.FirstName;
            TemplateData.ISDACounterParty = CounterPartiesList.SingleOrDefault(p => p.Key == templateData.ISDACounterParty);
            TemplateData.DaysToSettle = templateData.DaysToSettle;

            if (templateData.CreatedBy == 0 && SelectedInstrumentType == 0)
            {
                TemplateData.Id = 0;
                TemplateData.Name = string.Empty;
                TemplateData.Description = string.Empty;
            }
            else
            {
                TemplateData.Id = templateData.Id;
            }
            TemplateData.CreatedBy = createdBy == null || createdBy == "" ? "System" : createdBy;

        }

        /// <summary>
        /// Set CFD TemplateData
        /// </summary>
        /// <param name="templateData"></param>
        private void SetCFDTemplateData(SecMasterOTCData templateData)
        {
            var cfdData = templateData as SecMasterCFDData;
            CFDViewModel.SetData(cfdData);
            TitleBarText = cfdData.Name;
            ShowCustomTemplateViewModel.SetData(cfdData.CustomFields);
            SetOTCTemplateData(_createdBy, templateData);
        }

        /// <summary>
        /// SetEquitySwapTemplateData
        /// </summary>
        /// <param name="templateData"></param>
        private void SetEquitySwapTemplateData(SecMasterOTCData templateData)
        {
            var equitySwapData = templateData as SecMasterEquitySwap;
            EquitySwapViewModel.SetData(equitySwapData, DateTime.Now);
            TitleBarText = equitySwapData.Name;
            ShowCustomTemplateViewModel.SetData(equitySwapData.CustomFields);
            SetOTCTemplateData(_createdBy, templateData);
        }

        /// <summary>
        /// SetOTCDataTemplateData
        /// </summary>
        /// <param name="templateData"></param>
        private void SetOTCTemplateData(SecMasterOTCData templateData)
        {
            var otcTempData = templateData as SecMasterConvertibleBondData;
            ConvertibleBondViewModel.SetData(otcTempData, DateTime.Now);
            TitleBarText = otcTempData.Name;
            ShowCustomTemplateViewModel.SetData(otcTempData.CustomFields);
            SetOTCTemplateData(_createdBy, templateData);
        }

        /// <summary>
        /// Save Template Details Async
        /// </summary>
        private async void SaveTemplateDetailsAsync()
        {
            try
            {
                int templateDetails = 2;
                bool isAlreadyExist = false;

                await GetAllTheTemplates();

                isAlreadyExist = AllSavedTemplates.Any(cus => cus.Name == TemplateData.Name);

                if (isAlreadyExist)
                {
                    isAlreadyExist = SelectedInstrumentType != 0 ? false : true;
                }

                if (!isAlreadyExist)
                {
                    if (TemplateData.InstrumentType.Equals(InstrumentType.EquitySwap))
                    {
                        #region Equity Swap
                        if (TemplateData != null && EquitySwapViewModel.EquitySwap != null && TemplateData.Name != "")
                        {
                            var equityData = EquitySwapViewModel.EquitySwap;
                            SecMasterEquitySwap equitySwapDataFinal = new SecMasterEquitySwap();
                            var equitySwapData = EquitySwapViewModel.EquitySwap;
                            equitySwapDataFinal.Id = TemplateData.Id;
                            equitySwapDataFinal.OTCTemplateID = TemplateData.Id;
                            equitySwapDataFinal.Name = TemplateData.Name;
                            equitySwapDataFinal.Description = TemplateData.Description;
                            equitySwapDataFinal.InstrumentType = ((int)TemplateData.InstrumentType).ToString();
                            equitySwapDataFinal.UnderlyingAssetID = (int)TemplateData.UnderlyingAssetID;
                            equitySwapDataFinal.CreatedBy = TemplateData.CreatedBy == "System" ? 0 : CommonDataCache.CachedDataManager.GetInstance.GetUserID(TemplateData.CreatedBy);
                            equitySwapDataFinal.CreationDate = TemplateData.CreationDate;
                            equitySwapDataFinal.DaysToSettle = TemplateData.DaysToSettle;
                            equitySwapDataFinal.LastModifieDate = DateTime.Now;
                            equitySwapDataFinal.ISDACounterParty = templateData.ISDACounterParty.Key;
                            equitySwapDataFinal.LastModifiedBy = CommonDataCache.CachedDataManager.GetInstance.GetUserID(TemplateData.LastModifiedBy);
                            equitySwapDataFinal.CommissionBasis = ((int)equityData.CommissionBasis).ToString();
                            equitySwapDataFinal.EquityLeg_BulletSwap = equityData.EquityLeg_BulletSwap;
                            equitySwapDataFinal.EquityLeg_ExcludeDividends = equityData.EquityLeg_ExcludeDividends;
                            equitySwapDataFinal.EquityLeg_ImpliedCommission = equityData.EquityLeg_ImpliedCommission;
                            equitySwapDataFinal.FinanceLeg_DayCount = (int)equityData.FinanceLeg_DayCount;
                            equitySwapDataFinal.FinanceLeg_FixedRate = equityData.FinanceLeg_FixedRate;
                            equitySwapDataFinal.FinanceLeg_Frequency = ((int)equityData.FinanceLeg_Frequency).ToString();
                            equitySwapDataFinal.FinanceLeg_InterestRate = (int)equityData.FinanceLeg_InterestRate;
                            equitySwapDataFinal.FinanceLeg_SpreadBasisPoint = equityData.FinanceLeg_SpreadBasisPoint;
                            equitySwapDataFinal.HardCommissionRate = equityData.HardCommissionRate;
                            equitySwapDataFinal.EquityLeg_Frequency = ((int)equityData.SelectedEquityLeg_Frequency).ToString();
                            equitySwapDataFinal.SoftCommissionRate = equityData.SoftCommissionRate;
                            if (ShowCustomTemplateViewModel.CustomFields != null)
                            {
                                foreach (var field in ShowCustomTemplateViewModel.CustomFields)
                                {
                                    OTCCustomFields model = new OTCCustomFields()
                                    {
                                        DataType = ((int)(field.DataType)).ToString(),
                                        DefaultValue = field.DataType.Equals(DataTypes.Bool) ? field.DefaultBooleanValue.ToString() : field.DefaultValue,
                                        ID = field.ID,
                                        InstrumentType = ((int)field.InstrumentType).ToString(),
                                        Name = field.Name
                                    };
                                    equitySwapDataFinal.CustomFields.Add(model);
                                }
                            }
                            templateDetails = await SecMasterOTCServiceApi.GetInstance().SaveOTCTemplatesAsync(equitySwapDataFinal);
                        }
                        #endregion
                    }
                    else if (TemplateData.InstrumentType.Equals(InstrumentType.CFD))
                    {
                        #region CFD
                        if (TemplateData != null && CFDViewModel.CFDData != null && TemplateData.Name != "")
                        {
                            var _CFDData = CFDViewModel.CFDData;
                            SecMasterCFDData CFDDataFinal = new SecMasterCFDData();
                            var equitySwapData = CFDViewModel.CFDData;
                            CFDDataFinal.Id = TemplateData.Id;
                            CFDDataFinal.OTCTemplateID = TemplateData.Id;
                            CFDDataFinal.Name = TemplateData.Name;
                            CFDDataFinal.Description = TemplateData.Description;
                            CFDDataFinal.InstrumentType = ((int)TemplateData.InstrumentType).ToString();
                            CFDDataFinal.UnderlyingAssetID = (int)TemplateData.UnderlyingAssetID;
                            CFDDataFinal.CreatedBy = TemplateData.CreatedBy == "System" ? 0 : CommonDataCache.CachedDataManager.GetInstance.GetUserID(TemplateData.CreatedBy);
                            CFDDataFinal.CreationDate = TemplateData.CreationDate;
                            CFDDataFinal.DaysToSettle = TemplateData.DaysToSettle;
                            CFDDataFinal.LastModifieDate = DateTime.Now;
                            CFDDataFinal.ISDACounterParty = templateData.ISDACounterParty.Key;
                            CFDDataFinal.LastModifiedBy = CommonDataCache.CachedDataManager.GetInstance.GetUserID(TemplateData.LastModifiedBy);
                            CFDDataFinal.CFD_Commissionbasis = (int)_CFDData.CFD_Commissionbasis;
                            CFDDataFinal.CFD_HardCommRate = _CFDData.CFD_HardCommRate;
                            CFDDataFinal.CFD_SoftCommRate = _CFDData.CFD_SoftCommRate;
                            CFDDataFinal.Collateral_DayCount = (int)_CFDData.Collateral_DayCount;
                            CFDDataFinal.Collateral_Margin = _CFDData.Collateral_Margin;
                            CFDDataFinal.Collateral_Rate = _CFDData.Collateral_Rate;
                            CFDDataFinal.Finance_DayCount = (int)_CFDData.Finance_DayCount;
                            CFDDataFinal.Finance_Fixedrate = _CFDData.Finance_Fixedrate;
                            CFDDataFinal.Finance_InteresrRatebenchmark = (int)_CFDData.Finance_InteresrRatebenchmark;
                            CFDDataFinal.Finance_ScriptlendingFee = _CFDData.Finance_ScriptlendingFee;
                            CFDDataFinal.Finance_SpreadBP = _CFDData.Finance_SpreadBP;
                            if (ShowCustomTemplateViewModel.CustomFields != null)
                            {
                                foreach (var field in ShowCustomTemplateViewModel.CustomFields)
                                {
                                    OTCCustomFields model = new OTCCustomFields()
                                    {
                                        DataType = ((int)(field.DataType)).ToString(),
                                        DefaultValue = field.DataType.Equals(DataTypes.Bool) ? field.DefaultBooleanValue.ToString() : field.DefaultValue,
                                        ID = field.ID,
                                        InstrumentType = ((int)field.InstrumentType).ToString(),
                                        Name = field.Name
                                    };
                                    CFDDataFinal.CustomFields.Add(model);
                                }
                            }
                            templateDetails = await SecMasterOTCServiceApi.GetInstance().SaveCFDOTCTemplatesAsync(CFDDataFinal);

                        }
                        #endregion

                    }
                    else if (TemplateData.InstrumentType.Equals(InstrumentType.ConvertibleBond))
                    {
                        #region ConvertibleBond
                        if (TemplateData != null && ConvertibleBondViewModel.ConvertibleBondView != null && TemplateData.Name != "")
                        {
                            var _ConvertibleBondData = ConvertibleBondViewModel.ConvertibleBondView;
                            SecMasterConvertibleBondData OTCDataFinal = new SecMasterConvertibleBondData();
                            var equitySwapData = CFDViewModel.CFDData;
                            OTCDataFinal.Id = TemplateData.Id;
                            OTCDataFinal.OTCTemplateID = TemplateData.Id;
                            OTCDataFinal.Name = TemplateData.Name;
                            OTCDataFinal.Description = TemplateData.Description;
                            OTCDataFinal.InstrumentType = ((int)TemplateData.InstrumentType).ToString();
                            OTCDataFinal.UnderlyingAssetID = (int)TemplateData.UnderlyingAssetID;
                            OTCDataFinal.CreatedBy = TemplateData.CreatedBy == "System" ? 0 : CommonDataCache.CachedDataManager.GetInstance.GetUserID(TemplateData.CreatedBy);
                            OTCDataFinal.CreationDate = TemplateData.CreationDate;
                            OTCDataFinal.DaysToSettle = TemplateData.DaysToSettle;
                            OTCDataFinal.LastModifieDate = DateTime.Now;
                            OTCDataFinal.ISDACounterParty = templateData.ISDACounterParty.Key;
                            OTCDataFinal.LastModifiedBy = CommonDataCache.CachedDataManager.GetInstance.GetUserID(TemplateData.LastModifiedBy);
                            OTCDataFinal.EquityLeg_ConversionRatio = _ConvertibleBondData.EquityLeg_ConversionRatio;
                            OTCDataFinal.FinanceLeg_ZeroCoupon = _ConvertibleBondData.FinanceLeg_ZeroCoupon;
                            OTCDataFinal.FinanceLeg_IRBenchMark = (int)_ConvertibleBondData.FinanceLeg_IRBenchMark;
                            OTCDataFinal.FinanceLeg_FXRate = _ConvertibleBondData.FinanceLeg_FXRate;
                            OTCDataFinal.FinanceLeg_SBPoint = _ConvertibleBondData.FinanceLeg_SBPoint;
                            OTCDataFinal.FinanceLeg_DayCount = (int)_ConvertibleBondData.FinanceLeg_DayCount;
                            OTCDataFinal.FinanceLeg_CouponFreq = (int)_ConvertibleBondData.FinanceLeg_CouponFreq;
                            OTCDataFinal.Commission_Basis = (int)_ConvertibleBondData.Commission_Basis;
                            OTCDataFinal.Commission_HardCommRate = _ConvertibleBondData.Commission_HardCommRate;
                            OTCDataFinal.Commission_SoftCommRate = _ConvertibleBondData.Commission_SoftCommRate;
                            if (ShowCustomTemplateViewModel.CustomFields != null)
                            {
                                foreach (var field in ShowCustomTemplateViewModel.CustomFields)
                                {
                                    OTCCustomFields model = new OTCCustomFields()
                                    {
                                        DataType = ((int)(field.DataType)).ToString(),
                                        DefaultValue = field.DataType.Equals(DataTypes.Bool) ? field.DefaultBooleanValue.ToString() : field.DefaultValue,
                                        ID = field.ID,
                                        InstrumentType = ((int)field.InstrumentType).ToString(),
                                        Name = field.Name
                                    };
                                    OTCDataFinal.CustomFields.Add(model);
                                }
                            }
                            templateDetails = await SecMasterOTCServiceApi.GetInstance().SaveOTCDataTemplatesAsync(OTCDataFinal);

                        }
                        #endregion

                    }

                    if (templateDetails == 0)
                    {
                        ShowNotificationMessage("Data Saved", 1);

                    }
                    else if (templateDetails == 1)
                    {
                        ShowNotificationMessage("Failed!", 1);

                    }
                    else if (templateDetails == 2)
                    {
                        ShowNotificationMessage("Template name must not be null or empty!", 1);
                    }
                }
                else
                {
                    ShowNotificationMessage("Already Exist Template!", 1);

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
        /// Show Notification
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        private void ShowNotificationMessage(string message, int messageType)
        {
            try
            {
                TimeTickerIndicator();
                IsVisibleLblNotificationBar = Visibility.Visible;
                switch (messageType)
                {
                    case 1:
                        NotificationBarColor = Color.Green;
                        NotificationBarContent = message;
                        break;

                    case 2:
                        break;
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

        }

        private void HideNotificationMessage()
        {
            IsVisibleLblNotificationBar = Visibility.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        private void TimeTickerIndicator()
        {
            var timeTick = new Timer();
            timeTick.Interval = 10000; // it will Tick in 10 seconds
            timeTick.Tick += (s, e) =>
            {
                IsVisibleLblNotificationBar = Visibility.Hidden;
                timeTick.Stop();
            };
            timeTick.Start();
        }


        /// <summary>
        /// Called when [close button].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnCloseButton(object parameter)
        {
            try
            {
                System.ComponentModel.CancelEventArgs e = parameter as System.ComponentModel.CancelEventArgs;

                if (OnFormCloseButtonEvent != null)
                    OnFormCloseButtonEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Called when [form closed].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnFormClosed(object parameter)
        {
            try
            {
                Dispose();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    _cfdViewModel = null;
                    _equitySwapViewModel = null;
                    _showCustomTemplateViewModel = null;
                    SaveOTCTemplateButtonClickedCommand = null;
                    NextOTCTemplateButtonClickedCommand = null;
                    BackOTCTemplateButtonClickedCommand = null;
                    InstrumentSelectionChangedCommand = null;
                    FormCloseButton = null;
                    FormClosed = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void SetUp()
        {
            try
            {
                SetDefaultTemplatesValue();
            }
            catch (Exception)
            {
                throw;
            }
        }


        internal void SetUp(int templateID, InstrumentType instrumentType)
        {
            try
            {
                this.TemplateID = templateID;
                this.SelectedInstrumentType = instrumentType;
                IsEnableInstrumentType = false;
                GetTemplateDetailsAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
