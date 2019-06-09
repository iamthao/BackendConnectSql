using System.Collections.Generic;
using System.Security.Claims;
using Framework.DomainModel.Entities;

namespace ServiceLayer.Interfaces.Authentication
{
    public interface IClaimsManager
    {
        IEnumerable<Claim> CreateClaims(string username, string password);
        User ValidateQuickspatchUserLogin(List<Claim> claimset);
    }
}