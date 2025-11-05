using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class SecMasterConvertibleBondData : SecMasterOTCData
    {
        private int oTCTemplateID;
        public int OTCTemplateID
        {
            get { return oTCTemplateID; }
            set { oTCTemplateID = value; }
        }

        /// <summary>
        /// Collateral Margin
        /// </summary>
        private double _equityLeg_ConversionRatio;
        public double EquityLeg_ConversionRatio
        {
            get { return _equityLeg_ConversionRatio; }
            set { _equityLeg_ConversionRatio = value; }
        }
        /// <summary>
        /// Collateral Rate
        /// </summary>
        private bool _financeLeg_ZeroCoupon;
        public bool FinanceLeg_ZeroCoupon
        {
            get { return _financeLeg_ZeroCoupon; }
            set { _financeLeg_ZeroCoupon = value; }
        }

        /// <summary>
        /// Collateral DayCount
        /// </summary>
        private int _financeLeg_IRBenchMark;
        public int FinanceLeg_IRBenchMark
        {
            get { return _financeLeg_IRBenchMark; }
            set { _financeLeg_IRBenchMark = value; }
        }



        /// <summary>
        /// Fianace InteresrRatebenchmark
        /// </summary>
        private double _financeLeg_FXRate;
        public double FinanceLeg_FXRate
        {
            get { return _financeLeg_FXRate; }
            set { _financeLeg_FXRate = value; }
        }


        /// <summary>
        /// Fianace Fixedrate
        /// </summary>
        private double _financeLeg_SBPoint;
        public double FinanceLeg_SBPoint
        {
            get { return _financeLeg_SBPoint; }
            set { _financeLeg_SBPoint = value; }
        }


        /// <summary>
        /// fianace DayCount
        /// </summary>
        private int financeLeg_DayCount;
        public int FinanceLeg_DayCount
        {
            get { return financeLeg_DayCount; }
            set { financeLeg_DayCount = value; }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private int _financeLeg_CouponFreq;
        public int FinanceLeg_CouponFreq
        {
            get { return _financeLeg_CouponFreq; }
            set { _financeLeg_CouponFreq = value; }
        }


        /// <summary>
        ///  Commissionbasis
        /// </summary>
        private int _commissionbasis;
        public int Commission_Basis
        {
            get { return _commissionbasis; }
            set { _commissionbasis = value; }
        }


        /// <summary>
        ///  HardCommRate
        /// </summary>
        private double commission_HardCommRate;
        public double Commission_HardCommRate
        {
            get { return commission_HardCommRate; }
            set { commission_HardCommRate = value; }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private double _commission_SoftCommRate;
        public double Commission_SoftCommRate
        {
            get { return _commission_SoftCommRate; }
            set { _commission_SoftCommRate = value; }
        }


        /// <summary>
        /// CustomFields
        /// </summary>
        private List<OTCCustomFields> customFields = new List<OTCCustomFields>();
        public List<OTCCustomFields> CustomFields
        {
            get { return customFields; }
            set { customFields = value; }
        }


        /// <summary>
        /// EquityLeg Price
        /// </summary>
        private double _equityLeg_ConversionPrice;
        public double EquityLeg_ConversionPrice
        {
            get { return _equityLeg_ConversionPrice; }
            set
            {
                _equityLeg_ConversionPrice = value;
            }

        }


        /// <summary>
        /// Collateral Margin
        /// </summary>
        private DateTime _equityLeg_ConversionDate;
        public DateTime EquityLeg_ConversionDate
        {
            get { return _equityLeg_ConversionDate; }
            set
            {
                _equityLeg_ConversionDate = value;

            }
        }


        private DateTime financeLeg_FirstPaymentDate = DateTimeConstants.MinValue;
        public DateTime FinanceLeg_FirstPaymentDate
        {
            get { return financeLeg_FirstPaymentDate; }
            set
            {
                financeLeg_FirstPaymentDate = value;
            }
        }



        private DateTime financeLeg_FirstResetDate = DateTimeConstants.MinValue;
        public DateTime FinanceLeg_FirstResetDate
        {
            get { return financeLeg_FirstResetDate; }
            set
            {
                financeLeg_FirstResetDate = value;

            }
        }


        private string financeLeg_ParValue = string.Empty;
        public string FinanceLeg_ParValue
        {
            get { return financeLeg_ParValue; }
            set
            {
                financeLeg_ParValue = value;
            }
        }

    }
}
