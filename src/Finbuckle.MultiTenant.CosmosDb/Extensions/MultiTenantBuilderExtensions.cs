using System;
using System.Collections.Generic;
using System.Text;

using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.CosmosDb.Stores;

using Microsoft.Azure.Cosmos;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FinbuckleMultiTenantBuilderExtensions
    {
        /// <summary>
        /// Adds a CosmosDb based multi-tenant store to the application. Will also add the database context service unless it's already added.
        /// </summary>
        /// <typeparam name="CosmosDbStoreContext"></typeparam>
        /// <typeparam name="TTenantInfo"></typeparam>
        /// <param name="builder"></param>
        /// <param name="client"></param>
        /// <param name="collectionName"></param>
        /// <param name="databaseName"></param>
        /// <returns>The same <see cref="FinbuckleMultiTenantBuilder{TTenantInfo}"/> passed into the method.</returns>
        public static FinbuckleMultiTenantBuilder<TTenantInfo> WithCosmosDbStore<TTenantInfo>(this FinbuckleMultiTenantBuilder<TTenantInfo> builder,
            CosmosClient client,
            string databaseName,
            string collectionName)
            where TTenantInfo : class, ITenantInfo, new()
        {
            builder.Services.AddSingleton<CosmosDbStoreContext>(services =>
            {
                return new CosmosDbStoreContext(client.GetContainer(databaseName, collectionName));
            });
            return builder.WithStore<CosmosDbStore<TTenantInfo>>(ServiceLifetime.Scoped);
        }
    }
}
