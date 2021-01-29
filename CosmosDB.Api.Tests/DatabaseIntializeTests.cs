using System;
using Xunit;
using CosmosDb.Api.Interface;
using CosmosDb.Api.Models;
using CosmosDb.Api.Services;
using Microsoft.Extensions.Logging;
using CosmosDb.Api.Data.Configuration.Store;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CosmosDB.Api.Tests
{
    public class DatabaseIntializeTests
    {
        private readonly Mock<IDatabaseService> mockDatabaseService;
        //private readonly IDatabaseService _databaseService;
        private readonly IFamilyService _familyService;
        //private ILogger<DatabaseService> _loggerDatabase;

        private readonly ILogger<FamilyService> _loggerFamily;

        private readonly DatabaseService _dbService;
        private readonly FamilyService _famService;

        private CosmosStore _store;
        public DatabaseIntializeTests()
        {
            var _loggerDatabase = new Mock<ILogger<DatabaseService>>();
            var _loggerFamily = new Mock<ILogger<FamilyService>>();

            mockDatabaseService = new Mock<IDatabaseService>();

            CosmosSetting();

            _dbService = new DatabaseService(_store, _loggerDatabase.Object);

            _famService = new FamilyService(mockDatabaseService.Object, _store, _loggerFamily.Object);
        }

        //public DatabaseIntializeTests(IDatabaseService databaseService,
        //                              IFamilyService familyService,
        //                              ILogger<DatabaseService> loggerDatabase,
        //                              ILogger<FamilyService> loggerFamily)
        //{
        //    _databaseService = databaseService;
        //    _familyService = familyService;
        //    _loggerDatabase = loggerDatabase;
        //    _loggerFamily = loggerFamily;

        //    _dbService = new DatabaseService(store, _loggerDatabase);
        //    _famService = new FamilyService(_databaseService, store, loggerFamily);

        //}

        private void CosmosSetting()
        {
            var cosmosSetting = new CosmosStoreConfigs
            {
                AccountEndPoint = "https://cosmos-db-sample.documents.azure.com:443/",
                AccountKey = "tTL4FwbiqNVeZAgUYW3U3zCv5pe9CUHPO8zVUP48dbV8joW5UM39FeHoeO9jfTq0e67bD7qlCEzumpqA00SaTw==",
                Database = "FamilyTree",
            };

            var client = new CosmosClient(cosmosSetting.AccountEndPoint, cosmosSetting.AccountKey);
            _store = new CosmosStore
            {
                ClientStore = client,
                DatabaseCosmos = null,
                DatabaseName = cosmosSetting.Database
            };

        }

        [Fact]
        public void DatabaseIntialize_CreateIfNotExists()
        {
            var db = _dbService.GetDatabase();
            Assert.NotNull(db);
        }

        [Fact]
        public void ContainerInitialize_CreateIfNotExist()
        {
            var container = _dbService.GetContainer("Family", "/LastName");
            Assert.NotNull(container);

        }
        [Fact]
        public void Create_New_Family_Item_Tests()
        {
            // Create a family object for the Wakefield family
            Family wakefieldFamily = new Family
            {
                Id = "Wakefield.7",
                LastName = "Wakefield",
                Parents = new Parent[]
                {
                    new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
                    new Parent { FamilyName = "Miller", FirstName = "Ben" }
                },
                Children = new Child[]
                {
                    new Child
                    {
                        FamilyName = "Merriam",
                        FirstName = "Jesse",
                        Gender = "female",
                        Grade = 8,
                        Pets = new Pet[]
                        {
                            new Pet { GivenName = "Goofy" },
                            new Pet { GivenName = "Shadow" }
                        }
                    },
                    new Child
                    {
                        FamilyName = "Miller",
                        FirstName = "Lisa",
                        Gender = "female",
                        Grade = 1
                    }
                },
                Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
                IsRegsitered = true
            };

            var db = _dbService.GetDatabase();
            mockDatabaseService.Setup(m => m.GetDatabase()).Returns(db);
            var container = _dbService.GetContainer("Family", "/LastName");
            mockDatabaseService.Setup(m => m.GetContainer("Family","/LastName")).Returns(container);

            var familyNew = _famService.AddFamily(wakefieldFamily);

            Assert.NotNull(familyNew);
        }
    }
}
