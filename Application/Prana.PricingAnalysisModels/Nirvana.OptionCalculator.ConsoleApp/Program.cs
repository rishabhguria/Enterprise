using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.OptionCalculator.CalculationComponent;
using Nirvana.OptionCalculator.OptionConnectionManager;
using Nirvana.QueueManager;
using Nirvana.Server.Core;
namespace Nirvana.OptionCalculator.ConsoleApp
{
    class Program
    {
        static QueueProcessor  _impliedVolCalculatedqueue =null;
        static QueueProcessor _greekCalculatedQueue = null;
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Calculation Manager \n");
            _greekCalculatedQueue = new QueueProcessor(null);
            _impliedVolCalculatedqueue = new QueueProcessor(null);
            LoadBalancer.GetInstance.Initilise(_impliedVolCalculatedqueue, _greekCalculatedQueue);
            IQueueProcessor _incomingMsgQueue = new QueueProcessor(null);
            IQueueProcessor _sendMessageQueue = new QueueProcessor(null);
            ConnectionMgr.GetInstance.Initlise(_incomingMsgQueue);
            CommunicationManager.GetInstance().Initialise(_incomingMsgQueue, _sendMessageQueue);
            Dispatcher.GetInstance.Initlise(_greekCalculatedQueue, _impliedVolCalculatedqueue, _sendMessageQueue);
            
        }
    }
}
