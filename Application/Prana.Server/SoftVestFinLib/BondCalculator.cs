using Prana.LogManager;
using System;
using System.Collections;

namespace SoftVest.FinLib
{
    public class BondCalculator
    {
        // constructors
        //
        public BondCalculator() { }
        public BondCalculator(BondDescription bondDesc, BondCalculatorOptions options)
        {
            Init(bondDesc, options);
        }

        public BondCalculator(BondDescription bondDesc)
        {
            try
            {
                _bondDesc = new BondDescription(bondDesc);
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

        public BondCalculator(BondCalculatorOptions options)
        {
            try
            {
                _bondCalculatorOptions = new BondCalculatorOptions(options);
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


        // initialization routines to set values after construction of BondCalculator object
        //
        public void Init(double coupon, BondDescription.Frequency freq, DateTime dateMaturity, DateTime dateFirst,
            DateTime dateIssue, BondDescription.Accrual accrualBasis)
        {
            try
            {
                _bondDesc = new BondDescription(coupon, freq, dateMaturity, dateFirst, dateIssue, accrualBasis);
                _bCalcDone = false;
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

        public void Init(BondDescription bondDesc, BondCalculatorOptions options)
        {
            try
            {
                _bondDesc = new BondDescription(bondDesc);
                _bondCalculatorOptions = new BondCalculatorOptions(options);
                _bCalcDone = false;
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

        public void Init(BondDescription bondDesc)
        {
            try
            {
                _bondDesc = new BondDescription(bondDesc);
                _bCalcDone = false;
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
        public void Init(BondCalculatorOptions options)
        {
            try
            {
                _bondCalculatorOptions = new BondCalculatorOptions(options);
                _bCalcDone = false;
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

        // functions to get accrued interest, percentage or dollar value
        //
        public BondCalculatorResult GetAI()
        {
            try
            {
                bool bOK = CalcAI();
                if (bOK)
                {
                    _result.ResultVal = _dAI;
                    _result.ErrorMessage = "";
                }
                return new BondCalculatorResult(_result);
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

                return null;
            }
        }

        public BondCalculatorResult GetAIValue()
        {
            try
            {
                bool bOK = CalcAI();
                if (bOK)
                {
                    string sErr = "";
                    // calculate the $amount of ai, need to have price and par values
                    if (!CheckValidBondData(true, out sErr))
                    {
                        _result.IsError = true;
                        _result.ErrorMessage = sErr;
                        _result.ResultVal = Constants.NA_Value;
                    }
                    else
                    {
                        double holdingValue = _bondDesc.HoldingValue;
                        double price = _bondDesc.Price;
                        double par = _bondDesc.Par;
                        if (holdingValue != Constants.NA_Value && price != Constants.NA_Value)
                            _result.ResultVal = (_dAI) * holdingValue / (_bondDesc.Price);
                        else if (par != Constants.NA_Value)
                            _result.ResultVal = (_dAI) * par;
                        _result.ErrorMessage = "";
                    }
                }
                return new BondCalculatorResult(_result);
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

                return null;
            }
        }

        // static function that returns AI and AI amount for multiple bond description objects
        public static ArrayList CalculateAI(ArrayList arBondDesc, BondCalculatorOptions bondCalcOptions)
        {
            try
            {
                BondCalculator bondCalc = new BondCalculator();
                ArrayList arBondCalcAIResults = new ArrayList();

                // loop over each bond desc, calc ai and ai amount for each one
                for (int i = 0; i < arBondDesc.Count; i++)
                {
                    BondDescription bondDesc = (BondDescription)arBondDesc[i];
                    bondCalc.Init(bondDesc, bondCalcOptions);
                    bondCalc.CalcAI();
                    BondCalculatorAIResult bondCalcAIResult = new BondCalculatorAIResult();
                    bondCalcAIResult.AIResult = bondCalc.GetAI();
                    bondCalcAIResult.AIValueResult = bondCalc.GetAIValue();
                    arBondCalcAIResults.Add(bondCalcAIResult);
                }
                return arBondCalcAIResults;
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

                return null;
            }
        }

        // following routines for calculating accrued interest 
        // based on Fabozzi "Fixed Income Securities", p. 347,348 
        private bool CalcAI()
        {
            try
            {
                if (!_bCalcDone)
                {
                    // fill in any fields in the bond desc
                    FillInDefaultFields();

                    string sErr = "";
                    if (!CheckValidBondData(false, out sErr))
                    {
                        _result.IsError = true;
                        _result.ErrorMessage = sErr;
                        _result.ResultVal = Constants.NA_Value;
                    }
                    else
                    {
                        _dAI = _bondDesc.Coupon * (double)DaysCount() / (double)AYCount();
                        _result.IsError = false;
                    }
                    _bCalcDone = true;

                }
                return !_result.IsError;
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

                return false;
            }
        }


        private int DaysCount()
        {
            try
            {
                int nDays;

                DateTime dateCalcCoupon = _bondCalculatorOptions.DateSettlement;
                DateTime dateCalc = dateCalcCoupon;
                if (_bondCalculatorOptions.AI_UseEndOfDay == BondCalculatorOptions.UseEndOfDay.UseEndOfDay_Yes)
                    dateCalcCoupon = dateCalcCoupon.AddDays(1);

                switch (_bondDesc.AccrualBasis)
                {
                    case BondDescription.Accrual.Accrual_Actual_365:
                    case BondDescription.Accrual.Accrual_Actual_360:
                    case BondDescription.Accrual.Accrual_Actual_Actual:
                        {
                            // actual
                            TimeSpan ts = dateCalc - LastCouponPayDate(dateCalcCoupon);
                            nDays = (int)ts.TotalDays;
                            break;
                        }
                    case BondDescription.Accrual.Accrual_30_360:
                        {
                            DateTime date1 = LastCouponPayDate(dateCalcCoupon);
                            DateTime date2 = dateCalc;
                            int nDay1 = date1.Day;
                            int nDay2 = date2.Day;
                            if (nDay1 == 31) nDay1 = 30;
                            if (nDay2 == 31 && nDay1 == 30) nDay2 = 30;
                            nDays = (date2.Year - date1.Year) * 360 +
                                    (date2.Month - date1.Month) * 30 +
                                    (nDay2 - nDay1);
                            break;
                        }
                    case BondDescription.Accrual.Accrual_30E_360:
                        {
                            DateTime date1 = LastCouponPayDate(dateCalcCoupon);
                            DateTime date2 = dateCalc;
                            int nDay1 = date1.Day;
                            int nDay2 = date2.Day;
                            if (nDay1 == 31) nDay1 = 30;
                            if (nDay2 == 31) nDay2 = 30;
                            nDays = (date2.Year - date1.Year) * 360 +
                                    (date2.Month - date1.Month) * 30 +
                                    (nDay2 - nDay1);
                            break;
                        }
                    default:
                        {
                            TimeSpan ts = dateCalc - LastCouponPayDate(dateCalcCoupon);
                            nDays = (int)ts.TotalDays;
                            break;
                        }
                }
                return nDays;
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

                return Int32.MinValue;
            }
        }

        private int AYCount()
        {
            try
            {
                int ay;
                DateTime dateCalcCoupon = _bondCalculatorOptions.DateSettlement;
                DateTime dateCalc = dateCalcCoupon;
                if (_bondCalculatorOptions.AI_UseEndOfDay == BondCalculatorOptions.UseEndOfDay.UseEndOfDay_Yes)
                    dateCalcCoupon = dateCalcCoupon.AddDays(1);
                int nFreq = _bondDesc.FrequencyPerYear();

                switch (_bondDesc.AccrualBasis)
                {
                    case BondDescription.Accrual.Accrual_Actual_Actual:
                        {
                            // actual
                            DateTime date1 = LastCouponPayDate(dateCalcCoupon);
                            DateTime date2 = NextCouponPayDate(dateCalcCoupon);

                            AdjustForShortCouponPeriod(ref date1, ref date2);

                            // get days diff, must be between 181 and 184
                            // if not need to subtract from date2
                            TimeSpan ts = date2 - date1;
                            int nDays = (int)ts.TotalDays;
                            ay = (nDays) * nFreq;
                            break;
                        }
                    case BondDescription.Accrual.Accrual_30_360:
                    case BondDescription.Accrual.Accrual_Actual_360:
                    case BondDescription.Accrual.Accrual_30E_360:
                        {
                            ay = 360;
                            break;
                        }
                    case BondDescription.Accrual.Accrual_Actual_365:
                        {
                            ay = 365;
                            break;
                        }
                    default:
                        {
                            ay = 360;
                            break;
                        }
                }
                return ay;
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

                return Int32.MinValue;
            }
        }

        private bool CheckValidBondData(bool bNeedPriceOrPar, out string sErr)
        {
            try
            {
                bool bRet = true;
                sErr = "";
                if (_bondDesc.MaturityDate == Constants.NullDate)
                {
                    bRet = false;
                    sErr = "Maturity date is null.  AI cannot be calculated.";
                }
                else if (_bondDesc.MaturityDate < _bondCalculatorOptions.DateSettlement)
                {
                    bRet = false;
                    sErr = "Maturity date is before settlement date.  AI cannot be calculated.";
                }
                else if (_bondDesc.AccrualBasis == BondDescription.Accrual.Accrual_UseDefault && !_bondDesc.IsZero)
                {
                    bRet = false;
                    sErr = "Accrual basis is not defined and no default value can be determined.  AI cannot be calculated.";
                }
                else if (_bondDesc.Freq == BondDescription.Frequency.Frequency_UseDefault && !_bondDesc.IsZero)
                {
                    bRet = false;
                    sErr = "Frequency is not defined and no default value can be determined.  AI cannot be calculated.";
                }
                else if (bNeedPriceOrPar &&
                    ((_bondDesc.Par == Constants.NA_Value || _bondDesc.Par == 0) &&
                    (_bondDesc.Price == Constants.NA_Value || _bondDesc.Price == 0
                        || _bondDesc.HoldingValue == Constants.NA_Value || _bondDesc.HoldingValue == 0)))
                {
                    bRet = false;
                    sErr = "Missing price, par, or holding value.  AI value cannot be calculated without either par or price and holding value.";
                }
                return bRet;
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

                sErr = string.Empty;
                return false;
            }
        }


        public DateTime LastCouponPayDate(DateTime dateCalculator)
        {
            try
            {
                int nFreq = _bondDesc.FrequencyPerYear();
                if (nFreq == 0)
                    return _bondDesc.IssueDate;
                else
                {
                    // http://jira.nirvanasolutions.com:8080/browse/CS-47 Change if the first coupon date havn't arived, then obviously the last Coupon Date should be Issue Date.
                    if (_bondDesc.FirstCouponDate >= dateCalculator && _bondDesc.IssueDate != Constants.NullDate)
                    {
                        return _bondDesc.IssueDate;
                    }
                    else
                    {
                        DateTime date = _bondDesc.FirstCouponDate;
                        //Date dateNull;
                        if (date == Constants.NullDate)
                        {
                            date = _bondDesc.MaturityDate;
                            date = DateUtils.SetYear(date, dateCalculator.Year - 1);
                        }
                        int nDay = date.Day;        // save the day, used later in  AdjustEndOfMonth
                        if (date.Year < dateCalculator.Year - 1)
                            date = DateUtils.SetYear(date, dateCalculator.Year - 1);
                        DateTime dateTemp = date;
                        for (; dateTemp < dateCalculator;)
                        {
                            date = dateTemp;
                            dateTemp = dateTemp.AddMonths(12 / nFreq);
                            dateTemp = AdjustEndOfMonth(dateTemp, nDay);
                        }
                        if (date >= dateCalculator)
                            date = date.AddMonths(-12 / nFreq);
                        date = AdjustEndOfMonth(date, nDay);
                        return date;
                    }
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

                return new DateTime();
            }
        }

        public DateTime NextCouponPayDate(DateTime dateCalculator)
        {
            try
            {
                int nFreq = _bondDesc.FrequencyPerYear();

                if (nFreq == 0)
                    return _bondDesc.MaturityDate;
                else
                {
                    if (_bondDesc.FirstCouponDate > dateCalculator && _bondDesc.FirstCouponDate != Constants.NullDate)
                    {
                        return _bondDesc.FirstCouponDate;
                    }
                    else
                    {
                        DateTime date = _bondDesc.FirstCouponDate;
                        if (date == Constants.NullDate)
                        {
                            date = _bondDesc.MaturityDate;
                            date = DateUtils.SetYear(date, dateCalculator.Year - 1);
                        }
                        int nDay = date.Day;        // save the day, used later in  AdjustEndOfMonth
                        if (date.Year < dateCalculator.Year - 1)
                            date = DateUtils.SetYear(date, dateCalculator.Year - 1);
                        for (; date < dateCalculator;)
                        {
                            date = date.AddMonths(12 / nFreq);
                            date = AdjustEndOfMonth(date, nDay);
                        }
                        date = AdjustEndOfMonth(date, nDay);
                        return date;
                    }
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

                return new DateTime();
            }
        }

        private void AdjustForShortCouponPeriod(ref DateTime date1, ref DateTime date2)
        {
            try
            {
                // get days diff, must be between 181-184 for freq=semiannual, 89-92 for freq=qtr, 28-31 for freq=month, 365-366 for freq=annual

                TimeSpan ts = date2 - date1;
                int nDays = (int)ts.TotalDays;

                if (_bondDesc.Freq == BondDescription.Frequency.Frequency_Monthly && (nDays < 28 || nDays > 31))
                {
                    date1 = date2.AddMonths(-1);
                }
                else if (_bondDesc.Freq == BondDescription.Frequency.Frequency_Quarterly && (nDays < 89 || nDays > 92))
                {
                    date1 = date2.AddMonths(-3);
                }
                else if (_bondDesc.Freq == BondDescription.Frequency.Frequency_SemiAnnually && (nDays < 181 || nDays > 184))
                {
                    date1 = date2.AddMonths(-6);
                }
                else if (_bondDesc.Freq == BondDescription.Frequency.Frequency_Annually && (nDays < 365 || nDays > 366))
                {
                    date1 = date2.AddYears(-1);
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

        private DateTime AdjustEndOfMonth(DateTime date, int nDay)
        {
            try
            {
                // if nDay is higher than the day in the date, try to bump that up
                if (date.Day < nDay)
                {
                    int nAdd = nDay - date.Day;
                    DateTime dt = date;
                    for (int i = 0; i < nAdd; i++)
                    {
                        dt = dt.AddDays(1);
                        if (date.Month == dt.Month)
                            date = dt;
                        else
                            break;
                    }
                }
                return date;
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

                return new DateTime();
            }
        }

        private void FillInDefaultFields()
        {
            try
            {
                if (!_bondCalculatorOptions.DontFillInBondDescriptionDefaults)
                {
                    bool bUseEndOfDay = false;
                    _bondDesc.FillInDefaultFields();
                    if (_bondCalculatorOptions.AI_UseEndOfDay == BondCalculatorOptions.UseEndOfDay.UseEndOfDay_UseDefault)
                    {
                        if (bUseEndOfDay)
                            _bondCalculatorOptions.AI_UseEndOfDay = BondCalculatorOptions.UseEndOfDay.UseEndOfDay_Yes;
                        else
                            _bondCalculatorOptions.AI_UseEndOfDay = BondCalculatorOptions.UseEndOfDay.UseEndOfDay_No;
                    }
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

        private BondDescription _bondDesc = new BondDescription();
        private BondCalculatorOptions _bondCalculatorOptions = new BondCalculatorOptions();
        private BondCalculatorResult _result = new BondCalculatorResult();

        private bool _bCalcDone = false;
        private double _dAI = Constants.NA_Value;

    }
}
