using HttpHeaderSecurity.Extensions;
using HttpHeaderSecurity.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HttpHeaderSecurity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                    .AddJsonFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomHttpHeaders(Array.AsReadOnly(new[]
            {
                new HttpHeaderPolicy("X-XSS-Protection","1; mode=block"),
                new HttpHeaderPolicy("X-Frame-Options","SAMEORIGIN"),
                new HttpHeaderPolicy("X-Content-Type-Options","nosniff"),
                new HttpHeaderPolicy("Strict-Transport-Security","max-age=31536000; includeSubDomains; preload"),
            }));

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
