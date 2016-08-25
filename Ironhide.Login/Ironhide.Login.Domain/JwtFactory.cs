using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using Ironhide.Common;
using Ironhide.Login.Domain.Entities;
using Ironhide.Login.Domain.Services;

namespace Ironhide.Login.Domain
{
    public class JwtFactory : ITokenFactory
    {
        readonly IIdentityGenerator<Guid> _identityGenerator;
        readonly IKeyProvider _keyProvider;
        readonly ITimeProvider _timeProvider;
        readonly ITokenExpirationProvider _tokenExpirationProvider;

        public JwtFactory(ITimeProvider timeProvider,
            IIdentityGenerator<Guid> identityGenerator,
            ITokenExpirationProvider tokenExpirationProvider,
            IKeyProvider keyProvider)
        {
            _timeProvider = timeProvider;
            _identityGenerator = identityGenerator;
            _tokenExpirationProvider = tokenExpirationProvider;
            _keyProvider = keyProvider;
        }

        #region ITokenFactory Members

        public string Create(User executor)
        {
            Guid id = _identityGenerator.Generate();

            DateTime now = _timeProvider.Now();
            DateTime expirationDateTime = _tokenExpirationProvider.GetExpiration(now);

            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] symmetricKey = _keyProvider.GetKey();

            List<Claim> claims = executor.UserRoles.Select(x => new Claim(ClaimTypes.Role, x.Description)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, executor.Name));
            claims.Add(new Claim("userguid", executor.Id.ToString()));

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

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        #endregion
    }
}