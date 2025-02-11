using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IMemoryCache _memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        // Basic Example
        [HttpGet("set/{name}")]
        public void Set(string name)
        {
            // Key-Value format
            _memoryCache.Set("name", name);
            Console.WriteLine("başarılı");

        }
        [HttpGet("get")]
        public string Get()
        {
            //return _memoryCache.Get<string>("name");
            if (_memoryCache.TryGetValue<string>("name", out string name))
            {
                return name.Substring(5);
            }
            return "veri set edilmedi";
        }



        // absolute - sliding exapmle
        [HttpGet("setDate")]
        public void SetDate()
        {
            _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
            {
                // Life Time: 30 seconds
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                // Must be get in 5 seconds
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
        }

        [HttpGet("getDate")]
        public DateTime GetDate()
        {
            return _memoryCache.Get<DateTime>("date");
        }

    }
}
