using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UniversityAdmission.Models;

namespace UniversityAdmission.Settings
{
    public class JwtService
    {
        public static readonly string key = "tratatataH2#$000D!2231dawdd)))wd22";
        public static string GenerateToken(User user){
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);
            
            List<Claim> claims = new(){new Claim("Id", user.Id.ToString())};

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(1)
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }

        public static int? GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(JwtService.key))
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                var userIdClaim = principal.FindFirst("Id");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int Id))
                {
                    return Id;
                }

                return null; 
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
