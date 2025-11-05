using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Prana.KafkaWrapper.Contracts;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using static Prana.KafkaWrapper.NirvanaConsumer;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Newtonsoft.Json;
using Prana.KafkaWrapper.Extension.Classes;

namespace Prana.KafkaWrapper
{
    public class KafkaManager : IKafkaManager
    {
        private NirvanaProducer _nirvanaProducer = null;
        private Dictionary<string, NirvanaSubscriber> _nirvanaSubscribers = null;

        private AdminClientConfig _adminClientConfig = null;
        private ProducerConfig _producerConfig = null;
        private ConsumerConfig _consumerConfig = null;
        private ConsumerConfig _subscriberConfig = null;

        public delegate void ProducerReporter(string topic);
        public event ProducerReporter ProducerReporterEvent;
        public event ConsumerReporter ConsumerReporterEvent;

        private static readonly KafkaManager instance = new KafkaManager(null);
        private readonly ILogger<KafkaManager> logger;
        

        public static KafkaManager Instance
        {
            get { return instance; }
        }

        public KafkaManager(ILogger<KafkaManager> logger)
        {
            this.logger = logger;
        }

        public void Initialize(string configPath)
        {
            try
            {
                if (!File.Exists(configPath))
                {
                    throw new Exception("Unable to find Kafka Producer-Consumer config file on provided path: " + configPath);
                }

                //There is no use for the line below, but if it is removed, GreenField Services will crash while consuming messages from Kafka.
                //Actually, we have to use Microsoft.Extensions.Configuration in the code so that the related assemblies are loaded
                new ConfigurationBuilder();

                #region Configuration
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(configPath);

                XmlNode AdminClientConfigNode = xmlDocument.SelectSingleNode("//KafkaConfiguration/AdminClientConfig");
                XmlNode ProducerConfigNode = xmlDocument.SelectSingleNode("//KafkaConfiguration/ProducerConfig");
                XmlNode ConsumerConfigNode = xmlDocument.SelectSingleNode("//KafkaConfiguration/ConsumerConfig");
                XmlNode SubscriberConfigNode = xmlDocument.SelectSingleNode("//KafkaConfiguration/SubscriberConfig");

                Dictionary<string, string> dictAdminClientConfig = new Dictionary<string, string>();
                if (AdminClientConfigNode != null)
                {
                    foreach (XmlNode node in AdminClientConfigNode.ChildNodes)
                    {
                        dictAdminClientConfig.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }
                }

                Dictionary<string, string> dictProducerConfig = new Dictionary<string, string>();
                if (ProducerConfigNode != null)
                {
                    foreach (XmlNode node in ProducerConfigNode.ChildNodes)
                    {
                        dictProducerConfig.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }
                }

                Dictionary<string, string> dictConsumerConfig = new Dictionary<string, string>();
                if (ConsumerConfigNode != null)
                {
                    foreach (XmlNode node in ConsumerConfigNode.ChildNodes)
                    {
                        dictConsumerConfig.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }
                }

                Dictionary<string, string> dictSubscriberConfig = new Dictionary<string, string>();
                if (SubscriberConfigNode != null)
                {
                    foreach (XmlNode node in SubscriberConfigNode.ChildNodes)
                    {
                        dictSubscriberConfig.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }
                }

                _adminClientConfig = new AdminClientConfig(dictAdminClientConfig);
                _producerConfig = new ProducerConfig(dictProducerConfig);
                _consumerConfig = new ConsumerConfig(dictConsumerConfig);
                _subscriberConfig = new ConsumerConfig(dictSubscriberConfig);
                #endregion

                _nirvanaProducer = new NirvanaProducer(_producerConfig );
                _nirvanaSubscribers = new Dictionary<string, NirvanaSubscriber>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateTopics()
        {
            try
            {
                bool isTopicCreationSuccessful;

                var sw = new Stopwatch();
                sw.Start();
                using (var adminClient = new AdminClientBuilder(_adminClientConfig).Build())
                {
                    try
                    {
                        LogMsg(LogLevel.Information, "Kafka topic creation process started...");

                        TopicSpecification[] topics = typeof(KafkaConstants).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                            .Where(fi => fi.IsLiteral)
                            .Select(field => new TopicSpecification { Name = field.GetRawConstantValue().ToString(), NumPartitions = 1, ReplicationFactor = 1 })
                            .ToArray();

                        await adminClient.CreateTopicsAsync(topics);

                        isTopicCreationSuccessful = true;
                    }
                    catch (CreateTopicsException e)
                    {
                        e.Results.ForEach(res =>
                        {
                            if (!res.Error.Reason.Equals("Success") && !res.Error.Reason.EndsWith("already exists."))
                                LogMsg(LogLevel.Information, "An error occured creating topic {resTopic}: {resonse}", res.Topic, res.Error.Reason);
                        });
                        isTopicCreationSuccessful = false;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"An error occured creating topic {e.Message}");
                        logger?.LogError(e, "An error occured creating topic");
                        isTopicCreationSuccessful = false;
                    }

                    LogMsg(LogLevel.Information, "Kafka topic creation process completed in {0} ms", sw.ElapsedMilliseconds);

                    //logger?.LogInformation("Kafka topic creation process completed in {0} ms", sw.ElapsedMilliseconds);
                }
                return isTopicCreationSuccessful;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Synchronous Request Response
        public async Task<string> ProduceAndConsume(RequestResponseModel requestResponseObj,
            string produceTopic,
            string consumeTopic,
            bool isEarliest = false)
        {
            NirvanaConsumer nirvanaConsumer = null;
            try
            {
                bool isTraceEnabled = logger != null && logger.IsEnabled(LogLevel.Trace);

                if (isTraceEnabled)
                {
                    var reqBody = JsonConvert.SerializeObject(requestResponseObj);
                    logger.LogTrace("Request body for the topic {0} is {1}", produceTopic, reqBody);
                }

                
                nirvanaConsumer = new NirvanaConsumer(ConsumerConfigClone(_consumerConfig),
                    consumeTopic,
                    requestResponseObj,
                    logger,
                    isEarliest);

                nirvanaConsumer.ConsumerReporterEvent += ConsumerReporterEvent;

                Task<string> task = nirvanaConsumer.Consume();

                if (ProducerReporterEvent != null)
                    ProducerReporterEvent(produceTopic);

                nirvanaConsumer.ConsumerReadyEvent.WaitOne();
                System.Threading.Thread.Sleep(2000);

                LogMsg(LogLevel.Information, "Kafka ProduceAndConsume event, ProduceTopic: {1}, ConsumeTopic: {2}",
                     produceTopic, consumeTopic); 

                await _nirvanaProducer.Produce(produceTopic, requestResponseObj);

                var respData = await task;

                if(isTraceEnabled)
                    logger.LogTrace("Response body for the topic {0} is {1}", consumeTopic, respData);

                return respData;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                nirvanaConsumer.Dispose();
            }
        }
        #endregion

        #region Asynchronous Request Response
        public async Task Produce(string produceTopic, RequestResponseModel requestResponseObj, bool isLoggingRequired = true)
        {
            try
            {
                if (ProducerReporterEvent != null && isLoggingRequired)
                    ProducerReporterEvent(produceTopic);

                await _nirvanaProducer.Produce(produceTopic, requestResponseObj, isLoggingRequired);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SubscribeAndConsume(string consumeTopic, Action<string, RequestResponseModel> callback, bool isEarliest = false)
        {
            try
            {
                lock (_nirvanaSubscribers)
                {
                    NirvanaSubscriber nirvanaSubscriber;
                    if (_nirvanaSubscribers.TryGetValue(consumeTopic, out nirvanaSubscriber))
                        nirvanaSubscriber.AddCallback(callback);
                    else
                    {
                        nirvanaSubscriber = new NirvanaSubscriber(ConsumerConfigClone(_subscriberConfig), consumeTopic, callback, isEarliest);
                        nirvanaSubscriber.ConsumerReporterEvent += ConsumerReporterEvent;
                        _nirvanaSubscribers.Add(consumeTopic, nirvanaSubscriber);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public void Cleanup(NirvanaConsumer[] nirvanaConsumers)
        {
            try
            {
                foreach (var nirvanaConsumer in nirvanaConsumers)
                    nirvanaConsumer.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ConsumerConfig ConsumerConfigClone(ConsumerConfig originalConfig)
        {
            try
            {
                ConsumerConfig clonedConfig = new ConsumerConfig();

                PropertyInfo[] properties = typeof(ConsumerConfig).GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (PropertyInfo property in properties)
                {
                    if (property.CanRead && property.CanWrite)
                    {
                        object value = property.GetValue(originalConfig);
                        property.SetValue(clonedConfig, value);
                    }
                }

                return clonedConfig;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is temp and will be removed once serilog logging is done in greenfield 
        /// as this project is reference in greenfield service too. 
        private void LogMsg(LogLevel level, string msg, params object[] obj)
        {
            if (logger == null)
            {
                for (int i = 0; i < obj.Length; i++)
                    msg = msg.Replace("{" + i + "}", obj[i]?.ToString());
                Console.WriteLine("[" + DateTime.Now.ToString(KafkaLoggingConstants.CONST_DATET_TIME_FORMAT) + "] " + msg);
            }
            else
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        logger.LogDebug(msg, obj);
                        break;
                    case LogLevel.Information:
                        logger.LogInformation(msg, obj);
                        break;
                    case LogLevel.Error:
                        logger.LogError(msg, obj);
                        break;
                    default:
                        logger.LogInformation(msg, obj);
                        break;
                }
            }
        }
    }
}