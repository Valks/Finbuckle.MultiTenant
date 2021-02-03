using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;

using System;
using System.Collections.Generic;
using System.Text;

namespace Finbuckle.MultiTenant.AzureFunctions
{
    public static class FunctionsHostBuilderExtensions
    {
        public static IWebJobsBuilder AddTenantBindings<TTenantInfo>(this IWebJobsBuilder builder)
            where TTenantInfo : class, ITenantInfo, new()
        {
            if(builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<TenantExtensionProvider<TTenantInfo>>();
            return builder;
        }
    }
}
