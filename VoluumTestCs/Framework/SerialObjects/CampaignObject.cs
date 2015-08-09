using System;

namespace VoluumTestCs.Framework.SerialObjects
{
    /// <summary>
    /// Mock object for JSON serializtion
    /// </summary>
    public class CampaignObject
    {
        private string name, redirectUrl;
        private TrafficClassObject tSource;

        public string namePostfix { get { return name; } }
        public string directRedirectUrl { get { return redirectUrl; } }
        public string costModel{ get { return "NOT_TRACKED"; } }
        public string clickRedirectType{ get { return "REGULAR"; } }
        public string redirectTarget { get { return "DIRECT_URL"; } }

        public ClientObject client { get { return new ClientObject(); } }
        public TrafficClassObject trafficSource { get { return TrafficClassObject.ZeroPark; } }
        public CountryObject country { get { return CountryObject.Poland; } }

        public bool costModelHidden { get { return true; } }

        public CampaignObject(string name, string redirectUrl, TrafficClassObject trafficSource)
        {
            this.name = name;
            this.redirectUrl = redirectUrl;
            this.tSource = trafficSource;
        }
    }
}
