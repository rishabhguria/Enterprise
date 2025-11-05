using System;
using System.Collections.Generic;
using System.Threading;

namespace Prana.Utilities.MiscUtilities
{
    /// <summary>
    /// Can be used to call any function for specified number of retries if exception is thrown
    /// http://stackoverflow.com/questions/1563191/c-sharp-cleanest-way-to-write-retry-logic
    /// RetryHelper.Do(() => SomeFunctionThatCanFail(), TimeSpan.FromSeconds(1));
    /// RetryHelper.Do(SomeFunctionThatCanFail, TimeSpan.FromSeconds(1));
    /// int result = RetryHelper.Do(SomeFunctionWhichReturnsInt, TimeSpan.FromSeconds(1), 4);
    /// </summary>
    public static class RetryHelper
    {
        public static void Do(Action action, TimeSpan retryInterval, int retryCount = 3)
        {
            Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, retryCount);
        }

        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    Thread.Sleep(retryInterval);
                }
            }

            throw new AggregateException(exceptions);
        }

        public static T Do<T2, T>(Func<T2, T> action, TimeSpan retryInterval, T2 argument, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return action(argument);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    Thread.Sleep(retryInterval);
                }
            }
            throw new AggregateException(exceptions[0].Message, exceptions);
        }
    }
}
