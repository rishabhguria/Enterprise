using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Act40OrderGeneratorTool.Classes
{
    internal class Calculator
    {
        internal static List<Trade> Rebalance(List<Position> model, List<Position> destination, Double modelNav, Double originalNav, Double targetNav, Double longFactor, Double shortFactor, Double longLimit, Double shortLimit)
        {
            try
            {
                Double longValue = model.Where(x => x.PositionSide == Side.Long).Sum(x => x.DollarDelta);
                Double shortValue = model.Where(x => x.PositionSide == Side.Short).Sum(x => x.DollarDelta);
                Double longContribution = longValue / modelNav;
                Double shortContribution = shortValue / modelNav;

                Double extraLong = 0;
                Double extraShort = 0;

                shortLimit = -shortLimit;

                List<Trade> output = new List<Trade>();


                // handel the positions in the destination account
                foreach (Position pos in destination)
                {
                    Position mPos = model.FirstOrDefault(x => x.Symbol == pos.Symbol);
                    if (mPos == null)
                    {
                        // Close the position
                        Double oldContribution = pos.DollarDelta / originalNav * 100;
                        output.Add(Trade.Create(new Position(pos.Account, pos.Symbol, Side.Long, pos.Quantity, pos.Price, pos.Sector, pos.DollarDelta), null, oldContribution, 0));
                    }
                    else
                    {
                        Double oldContribution = pos.DollarDelta / originalNav * 100;
                        Double symbolContribution = mPos.DollarDelta / modelNav;
                        if (mPos.PositionSide == Side.Long)
                        {
                            Double newContribution = (symbolContribution / longContribution * longFactor);
                            if (newContribution > longLimit)
                            {
                                extraLong = extraLong + (newContribution - longLimit);
                                newContribution = longLimit;
                            }
                            Double newQuantity = newContribution * targetNav / pos.Price / 100;
                            output.Add(Trade.Create(pos, new Position(pos.Account, pos.Symbol, Side.Long, newQuantity, pos.Price, pos.Sector, pos.DollarDelta), oldContribution, newContribution));
                        }
                        else
                        {
                            Double newContribution = -(symbolContribution / shortContribution * shortFactor);
                            if (newContribution < shortLimit)
                            {
                                extraShort = extraShort + (newContribution - shortLimit);
                                newContribution = shortLimit;
                            }

                            Double newQuantity = newContribution * targetNav / pos.Price / 100;
                            output.Add(Trade.Create(pos, new Position(pos.Account, pos.Symbol, Side.Short, newQuantity, pos.Price, pos.Sector, pos.DollarDelta), oldContribution, newContribution));
                        }
                    }
                }

                // Add the positions that are in the model account
                foreach (Position pos in model)
                {
                    Position dPos = destination.FirstOrDefault(x => x.Symbol == pos.Symbol);
                    if (dPos == null)
                    {
                        Double symbolContribution = pos.DollarDelta / modelNav;
                        if (pos.PositionSide == Side.Long)
                        {
                            Double newContribution = (symbolContribution / longContribution * longFactor);
                            if (newContribution > longLimit)
                            {
                                extraLong = extraLong + (newContribution - longLimit);
                                newContribution = longLimit;
                            }
                            Double newQuantity = newContribution * targetNav / pos.Price / 100;
                            output.Add(Trade.Create(null, new Position("", pos.Symbol, Side.Long, newQuantity, pos.Price, pos.Sector, pos.DollarDelta), 0, newContribution));
                        }
                        else
                        {
                            Double newContribution = -(symbolContribution / shortContribution * shortFactor);
                            if (newContribution < shortLimit)
                            {
                                extraShort = extraShort + (newContribution - shortLimit);
                                newContribution = shortLimit;
                            }
                            Double newQuantity = newContribution * targetNav / pos.Price / 100;
                            output.Add(Trade.Create(null, new Position("", pos.Symbol, Side.Short, newQuantity, pos.Price, pos.Sector, pos.DollarDelta), 0, newContribution));
                        }
                    }
                }

                if (extraLong != 0)
                {
                    // get the eligible trades in sorted order
                    var a = output.OrderBy(x => x.TargetContribution).ToList().Where(x => x.TargetContribution > 0 && x.TargetContribution < longLimit).ToList();
                    Double total = a.Sum(x => x.TargetContribution);
                    foreach (var g in a)
                    {
                        Double eligible = g.TargetContribution / total * extraLong;
                        if (g.TargetContribution + eligible <= longLimit)
                        {
                            g.TargetContribution = g.TargetContribution + eligible;
                        }
                        else
                        {
                            // the new total exeeds the limit, assign it all, and recalculate 'total'
                            g.TargetContribution = longLimit;
                            total = output.OrderBy(x => x.TargetContribution).ToList().Where(x => x.TargetContribution > 0 && x.TargetContribution < longLimit).ToList().Sum(x => x.TargetContribution);
                        }
                        // calculate new quantity based on the contribution
                        g.TargetQuantity = Math.Floor(g.TargetContribution * targetNav / g.SelectedFeedPrice / 100);
                        g.TradeQuantity = Math.Abs(g.TargetQuantity - g.OriginalQuantity);
                    }
                }

                if (extraShort != 0)
                {
                    // get the eligible trades in sorted order
                    var a = output.OrderByDescending(x => x.TargetContribution).ToList().Where(x => x.TargetContribution < 0 && x.TargetContribution > shortLimit).ToList();
                    Double total = a.Sum(x => x.TargetContribution);
                    foreach (var g in a)
                    {
                        Double eligible = g.TargetContribution / total * extraShort;
                        if (g.TargetContribution + eligible >= shortLimit)
                        {
                            g.TargetContribution = g.TargetContribution + eligible;
                        }
                        else
                        {
                            // the new total exeeds the limit, assign it all, and recalculate 'total'
                            g.TargetContribution = shortLimit;
                            total = output.Where(x => x.TargetContribution < 0 && x.TargetContribution > shortLimit).ToList().Sum(x => x.TargetContribution);
                        }
                        // calculate new quantity based on the contribution
                        g.TargetQuantity = Math.Floor(g.TargetContribution * targetNav / g.SelectedFeedPrice / 100);
                        g.TradeQuantity = Math.Abs(g.TargetQuantity - g.OriginalQuantity);
                    }
                }


                return output;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
    }
}