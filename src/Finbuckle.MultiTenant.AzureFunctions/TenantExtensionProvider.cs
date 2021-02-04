using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;

using System;
using System.Collections.Generic;
using System.Text;

namespace Finbuckle.MultiTenant.AzureFunctions
{
    [Extension(nameof(TenantExtensionProvider))]
    public class TenantExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddOpenConverter<HttpRequest, Tenant<OpenType>>(typeof(TenantConverter<>));

            // Creates a rule that links the attribute to the binding
            var provider = new TenantBindingProvider();
            var rule = context.AddBindingRule<TenantAttribute>().Bind(provider);

        }
    }
}