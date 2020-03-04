using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions.Convert.ConvertHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Volunteer.Core.Interfaces;
using Volunteer.Infrastructure.Database;
using Volunteer.Infrastructure.Repositories;
using Volunteer.MQ.Listener;

namespace Volunteer.WebApi
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            ILogger<Startup> logger,
            IHostingEnvironment env
            )
        {
            Configuration = configuration;
            Logger = logger;
            Env = env;
            IsApollo = Env.IsProduction();
        }

        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }

        public  IHostingEnvironment Env { get; }

        public bool IsApollo { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new Info()
                    {
                        Title = "志愿服务",
                        Version = "v1",
                        Description = "志愿服务平台API",
                        Contact = new Contact()
                        {
                            Name = "WangZhaohun",
                            Email = string.Empty,
                            Url = string.Empty
                        }
                    });
                    //为 Swagger JSON and UI设置xml文档注释路径
                    var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                    var xmlPath = Path.Combine(basePath, "Volunteer.WebApi.xml");
                    c.IncludeXmlComments(xmlPath);
                });
            services.AddDbContext<VolunteerContext>(option =>
            {
                option.UseMySql(Configuration.GetConnectionString("Mysql"));
            });
            // 注入 UnitOfWork
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            // 注入  办公室   repository
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            // 注入 寝室楼  repository
            services.AddScoped<IDormitoryRepository, DormitoryRepository>();
            // 注入 文章动态  repository
            services.AddScoped<IPageInfoRepository, PageInfoRepository>();
            // 注入 志愿组织管理 repository
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            // 注入 报名记录  repository
            services.AddScoped<ISignActivityNotesRepository, SignActivityNotesRepository>();
            // 注入  志愿活动 repository
            services.AddScoped<IActivityRepository, ActivityRepository>();
            //  注入 用户  志愿信息  repository
            services.AddScoped<IUserVolunteerRepository, UserVolunteerRepository>();
            // 注入  学生信息认证
            services.AddScoped<ISchoolUserRepository, SchoolUserRepository>();
            // 注入  文件上传
            services.AddScoped<IFileManagerRepository, FileManagerRepository>();

            // 注入 MQListener
            //services.AddHostedService<SchoolUserListener>();
            services.AddCors(options =>
            {
                options.AddPolicy("spa", config =>
                {
                    config
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyOrigin();
                });
            });
            // 配置 redis
            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = Configuration["Redis:Name"];
                options.Configuration = Configuration["Redis:Con"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, IDistributedCache cache)
        {
            // 使用缓存的当前时间
            //lifetime.ApplicationStarted.Register(() =>
            //{
            //    var currentTimeUTC = DateTime.UtcNow.ToString();
            //    byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
            //    var options = new DistributedCacheEntryOptions()
            //        .SetSlidingExpiration(TimeSpan.FromSeconds(20));
            //    cache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
            //});
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Upload")),
                RequestPath = "/Upload"
            });
            app.UseCors("spa");
            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            app.UseMvc();
        }
    }
}
