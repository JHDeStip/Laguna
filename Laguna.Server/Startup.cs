using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using JhDeStip.Laguna.Server.Dal;
using JhDeStip.Laguna.Server.Services;
using JhDeStip.Laguna.Server.Filters;
using JhDeStip.Laguna.Server.Domain;

namespace JhDeStip.Laguna.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("hosting.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<YoutubeServiceConfig>(Configuration.GetSection("AppSettings:YoutubeConfig"));
            services.Configure<ApiConfig>(Configuration.GetSection("AppSettings:ApiConfig"));
            services.AddSingleton<IYoutubeService, YoutubeService>();
            services.AddSingleton<IPlaylistService, PlaylistService>();
            services.AddSingleton<IServiceAvailabilityService, ServiceAvailabilityService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ServiceAvailabilityFilter, ServiceAvailabilityFilter>();

            // Add framework services.
            services.AddMvc();

            var serviceProvider = services.BuildServiceProvider();
            var playlistService = serviceProvider.GetService<IPlaylistService>();
            var youtubeService = serviceProvider.GetService<IYoutubeService>();
            var serviceAvailabilityService = serviceProvider.GetService<IServiceAvailabilityService>();

            List<PlayableItemInfo> playlist = youtubeService.GetFallbackPlaylistItemsAsync().Result;
            playlistService.SetFallbackPlaylistAsync(playlist);

            serviceAvailabilityService.IsServiceAvailable = false;

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
