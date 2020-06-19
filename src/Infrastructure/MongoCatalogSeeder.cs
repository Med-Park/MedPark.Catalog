using MedPark.Catalog.Domain;
using MedPark.Common.Mongo;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.Catalog.Infrastructure
{
    public class MongoCatalogSeeder : IMongoDbSeeder
    {
        protected readonly IMongoDatabase Database;

        public MongoCatalogSeeder(IMongoDatabase database)
        {
            Database = database;
        }

        public async Task SeedAsync()
        {
            await CustomSeedAsync();
        }

        protected virtual async Task CustomSeedAsync()
        {
            var cursor = await Database.ListCollectionsAsync();

            var collections = await cursor.ToListAsync();

            if (collections.Any())
            {
                SeedCatalogData();
            }
            else
            {
                //Create collections and seed data
                Database.CreateCollection("category");
                Database.CreateCollection("product");
                Database.CreateCollection("product-catalog");

                SeedCatalogData();
            }

            await Task.CompletedTask;
        }

        private void SeedCatalogData()
        {
            var categoryCollection = Database.GetCollection<Category>("category");

            if (categoryCollection.Count(FilterDefinition<Category>.Empty) == 0)
            {
                categoryCollection.InsertMany(GetCategoriesToSeed());

                var productCollection = Database.GetCollection<Product>("product");
                productCollection.InsertMany(GetSeedProducts());

                var catalogCollection = Database.GetCollection<ProductCatalog>("product-catalog");
                catalogCollection.InsertMany(GetCatalogToSeed());
            }
        }

        private List<Category> GetCategoriesToSeed()
        {
            return JsonConvert.DeserializeObject<List<Category>>(File.ReadAllText(Environment.CurrentDirectory + @"/DummyData/Categories.json"));
        }

        private List<Product> GetSeedProducts()
        {
            return JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(Environment.CurrentDirectory + @"/DummyData/Products.json"));
        }

        private List<ProductCatalog> GetCatalogToSeed()
        {
            return JsonConvert.DeserializeObject<List<ProductCatalog>>(File.ReadAllText(Environment.CurrentDirectory + @"/DummyData/Catalog.json"));
        }
    }
}
