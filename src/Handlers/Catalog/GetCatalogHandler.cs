using MedPark.Catalog.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedPark.Common;
using MedPark.Catalog.Queries;
using MedPark.Common.Handlers;
using MedPark.Catalog.Domain;
using AutoMapper;
using MedPark.Common.Types;
using MedPark.Common.Mongo;
using MedPark.Common.Services;

namespace MedPark.Catalog.Handlers.Catalog
{
    public class GetCatalogHandler : IQueryHandler<CatalogQuery, CatalogDetailDto>
    {
        private IMongoRepository<Product> _productsRepo { get; }
        private IMongoRepository<ProductCatalog> _catalogRepo { get; }
        private IMongoRepository<Category> _categoryRepo { get; }

        private IMapper _mapper { get; }

        public GetCatalogHandler(IMongoRepository<Product> productsRepo, IMapper mapper, IMongoRepository<ProductCatalog> catalogRepo, IMongoRepository<Category> categoryRepo)
        {
            _productsRepo = productsRepo;
            _mapper = mapper;
            _catalogRepo = catalogRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<CatalogDetailDto> HandleAsync(CatalogQuery query)
        {
            IEnumerable<Category> categories = new List<Category>();

            //Get the entire catalog
            categories = await _categoryRepo.FindAsync(x => x.Available);

            CatalogDetailDto catalogDto = new CatalogDetailDto
            {
                Categories = _mapper.Map<List<CategoryDto>>(categories)
            };

            return catalogDto;
        }
    }
}
