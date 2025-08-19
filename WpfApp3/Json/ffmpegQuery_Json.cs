using Newtonsoft.Json;
using System.Collections.Generic;

namespace HaruaConvert.Json
{
    public class CommandHistory
    {
        [JsonProperty("ffQueryToken")]
        public List<string> ffQueryToken { get; set; } = new();

    }
}
