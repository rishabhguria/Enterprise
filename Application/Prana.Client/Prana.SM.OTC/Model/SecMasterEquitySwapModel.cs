using Prana.BusinessObjects;
using System;

namespace Prana.SM.OTC
{
    public class SecMasterEquitySwapModel : SecMasterOTCDataModel
    {
        /// <summary>
        /// OTCTemplateID
        /// </summary>
        private int oTCTemplateID;
        public int OTCTemplateID
        {
            get { return oTCTemplateID; }
            set { oTCTemplateID = value; OnPropertyChanged("OTCTemplateID"); }
        }

        /// <summary>
        /// EquityLeg Frequency
        /// </summary>
        private PeriodFrequency _selectedEquityLeg_Frequency;
        public PeriodFrequency SelectedEquityLeg_Frequency
        {
            get { return _selectedEquityLeg_Frequency; }
            set { _selectedEquityLeg_Frequency = value; OnPropertyChanged("SelectedEquityLeg_Frequency"); }
        }

        /// <summary>
        /// EquityLeg BulletSwap
        /// </summary>
        private bool equityLeg_BulletSwap;
        public bool EquityLeg_BulletSwap
        {
            get { return equityLeg_BulletSwap; }
            set { equityLeg_BulletSwap = value; OnPropertyChanged("EquityLeg_BulletSwap"); }
        }

        /// <summary>
        /// EquityLeg ExcludeDividends
        /// </summary>
        private bool equityLeg_ExcludeDividends;
        public bool EquityLeg_ExcludeDividends
        {
            get { return equityLeg_ExcludeDividends; }
            set { equityLeg_ExcludeDividends = value; OnPropertyChanged("EquityLeg_ExcludeDividends"); }
        }

        /// <summary>
        /// EquityLeg ImpliedCommission
        /// </summary>
        private bool equityLeg_ImpliedCommission;
        public bool EquityLeg_ImpliedCommission
        {
            get { return equityLeg_ImpliedCommission; }
            set { equityLeg_ImpliedCommission = value; OnPropertyChanged("EquityLeg_ImpliedCommission"); }
        }

        /// <summary>
        /// Commission Basis
        /// </summary>
        private CommisionType commissionBasis;
        public CommisionType CommissionBasis
        {
            get { return commissionBasis; }
            set { commissionBasis = value; OnPropertyChanged("CommissionBasis"); }
        }

        /// <summary>
        /// Hard Commission Rate
        /// </summary>
        private double hardCommissionRate;
        public double HardCommissionRate
        {
            get { return hardCommissionRate; }
            set { hardCommissionRate = value; OnPropertyChanged("HardCommissionRate"); }
        }

        /// <summary>
        /// Soft Commission Rate
        /// </summary>
        private double softCommissionRate;
        public double SoftCommissionRate
        {
            get { return softCommissionRate; }
            set { softCommissionRate = value; OnPropertyChanged("SoftCommissionRate"); }
        }

        /// <summary>
        /// FinanceLeg InterestRate
        /// </summary>
        private RateType financeLeg_InterestRate;
        public RateType FinanceLeg_InterestRate
        {
            get { return financeLeg_InterestRate; }
            set { financeLeg_InterestRate = value; OnPropertyChanged("FinanceLeg_InterestRate"); }
        }

        /// <summary>
        /// FinanceLeg SpreadBasisPoint
        /// </summary>
        private double financeLeg_SpreadBasisPoint;
        public double FinanceLeg_SpreadBasisPoint
        {
            get { return financeLeg_SpreadBasisPoint; }
            set { financeLeg_SpreadBasisPoint = value; OnPropertyChanged("FinanceLeg_SpreadBasisPoint"); }
        }

        /// <summary>
        /// FinanceLeg FixedRate
        /// </summary>
        private double financeLeg_FixedRate;
        public double FinanceLeg_FixedRate
        {
            get { return financeLeg_FixedRate; }
            set { financeLeg_FixedRate = value; OnPropertyChanged("FinanceLeg_FixedRate"); }
        }

        /// <summary>
        /// FinanceLeg Frequency
        /// </summary>
        private PeriodFrequency financeLeg_Frequency;
        public PeriodFrequency FinanceLeg_Frequency
        {
            get { return financeLeg_Frequency; }
            set { financeLeg_Frequency = value; OnPropertyChanged("FinanceLeg_Frequency"); }
        }

        /// <summary>
        /// FinanceLeg DayCount
        /// </summary>
        private DayCount financeLeg_DayCount;
        public DayCount FinanceLeg_DayCount
        {
            get { return financeLeg_DayCount; }
            set { financeLeg_DayCount = value; OnPropertyChanged("FinanceLeg_DayCount"); }
        }

        private DateTime equityLeg_FirstPaymentDate = DateTimeConstants.MinValue;
        public DateTime EquityLeg_FirstPaymentDate
        {
            get { return equityLeg_FirstPaymentDate; }
            set
            {
                equityLeg_FirstPaymentDate = value;
                OnPropertyChanged("EquityLeg_FirstPaymentDate");
            }
        }

        private DateTime equityLeg_ExpirationDate = DateTimeConstants.MinValue;

        public DateTime EquityLeg_ExpirationDate
        {
            get { return equityLeg_ExpirationDate; }
            set
            {
                equityLeg_ExpirationDate = value;
                OnPropertyChanged("EquityLeg_ExpirationDate");
            }
        }

        private DateTime financeLeg_FirstResetDate = DateTimeConstants.MinValue;

        public DateTime FinanceLeg_FirstResetDate
        {
            get { return financeLeg_FirstResetDate; }
            set
            {
                financeLeg_FirstResetDate = value;
                OnPropertyChanged("FinanceLeg_FirstResetDate");
            }
        }

        private DateTime financeLeg_FirstPaymentDate = DateTimeConstants.MinValue;

        public DateTime FinanceLeg_FirstPaymentDate
        {
            get { return financeLeg_FirstPaymentDate; }
            set
            {
                financeLeg_FirstPaymentDate = value;
                OnPropertyChanged("FinanceLeg_FirstPaymentDate");
            }
        }


    }
}
