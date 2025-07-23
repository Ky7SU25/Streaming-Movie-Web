using StreamingMovie.Application.Services;
using StreamingMovie.Application.Interfaces;
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
});

var app = builder.Build();

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
app.MapHub<CommentHub>("/commentHub");
app.UseStatusCodePagesWithReExecute("/Home/Error404");

app.Run();
