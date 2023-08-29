using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodosApp.DAL;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public AccountController(IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
        IMapper mapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<ActionResult<UserDto>> RegisterAdmin(UserDto registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, "Admin"),
        };
            await _userManager.AddClaimsAsync(user, claims);

            return Ok("Admin Registered Successfully");
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<ActionResult<UserDto>> RegisterUser(UserDto registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, "User"),
        };
            await _userManager.AddClaimsAsync(user, claims);

            return Ok("User Registered Successfully");
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenDto>> Login(LoginDto credentials)
        {
            var user = await _userManager.FindByEmailAsync(credentials.Email);
            if (user != null)
            {
                if (!await _userManager.CheckPasswordAsync(user, credentials.Password))
                    return Unauthorized();
            }
            else
                return Unauthorized();

            var claimsList = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            claimsList.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            foreach (var role in roles)
            {
                claimsList.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKeyString = _configuration.GetValue<string>("SecretKey");
            var secretyKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString ?? string.Empty);
            var secretKey = new SymmetricSecurityKey(secretyKeyInBytes);

            var signingCredentials = new SigningCredentials(secretKey,
                SecurityAlgorithms.HmacSha256Signature);

            var expiryDate = DateTime.Now.AddDays(5);
            var token = new JwtSecurityToken(
                claims: claimsList,
                expires: expiryDate,
                signingCredentials: signingCredentials);


            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString =  tokenHandler.WriteToken(token);

            return new TokenDto
            {
                Token = tokenString,
                ExpiryDate = expiryDate
            };
        }

    }
}
