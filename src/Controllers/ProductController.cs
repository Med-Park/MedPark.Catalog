using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedPark.Catalog.Dto;
using MedPark.Catalog.Queries;
using MedPark.Common;
using MedPark.Common.Cache;
using MedPark.Common.Dispatchers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedPark.Catalog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IDispatcher _dispatcher { get; }

        public ProductController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }


        [HttpGet("{productid}")]
        [Cached(Constants.Day_In_Seconds)]
        public async Task<IActionResult> Get([FromRoute] ProductQueries query)
        {
            try
            {
                ProductDetailDto product = await _dispatcher.QueryAsync(query);

                return Ok(QueryResult.QuerySuccess(product));
            }
            catch (Exception ex)
            {
                return BadRequest(QueryResult.QueryFail(ex.Message));
            }
        }

        [HttpGet("isproductavailable/{productid}")]
        public async Task<IActionResult> IsProductInStock([FromRoute] IsProductInStockQuery query)
        {
            try
            {
                bool staockStatus = await _dispatcher.QueryAsync(query);

                return Ok(QueryResult.QuerySuccess(staockStatus));
            }
            catch (Exception ex)
            {
                return BadRequest(QueryResult.QueryFail(ex.Message));
            }
        }
    }
}