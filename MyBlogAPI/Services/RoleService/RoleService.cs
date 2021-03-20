﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.UnitOfWork;
using MyBlogAPI.DTO.Role;

namespace MyBlogAPI.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IRoleRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetRoleDto>> GetAllRoles()
        {
            return (await _repository.GetAllAsync()).Select(c =>
            {
                var roleDto = _mapper.Map<GetRoleDto>(c);
                roleDto.Users = c.UserRoles.Select(x => x.UserId);
                return roleDto;
            }).ToList();
        }

        public async Task<GetRoleDto> GetRole(int id)
        {
            var role = await _repository.GetAsync(id);
            var roleDto = _mapper.Map<GetRoleDto>(role);
            roleDto.Users = role.UserRoles.Select(x => x.UserId);
            return roleDto;
        }

        public void CheckRoleValidity(IRoleDto role)
        {
            if (role == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(role.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (role.Name.Length > 20)
                throw new ArgumentException("Name cannot exceed 20 characters.");
        }

        public async Task CheckRoleValidity(AddRoleDto role)
        {
            CheckRoleValidity((IRoleDto)role);
            if (await _repository.NameAlreadyExists(role.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task<GetRoleDto> AddRole(AddRoleDto role)
        {
            await CheckRoleValidity(role);
            var result = await _repository.AddAsync(_mapper.Map<Role>(role));
            _unitOfWork.Save();
            return _mapper.Map<GetRoleDto>(result);
        }

        public async Task UpdateRole(UpdateRoleDto role)
        {
            if (await RoleAlreadyExistsWithSameProperties(role))
                return;
            CheckRoleValidity(role);
            var roleEntity = await _repository.GetAsync(role.Id);
            roleEntity.Name = role.Name;
            _unitOfWork.Save();
        }

        public async Task DeleteRole(int id)
        {
            await _repository.RemoveAsync(await _repository.GetAsync(id));
            _unitOfWork.Save();
        }

        private async Task<bool> RoleAlreadyExistsWithSameProperties(UpdateRoleDto role)
        {
            if (role == null)
                throw new ArgumentNullException();
            var roleDb = await _repository.GetAsync(role.Id);
            return role.Name == roleDb.Name;
        }
    }
}
