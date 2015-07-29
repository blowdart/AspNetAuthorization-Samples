using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleAuthTypes.Models
{
    public class BearerIdentity
    {
        public string[] Names { get; set; }

        public bool IsAuthenticated { get; set; }

        public BearerClaim[] Claims { get; set; }
    }

    public class BearerClaim
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string ClaimIssuer { get; set; }

    }
}
