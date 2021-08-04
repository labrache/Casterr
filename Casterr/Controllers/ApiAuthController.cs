using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Casterr.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CasterrLib.classes;

namespace Casterr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiAuthController : ControllerBase
    {
        private SignInManager<AppUser> _signinManager;
        private IConfiguration _conf;
        public ApiAuthController(SignInManager<AppUser> signinManager, IConfiguration configuration)
        {
            _signinManager = signinManager;
            _conf = configuration;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            AuthenticateRes resp = new AuthenticateRes()
            {
                success = false
            };
            Microsoft.AspNetCore.Identity.SignInResult result = await _signinManager.PasswordSignInAsync(model.Username, model.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                resp.success = true;
            }
            else
            {
                resp.Message = "Invalid login attempt.";
            }
            if (result.RequiresTwoFactor)
            {
                resp.Message = "LoginWith2fa";
            }
            if (result.IsLockedOut)
            {
                resp.Message = "User account locked out.";
            }
            if (result.IsNotAllowed)
            {
                resp.Message = "User not allowed.";
            }
            return Ok(resp);
        }
    }
}
