using Microsoft.AspNetCore.Mvc;

namespace MultitenantDbPoc.Controllers;

[ApiController]
[Route("[controller]")]
public class KeyVaultController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public KeyVaultController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    [Route("secrets")]
    public IActionResult GetSecrets()
    {
        return Ok(_configuration.GetChildren());
    }

    [HttpGet]
    [Route("secrets/{name}")]
    public IActionResult GetSecrets(string name)
    {
        return Ok(_configuration[name]);
    }
}