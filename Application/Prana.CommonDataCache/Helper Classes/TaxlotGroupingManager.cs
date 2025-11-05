using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.CommonDataCache
{
    public class TaxlotGroupingManager
    {

        private Dictionary<string, List<TaxLot>> CreateSymbolWiseDictionary(List<TaxLot> positionList)
        {
            Dictionary<string, List<TaxLot>> dictSymbolWisePositions = new Dictionary<string, List<TaxLot>>();
            try
            {
                foreach (TaxLot pos in positionList)
                {
                    if (!dictSymbolWisePositions.ContainsKey(pos.Symbol))
                    {
                        List<TaxLot> listPos = new List<TaxLot>();
                        listPos.Add(pos);
                        dictSymbolWisePositions.Add(pos.Symbol, listPos);
                    }
                    else
                    {
                        dictSymbolWisePositions[pos.Symbol].Add(pos);
                    }
                }
            }
            #region Catch
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
            return dictSymbolWisePositions;
            #endregion
        }
        public List<TaxLot> CreateGroupedData(List<string> listGrouping, List<TaxLot> taxlotstogroup)
        {
            List<TaxLot> groupedTaxlots = new List<TaxLot>();
            try
            {
                if (listGrouping == null)
                {
                    return taxlotstogroup;
                }
                //listGrouping.Add("Side");
                //listGrouping.Add("Symbol");
                Dictionary<string, TaxLot> dictFinalGroupPos = new Dictionary<string, TaxLot>();
                //create dictionary based on symbols 
                Dictionary<string, List<TaxLot>> dictSymbolWisePositions = CreateSymbolWiseDictionary(taxlotstogroup);

                foreach (KeyValuePair<string, List<TaxLot>> symbolListPos in dictSymbolWisePositions)
                {
                    List<TaxLot> listPos = symbolListPos.Value;

                    //if (listPos.Count == 1)
                    //{   // Grouping effort is not needed
                    //    groupedTaxlots.Add((TaxLot)listPos[0].Clone());
                    //    continue;
                    //}

                    foreach (TaxLot pos in listPos)
                    {
                        string compareKey = GetCompareKey(pos, listGrouping);

                        if (!dictFinalGroupPos.ContainsKey(compareKey))
                        {
                            dictFinalGroupPos.Add(compareKey, (TaxLot)pos.Clone());
                        }
                        else
                        {
                            TaxLot targetPos = dictFinalGroupPos[compareKey];
                            GroupPositionDetails(targetPos, pos);
                        }
                    }
                }

                // for which grouping is tried
                foreach (KeyValuePair<string, TaxLot> groupPos in dictFinalGroupPos)
                {
                    groupedTaxlots.Add(groupPos.Value);
                }

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return groupedTaxlots;
        }

        public void GroupPositionDetails(TaxLot targetPos, TaxLot pos)
        {



            //Avg Price
            //Avg Price should be weighted for same position Types
            if (targetPos.SideMultiplier == pos.SideMultiplier)
            {
                if (targetPos.Quantity + pos.Quantity != 0)
                {
                    targetPos.AvgPrice = (targetPos.Quantity * targetPos.AvgPrice + pos.Quantity * pos.AvgPrice) / (targetPos.Quantity + pos.Quantity);
                }
            }
            //for opposite sides  
            else
            {
                if (targetPos.Quantity + pos.Quantity == 0)
                {
                    targetPos.AvgPrice = 0;
                }
                else if (targetPos.TaxLotQty < pos.TaxLotQty)
                {
                    targetPos.AvgPrice = pos.AvgPrice;
                }
            }

            // Quantity
            targetPos.Quantity += pos.Quantity;
            targetPos.TaxLotQty = Math.Abs(targetPos.Quantity);
            // Position Side
            if (targetPos.Quantity >= 0)//targetPos.TaxLotQty*
            {
                targetPos.PositionType = "Long";
                targetPos.SideMultiplier = 1;
            }
            else
            {
                targetPos.PositionType = "Short";
                targetPos.SideMultiplier = -1;
            }

            // Account Name
            if (targetPos.Level1Name != pos.Level1Name)
            {
                targetPos.Level1Name = "Multiple";
            }

            // Strategy
            if (targetPos.Level2ID != pos.Level2ID)
            {
                targetPos.Level2Name = "Multiple";
            }

            // AUELocalDate
            if (targetPos.AUECLocalDate != pos.AUECLocalDate)
            {
                targetPos.AUECLocalDate = DateTime.Parse("1/1/1800");
            }

            // CompanyUserName
            if (targetPos.CompanyUserName != pos.CompanyUserName)
            {
                targetPos.CompanyUserName = "Multiple";
            }

            // CounterPartyName
            if (targetPos.CounterPartyName != pos.CounterPartyName)
            {
                targetPos.CounterPartyName = "Multiple";
            }

            // ExpirationDate
            if (targetPos.ExpirationDate != pos.ExpirationDate)
            {
                targetPos.ExpirationDate = DateTime.Parse("1/1/1800");
            }
        }
        private string GetCompareKey(TaxLot pos, List<string> listGrouping)
        {

            StringBuilder compareKey = new StringBuilder();
            compareKey.Append(pos.Symbol);
            compareKey.Append(pos.OrderSideTagValue);

            if (listGrouping.Contains("Account"))
            {
                compareKey.Append(pos.Level1Name);
            }
            if (listGrouping.Contains("Strategy"))
            {
                compareKey.Append(pos.Level2Name);
            }
            return compareKey.ToString();
        }

        // group wise dictionary like Account ---- Taxlots

        public Dictionary<string, List<TaxLot>> CreateGroups(string groupingViewcreteria, List<TaxLot> positionList)
        {
            Dictionary<string, List<TaxLot>> _dictGroup = new Dictionary<string, List<TaxLot>>();
            foreach (TaxLot taxlot in positionList)
            {
                string groupingKey = GetGroupingKey(taxlot, groupingViewcreteria);
                List<TaxLot> listTaxlots;
                if (!_dictGroup.ContainsKey(groupingKey))
                {
                    listTaxlots = new List<TaxLot>();
                    listTaxlots.Add(taxlot);
                    _dictGroup.Add(groupingKey, listTaxlots);
                }
                else
                {
                    listTaxlots = _dictGroup[groupingKey];
                    listTaxlots.Add(taxlot);
                }

            }
            if (!_dictGroup.ContainsKey("Portfolio"))
            {
                _dictGroup.Add("Portfolio", positionList);
            }

            return _dictGroup;

        }
        private string GetGroupingKey(TaxLot pos, string groupingCreteria)
        {

            StringBuilder compareKey = new StringBuilder();
            if (groupingCreteria.Equals("Account", StringComparison.OrdinalIgnoreCase))
            {
                compareKey.Append(pos.Level1Name);
            }
            if (groupingCreteria.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                compareKey.Append(pos.Symbol);
            }
            if (groupingCreteria.Equals("SecurityTypeName", StringComparison.OrdinalIgnoreCase))
            {
                compareKey.Append(pos.SecurityTypeName);
            }
            if (groupingCreteria.Equals("SectorName", StringComparison.OrdinalIgnoreCase))
            {
                compareKey.Append(pos.SectorName);
            }

            if (groupingCreteria.Equals("SubSectorName", StringComparison.OrdinalIgnoreCase))
            {
                compareKey.Append(pos.SubSectorName);
            }
            if (groupingCreteria.Equals("CountryName", StringComparison.OrdinalIgnoreCase))
            {
                compareKey.Append(pos.CountryName);
            }
            return compareKey.ToString();
        }
    }
}
