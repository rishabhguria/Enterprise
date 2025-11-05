using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class SecMasterCFDData : SecMasterOTCData
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
        private double collateral_Margin;
        public double Collateral_Margin
        {
            get { return collateral_Margin; }
            set { collateral_Margin = value; }
        }
        /// <summary>
        /// Collateral Rate
        /// </summary>
        private double collateral_Rate;
        public double Collateral_Rate
        {
            get { return collateral_Rate; }
            set { collateral_Rate = value; }
        }

        /// <summary>
        /// Collateral DayCount
        /// </summary>
        private int collateral_DayCount;
        public int Collateral_DayCount
        {
            get { return collateral_DayCount; }
            set { collateral_DayCount = value; }
        }

        /// <summary>
        /// Fianace InteresrRatebenchmark
        /// </summary>
        private int finance_InteresrRatebenchmark;
        public int Finance_InteresrRatebenchmark
        {
            get { return finance_InteresrRatebenchmark; }
            set { finance_InteresrRatebenchmark = value; }
        }



        /// <summary>
        /// Fianace InteresrRatebenchmark
        /// </summary>
        private double finance_Fixedrate;
        public double Finance_Fixedrate
        {
            get { return finance_Fixedrate; }
            set { finance_Fixedrate = value; }
        }


        /// <summary>
        /// Fianace Fixedrate
        /// </summary>
        private double finance_SpreadBP;
        public double Finance_SpreadBP
        {
            get { return finance_SpreadBP; }
            set { finance_SpreadBP = value; }
        }


        /// <summary>
        /// fianace DayCount
        /// </summary>
        private int finance_DayCount;
        public int Finance_DayCount
        {
            get { return finance_DayCount; }
            set { finance_DayCount = value; }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private double finance_ScriptlendingFee;
        public double Finance_ScriptlendingFee
        {
            get { return finance_ScriptlendingFee; }
            set { finance_ScriptlendingFee = value; }
        }


        /// <summary>
        /// CFD Commissionbasis
        /// </summary>
        private int cfd_Commissionbasis;
        public int CFD_Commissionbasis
        {
            get { return cfd_Commissionbasis; }
            set { cfd_Commissionbasis = value; }
        }


        /// <summary>
        /// CFD HardCommRate
        /// </summary>
        private double cfd_HardCommRate;
        public double CFD_HardCommRate
        {
            get { return cfd_HardCommRate; }
            set { cfd_HardCommRate = value; }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private double cfd_SoftCommRate;
        public double CFD_SoftCommRate
        {
            get { return cfd_SoftCommRate; }
            set { cfd_SoftCommRate = value; }
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
    }
}
