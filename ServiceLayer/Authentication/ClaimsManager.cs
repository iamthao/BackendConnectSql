using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Interfaces;
using Repositories.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace ServiceLayer.Authentication
{
    public class ClaimsManager : IClaimsManager
    {
        public ClaimsManager(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public IUserRepository UserRepository { get; set; }

        public IEnumerable<Claim> CreateClaims(string username, string password)
        {
            // Construct the claims resulting from entering a username / password:
            // 1) Provider = Username / Password
            var providerClaim = new Claim(ClaimsDeclaration.AuthenticationTypeClaimType,
                ClaimsDeclaration.AuthenticationTypeClaimUsernamePassword);

            // 2) Username Claim
            var nameClaim = new Claim(ClaimsDeclaration.NameClaimType, username);

            // 3) Password Claim
            var passwordClaim = new Claim(ClaimsDeclaration.PasswordClaim, password);

            var claims = new List<Claim> { nameClaim, providerClaim, passwordClaim };

            return claims;
        }

        public User ValidateQuickspatchUserLogin(List<Claim> claimset)
        {
            var user = new User();
            // determine, wether we want to evaluate username/password
            if (claimset.Any(x => ((x.Type == ClaimsDeclaration.AuthenticationTypeClaimType) &&
                                   (x.Value ==
                                    ClaimsDeclaration.AuthenticationTypeClaimUsernamePassword))))
            {
                var passwordClaim = from claim in claimset
                    where
                        claim.Type == ClaimsDeclaration.PasswordClaim
                    select claim.Value;

                var nameClaim = (from claim in claimset
                    where claim.Type == ClaimsDeclaration.NameClaimType
                    select claim.Value).SingleOrDefault();

                user.UserName = nameClaim;

                // when validated successfully, remove password claims from the claimset for security
                var userLogin = UserRepository.GetUserByUserNameAndPass(nameClaim, passwordClaim.FirstOrDefault());
                if (userLogin == null)
                {
                    return user;
                }
                var passwordClaims =
                    claimset.Where(x => x.Type == ClaimsDeclaration.PasswordClaim).ToList();

                passwordClaims.ForEach(x => claimset.Remove(x)); // remove password for safely.
                user = userLogin;
                user.IsQuickspatchUser = true;
                return user;
            }
            return user;
        }
    }
}