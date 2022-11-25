using Microsoft.AspNetCore.Mvc;

namespace MultitenantDbPoc.Controllers;

public class BooksController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}