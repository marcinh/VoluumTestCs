using System;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using VoluumTestCs.Framework.Exceptions;
using VoluumTestCs.Framework.SerialObjects;
using VoluumTestCs.Properties;

namespace VoluumTestCs.Framework
{
    /// <summary>
    /// Class containing logical functions build with basic rest functions from parent class
    /// </summary>
    public class VoluumApp : RestBase, IDisposable
    {
        private string securityUrl;
        private string coreUrl;

        public VoluumApp()
            : base()
        {
            securityUrl = Settings.Default.SecurityUrl;
            coreUrl = Settings.Default.CoreUrl;
        }

        /// <summary>
        /// Login method using REST api
        /// </summary>
        /// <param name="user">Username to log in</param>
        /// <param name="password">User password to log in</param>
        public void Login(string user, string password)
        {
            client.BaseUrl = new Uri(securityUrl);
            client.Authenticator = new HttpBasicAuthenticator(user, password);

            var response = Execute("/login", Method.GET);

            bool loggedIn = response.GetJsonContent().loggedIn;

            if (!loggedIn) throw new UserNotLoggedInException();

            securityToken = response.GetJsonContent().token;
        }

        /// <summary>
        /// Login method using REST api with credentials from project settings
        /// </summary>
        public void Login()
        {
            Login(Settings.Default.User, Settings.Default.Password);
        }

        /// <summary>
        /// Logout method using REST api
        /// </summary>
        public void Logout()
        {
            client = new RestClient(securityUrl);

            var response = ExecuteSecurely("/session/logout", Method.GET);
        }

        /// <summary>
        /// Method used to create new campaign based on argument passed
        /// </summary>
        /// <param name="campaignObject">Object describing campaign details</param>
        /// <returns></returns>
        public RestResponse CreateCampaign(CampaignObject campaignObject)
        {
            client = new RestClient(coreUrl);

            return ExecuteSecurely("/campaigns", Method.POST, request =>
            {
                request.AddParameter("application/json", JsonConvert.SerializeObject(campaignObject), ParameterType.RequestBody);
            });
        }

        /// <summary>
        /// Method getting campaign statistics from reports page based on campaign id provided
        /// </summary>
        /// <param name="id">Unique campaign id</param>
        /// <returns></returns>
        public CampaignReportObject GetCampaignStatistics(string id)
        {
            client = new RestClient(Settings.Default.ReportsUrl);

            var result = ExecuteSecurely("/report?from=2015-08-09T00%3A00%3A00Z&to=2015-08-10T00%3A00%3A00Z&groupBy=lander&include=active&filter1=campaign&filter1Value={id}", Method.GET, request =>
                request.AddUrlSegment("id", id));

            JArray array = result.GetJsonContent().rows;

            return array.First.ToObject<CampaignReportObject>();
        }

        /// <summary>
        /// Method getting campaign details based on campaign id provided
        /// </summary>
        /// <param name="id">Unique campaign id</param>
        /// <returns></returns>
        public RestResponse GetCampaighDetails(string id)
        {
             client = new RestClient(coreUrl);

             return ExecuteSecurely("/campaigns/{id}", Method.GET, request =>
                request.AddUrlSegment("id", id));            
        }
        

        public void Dispose()
        {
            Logout();

            client = null;
        }

        public static VoluumApp LoggedIn
        {
            get
            {
                var vApp = new VoluumApp();
                vApp.Login();

                return vApp;
            }
        }
    }
}
