using Microsoft.AspNetCore.Mvc;
using MultitenantDbPoc.Models;
using MultitenantDbPoc.Persistence;

namespace MultitenantDbPoc.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public BooksController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("/{institutionId}")]
    //[HeaderDictionary()]
    public IActionResult Get(string institutionId)
    {
        return Ok(_dbContext.Set<Book>().ToList());
    }
}