using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Diagnostics;
using System.Linq;
using Nirvana.Middleware.Linq;

namespace Nirvana.Middleware
{
    /// <summary>
    /// Nirvana Middleware API
    /// </summary>
    /// <remarks></remarks>
    public class Functions
    {

        /// <summary>
        /// Count of  Up/Down days.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double CountUpDownDays(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity, string direction)
        {

            using (var db = new NirvanaDataContext())
            {
                var Entities = new List<Entity>();
                Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

                if (String.Compare(direction, "up", true) == 0)
                {

                    if (String.Compare(groupField.ToLower(), "aggregate", true) == 0)
                    {
                        return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                                where t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate
                                group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                                where g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend) > 0
                                select new { value = 1 }).Count();
                    }
                    else
                    {
                        return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                                where t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate && (Entities).
                                Contains(new Entity()
                                {
                                    Value =
                                    (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                                    String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                                    String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                                    String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                                    String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : (System.String)null)
                                })
                                group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                                where g.Sum(p => (double)p.TotalUnrealizedPNLMTM + (double)p.TotalRealizedPNLMTM + (double)p.Dividend) > 0
                                select new { value = 1 }).Count();
                    }
                }
                else
                {
                    if (String.Compare(groupField.ToLower(), "aggregate", true) == 0)
                    {
                        return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                                where t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate
                                group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                                where g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend) < 0
                                select new { value = 1 }).Count();
                    }
                    else
                    {
                        return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                                where t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate && (Entities).
                                Contains(new Entity()
                                {
                                    Value =
                                    (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                                    String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                                    String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                                    String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                                    String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : (System.String)null)
                                })
                                group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                                where g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend) < 0
                                select new { value = 1 }).Count();
                    }
                }

            }
        }

        /// <summary>
        /// Count of long/short symbol.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int CountLongShortSymbol(NirvanaDataContext dbo, DateTime toDate, string groupField, string entity, string direction)
        {
            using (var db = new NirvanaDataContext())
            {
                var Entities = new List<Entity>();
                Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

                //return db.t_mw_genericpnls.Where(w => w.Side == direction).Select(s => s.Symbol).Distinct().Count();

                return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                        where t_mw_genericpnls.Rundate == toDate && String.Compare(t_mw_genericpnls.Side, direction, false) == 0 && (Entities).
                        Contains(new Entity()
                        {
                            Value =
                            (String.Compare(groupField, "Fund", false) == 0 ? t_mw_genericpnls.Fund :
                             String.Compare(groupField, "Asset", false) == 0 ? t_mw_genericpnls.Asset :
                             String.Compare(groupField, "UDASector", false) == 0 ? t_mw_genericpnls.UDASector :
                             String.Compare(groupField, "UDASubSector", true) == 0 ? t_mw_genericpnls.UDASubSector :
                             String.Compare(groupField, "Symbol", false) == 0 ? t_mw_genericpnls.Symbol :
                             String.Compare(groupField, "UnderlyingSymbol", false) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                        })
                        group t_mw_genericpnls by new { t_mw_genericpnls.Symbol } into g

                        select new { value = 1 }).Count();
            }
        }

        /// <summary>
        /// Gets the daily PNL average.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetLongShortDailyPnlAverage(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity, string direction)
        {
            using (var db = new NirvanaDataContext())
            {
                var Entities = new List<Entity>();
                Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

                return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                        where t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate && (Entities).
                        Contains(new Entity()
                        {
                            Value =
                            (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                            String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                            String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                            String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                            String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                        })

                        && String.Compare(t_mw_genericpnls.Side,
                            (String.Compare(direction, "Long", true) == 0 ? "Long" :
                            String.Compare(direction, "Short", true) == 0 ? "Short" : t_mw_genericpnls.Side), true) == 0

                        group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                        where g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend) != 0
                        select new { daypnl = (System.Double?)g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend) }).Average(a => a.daypnl).GetValueOrDefault(0.0F);
            }
        }

        /// <summary>
        /// Gets the beta exposure.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetLongShortBetaExposure(NirvanaDataContext dbo, DateTime toDate, string groupField, string entity, string direction)
        {
            if (string.IsNullOrEmpty(direction))
            {
                return GetLongShortBetaExposure(dbo, toDate, groupField, entity, "Long") +
                    Math.Abs((double)GetLongShortBetaExposure(dbo, toDate, groupField, entity, "Short"));
            }

            using (var db = new NirvanaDataContext())
            {
                var Entities = new List<Entity>();
                Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

                if (String.Compare(direction, "Long", true) == 0)
                {
                    return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                            where t_mw_genericpnls.Rundate == toDate && t_mw_genericpnls.BetaExposureBase > 0 &&
                            String.Compare(t_mw_genericpnls.Open_CloseTag, "o", true) == 0 &&
                            (Entities).Contains(new Entity()
                            {
                                Value =
                                (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                                 String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                                 String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                                 String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                                 String.Compare(groupField, "UDACountry", true) == 0 ? t_mw_genericpnls.UDACountry :
                                 String.Compare(groupField, "UDAAssetClass", true) == 0 ? t_mw_genericpnls.UDAAssetClass :
                                 String.Compare(groupField, "UDASubSector", true) == 0 ? t_mw_genericpnls.UDASubSector :
                                 String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                            })
                            group t_mw_genericpnls by new { t_mw_genericpnls.Symbol } into g

                            select new { value = (System.Double?)g.Sum(s => s.BetaExposureBase) }).Sum(s => s.value).GetValueOrDefault(0.0F);


                }
                else if (String.Compare(direction, "Short", true) == 0)
                {
                    return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                            where t_mw_genericpnls.Rundate == toDate && t_mw_genericpnls.BetaExposureBase < 0 && String.Compare(t_mw_genericpnls.Open_CloseTag, "o", true) == 0 && (Entities).
                            Contains(new Entity()
                            {
                                Value =
                                (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                                 String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                                 String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                                 String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                                 String.Compare(groupField, "UDACountry", true) == 0 ? t_mw_genericpnls.UDACountry :
                                 String.Compare(groupField, "UDAAssetClass", true) == 0 ? t_mw_genericpnls.UDAAssetClass :
                                 String.Compare(groupField, "UDASubSector", true) == 0 ? t_mw_genericpnls.UDASubSector :
                                 String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                            })
                            group t_mw_genericpnls by new { t_mw_genericpnls.Symbol } into g

                            select new { value = (System.Double?)g.Sum(s => s.BetaExposureBase) }).Sum(a => a.value).GetValueOrDefault(0.0F);
                }
            }
            return 0.0F;
        }

        /// <summary>
        /// Gets the delta exposure.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetLongShortDeltaExposure(NirvanaDataContext dbo, DateTime toDate, string groupField, string entity, string direction)
        {
            if (string.IsNullOrEmpty(direction))
            {
                return GetLongShortDeltaExposure(dbo, toDate, groupField, entity, "Long") + Math.Abs((double)GetLongShortDeltaExposure(dbo, toDate, groupField, entity, "Short"));
            }
            using (var db = new NirvanaDataContext())
            {
                var Entities = new List<Entity>();
                Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

                if (String.Compare(direction, "Long", true) == 0)
                {
                    return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                            where t_mw_genericpnls.Rundate == toDate && t_mw_genericpnls.DeltaExposureBase > 0 && String.Compare(t_mw_genericpnls.Open_CloseTag, "o", true) == 0 && (Entities).
                            Contains(new Entity()
                            {
                                Value =
                                (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                                 String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                                 String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                                 String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                                 String.Compare(groupField, "UDACountry", true) == 0 ? t_mw_genericpnls.UDACountry :
                                 String.Compare(groupField, "UDAAssetClass", true) == 0 ? t_mw_genericpnls.UDAAssetClass :
                                 String.Compare(groupField, "UDASubSector", true) == 0 ? t_mw_genericpnls.UDASubSector :
                                 String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                            })
                            group t_mw_genericpnls by new { t_mw_genericpnls.Symbol } into g

                            select new { value = (System.Double?)g.Sum(s => s.DeltaExposureBase) }).Sum(s => s.value).GetValueOrDefault(0.0F);


                }
                else if (String.Compare(direction, "Short", true) == 0)
                {
                    return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                            where t_mw_genericpnls.Rundate == toDate && t_mw_genericpnls.DeltaExposureBase < 0 && String.Compare(t_mw_genericpnls.Open_CloseTag, "o", true) == 0 && (Entities).
                            Contains(new Entity()
                            {
                                Value =
                                (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                                 String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                                 String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                                 String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                                 String.Compare(groupField, "UDACountry", true) == 0 ? t_mw_genericpnls.UDACountry :
                                 String.Compare(groupField, "UDAAssetClass", true) == 0 ? t_mw_genericpnls.UDAAssetClass :
                                 String.Compare(groupField, "UDASubSector", true) == 0 ? t_mw_genericpnls.UDASubSector :
                                 String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                            })
                            group t_mw_genericpnls by new { t_mw_genericpnls.Symbol } into g

                            select new { value = (System.Double?)g.Sum(s => s.DeltaExposureBase) }).Sum(a => a.value).GetValueOrDefault(0.0F);
                }
            }
            return 0.0F;
        }

        /// <summary>
        /// Gets the ending market value.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetLongShortEndingMarketValue(NirvanaDataContext dbo, DateTime toDate, string groupField, string entity, string direction)
        {
            if (string.IsNullOrEmpty(direction))
            {
                return GetLongShortEndingMarketValue(dbo, toDate, groupField, entity, "Long") + Math.Abs((double)GetLongShortEndingMarketValue(dbo, toDate, groupField, entity, "Short"));
            }

            using (var db = new NirvanaDataContext())
            {
                //  return db.t_mw_genericpnls.Where(w => w.Rundate == toDate && w.Side == direction && w.Open_CloseTag == "o").Sum(s => s.EndingMarketValueBase).GetValueOrDefault(0.0F);

                var Entities = new List<Entity>();
                Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

                return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                        where t_mw_genericpnls.Rundate == toDate && String.Compare(t_mw_genericpnls.Side, direction, false) == 0 && t_mw_genericpnls.Open_CloseTag == "o" && (Entities).
                        Contains(new Entity()
                        {
                            Value =
                            (String.Compare(groupField, "Fund", false) == 0 ? t_mw_genericpnls.Fund :
                             String.Compare(groupField, "Asset", false) == 0 ? t_mw_genericpnls.Asset :
                             String.Compare(groupField, "UDASector", false) == 0 ? t_mw_genericpnls.UDASector :
                             String.Compare(groupField, "Symbol", false) == 0 ? t_mw_genericpnls.Symbol :
                             String.Compare(groupField, "UDACountry", false) == 0 ? t_mw_genericpnls.UDACountry :
                             String.Compare(groupField, "UDAAssetClass", false) == 0 ? t_mw_genericpnls.UDAAssetClass :
                             String.Compare(groupField, "UDASubSector", false) == 0 ? t_mw_genericpnls.UDASubSector :
                             String.Compare(groupField, "UnderlyingSymbol", false) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                        })
                        group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g

                        select new { value = (System.Double?)g.Sum(s => s.EndingMarketValueBase) }).Sum(s => s.value).GetValueOrDefault(0.0F);
            }
        }

        /// <summary>
        /// Gets the profit loss.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetProfitLoss(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));


            using (var db = new NirvanaDataContext())
            {
                //var ds = db.t_mw_genericpnls.Where(w => w.Rundate >= fromDate && w.Rundate <= toDate);

                if (string.Compare(groupField, "aggregate", true) == 0)
                {
                    return db.T_MW_GenericPNLs.
                        Where(w => w.Rundate >= fromDate && w.Rundate <= toDate).
                        Sum(s => s.TotalRealizedPNLMTM + s.TotalUnrealizedPNLMTM + s.Dividend).GetValueOrDefault(0.0F);
                }
                else if (string.Compare(groupField, "Fund", true) == 0)
                {
                    return db.T_MW_GenericPNLs.
                        Where(w => w.Rundate >= fromDate && w.Rundate <= toDate && Entities.Contains(new Entity() { Value = w.Fund })).
                        Sum(s => s.TotalRealizedPNLMTM + s.TotalUnrealizedPNLMTM + s.Dividend).GetValueOrDefault(0.0F);
                }
                else if (string.Compare(groupField, "Asset", true) == 0)
                {
                    return db.T_MW_GenericPNLs.
                     Where(w => w.Rundate >= fromDate && w.Rundate <= toDate && Entities.Contains(new Entity() { Value = w.Asset })).
                     Sum(s => s.TotalRealizedPNLMTM + s.TotalUnrealizedPNLMTM + s.Dividend).GetValueOrDefault(0.0F);
                }
                else if (string.Compare(groupField, "UDASector", true) == 0)
                {
                    return db.T_MW_GenericPNLs.
                        Where(w => w.Rundate >= fromDate && w.Rundate <= toDate && Entities.Contains(new Entity() { Value = w.UDASector })).
                        Sum(s => s.TotalRealizedPNLMTM + s.TotalUnrealizedPNLMTM + s.Dividend).GetValueOrDefault(0.0F);
                }
                else if (string.Compare(groupField, "Symbol", true) == 0)
                {
                    return db.T_MW_GenericPNLs.
                     Where(w => w.Rundate >= fromDate && w.Rundate <= toDate && Entities.Contains(new Entity() { Value = w.Symbol })).
                     Sum(s => s.TotalRealizedPNLMTM + s.TotalUnrealizedPNLMTM + s.Dividend).GetValueOrDefault(0.0F);
                }
                else if (string.Compare(groupField, "UnderlyingSymbol", true) == 0)
                {
                    return db.T_MW_GenericPNLs.
                     Where(w => w.Rundate >= fromDate && w.Rundate <= toDate && Entities.Contains(new Entity() { Value = w.UnderlyingSymbol })).
                     Sum(s => s.TotalRealizedPNLMTM + s.TotalUnrealizedPNLMTM + s.Dividend).GetValueOrDefault(0.0F);
                }
                return 0.0F;

            }
        }

        /// <summary>
        /// Gets the average capital base.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetAverageCapitalBase(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

            var span = new TimeSpan(toDate.Ticks - fromDate.Ticks);
            int daySpan = (int)span.TotalDays + 1;

            using (var db = new NirvanaDataContext())
            {
                var prevDate = db.AdjustBusinessDays(fromDate, -1, 1);
                if (string.Compare(groupField, "aggregate", true) == 0)
                {
                    var BMVSecurities = db.T_MW_GenericPNLs.
                        Where(w => w.Rundate == fromDate && SqlMethods.DateDiffDay(w.TradeDate, w.Rundate) > 0).
                        Sum(s => s.BeginningMarketValueBase).GetValueOrDefault(0.0F);

                    var BMVCashValue = db.PM_CompanyFundCashCurrencyValues.
                        Where(w => w.Date == prevDate).
                        Join(db.T_CompanyFunds, on => on.FundID, on => on.CompanyFundID, (c, b) => c).
                        Sum(s => s.CashValueBase).GetValueOrDefault(0.0F);


                    var BMVCashFlow = db.T_SubAccountCashValues.
                        Where(w => w.PayOutDate >= fromDate && w.PayOutDate <= toDate).
                        Join(db.T_SubAccounts, on => on.SubAccountID, on => on.SubAccountID, (c, b) => c).
                        Join(db.T_CompanyFunds, on => on.FundID, on => on.CompanyFundID, (c, b) => c).
                        Sum(s => ((SqlMethods.DateDiffDay(s.PayOutDate, toDate) + 1) / daySpan) * s.CashValue).GetValueOrDefault(0.0F);

                    return Math.Abs(BMVSecurities + BMVCashValue + BMVCashFlow);
                }
                else if (string.Compare(groupField, "fund", true) == 0)
                {
                    var BMVSecurities = db.T_MW_GenericPNLs.
                       Where(w => w.Rundate == fromDate && SqlMethods.DateDiffDay(w.TradeDate, w.Rundate) > 0 && Entities.Contains(new Entity() { Value = w.Fund })).
                       Sum(s => s.BeginningMarketValueBase).GetValueOrDefault(0.0F);

                    var BMVCashValue = db.PM_CompanyFundCashCurrencyValues.
                        Where(w => w.Date == prevDate && Entities.Contains(new Entity() { Value = w.T_CompanyFund.FundName })).
                        Join(db.T_CompanyFunds, on => on.FundID, on => on.CompanyFundID, (c, b) => c).
                        Sum(s => s.CashValueBase).GetValueOrDefault(0.0F);

                    var BMVCashFlow = db.T_SubAccountCashValues.
                        Where(w => w.PayOutDate >= fromDate && w.PayOutDate <= toDate && Entities.Contains(new Entity() { Value = w.T_CompanyFund.FundName })).
                        Join(db.T_SubAccounts, on => on.SubAccountID, on => on.SubAccountID, (c, b) => c).
                        Join(db.T_CompanyFunds, on => on.FundID, on => on.CompanyFundID, (c, b) => c).
                        Sum(s => ((SqlMethods.DateDiffDay(s.PayOutDate, toDate) + 1) / daySpan) * s.CashValue).GetValueOrDefault(0.0F);
                }
                else
                    throw new Exception("Aggregate and Fund are the only common denominators. A call to GetAverageCapitalBase is invalid");


            }
            return null;
        }

        /// <summary>
        /// Gets the contribution.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetContribution(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

            using (var db = new NirvanaDataContext())
            {
                var pnl = GetProfitLoss(db, fromDate, toDate, groupField, entity);
                var acb = GetAverageCapitalBase(db, fromDate, toDate, groupField, entity);

                if (pnl == 0 || acb == 0)
                    return null;

                return pnl / acb;
            }
        }

        /// <summary>
        /// Gets the cash value.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetCashValue(NirvanaDataContext dbo, DateTime toDate, string groupField, string entity)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

            using (var db = new NirvanaDataContext())
            {
                if (db.IsBusinessDay(toDate, 11) == false)
                    toDate = (DateTime)db.AdjustBusinessDays(toDate, -1, 11);

                if (string.Compare(groupField, "aggregate", true) == 0)
                {
                    return db.PM_CompanyFundCashCurrencyValues.
                        Where(w => w.Date == toDate).
                        Join(db.T_CompanyFunds, on => on.FundID, on => on.CompanyFundID, (c, b) => c).
                        Sum(s => s.CashValueBase).GetValueOrDefault(0.0F);
                }
                else if (string.Compare(groupField, "fund", true) == 0)
                {
                    return db.PM_CompanyFundCashCurrencyValues.
                       Where(w => w.Date == toDate && Entities.Contains(new Entity() { Value = w.T_CompanyFund.FundName })).
                       Join(db.T_CompanyFunds, on => on.FundID, on => on.CompanyFundID, (c, b) => c).
                       Sum(s => s.CashValueBase).GetValueOrDefault(0.0F);
                }


                //if (string.Compare(groupField, "aggregate", true) == 0)
                //{
                //        return (from t in
                //            (from t in
                //                 (
                //                     (from CashCurrencyValues in db.PM_CompanyFundCashCurrencyValues
                //                      join t_companyfunds in db.T_CompanyFunds on new { FundID = CashCurrencyValues.FundID } equals new { FundID = t_companyfunds.CompanyFundID }
                //                      where
                //                      CashCurrencyValues.Date == toDate
                //                      select new
                //                      {
                //                          t_companyfunds.FundName,
                //                          CashCurrencyValues.CashValueBase
                //                      }))
                //             where
                //             Entities.Contains(new Entities() { Entity = t.FundName })

                //             select new
                //             {
                //                 t.CashValueBase,
                //                 Dummy = "x"
                //             })
                //        group t by new { t.Dummy } into g
                //        select new
                //        {
                //            Value = (System.Double?)g.Sum(p => p.CashValueBase)
                //        }).Sum(s => s.Value).GetValueOrDefault(0.0F);
                //}
                //else if (string.Compare(groupField, "Fund", true) == 0)
                //{
                //    return (from pm_companyfundcashcurrencyvalues in
                //                (from CashCurrencyValues in db.PM_CompanyFundCashCurrencyValues
                //                 join t_companyfunds in db.T_CompanyFunds on new { FundID = CashCurrencyValues.FundID } equals new { FundID = t_companyfunds.CompanyFundID }
                //                 where
                //                   CashCurrencyValues.Date == toDate
                //                 select new
                //                 {
                //                     CashCurrencyValues.CashValueBase,
                //                     Dummy = "x"
                //                 })
                //            group pm_companyfundcashcurrencyvalues by new { pm_companyfundcashcurrencyvalues.Dummy } into g
                //            select new
                //            {
                //                Column1 = (System.Double?)g.Sum(p => p.CashValueBase)
                //            }).Sum(s=> s.Column1).GetValueOrDefault(0.0F);
                //}

            }
            return null;
        }

        /// <summary>
        /// Gets the historical value at risk.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="confidence">The confidence.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetHistoricalValueAtRisk(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity, int confidence = 95)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

            using (var db = new NirvanaDataContext())
            {
                var v = from t_mw_genericpnls in db.T_MW_GenericPNLs
                        where
                          t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate
                        group t_mw_genericpnls by new
                        {
                            t_mw_genericpnls.Rundate
                        } into g
                        where g.Sum(p => p.TotalRealizedPNLMTM + p.TotalUnrealizedPNLMTM + p.Dividend) != 0
                        orderby
                          (System.Double?)g.Sum(p => p.TotalRealizedPNLMTM + p.TotalUnrealizedPNLMTM + p.Dividend)
                        select new
                        {
                            pnl = (System.Double?)g.Sum(p => p.TotalRealizedPNLMTM + p.TotalUnrealizedPNLMTM + p.Dividend)
                        };
                return v.Min(m => m.pnl);
            }

        }

        /// <summary>
        /// Gets the largest day gain loss.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetLargestDayGainLoss(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity, string direction)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));


            using (var db = new NirvanaDataContext())
            {
                var v = (from t_mw_genericpnls in db.T_MW_GenericPNLs
                         where t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate && (Entities).
                         Contains(new Entity()
                         {
                             Value =
                             (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                             String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                             String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                             String.Compare(groupField, "UDASubSector", true) == 0 ? t_mw_genericpnls.UDASubSector :
                             String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                             String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                         })
                         group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                         select new { daypnl = (System.Double?)g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend) });

                if (String.Compare(direction, "Gain", true) == 0)
                {
                    return v.Where(w => w.daypnl > 0).Max(m => m.daypnl);
                }
                else if (String.Compare(direction, "Loss", true) == 0)
                {
                    return v.Where(w => w.daypnl < 0).Min(m => m.daypnl);
                }
                else return null;
            }
        }

        /// <summary>
        /// Gets the long short profit loss.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetLongShortProfitLoss(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity, string direction)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));


            using (var db = new NirvanaDataContext())
            {
                return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                        where t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate && String.Compare(t_mw_genericpnls.Side, direction, true) == 0 && (Entities).
                         Contains(new Entity()
                         {
                             Value =
                             (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                             String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                             String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                             String.Compare(groupField, "UDASubSector", true) == 0 ? t_mw_genericpnls.UDASubSector :
                             String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                             String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                         })
                        group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                        select new { daypnl = (System.Double?)g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend) }).Sum(s => s.daypnl).GetValueOrDefault(0.0F);
            }

        }

        /// <summary>
        /// Gets the linked mod dietz.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetLinkedModDietz(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity)
        {

            List<DateRange> Ranges = new List<DateRange>();
            bool IsMonthPlus = false;
            DateTime LtoDate, MtoDate, MfromDate;

            if (fromDate.LastDateInMonth() >= toDate)
            {
                LtoDate = toDate;
                IsMonthPlus = false;
            }
            else
            {
                LtoDate = fromDate.LastDateInMonth();
                IsMonthPlus = true;
            }
            Ranges.Add(new DateRange() { From = fromDate, To = LtoDate });
            if (IsMonthPlus)
            {
                MfromDate = LtoDate.AddMonths(1).FirstDateInMonth();
                MtoDate = LtoDate.AddMonths(1).LastDateInMonth();

                while (MfromDate <= toDate)
                {
                    if (MtoDate > toDate)
                    {
                        MtoDate = toDate;
                        Ranges.Add(new DateRange() { From = MfromDate, To = MtoDate });
                        break;
                    }
                    else
                    {
                        Ranges.Add(new DateRange() { From = MfromDate, To = MtoDate });
                        MfromDate = MtoDate.AddDays(1);
                        MtoDate = MfromDate.LastDateInMonth();
                    }
                }
            }

            return (from range in
                        (from range in Ranges
                         select new
                         {
                             From = range.From,
                             To = range.To,
                             Value = (Double)Math.Log((double)1 + (double)GetModDietzReturn(dbo, range.From, range.To, groupField, entity)),
                             Dummy = "x"
                         }
                       )
                    group range by new { range.Dummy } into g
                    select new { Value = (double?)Math.Exp(g.Sum(s => s.Value)) - 1 }).FirstOrDefault().Value.GetValueOrDefault(0.0F);

        }

        /// <summary>
        /// Gets the mod dietz return.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetModDietzReturn(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity)
        {

            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

            var span = new TimeSpan(toDate.Ticks - fromDate.Ticks);
            int daySpan = (int)span.TotalDays + 1;

            using (var db = new NirvanaDataContext())
            {
                var prevDate = db.AdjustBusinessDays(fromDate, -1, 1);
                if (string.Compare(groupField, "aggregate", true) == 0)
                {
                    var pnl = db.T_MW_GenericPNLs.
                        Where(w => w.Rundate >= fromDate && w.Rundate <= toDate).
                        Sum(s => s.TotalRealizedPNLMTM + s.TotalUnrealizedPNLMTM + s.Dividend).GetValueOrDefault(0.0F);

                    var BMVSecurities = db.T_MW_GenericPNLs.
                        Where(w => w.Rundate == fromDate && SqlMethods.DateDiffDay(w.TradeDate, w.Rundate) > 0).
                        Sum(s => s.BeginningMarketValueBase).GetValueOrDefault(0.0F);

                    var BMVCashValue = db.PM_CompanyFundCashCurrencyValues.
                        Where(w => w.Date == prevDate).
                        Join(db.T_CompanyFunds, on => on.FundID, on => on.CompanyFundID, (c, b) => c).
                        Sum(s => s.CashValueBase).GetValueOrDefault(0.0F);

                    var BMVCashFlow = db.T_SubAccountCashValues.
                        Where(w => w.PayOutDate >= fromDate && w.PayOutDate <= toDate).
                        Join(db.T_SubAccounts, on => on.SubAccountID, on => on.SubAccountID, (c, b) => c).
                        Join(db.T_CompanyFunds, on => on.FundID, on => on.CompanyFundID, (c, b) => c).
                        Sum(s => ((SqlMethods.DateDiffDay(s.PayOutDate, toDate) + 1) / daySpan) * s.CashValue).GetValueOrDefault(0.0F);

                    if (pnl == 0 || (BMVSecurities + BMVCashValue + BMVCashFlow) == 0) return 0.0F;

                    var result = pnl / Math.Abs(BMVSecurities + BMVCashValue + BMVCashFlow);

                    if (result <= -1)
                        result = -.999;

                    return result;

                }
                else
                    Debug.Assert(false);

            }
            return null;
        }

        /// <summary>
        /// Gets the net asset value.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetNetAssetValue(NirvanaDataContext dbo, DateTime toDate, string groupField, string entity)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

            using (var db = new NirvanaDataContext())
            {
                if (string.Compare(groupField, "aggregate", true) == 0)
                {
                    var EndingValue = db.T_MW_GenericPNLs.
                        Where(w => String.Compare(w.Open_CloseTag, "o", true) == 0 && w.Rundate == toDate).
                        Sum(s => s.EndingMarketValueBase);

                    var EndingCash = db.PM_CompanyFundCashCurrencyValues.
                        Where(w => w.Date == toDate).
                        Join(db.T_CompanyFunds, on => on.FundID, on => on.CompanyFundID, (c, b) => c).
                        Sum(s => s.CashValueBase).GetValueOrDefault(0.0F);

                    return EndingCash + EndingValue;
                }
                else
                    Debug.Assert(false);
            }
            return null;
        }

        /// <summary>
        /// Gets the net beta exposure.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetNetBetaExposure(NirvanaDataContext dbo, DateTime toDate, string groupField, string entity)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));


            using (var db = new NirvanaDataContext())
            {
                return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                        where t_mw_genericpnls.Rundate == toDate && String.Compare(t_mw_genericpnls.Open_CloseTag, "o", true) == 0 && (Entities).
                         Contains(new Entity()
                         {
                             Value =
                             (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                             String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                             String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                             String.Compare(groupField, "UDASubSector", true) == 0 ? t_mw_genericpnls.UDASubSector :
                             String.Compare(groupField, "UDACountry", true) == 0 ? t_mw_genericpnls.UDACountry :
                             String.Compare(groupField, "UDASecurityType", true) == 0 ? t_mw_genericpnls.UDASecurityType :
                             String.Compare(groupField, "UDAAssetClass", true) == 0 ? t_mw_genericpnls.UDAAssetClass :
                             String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                             String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                         })
                        group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                        select new { exposure = (System.Double?)g.Sum(s => s.BetaExposureBase) }).Sum(s => s.exposure).GetValueOrDefault(0.0F);
            }
        }

        /// <summary>
        /// Gets the net delta exposure.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetNetDeltaExposure(NirvanaDataContext dbo, DateTime toDate, string groupField, string entity)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));


            using (var db = new NirvanaDataContext())
            {
                return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                        where t_mw_genericpnls.Rundate == toDate && String.Compare(t_mw_genericpnls.Open_CloseTag, "o", true) == 0 && (Entities).
                         Contains(new Entity()
                         {
                             Value =
                             (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                             String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                             String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                             String.Compare(groupField, "UDASubSector", true) == 0 ? t_mw_genericpnls.UDASubSector :
                             String.Compare(groupField, "UDACountry", true) == 0 ? t_mw_genericpnls.UDACountry :
                             String.Compare(groupField, "UDASecurityType", true) == 0 ? t_mw_genericpnls.UDASecurityType :
                             String.Compare(groupField, "UDAAssetClass", true) == 0 ? t_mw_genericpnls.UDAAssetClass :
                             String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                             String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                         })
                        group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                        select new { exposure = (System.Double?)g.Sum(s => s.DeltaExposureBase) }).Sum(s => s.exposure).GetValueOrDefault(0.0F);
            }
        }

        /// <summary>
        /// Gets the ending market value.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetNetEndingMarketValue(NirvanaDataContext dbo, DateTime toDate, string groupField, string entity)
        {
            List<Entity> Entities = new List<Entity>();
            Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

            using (var db = new NirvanaDataContext())
            {
                return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                        where t_mw_genericpnls.Rundate == toDate && String.Compare(t_mw_genericpnls.Open_CloseTag, "o", true) == 0 && (Entities).
                         Contains(new Entity()
                         {
                             Value =
                             (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                             String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                             String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                             String.Compare(groupField, "UDASubSector", true) == 0 ? t_mw_genericpnls.UDASubSector :
                             String.Compare(groupField, "UDACountry", true) == 0 ? t_mw_genericpnls.UDACountry :
                             String.Compare(groupField, "UDASecurityType", true) == 0 ? t_mw_genericpnls.UDASecurityType :
                             String.Compare(groupField, "UDAAssetClass", true) == 0 ? t_mw_genericpnls.UDAAssetClass :
                             String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                             String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                         })
                        group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                        select new { value = (System.Double?)g.Sum(s => s.EndingMarketValueBase) }).Sum(s => s.value).GetValueOrDefault(0.0F);
            }
        }

        /// <summary>
        /// Gets the profit loss STD dev.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetProfitLossStdDev(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity, string direction)
        {
            using (var db = new NirvanaDataContext())
            {
                var Entities = new List<Entity>();
                Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

                return (from t_mw_genericpnls in db.T_MW_GenericPNLs
                        where t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate && (Entities).
                        Contains(new Entity()
                        {
                            Value =
                            (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                            String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                            String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                            String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                            String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                        })

                        && String.Compare(t_mw_genericpnls.Side,
                            (String.Compare(direction, "Long", true) == 0 ? "Long" :
                            String.Compare(direction, "Short", true) == 0 ? "Short" : t_mw_genericpnls.Side), true) == 0

                        group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g
                        where g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend) != 0
                        select (System.Double)g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend)).ToArray().StdDev();

            }
        }

        /// <summary>
        /// Gets the top N profit loss by symbol.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="TopN">The top N.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetTopNProfitLossBySymbol(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity, int TopN, string direction)
        {
            using (var db = new NirvanaDataContext())
            {
                var Entities = new List<Entity>();
                Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

                var v = (from t_mw_genericpnls in db.T_MW_GenericPNLs
                         where t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate && (Entities).
                         Contains(new Entity()
                         {
                             Value =
                             (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                             String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                             String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                             String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                             String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                         })

                         && String.Compare(t_mw_genericpnls.Side,
                             (String.Compare(direction, "Long", true) == 0 ? "Long" :
                             String.Compare(direction, "Short", true) == 0 ? "Short" : t_mw_genericpnls.Side), true) == 0

                         group t_mw_genericpnls by new { t_mw_genericpnls.Symbol } into g
                         where g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend) != 0
                         select new { daypnl = (System.Double?)g.Sum(p => p.TotalUnrealizedPNLMTM + p.TotalRealizedPNLMTM + p.Dividend) });

                return String.Compare(direction, "Winner", true) == 0 ?
                    v.OrderByDescending(o => o.daypnl).Where(w => w.daypnl >= 0).Take(TopN).Sum(s => s.daypnl).GetValueOrDefault(0.0F)
                    :
                    v.OrderBy(o => o.daypnl).Where(w => w.daypnl <= 0).Take(TopN).Sum(s => s.daypnl).GetValueOrDefault(0.0F);
            }
        }

        /// <summary>
        /// Gets the top N long short value by symbol.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="topN">The top N.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetTopNLongShortValueBySymbol(NirvanaDataContext dbo, DateTime toDate, string groupField, string entity, int topN, string direction)
        {
            using (var db = new NirvanaDataContext())
            {
                var Entities = new List<Entity>();
                Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

                var v = (from t_mw_genericpnls in db.T_MW_GenericPNLs
                         where t_mw_genericpnls.Rundate == toDate &&
                                String.Compare(t_mw_genericpnls.Side, direction, true) == 0 &&
                                String.Compare(t_mw_genericpnls.Open_CloseTag, "o", true) == 0 && (Entities).
                         Contains(new Entity()
                         {
                             Value =
                             (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
                             String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
                             String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
                             String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
                             String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : entity)
                         })

                         group t_mw_genericpnls by new { t_mw_genericpnls.Symbol } into g
                         select new { emv = (System.Double?)g.Sum(p => p.EndingMarketValueBase) });

                return String.Compare(direction, "Long", true) == 0 ?
                    v.OrderByDescending(o => o.emv).Where(w => w.emv >= 0).Take(topN).Sum(s => s.emv).GetValueOrDefault(0.0F)
                    :
                    v.OrderBy(o => o.emv).Where(w => w.emv <= 0).Take(topN).Sum(s => s.emv).GetValueOrDefault(0.0F);
            }
        }

        /// <summary>
        /// Gets the win loss ratio.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double? GetWinLossRatio(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField, string entity)
        {
            using (var db = new NirvanaDataContext())
            {
                var Entities = new List<Entity>();
                Array.ForEach(entity.Split(','), fund => Entities.Add(new Entity { Value = fund }));

                var up = db.T_MW_GenericPNLs.
                    Where(w => w.Rundate >= fromDate && w.Rundate <= toDate && w.TotalRealizedPNLMTM + w.TotalUnrealizedPNLMTM + w.Dividend > 0).Count();

                var dn = db.T_MW_GenericPNLs.
                    Where(w => w.Rundate >= fromDate && w.Rundate <= toDate && w.TotalRealizedPNLMTM + w.TotalUnrealizedPNLMTM + w.Dividend < 0).Count();

                //avoid divi by Zero
                if (dn == 0) return 0.0F;

                return (double)up / (double)dn;
            }
        }

        /// <summary>
        /// Updates the greeks.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="GenericPNL">The generic PNL.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="InterestFreeRate">The interest free rate.</param>
        /// <remarks></remarks>
        public static void UpdateGreeks(List<T_MW_GenericPNL> GenericPNL, DateTime fromDate, DateTime toDate, double InterestFreeRate, bool UseWinDaleToCalculateImpliedVol, bool UseUserPrefToCalculateImpliedVol)
        {
            System.Diagnostics.TextWriterTraceListener myWriter = new System.Diagnostics.TextWriterTraceListener(System.Console.Out);

            using (NirvanaDataContext db = new NirvanaDataContext())
            {
                myWriter.WriteLine("Greeks 1 " + DateTime.Now);

                var greeks = GenericPNL.Where(o => (o.Rundate >= fromDate && o.Rundate <= toDate) && (o.Open_CloseTag.ToLower() == "o" && o.Asset.ToLower() != "cash")).ToList();

                myWriter.WriteLine("Greeks 2 " + DateTime.Now);

                var dividend_PM_DailyDividendYield = db.PM_DailyDividendYields.ToList();

                var vol_PM_DailyVolatility = db.PM_DailyVolatility.ToList();

                var symbols = greeks.GroupBy(gp =>
                    new GreeksModel
                    {
                        Asset = gp.Asset,
                        Symbol = gp.Symbol,
                        SymbolPrice = gp.EndingPriceLocal,
                        Underlying = gp.UnderlyingSymbol,
                        UnderlyingPrice = gp.UnderlyingSymbolPrice,
                        PutOrCall = gp.PutOrCall,
                        ExercisePrice = gp.StrikePrice,
                        Time = Math.Max(.00001d, (double)SqlMethods.DateDiffDay(gp.Rundate, gp.ExpirationDate)) / 365.0d,
                        Interest = InterestFreeRate,
                        Dividend = dividend_PM_DailyDividendYield.Where(x => x.Symbol == gp.Symbol && x.Date == gp.Rundate).Count() > 0 ? dividend_PM_DailyDividendYield.Where(x => x.Symbol == gp.Symbol && x.Date == gp.Rundate).Single().DividendYield.Value : 0.0d,
                        ImpliedVolatility = vol_PM_DailyVolatility.Where(x => x.Symbol == gp.Symbol && x.Date == gp.Rundate).Count() > 0 ? vol_PM_DailyVolatility.Where(x => x.Symbol == gp.Symbol && x.Date == gp.Rundate).Single().Volatility.Value : 0.0d,
                    }, new GreeksModelEquality()).ToList();

                myWriter.WriteLine("Greeks 3 " + DateTime.Now);

                Hashtable assetModels = NirvanaDataContext.AssetModels;

                foreach (var symbol in symbols)
                {
                    var _Symbol = symbol.Key;

                    bool _Underlying = !string.IsNullOrWhiteSpace(_Symbol.Underlying);

                    bool _UnderlyingPrice = _Symbol.UnderlyingPrice.HasValue && _Symbol.UnderlyingPrice != 0.0d;

                    bool _PutOrCall = !string.IsNullOrWhiteSpace(_Symbol.PutOrCall) && (_Symbol.PutOrCall.Trim().ToUpper().StartsWith("P") || _Symbol.PutOrCall.Trim().ToUpper().StartsWith("C"));

                    if (_Underlying && _UnderlyingPrice)
                    {
                        GreeksModel _Proxy;

                        string model = (string)assetModels[_Symbol.Asset];
                        if (string.IsNullOrWhiteSpace(model))
                            model = string.Empty;
                        switch (model)
                        {
                            case "Black76":
                                _Proxy = new Black76Model(_Symbol);
                                break;
                            default:
                                _Proxy = new BlackScholesModel(_Symbol);
                                break;
                        }

                        if (_PutOrCall)
                        {
                            _Proxy.ImpliedVolatility = _Symbol.ImpliedVolatility;
                            _Proxy.Calculate(UseWinDaleToCalculateImpliedVol, UseUserPrefToCalculateImpliedVol);
                        }

                        var rows = symbol.ToList();

                        foreach (var row in rows)
                        {
                            if (_PutOrCall)
                            {
                                row.impliedVol = _Proxy.ImpliedVolatility;
                                row.Delta = _Proxy.Delta;
                                row.Theta = _Proxy.Theta;
                                row.Rho = _Proxy.Rho;
                                row.Vega = _Proxy.Vega;
                                row.Gamma = _Proxy.Gamma;
                            }

                            //Mia20140520: Removing Inline Underlying Delta Calculation
                            //Ankit20140513: Adding Leveraged factor to Exposure Calculation

                            row.DeltaExposureLocal = Convert.ToInt16(row.SideMultiplier) * row.LeveragedFactor * row.BeginningQuantity * row.UnderlyingSymbolPrice * row.Multiplier * row.Delta ;
                            row.DeltaExposureBase = Convert.ToInt16(row.SideMultiplier) * row.LeveragedFactor * row.BeginningQuantity * (row.EndingFXRate == 0.0d ? 1.0d : row.EndingFXRate) * row.UnderlyingSymbolPrice * row.Multiplier * row.Delta;
                            row.BetaExposureLocal = row.DeltaExposureLocal * row.Beta;
                            row.BetaExposureBase = row.DeltaExposureBase * row.Beta;
                        }
                    }
                }

                myWriter.WriteLine("Greeks 4 " + DateTime.Now);
            }

            myWriter.WriteLine("Greeks Function End " + DateTime.Now);
        }

        /// <summary>
        /// Updates the betas.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="GenericPNL">The generic PNL.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <remarks></remarks>
        public static void UpdateBetas(List<T_MW_GenericPNL> GenericPNL, DateTime fromDate, DateTime toDate)
        {
            using (NirvanaDataContext db = new NirvanaDataContext())
            {
                System.Diagnostics.TextWriterTraceListener myWriter = new System.Diagnostics.TextWriterTraceListener(System.Console.Out);

                myWriter.WriteLine("Betas 1 " + DateTime.Now);

                var betas = from gp in GenericPNL.Where(o => (o.Rundate >= fromDate && o.Rundate <= toDate) && ((o.Open_CloseTag.ToLower() == "o" && o.Asset.ToLower() != "cash") || o.Open_CloseTag.ToLower() == "d"))
                            select gp;

                myWriter.WriteLine("Betas 2 " + DateTime.Now);

                var underlyings = betas.Select(p => p.UnderlyingSymbol).Distinct().Where(p => !string.IsNullOrWhiteSpace(p));

                myWriter.WriteLine("Betas 3 " + DateTime.Now);

                var dailybetas = underlyings.GroupJoin(db.PM_DailyBetas.Where(b => b.Beta != 0), o => o, i => i.Symbol,
                    (o, i) => new { Symbol = o, Beta = i.OrderBy(g => g.Date).FirstOrDefault(g => g.Date >= toDate) ?? i.OrderByDescending(g => g.Date).FirstOrDefault(g => g.Date <= toDate) }).ToList();

                myWriter.WriteLine("Betas 4 " + DateTime.Now);

                foreach (var dailybeta in dailybetas.Where(p => p.Beta != null).ToList())
                {
                    foreach (var row in betas.Where(w => w.UnderlyingSymbol == dailybeta.Symbol).ToList())
                    {
                        row.Beta = dailybeta.Beta.Beta;
                    }
                }

                myWriter.WriteLine("Betas 5 " + DateTime.Now);

                myWriter.WriteLine("Betas Calculation End " + DateTime.Now);
            }
        }

        /// <summary>
        /// Gets the available entities.
        /// </summary>
        /// <param name="dbo">The dbo.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="groupField">The group field.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IQueryable<string> GetAvailableEntities(NirvanaDataContext dbo, DateTime fromDate, DateTime toDate, string groupField)
        {
            throw new NotImplementedException();

            //using (var db = new NirvanaDataContext() )
            //{
            //    var Entities = new List<Entities>();
            //    Array.ForEach(groupField.Split(','), fund => Entities.Add(new Entities { Entity = fund }));

            //    return (from t_mw_genericpnls in db.t_mw_genericpnls
            //            where t_mw_genericpnls.Rundate >= fromDate && t_mw_genericpnls.Rundate <= toDate && (Entities).
            //            Contains(new Entities()
            //            {
            //                Entity =
            //                (String.Compare(groupField, "Fund", true) == 0 ? t_mw_genericpnls.Fund :
            //                String.Compare(groupField, "Asset", true) == 0 ? t_mw_genericpnls.Asset :
            //                String.Compare(groupField, "UDASector", true) == 0 ? t_mw_genericpnls.UDASector :
            //                String.Compare(groupField, "Symbol", true) == 0 ? t_mw_genericpnls.Symbol :
            //                String.Compare(groupField, "UnderlyingSymbol", true) == 0 ? t_mw_genericpnls.UnderlyingSymbol : (System.String)null)
            //            })
            //            group t_mw_genericpnls by new { t_mw_genericpnls.Rundate } into g

            //            select new { value = g.Distinct() });
            //}
        }
    }

}
