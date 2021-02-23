﻿using AutoMapper;
using DbAccess.Repositories.Post;

namespace MyBlogAPI.DTO.Post
{
    public class PostDtoConverter : ITypeConverter<int, DbAccess.Data.POCO.Post>
    {
        private readonly IPostRepository _repository;

        public PostDtoConverter(IPostRepository repository)
        {
            _repository = repository;
        }

        public DbAccess.Data.POCO.Post Convert(int source, DbAccess.Data.POCO.Post destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
