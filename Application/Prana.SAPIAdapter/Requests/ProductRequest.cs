// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-11-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-14-2013
// ***********************************************************************
// <copyright file="UserRequest.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System;


namespace Bloomberg.Library.Requests
{
    /// <summary>
    /// Class UserRequest
    /// </summary>
    public class ProductRequest : AbstractRequest
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRequest"></see> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public ProductRequest(Request request)
            : base(request)
        {

        }

        /// <summary>
        /// Adds the security.
        /// </summary>
        /// <param name="ticker">The ticker.</param>
        /// <param name="asset">The asset.</param>
        public virtual void AddSecurity(string ticker, string asset)
        {
            string value = string.Format("{0} {1}", ticker, asset);
            GetElement("securities").AppendValue(value);
        }

        /// <summary>
        /// Adds the security.
        /// </summary>
        /// <param name="ticker">The ticker.</param>
        /// <param name="asset">The asset.</param>
        public virtual void AddSecurity(string ticker, SecurityType asset)
        {
            string value = string.Format("{0} {1}", ticker, ElementValue.ElementName(asset));
            GetElement("securities").AppendValue(value);
        }

        /// <summary>
        /// Adds the securities.
        /// </summary>
        /// <param name="securities">The securities.</param>
        public virtual void AddSecurities(params string[] securities)
        {
            foreach (string security in securities)
            {
                GetElement("securities").AppendValue(security);
            }
        }

        /// <summary>
        /// Adds the fields.
        /// </summary>
        /// <param name="values">The values.</param>
        public virtual void AddFields(params string[] values)
        {
            foreach (string value in values)
            {
                GetElement("fields").AppendValue(value);

            }
        }
        /// <summary>
        /// Adds the overrides.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public virtual void AddOverride(string name, string value)
        {
            Element a = request.GetElement(new Name("overrides"));
            Element b = a.AppendElement();
            b.SetElement(new Name("fieldId"), name);
            b.SetElement(new Name("value"), value);
        }
    }
}
