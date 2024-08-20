using IronDomeAPI.Models;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


using System.Text;



namespace IronDomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private string GenerateToken(string userIP)
        {
            // token handler can create token
            var tokenHandler = new JwtSecurityTokenHandler();
            string secretKey = "1234u;jhurigt;rkjgj;rkgnhwlkrugthkbhlkjds.jvb;wkjhg;qkjegh;kjeq;j;kqejngveq;kjghbeqjg;veqhgikjg.jbg;hg;krhbgiurhjgkbg;h.gbwruijhbgw;ikgnbikjgvnvn;ouhguhguhguhguhguhguhguhguhgjvh;kwjvwnvjjkertkjknv;wejrhbiu;kjh5678";//למחוק מהקוד!!!
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            // token descriptor describe HOW to create the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userIP),
                    }
                ),
                // expiration time of the token
                Expires = DateTime.Now.AddMinutes(3),
                // the secret key of the token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            // creating the token
            var token =  tokenHandler.CreateToken( tokenDescriptor);
            // converting the token to string
            var tokenString =  tokenHandler.WriteToken(token);

            return tokenString;
        }
        [HttpPost("login")]
        public IActionResult Login(LoginObject loginObject)
        {
            if (loginObject.UserName == "admin" && loginObject.Password == "123456")
            {
                string userIP = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                return StatusCode(200, new { token = GenerateToken(userIP) });
            }
            return StatusCode(401, new { error = "invalid" });
        }
    }
}
