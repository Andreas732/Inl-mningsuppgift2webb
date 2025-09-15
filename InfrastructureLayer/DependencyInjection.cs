using ApplicationLayer.Abstractions;
using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.Behaviors;
using ApplicationLayer.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using InfrastructureLayer.Persistence;
using InfrastructureLayer.Repositories;

namespace InfrastructureLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(connectionString));

        // Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // MediatR (v11 syntax)
        services.AddMediatR(typeof(ApplicationLayer.Categories.Commands.CreateCategoryCommand).Assembly);

        // AutoMapper + Validators
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

        // Pipeline behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }

    private sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _ctx;
        public UnitOfWork(AppDbContext ctx) => _ctx = ctx;
        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
    }
}
