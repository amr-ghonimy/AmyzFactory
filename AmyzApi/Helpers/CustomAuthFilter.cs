using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace AmyzApi.Helpers
{
    public class CustomAuthFilter : AuthorizeAttribute, IAuthenticationFilter
    {

        public bool AllowMultiple
        {
            get { return false; }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string authParameter = string.Empty;
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            string[] tokenWithUserName = null;


            if (authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Authorization Header", request);
                return;
            }
            if (authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid Authorize Schema", request);
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Token", request);
                return;
            }


           string token=authorization.Parameter;


            // this for wibsite
        //    tokenWithUserName = authorization.Parameter.Split(':');
          //  string tokenNumber = tokenWithUserName[0];
            //string userName = tokenWithUserName[1];

     string validUserName = TokenManager.validToken(token);

            if (string.IsNullOrEmpty(validUserName))
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token for User!", request);
                return;
            }

            context.Principal = TokenManager.GetPrincipal(validUserName);

        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result=await context.Result.ExecuteAsync(cancellationToken) ;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", "realm=localhost"));
            }

            context.Result = new ResponseMessageResult(result);

        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public string reasonphrase;
        public HttpRequestMessage request;

        public AuthenticationFailureResult(string responsePhrase, HttpRequestMessage request)
        {
            this.reasonphrase = responsePhrase;
            this.request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(execute());
        }

        public HttpResponseMessage execute()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            responseMessage.RequestMessage = request;
            responseMessage.ReasonPhrase = reasonphrase;
            return responseMessage;
        }
    }
}