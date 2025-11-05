using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.PostTrade
{
    class GenericBucket
    {


        private Dictionary<string, BuySellBucket> _buySellDictionary = new Dictionary<string, BuySellBucket>();


        public void CreateGenericBucket(List<TaxLot> openingTaxLots, List<TaxLot> closingTaxLots, bool isMatchStrategy)
        {
            try
            {
                //this method is called to reorganize closing and opening taxlots
                //reorganize is needed because BuyToClose is opening side and SellShort/SellToOpen is closing side
                //ReorganizeOpeningAndClosingTaxlots(ref openingTaxLots, ref closingTaxLots);

                foreach (TaxLot openingTrade in openingTaxLots)
                {
                    string key = openingTrade.Symbol.ToString().ToLower() + openingTrade.Level1ID.ToString();
                    //Narendra Kumar Jangir, 2013 May 28
                    //Does not match strategy when closing is populated from new Close order ui
                    if (isMatchStrategy)
                    {
                        key += openingTrade.Level2ID.ToString();
                    }

                    if (openingTrade.ISSwap)
                    {
                        key += openingTrade.ISSwap.ToString();
                    }

                    if (_buySellDictionary.ContainsKey(key))
                    {
                        _buySellDictionary[key].Buytaxlotsandpositions.Add(openingTrade);
                    }
                    else
                    {
                        BuySellBucket buySellbucket = new BuySellBucket();
                        buySellbucket.Buytaxlotsandpositions.Add(openingTrade);
                        _buySellDictionary.Add(key, buySellbucket);
                    }
                }

                foreach (TaxLot closingTrade in closingTaxLots)
                {
                    string key = closingTrade.Symbol.ToString().ToLower() + closingTrade.Level1ID.ToString();
                    //Narendra Kumar Jangir, 2013 May 28
                    //Does not match strategy when closing is populated from new Close order ui
                    if (isMatchStrategy)
                    {
                        key += closingTrade.Level2ID.ToString();
                    }
                    if (closingTrade.ISSwap)
                    {
                        key += closingTrade.ISSwap.ToString();
                    }
                    if (_buySellDictionary.ContainsKey(key))
                    {
                        _buySellDictionary[key].Selltaxlotsandpositions.Add(closingTrade);
                    }
                }


                Dictionary<string, BuySellBucket> dictBuySellCopy = new Dictionary<string, BuySellBucket>();

                foreach (KeyValuePair<string, BuySellBucket> pair in _buySellDictionary)//// Copying references to the copy as we need to remove those values from the BuySelldictionary for which closing transactions dont exist. 
                {
                    if (!dictBuySellCopy.ContainsKey(pair.Key))
                    {
                        dictBuySellCopy.Add(pair.Key, pair.Value);
                    }

                }

                foreach (KeyValuePair<string, BuySellBucket> pair in dictBuySellCopy)
                {
                    if (pair.Value.Buytaxlotsandpositions.Count == 0 || pair.Value.Selltaxlotsandpositions.Count == 0)
                    {
                        _buySellDictionary.Remove(pair.Key);
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

        public List<BuySellBucket> GetOpenTaxlotsforClosing()
        {
            List<BuySellBucket> bucketList = new List<BuySellBucket>();
            try
            {
                bucketList.AddRange(_buySellDictionary.Values);
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
            return bucketList;
        }
    }
}
