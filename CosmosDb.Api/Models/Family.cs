using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut.Attributes;
using Newtonsoft;
using Newtonsoft.Json;

namespace CosmosDb.Api.Models
{
    public class Family
    {
        [CosmosPartitionKey]
        public string PartitionKey { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
        public string LastName { get; set; }
        public Parent[] Parents { get; set; }
        public Child[] Children { get; set; }
        public Address Address { get; set; }
        public bool IsRegsitered { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
