using System;
using System.Collections.Generic;
using System.Security.Claims;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Interfaces;

namespace ServiceLayer.Authentication
{
    public class QuickspatchPrincipal : ClaimsPrincipal, IQuickspatchPrincipal
    {
        private QuickspatchIdentity _currentIdentity;
        private List<ClaimsIdentity> _identities;

        public QuickspatchPrincipal()
        {
        }

        public QuickspatchPrincipal(ClaimsIdentity identity)
            : base(identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            // Transform the ClaimsIndentity to an QuickspatchIdentity that extends
            // ClaimsIdentity with application-specific behavior.
            var advantageIdentity = new QuickspatchIdentity(identity.Claims);

            _currentIdentity = advantageIdentity;
            _identities = new List<ClaimsIdentity> { advantageIdentity };
        }

        public QuickspatchPrincipal(QuickspatchIdentity identity)
            : base(identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            _currentIdentity = identity;
            _identities = new List<ClaimsIdentity> { _currentIdentity };
        }

        public new IEnumerable<ClaimsIdentity> Identities
        {
            get { return _identities; }
        }

        public string AuthToken { get; set; }

        public User User { get; set; }

        public new QuickspatchIdentity Identity
        {
            get { return _currentIdentity; }
            set { _currentIdentity = value; }
        }
    }
}