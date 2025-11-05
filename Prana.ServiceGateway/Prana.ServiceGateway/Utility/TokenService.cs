using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Serilog;
using Newtonsoft.Json;

namespace Prana.ServiceGateway.Utility
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtOptions> _jwtOptions;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration,
            IDistributedCache cache, IHttpContextAccessor httpContextAccessor,
            IOptions<JwtOptions> jwtOptions,
            ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
            this._logger = logger;
        }

        public string CreateToken(UserDto userDto)
        {
            try
            {
                _logger.LogInformation("Creating token info userId:{uId}, isSupport:{support}, isAdmin:{isAdmin}",
                    userDto.CompanyUserId, userDto.IsSupport, userDto.IsAdmin);

                var securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration[UtilityConstants.CONST_AUTH_SETTINGS_SECRET_KEY]));

                var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                var clims = new[]
                   {
                    new Claim(ApiContants.ADMIN, userDto.IsAdmin.ToString()),
                    new Claim(ApiContants.SUPPORT, userDto.IsSupport.ToString()),
                    new Claim(ApiContants.COMPANY_USER_ID, userDto.CompanyUserId.ToString()),
                    new Claim(ApiContants.USER_NAME, userDto.UserName.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                       };

                var tokenDescriptor = new JwtSecurityToken
                (
                    issuer: _configuration[UtilityConstants.CONST_AUTH_SETTINGS_ISSUER],
                    audience: _configuration[UtilityConstants.CONST_AUTH_SETTINGS_AUDIENCE],
                    claims: clims,
                    expires: DateTime.UtcNow.AddYears(1), //Never expire token
                    signingCredentials: signinCredentials
                );

                var tokenstring = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

                try
                {
                    string encryptedToken = EncryptToken(tokenstring,
                        GetKeyBytes(_configuration[GlobalConstants.CONST_AUTHSETTINGS_SECRET_KEY].ToString(),
                        _configuration[GlobalConstants.CONST_AUTHSETTINGS_SALT]));

                    return encryptedToken;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while encrypting token");
                    throw;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateToken of TokenService");
                throw;
            }
        }

        public UserDto GetUserDtoFromTokenClain(ClaimsIdentity claimIdentity)
        {
            try
            {
                if (claimIdentity == null)
                    throw new Exception(MessageConstants.MSG_CONST_CLAIM_IDENTITY_NULL_CALL_GENERATE_API);

                if (claimIdentity.Claims == null || !claimIdentity.Claims.Any())
                    throw new Exception(MessageConstants.MSG_CONST_NO_CLAIM_FOUND_GENERATE_API_AND_TRY_AGAIN);

                var adminClaim = claimIdentity.Claims.First(c => c.Type == ApiContants.ADMIN).Value;
                var supportClaim = claimIdentity.Claims.First(c => c.Type == ApiContants.ADMIN).Value;
                var userIdClaim = claimIdentity.Claims.First(c => c.Type == ApiContants.COMPANY_USER_ID).Value;
                var userName = claimIdentity.Claims.First(c => c.Type == ApiContants.USER_NAME).Value;

                var userDto = new UserDto
                {
                    CompanyUserId = Convert.ToInt32(userIdClaim),
                    IsAdmin = adminClaim.ToLower() == UtilityConstants.CONST_TRUE,
                    IsSupport = supportClaim.ToLower() == UtilityConstants.CONST_TRUE,
                    UserName = userName,
                };
                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserDtoFromTokenClain of TokenService");
                throw;
            }
        }

        /// <summary>
        /// This Method handles the Encryption of Token
        /// </summary>
        public static string EncryptToken(string token, byte[] key)
        {
            try
            {
                byte[] encryptedBytes;
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.KeySize = 256;
                    aesAlg.Key = key;
                    aesAlg.GenerateIV();
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
                            csEncrypt.Write(tokenBytes, 0, tokenBytes.Length);
                            csEncrypt.FlushFinalBlock();
                        }
                        encryptedBytes = msEncrypt.ToArray();
                    }
                }
                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in EncryptToken of TokenService");
                throw;
            }
        }

        /// <summary>
        /// This Method handles the Decryption of Token
        /// </summary>
        public static string DecryptToken(string token, byte[] key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token) || token?.ToLower() == "null")
                {
                    throw new ArgumentNullException($"Access {nameof(token)} is null or empty. This may indicate that the user is not properly logged in or that a validation failure occurred during login. If no validation message appears in the notification, please check the Notification Service status in the OpenFin Process Manager");
                }

                byte[] encryptedBytes = Convert.FromBase64String(token);
                byte[] iv = new byte[16];
                byte[] tokenBytes;
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.KeySize = 256;
                    aesAlg.Key = key;
                    Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
                    aesAlg.IV = iv;
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                tokenBytes = Encoding.UTF8.GetBytes(srDecrypt.ReadToEnd());
                            }
                        }
                    }
                }
                return Encoding.UTF8.GetString(tokenBytes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Decrypt Token Error");
                throw;
            }
        }

        /// <summary>
        /// This Method handles the convertion of key into byte array with the use of token salt .
        /// </summary>
        public static byte[] GetKeyBytes(string key, string salt)
        {
            try
            {
                byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
                using (Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(key, saltBytes, 10000, HashAlgorithmName.SHA256))
                {
                    return keyDerivation.GetBytes(32);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetKeyBytes in tokenService");
                throw;
            }
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
            => await _cache.GetStringAsync(GetKey(token)) == null;

        public async Task DeactivateAsync(string token)
            => await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(_jwtOptions.Value.ExpiryMinutes)
                });

        private string GetCurrentAsync()
        {
            try
            {
                var authorizationHeader = _httpContextAccessor
                       .HttpContext.Request.Headers[UtilityConstants.CONST_AUTHORIZATION];

                return authorizationHeader == StringValues.Empty
                    ? string.Empty
                    : authorizationHeader.Single().Split(" ").Last();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCurrentAsync of tokenService");
                throw;
            }
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";


        /// <summary>
        /// Validates the provided JWT token using the specified token salt and configuration settings.
        /// </summary>
        /// <param name="token">The JWT token to be validated.</param>
        /// <param name="tokenSalt">The salt used for token decryption.</param>
        /// <param name="configuration">The configuration object.</param>
        /// <returns>
        /// Returns <c>true</c> if the token is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTokenValid(string token, string tokenSalt, IConfiguration configuration)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    Log.Error(MessageConstants.MSG_CONST_TOKEN_MISSING_OR_EMPTY);
                    return false;
                }

                byte[] decryptionKey = GetKeyBytes(
                    configuration[GlobalConstants.CONST_AUTHSETTINGS_SECRET_KEY].ToString(),
                    tokenSalt
                );

                // Replace spaces with '+' for fixed token formatting
                string fixedToken = token.Replace(" ", "+");

                string decryptedToken = DecryptToken(fixedToken, decryptionKey);

                var tokenHandler = new JwtSecurityTokenHandler();

                // Check if the decrypted token is in a valid JWT format
                if (!tokenHandler.CanReadToken(decryptedToken))
                {
                    Log.Error($"{MessageConstants.MSG_CONST_INVALID_TOKEN_FORMAT} {decryptedToken}");
                    return false;
                }

                var validationParameters = HelperFunctions.GetTokenValitionParam(configuration);
                tokenHandler.ValidateToken(decryptedToken, validationParameters, out SecurityToken validatedToken);

                // If no exception is thrown during validation, the token is valid
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"{MessageConstants.MSG_CONST_TOKEN_VALIDATION_FAILD} {ex}");
                return false;
            }
        }

        /// <summary>
        /// Creates a touch otk using the provided TouchTokenDto object.
        /// </summary>
        /// <param name="touchTokenDto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string CreateTouchOtk(TouchTokenDto touchTokenDto)
        {
            if (touchTokenDto == null)
            {
                _logger.LogWarning(MessageConstants.CONST_TOUCH_TOKEN_DTO_NULL);
                throw new ArgumentNullException(nameof(touchTokenDto), MessageConstants.CONST_TOUCH_TOKEN_DTO_NULL);
            }
            _logger.LogTrace(MessageConstants.CONST_TOUCH_OTK_CREATIION, touchTokenDto.UserName);
            string WebAppSecretKey = _configuration[UtilityConstants.CONST_AUTH_SETTINGS_SECRET_KEY];

            // Creating string using dto object
            string dtoString = JsonConvert.SerializeObject(touchTokenDto);
            string encryptedToken = EncryptToken(dtoString, GetKeyBytes(WebAppSecretKey, WebAppSecretKey));

            return encryptedToken;
        }

    }

}