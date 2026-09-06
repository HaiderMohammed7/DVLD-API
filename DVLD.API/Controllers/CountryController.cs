using DVLD.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Country")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepositroy _country;
        public CountryController(ICountryRepositroy country)
        {
            _country = country;
        }

        [HttpGet()]
        public async Task<IActionResult> GetCountryName()
        {
            var result = await _country.GetCountryNameAsync();
            return Ok(result);
        }
    }
}