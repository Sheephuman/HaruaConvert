using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace HaruaConvert.Json
{
    public class QuerySaver
    {

        public void SaveToJsonFile<T>(T data, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }


        public Dictionary<string, QueryCheckRules> ReadtoRulesQuery(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<
                Dictionary<string, QueryCheckRules>>(jsonData)
                ?? new Dictionary<string, QueryCheckRules>();
        }


        public void ReadtoRulesQuery(string filePath, out Dictionary<string, QueryCheckRules> rules)
        {
            rules = new Dictionary<string, QueryCheckRules>();

          

            var jsonData = File.ReadAllText(filePath);

            Debug.WriteLine($"Read JSON: {jsonData}");
            Debug.WriteLine($"Path: {filePath}");

            var ruleConfigs =
                JsonConvert.DeserializeObject<Dictionary<string, QueryCheckRules>>(jsonData);

            foreach (var rule in ruleConfigs.Keys)
            {
                rules.Add(rule, ruleConfigs[rule]);
            }
        }

        public List<string> ReadtoCommandHistory<T>(string filePath)
        {
            List<string> tokens = new();

            string args = string.Empty;
            var jsonData = File.ReadAllText(filePath);
            Debug.WriteLine($"Read JSON: {jsonData}");
            Debug.WriteLine($"Path: {filePath}");

            var qHistory = JsonConvert.DeserializeObject<CommandHistory>(jsonData);

            foreach (var to in qHistory.ffQueryToken)
            {
                tokens.Add(to);
            }
            return tokens;
        }

    }
}

