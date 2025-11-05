using System;
using System.Collections.Generic;
using Prana.BusinessObjects.AppConstants;

namespace Prana.BlpDLWSAdapter
{
    /// <summary>
    /// Symbol Request   Copied from Matt's Sapi adapter to suit the needs.
    /// </summary>
    [Serializable]
    public class SecurityInfo
    {
        /// <summary>
        /// The security
        /// </summary>
        //public AssetCategory AssetCategory = AssetCategory.None;
        private readonly string Security = null;
        public string Asset = null;
        private readonly string Product = string.Empty;
        //public bool IsOption = false;
        //public DateTime ExpirationDate;
        //public double StrikePrice = 0.0;
        private string OptionType = "None";

        static string[] ValidTypes =  { " equity", " index", " comdty", " curncy", " govt", " corp", " pfd", " mtge", " muni", " mmkt" };
        public static string[] FixedIncome = { "Govt", "Corp", "Mtge", "Muni", "Mmkt","Mmkt Curncy" };

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
                return Prana.BusinessObjects.AppConstants.OptionType.Put;
            else if (OptionType == "C") 
                return Prana.BusinessObjects.AppConstants.OptionType.Call;
            else             
                return Prana.BusinessObjects.AppConstants.OptionType.None;
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
            if (value.StartsWith("P", StringComparison.InvariantCultureIgnoreCase))
                return Prana.BusinessObjects.AppConstants.OptionType.Put;
            else if (value.StartsWith("C", StringComparison.InvariantCultureIgnoreCase))
                return Prana.BusinessObjects.AppConstants.OptionType.Call;            
            else
                return Prana.BusinessObjects.AppConstants.OptionType.None;
        }
    }
}
