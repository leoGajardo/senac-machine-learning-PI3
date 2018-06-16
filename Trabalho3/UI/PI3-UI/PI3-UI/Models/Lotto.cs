using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI3_UI.Models
{
    public class Lotto
    {
        [JsonProperty("numbers")]
        public int[] nums { get; set; }
    }
}
