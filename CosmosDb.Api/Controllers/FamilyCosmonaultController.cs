using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDb.Api.Interface;
using CosmosDb.Api.Models;
using CosmosDb.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosmosDb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyCosmonaultController : ControllerBase
    {
        private readonly IFamilyServiceCosmonault _familyServiceCosmonault;

        private readonly ILogger<FamilyServiceCosmonaut> _logger;

        public FamilyCosmonaultController(IFamilyServiceCosmonault familyServiceCosmonault, ILogger<FamilyServiceCosmonaut> logger)
        {
            _familyServiceCosmonault = familyServiceCosmonault;
            _logger = logger;
        }

        // GET: api/<FamilyCosmonaultController>
        [HttpGet]
        public async Task<List<Family>> Get()
        {
            return await _familyServiceCosmonault.GetAllFamily();
        }

        // GET api/<FamilyCosmonaultController>/5
        [HttpGet("{id}")]
        public async Task<List<Family>> Get(string id, string lastName)
        {
            return await _familyServiceCosmonault.GetFamily(lastName);
        }

        // POST api/<FamilyCosmonaultController>
        [HttpPost]
        public async Task<Family> Post([FromBody] Family family)
        {
            return await _familyServiceCosmonault.AddFamily(family);
        }

        // PUT api/<FamilyCosmonaultController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] string lastName)
        {
            await _familyServiceCosmonault.UpdateFamily(id, lastName);
        }

        // DELETE api/<FamilyCosmonaultController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id, string partitionKey)
        {
            await _familyServiceCosmonault.DeleteFamily(id, partitionKey);
        }
    }
}
