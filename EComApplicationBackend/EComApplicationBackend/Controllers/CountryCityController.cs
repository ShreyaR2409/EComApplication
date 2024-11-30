using App.Core.App.CountryCity.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EComApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryCityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryCityController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("Country")]
        public async Task<IActionResult> GetCountries()
        {
            var result = await _mediator.Send(new GetAllCountriesQuery());
            return Ok(result);  
        }

        [HttpGet("State")]
        public async Task<IActionResult> GetStates(int id)
        {
            var result = await _mediator.Send(new GetStatesByCountryIdQuery { Id = id});
            return Ok(result);
        }
    }
}
