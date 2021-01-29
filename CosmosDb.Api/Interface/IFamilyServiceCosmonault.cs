using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDb.Api.Models;
using Microsoft.Azure.Cosmos;

namespace CosmosDb.Api.Interface
{
    public interface IFamilyServiceCosmonault
    {
        Task<Family> AddFamily(Family family);
        Task UpdateFamily(string id, string lastName);
        Task DeleteFamily(string id, string partitionKey);
        Task<List<Family>> GetFamily(string lastName);
        Task<List<Family>> GetAllFamily();
    }
}
