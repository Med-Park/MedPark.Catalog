using MedPark.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.Catalog.Domain
{
    public class Category : BaseIdentifiable
    {
        public Category(Guid id) : base(id)
        {

        }

        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string Description { get; private set; }
        [JsonProperty]
        public bool Available { get; private set; }
        [JsonProperty]
        public Guid? ParentCategory { get; private set; }
    }
}
