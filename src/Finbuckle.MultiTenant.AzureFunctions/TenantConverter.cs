using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;

namespace Finbuckle.MultiTenant.AzureFunctions
{
    internal class TenantConverter<TTenantInfo> : IConverter<HttpRequest, Tenant<TTenantInfo>>
        where TTenantInfo : class, ITenantInfo, new()
    {
        public Tenant<TTenantInfo> Convert(HttpRequest request)
        {
            return new Tenant<TTenantInfo>
            {
                Value = request.HttpContext.GetMultiTenantContext<TTenantInfo>()?.TenantInfo
            };
        }
    }
}
