using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Prana.KafkaWrapper
{
    public class KafkaSerializer : ISerializer<RequestResponseModel>, IDeserializer<RequestResponseModel>
    {
        public RequestResponseModel Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            try
            {
                return JsonConvert.DeserializeObject<RequestResponseModel>(Encoding.UTF8.GetString(data.ToArray()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public byte[] Serialize(RequestResponseModel data, SerializationContext context)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(data);
                return Encoding.UTF8.GetBytes(jsonString);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
