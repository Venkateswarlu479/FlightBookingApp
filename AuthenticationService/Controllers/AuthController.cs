using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Database;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Cors;

namespace AuthenticationService.Controllers
{
    /// <summary>
    /// AuthController class
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AuthController : ControllerBase
    {
        #region Private Properties
        /// <summary>
        /// _config
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// _repository
        /// </summary>
        private readonly IDataRepository _repository;

        /// <summary>
        /// _mapper
        /// </summary>
        private readonly IMapper _mapper;
        #endregion Private Properties

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public AuthController(IConfiguration config, IDataRepository repository, IMapper mapper)
        {
            _repository = repository;
            _config = config;
            _mapper = mapper;
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        /// To Get User details from database
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("GetUserDetails")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> GetUserDetailsAsync(string userName)
        {
            var userDetails = await _repository.GetUserDetails(userName).ConfigureAwait(false);
            if (userDetails == null)
                return BadRequest("No Users Found");

            //var result = _mapper.Map<User, UserModel>(userDetails);
            return userDetails;
        }

        /// <summary>
        /// To register user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<ActionResult<User>> RegisterUserAsync([FromBody] UserRequestModel userModel)
        {
            CreatePasswordHash(userModel.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var userData = new User()
            {
                UserName = userModel.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedBy = userModel.UserName,
                CreatedDateTime = DateTime.Now,
                LastChangedBy = userModel.UserName,
                LastChangedDateTime = DateTime.Now
            };
            var result = await _repository.SaveUserDetails(userData, "register").ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        /// To provide login to user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ActionResult<string>> LoginAsync([FromBody] UserRequestModel userModel)
        {
            var user = await _repository.GetUserDetails(userModel.UserName).ConfigureAwait(false);
            if(user == null)
                return BadRequest("User does not exist");
            if (user.UserName != userModel.UserName)
            {
                return BadRequest("User does not exist");
            }
            if(!VerifyPassword(userModel.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong Password");
            }

            var token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user);

            var userInfo = _mapper.Map<UserModel, User>(user);
            userInfo.LastChangedDateTime = DateTime.Now;
            userInfo.LastChangedBy = userModel.UserName;
            var result = await _repository.SaveUserDetails(userInfo, "login").ConfigureAwait(false);
            if (result == null)
                return "Result returned null while updating data in DB";

            return Ok(token);
        }

        /// <summary>
        /// To refresh token 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<string>> RefreshTokenAsync(string userName)
        {
            var user = await _repository.GetUserDetails(userName).ConfigureAwait(false);
            if (user == null)
                return BadRequest("User does not exist");

            var refreshToken = Request.Cookies["refreshToken"];
            if (!user.RefreshToken.Equals(refreshToken))
                return Unauthorized("Invalid refresh token");
            if (user.TokenExpires < DateTime.Now)
                return Unauthorized("Token expired");

            var token = CreateToken(user);

            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user);

            return Ok(token);
        }
        #endregion Public Methods

        #region Private Methods
        private void SetRefreshToken(RefreshToken newRefreshToken, UserModel user)
        {
            var cookiesOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookiesOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenExpires = newRefreshToken.Expires;
            user.TokenCreated = newRefreshToken.Created;
        }

        private RefreshToken GenerateRefreshToken()
        {
            byte[] random = new byte[64];
            var refreshToken = new RefreshToken()
            {
                //Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Token = Convert.ToBase64String(random),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(2)
            };

            return refreshToken;
        }

        private string CreateToken(UserModel user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion Private Methods
    }
}
