using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Settings
{
    public class JwtService
    {
        public static readonly string key = "R4t14_N3_R083_80l070_P0_K0l1n0_S3cr3t_K3y";
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

        public static ObjectId? GetUserIdFromToken(string token)
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
                if (userIdClaim != null && ObjectId.TryParse(userIdClaim.Value, out ObjectId Id))
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
