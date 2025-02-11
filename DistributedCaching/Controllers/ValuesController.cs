using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace DistributedCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("set/{name}/{surname}")]

        public async Task<IActionResult> Set(string name, string surname)
        {
            
            await _distributedCache.SetStringAsync("name", name, options: new()
            {
                //config
                AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            });
            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            });
            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetStringAsync("name");
            var surnameB = await _distributedCache.GetAsync("surname");
            var surname = Encoding.UTF8.GetString(surnameB);
            return Ok(new
            {
                name,
                surname
            });
        }
    }
}
