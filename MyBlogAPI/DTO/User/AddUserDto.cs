﻿namespace MyBlogAPI.DTO.User
{
    public class AddUserDto : IUserDto
    {
        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string UserDescription { get; set; }
    }
}
