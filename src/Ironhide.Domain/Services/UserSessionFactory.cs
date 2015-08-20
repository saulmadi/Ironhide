using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain.Services
{
    public class UserSessionFactory : IUserSessionFactory
    {
        readonly ITimeProvider _timeProvider;
        readonly ITokenExpirationProvider _tokenExpirationProvider;
        readonly IIdentityGenerator<Guid> _identityGenerator;
        readonly IWriteableRepository _writeableRepository;
        readonly IKeyProvider _keyProvider;

        public UserSessionFactory(IWriteableRepository writeableRepository,
                                  ITimeProvider timeProvider,
                                  IIdentityGenerator<Guid> identityGenerator,
                                  ITokenExpirationProvider tokenExpirationProvider,
                                  IKeyProvider keyProvider)
        {
            _writeableRepository = writeableRepository;
            _timeProvider = timeProvider;
            _identityGenerator = identityGenerator;
            _tokenExpirationProvider = tokenExpirationProvider;
            _keyProvider = keyProvider;
        }

        #region IUserSessionFactory Members

        public UserLoginSession Create(User executor)
        {
            var id = _identityGenerator.Generate();

            var now = _timeProvider.Now();
            DateTime expirationDateTime = _tokenExpirationProvider.GetExpiration(now);

            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricKey = _keyProvider.GetKey();

            var claims = executor.UserRoles.Select(x => new Claim(ClaimTypes.Role, x.Description)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, executor.Name));
            claims.Add(new Claim("jti", id.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                TokenIssuerName = "self",
                AppliesToAddress = "http://www.example.com",
                Lifetime = new Lifetime(now, expirationDateTime),
                SigningCredentials = new SigningCredentials(
                    new InMemorySymmetricSecurityKey(symmetricKey),
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256"),


            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            var userSession = new UserLoginSession(id, executor, expirationDateTime, tokenString);

            _writeableRepository.Create(userSession);

            return userSession;
        }

        #endregion
    }
}