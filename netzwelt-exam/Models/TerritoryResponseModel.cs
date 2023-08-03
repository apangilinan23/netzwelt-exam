using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace netzwelt_exam.Models
{
    [Serializable]
    public class TerritoryResponseModel
    {
        [JsonProperty("data")]
        public List<TerritoryData> Territories { get; set; }        
    }

    [Serializable]
    public class TerritoryData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent")]
        public int? Parent { get; set; }
    }
}
