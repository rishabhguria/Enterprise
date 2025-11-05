using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.KafkaWrapper
{
    public class NirvanaConsumer : IDisposable
    {
        private IConsumer<Null, RequestResponseModel> _kafkaConsumer = null;
        private CancellationTokenSource _cancellationTokenSource = null;
        private Timer _timer;
        private int _timerCount;
        private RequestResponseModel _requestResponseModel;
        private static ConcurrentDictionary<Guid, string> _responseCollection = new ConcurrentDictionary<Guid, string>();

        public ManualResetEvent ConsumerReadyEvent = new ManualResetEvent(false);
        private readonly ILogger<object> logger;

        public delegate void ConsumerReporter(string topic);
        public event ConsumerReporter ConsumerReporterEvent;

        public NirvanaConsumer()
        {
        }

        internal NirvanaConsumer(ConsumerConfig consumerConfig,
            string topic,
            RequestResponseModel requestResponseModel,
             ILogger<object> logger,
            bool isEarliest = false
           )
        {
            this.logger = logger;
            try
            {
                consumerConfig.GroupId = "cgrp-" + topic + "-" + requestResponseModel.RequestID;
                consumerConfig.AutoOffsetReset = isEarliest ? AutoOffsetReset.Earliest : AutoOffsetReset.Latest;
                //consumerConfig.HeartbeatIntervalMs = 10;
                _kafkaConsumer = new ConsumerBuilder<Null, RequestResponseModel>(consumerConfig)
                    .SetValueDeserializer(new KafkaSerializer()).Build();
                _kafkaConsumer.Subscribe(topic);

                _cancellationTokenSource = new CancellationTokenSource();
                _timer = new Timer(ResponseFetchTimeout, null, Timeout.Infinite, Timeout.Infinite);
                _requestResponseModel = requestResponseModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ResponseFetchTimeout(object state)
        {
            try
            {
                if (_timerCount > 5 || _responseCollection.ContainsKey(_requestResponseModel.RequestID))
                {
                    if (_timerCount > 5)
                    {
                        var subsList = String.Join(",", _kafkaConsumer?.Subscription?.ToArray());
                        Console.WriteLine("[" +
                            DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + "] Response is not received in defined time. Force cancelling the current request. RequestID: " + _requestResponseModel.RequestID);

                        logger.LogCritical("Response is not received in defined time {0} s for consumer {1}. Force cancelling the current request with id:{2} ",
                            _timerCount,
                            subsList,
                            _requestResponseModel.RequestID);
                    }

                    _cancellationTokenSource.Cancel();
                }
                _timerCount++;
            }
            catch (Exception exp)
            {
                LogMsg(LogLevel.Error, $"Exception in ResponseFetchTimeout method: {exp.Message + Environment.NewLine + exp.StackTrace}");
            }
        }

        internal async Task<string> Consume()
        {
            try
            {
                ConsumeResult<Null, RequestResponseModel> consumerResult = null;

                return await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        while (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            try
                            {
                                _timer.Change(10000, 10000);

                                ConsumerReadyEvent.Set();
                                consumerResult = _kafkaConsumer.Consume(_cancellationTokenSource.Token);


                                if (consumerResult != null
                                    && consumerResult.Message != null
                                    && consumerResult.Message.Value != null)
                                {
                                    var respObj = consumerResult.Message.Value;
                                    var isReqMatched = respObj.RequestID == _requestResponseModel.RequestID;

                                    LogMsg(LogLevel.Debug, "Kafka response message retrived for topic:{0}, req matched:{1} & responseId:{2}",
                                        consumerResult.Topic,
                                        isReqMatched.ToString(),
                                        respObj.RequestID
                                        );

                                    if (isReqMatched)
                                    {
                                        if (!respObj.IsSuccess)
                                            LogMsg(LogLevel.Error, "{0}", respObj.ErrorMsg);

                                        if (string.IsNullOrWhiteSpace(respObj.Data))
                                            LogMsg(LogLevel.Information, "Null or blank data recived in NirvanaConsumer for request id {0}",
                                                respObj.RequestID);

                                        _cancellationTokenSource.Cancel();

                                        if (ConsumerReporterEvent != null)
                                            ConsumerReporterEvent(consumerResult.Topic);
                                    }
                                    else
                                    {
                                        _responseCollection.TryAdd(respObj.RequestID,
                                            respObj.Data);
                                    }

                                }

                                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                            }
                            catch (OperationCanceledException ex)
                            {
                                var subsList = String.Join(",", _kafkaConsumer?.Subscription?.ToArray());
                                LogMsg(LogLevel.Error, $"OperationCanceledException in consume method for consumer {subsList}. Error: {ex.Message}");
                            }
                            catch (Exception exp)
                            {
                                LogMsg(LogLevel.Error, $"Exception in consume method of while loop: {exp.Message + Environment.NewLine + exp.StackTrace}");
                                Console.WriteLine(exp.Message + Environment.NewLine + exp.StackTrace);
                                throw;
                            }
                        }

                        if (consumerResult != null)
                        {
                            return consumerResult.Message.Value.Data;
                        }
                        else
                        {
                            string response;
                            if (_responseCollection.TryRemove(_requestResponseModel.RequestID, out response))
                            {
                                return response;
                            }
                            else
                            {
                                return string.Empty;
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        LogMsg(LogLevel.Error, $"Exception in consume method: {exp.Message + Environment.NewLine + exp.StackTrace}");
                        throw;
                    }
                    finally
                    {
                        _cancellationTokenSource.Cancel();
                        _kafkaConsumer.Close();
                        _kafkaConsumer.Dispose();
                        _kafkaConsumer = null;
                    }
                });
            }
            catch (Exception exp)
            {
                LogMsg(LogLevel.Error, $"Exception in consume method outer block : {exp.Message + Environment.NewLine + exp.StackTrace}");
                throw;
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        private void LogMsg(LogLevel level, string msg, params object[] obj)
        {
            if (logger == null)
            {
                string formattedMessage = string.Format(msg, obj);
                Console.WriteLine("[" + DateTime.Now.ToString(KafkaLoggingConstants.CONST_DATET_TIME_FORMAT) + "] " + formattedMessage);
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
