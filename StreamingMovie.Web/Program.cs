using System;
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Application.Services;
using StreamingMovie.Application.Services.BackgroundServices;
using StreamingMovie.Infrastructure.Data;
using StreamingMovie.Infrastructure.Extensions;
using StreamingMovie.Infrastructure.ExternalServices.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = false;
});
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddCoreInfrastructure(builder.Configuration);

//background service
builder.Services.AddHostedService<SubscriptionCheckService>();

// Add new video upload services
builder.Services.AddScoped<IVideoUploadService, VideoUploadService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;

    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
var app = builder.Build();
app.UseForwardedHeaders();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<MovieDbContext>();
//    var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();

//    var seeder = new MovieSeeder(context, httpClientFactory.CreateClient());
//    await seeder.SeedAsync(); // Seeder sẽ gọi Flask và lưu EmbeddingJson
//}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.Use(
    async (context, next) =>
    {
        Console.WriteLine("SCHEME: " + context.Request.Scheme);
        await next();
    }
);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<CommentHub>("/commentHub");
app.UseStatusCodePagesWithReExecute("/Home/Error404");

app.Run();
