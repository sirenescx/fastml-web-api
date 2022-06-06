using System.Reflection;
using Fast.ML.Controllers;
using Fast.ML.Extensions;
using Fast.ML.Hubs;
using Microsoft.OpenApi.Models;

namespace Fast.ML;

public sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(builder => { builder.AddFile("web-api.log"); });
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddDependencies(_configuration);
        services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(_ => true);
            }));
        services.AddSignalR();

        services.AddGrpc();
        services.AddGrpcHttpApi();
        services.AddGrpcReflection();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "Fast ML", Version = "v1"});
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        services.AddGrpcSwagger();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fast ML V1");
        });

        app.UseHsts();
        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<PageUpdateHub>("/pageUpdateHub");
            endpoints.MapControllers();
            endpoints.MapGrpcService<ModelController>();
            endpoints.MapGrpcReflectionService();
        });
    }
}