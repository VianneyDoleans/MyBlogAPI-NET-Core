﻿using DbAccess.Data.POCO;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.Permissions;
using MyBlogAPI.Services.CategoryService;
using MyBlogAPI.Services.CommentService;
using MyBlogAPI.Services.JwtService;
using MyBlogAPI.Services.LikeService;
using MyBlogAPI.Services.PostService;
using MyBlogAPI.Services.RoleService;
using MyBlogAPI.Services.TagService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Extensions
{
    /// <summary> 
    /// Extension of <see cref="IServiceCollection"/> adding methods to inject MyBlog Services 
    /// </summary> 
    public static class ServiceCollectionExtensions
    {
        /// <summary> 
        /// class used to register repository services 
        /// </summary> 
        /// <param name="services"></param> 
        /// <returns></returns> 
        public static IServiceCollection RegisterRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        /// <summary> 
        /// class used to register resource services 
        /// </summary> 
        /// <param name="services"></param> 
        /// <returns></returns> 
        public static IServiceCollection RegisterResourceServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }

        /// <summary> 
        /// class used to register resource services 
        /// </summary> 
        /// <param name="services"></param> 
        /// <returns></returns> 
        public static IServiceCollection RegisterAuthorizationHandlers(this IServiceCollection services)
        {
            // Resource Handlers
            services.AddScoped<IAuthorizationHandler, AllPermissionRangeAuthorizationHandler<Role>>();
            services.AddScoped<IAuthorizationHandler, AllPermissionRangeAuthorizationHandler<Tag>>();
            services.AddScoped<IAuthorizationHandler, AllPermissionRangeAuthorizationHandler<Category>>();
            services.AddScoped<IAuthorizationHandler, OwnOrAllPermissionRangeForHasAuthorAuthorizationHandler<Comment>>();
            services.AddScoped<IAuthorizationHandler, OwnOrAllPermissionRangeForHasAuthorAuthorizationHandler<Post>>();
            services.AddScoped<IAuthorizationHandler, OwnOrAllPermissionRangeForHasUserAuthorizationHandler<Like>>();
            services.AddScoped<IAuthorizationHandler, OwnOrAllPermissionRangeForUserResourceAuthorizationHandler>();

            // DTO Handlers
            services.AddScoped<IAuthorizationHandler, OwnOrAllPermissionRangeForHasAuthorDtoAuthorizationHandler<ICommentDto>>();
            services.AddScoped<IAuthorizationHandler, OwnOrAllPermissionRangeForHasAuthorDtoAuthorizationHandler<IPostDto>>();
            services.AddScoped<IAuthorizationHandler, OwnOrAllPermissionRangeForHasUserDtoAuthorizationHandler<ILikeDto>>();

            // Resource Attribute Handler
            services.AddScoped<IAuthorizationHandler, PermissionWithRangeAuthorizationHandler>();

            return services;
        }
    }
}
