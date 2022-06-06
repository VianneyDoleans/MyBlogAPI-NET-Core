﻿using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MyBlogAPI.Authorization.Permissions;
using MyBlogAPI.DTOs.Permission;
using MyBlogAPI.Services.RoleService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Authorization.PermissionHandlers.Attributes
{
    public class PermissionWithRangeAuthorizationHandler : AuthorizationHandler<PermissionWithRangeRequirement>
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public PermissionWithRangeAuthorizationHandler(IUserService userService, IRoleService roleService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var req in context.Requirements.OfType<PermissionWithRangeRequirement>())
            {
                await HandleRequirementAsync(context, req);
            }
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionWithRangeRequirement withRangeRequirement)
        {
            var userId = context.User.Claims
                .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            var user = await _userService.GetUser(int.Parse(userId));
            if (user.Roles.Any())
            {
                var requirementAction = _mapper.Map<PermissionActionDto>(withRangeRequirement.Permission);
                var requirementTarget = _mapper.Map<PermissionTargetDto>(withRangeRequirement.PermissionTarget);
                var requirementRange = _mapper.Map<PermissionRangeDto>(withRangeRequirement.PermissionRange);

                foreach (var role in user.Roles)
                {
                    var permissions = await _roleService.GetPermissionsAsync(role);

                    if (permissions != null && permissions.Any(permission =>
                            requirementAction.Id == permission.PermissionAction.Id &&
                            requirementTarget.Id == permission.PermissionTarget.Id &&
                            requirementRange.Id == permission.PermissionRange.Id))
                    {
                        context.Succeed(withRangeRequirement);
                        return;
                    }
                }
            }
        }
    }
}
