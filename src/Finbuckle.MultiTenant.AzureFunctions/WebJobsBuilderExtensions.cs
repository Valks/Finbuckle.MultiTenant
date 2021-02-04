using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;

using System;
using System.Collections.Generic;
using System.Text;

namespace Finbuckle.MultiTenant.AzureFunctions
{
    public static class WebJobsBuilderExtensions
    {
        public static IWebJobsBuilder AddTenantBindings(this IWebJobsBuilder builder)
        {
            if(builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<TenantExtensionProvider>();
            return builder;
        }
    }
}
