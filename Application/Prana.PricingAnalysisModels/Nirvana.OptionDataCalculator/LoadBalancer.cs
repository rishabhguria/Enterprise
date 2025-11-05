using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.OptionCalculator.Common;
using Prana.QueueManager;
using Prana.SocketCommunication;
using System;
using System.Collections.Generic;
using System.Threading;
namespace Prana.OptionCalculator.CalculationComponent
{
    public class LoadBalancer
    {
        static LoadBalancer _loadBalancer = null;
        static object locker = new object();
        List<GreeksCalculator> _lstGreekCalculator = new List<GreeksCalculator>();
        BroadcastMemoryQueueManager _greekQueue = null;
        DataCopyComponent _datacopyComponent = null;

        private LoadBalancer()
        {
        }

        public static LoadBalancer GetInstance
        {
            get
            {
                if (_loadBalancer == null)
                {
                    lock (locker)
                    {
                        if (_loadBalancer == null)
                        {
                            _loadBalancer = new LoadBalancer();
                        }
                    }
                }
                return _loadBalancer;
            }
        }

        public void Initialise(IQueueProcessor outPricingQueue)
        {
            try
            {
                for (int count = 0; count < FeedBackDetails.MAX_THREADSALLOWEDTO_GREEK_CALC; count++)
                {
                    GreeksCalculator greekCalc = new GreeksCalculator(outPricingQueue, count);
                    _lstGreekCalculator.Add(greekCalc);
                }

                _greekQueue = new BroadcastMemoryQueueManager();
                _greekQueue.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(_greekQueue_MessageReceived);
                _datacopyComponent = new DataCopyComponent(_greekQueue);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void _greekQueue_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {
            CalculateGreek(e.Value);
        }

        private void CalculateGreek(QueueMessage message)
        {
            try
            {
                Dictionary<string, SymbolData> dictfullLiveFeedData = (Dictionary<string, SymbolData>)message.Message;
                switch (message.MsgType)
                {
                    case OptionDataFormatter.MSGTYPE_PricingData:
                    case OptionDataFormatter.MSGTYPE_PricingDataFile:

                        #region Live Data
                        int numberofSymbols = dictfullLiveFeedData.Count;
                        int workinglot = Convert.ToInt16(numberofSymbols / FeedBackDetails.MAX_THREADSALLOWEDTO_GREEK_CALC);
                        int numberOfWorkingThreads = FeedBackDetails.MAX_THREADSALLOWEDTO_GREEK_CALC;
                        if (workinglot == 0)
                        {
                            workinglot = numberofSymbols;
                            numberOfWorkingThreads = 1;
                        }

                        WaitHandle[] events = new WaitHandle[numberOfWorkingThreads];
                        List<Dictionary<string, SymbolData>> dividedTaskArray = new List<Dictionary<string, SymbolData>>();
                        int loopCount = 0;
                        int lotCount = 1;
                        foreach (KeyValuePair<string, SymbolData> keyValueData in dictfullLiveFeedData)
                        {
                            if (loopCount == workinglot && numberOfWorkingThreads > lotCount)
                            {
                                loopCount = 0;
                                lotCount++;
                            }
                            if (loopCount == 0)
                            {
                                Dictionary<string, SymbolData> liveFeedData = new Dictionary<string, SymbolData>();
                                dividedTaskArray.Add(liveFeedData);
                            }
                            dividedTaskArray[dividedTaskArray.Count - 1].Add(keyValueData.Key, keyValueData.Value);
                            loopCount++;
                        }
                        int count = 0;
                        foreach (GreeksCalculator greekCalc in _lstGreekCalculator)
                        {
                            if (numberOfWorkingThreads > count) //calculate only for Required Threads
                            {
                                events[count] = new AutoResetEvent(false);
                                QueueMessage queueMsg = new QueueMessage(message.MsgType, dividedTaskArray[count]);
                                greekCalc.CalculateGreek(queueMsg, events[count]);
                                count++;
                            }
                        }
                        //wait for all of them to finish
                        WaitHandle.WaitAll(events);
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
