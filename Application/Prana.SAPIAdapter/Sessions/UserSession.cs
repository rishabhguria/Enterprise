// ***********************************************************************
// Assembly         : Bloomberg Library
// Author           : MJCarlucci
// Created          : 04-29-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 04-29-2013
// ***********************************************************************
// <copyright file="UserSession.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberg.Library.Requests;
using Bloomberglp.Blpapi;
using Prana.LogManager;
using System.Collections.Generic;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class UserSession
    /// </summary>
    public class UserSession : Session
    {

        /// <summary>
        /// The event queue
        /// </summary>
        private readonly EventQueue eventQueue = new EventQueue();


        /// <summary>
        /// Initializes a new instance of the <see cref="UserSession" /> class.
        /// </summary>
        public UserSession()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSession" /> class.
        /// </summary>
        /// <param name="sessionOptions">The session options.</param>
        /// <param name="handler">The handler.</param>
        public UserSession(SessionOptions sessionOptions, Bloomberglp.Blpapi.EventHandler handler)
            : base(sessionOptions, handler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSession" /> class.
        /// </summary>
        /// <param name="sessionOptions">The session options.</param>
        public UserSession(SessionOptions sessionOptions)
            : base(sessionOptions, ProcessEvents)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSession" /> class.
        /// </summary>
        /// <param name="sessionOptions">The session options.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public UserSession(SessionOptions sessionOptions, Bloomberglp.Blpapi.EventHandler handler, EventDispatcher dispatcher)
            : base(sessionOptions, handler, dispatcher)
        {
        }

        /// <summary>
        /// Opens the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The service.</param>
        /// <returns>``0.</returns>
        public T OpenService<T>(T service) where T : UserService
        {
            if (base.OpenService(service.Url) == false)
                return null;

            service.Session = this;

            service.Service = base.GetService(service.Url);
            return service;
        }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The request.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>CorrelationID.</returns>
        public CorrelationID SendRequest<T>(T request, CorrelationID correlationId) where T : AbstractRequest
        {
            return base.SendRequest(request.request, correlationId);
        }

        /// <summary>
        /// Sends the authorization request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>CorrelationID.</returns>
        public CorrelationID SendAuthorizationRequest(AuthorizationRequest request)
        {
            return SendAuthorizationRequest(request.request, request.Identity, request.CorrelationId);
        }

        /// <summary>
        /// Processes the events.
        /// </summary>
        /// <param name="eventObj">The event obj.</param>
        /// <param name="session">The session.</param>
        private static void ProcessEvents(Event eventObj, Session session)
        {
            try
            {
                foreach (Message message in eventObj)
                {
                }
            }
            catch (System.Exception e)
            {
                Logger.LoggerWrite(e.ToString());
            }
        }

        /// <summary>
        /// Prints the event.
        /// </summary>
        /// <param name="eventObj">The event obj.</param>
        private static void printEvent(Event eventObj)
        {
            foreach (Message msg in eventObj)
            {
                CorrelationID correlationId = msg.CorrelationID;
                if (correlationId != null)
                {
                    Logger.LoggerWrite("Correlator: " + correlationId);
                }

                Service service = msg.Service;
                if (service != null)
                {
                    Logger.LoggerWrite("Service: " + service.Name);
                }
                Logger.LoggerWrite(msg);
            }
        }

        /// <summary>
        /// Subscribes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Subscribe(MarketDataRequest request)
        {
            base.Subscribe(request.Subscriptions);
        }

        /// <summary>
        /// Element Iterator
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="tree">The tree.</param>
        /// <param name="node">The node.</param>
        public static void IterateElements(Element parent, ElementTree tree, LinkedListNode<Element> node)
        {
            if (node == null)
            {
                node = new LinkedListNode<Element>(parent);
                tree.AddFirst(node);
            }
            else
                tree.AddAfter(node, parent);

            foreach (Element child in parent.Elements)
            {
                for (int index = 0; index < child.NumValues; index++)
                {
                    if (child.IsArray && child.Datatype == Schema.Datatype.SEQUENCE)
                    {
                        IterateElements(child.GetValueAsElement(index), tree, node);
                    }
                }
                IterateElements(child, tree, node);
            }
        }

    }
}
