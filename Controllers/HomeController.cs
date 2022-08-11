using MemoryCacheSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace SampleMemoryCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {

            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("time", out string timeValue))
                _memoryCache.Set("time", DateTime.Now.ToString(), new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(10),
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30)
                });

            return View();
        }

        public IActionResult ShowTime()
        {
            ViewBag.Time = _memoryCache.Get<string>("time");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}