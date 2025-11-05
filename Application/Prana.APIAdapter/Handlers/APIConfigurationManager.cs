using AutoMapper;
using Newtonsoft.Json.Linq;
using Prana.APIAdapter.Models;
using Prana.APIAdapter.Sessions;
using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Prana.APIAdapter.Handlers
{
    internal class APIConfigurationManager
    {

        /// <summary>
        /// Initialize Configuration
        /// </summary>
        public static void InitializeConfiguration()
        {
            try
            {

                var configAuthentication = GetAuthenticationMappingConfig();
                var configPricing = GetAuthenticationMappingConfig();
                SessionManager.Instance.AuthenticationColumnMappingConfig = configAuthentication;
                SessionManager.Instance.PricingColumnMappingConfig = configPricing;


                ConfigurationRetriever config = new ConfigurationRetriever();
                HttpRequestConfigurationSection httpRequestConfig = config.HttpRequestConfig();
                var authHttpConfig = httpRequestConfig.HttpRequestConfigs["AuthenticationApi"];
                var dataHttpConfig = httpRequestConfig.HttpRequestConfigs["DataApi"];

                SessionManager.Instance.AuthHTTPRequestParameters = GetHttpRequestParam(authHttpConfig);
                SessionManager.Instance.PriceDataHTTPRequestParameters = GetHttpRequestParam(dataHttpConfig);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static HTTPRequestParameters GetHttpRequestParam(HttpRequestConfigElement httpConfig)
        {
            HTTPRequestParameters httpParams = new HTTPRequestParameters()
            {

                URL = httpConfig.URL,
                HttpMethod = new HttpMethod(httpConfig.HttpMethod),
                Content = GetHttpContent(httpConfig.HttpContents, httpConfig.HttpContentType),
                HttpHeader = GetHttpHeaders(httpConfig.HttpHeaders)

            };

            return httpParams;
        }

        /// <summary>
        /// Get Http Headers
        /// </summary>
        /// <param name="httpHeaderCollection"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetHttpHeaders(HttpHeaderCollection HttpHeaders)
        {
            var contentDict = new Dictionary<string, string>();
            foreach (HttpHeaderConfigElement item in HttpHeaders)
            {
                contentDict.Add(item.Key, item.Value);
            }
            return contentDict;
        }

        /// <summary>
        /// Get Http Content
        /// </summary>
        /// <param name="HttpContents"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static HttpContent GetHttpContent(HttpContentsCollection HttpContents, string contentType)
        {
            try
            {
                HttpContent httpContent = null;
                if (HttpContents != null)
                {

                    switch (contentType.ToLower())
                    {
                        case "application/x-www-form-urlencoded":
                            var contentDict = new Dictionary<string, string>();
                            foreach (HttpContentConfigElement item in HttpContents)
                            {
                                contentDict.Add(item.Key, item.Value);
                            }
                            httpContent = new FormUrlEncodedContent(contentDict);

                            break;
                    }
                }
                return httpContent;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get Authenticatio nMapping Config
        /// </summary>
        /// <returns></returns>
        private static MapperConfiguration GetAuthenticationMappingConfig()
        {
            Dictionary<string, string> columMapping = new Dictionary<string, string>();
            columMapping.Add("key", "key");
            columMapping.Add("expires", "expires");
            columMapping.Add("isAuthenticated", "isAuthenticated");
            columMapping.Add("message", "message");

            var config = new MapperConfiguration(cfg =>
            {

                var map = cfg.CreateMap<JObject, AuthSession>();
                foreach (KeyValuePair<string, string> item in columMapping)
                {
                    List<string> nestedItems = item.Key.Split('^').ToList();
                    if (nestedItems.Count == 1)
                        map.ForMember(item.Value, m => { m.MapFrom(s => s[item.Key]); });

                    else if (nestedItems.Count == 2)
                        map.ForMember(item.Value, m => { m.MapFrom(s => s[nestedItems[0]][nestedItems[1]]); });
                }
            });

            return config;
        }

        /// <summary>
        /// Get Price Mapping Config
        /// </summary>
        /// <returns></returns>
        private static MapperConfiguration GetPriceMappingConfig()
        {
            Dictionary<string, string> columMapping = new Dictionary<string, string>();
            columMapping.Add("Symbol", "Symbol");
            columMapping.Add("Cusip", "CusipNo");
            columMapping.Add("Sedol", "SedolSymbol");
            columMapping.Add("Isin", "ISIN");
            columMapping.Add("Bid", "Bid");
            columMapping.Add("Ask", "Ask");
            columMapping.Add("Last", "LastPrice");
            columMapping.Add("Best", "High");
            columMapping.Add("Close", "Previous");
            columMapping.Add("Vwap", "VWAP");
            //columMapping.Add("TClose", "");
            // columMapping.Add("CountryCode", "");
            //columMapping.Add("ExchangeRate", "");
            columMapping.Add("Currency", "CurencyCode");

            columMapping.Add("Name", "FullCompanyName");

            var config = new MapperConfiguration(cfg =>
            {
                var map = cfg.CreateMap<InstrumentPrice, SymbolData>();

                foreach (KeyValuePair<string, string> item in columMapping)
                {
                    List<string> nestedItems = item.Key.Split('^').ToList();
                    if (nestedItems.Count == 1)
                        map.ForMember(item.Value, m => { m.MapFrom(item.Key); });

                    //else if (nestedItems.Count == 2)
                    //    map.ForMember(item.Value, m => { m.MapFrom(s => s[nestedItems[0]][nestedItems[1]]); });
                }

            });

            return config;
        }

    }
}
