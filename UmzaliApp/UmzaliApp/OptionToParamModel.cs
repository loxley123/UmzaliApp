using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace UmzaliApp
{
    public class OptionToParamModel
    {
        private Dictionary<string, string[]> _data;

        public OptionToParamModel()
        {
            Populate();
        }

        private void Populate()
        {
            var fileData = File.ReadAllText("data.json");
            _data = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(fileData);

        }

        public string GetParamForIndex(string tableKey, int index)
        {
            if (!_data.TryGetValue(tableKey, out var values))
                throw new NullReferenceException($"Table with key '{tableKey}' does not exist in stored data.");

            return $"@{values[index]}";
        }
    }
}
