using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDb.Api.Data.Configuration.Store
{
    public class CosmosStore
    {
        public CosmosClient ClientStore { get; set; }
        public Database DatabaseCosmos { get; set; }
        public string DatabaseName { get; set; }

    }
}
