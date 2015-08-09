using System;

using RestSharp;

namespace VoluumTestCs.Framework
{
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

        protected RestResponse Execute(string resource, Method method, Action<RestRequest> action)
        {
            RestRequest request = new RestRequest(resource, method);

            if (action != null)
                action(request);

            return client.Execute(request) as RestResponse;
        }

        protected RestResponse Execute(string resource, Method method)
        {
            return Execute(resource, method, null);
        }

        protected RestResponse ExecuteSecurely(string resource, Method method, Action<RestRequest> action)
        {
            return Execute(resource, method, request =>
            {
                request.AddHeader("cwauth-token", securityToken);

                if (action != null)
                    action(request);
            });
        }

        protected RestResponse ExecuteSecurely(string resource, Method method)
        {
            return ExecuteSecurely(resource, method, null);
        }

        public RestResponse Get(string resource, bool followRedirects)
        {
            client = new RestClient(resource);
            client.FollowRedirects = followRedirects;

            return Execute(string.Empty, Method.GET);
        }
    }
}
