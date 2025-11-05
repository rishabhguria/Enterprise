// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-14-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-15-2013
// ***********************************************************************
// <copyright file="Security.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using Prana.BusinessObjects.AppConstants;
using System;


namespace Bloomberg.Library
{
    /// <summary>
    /// Symbol Request
    /// </summary>
    [Serializable]
    public class SecurityInfo
    {
        /// <summary>
        /// The security
        /// </summary>
        public AssetCategory AssetCategory = AssetCategory.None;
        public readonly string Security = null;
        public string Asset = null;
        public readonly string Product = string.Empty;
        public readonly CorrelationID Key = null;
        public bool IsOption = false;
        public DateTime ExpirationDate;
        public double StrikePrice = 0.0;
        public string OptionType = "None";

        static string[] ValidTypes = { " equity", " index", " comdty", " curncy", " govt", " corp", " pfd", " mtge", " muni", " mmkt" };
        public static string[] FixedIncome = { "Govt", "Corp", "Pfd", "Mtge", "Muni", "Mmkt" };

        /// <summary>
        /// Make sure symbol has proper suffix
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static bool Validate(string symbol)
        {
            for (int i = 0; i < ValidTypes.Length; i++)
                if (symbol.ToLower().Contains(ValidTypes[i].ToLower()))
                    return true;

            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Security" /> class.
        /// </summary>
        /// <param name="securities">The securities.</param>
        /// <param name="ticker">The ticker.</param>
        public SecurityInfo(Element securities, string security)
        {
            securities.AppendValue(security);
            this.Security = (string)securities[securities.NumValues - 1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="security"></param>
        /// <param name="asset"></param>
        private SecurityInfo(string security, string asset)
        {
            string[] ticker = security.Split(' ');
            Product = ticker[ticker.Length - 1];
            Security = security.Replace(Product, "").Trim();
            Asset = asset;
            Key = new CorrelationID(security);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="security"></param>
        public SecurityInfo(string security)
        {
            string[] ticker = security.Trim().Split(' ');

            Product = ticker[ticker.Length - 1];
            Security = security.Replace(Product, "").Trim();
            Asset = Product;
            Key = new CorrelationID(security);


            //if (Product.ToLower() == "equity")
            //{
            //    ticker = security.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //    for (int i = 0; i < ticker.Length; i++)
            //    {
            //        DateTime time;
            //        if (DateTime.TryParse(ticker[i], out time))
            //        {
            //            IsOption = true;
            //            OptionType = ticker[i + 1].Substring(0,1);
            //            StrikePrice = double.Parse(ticker[i+1].Substring(1));
            //            ExpirationDate = time;
            //            Asset = "EquityOption";                       

            //            break;
            //        }
            //    }
            //}
            //else if (Product.ToLower() == "comdty" || Product.ToLower() == "index")
            //{
            //    ticker = security.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //    if (ticker.Length >= 3)
            //    {                    
            //        if (Double.TryParse(ticker[ticker.Length - 2], out StrikePrice))
            //        {
            //            IsOption = true;
            //            OptionType = ticker[0].Substring(ticker[0].Length - 1);
            //            Asset = "FutureOption";
            //        }
            //        else if (Product.ToLower() == "index")
            //        {
            //            Asset = "Indices";
            //        }
            //    }
            //    else if (Product.ToLower() == "comdty")
            //    {
            //        Asset = "Future";
            //    }
            //}
            //else if (Product.ToLower() == "curncy")
            //{
            //    Asset = "FX";
            //}
            //else
            //{
            //    foreach (string item in FixedIncome)
            //    {
            //        if (Product == item)
            //        {
            //            Asset = "FixedIncome";
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="security"></param>
        /// <param name="product"></param>
        /// <param name="asset"></param>
        private SecurityInfo(string security, string product, string asset)
        {
            Product = product;
            Security = security;
            Asset = asset;
            Key = new CorrelationID(security);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="security"></param>
        /// <param name="product"></param>
        /// <param name="asset"></param>
        /// <param name="key"></param>
        private SecurityInfo(string security, string product, string asset, CorrelationID key)
        {
            Product = product;
            Security = security;
            Asset = asset;
            Key = key;
        }

        /// <summary>
        /// Used to pass to bloomberg
        /// </summary>
        public string RequestId
        {
            get
            {
                if (Product == null) return Security;

                if (Security.Contains(Product) == false)
                    return string.Format("{0} {1}", Security, Product);
                else
                    return Security;
            }
        }

        /// <summary>
        /// Returns the OptionType Enum
        /// </summary>
        /// <returns></returns>
        public Prana.BusinessObjects.AppConstants.OptionType GetOptionType()
        {
            if (OptionType == "P")
                return Prana.BusinessObjects.AppConstants.OptionType.PUT;
            else if (OptionType == "C")
                return Prana.BusinessObjects.AppConstants.OptionType.CALL;
            else
                return Prana.BusinessObjects.AppConstants.OptionType.NONE;
        }

        /// <summary>
        /// Return the enum equivant
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Prana.BusinessObjects.AppConstants.OptionType GetOptionType(string value)
        {
            //note: bloomberg has additional types
            //'C' for a call option, 'P' for a put option, 'T' for a touch option, 'F' for a forward or 'M' for a multi-leg option.
            if (value.StartsWith("P"))
                return Prana.BusinessObjects.AppConstants.OptionType.PUT;
            else if (value.StartsWith("C"))
                return Prana.BusinessObjects.AppConstants.OptionType.CALL;
            else
                return Prana.BusinessObjects.AppConstants.OptionType.NONE;
        }
    }
}
