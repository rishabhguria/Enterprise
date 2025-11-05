using Prana.BusinessObjects;
using System;

namespace Prana.Interfaces
{
    public interface IFixedIncomeAdapter
    {
        //for calculating the value of the interestAccured
        //double GetAIValue(AccrualBasis accrualBasis, CouponFrequency couponFrequency, SecurityType bondType, double coupon, double avgPrice, double quantity, double notionalValue, DateTime interestCalculationDate, DateTime dateMaturity, DateTime dateIssue, DateTime dateFirst, string countryCode, bool isZero);
        double CalculateAccruedInterest(PranaBasicMessage Taxlot);

        //for calculating the percentage Interest Accrued
        //double GetAI(AccrualBasis accrualBasis, CouponFrequency couponFrequency, double coupon, DateTime dateMaturity, DateTime dateIssue, DateTime dateFirst);

        DateTime GetNextCouponPayDate(PranaBasicMessage Taxlot, DateTime date);

        DateTime GetLastCouponPayDate(PranaBasicMessage Taxlot, DateTime date);

    }
}
