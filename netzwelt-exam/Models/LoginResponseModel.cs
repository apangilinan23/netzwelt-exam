using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace netzwelt_exam.Models
{
    [Serializable]
    public class LoginResponseModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
    }
}
