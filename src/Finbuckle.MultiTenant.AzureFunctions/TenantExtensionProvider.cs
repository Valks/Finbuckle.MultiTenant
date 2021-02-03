using Microsoft.Azure.WebJobs.Host.Config;

using System;
using System.Collections.Generic;
using System.Text;

namespace Finbuckle.MultiTenant.AzureFunctions
{
    public class TenantExtensionProvider<TTenantInfo> : IExtensionConfigProvider
        where TTenantInfo : class, ITenantInfo, new()
    {
        public void Initialize(ExtensionConfigContext context)
        {
            // Creates a rule that links the attribute to the binding
            var provider = new TenantBindingProvider<TTenantInfo>();
            var rule = context.AddBindingRule<TenantAttribute>().Bind(provider);
        }
    }
}
