using Cofoundry.Core.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CofoundryExample.SSOAndContent
{
    /// <summary>
    /// Iv'e created some custom services for this sample so we need to 
    /// register them with the DI container using an IDependencyRegistration 
    /// implementation. 
    /// 
    /// You can have as many of these as you want and locate them anywhere in 
    /// the solution if you want to keep your code modular. The DI container will 
    /// always scan for them and register them automatically.
    /// </summary>
    public class DependencyRegistration : IDependencyRegistration
    {
        public void Register(IContainerRegister container)
        {
            container.RegisterType<IContentRepository, ContentRepository>();
            container.RegisterType<IMemberSignInService, MemberSignInService>();
        }
    }
}