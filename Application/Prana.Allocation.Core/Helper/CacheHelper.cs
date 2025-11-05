// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : Disha Sharma
// Created          : 08-11-2016
// ***********************************************************************
// <copyright file="CacheHelper.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Core.DataAccess;
using Prana.Allocation.Core.Extensions;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prana.Allocation.Core.Helper
{
    internal class CacheHelper
    {
        /// <summary>
        /// Gets the security details.
        /// </summary>
        /// <param name="uniqueSymbol">The unique symbol.</param>
        /// <returns></returns>
        internal static Dictionary<string, SecMasterBaseObj> GetSecurityDetails(List<AllocationGroup> groups)
        {
            Dictionary<string, SecMasterBaseObj> secmasterCollection = new Dictionary<string, SecMasterBaseObj>();
            try
            {
                object lockerSecMasterCollection = new object();
                HashSet<string> unidentifiedSymbols = new HashSet<string>();

                //Getting unique list of allocation groups. where symbol is not blank
                List<string> uniqueSymbol = (from c in groups
                                             where !string.IsNullOrWhiteSpace(c.Symbol)
                                             select c.Symbol).Distinct().ToList();

                //getting list of group id where symbol is blank
                List<string> blankSymbolGroups = (from c in groups
                                                  where string.IsNullOrWhiteSpace(c.Symbol)
                                                  select c.GroupID).Distinct().ToList();

                //logging groupid for which symbol if blank
                if (blankSymbolGroups.Count > 0)
                    InformationLogging("Symbols are blank for these group Ids: ", blankSymbolGroups);

                Parallel.ForEach(uniqueSymbol, symbol =>
                {
                    SecMasterBaseObj obj = ServiceProxyConnector.SecmasterProxy.GetSecMasterDataForSymbol(symbol);
                    if (obj != null)
                    {
                        lock (lockerSecMasterCollection)
                        {
                            secmasterCollection.Add(symbol, obj);
                        }
                    }
                    else
                    {
                        unidentifiedSymbols.Add(symbol);
                    }
                });

                //Log unidentified symbols for which security do not exist
                if (unidentifiedSymbols.Count > 0)
                    InformationLogging("Security does not exists for symbols: ", unidentifiedSymbols.ToList());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return secmasterCollection;
        }

        /// <summary>
        /// Updates the closing status.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static void UpdateClosingStatus(AllocationGroup group)
        {
            try
            {
                /* Updating group closing state in case of group already present in cache
                * TODO: Not best way of doing this, ideally should be listening to published data from closing service and updating this cache
                */
                foreach (TaxLot taxlot in group.TaxLots)
                {
                    taxlot.ClosingStatus = ClosingStatus.Open;
                    ServiceProxyConnector.ClosingProxy.SetTaxlotClosingStatus(taxlot);
                }
                group.UpdateGroupClosingStatus();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Log Information
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="list">The list.</param>
        internal static void InformationLogging(string message, List<string> list)
        {
            try
            {
                if (list != null)
                {
                    message += String.Join(",", list.Select(x => x.ToString()).ToArray());
                }

                InformationReporter.GetInstance.Write(message);
                Logger.LoggerWrite(message);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
