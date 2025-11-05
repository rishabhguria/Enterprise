using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.ThirdPartyManager.BusinessLogic;
using Quartz;
using System;
using System.Collections.Generic;

namespace Prana.ThirdPartyManager.Helpers
{
    public class ThirdPartySendTimeBatchesJob : IJob
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(JobExecutionContext context)
        {
            try
            {
                if (context.JobDetail.Group.Equals(ThirdPartyConstants.THIRD_PARTY_TIME_BATCH_GROUP))
                {
                    bool isSuccessful = false;
                    int batchId = Convert.ToInt32(context.JobDetail.JobDataMap["BatchId"].ToString());
                    DateTime scheduledTime = Convert.ToDateTime(context.JobDetail.JobDataMap["ScheduledTime"].ToString());
                    int timeBatchId = Convert.ToInt32(context.JobDetail.JobDataMap["TimeBatchId"].ToString());

                    ThirdPartyBatch thirdPartyBatch = ThirdPartyLogic.GetThirdPartyBatch(batchId);
                    if (thirdPartyBatch != null && thirdPartyBatch.Active)
                    {
                        isSuccessful = ThirdPartyLogic.SendAutomatedBatch(thirdPartyBatch, scheduledTime.ToString(), timeBatchId);

                        string status = ThirdPartyLogic.GetAutomatedBatchStatus(scheduledTime, false, isSuccessful);
                        PublishMessage(batchId, status);
                    }
                }
                else if (context.JobDetail.Group.Equals(ThirdPartyConstants.THIRD_PARTY_JOB_EXECUTION_NOTIFICATION_GROUP))
                {
                    int batchId = Convert.ToInt32(context.JobDetail.JobDataMap["BatchId"].ToString());
                    DateTime scheduledTime = Convert.ToDateTime(context.JobDetail.JobDataMap["ScheduledTime"].ToString());
                    string status = ThirdPartyLogic.GetAutomatedBatchStatus(scheduledTime, true);
                    PublishMessage(batchId, status);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void PublishMessage(int batchId, string status)
        {
            try
            {
                if(ThirdPartyCache.AutomatedBatchStatus.ContainsKey(batchId))
                    ThirdPartyCache.AutomatedBatchStatus[batchId] = status;

                Dictionary<int, string> dictAutomatedBatchStatus = new Dictionary<int, string> { { batchId, status } };

                MessageData messageData = new MessageData();
                messageData.EventData = new List<object> { JsonHelper.SerializeObject(dictAutomatedBatchStatus), true };
                messageData.TopicName = Topics.Topic_ThirdPartyAutomatedBatchStatus;

                ThirdPartyLogic.Publish(messageData);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }      
    }
}
