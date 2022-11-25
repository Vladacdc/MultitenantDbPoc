using Microsoft.AspNetCore.Mvc;

namespace MultitenantDbPoc.Controllers;

public class KeyVault : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}