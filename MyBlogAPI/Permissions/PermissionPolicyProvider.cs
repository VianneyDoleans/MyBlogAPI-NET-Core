﻿using System;
using System.Threading.Tasks;
using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MyBlogAPI.Attributes;

namespace MyBlogAPI.Permissions
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options.Value;
        }

        private static PermissionWithRangeRequirement GetPermissionWithRangeRequirement(string policyName)
        {
            if (policyName.StartsWith("permission."))
            {
                var values = policyName.Split('.');
                if (values.Length == 4)
                {
                    Enum.TryParse(values[1], out PermissionAction permissionAction);
                    Enum.TryParse(values[2], out PermissionTarget permissionTarget);
                    Enum.TryParse(values[3], out PermissionRange permissionRange);
                    var permissionRequirement = new PermissionWithRangeRequirement(permissionAction, permissionTarget, permissionRange);
                    return permissionRequirement;
                }
            }
            return null;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            return await base.GetPolicyAsync(policyName)
                   ?? new AuthorizationPolicyBuilder()
                       .AddRequirements(GetPermissionWithRangeRequirement(policyName)).Build();
        }
    }
}
