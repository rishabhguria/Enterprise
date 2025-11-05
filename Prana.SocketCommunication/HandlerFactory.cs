using Prana.BusinessObjects;
using Prana.Interfaces;
using System;
using System.Collections.Generic;
namespace Prana.SocketCommunication
{
    public class HandlerFactory
    {
        static Dictionary<HandlerType, IQueueProcessor> _dictHandlers = new Dictionary<HandlerType, IQueueProcessor>();
        public static void RegisterHandler(IQueueProcessor queueProcessor)
        {
            if (!_dictHandlers.ContainsKey(queueProcessor.HandlerType))
                _dictHandlers.Add(queueProcessor.HandlerType, queueProcessor);
        }

        public static void UnRegisterHandler(HandlerType handlerType)
        {
            if (_dictHandlers.ContainsKey(handlerType))
                _dictHandlers.Remove(handlerType);
        }

        public static void SendToHandler(QueueMessage qMsg, HandlerType handlerType)
        {
            if (_dictHandlers.ContainsKey(handlerType))
            {
                _dictHandlers[handlerType].SendMessage(qMsg);
            }
            //Raturi: Throw the exception only if the handler is set and does not exist in the dictionary
            //http://jira.nirvanasolutions.com:8080/browse/PRANA-7115
            else if (handlerType != HandlerType.NotSet)
            {
                throw new Exception("Handler: " + handlerType.ToString() + "\n qMsg: " + qMsg.ToString());
            }
        }
    }
}
