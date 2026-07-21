using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace DVLD.Infrastructure.HTTPClient
{
    public class AuthTokenForwardingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthTokenForwardingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}