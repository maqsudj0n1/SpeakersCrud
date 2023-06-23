using Infrastructure.DataAccess;
using Infrastructure;
using Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthentication().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = builder.Configuration["web:google_client_id"];
            googleOptions.ClientSecret = builder.Configuration["web:google_client_secret"];
        });

        builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
        {
            facebookOptions.AppId = builder.Configuration["web:facebookAppId"];
            facebookOptions.AppSecret = builder.Configuration["web:facebookAppSecret"];   
        });
        builder.Services.AddAuthentication().AddGitHub(gitHubOptions =>
        {
            gitHubOptions.ClientId = builder.Configuration["web:githubClientId"];
            gitHubOptions.ClientSecret = builder.Configuration["web:githubClientSecret"];
        });

        builder.Services.AddAuthentication().AddMicrosoftAccount(microsoft =>
        {
            microsoft.ClientSecret = builder.Configuration["web:microsoftClientSecret"];
            microsoft.ClientId = builder.Configuration["web:microsoftClientId"];
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://example.com",
                                                          "https://www.microsoft.com/en-us/")
                                                          .AllowAnyHeader()
                                                          .AllowAnyMethod();
                                  });
        });
        // Add services to the container.
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddRazorPages();
        builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true).
            AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
       app.UseCors(MyAllowSpecificOrigins);

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapRazorPages();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
