using System;
using RestSharp;
using Newtonsoft.Json;

namespace VoluumTestCs.Framework
{
    public static class Extensions
    {
        public static dynamic GetJsonContent(this RestResponse response)
        {
            return JsonConvert.DeserializeObject(response.Content);
        }

        public static string GetReplacedQuery(this Uri uri, string replacement)
        {
            return uri.Query.Replace("%7BclickId%7D", replacement);
        }

        public static string GetFullHost(this Uri uri)
        {
            return uri.OriginalString.Replace(uri.AbsolutePath, string.Empty);
        }

        public static string GetCid(this Uri uri)
        {
            return uri.Query.Replace("?subid=", string.Empty);
        }
    }
}
