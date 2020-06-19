using MedPark.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.Catalog.Domain
{
    public class ProductCatalog : BaseIdentifiable
    {
        public ProductCatalog(Guid id) : base(id)
        {

        }

        [JsonProperty]
        public Guid ProductId { get; private set; }
        [JsonProperty]
        public Guid CategoryId { get; private set; }
    }
}
