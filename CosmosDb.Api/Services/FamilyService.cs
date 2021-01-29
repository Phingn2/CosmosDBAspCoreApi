using CosmosDb.Api.Interface;
using CosmosDb.Api.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;
using CosmosDb.Api.Data.Configuration.Store;

namespace CosmosDb.Api.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<FamilyService> _logger;
        private readonly CosmosStore _store;

        const string containerId = "Family";
        const string partitionKey = "/LastName";

        private Container container;
        private Database database;

        public FamilyService(IDatabaseService databaseService, CosmosStore store, ILogger<FamilyService> logger)
        {
            _databaseService = databaseService;
            _store = store;
            _logger = logger;

            StoreInitialize();
        }

        private async void StoreInitialize()
        {
            database = await _databaseService.GetDatabase();

            var client = new CosmosClient(@"https://cosmos-db-sample.documents.azure.com:443/", @"tTL4FwbiqNVeZAgUYW3U3zCv5pe9CUHPO8zVUP48dbV8joW5UM39FeHoeO9jfTq0e67bD7qlCEzumpqA00SaTw==");
            database = await client.CreateDatabaseIfNotExistsAsync("FamilyTree");

            container = await database.CreateContainerIfNotExistsAsync(containerId, partitionKey);
        }

        public async Task<ItemResponse<Family>> AddFamily(Family family)
        {
            ItemResponse<Family> familyItemRes = null;
            try
            {
                //familyItemRes = await this.container.ReadItemAsync<Family>(family.Id, new PartitionKey(family.LastName));
                familyItemRes = await this.container.CreateItemAsync<Family>(family, new PartitionKey(family.LastName));

                _logger.LogInformation($"Request charged: {familyItemRes.RequestCharge.ToString()}");

            }
            catch(CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError(@$"Adding new {containerId}, LastName:{family.LastName} failed \n\r{ex.ToString()}");
            }

            return familyItemRes;
        }

        public Task DeleteFamily(string familyId, string partitionKey)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Family>> GetFamily(string lastName)
        {
            List<Family> families = new List<Family>();
            var cosSql = $"SELECT * FROM f WHERE f.LastName = '{lastName}'";
            try
            {
                QueryDefinition queryDefinition = new QueryDefinition(cosSql);
                FeedIterator<Family> feedIterator = this.container.GetItemQueryIterator<Family>(queryDefinition);

                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<Family> feedResponse = await feedIterator.ReadNextAsync();
                    foreach(Family family in feedResponse)
                    {
                        families.Add(family);
                    }
                }
            }
            catch (CosmosException ex)
            {
                _logger.LogError(@$"Querying {containerId}, LastName:{lastName} failed \n\r{ex.ToString()}");
            }
            throw new NotImplementedException();
        }

        public Task UpdateFamily(string Id, string familyLastname)
        {
            throw new NotImplementedException();
        }
    }
}
