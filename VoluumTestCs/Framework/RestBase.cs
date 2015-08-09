using System;

using RestSharp;

namespace VoluumTestCs.Framework
{
    /// <summary>
    /// Abstract class to handle Rest communication
    /// </summary>
    public abstract class RestBase
    {
        protected string securityToken;
        protected RestClient client;

        public RestBase()
        {
            client = new RestClient();
        }

        public RestBase(string token) : this()
        {
            securityToken = token;
        }
        
        /// <summary>
        /// Sends REST request and returns response
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="method"></param>
        /// <param name="action">Additional action on REST request</param>
        /// <returns></returns>
        protected RestResponse Execute(string resource, Method method, Action<RestRequest> action)
        {
            RestRequest request = new RestRequest(resource, method);

            if (action != null)
                action(request);

            return client.Execute(request) as RestResponse;
        }

        /// <summary>
        /// Sends REST request and returns response
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        protected RestResponse Execute(string resource, Method method)
        {
            return Execute(resource, method, null);
        }

        /// <summary>
        /// Sends REST request and returns response within secure context
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="method"></param>
        /// <param name="action">Additional action on REST request</param>
        /// <returns></returns>
        protected RestResponse ExecuteSecurely(string resource, Method method, Action<RestRequest> action)
        {
            return Execute(resource, method, request =>
            {
                request.AddHeader("cwauth-token", securityToken);

                if (action != null)
                    action(request);
            });
        }

        /// <summary>
        /// Sends REST request and returns response within secure context
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        protected RestResponse ExecuteSecurely(string resource, Method method)
        {
            return ExecuteSecurely(resource, method, null);
        }


        /// <summary>
        /// Sends simple GET request and returns response
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="followRedirects">Indicates if request should follow redirection</param>
        /// <returns></returns>
        public RestResponse Get(string resource, bool followRedirects)
        {
            client = new RestClient(resource);
            client.FollowRedirects = followRedirects;

            return Execute(string.Empty, Method.GET);
        }
    }
}
