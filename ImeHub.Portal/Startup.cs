using ImeHub.Portal.Areas.Identity;
using ImeHub.Portal.Data;
using ImeHub.Portal.Library;
using ImeHub.Portal.Library.Security;
using ImeHub.Portal.Services.Email;
using ImeHub.Portal.Services.DateTimeService;
using ImeHub.Portal.Services.FileSystem;
using ImeHub.Portal.Services.HtmlToPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sharp.RazorToString;
using System.Net.Http;
using TailBlazor.Toast;

namespace ImeHub.Portal
{
    public class Startup
    {
        private IConfiguration _config { get; }
        private IWebHostEnvironment _env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<LocalFileSystemOptions>()
                .Bind(_config.GetSection(LocalFileSystemOptions.SectionName));
            services.Configure<LocalFileSystemOptions>(_config.GetSection(LocalFileSystemOptions.SectionName));

            services.AddOptions<Html2PdfRocketOptions>()
                .Bind(_config.GetSection(Html2PdfRocketOptions.SectionName));
            services.Configure<Html2PdfRocketOptions>(_config.GetSection(Html2PdfRocketOptions.SectionName));


//#if DEBUG
//            services.AddDbContext<ApplicationDbContext>(options =>
//                            options.UseSqlServer(
//                                _config.GetConnectionString("OrvosiDbContext"))
//                            .EnableSensitiveDataLogging());
//#else
//            services.AddDbContext<ApplicationDbContext>(options =>
//                            options.UseSqlServer(
//                                _config.GetConnectionString("OrvosiDbContext")));
//#endif
#if DEBUG
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    _config.GetConnectionString("OrvosiDbContext"))
                .EnableSensitiveDataLogging());
#else
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    _config.GetConnectionString("OrvosiDbContext")));
#endif

            // this is needed because the Razor Pages components (asp.net identity) injects this directly.
            services.AddScoped<ApplicationDbContext>(p =>
                p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>()
                .CreateDbContext());

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddRoleManager<RoleManager<ApplicationRole>>();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,
                AdditionalUserClaimsPrincipalFactory>();

            // SECURITY SETUP
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();

                var isSystemAdminPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new IsSystemAdminRequirement())
                    .Build();
                
                options.AddPolicy(AuthorizationPolicies.SystemAdminOnly, isSystemAdminPolicy);
            });

            services.AddScoped<AuthenticationStateProvider, 
                RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

            // This is used for emails and reports
            services.AddTransient<IRazorToStringViewRenderer, RazorToStringViewRenderer>();
            services.AddTransient<IPageRenderService, PageRenderService>();

            // File Systems
            services.AddSingleton<FileSystemFactory>();

            services.AddHttpClient();
            services.AddTransient<IHtmlToPdf, Html2PdfRocket>();

            services.AddTransient<IDateTime, SystemDateTime>();

            services.AddControllersWithViews();

            var mvcBuilder = services.AddRazorPages();
            if (_env.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            services.AddServerSideBlazor();

            services.AddTailBlazorToast();

            if (!_env.IsDevelopment())
            {
                services.Configure<LocalhostOptions>(_config.GetSection(LocalhostOptions.SectionName));
                services.AddTransient<IEmailSender, Localhost>();
            }
            else
            {
                services.Configure<SendGridOptions>(_config.GetSection(SendGridOptions.SectionName));
                services.AddTransient<IEmailSender, Services.Email.SendGrid>();
            }

            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
