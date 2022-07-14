using Microsoft.AspNetCore.Mvc;
using SearchEngine.Core.Service.Interface;
using System.Diagnostics;

namespace SearchEngine.Web.Controllers
{
    public class HomeController : Controller
    {
        IPageService _pageService;
        public HomeController(IPageService pageService)
        {
            _pageService = pageService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Search(string q)
        {
            var model = _pageService.Search(q);
            ViewBag.q = q;
            ViewBag.IsImage = false;
            return View("Index",model);
        }
        [HttpGet]
        public IActionResult SearchImage(string q)
        {
            var model = _pageService.SearchImage(q);
            ViewBag.IsImage = true;
            ViewBag.q = q;
            return View("Index", model);
        }
    }
}