using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Interfaces;
using Prana.LogManager;
using SoftVest.FinLib;
using System;

namespace SoftVestFinLib
{
    public class SoftVestFixedIncomeAdapter : IFixedIncomeAdapter
    {
        #region IFixedIncome Members

        public double CalculateAccruedInterest(PranaBasicMessage Taxlot)
        {
            double AccruedInterest = 0.0;
            try
            {


                BondCalculatorOptions bondOptions = new BondCalculatorOptions();
                // bondOptions.AI_UseEndOfDay = BondCalculatorOptions.UseEndOfDay.UseEndOfDay_Yes; ;
                bondOptions.DateSettlement = Taxlot.SettlementDate.Date;
                BondDescription bondDescription = CreateBondDescriptionObject(Taxlot);
                BondCalculator bondCalculator = new BondCalculator(bondDescription, bondOptions);
                BondCalculatorResult result = bondCalculator.GetAIValue();
                if (!result.IsError)
                {
                    AccruedInterest = result.ResultVal;
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

            return AccruedInterest;

        }

        private BondDescription CreateBondDescriptionObject(PranaBasicMessage allocationGroup)
        {
            try
            {
                double holdingValue = Constants.NA_Value;
                //variable not in used.
                //double notionalValue = double.MinValue;
                string countryCode = string.Empty;

                AccrualBasis accrualBasis = allocationGroup.AccrualBasis;
                CouponFrequency couponFrequency = allocationGroup.Freq;
                SecurityType bondType = allocationGroup.BondType;

                BondDescription.Accrual accrual = (BondDescription.Accrual)Enum.ToObject(typeof(BondDescription.Accrual), accrualBasis);
                BondDescription.Frequency frequency = (BondDescription.Frequency)Enum.ToObject(typeof(BondDescription.Frequency), couponFrequency);
                BondDescription.BondSecurityType SecurityType = (BondDescription.BondSecurityType)Enum.ToObject(typeof(BondDescription.BondSecurityType), bondType);

                double coupon = allocationGroup.CouponRate;
                double avgPrice = allocationGroup.AvgPrice;
                // multiplier was not being earlier considered calculating accrued interest as multiplier wasn't multiplied to quantity.
                //     http://jira.nirvanasolutions.com:8080/browse/CS-32
                double quantity = allocationGroup.CumQty * allocationGroup.ContractMultiplier;

                DateTime interestCalculationDate = allocationGroup.SettlementDate.Date;
                DateTime dateMaturity = allocationGroup.MaturityDate.Date;
                //if (dateMaturity == DateTimeConstants.MinValue)
                //{
                //    dateMaturity = Constants.NullDate;
                //}
                DateTime dateIssue = allocationGroup.IssueDate.Date;
                if (dateIssue.Date == DateTimeConstants.MinValue.Date)
                {
                    dateIssue = Constants.NullDate;
                }
                DateTime dateFirst = allocationGroup.FirstCouponDate.Date;
                if (dateFirst.Date == DateTimeConstants.MinValue.Date)
                {
                    dateFirst = Constants.NullDate;
                }

                bool isZero = allocationGroup.IsZero;
                BondDescription bondDescription = new BondDescription(avgPrice, quantity, holdingValue, coupon, isZero, frequency, dateMaturity, DateTime.MinValue, DateTime.MinValue, dateFirst, dateIssue, accrual, SecurityType, countryCode);

                return bondDescription;
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

        public DateTime GetNextCouponPayDate(PranaBasicMessage Taxlot, DateTime date)
        {
            try
            {
                BondCalculatorOptions bondOptions = new BondCalculatorOptions();
                //bondOptions.AI_UseEndOfDay = BondCalculatorOptions.UseEndOfDay.UseEndOfDay_Yes; ;
                bondOptions.DateSettlement = date;
                BondDescription bondDescription = CreateBondDescriptionObject(Taxlot);
                BondCalculator bondCalculator = new BondCalculator(bondDescription, bondOptions);

                return bondCalculator.NextCouponPayDate(date);
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

        public DateTime GetLastCouponPayDate(PranaBasicMessage Taxlot, DateTime date)
        {
            try
            {
                BondCalculatorOptions bondOptions = new BondCalculatorOptions();
                //bondOptions.AI_UseEndOfDay = BondCalculatorOptions.UseEndOfDay.UseEndOfDay_Yes; ;
                bondOptions.DateSettlement = date;
                BondDescription bondDescription = CreateBondDescriptionObject(Taxlot);
                BondCalculator bondCalculator = new BondCalculator(bondDescription, bondOptions);

                return bondCalculator.LastCouponPayDate(date);
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
        #endregion
    }
}
