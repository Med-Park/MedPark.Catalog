using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedPark.Catalog.Dto;
using MedPark.Catalog.Queries;
using MedPark.Common;
using MedPark.Common.Cache;
using MedPark.Common.Dispatchers;
using MedPark.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedPark.Catalog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private IDispatcher _dispatcher { get; }

        public CatalogController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }


        [HttpGet("")]
        [Cached(Constants.Day_In_Seconds)]
        public async Task<IActionResult> Get([FromRoute] CatalogQuery query)
        {
            try
            {
                CatalogDetailDto catalog = await _dispatcher.QueryAsync<CatalogDetailDto>(query);

                return Ok(QueryResult.QuerySuccess(catalog));
            }
            catch (Exception ex)
            {
                return BadRequest(QueryResult.QueryFail(ex.Message));
            }
        }

        [HttpGet("category/{categoryid}")]
        [Cached(Constants.Day_In_Seconds)]
        public async Task<IActionResult> GetCategoryDetails([FromRoute] CategoryQueries query)
        {
            try
            {
                CategoryDetailDto category = await _dispatcher.QueryAsync<CategoryDetailDto>(query);

                return Ok(QueryResult.QuerySuccess(category));
            }
            catch (Exception ex)
            {
                return BadRequest(QueryResult.QueryFail(ex.Message));
            }
        }
    }
}