using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CofoundryExample.SSOAndContent
{
    /// <summary>
    /// Here we use the repositories for data access. This are simple wrappers
    /// around command and query execution which are a bit simpler
    /// </summary>
    public class MemberSignInService
    {
        #region constructor

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IExecutionContextFactory _executionContextFactory;
        private readonly ILoginService _loginService;

        public MemberSignInService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ILoginService loginService,
            IExecutionContextFactory executionContextFactory
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _loginService = loginService;
            _executionContextFactory = executionContextFactory;
        }

        #endregion

        #region public implementation

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
            // Because we're not logged in yet we need to elevate permissions here to save a new user,
            // otherwise the ambient anonymous user will be used and a permission exception will be thrown
            var systemExecutionContext = await _executionContextFactory.CreateSystemUserExecutionContextAsync();

            var existingUser = await _userRepository.GetUserMicroSummaryByEmailAsync(command.Email, MemberUserArea.AreaCode, systemExecutionContext);
            int userId;

            if (existingUser == null)
            {
                // If we haven't logged in with this user before, we'll create a Cofoundry user to match your
                // SSO login.

                var role = await GetMemberRole(systemExecutionContext);
                var addUserCommand = MapAddUserCommand(command, role);

                await _userRepository.AddUserAsync(addUserCommand, systemExecutionContext);

                // Note that the new user id is set in the OutputUserId which is a 
                // convention used by the CQS framework (see https://github.com/cofoundry-cms/cofoundry/wiki/CQS)
                userId = addUserCommand.OutputUserId;
            }
            else
            {
                // If the user already exists, we sign in using that Id
                userId = existingUser.UserId;
            }

            await _loginService.LogAuthenticatedUserInAsync(MemberUserArea.AreaCode, userId, true);
        }

        public Task SignOutAsync()
        {
            // TODO: Add in your own sign out logic here

            return _loginService.SignOutAsync(MemberUserArea.AreaCode);
        }

        #endregion

        #region private helpers

        /// <summary>
        /// Every user needs to be assigned a role. We've created a MemberRole in 
        /// code, so we can use our code definition to get the role we need to create 
        /// the new user with
        /// </summary>
        private Task<RoleDetails> GetMemberRole(IExecutionContext executionContext)
        {
            return _roleRepository.GetRoleDetailsByRoleCodeAsync(MemberRole.MemberRoleCode, executionContext);
        }

        /// <summary>
        /// We're going to make use of the built in AddUserCommand which will take 
        /// care of most of the user creation logic for us. Here we map from our 
        /// domain command to the Cofoundry one.
        /// </summary>
        private AddUserCommand MapAddUserCommand(SignMemberInCommand command, RoleDetails role)
        {
            var addUserCommand = new AddUserCommand();
            addUserCommand.Email = command.Email;
            addUserCommand.FirstName = "Unknown";
            addUserCommand.LastName = "Unknown";
            addUserCommand.RoleId = role.RoleId;
            addUserCommand.UserAreaCode = MemberUserArea.AreaCode;

            return addUserCommand;
        }

        #endregion
    }
}