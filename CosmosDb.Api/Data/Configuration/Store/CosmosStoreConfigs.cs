using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDb.Api.Data.Configuration.Store
{
    public class CosmosStoreConfigs
    {
        public string Database { get; set; }
        public string AccountKey { get; set; }
        public string AccountEndPoint { get; set; }
    }
}
