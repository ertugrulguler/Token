using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Token.Controllers
{
    [Route("token")]
    [AllowAnonymous()]
    public class TokenController : ControllerBase
    {
        [HttpPost("getnewaccesstoken")]
        public IActionResult GetToken([FromBody]UserInfo user)
        {
            Console.WriteLine("User name:{0}", user.Username);
            Console.WriteLine("Password:{0}", user.Password);
            if (IsValidUserAndPassword(user.Username, user.Password))
                return new ObjectResult(GenerateToken(user.Username));

            return Unauthorized();
        }

        private string GenerateToken(string userName)
        {
            var someClaims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.UniqueName,userName),
                new Claim(JwtRegisteredClaimNames.Email,"kuthaygumus@silverlab.com"),
                new Claim(JwtRegisteredClaimNames.NameId,Guid.NewGuid().ToString())
            };

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bu_benim_muhtesem_uzunluktaki_muhtesem_saklanmis_guvelik_keyim"));
            var token = new JwtSecurityToken(
                issuer: "kuthay-gumus.silverlab.com",
                audience: "kuthaygumus.silverlab.com",
                claims: someClaims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsValidUserAndPassword(string userName, string password)
        {
            //Demo için sürekli True döndürdük.
            //Internal bir NoSQL çözüm üzerinde, username ve passwordleri tutabiliriz.
            //Client, validation kontrolünden sonra, token almaya hak kazanmalı.
            return true;
        }
    }

    public class UserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
