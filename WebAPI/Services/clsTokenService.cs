using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class clsTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        
        public clsTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public string GenerateAccessToken(int userId, string username, string RoleName)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("JwtSettings");
            string secretKey = jwtSettings["SecretKey"];
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, RoleName)
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(int userId)
        {
            byte[] randomNumber = new byte[64];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            string rawRefreshToken = Convert.ToBase64String(randomNumber);

            string tokenHash = ComputeSha256Hash(rawRefreshToken);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO UserTokens (UserID, RefreshTokenHash, ExpiryDate) VALUES (@UserID, @TokenHash, @ExpiryDate)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@TokenHash", tokenHash);
                    cmd.Parameters.AddWithValue("@ExpiryDate", DateTime.UtcNow.AddDays(7));

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return rawRefreshToken;
        }

        public async Task<int> ValidateAndRevokeRefreshTokenAsync(int userId, string rawRefreshToken)
        {
            string tokenHash = ComputeSha256Hash(rawRefreshToken);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT TokenID FROM UserTokens WHERE UserID = @UserID AND RefreshTokenHash = @TokenHash AND ExpiryDate > @Now AND RevokedAt IS NULL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@TokenHash", tokenHash);
                    cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);

                    await conn.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    
                    if (result != null)
                    {
                        int tokenId = Convert.ToInt32(result);
                        await RevokeTokenByIdAsync(tokenId);
                        return tokenId;
                    }
                }
            }

            return -1;
        }

        public async Task RevokeTokenByRawAsync(int userId, string rawRefreshToken)
        {
            string tokenHash = ComputeSha256Hash(rawRefreshToken);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE UserTokens SET RevokedAt = @Now WHERE UserID = @UserID AND RefreshTokenHash = @TokenHash AND RevokedAt IS NULL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@TokenHash", tokenHash);
                    cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task RevokeTokenByIdAsync(int tokenId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE UserTokens SET RevokedAt = @Now WHERE TokenID = @TokenID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TokenID", tokenId);
                    cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}