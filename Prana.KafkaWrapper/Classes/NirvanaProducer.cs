using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Prana.KafkaWrapper
{
    internal class NirvanaProducer
    {
        private IProducer<Null, RequestResponseModel> _producer = null;
        internal NirvanaProducer(ProducerConfig producerConfig)
        {
            try
            {
                _producer = new ProducerBuilder<Null, RequestResponseModel>(producerConfig)
                    .SetValueSerializer(new KafkaSerializer())
                    .Build();
            }
            catch (Exception)
            {
                throw;
            }

        }

        internal async Task Produce(string topic, RequestResponseModel requestResponseObj, bool isLoggingRequired = true)
        {
            try
            {
                DeliveryResult<Null, RequestResponseModel> deliveryResult = await _producer.ProduceAsync(topic,
                    new Message<Null, RequestResponseModel> { Value = requestResponseObj });

                //if (isLoggingRequired)
                //    Console.WriteLine("[" + DateTime.Now.ToString(KafkaLoggingConstants.CONST_DATET_TIME_FORMAT) + "] Produced > RequestID: "
                //    + requestResponseObj.RequestID + ", ProduceTopic: " + topic);
            }
            catch (ProduceException<Null, RequestResponseModel> exp)
            {
                Console.WriteLine($"Error producing message: {exp.Error?.Reason}");
                Console.WriteLine(exp.StackTrace);
                throw;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message + Environment.NewLine + exp.StackTrace);
                throw;
            }
            finally
            {
                _producer.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}
