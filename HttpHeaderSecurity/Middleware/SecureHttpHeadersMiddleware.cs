using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HttpHeaderSecurity.Middleware
{
    internal sealed class SecureHttpHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IReadOnlyCollection<HttpHeaderPolicy> _httpHeaders;

        public SecureHttpHeadersMiddleware(RequestDelegate next, IReadOnlyCollection<HttpHeaderPolicy> httpHeaders)
        {
            _next = next;
            _httpHeaders = httpHeaders;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            ManageResponseHttpHeaders(httpContext.Response.Headers);

            await _next.Invoke(httpContext);
        }

        private void ManageResponseHttpHeaders(IHeaderDictionary headers)
        {
            foreach (HttpHeaderPolicy httpHeaderPolicy in _httpHeaders)
            {
                if (headers.ContainsKey(httpHeaderPolicy.HeaderName))
                {
                    if (httpHeaderPolicy.PolicyAction == HttpHeaderPolicyAction.Remove)
                    {
                        headers.Remove(httpHeaderPolicy.HeaderName);
                    }
                }
                else
                {
                    if (httpHeaderPolicy.PolicyAction == HttpHeaderPolicyAction.Add)
                    {
                        headers.Add(httpHeaderPolicy.HeaderName, httpHeaderPolicy.HeaderValue);
                    }
                }
            }
        }
    }

    internal enum HttpHeaderPolicyAction
    {
        Add, Remove
    }

    internal struct HttpHeaderPolicy
    {
        public HttpHeaderPolicy(string httpHeaderName, string httpHeaderValue)
        {
            HeaderName = httpHeaderName;
            HeaderValue = httpHeaderValue;
            PolicyAction = HttpHeaderPolicyAction.Add;
        }

        public HttpHeaderPolicy(string httpHeaderName)
        {
            HeaderName = httpHeaderName;
            HeaderValue = null;
            PolicyAction = HttpHeaderPolicyAction.Remove;
        }

        public string HeaderName { get; }
        public string HeaderValue { get; }
        public HttpHeaderPolicyAction PolicyAction { get; }
    }
}
