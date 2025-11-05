using Prana.KafkaWrapper;
using Prana.LogManager;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.GreenfieldServices.Common
{
    public static class ServiceConnectionPoller
    {
        public static void PollUntilServiceReady(
            Func<bool> isConnected,
            string requestTopic,
            string serviceDisplayName,
            bool useNewThread = true,
            int intervalMs = 3000)
        {
            async Task PollLoop()
            {
                while (!isConnected())
                {
                    Logger.LogMsg(LoggerLevel.Information, $"Requesting to {serviceDisplayName}...");

                    try
                    {
                        await KafkaManager.Instance.Produce(requestTopic, new RequestResponseModel(0, ""));
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, $"Error producing to {requestTopic}");
                    }

                    await Task.Delay(intervalMs);
                }
            }

            if (useNewThread)
            {
                _ = Task.Run(PollLoop);
            }
            else
            {
                // Run same loop synchronously (blocking)
                PollLoop().GetAwaiter().GetResult();
            }
        }
    }
}
