using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using Application;
using Infrastructure;
using Api.Extensions;
using Domain.Entities.Identity;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
var services = builder.Services;

services.AddApplication();
services.AddInfrastructure();
services.AddPersistence(configuration);

services.AddControllers(options => options.Filters.Add(new Api.Filters.ApiExceptionFilter()));

services.AddModelValidation();
services.AddIdentityServices(configuration);
services.AddSwaggerDocumentation();

// For Blazor
services.AddControllersWithViews();
services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var scopedServices = scope.ServiceProvider;
    var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = scopedServices.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        // Seeding users
        var userManager = scopedServices.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
        await AppDbContextSeed.SeedUsersAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred during migration");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
    app.UseHttpsRedirection();
    app.UseSwaggerDocumention();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
