using DASHOP.DAL.DTO.Request;
using DASHOP.DAL.DTO.Response;
using DASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DASHOP.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user is null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid Email"

                    };
                }
                if (await _userManager.IsLockedOutAsync(user))
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account is locked. Please try again later."

                    };


                }
                var resutl = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
                if (resutl.IsLockedOut)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account Locked duo to multiple Failed attempts"
                    };
                }
                else if (resutl.IsNotAllowed)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Please confirm your email before logging in."
                    };
                }
                if (!resutl.Succeeded)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid password."
                    };
                }



                return new LoginResponse
                {
                    Success = true,
                    Message = "Login successfully",
                    AccessToken = await GenerateAccessToken(user)
                };

            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "An error occurred during registration",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var user = request.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = "User registration failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }
                else
                {

                    try
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        token = Uri.EscapeDataString(token);
                        var emailUrl = $"http://localhost:5019/api/Account/ConfirmEmail?token={token}&userId={user.Id}";
                        await _emailSender.SendEmailAsync(
                           user.Email,
                       "Welcome to DASHOP",
                              $"<h1>welcome .. {user.UserName} </h1> <a href='{emailUrl}'>confirm email </a>"
                      );


                    }
                    catch (Exception ex)
                    {
                        return new RegisterResponse
                        {
                            Success = true,
                            Message = "User registered, but email failed: " + ex.Message
                        };
                    }

                    return new RegisterResponse
                    {
                        Success = true,
                        Message = "User registered successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "An error occurred during registration",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return false;
            return true;
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
                {
             new Claim(ClaimTypes.NameIdentifier, user.Id),
             new Claim(ClaimTypes.Name, user.UserName),
             new Claim(ClaimTypes.Email, user.Email),
              };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);


        }

        public async Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ForgotPasswordResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            user.CodeResetPassword = code;
            user.CodeResetPasswordExpiration = DateTime.Now.AddMinutes(15);
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(
                user.Email,
                "Password Reset Request",
                $"<h1>Password Reset Code</h1><p>Your password reset code is: <strong>{code}</strong></p><p>This code will expire in 15 minutes.</p>"
            );

            return new ForgotPasswordResponse
            {
                Success = true,
                Message = "Password reset code sent"
            };
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            if (user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Invalid code"
                };
            }
            if (user.CodeResetPasswordExpiration < DateTime.Now)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Code expired"
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Password reset failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            await _emailSender.SendEmailAsync(
                user.Email,
                "Password Reset Successful",
                $"<h1>Password Reset Successful</h1><p>Your password has been reset successfully.</p>"
            );

            return new ResetPasswordResponse
            {
                Success = true,
                Message = "Password reset successfully"
            };
        }
    }
}
