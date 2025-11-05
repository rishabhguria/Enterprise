using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Prana.KafkaWrapper.NirvanaConsumer;

namespace Prana.KafkaWrapper
{
    internal class NirvanaSubscriber : IDisposable
    {
        private List<Action<string, RequestResponseModel>> _callbacks = null;

        private IConsumer<Null, RequestResponseModel> _kafkaConsumer = null;
        private CancellationTokenSource _cancellationTokenSource = null;
        public event ConsumerReporter ConsumerReporterEvent;

        internal NirvanaSubscriber(ConsumerConfig subscriberConfig, string topic, Action<string, RequestResponseModel> callback, bool isEarliest = false)
        {
            try
            {
                _callbacks = new List<Action<string, RequestResponseModel>> { callback };

                subscriberConfig.GroupId = "sgrp-" + topic + "-" + Guid.NewGuid();
                subscriberConfig.AutoOffsetReset = isEarliest ? AutoOffsetReset.Earliest : AutoOffsetReset.Latest;

                _kafkaConsumer = new ConsumerBuilder<Null, RequestResponseModel>(subscriberConfig).SetValueDeserializer(new KafkaSerializer()).Build();
                _kafkaConsumer.Subscribe(topic);

                _cancellationTokenSource = new CancellationTokenSource();

                Consume();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void Consume()
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        try
                        {
                            var consumerResult = _kafkaConsumer.Consume(_cancellationTokenSource.Token);
                            if (consumerResult.Message != null)
                            {
                                if (ConsumerReporterEvent != null)
                                {
                                    ConsumerReporterEvent(consumerResult.Topic);
                                }

                                foreach (var callback in _callbacks)
                                {
                                    callback(consumerResult.Topic, consumerResult.Message.Value);
                                }

                                _kafkaConsumer.Commit(consumerResult);
                            }
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine(exp.Message + Environment.NewLine + exp.StackTrace);
                            throw;
                        }
                    }
                });
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message + Environment.NewLine + exp.StackTrace);
                throw;
            }
        }

        internal void AddCallback(Action<string, RequestResponseModel> callback)
        {
            try
            {
                _callbacks.Add(callback);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                if (_kafkaConsumer != null)
                {
                    _kafkaConsumer.Close();
                    _kafkaConsumer.Dispose();
                    _kafkaConsumer = null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
