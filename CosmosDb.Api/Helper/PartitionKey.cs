using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDb.Api.Helper
{
    public static class PartitionKey
    {
        public static string Generate(string prefix, string id, int partitionCount)
        {
            var md5 = MD5.Create();

            var hashedValue = md5.ComputeHash(Encoding.UTF8.GetBytes(id));
            var intValue = BitConverter.ToInt32(hashedValue, 0);

            intValue = intValue == int.MinValue ? intValue + 1 : intValue;

            return $"{prefix}{Math.Abs(intValue) % partitionCount}";
        }
    }
}
