using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CofoundryExample.SSOAndContent
{
    public interface IMemberSignInService
    {
        bool IsAuthenticated(SignMemberInCommand command);

        Task SignMemberInAsync(SignMemberInCommand command);

        void SignOut();
    }
}