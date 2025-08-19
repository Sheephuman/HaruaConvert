using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace HaruaConvert.Json
{
    public class CommandHistoryIO
    {

        public void SaveToJsonFile<T>(T data, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }


        public List<string> ReadtoJsonFile<T>(string filePath)
        {
            List<string> tokens = new();

            string args = string.Empty;
            var jsonData = File.ReadAllText(filePath);

            var qHistory = JsonConvert.DeserializeObject<CommandHistory>(jsonData);

            foreach (var to in qHistory.ffQueryToken)
            {
                {
                    //// エスケープされたクォートを除去
                    //var sanitizedOption = JsonConvert.DeserializeObject<string>($"\"{opt}\"");

                    tokens.Add(to);
                }

            }
            return tokens;
        }

    }
}

