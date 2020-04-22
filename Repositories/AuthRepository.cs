using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BaseSecurity.Db.Scripts;
using BaseSecurity.Interfaces;
using BaseSecurity.Models;
using BaseSecurity.Security;
using Dapper;
using Extensions.MV;
using Microsoft.IdentityModel.Tokens;

namespace BaseSecurity.Repositories
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        private SigningConfiguration _signingConfig;

        private TokenConfiguration _tokenConfiguration;

        public AuthRepository(
            IDbSession dbSession, 
            SigningConfiguration signingConfiguration,
            TokenConfiguration tokenConfigurations) 
            : base(dbSession) { 
                _signingConfig = signingConfiguration;
                _tokenConfiguration = tokenConfigurations;
        }

        public async Task<User> GetUser(string username) {
            using (var connection = CreateConnection()) {
                var user = await connection.QueryFirstOrDefaultAsync<User>(UserScripts.Get, new { username });
                return user;
            }
        }

        public string CreateToken(User user) {
            
            var claims = new [] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var tokenDescriptor = new SecurityTokenDescriptor {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = _signingConfig.Credentials
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> Register(User user)
        {
            using (var connection = CreateConnection()) {
                await connection.ExecuteAsync(UserScripts.Insert, user);
            }
            return user;
        }

        public int GetUserIdFromToken(string token)
        {
            try 
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenData = tokenHandler.ReadJwtToken(token);
                var userId = tokenData.Claims.AsList().FirstOrDefault(x => x.Type == "nameid").Value;
                return userId.ToInt();
            }
            catch (Exception ex) 
            {
                throw new Exception("Session Expired, try logging in again", ex);
            }
        }
    }
}