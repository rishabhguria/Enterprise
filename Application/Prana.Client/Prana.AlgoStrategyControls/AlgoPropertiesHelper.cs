using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.AlgoStrategyControls
{
    public class AlgoPropertiesHelper
    {
        /// <summary>
        /// Set Algo Parameters
        /// </summary>
        /// <param name="stageOrder"></param>
        public static void SetAlgoParameters(Prana.BusinessObjects.OrderSingle stageOrder)
        {
            try
            {
                if (!string.IsNullOrEmpty(stageOrder.AlgoStrategyID) && stageOrder.UnderlyingID > 0 && stageOrder.CounterPartyID > 0)
                {
                    Dictionary<string, AlgoStrategyUserControl> selectedStrategyCtrls;
                    AlgoStrategy selectedAlgostrat = null;

                    selectedStrategyCtrls = AlgoStrategyControls.AlgoControlsDictionary.GetInstance().GetSelectedStrategyControls(stageOrder.CounterPartyID.ToString(), stageOrder.AlgoStrategyID, Prana.CommonDataCache.CachedDataManager.GetInstance.GetUnderLyingText(stageOrder.UnderlyingID));
                    selectedAlgostrat = AlgoControlsDictionary.GetInstance().GetAlgoStrategyDatils(stageOrder.CounterPartyID.ToString(), stageOrder.AlgoStrategyID);
                    Dictionary<string, string> completeFixDict = new Dictionary<string, string>();
                    if (selectedStrategyCtrls != null)
                    {
                        foreach (KeyValuePair<string, AlgoStrategyUserControl> algoFixDictCtrlItem in selectedStrategyCtrls)
                        {
                            Dictionary<string, string> ctrlFixDict = algoFixDictCtrlItem.Value.GetFixValue();
                            if (ctrlFixDict != null)
                            {
                                foreach (KeyValuePair<string, string> pair in ctrlFixDict)
                                {
                                    if (!string.IsNullOrEmpty(pair.Value))
                                        completeFixDict.Add(pair.Key, pair.Value);
                                }
                            }
                        }
                        if (selectedAlgostrat != null)
                        {
                            foreach (KeyValuePair<string, string> pair in selectedAlgostrat.StrategyTagValues)
                            {
                                completeFixDict.Add(pair.Key, pair.Value);
                            }
                        }
                        OrderAlgoStartegyParameters algoProp = new OrderAlgoStartegyParameters();
                        algoProp.TagValueDictionary = completeFixDict;
                        stageOrder.AlgoProperties = algoProp;
                    }
                    else
                    {
                        stageOrder.AlgoStrategyID = string.Empty;
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
                throw;
            }
        }

    }
}