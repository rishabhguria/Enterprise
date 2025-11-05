using System;

namespace SoftVest.FinLib
{
    public class BondCalculatorOptions
    {
        // UseEndOfDay, if set as UseDefault, FillInDefaultFields() can figure out values
        public enum UseEndOfDay
        {
            UseEndOfDay_UseDefault = Constants.UseDefaultValue,
            UseEndOfDay_Yes,
            UseEndOfDay_No
        }

        // constructors
        //
        public BondCalculatorOptions()
        {
        }

        public BondCalculatorOptions(BondCalculatorOptions options)
        {
            _dateSettlement = options._dateSettlement;
            _AI_UseEndOfDay = options._AI_UseEndOfDay;
            _bDontFillInBondDescriptionDefaults = options._bDontFillInBondDescriptionDefaults;
        }

        // DateSettlement - settlement date on which to value the AI
        public DateTime DateSettlement
        {
            get { return _dateSettlement; }
            set { _dateSettlement = value; }
        }

        // AI_UseEndOfDay - AI can either be calculated using the beginning of the day, or using the end of the day
        // For example, if the accrued interest for a fixed income security calculated for 12/31 has a coupon paid on 12/31, 
        // then if AI_UseEndOfDay is ’yes’, the value of the accrued interest will be based on a single day of accrual, 
        // and if AI_UseEndOfDay = ‘no’, then the value of the accrued interest will be the value of a full coupon.
        //
        public UseEndOfDay AI_UseEndOfDay
        {
            get { return _AI_UseEndOfDay; }
            set { _AI_UseEndOfDay = value; }
        }

        // DontFillInBondDescriptionDefaults - Set this flag to true if BondCalculator should not try to fill in default
        // values for BondDescription fields not set or set to UseDefault
        public bool DontFillInBondDescriptionDefaults
        {
            get { return _bDontFillInBondDescriptionDefaults; }
            set { _bDontFillInBondDescriptionDefaults = value; }
        }

        private DateTime _dateSettlement = DateTime.Today;
        private UseEndOfDay _AI_UseEndOfDay = UseEndOfDay.UseEndOfDay_No;
        private bool _bDontFillInBondDescriptionDefaults = false;
    }
}
