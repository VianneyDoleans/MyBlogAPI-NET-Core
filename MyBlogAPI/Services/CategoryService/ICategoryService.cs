﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTOs.Category;

namespace MyBlogAPI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetCategoryDto>> GetAllCategories();

        public Task<IEnumerable<GetCategoryDto>> GetCategories(FilterSpecification<Category> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Category> sortSpecification = null);

        public Task<int> CountCategoriesWhere(FilterSpecification<Category> filterSpecification = null);

        Task<GetCategoryDto> GetCategory(int id);

        Task<GetCategoryDto> AddCategory(AddCategoryDto category);

        Task UpdateCategory(UpdateCategoryDto category);

        Task DeleteCategory(int id);
    }
}
