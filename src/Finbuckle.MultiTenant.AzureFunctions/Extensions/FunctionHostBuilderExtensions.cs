
using Finbuckle.MultiTenant.AspNetCore;
using Finbuckle.MultiTenant.AzureFunctions.Host.Extensions;

namespace Microsoft.Azure.Functions.Extensions.DependencyInjection
{
    public static class FunctionHostBuilderExtensions
    {
        public static IFunctionsHostBuilder UseMultiTenant(this IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpMiddleware(nameof(MultiTenantMiddleware),
                async (context, next) =>
                {
                    try
                    {
                        var multiTenantMiddleware = new MultiTenantMiddleware(next);
                        await multiTenantMiddleware.Invoke(context);
                    }
                    catch { }

                    await next(context);
                });
            return builder;
        }
    }
}
