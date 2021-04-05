using ImeHub.Portal.Areas.Identity;
using ImeHub.Portal.Data;
using ImeHub.Portal.Library.Security;
using ImeHub.Portal.Services.DateTimeService;
using ImeHub.Portal.Services.FileSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<FileSystemOptions>()
                .Bind(_config.GetSection(FileSystemOptions.SectionName));

            services.Configure<FileSystemOptions>(_config.GetSection(FileSystemOptions.SectionName));

#if DEBUG
            services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(
                                _config.GetConnectionString("OrvosiDbContext"))
                            .EnableSensitiveDataLogging());
#else
            services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(
                                _config.GetConnectionString("OrvosiDbContext"));
#endif
            //#if DEBUG
            //            services.AddDbContextFactory<ApplicationDbContext>(options =>
            //                options.UseSqlServer(
            //                    _config.GetConnectionString("DefaultConnection"))
            //                .EnableSensitiveDataLogging());
            //#else
            //            services.AddDbContextFactory<ApplicationDbContext>(options =>
            //                options.UseSqlServer(
            //                    _config.GetConnectionString("DefaultConnection")));
            //#endif

            //services.AddScoped<ApplicationDbContext>(p =>
            //    p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>()
            //    .CreateDbContext());

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

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
            });

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

            services.AddTransient<IFileSystemProvider, LocalFileSystem>();
            services.AddSingleton<DateTimeProvider>();

            var mvcBuilder = services.AddRazorPages();
            if (_env.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }
            services.AddServerSideBlazor();

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
