using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.CQRS.EventBus
{
    public class HostEvent
    {
        [JsonProperty("host")]
        public string Host { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("usr")]
        public string UserName { get; set; }
        [JsonProperty("pwd")]
        public string Password { get; set; }
    }
}
