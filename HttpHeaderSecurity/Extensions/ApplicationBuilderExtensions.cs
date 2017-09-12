using HttpHeaderSecurity.Middleware;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace HttpHeaderSecurity.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder UseCustomHttpHeaders(this IApplicationBuilder builder, IReadOnlyCollection<HttpHeaderPolicy> httpHeaders)
        {
            return builder.UseMiddleware<SecureHttpHeadersMiddleware>(httpHeaders);
        }
    }
}
