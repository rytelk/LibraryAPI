using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Library.FunctionalTests.Infrastructure
{
    public class TestClaimsProvider
    {
        public IList<Claim> Claims { get; }

        public TestClaimsProvider(IList<Claim> claims)
        {
            Claims = claims;
        }

        public TestClaimsProvider()
        {
            Claims = new List<Claim>();
        }

        public static TestClaimsProvider WithLibrarianClaims()
        {
            var provider = new TestClaimsProvider();

            provider.Claims.Add(new Claim(ClaimTypes.Name, "1"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, "Librarian"));

            return provider;
        }

        public static TestClaimsProvider WithReaderClaims()
        {
            var provider = new TestClaimsProvider();

            provider.Claims.Add(new Claim(ClaimTypes.Name, "2"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, "Reader"));

            return provider;
        }
    }
}