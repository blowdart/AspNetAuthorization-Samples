using System;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace PolicyProvider
{
    public class SamplePolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        // We're taking AuthorizationOptions so we can fall through to 
        // policies configured during startup. If all your policies are going
        // to be dynamic then you don't need to do this.
        //
        // Alternatively you could derive from DefaultAuthorizationPolicyProvider
        // and call base.GetPolicyAsync().
        public SamplePolicyProvider(IOptions<AuthorizationOptions> options)
        {          
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Value;
        }

        /// <summary>
        /// Gets the default authorization policy.
        /// </summary>
        /// <returns>The default authorization policy.</returns>
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(_options.DefaultPolicy);
        }

        // And this is where the magic happens.
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // Check to see if there's a configured policy with the name.
            var _policy = _options.GetPolicy(policyName);

            if (_policy == null)
            {
                // There's no matching policy name in the policies configured in start
                // So let's do our thing.

                // For our sample we have the following policy name format,
                // ClaimName|ExpectedValue and we assume that neither the claim name nor the 
                // value have a | character in them, because it's only a sample

                var policyDetails = policyName.Split('|');

                if (policyDetails.Length != 2)
                {
                    return Task.FromResult((AuthorizationPolicy)null);
                }

                // Now we can create our custom requirements with the right parameters
                // then build a policy around them, and finally return that policy for evaluation.

                var requirements = new IAuthorizationRequirement[1];
                requirements[0] = new CustomAuthorizationRequirement { ClaimType = policyDetails[0], ClaimValue = policyDetails[1] };

                return Task.FromResult(new AuthorizationPolicyBuilder().AddRequirements(requirements).Build());

            }

            return Task.FromResult(_policy);
        }
    }
}
