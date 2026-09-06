using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/application")]
    public class ApplicationController : ControllerBase
    {

    }
}