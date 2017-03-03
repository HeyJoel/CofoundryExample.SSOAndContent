using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using Cofoundry.Domain.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CofoundryExample.SSOAndContent
{
    /// <summary>
    /// Note that we haven't created a repository facade for User data access,
    /// and some of the data access functions are not there yet so this is a little
    /// more complicated than it needs to be, but i'll add them soon.
    /// </summary>
    public class MemberSignInService : IMemberSignInService
    {
        #region constructor

        private readonly CofoundryDbContext _dbContext;
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IExecutionContextFactory _executionContextFactory;
        private readonly ILoginService _loginService;

        public MemberSignInService(
            CofoundryDbContext dbContext,
            IQueryExecutor queryExecutor,
            ICommandExecutor commandExecutor,
            ILoginService loginService,
            IExecutionContextFactory executionContextFactory
            )
        {
            _dbContext = dbContext;
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
            _loginService = loginService;
            _executionContextFactory = executionContextFactory;
        }

        #endregion

        #region public implementation

        public bool IsAuthenticated(SignMemberInCommand command)
        {
            // TODO: Add in your own auth here

            return true;
        }

        public async Task SignMemberInAsync(SignMemberInCommand command)
        {
            // Because we're not logged in yet we need to elevate permissions here to save a new user,
            // otherwise the ambient anonymous user will be used and a permission exception will be thrown
            var systemExecutionContext = await _executionContextFactory.CreateSystemUserExecutionContextAsync();

            var existingUser = GetExistingUser(command, systemExecutionContext);
            int userId;

            if (existingUser == null)
            {
                // If we haven't logged in with this user before, we'll create a Cofoundry user to match your
                // SSO login.

                int roleId = await GetMemberRoleId();
                var addUserCommand = MapAddUserCommand(command, roleId);

                await _commandExecutor.ExecuteAsync(addUserCommand, systemExecutionContext);

                // Note that the new user id is set in the OutputUserId which is a 
                // convention used by the CQS framework (see https://github.com/cofoundry-cms/cofoundry/wiki/CQS)
                userId = addUserCommand.OutputUserId;
            }
            else
            {
                // If the user already exists, we sign in using that Id
                userId = existingUser.UserId;
            }

            await _loginService.LogAuthenticatedUserInAsync(userId, true);
        }

        public void SignOut()
        {
            // TODO: Add in your own sign out logic here

            _loginService.SignOut();
        }

        #endregion

        #region private helpers

        private UserMicroSummary GetExistingUser(SignMemberInCommand command, IExecutionContext systemExecutionContext)
        {
            var query = new GetUserMicroSummaryByEmailQuery(command.Email, MemberUserArea.AreaCode);
            // Currently the async version of this query is missing (https://github.com/cofoundry-cms/cofoundry/issues/76)
            var existingMember = _queryExecutor.Execute(query, systemExecutionContext);

            return existingMember;
        }

        /// <summary>
        /// Every user needs to be assigned a role. We've created a MemberRole in 
        /// code, so we can our code definition to find out the database id which 
        /// we need to create the new user
        /// </summary>
        private async Task<int> GetMemberRoleId()
        {
            return await _dbContext
                .Roles
                .AsNoTracking()
                .Where(r => r.SpecialistRoleTypeCode == MemberRole.RoleCode && r.UserAreaCode == MemberUserArea.AreaCode)
                .Select(r => r.RoleId)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// We're going to make use of the built in AddUserCommand which will take 
        /// care of most of the user creation logic for us. Here we map from our 
        /// domain command to the Cofoundry one.
        /// </summary>
        private AddUserCommand MapAddUserCommand(SignMemberInCommand command, int roleId)
        {
            var addUserCommand = new AddUserCommand();
            addUserCommand.Email = command.Email;
            addUserCommand.FirstName = "Unknown";
            addUserCommand.LastName = "Unknown";
            addUserCommand.RoleId = roleId;
            addUserCommand.UserAreaCode = MemberUserArea.AreaCode;

            return addUserCommand;
        }

        #endregion
    }
}