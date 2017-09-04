using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VkGroupManager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using VkGroupManager.Service.Telegram;
using VkGroupManager.Service.JSON;
using VkGroupManager.Service.VK;
using VkGroupManager.Service.Text;
using Microsoft.AspNetCore.Mvc;
using VkGroupManager.Services;
using VkGroupManager.Service.Instagram;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IO.Compression;
using VkGroupManager.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using VkGroupManager.Authorization;
using Microsoft.AspNetCore.Rewrite;
using AspNetCore.Security.OAuth.Vkontakte;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;

namespace VkGroupManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Authorization handlers.
            services.AddScoped<IAuthorizationHandler,
                      ContactIsOwnerAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler,
                                  ContactAdministratorsAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler,
                                  ContactManagerAuthorizationHandler>();

            services.AddTransient<ITelegramService, TelegramService>();
            services.AddTransient<ITextService, TextService>();
            services.AddTransient<IJsonService, JsonService>();
            services.AddTransient<IVkService, VkService>();
            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<IInstagramService, InstagramService>();

            #region Gzip Deflate
            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;

                options.MimeTypes = new[]
                {
                    // General
                    "text/plain",
                    // Static files
                    "text/css",
                    "application/javascript",
                    // MVC
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml"
                };

                options.Providers.Add<GzipCompressionProvider>();
                //options.Providers.Add(new DeflateCompressionProvider());
            });
            #endregion

            //string connection = Configuration.GetConnectionString("DefaultConnection");
            string connection = Configuration.GetConnectionString("RegRu");
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection));

            #region Identity
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false,
                    RequireLowercase = true,
                    RequiredLength = 5,
                };

                //options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                //{
                //    OnRedirectToLogin = ctx =>
                //    {
                //        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //        return Task.FromResult<object>(null);
                //    }
                //};

                // Lockout settings
                options.Lockout = new LockoutOptions
                {
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30),
                    MaxFailedAccessAttempts = 5,
                    AllowedForNewUsers = true
                };

                // Cookie settings
                options.Cookies.ApplicationCookie.CookieDomain = "https://vkgraber.ru";
                options.Cookies.ApplicationCookie.CookieName = "VkManagerCookie";
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOff";
                options.Cookies.ExternalCookie.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.Cookies.TwoFactorUserIdCookie.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
                options.Cookies.ApplicationCookie.AutomaticChallenge = true;
                options.Cookies.ApplicationCookie.AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Cookies.ApplicationCookie.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;

                options.SignIn.RequireConfirmedEmail = true;
                // User settings
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            #endregion

            // Adds a default in-memory implementation of IDistributedCache.
            //services.AddDistributedMemoryCache();
            //services.AddSession(options =>
            //{
            //    // Set a short timeout for easy testing.
            //    options.IdleTimeout = TimeSpan.FromSeconds(24 * 60 * 60 * 30);
            //    options.CookieHttpOnly = false;
            //});

            services.AddMvc(options =>
            {
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                //options.SslPort = 44391;
                options.Filters.Add(new RequireHttpsAttribute());
            });
        }

        #region Configure
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = content =>
                {
                    var time = 7 * 24 * 60 * 60;

                    content.Context.Response.Headers[HeaderNames.CacheControl] = $"public,max-age={time}";
                    //context.Context.Response.Headers["Pragma"] = "no-cache";
                    content.Context.Response.Headers[HeaderNames.Expires] = DateTime.UtcNow.AddDays(7).ToString("R"); // Format RFC1123
                }
            });

            app.UseIdentity();

            //app.UseSession();
            app.UseResponseCompression();

            //var options = new RewriteOptions()
            //    .AddRedirect("(.*)/$", "$1")                    // Redirect using a regular expression
            //    .AddRewrite(@"app/(\d+)", "app?id=$1", skipRemainingRules: false) // Rewrite based on a Regular expression
            //                                                                      //.AddRedirectToHttpsPermanent()
            //    .AddRedirectToHttps(302, 5001)                  // Redirect to a different port and use HTTPS
            //                                                    //.AddIISUrlRewrite(env.ContentRootFileProvider, "UrlRewrite.xml")        // Use IIS UrlRewriter rules to configure
            //                                                    //.AddApacheModRewrite(env.ContentRootFileProvider, "Rewrite.txt")
            //    ;       // Use Apache mod_rewrite rules to configure

            //app.UseRewriter(options);

            //var options = new RewriteOptions()
            //    );

            //app.UseRewriter(options);

            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
            //    ForwardedHeaders.XForwardedProto
            //});

            app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = "115934062379975",
                AppSecret = "5f7208f68c62a3ad00a1015508890ab1"
            });

            app.UseTwitterAuthentication(new TwitterOptions()
            {
                ConsumerKey = "5U0r7vnzQ45C1UFWlVVlY5lMl",
                ConsumerSecret = "HXh4PRZmdi8eyrCmikNjUagV2wiEHKi7u1ZK0JetZS1qXfihO4"
            });

            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = "1099392282891-uba7ck2v8fd12ks7vd8er1h0kpl90fo7",
                ClientSecret = "agNyrR5bX2uzfOCLdVBpXhyL"
            });

            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountOptions()
            {
                ClientId = "6ce60bb2-2bc4-4c48-95ef-9a85a4e7a768",
                ClientSecret = "mBxwTEELW0MZqFXhYbyWKaN"
            });

            app.UseVkontakteAuthentication(new VkontakteAuthenticationOptions()
            {
                ClientId = "6134363",
                ClientSecret = "d2qfI0XgEZXALzvQ35Ji",
                AuthenticationScheme = "ВКонтакте",
                DisplayName = "ВКонтакте",
                //SaveTokens = true
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                ExpireTimeSpan = TimeSpan.FromDays(30)
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DatabaseInitialize(app.ApplicationServices).Wait();
        }
        #endregion

        public async Task DatabaseInitialize(IServiceProvider serviceProvider)
        {
            UserManager<User> userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminEmail = "admin@gmail.com";
            string password = "_FmmK31LS";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
