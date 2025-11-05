using Prana.BusinessObjects;
using System.Collections.Generic;

namespace Prana.SM.OTC
{
    public enum InerestRatebenchmark
    {
        Fixed = 1
    }

    public class SecMasterCFDModel : SecMasterOTCDataModel
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
        /// Collateral Margin
        /// </summary>
        private double collateral_Margin;
        public double Collateral_Margin
        {
            get { return collateral_Margin; }
            set { collateral_Margin = value; OnPropertyChanged("Collateral_Margin"); }
        }
        /// <summary>
        /// Collateral Rate
        /// </summary>
        private double collateral_Rate;
        public double Collateral_Rate
        {
            get { return collateral_Rate; }
            set { collateral_Rate = value; OnPropertyChanged("Collateral_Rate"); }
        }

        /// <summary>
        /// Collateral DayCount
        /// </summary>
        private DayCount collateral_DayCount;
        public DayCount Collateral_DayCount
        {
            get { return collateral_DayCount; }
            set { collateral_DayCount = value; OnPropertyChanged("Collateral_DayCount"); }
        }

        /// <summary>
        /// CustomFields
        /// </summary>
        private List<OTCCustomFields> customFields;
        public List<OTCCustomFields> CustomFields
        {
            get { return customFields; }
            set { customFields = value; OnPropertyChanged("CustomFields"); }
        }


        /// <summary>
        /// Fianace InteresrRatebenchmark
        /// </summary>
        private RateType finance_InteresrRatebenchmark;
        public RateType Finance_InteresrRatebenchmark
        {
            get { return finance_InteresrRatebenchmark; }
            set { finance_InteresrRatebenchmark = value; OnPropertyChanged("Finance_InteresrRatebenchmark"); }
        }



        /// <summary>
        /// Fianace InteresrRatebenchmark
        /// </summary>
        private double finance_Fixedrate;
        public double Finance_Fixedrate
        {
            get { return finance_Fixedrate; }
            set { finance_Fixedrate = value; OnPropertyChanged("Finance_Fixedrate"); }
        }


        /// <summary>
        /// Fianace Fixedrate
        /// </summary>
        private double finance_SpreadBP;
        public double Finance_SpreadBP
        {
            get { return finance_SpreadBP; }
            set { finance_SpreadBP = value; OnPropertyChanged("Finance_SpreadBP"); }
        }


        /// <summary>
        /// fianace DayCount
        /// </summary>
        private DayCount finance_DayCount;
        public DayCount Finance_DayCount
        {
            get { return finance_DayCount; }
            set { finance_DayCount = value; OnPropertyChanged("Finance_DayCount"); }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private double finance_ScriptlendingFee;
        public double Finance_ScriptlendingFee
        {
            get { return finance_ScriptlendingFee; }
            set { finance_ScriptlendingFee = value; OnPropertyChanged("Finance_ScriptlendingFee"); }
        }


        /// <summary>
        /// CFD Commissionbasis
        /// </summary>
        private CommisionType cfd_Commissionbasis;
        public CommisionType CFD_Commissionbasis
        {
            get { return cfd_Commissionbasis; }
            set { cfd_Commissionbasis = value; OnPropertyChanged("CFD_Commissionbasis"); }
        }


        /// <summary>
        /// CFD HardCommRate
        /// </summary>
        private double cfd_HardCommRate;
        public double CFD_HardCommRate
        {
            get { return cfd_HardCommRate; }
            set { cfd_HardCommRate = value; OnPropertyChanged("CFD_HardCommRate"); }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private double cfd_SoftCommRate;
        public double CFD_SoftCommRate
        {
            get { return cfd_SoftCommRate; }
            set { cfd_SoftCommRate = value; OnPropertyChanged("CFD_SoftCommRate"); }
        }
    }
}
