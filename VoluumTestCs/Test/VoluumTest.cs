using System;
using System.Linq;
using System.Net;
using NUnit.Framework;

using VoluumTestCs.Framework;
using VoluumTestCs.Framework.SerialObjects;

namespace VoluumTestCs.Test
{
    [TestFixture]
    public class VoluumTest
    {
        //Example campaigh ID for test purposes
        const string CAMPAIGNID = "806f4cf7-3168-4866-a212-837ecce37fef";
        const string CAPMPAIGHNAME = "TEST001";

        private Uri redirectUri;

        [TestFixtureSetUp]
        public void Setup()
        {
            redirectUri = new Uri("http://example.com?subid={clickId}");
        }

        [Test]
        public void Test_01()
        {
            using (var vApp = VoluumApp.LoggedIn)
            {
                var result = vApp.CreateCampaign(new CampaignObject(CAPMPAIGHNAME, redirectUri.OriginalString, TrafficClassObject.ZeroPark));

                Assert.AreEqual(result.StatusCode, HttpStatusCode.Created, "Campaign was created");

                string linkResponse = result.GetJsonContent().url;

                result = vApp.Get(linkResponse,false);

                Assert.AreEqual(HttpStatusCode.Redirect, result.StatusCode, "Campaign link redirects to new page");

                var currentUri = new Uri(result.Headers.First(h => h.Name == "Location").Value as string);
                string expSubId = redirectUri.GetReplacedQuery("[A-Za-z0-9]{24}");

                StringAssert.StartsWith(redirectUri.Host, currentUri.OriginalString, "Campaign link redirects to new page");
                StringAssert.IsMatch(expSubId, currentUri.Query, "Offer URL has resolved subid parameter’s value");
            }
        }

        [Test]
        public void Test_02()
        {
            using (var vApp = VoluumApp.LoggedIn)
            {
                var visitsNumber = vApp.GetCampaignStatistics(CAMPAIGNID).visits;

                var response = vApp.GetCampaighDetails(CAMPAIGNID);

                string url = response.GetJsonContent().url;

                vApp.Get(url, true);

                bool visitIncremented = false;
                
                for (int i = 0; i < 10; i++)
                {
                    System.Threading.Thread.Sleep(1000);

                    if (vApp.GetCampaignStatistics(CAMPAIGNID).visits == visitsNumber + 1)
                    {
                        visitIncremented = true;
                        break;
                    }                    
                }

                Assert.IsTrue(visitIncremented, "Number of visits was incrementd within 10 seconds");
            }
        }

        [Test]
        public void Test_03()
        {
            using (var vApp = VoluumApp.LoggedIn)
            {
                var conversionsNumber = vApp.GetCampaignStatistics(CAMPAIGNID).conversions;

                var response = vApp.GetCampaighDetails(CAMPAIGNID);

                var uri = new Uri((string)response.GetJsonContent().url);

                response = vApp.Get(uri.OriginalString, true);

                string cId = response.ResponseUri.GetCid();

                vApp.Get(string.Format("{0}/postback?cid={1}", uri.GetFullHost(), cId), true);

                bool conversionsIncrementd = false;

                for (int i = 0; i < 30; i++)
                {
                    System.Threading.Thread.Sleep(1000);

                    if (vApp.GetCampaignStatistics(CAMPAIGNID).conversions == conversionsNumber + 1)
                    {
                        conversionsIncrementd = true;
                        break;
                    }
                }

                Assert.IsTrue(conversionsIncrementd, "Number of conversions was incrementd within 30 seconds");
            }
        }
    }
}
