using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDb.Api.Models;
using Microsoft.Azure.Cosmos;

namespace CosmosDb.Api.Interface
{
    public interface IFamilyService
    {
        Task<ItemResponse<Family>> AddFamily(Family family);
        Task UpdateFamily(string Id, string familyLastname);
        Task DeleteFamily(string familyId, string partitionKey);
        Task<List<Family>> GetFamily(string lastName);
    }
}
