using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using APISample.Filters;
using APISample.Middleware;
using APISample.Helper;


namespace APISample
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
            //加入跨原始要求
            services.AddCors();

            //加入MVC服務-加入錯誤過濾器
            services.AddMvc(config =>
                {
                    //執行發生錯誤套用
                    config.Filters.Add<ExceptionFilter>();

                    //執行成功套用過濾器
                    config.Filters.Add<ResultFilter>();
                }
            );

            //加入將server log寫到 app.log服務
            //services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.AddFile("app.log", append: true);
            //});

            //加入swagger服務
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "APISample API",
                    Description = "APISample - ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "SHIH-WU CHENG",
                        Email = string.Empty
                    }
                });
            });  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())//開發環境
            {
                // 錯誤處理
                app.UseErrorHandling();
            }
            else//正式環境
            {
                app.UseExceptionHandler("/error");
            }

            //啟用cros 
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            //啟用swagger服務
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            //啟用MVC服務
            app.UseMvc();

            //啟用wwwroot靜態檔案
            app.UseStaticFiles();

            //啟用預設文件
            app.UseDefaultFiles();

            //提供靜態檔案和預設檔案。 未啟用目錄瀏覽功能
            app.UseFileServer();
        }
    }
}
