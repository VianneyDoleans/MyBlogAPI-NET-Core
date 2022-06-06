﻿using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;
using MyBlogAPI.Authorization.Permissions;
using MyBlogAPI.DTOs.Permission;
using MyBlogAPI.Services.RoleService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Authorization.PermissionHandlers.Resources
{
    public class HasOwnOrAllPermissionRangeForUserResourceAuthorizationHandler : AuthorizationHandler<PermissionRequirement, User>
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public HasOwnOrAllPermissionRangeForUserResourceAuthorizationHandler(IUserService userService, IRoleService roleService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement, User entity)
        {
            var userId = int.Parse(context.User.Claims
                .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);

            var user = await _userService.GetUser(userId);
            if (user.Roles.Any())
            {
                var requirementAction = _mapper.Map<PermissionActionDto>(requirement.Permission);
                var requirementTarget = _mapper.Map<PermissionTargetDto>(requirement.PermissionTarget);

                foreach (var role in user.Roles)
                {
                    var permissions = await _roleService.GetPermissionsAsync(role);

                    if (permissions != null && permissions.Any(permission =>
                            requirementAction.Id == permission.PermissionAction.Id &&
                            requirementTarget.Id == permission.PermissionTarget.Id &&
                            ((permission.PermissionRange.Id == (int)PermissionRange.Own && entity.Id == userId)
                             || permission.PermissionRange.Id == (int)PermissionRange.All)))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
        }
    }
}
