// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-11-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-11-2013
// ***********************************************************************
// <copyright file="UserService.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class UserService
    /// </summary>
    public abstract class UserService : Service
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public abstract string Url { get; set; }

        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        /// <value>The service.</value>
        public Service Service;

        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>The session.</value>
        public UserSession Session;

        /// <summary>
        /// Gets the name of the authorization service.
        /// </summary>
        /// <value>The name of the authorization service.</value>
        public override string AuthorizationServiceName
        {
            get { return Service.AuthorizationServiceName; }
        }
        /// <summary>
        /// Creates the admin event.
        /// </summary>
        /// <returns>Event.</returns>
        [Obsolete("This method overrides an obsolete member in the base class.")]
        public override Event CreateAdminEvent()
        {
            return Service.CreateAdminEvent();
        }

        /// <summary>
        /// Creates the authorization request.
        /// </summary>
        /// <returns>Request.</returns>
        public override Request CreateAuthorizationRequest()
        {
            return Service.CreateAuthorizationRequest();
        }
        /// <summary>
        /// Creates the publish event.
        /// </summary>
        /// <returns>Event.</returns>
        public override Event CreatePublishEvent()
        {
            return Service.CreatePublishEvent();
        }
        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <param name="operationName">Name of the operation.</param>
        /// <returns>Request.</returns>
        public override Request CreateRequest(string operationName)
        {
            return Service.CreateRequest(operationName);
        }
        /// <summary>
        /// Creates the response event.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>Event.</returns>
        public override Event CreateResponseEvent(CorrelationID correlationId)
        {
            return Service.CreateResponseEvent(correlationId);
        }
        /// <summary>
        /// Gets the event definition.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>SchemaElementDefinition.</returns>
        public override SchemaElementDefinition GetEventDefinition(Name name)
        {
            return Service.GetEventDefinition(name);
        }
        /// <summary>
        /// Gets the event definition.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>SchemaElementDefinition.</returns>
        public override SchemaElementDefinition GetEventDefinition(string name)
        {
            return Service.GetEventDefinition(name);
        }
        /// <summary>
        /// Gets the event definition.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>SchemaElementDefinition.</returns>
        public override SchemaElementDefinition GetEventDefinition(int index)
        {
            return Service.GetEventDefinition(index);
        }
        /// <summary>
        /// Gets the operation.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Operation.</returns>
        public override Operation GetOperation(Name name)
        {
            return Service.GetOperation(name);
        }
        /// <summary>
        /// Gets the operation.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Operation.</returns>
        public override Operation GetOperation(string name)
        {
            return Service.GetOperation(name);
        }
        /// <summary>
        /// Gets the operation.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Operation.</returns>
        public override Operation GetOperation(int index)
        {
            return Service.GetOperation(index);
        }
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get
            {
                return Service.Name;
            }
        }
        /// <summary>
        /// Gets the num event definitions.
        /// </summary>
        /// <value>The num event definitions.</value>
        public override int NumEventDefinitions
        {
            get
            {
                return Service.NumEventDefinitions;
            }
        }
        /// <summary>
        /// Gets the num operations.
        /// </summary>
        /// <value>The num operations.</value>
        public override int NumOperations
        {
            get
            {
                return Service.NumOperations;
            }
        }
        /// <summary>
        /// Prints the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Print(System.IO.TextWriter writer)
        {
            Service.Print(writer);
        }
        /// <summary>
        /// Prints the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        public override void Print(System.IO.Stream output)
        {
            Service.Print(output);
        }
    }

}
