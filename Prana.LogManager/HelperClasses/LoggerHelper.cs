using System;

namespace Prana.LogManager
{
    public static class LoggerHelper
    {
        /// <summary>
        /// This is to add property in logs of serilog, Here we can have only correlationId, kakfaRqId, and userId
        /// Any new property that need to be added, modification is req in serilogSection of app.config.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="kafkaReqId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static IDisposable PushLoggingProperties(
            string correlationId,
            Guid? kafkaReqId,
            int? userid)
        {
            try
            {
                var correlationIdProperty = Logger.PushProperty(LoggingConstants.SerilogConstant.CORRELATION_ID, correlationId);
                var kafkaRequestIdProperty = Logger.PushProperty(LoggingConstants.SerilogConstant.KAFKA_REQUEST_ID, Convert.ToString(kafkaReqId));
                var userIdProperty = Logger.PushProperty(LoggingConstants.SerilogConstant.USER_ID, Convert.ToString(userid));

                return new CompositeDisposable(correlationIdProperty, kafkaRequestIdProperty, userIdProperty);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "PushLoggingProperties encountered an error");
                return Logger.PushProperty(LoggingConstants.SerilogConstant.CORRELATION_ID, "Error");
            }
        }

        public static IDisposable PushUserPropInLogging(int userId)
        {
            return Logger.PushProperty(LoggingConstants.SerilogConstant.USER_ID, Convert.ToString(userId));
        }


        private class CompositeDisposable : IDisposable
        {
            private readonly IDisposable[] _disposables;

            public CompositeDisposable(params IDisposable[] disposables)
            {
                _disposables = disposables;
            }

            public void Dispose()
            {
                foreach (var disposable in _disposables)
                {
                    if (disposable == null)
                        continue;
                    disposable.Dispose();
                }
            }
        }
    }
}