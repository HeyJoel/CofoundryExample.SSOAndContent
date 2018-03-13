using Cofoundry.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CofoundryExample.SSOAndContent
{
    /// <summary>
    /// For more info see https://github.com/cofoundry-cms/cofoundry/wiki/User-Areas
    /// </summary>
    public class MemberUserArea : IUserAreaDefinition
    {
        /// <summary>
        /// Static access to the area code for querying
        /// </summary>
        public const string AreaCode = "MEM";

        /// <summary>
        /// A unique 3 letter code identifying this user area. The cofoundry 
        /// user are uses the code "COF" so you can pick anything else!
        /// </summary>
        public string UserAreaCode { get { return AreaCode; } }

        /// <summary>
        /// Disallow password logins because we're going to use an alternative auth mechanism
        /// </summary>
        public bool AllowPasswordLogin { get { return false; } }

        public string Name { get { return "Member"; } }

        /// <summary>
        /// I'll assume you have an email from your SSO provider but
        /// otherwise this can be false.
        /// </summary>
        public bool UseEmailAsUsername { get { return true; } }

        /// <summary>
        /// The path to redirect the user to when not lgoged in.
        /// </summary>
        public string LoginPath => "/auth/signin";

        /// <summary>
        /// Cofoundry creates an auth schema for each user area. Use this property to set
        /// this user area as the default auth schema, which means the HttpContext.User property
        /// will be set to this identity.
        /// </summary>
        public bool IsDefaultAuthSchema => true;
    }
}