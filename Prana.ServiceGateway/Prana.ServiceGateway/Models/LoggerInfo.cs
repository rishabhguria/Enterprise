using System.ComponentModel.DataAnnotations;
using Prana.ServiceGateway.Constants;

namespace Prana.ServiceGateway.Models
{
    public class LoggerInfo
    {
        [Required(ErrorMessage = MessageConstants.MSG_CONST_LOG_LEVEL_REQUIRED)]
        public int? level { get; set; }

        [Required(ErrorMessage = MessageConstants.MSG_CONST_LOG_MESSAGE_REQUIRED)]
        public string message { get; set; }

        public string userId { get; set; }

        public string sourceContext { get; set; }
        public string exception { get; set; }
    }
}
