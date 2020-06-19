using MedPark.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.Catalog.Domain
{
    public class Product : BaseIdentifiable
    {
        public Product(Guid id) : base(id)
        {
        }

        [JsonProperty]
        public string Code { get; private set; }
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string Description { get; private set; }
        [JsonProperty]
        public bool Available { get; private set; }
        [JsonProperty]
        public int AvailableQuantity { get; private set; }
        [JsonProperty]
        public bool HasMarkup { get; private set; }
        [JsonProperty]
        public int Markup { get; private set; }
        [JsonProperty]
        public decimal Price { get; private set; }
        [JsonProperty]
        public string NappiCode { get; private set; }
    }
}
