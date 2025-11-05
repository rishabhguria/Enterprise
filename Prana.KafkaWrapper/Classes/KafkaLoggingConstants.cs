using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.KafkaWrapper
{
    public class KafkaLoggingConstants
    {
        public const string MSG_KAFKA_PRODUCE = "Producing {0} to Kafka";
        public const string MSG_KAFKA_CONSUMER = " message recieved from Kafka";
        public const string CONST_DATET_TIME_FORMAT = "MM/dd/yyyy hh:mm:ss.fff tt";
        public const string CONST_PROBLEM_CACHE_CREATION = "Problem in Cache Creation.";
        public const string CONST_ARROW = "->";
    }
}
