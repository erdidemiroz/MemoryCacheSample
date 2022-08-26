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
            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                .SetAbsoluteExpiration(DateTime.Now.AddSeconds(30))
                .SetPriority(CacheItemPriority.High)
                .RegisterPostEvictionCallback((key, value, reason, state) => { _memoryCache.Set("callback", $"{key}-{value}-{reason}"); });

            if (!_memoryCache.TryGetValue("time", out string timeValue))
                _memoryCache.Set("time", DateTime.Now.ToString(), options);

            return View();
        }

        public IActionResult ShowTime()
        {
            ViewBag.Time = _memoryCache.Get<string>("time");
            ViewBag.Callback = _memoryCache.Get<string>("callback");
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