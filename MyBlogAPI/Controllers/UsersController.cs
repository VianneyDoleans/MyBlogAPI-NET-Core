﻿using System;
using System.Threading.Tasks;
using DbAccess.Specifications;
using Microsoft.AspNetCore.Mvc;
using MyBlogAPI.DTO.User;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.User;
using MyBlogAPI.Responses;
using MyBlogAPI.Services.CommentService;
using MyBlogAPI.Services.LikeService;
using MyBlogAPI.Services.PostService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Controllers
{
    /// <summary>
    /// Controller used to expose User resources of the API.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILikeService _likeService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="likeService"></param>
        /// <param name="postService"></param>
        /// <param name="commentService"></param>
        public UsersController(IUserService userService, ILikeService likeService, IPostService postService, 
            ICommentService commentService)
        {
            _userService = userService;
            _likeService = likeService;
            _postService = postService;
            _commentService = commentService;
        }

        /// <summary>
        /// Get list of users.
        /// </summary>
        /// <remarks>
        /// Get list of users. The endpoint uses pagination and sort. Filter(s) can be applied for research.
        /// </remarks>
        /// <param name="sortingDirection"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="registerBefore"></param>
        /// <param name="lastLoginBefore"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetUsers(string sortingDirection = "ASC", string orderBy = null, int page = 1,
            int size = 10, string name = null, DateTime? registerBefore = null, DateTime? lastLoginBefore = null)
        {
            var validPagination = new PaginationFilter(page, size);
            var filterSpecification = new UserQueryFilter(name, lastLoginBefore, registerBefore).GetFilterSpecification();
            var data = await _userService.GetUsers(filterSpecification,
                new PagingSpecification((validPagination.Offset - 1) * validPagination.Limit, validPagination.Limit),
                new SortUserFilter(sortingDirection, orderBy).GetSorting());

            return Ok(new PagedBlogResponse<GetUserDto>(data, validPagination.Offset, validPagination.Limit,
                await _userService.CountUsersWhere(filterSpecification)));
        }

        /// <summary>
        /// Get a user by giving its Id.
        /// </summary>
        /// <remarks>
        /// Get a user by giving its Id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _userService.GetUser(id));
        }

        /// <summary>
        /// Add a user.
        /// </summary>
        /// <remarks>
        /// Add a user.
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto user)
        {
            return Ok(await _userService.AddUser(user));
        }

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <remarks>
        /// Update a user.
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto user)
        {
            if (await _userService.GetUser(user.Id) == null)
                return NotFound();
            await _userService.UpdateUser(user);
            return Ok();
        }

        /// <summary>
        /// Delete a user by giving its id.
        /// </summary>
        /// <remarks>
        /// Delete a user by giving its id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _userService.GetUser(id) == null)
                return NotFound();
            await _userService.DeleteUser(id);
            return Ok();
        }

        /// <summary>
        /// Get posts written by a user by giving user's id.
        /// </summary>
        /// <remarks>
        /// Get posts written by a user by giving user's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Posts/")]
        public async Task<IActionResult> GetPostsFromUser(int id)
        {
            return Ok(await _postService.GetPostsFromUser(id));
        }

        /// <summary>
        /// Get comments written by a user by giving user's id.
        /// </summary>
        /// <remarks>
        /// Get comments written by a user by giving user's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Comments/")]
        public async Task<IActionResult> GetCommentsFromUser(int id)
        {
            return Ok(await _commentService.GetCommentsFromUser(id));
        }

        /// <summary>
        /// Get likes given by a user by giving user's id.
        /// </summary>
        /// <remarks>
        /// Get likes given by a user by giving user's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Likes/")]
        public async Task<IActionResult> GetLikesFromUser(int id)
        {
            return Ok(await _likeService.GetLikesFromUser(id));
        }
    }
}
