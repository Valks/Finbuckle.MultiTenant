using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;

using System;
using System.Threading.Tasks;

namespace Finbuckle.MultiTenant.AzureFunctions
{
    /// <summary>
    /// Creates a <see cref="TenantInfo"/> instance for the supplied header and configuration values.
    /// </summary>
    public class TenantValueProvider<TTenantInfo> : IValueProvider
        where TTenantInfo : class, ITenantInfo, new()
    {
        private readonly HttpRequest _request;

        public Type Type { get; }

        public TenantValueProvider(HttpRequest request)
        {
            _request = request;
        }

        public Task<object> GetValueAsync()
        {
            return Task.FromResult((object)_request.HttpContext.GetMultiTenantContext<TTenantInfo>()?.TenantInfo);
        }

        public string ToInvokeString()
        {
            throw new NotImplementedException();
        }
    }
}