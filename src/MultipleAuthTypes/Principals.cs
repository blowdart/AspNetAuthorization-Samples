using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MultipleAuthTypes
{
    public static class PrincipalFactory
    {
        private const string Issuer = "urn:microsoft.example";
        private const string AuthenticationType = "bearer";

        private static List<ClaimsPrincipal> _principals;

        static PrincipalFactory()
        {
            _principals = new List<ClaimsPrincipal>();

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "adam", ClaimValueTypes.String, Issuer));
            claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(2000, 01, 01).ToString("u"), ClaimValueTypes.DateTime, Issuer));
            claims.Add(new Claim(ClaimTypes.Role, "User", ClaimValueTypes.String, Issuer));
            claims.Add(new Claim("Documents", "CRUD", ClaimValueTypes.String, "urn:microsoft.com"));
            _principals.Add(new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationType)));
            claims.Clear();

            claims.Add(new Claim(ClaimTypes.Name, "barry", ClaimValueTypes.String, Issuer));
            claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(1970, 06, 08).ToString("u"), ClaimValueTypes.DateTime, Issuer));
            claims.Add(new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, Issuer));
            claims.Add(new Claim(ClaimTypes.Role, "User", ClaimValueTypes.String, Issuer));
            claims.Add(new Claim("CanWeFixIt", "YesWeCan", ClaimValueTypes.String, "urn:bobthebuilder.com"));
            claims.Add(new Claim("Documents", "CRUD", ClaimValueTypes.String, "urn:idunno.org"));
            _principals.Add(new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationType)));
            claims.Clear();

            claims.Add(new Claim(ClaimTypes.Name, "charlie", ClaimValueTypes.String, Issuer));
            claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(1990, 01, 01).ToString("u"), ClaimValueTypes.DateTime, Issuer));
            claims.Add(new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, Issuer));
            claims.Add(new Claim("CanWeFixIt", "NoWeCant", ClaimValueTypes.String, "urn:bobthebuilder.com"));
            claims.Add(new Claim("Documents", "R", ClaimValueTypes.String, "urn:idunno.org"));
            _principals.Add(new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationType)));
            claims.Clear();

            claims.Add(new Claim(ClaimTypes.Name, "david", ClaimValueTypes.String, Issuer));            
            claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(1990, 01, 01).ToString("u"), ClaimValueTypes.DateTime, Issuer));
            claims.Add(new Claim(ClaimTypes.Role, "Guest", ClaimValueTypes.String, Issuer));
            _principals.Add(new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationType)));
            claims.Clear();
        }

        public static ClaimsPrincipal Get(string username)
        {
            var principals =
                (from p in _principals
                 where
                     p.Claims.Any(c => c.Type == ClaimTypes.Name && c.Value == username)
                 select p);

            return principals.FirstOrDefault();
        }

        public static IEnumerable<string> UserNames
        {
            get
            {
                return from p in _principals
                       from c in p.Claims
                       where c.Type == ClaimTypes.Name
                       select c.Value;
            }
        }
    }
}