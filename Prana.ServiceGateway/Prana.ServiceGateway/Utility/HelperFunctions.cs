using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prana.ServiceGateway.Constants;
using Serilog;
using Prana.KafkaWrapper;
using System.Reflection;
using System.ComponentModel;
using System.IO.Compression;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Prana.ServiceGateway.CacheStore;

namespace Prana.ServiceGateway.Utility
{
    public class HelperFunctions
    {
        /// <summary>
        /// Get JWT token valition parameters from appsettings.json file
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        internal static TokenValidationParameters GetTokenValitionParam(IConfiguration configuration)
        {
            try
            {
                return new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration[UtilityConstants.CONST_AUTH_SETTINGS_ISSUER],
                    ValidAudience = configuration[UtilityConstants.CONST_AUTH_SETTINGS_AUDIENCE],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration[UtilityConstants.CONST_AUTH_SETTINGS_SECRET_KEY])),
                    ClockSkew = TimeSpan.Zero
                };
            }

            catch (Exception ex)
            {
                Log.ForContext<HelperFunctions>().Error(ex, "Error in Service");
                throw;
            }

        }

        /// <summary>
        ///Gets the enum decription from the ENUM
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string GetEnumDescription(Enum value)
        {
            try
            {
                // Get the Description attribute value for the enum value
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            catch (Exception ex)
            {
                Log.ForContext<HelperFunctions>().Error(ex, "Error in Service");
                throw;
            }
        }

        /// <summary>
        /// GetUpdatedTopicForResponse
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        public static string GetUpdatedTopicForResponse(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = topic;

                if (message.CompanyUserID != 0)
                {
                    int companyUserId = message.CompanyUserID;
                    topicToListen = topicToListen + "_" + companyUserId;
                }

                return topicToListen;
            }
            catch (Exception ex)
            {
                Log.ForContext<HelperFunctions>().Error(ex, "Error in LoadLayoutHandler");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        // Helper method to compress a string using GZip
        public static byte[] CompressData(string str)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(str);

                using (var outputStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        gzipStream.Write(bytes, 0, bytes.Length);
                    }
                    return outputStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Log.ForContext<HelperFunctions>().Error(ex, "Error in Compress Data");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public static double GetTimeDiffInSecs(string dateInLong)
        {
            try
            {
                long jsTimestamp = Convert.ToInt64(dateInLong);
                DateTime jsDate = DateTimeOffset.FromUnixTimeMilliseconds(jsTimestamp).UtcDateTime;
                DateTime serverDate = DateTime.UtcNow;
                TimeSpan timeDifference = serverDate - jsDate;
                double totalSeconds = timeDifference.TotalSeconds;
                return totalSeconds;
            }
            catch (Exception)
            {
                return -1;  //no need to log exceptions,
            }

        }

        /// <summary>
        /// This method is to check if broker connection status is changed for any of the available brokers
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool IsBrokerConnectionStatusChanged(RequestResponseModel message)
        {
            try
            {
                dynamic brokerStatus = JsonConvert.DeserializeObject<dynamic>(message.Data);
                foreach (var userWiseBrokerData in TradingTicketCache.GetInstance().BrokerData)
                {
                    var allBrokerData = JsonConvert.DeserializeObject<dynamic>(TradingTicketCache.GetInstance().BrokerData[userWiseBrokerData.Key]);
                    var allBrokerConnectionDetails = JsonConvert.DeserializeObject<List<JObject>>(allBrokerData.BrokerConnectionStatus.ToString());

                    foreach (var brokerConnectionDetail in allBrokerConnectionDetails)
                    {
                        if (brokerConnectionDetail[ServicesMethodConstants.CONST_COUNTER_PARTY_ID] == brokerStatus.CounterPartyID
                            && brokerConnectionDetail[ServicesMethodConstants.CONST_CONNECTION_ID] == brokerStatus.ConnectionID
                            && brokerConnectionDetail[ServicesMethodConstants.CONST_CONNECTION_STATUS] != brokerStatus.ConnStatus)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.ForContext<HelperFunctions>().Error(ex, "Error in IsBrokerConnectionStatusChanged");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
            return false;
        }
    }
}
