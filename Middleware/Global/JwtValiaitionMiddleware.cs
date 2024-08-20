using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IronDomeAPI.Middleware.Global
{
    public class JwtValiaitionMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtValiaitionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            // Headers {Authorization: Bearer ey37729ythkwaw4i}
            // Bearer ey37729ythkwaw4i
            // [Bearer,ey37729ythkwaw4i]
            var bearerToken = context.Request.Headers["Authorization"].FirstOrDefault();
            string Token = bearerToken.Split(" ").Last();
            if (Token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("1234u;jhurigt;rkjgj;rkgnhwlkrugthkbhlkjds.jvb;wkjhg;qkjegh;kjeq;j;kqejngveq;kjghbeqjg;veqhgikjg.jbg;hg;krhbgiurhjgkbg;h.gbwruijhbgw;ikgnbikjgvnvn;ouhguhguhguhguhguhguhguhguhgjvh;kwjvwnvjjkertkjknv;wejrhbiu;kjh5678");
                try
                {
                    tokenHandler.ValidateToken(Token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;

                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Token has expired");
                        return;
                    }
                }
                catch
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid Token");
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized - Token is missing");
                return;
            }
            await _next(context);

        }
    }
}
