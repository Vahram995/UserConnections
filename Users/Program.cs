using FluentValidation;
using FluentValidation.AspNetCore;
using Users.DAL.Extensions;
using Users.DAL.Repositories.Abstract;
using Users.DAL.Repositories.Concrete;
using Users.Middlewares;
using Users.Models.RequestModels;
using Users.Models.ValidationModels;
using Users.Services.Abstract;
using Users.Services.Concrete;

public partial class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddLogging();
        builder.Services.AddControllers();
        builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

        builder.Services.AddAppDbContext(builder.Configuration);

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
        });

        builder.Services.AddScoped<IValidator<UserConnectionRequestModel>, UserConnectionRequestModelValidator>();
        builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
        builder.Services.AddScoped<IUserConnectionService, UserConnectionService>();
        builder.Services.AddScoped<IUserConnectionRepository, UserConnectionRepository>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseRouting();
        app.MapControllers();

        app.Run();
    }
}