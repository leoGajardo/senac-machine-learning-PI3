using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI3_UI.Models
{
    [JsonObject()]
    public class Fortune
    {
        [JsonProperty("message")]
        public string message { get; set; }
    }
}
