using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;

using System;
using System.Threading.Tasks;

namespace Finbuckle.MultiTenant.AzureFunctions
{
    /// <summary>
    /// Creates a <see cref="TenantInfo"/> instance for the supplied header and configuration values.
    /// </summary>
    public class TenantValueProvider : IValueProvider
    {
        private readonly HttpRequest _request;

        public TenantValueProvider(HttpRequest request)
        {
            _request = request;
        }

        public Type Type { get; }

        public Task<object> GetValueAsync()
        {
            return Task.FromResult((object)_request.HttpContext.GetMultiTenantContext<TenantInfo>()?.TenantInfo);
        }

        public string ToInvokeString()
        {
            throw new NotImplementedException();
        }
    }
}