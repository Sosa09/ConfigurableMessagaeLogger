using System;
using Newtonsoft.Json;

namespace ConfigurableMessagaeLogger
{
    public class LogMessage
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("type")]
        public LogTypes Type { get; set; }
        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
        [JsonProperty("application")]
        public string Application{ get; set; }
    }
}
