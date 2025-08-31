using Newtonsoft.Json;
using System.Collections.Generic;

namespace HaruaConvert.Json
{
    public class CommandHistory
    {
        [JsonProperty(nameof(ffQueryToken))]
        public List<string> ffQueryToken { get; set; } = new();

    }
}
