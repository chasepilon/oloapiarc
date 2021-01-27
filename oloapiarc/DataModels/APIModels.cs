using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace oloapiarc.DataModels
{
    public class BaseAPIModel
    {
        public string baseUrl { get; set; }
        public string endpoint { get; set; }
        public JObject body { get; set; }
        

        public Dictionary<string, string> Authentication = new Dictionary<string, string>(); 

        public string GetFullUrl() { return baseUrl + endpoint; }

        public readonly string[] Contract =
        {
            "userId",
            "id",
            "title",
            "body"
        };
    }

    public enum RequestType
    {
        GET,
        POST,
        PUT,
        PATCH,
        DELETE,
    }  
}
