using Finbuckle.MultiTenant.AzureFunctions;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

using System;
using System.Collections.Generic;
using System.Text;

[assembly: WebJobsStartup(typeof(TenantWebJobsStartup))]
namespace Finbuckle.MultiTenant.AzureFunctions
{
    public class TenantWebJobsStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddTenantBindings();
        }
    }
}
