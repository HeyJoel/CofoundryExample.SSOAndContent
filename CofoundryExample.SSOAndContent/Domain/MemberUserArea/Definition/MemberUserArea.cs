using Cofoundry.Domain;

namespace CofoundryExample.SSOAndContent
{
    /// <summary>
    /// For more info see https://github.com/cofoundry-cms/cofoundry/wiki/User-Areas
    /// </summary>
    public class MemberUserArea : IUserAreaDefinition
    {
        /// <summary>
        /// By convention we add a constant for the user area code
        /// to make it easier to reference.
        /// </summary>
        public const string Code = "MEM";

        /// <summary>
        /// A unique 3 letter code identifying this user area. The cofoundry 
        /// user are uses the code "COF" so you can pick anything else!
        /// </summary>
        public string UserAreaCode => Code;

        /// <summary>
        /// Disallow password logins because we're going to use an alternative auth mechanism
        /// </summary>
        public bool AllowPasswordSignIn => false;

        /// <summary>
        /// Display name of the area, used in the Cofoundry admin panel
        /// as the navigation link to manage your users. This should be singular
        /// because "Users" is appended to the link text.
        /// </summary>
        public string Name => "Member";

        /// <summary>
        /// We could set this to true if our SSO provider used an email address as 
        /// the username identifier.
        /// </summary>
        public bool UseEmailAsUsername => false;

        /// <summary>
        /// The path to redirect the user to when not signed in.
        /// </summary>
        public string SignInPath => "/auth/signin";

        /// <summary>
        /// Cofoundry creates an auth scheme for each user area, but only one can be the 
        /// default. In an application with multiple user areas a client can be signed in
        /// to multiple users at the same time, so the default scheme dictates which user
        /// area is authenticated by default when querying the "current user" and is therefore
        /// also used for checking permissions. It's rare that a site would implement more 
        /// than one custom user area, so in most cases this should be set to <see langword="true"/>.
        /// </summary>
        public bool IsDefaultAuthScheme => true;

        public void ConfigureOptions(UserAreaOptions options)
        {
            // No advanced config
        }
    }
}