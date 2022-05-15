﻿using System;
using System.Collections.Generic;

namespace MyBlogAPI.DTOs.User
{
    /// <summary>
    /// GET Dto type of <see cref="DbAccess.Data.POCO.User"/>.
    /// </summary>
    public class GetUserDto : ADto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime RegisteredAt { get; set; }

        public DateTime LastLogin { get; set; }

        public string UserDescription { get; set; }

        public virtual IEnumerable<int> Roles { get; set; }
    }
}
