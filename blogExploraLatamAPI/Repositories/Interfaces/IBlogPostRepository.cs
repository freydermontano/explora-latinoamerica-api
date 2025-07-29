﻿using CodeBlog.API.Models.Domain;

namespace blogExploraLatamAPI.Repositories.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetByIdAsync(Guid id);


    }
}
