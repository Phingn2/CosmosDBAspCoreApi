using CosmosDb.Api.Interface;
using CosmosDb.Api.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using CosmosDb.Api.Data.Configuration.Store;
using CosmosDb.Api.Helper;
using Cosmonaut.Extensions;

namespace CosmosDb.Api.Services
{
    public class FamilyServiceCosmonaut : IFamilyServiceCosmonault
    {
        private readonly ILogger<FamilyServiceCosmonaut> _logger;

        private readonly CosmosStoreSettings _cosmonaultStore;

        private readonly ICosmosStore<Family> store;

        public FamilyServiceCosmonaut(CosmosStoreSettings cosmonaultStore, ILogger<FamilyServiceCosmonaut> logger)
        {
            _cosmonaultStore = cosmonaultStore;
            _logger = logger;

            store = new CosmosStore<Family>(_cosmonaultStore);
        }
        public async Task<Family> AddFamily(Family family)
        {
            try
            {
                var id = Guid.NewGuid().ToString("D");

                family.Id = id;
                family.PartitionKey = PartitionKey.Generate("R", id, 1000);

                return await store.AddAsync(family);
            }
            catch(Exception ex)
            {
                _logger.LogError(@$"Adding item, LastName:{family.LastName} failed \n\r{ex.ToString()}");
            }

            return family;
        }

        public async Task DeleteFamily(string id, string partitionKey)
        {
            await store.RemoveByIdAsync(id, partitionKey);
        }

        public async Task<List<Family>> GetAllFamily()
        {
            return await store.Query().ToListAsync();
        }

        public async Task<List<Family>> GetFamily(string lastName)
        {
            return await store.Query().Where(f => f.LastName == lastName).ToListAsync();
        }

        public async Task UpdateFamily(string id, string lastName)
        {
            var family = await GetSingleFamily(id);

            if (family != null)
            {
                family.LastName = lastName;
                await store.UpdateAsync(family);
            }
        }

        private async Task<Family> GetSingleFamily(string id)
        {
            return await store.Query().FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
