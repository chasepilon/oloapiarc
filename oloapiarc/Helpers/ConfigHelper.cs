using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace oloapiarc.Helpers
{
    public static class ConfigHelper
    {
        private static object _lock = new object();
        private static string _baseUrlPrefix = "BaseUrl.";
        
        public static Dictionary<string, string> BaseUrls = ConfigureUrls(_baseUrlPrefix, _baseUrls);
        private static Dictionary<string, string> _baseUrls;

        private static Dictionary<string, string> ConfigureUrls(string urlType, Dictionary<string, string> lookup)
        {
            var serviceKeys = ConfigurationManager.AppSettings.AllKeys.Where(k => k.StartsWith(urlType));
            if (serviceKeys.Any())
            {
                if (lookup == null)
                {
                    lock (_lock)
                    {
                        if (lookup == null)
                        {
                            lookup = new Dictionary<string, string>();
                            foreach (var key in serviceKeys)
                            {
                                var url = ConfigurationManager.AppSettings[key];
                                var urlKey = key.Replace(urlType, string.Empty);
                                if (!lookup.ContainsKey(urlKey))
                                {
                                    lookup.Add(urlKey, url);
                                }
                            }
                        }
                    }
                }
            }

            return lookup;
        }
    }
}
