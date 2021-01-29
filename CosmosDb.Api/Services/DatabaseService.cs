using CosmosDb.Api.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using CosmosDb.Api.Data.Configuration.Store;
using System.Net;

namespace CosmosDb.Api.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly CosmosStore _store;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(CosmosStore store, ILogger<DatabaseService> logger)
        {
            _store = store;
            _logger = logger;
        }

        public async Task<Database> GetDatabase()
        {
            Database database = null;

            try
            {
                database = await _store.ClientStore.CreateDatabaseIfNotExistsAsync(_store.DatabaseName);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError(@$"Error: Getting database: {_store.DatabaseName}\n\r {ex.ToString()}");
            }

            return database;
        }

        public async Task<Container> GetContainer(string containerId, string partitionKey)
        {
            Container container = null;

            try
            {
                container = await _store.DatabaseCosmos.CreateContainerIfNotExistsAsync(containerId, $"/{partitionKey}");
            }
            catch(CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError($@"Error: Getting container: {containerId} \n\r {ex.ToString()}");
            }

            return container;

        }
    }
}
