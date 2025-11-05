using System;
using System.Configuration;

namespace Prana.APIAdapter
{
    public class ConfigurationRetriever
    {


        public HttpRequestConfigurationSection HttpRequestConfig()
        {
            try
            {
                HttpRequestConfigurationSection section = ConfigurationManager.GetSection("APIConfig") as HttpRequestConfigurationSection;
                return section;
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
