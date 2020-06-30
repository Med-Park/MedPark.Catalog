using MedPark.Catalog.Domain;
using MedPark.Catalog.Queries;
using MedPark.Common.Handlers;
using MedPark.Common.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.Catalog.Handlers.Catalog
{
    public class GetIsProductInStockQueryHandler : IQueryHandler<IsProductInStockQuery, Boolean>
    {
        private IMongoRepository<Product> _productsRepo { get; }

        public GetIsProductInStockQueryHandler(IMongoRepository<Product> productsRepo)
        {
            _productsRepo = productsRepo;
        }

        public async Task<bool> HandleAsync(IsProductInStockQuery query)
        {
            var prod = await _productsRepo.GetAsync(query.ProductId);

            return (prod.AvailableQuantity > 0 ? true : false);
        }
    }
}
