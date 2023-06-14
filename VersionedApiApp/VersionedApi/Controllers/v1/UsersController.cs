using Microsoft.AspNetCore.Mvc;

namespace VersionedApi.Controllers.v1;

// api/v1/users
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0", Deprecated = true)]
public class UsersController : ControllerBase
{
    // GET: api/<UsersController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "V1 value1", "V1 value2" };
    }
}
