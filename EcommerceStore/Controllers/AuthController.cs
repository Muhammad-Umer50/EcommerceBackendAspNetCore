using ECommerceStore.Dtos.AuthDtos;
using ECommerceStore.Models;
using ECommerceStore.Services;
using ECommerceStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ICartService _cartService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ICartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _cartService = cartService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser is not null)
                return Conflict("A user with this email already exists.");

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.Name
            };
            if(dto.Password == null)
                throw new ArgumentNullException(nameof(dto.Password), "Password cannot be null.");
        
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            // Optional: assign a default role
            await _userManager.AddToRoleAsync(user, "User");

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiresAt) = _tokenService.CreateToken(user, roles);
            var cart = await _cartService.CreateCartAsync(user.Id);
            var cartid = await _cartService.GetCartByUserIdAsync(user.Id);
            return Ok(new AuthResponseDto  { Email = user.Email, Token = token, ExpiresAt = expiresAt ,CartId = cartid.Id });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if(dto.Email == "" && dto.password  == "")
                return BadRequest("Email and password are required.");

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return Unauthorized("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiresAt) = _tokenService.CreateToken(user, roles);
            var cartid = await _cartService.GetCartByUserIdAsync(user.Id);
            return Ok(new AuthResponseDto { Email = user.Email, Token = token, ExpiresAt = expiresAt , CartId = cartid.Id });
        }
    }
}
