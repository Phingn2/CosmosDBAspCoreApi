using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDb.Api.Interface
{
    public interface IDatabaseService
    {
        Task<Database> GetDatabase();
        Task<Container> GetContainer(string containerId, string partitionKey);
    }
}
