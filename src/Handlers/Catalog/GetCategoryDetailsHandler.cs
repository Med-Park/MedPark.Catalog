using AutoMapper;
using MedPark.Catalog.Domain;
using MedPark.Catalog.Dto;
using MedPark.Catalog.Queries;
using MedPark.Common;
using MedPark.Common.Handlers;
using MedPark.Common.Mongo;
using MedPark.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.Catalog.Handlers.Catalog
{
    public class GetCategoryDetailsHandler : IQueryHandler<CategoryQueries, CategoryDetailDto>
    {
        private IMongoRepository<Product> _productsRepo { get; }
        private IMongoRepository<ProductCatalog> _catalogRepo { get; }
        private IMongoRepository<Category> _categoryRepo { get; }

        private IMapper _mapper { get; }

        public GetCategoryDetailsHandler(IMongoRepository<Product> productsRepo, IMapper mapper, IMongoRepository<ProductCatalog> catalogRepo, IMongoRepository<Category> categoryRepo)
        {
            _productsRepo = productsRepo;
            _mapper = mapper;
            _catalogRepo = catalogRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<CategoryDetailDto> HandleAsync(CategoryQueries query)
        {
            Category cat = await _categoryRepo.GetAsync(query.CategoryId);

            if (cat is null)
                throw new MedParkException("category_does_not_exist", $"The category { query.CategoryId} does not exist.");

            CategoryDetailDto categoryDetails = _mapper.Map<CategoryDetailDto>(cat);

            var categoryCatalog = await _catalogRepo.GetAllAsync(x => x.CategoryId == cat.Id);

            var catalogIds = categoryCatalog.ToList().Select(x => x.ProductId);

            var catProducts = await _productsRepo.GetAllAsync(x => catalogIds.Contains(x.Id));

            categoryDetails.Products = _mapper.Map<List<ProductDetailDto>>(catProducts);

            return categoryDetails;
        }
    }
}
