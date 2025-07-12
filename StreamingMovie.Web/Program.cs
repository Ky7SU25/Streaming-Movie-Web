using Microsoft.AspNetCore.Identity;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Infrastructure.Data;
using StreamingMovie.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = false;
});

builder.Services.AddControllersWithViews();
builder.Services.AddCoreInfrastructure(builder.Configuration);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MovieDbContext>();

    var canConnect = await context.Database.CanConnectAsync();

    if (!canConnect)
    {
        throw new HostAbortedException(message: "Cannot connect database.");
    }

    var seeder = new DatabaseSeeder(
        context: context,
        scope.ServiceProvider.GetRequiredService<UserManager<User>>(),
        scope.ServiceProvider.GetRequiredService<RoleManager<Role>>()
    );
    var seedResult = await seeder.SeedAllAsync();

    if (!seedResult)
    {
        throw new HostAbortedException(message: "Database seeding is false.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseStatusCodePagesWithReExecute("/Home/Error404");

app.Run();
