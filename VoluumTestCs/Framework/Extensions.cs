using System;
using System.Linq;
using RestSharp;
using Newtonsoft.Json;

namespace VoluumTestCs.Framework
{
    /// <summary>
    /// Different class extensions
    /// </summary>
    public static class Extensions
    {
        public static dynamic GetJsonContent(this RestResponse response)
        {
            return JsonConvert.DeserializeObject(response.Content);
        }

        public static string GetLocation(this RestResponse response)
        {
            return response.Headers.First(h => h.Name == "Location").Value as string;
        }

        public static string GetReplacedQuery(this Uri uri, string replacement)
        {
            return uri.Query.Replace("%7BclickId%7D", replacement).Substring(1);
        }

        public static string GetFullHost(this Uri uri)
        {
            return uri.GetLeftPart(UriPartial.Authority);
        }

        public static string GetCid(this Uri uri)
        {
            return uri.Query.Replace("?subid=", string.Empty);
        }
    }
}
