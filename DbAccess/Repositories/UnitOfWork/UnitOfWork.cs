﻿using DbAccess.DataContext;
using DbAccess.Repositories.User;

namespace DbAccess.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyBlogContext _context;

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public UnitOfWork(MyBlogContext context)
        {
            _context = context;
        }
    }
}
