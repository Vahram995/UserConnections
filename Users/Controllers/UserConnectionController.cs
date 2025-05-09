using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Models.RequestModels;
using Users.Services.Abstract;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserConnectionController(IUserConnectionService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddConnection([FromBody] UserConnectionRequestModel model)
        {
            await service.AddConnectionAsync(model.UserId, model.IpAddress);
            return Ok();
        }

        [HttpGet("searchByIp")]
        public async Task<IActionResult> FindUsers([FromQuery] string ipPrefix)
        {
            var users = await service.FindUsersByIpPrefixAsync(ipPrefix);
            return Ok(users);
        }

        [HttpGet("{userId}/ips")]
        public async Task<IActionResult> GetUserIps(long userId)
        {
            var ips = await service.GetUserIpsAsync(userId);
            return Ok(ips);
        }

        [HttpGet("{userId}/last")]
        public async Task<IActionResult> GetUserLast(long userId)
        {
            var result = await service.GetUserLastConnectionAsync(userId);
            return Ok(result);
        }

        [HttpGet("{userId}/lastIps")]
        public async Task<IActionResult> GetLastSeen([FromQuery] long userId)
        {
            var result = await service.GetLastSeenPerIpAsync(userId);
            return Ok(result);
        }
    }
}
