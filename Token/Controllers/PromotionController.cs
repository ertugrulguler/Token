using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Token.Controllers
{
    public class PromotionController : Controller
    {
        [Authorize]
        [Route("promotion")]

        [HttpGet("new")]
        public IActionResult Get()
        {
            foreach (var claim in HttpContext.User.Claims)
            {
                Console.WriteLine("Claim Type: {0}:\nClaim Value:{1}\n", claim.Type, claim.Value);
            }
            var promotionCode = Guid.NewGuid();
            return new ObjectResult($"İşte Muhteşem Promosyon Kodunuz {promotionCode}");
        }

    }
}