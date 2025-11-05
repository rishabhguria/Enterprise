using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.UnitTest.Commons;
using Prana.ServiceGateway.Utility;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class TokenServiceTest : BaseControllerTest
    {
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IOptions<JwtOptions>> _mockJwtOptions;
        private readonly Mock<ILogger<TokenService>> _mockLogger;
        private readonly TokenService _tokenService;

        // Secret key and salt for consistent testing
        private const string TestSecretKey = "thisisalongandsecurekeyforjwttokencreationtest"; // Must be at least 32 bytes for Aes-256
        private const string TestSalt = "thisisalongandsaltfortokenencryption";
        private const string TestIssuer = "testIssuer";
        private const string TestAudience = "testAudience";

        public TokenServiceTest()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockCache = new Mock<IDistributedCache>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockJwtOptions = new Mock<IOptions<JwtOptions>>();
            _mockLogger = new Mock<ILogger<TokenService>>();

            // Setup common configuration values
            _mockConfiguration.Setup(c => c[UtilityConstants.CONST_AUTH_SETTINGS_SECRET_KEY]).Returns(TestSecretKey);
            _mockConfiguration.Setup(c => c[GlobalConstants.CONST_AUTHSETTINGS_SECRET_KEY]).Returns(TestSecretKey);
            _mockConfiguration.Setup(c => c[GlobalConstants.CONST_AUTHSETTINGS_SALT]).Returns(TestSalt);
            _mockConfiguration.Setup(c => c[UtilityConstants.CONST_AUTH_SETTINGS_ISSUER]).Returns(TestIssuer);
            _mockConfiguration.Setup(c => c[UtilityConstants.CONST_AUTH_SETTINGS_AUDIENCE]).Returns(TestAudience);

            // Setup JwtOptions
            _mockJwtOptions.Setup(o => o.Value).Returns(new JwtOptions { ExpiryMinutes = 5 }); // Set a short expiry for cache testing

            // Setup HttpContextAccessor to return a default HttpContext
            _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext());

            // Configure Serilog.Log to avoid null reference issues in static methods
            // In a real application, Serilog would be configured globally.
            // For testing, we just prevent it from throwing.
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() // or any other sink that discards output
                .CreateLogger();

            _tokenService = new TokenService(
                _mockConfiguration.Object,
                _mockCache.Object,
                _mockHttpContextAccessor.Object,
                _mockJwtOptions.Object,
                _mockLogger.Object);
        }

        // Test cases for CreateToken
        [Fact]
        public void CreateToken_ValidUserDto_ReturnsEncryptedToken()
        {
            // Arrange
            var userDto = new UserDto
            {
                CompanyUserId = 123,
                IsAdmin = true,
                IsSupport = false,
                UserName = "testuser"
            };

            // Act
            var token = _tokenService.CreateToken(userDto);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public void GetUserDtoFromTokenClain_NullClaimIdentity_ThrowsException()
        {
            // Arrange
            ClaimsIdentity claimIdentity = null;

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _tokenService.GetUserDtoFromTokenClain(claimIdentity));
            Assert.Equal(MessageConstants.MSG_CONST_CLAIM_IDENTITY_NULL_CALL_GENERATE_API, ex.Message);
        }

        [Fact]
        public void GetUserDtoFromTokenClain_NoClaims_ThrowsException()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _tokenService.GetUserDtoFromTokenClain(claimsIdentity));
            Assert.Equal(MessageConstants.MSG_CONST_NO_CLAIM_FOUND_GENERATE_API_AND_TRY_AGAIN, ex.Message);
        }

        [Fact]
        public void GetUserDtoFromTokenClain_MissingRequiredClaim_ThrowsInvalidOperationException()
        {
            // Arrange (missing ADMIN claim)
            var claims = new Claim[]
            {
            new Claim(ApiContants.SUPPORT, "False"),
            new Claim(ApiContants.COMPANY_USER_ID, "456"),
            new Claim(ApiContants.USER_NAME, "jane.doe")
            };
            var claimsIdentity = new ClaimsIdentity(claims);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _tokenService.GetUserDtoFromTokenClain(claimsIdentity));
        }

        [Fact]
        public void GetUserDtoFromTokenClain_InvalidCompanyUserIdFormat_ThrowsFormatException()
        {
            // Arrange
            var claims = new Claim[]
            {
            new Claim(ApiContants.ADMIN, "True"),
            new Claim(ApiContants.SUPPORT, "False"),
            new Claim(ApiContants.COMPANY_USER_ID, "not-an-int"), // Invalid format
            new Claim(ApiContants.USER_NAME, "jane.doe")
            };
            var claimsIdentity = new ClaimsIdentity(claims);

            // Act & Assert
            Assert.Throws<FormatException>(() => _tokenService.GetUserDtoFromTokenClain(claimsIdentity));
        }

        // Test cases for EncryptToken (static)
        [Fact]
        public void EncryptToken_ValidTokenAndKey_ReturnsEncryptedString()
        {
            // Arrange
            var token = "mySecretToken123";
            var key = TokenService.GetKeyBytes(TestSecretKey, TestSalt);

            // Act
            var encryptedToken = TokenService.EncryptToken(token, key);

            // Assert
            Assert.False(string.IsNullOrEmpty(encryptedToken));
            Assert.NotEqual(token, encryptedToken);
        }

        [Fact]
        public void EncryptToken_EmptyToken_ReturnsEncryptedEmptyString()
        {
            // Arrange
            var token = "";
            var key = TokenService.GetKeyBytes(TestSecretKey, TestSalt);

            // Act
            var encryptedToken = TokenService.EncryptToken(token, key);

            // Assert
            Assert.False(string.IsNullOrEmpty(encryptedToken)); // It will still produce a base64 string
        }

        [Fact]
        public void EncryptToken_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            var token = "mySecretToken";
            byte[] key = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => TokenService.EncryptToken(token, key));
        }


        // Test cases for DecryptToken (static)
        [Fact]
        public void DecryptToken_ValidEncryptedTokenAndKey_ReturnsOriginalToken()
        {
            // Arrange
            var originalToken = "This is a test token to be encrypted and then decrypted.";
            var key = TokenService.GetKeyBytes(TestSecretKey, TestSalt);
            var encryptedToken = TokenService.EncryptToken(originalToken, key);

            // Act
            var decryptedToken = TokenService.DecryptToken(encryptedToken, key);

            // Assert
            Assert.Equal(originalToken, decryptedToken);
        }

        [Fact]
        public void DecryptToken_NullToken_ThrowsArgumentNullException()
        {
            // Arrange
            string token = null;
            var key = TokenService.GetKeyBytes(TestSecretKey, TestSalt);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => TokenService.DecryptToken(token, key));
            Assert.Contains("Access token is null or empty", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("null")] // Specific check for "null" string
        public void DecryptToken_EmptyOrNullStringToken_ThrowsArgumentNullException(string tokenValue)
        {
            // Arrange
            var key = TokenService.GetKeyBytes(TestSecretKey, TestSalt);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => TokenService.DecryptToken(tokenValue, key));
            Assert.Contains("Access token is null or empty", ex.Message);
        }


        [Fact]
        public void DecryptToken_InvalidBase64Token_ThrowsFormatException()
        {
            // Arrange
            var invalidToken = "not-a-valid-base64-string!";
            var key = TokenService.GetKeyBytes(TestSecretKey, TestSalt);

            // Act & Assert
            Assert.Throws<FormatException>(() => TokenService.DecryptToken(invalidToken, key));
        }

        [Fact]
        public void DecryptToken_IncorrectKey_ThrowsCryptographicException()
        {
            // Arrange
            var originalToken = "someToken";
            var correctKey = TokenService.GetKeyBytes(TestSecretKey, TestSalt);
            var encryptedToken = TokenService.EncryptToken(originalToken, correctKey);

            var incorrectKey = TokenService.GetKeyBytes("wrongkey", TestSalt); // Different key

            // Act & Assert
            Assert.Throws<System.Security.Cryptography.CryptographicException>(() => TokenService.DecryptToken(encryptedToken, incorrectKey));
        }

        // Test cases for GetKeyBytes (static)
        [Fact]
        public void GetKeyBytes_ValidKeyAndSalt_Returns32ByteKey()
        {
            // Arrange
            var key = "MyEncryptionKey";
            var salt = "MySaltForHashing";

            // Act
            var keyBytes = TokenService.GetKeyBytes(key, salt);

            // Assert
            Assert.NotNull(keyBytes);
            Assert.Equal(32, keyBytes.Length); // AES-256 requires 32 bytes (256 bits)
        }

        [Fact]
        public void GetKeyBytes_EmptyKeyOrSalt_Returns32ByteKey()
        {
            // Arrange
            var key = "";
            var salt = "";

            // Act
            var keyBytes = TokenService.GetKeyBytes(key, salt);

            // Assert
            Assert.NotNull(keyBytes);
            Assert.Equal(32, keyBytes.Length);
        }

        [Fact]
        public void GetKeyBytes_DifferentSalt_ReturnsDifferentKey()
        {
            // Arrange
            var key = "MyEncryptionKey";
            var salt1 = "MySaltForHashing1";
            var salt2 = "MySaltForHashing2";

            // Act
            var keyBytes1 = TokenService.GetKeyBytes(key, salt1);
            var keyBytes2 = TokenService.GetKeyBytes(key, salt2);

            // Assert
            Assert.NotEqual(keyBytes1, keyBytes2);
        }

        // Test cases for CreateTouchOtk
        [Fact]
        public void CreateTouchOtk_ValidTouchTokenDto_ReturnsEncryptedString()
        {
            // Arrange
            var touchTokenDto = new TouchTokenDto("touchUser");
            // Mock the configuration for the secret key again if needed, ensure it's sufficient for encryption
            _mockConfiguration.Setup(c => c[UtilityConstants.CONST_AUTH_SETTINGS_SECRET_KEY]).Returns(TestSecretKey);

            // Act
            var encryptedOtk = _tokenService.CreateTouchOtk(touchTokenDto);

            // Assert
            Assert.False(string.IsNullOrEmpty(encryptedOtk));
            // Verify it's decryptable
            var decryptedJson = TokenService.DecryptToken(encryptedOtk, TokenService.GetKeyBytes(TestSecretKey, TestSecretKey));
            var deserializedDto = JsonConvert.DeserializeObject<TouchTokenDto>(decryptedJson);
            Assert.Equal(touchTokenDto.UserName, deserializedDto.UserName);
        }

        [Fact]
        public void CreateTouchOtk_NullTouchTokenDto_ThrowsArgumentNullException()
        {
            // Arrange
            TouchTokenDto touchTokenDto = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _tokenService.CreateTouchOtk(touchTokenDto));
            Assert.Equal("touchTokenDto", ex.ParamName);
            Assert.Contains(MessageConstants.CONST_TOUCH_TOKEN_DTO_NULL, ex.Message);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(MessageConstants.CONST_TOUCH_TOKEN_DTO_NULL)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
