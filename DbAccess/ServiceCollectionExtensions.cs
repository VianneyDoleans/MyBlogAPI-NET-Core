﻿using System;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbAccess
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDatabaseProvider(this IServiceCollection services,
            IConfiguration configuration)
        {
            var dbProvider = configuration.GetSection("DatabaseProvider");
            if (!dbProvider.Exists())
                throw new Exception("Database provider is not specified inside the configuration.");
            switch (dbProvider.Value)
            {
                case "MsSQL":
                    services.AddDbContext<MyBlogContext, MsSqlDbContext>(o => o.UseSqlServer(
                        configuration.GetConnectionString("Default")));
                    break;

                case "PostgreSQL": 
                    services.AddDbContext<MyBlogContext, PostgreSqlDbContext>(o => o.UseNpgsql(
                    configuration.GetConnectionString("Default")));
                break;
                case "HerokuPostgreSQL":
                    var builder = new PostgreSqlConnectionStringBuilder(configuration["DATABASE_URL"])
                    {
                        Pooling = true,
                        TrustServerCertificate = true,
                        SslMode = SslMode.Require
                    };
                    services.AddDbContext<MyBlogContext, PostgreSqlDbContext>(o =>
                        o.UseNpgsql(builder.ConnectionString));
                    break;
                default:
                    throw new Exception("Unsupported database provider : " + dbProvider);
            }

            return services;
        }
    }
}
