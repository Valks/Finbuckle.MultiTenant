using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;

using System;
using System.Threading.Tasks;

namespace Finbuckle.MultiTenant.AzureFunctions
{
    /// <summary>
    /// Runs on every request and passes the function context (e.g. Http request and host configuration) to a value provider.
    /// </summary>
    public class TenantBinding<TTenantInfo> : IBinding
        where TTenantInfo : class, ITenantInfo, new()
    {
        public bool FromAttribute { get { return false; } }

        /// <summary>
        /// Runs on every request and passes the function context (e.g. Http request and host configuration) to a value provider.
        /// </summary>
        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var request = context.BindingData[TenantBindingProvider<TTenantInfo>.RequestBindingName];

            return BindAsync(request, context.ValueContext);
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            var request = value as HttpRequest;
            if(request != null)
            {
                var binding = new TenantValueProvider<TTenantInfo>(request);
                return Task.FromResult<IValueProvider>(binding);
            }
            throw new InvalidOperationException($"value must be an {nameof(HttpRequest)}");
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor()
            {
                Name = "tenant",
            };
        }
    }
}