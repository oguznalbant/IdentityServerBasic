using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GarantiAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GarantiBankController : ControllerBase
    {
        [HttpGet("{musteriId}")]
        public double Bakiye(int musteriId)
        {
            //....
            return 1000;
        }
        [HttpGet("{musteriId}")]
        [Authorize(Policy = "ReadGaranti")]
        public List<string> TumHesaplar(int musteriId)
        {
            //....
            return new List<string>()
            {
                "123456789",
                "987654321",
                "564738291"
            };
        }
    }
}
