using Cofoundry.Domain;
using System.Threading.Tasks;

namespace CofoundryExample.SSOAndContent
{
    /// <summary>
    /// Here we use IAdvancedContentRepository for data access. This is a fluent API
    /// wrapper around command and query execution to aid discoverability and ease of use.
    /// </summary>
    public class MemberSignInService
    {
        private readonly IAdvancedContentRepository _advancedContentRepository;

        public MemberSignInService(
            IAdvancedContentRepository advancedContentRepository
            )
        {
            _advancedContentRepository = advancedContentRepository;
        }

        public bool IsAuthenticated(SignMemberInCommand command)
        {
            // TODO: Add in your own auth integration here

            return true;
        }

        /// <summary>
        /// Sign in a user that has already been authenticated.
        /// </summary>
        public async Task SignMemberInAsync(SignMemberInCommand command)
        {
            // Because the user is not signed in yet we need to elevate permissions,
            // otherwise the anonymous user will be used and a permission exception will be thrown
            var existingUser = await _advancedContentRepository
                .WithElevatedPermissions()
                .Users()
                .GetByUsername<MemberUserArea>(command.Username)
                .AsMicroSummary()
                .ExecuteAsync();

            int userId;

            if (existingUser == null)
            {
                // If we haven't signed in with this user before, we'll provision a new user account
                // to match the SSO user account.
                userId = await _advancedContentRepository
                    .WithElevatedPermissions()
                    .Users()
                    .AddAsync(new AddUserCommand()
                    {
                        UserAreaCode = MemberUserArea.Code,
                        RoleCode = MemberRole.Code,
                        Username = command.Username
                    });
            }
            else
            {
                // If the user already exists, we sign in using that UserId
                userId = existingUser.UserId;
            }

            await _advancedContentRepository
                .Users()
                .Authentication()
                .SignInAuthenticatedUserAsync(new SignInAuthenticatedUserCommand()
                {
                    UserId = userId,
                    RememberUser = true
                });
        }

        public async Task SignOutAsync()
        {
            // TODO: Add in your own sign out logic here

            await _advancedContentRepository
                .Users()
                .Authentication()
                .SignOutAsync();
        }
    }
}