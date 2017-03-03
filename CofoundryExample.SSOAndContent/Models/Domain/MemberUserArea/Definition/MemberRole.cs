﻿using Cofoundry.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CofoundryExample.SSOAndContent
{
    /// <summary>
    /// Roles can be defined in code as well as in the admin panel. Defining
    /// a role in code means that it gets added automatically at startup. 
    /// Additionally we have a SpecialistRoleTypeCode that we can use to query 
    /// the role programatically.
    /// 
    /// See https://github.com/cofoundry-cms/cofoundry/wiki/Roles-&-Permissions
    /// </summary>
    public class MemberRole : IRoleDefinition
    {
        public const string RoleCode = "MEM";

        /// <summary>
        /// The role title is used to identify the role and select it in the admin 
        /// UI and therefore must be unique. Max 50 characters.
        /// </summary>
        public string Title { get { return "Member"; } }

        /// <summary>
        /// The specialist role type code is a unique three letter code that
        /// can be used to reference the role programatically. The code must be unique
        /// and convention is to use upper case, although code matching is case insensitive.
        /// </summary>
        public string SpecialistRoleTypeCode { get { return RoleCode; } }

        /// <summary>
        /// A role must be assigned to a user area, in this case the role is used for members
        /// </summary
        public string UserAreaCode { get { return MemberUserArea.AreaCode; } }
    }
}