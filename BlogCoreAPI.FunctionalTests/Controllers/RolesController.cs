﻿using System;
using System.Threading.Tasks;
using BlogCoreAPI.DTOs.Role;
using BlogCoreAPI.FunctionalTests.GenericTests;
using BlogCoreAPI.FunctionalTests.Helpers;
using BlogCoreAPI.Models.DTOs.Role;
using Xunit;

namespace BlogCoreAPI.FunctionalTests.Controllers
{
    public sealed class RolesController : AGenericTests<GetRoleDto, AddRoleDto, UpdateRoleDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetRoleDto, AddRoleDto, UpdateRoleDto> Helper { get; set; }
        public override async Task<GetRoleDto> AddRandomEntity()
        {
            var role = new AddRoleDto()
            {
                Name = Guid.NewGuid().ToString("N")[..20]
            };
            return await Helper.AddEntity(role);
        }

        public RolesController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new RoleHelper(Client);
        }
    }
}
