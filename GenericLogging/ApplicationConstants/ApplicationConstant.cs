using System;
using Prana.Global;

namespace GenericLogging.ApplicationConstants
{
    public class ApplicationConstant
    {
        public static readonly string StartupPath = System.Windows.Forms.Application.StartupPath;
        public static readonly string FolderName = Convert.ToString(ConfigurationHelper.Instance.GetAppSettingValueByKey("FolderName"));
        public static readonly string TextFileName = Convert.ToString(ConfigurationHelper.Instance.GetAppSettingValueByKey("TextFileName"));
        public static readonly int NoOfDays = Convert.ToInt16(ConfigurationHelper.Instance.GetAppSettingValueByKey("NoOfDays")) * -1;
        public static readonly int QueueReadTimer = Convert.ToInt16(ConfigurationHelper.Instance.GetAppSettingValueByKey("QueueReadTimer")) * 1000;
        public static readonly string QueuePath = ConfigurationHelper.Instance.GetAppSettingValueByKey("QueuePath");
        public static readonly string QueueName = Convert.ToString(ConfigurationHelper.Instance.GetAppSettingValueByKey("QueueName"));

        public static readonly string MessageInQueue = "1 Message Processed in the Queue. Remaining no. of Messages in Queue = ";
        public static readonly string MessageConsumed = "All Messages contained in the Queue are Processed. Waiting for the next message";
        public static readonly string ToContinue = "Press Enter to continue";
    }
}
