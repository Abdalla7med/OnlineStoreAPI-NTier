
using Common.DTOs;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings;
namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {


        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration configuration)
        {
            this.userManager = userManager;
            _config = configuration;
        }


        [HttpPost("Registration")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            var userExists = await userManager.FindByNameAsync(registrationDto.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = registrationDto.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registrationDto.UserName
                };

                IdentityResult result = await userManager.CreateAsync(user, registrationDto.Password);
                if (!result.Succeeded)
                    return StatusCode(
                                    StatusCodes.Status500InternalServerError,
                                    new { Status = "Error", Message = "User creation failed! Please check user details and try again." }
                             );

                return Ok(
                        new
                        {
                            Status = "Success",
                            Message = "User created successfully!"
                        });
            }

            return StatusCode(
                                  StatusCodes.Status500InternalServerError,
                                  new { Status = "Error", Message = "User creation failed! Please check user details and try again." }
                           );

        }
       /* 
        * 
        * Old Registration Action 
        public async Task<IActionResult> Register(RegistrationDto userDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email
                };
                
                IdentityResult result = await userManager.CreateAsync(user, userDto.Password);

                if(result.Succeeded)
                {

                    return Ok("Registration is done Successfully");

                }

                List<string> errors = new List<string>();
                foreach(var error in result.Errors)
                {
                    errors.Add(error?.ToString() ?? string.Empty);
                }

                return BadRequest(errors.ToString());
            }

            return BadRequest("Try Again with correct data ");
        }
*/
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(loginDto.UserName);

                if(user != null)
                {

                    bool found = await userManager.CheckPasswordAsync(user, loginDto.Password);

                    if (found)
                    {


                        List<Claim> claims = new List<Claim>();

                        claims.Add(new Claim(ClaimTypes.Name, user.UserName ?? string.Empty));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles =await  userManager.GetRolesAsync(user);


                        /// create SigningCredentials 
                        /// needed at first a ValidSecurityKey
                        var k = _config["Jwt:Key"];
                        Console.WriteLine(k);
                        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                       

                        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        /// Create Token 
                        /// 
                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: _config["Jwt:Issuer"],
                            audience: _config["Jwt:Audience"],
                            claims:claims,
                            signingCredentials: credentials,
                            expires : DateTime.Now.AddHours(2) // valid to two hours after the creation 
                            );


                        return Ok(
                            new
                            {
                                Token = new JwtSecurityTokenHandler().WriteToken(token),
                                expires = token.ValidTo

                            });
                    }
                    return BadRequest("Failed To Sign in");

                }
                return Unauthorized();
            }

            return Unauthorized();
        }

    }

}
