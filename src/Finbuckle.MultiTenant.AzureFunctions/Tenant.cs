using Microsoft.Azure.WebJobs.Host.Bindings;

using System;

namespace Finbuckle.MultiTenant.AzureFunctions
{
    public class Tenant<T> : OpenType.Poco
    {
        public T Value { get; set; }
    }
}