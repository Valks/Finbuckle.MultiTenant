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
            string databaseName,
            string collectionName,
            ThroughputProperties throughput = null)
            where TTenantInfo : class, ITenantInfo, new()
        {
            if(throughput is null)
            {
                throughput = ThroughputProperties.CreateManualThroughput(400);
            }

            builder.Services.AddSingleton<CosmosDbStoreContext>(services =>
            {
                var client = services.GetRequiredService<CosmosClient>();
                // Create the database and container if they don't exist.
                var status = client.CreateDatabaseIfNotExistsAsync(databaseName, throughput)
                                   .ConfigureAwait(true).GetAwaiter().GetResult().StatusCode;
                Console.WriteLine($"Database Status: {status}");
                if(status != System.Net.HttpStatusCode.OK && status != System.Net.HttpStatusCode.Created)
                    throw new ApplicationException($"Failed to Open or Create Database. {databaseName}");
                var database = client.GetDatabase(databaseName);
                status = database.CreateContainerIfNotExistsAsync(new ContainerProperties(collectionName, $"/{nameof(ITenantInfo.Id).ToLower()}"), throughput)
                                 .ConfigureAwait(false).GetAwaiter().GetResult().StatusCode;
                Console.WriteLine($"Container Status: {status}");
                if (status != System.Net.HttpStatusCode.OK && status != System.Net.HttpStatusCode.Created)
                    throw new ApplicationException($"Failed to Open or Create Container. {collectionName}");

                return new CosmosDbStoreContext(client.GetContainer(databaseName, collectionName));
            });
            return builder.WithStore<CosmosDbStore<TTenantInfo>>(ServiceLifetime.Scoped);
        }
    }
}