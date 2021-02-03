using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Finbuckle.MultiTenant.AzureFunctions
{
    /// <summary>
    /// Provides a new binding instance for the function host.
    /// </summary>
    public class TenantBindingProvider<TTenantInfo> : IBindingProvider
        where TTenantInfo : class, ITenantInfo, new()
    {
        internal const string RequestBindingName = "$request";

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if(context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var parameter = context.Parameter;
            if(parameter.ParameterType == typeof(HttpRequest))
            {
                // Not already claimed by another trigger?
                if (!HasBindingAttributes(parameter))
                {
                    return Task.FromResult<IBinding>(new TenantBinding<TTenantInfo>());
                }
            }

            return Task.FromResult<IBinding>(null);
        }

        private static bool HasBindingAttributes(ParameterInfo parameter)
        {
            foreach (Attribute attr in parameter.GetCustomAttributes(false))
            {
                if (IsBindingAttribute(attr))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsBindingAttribute(Attribute attribute)
        {
            return attribute.GetType().GetCustomAttribute<TenantAttribute>() != null;
        }
    }
}
