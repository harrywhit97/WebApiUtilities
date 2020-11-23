using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiUtilities.Concrete;

namespace WebApiUtilities.Identity
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<User> Get(string id);
        IEnumerable<User> GetAll();
        Task<User> Register(UserRegistration userRegistration);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppSettings _appSettings;

        public UserService(UserManager<User> userManager, AppSettings appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings;
        }

        private async Task<User> AuthenticateUser(AuthenticateRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return null;

            var validators = _userManager.PasswordValidators
                .Select(x => x.ValidateAsync(_userManager, user, model.Password));

            var results = await Task.WhenAll(validators);

            if (!results.Any(x => x.Succeeded))
                return null;

            return user;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await AuthenticateUser(model);

            if (user == null) 
                return null;

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public async Task<User> Register(UserRegistration userRegistration)
        {
            var user = await _userManager.FindByEmailAsync(userRegistration.Email);

            if(user != null)
                throw new Exception("A user already exists with that email.");

            user = new User()
            {
                Email = userRegistration.Email,
                UserName = userRegistration.UserName
            };
            var result = await _userManager.CreateAsync(user, userRegistration.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new Exception($"Failed to register user: {errors}");
            }
            return await _userManager.FindByEmailAsync(user.Email);
        }

        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWTKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> Get(string id)
            => await _userManager.FindByIdAsync(id);

        public IEnumerable<User> GetAll()
            => _userManager.Users;

    }
}
