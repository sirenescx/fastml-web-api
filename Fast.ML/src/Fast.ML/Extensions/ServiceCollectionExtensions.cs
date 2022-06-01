using Fast.ML.Database;
using Fast.ML.Hubs;
using Fast.ML.Options;
using Fast.ML.Registries;
using Fast.ML.Registries.Interfaces;
using Fast.ML.Services;
using Fast.ML.Services.Interfaces;
using Fast.ML.Services.ModelServices.Classification;
using Fast.ML.Services.ModelServices.Regression;
using PreprocessingService = Fast.ML.Preprocessor.Grpc.PreprocessingService;

namespace Fast.ML.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddGrpcClients(configuration)
            .AddDatabase()
            .AddServices();
    }

    private static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PreprocessingClientOptions>(configuration.GetSection("FastMLPreprocessingApiGrpcClientOptions"));
        services.AddGrpcClient<PreprocessingService.PreprocessingServiceClient>(
            configureClient: options => options.Address = new Uri("http://localhost:82")
            );
        
        services.AddGrpcClient<Linear.Regression.Grpc.LinearRegressionService.LinearRegressionServiceClient>(
            configureClient: options => options.Address = new Uri("http://localhost:84")
        );
        services.AddGrpcClient<LogisticRegression.Grpc.LogisticRegressionService.LogisticRegressionServiceClient>(
            configureClient: options => options.Address = new Uri("http://localhost:89")
        );
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IMachineLearningService, MachineLearningService>()
            .AddSingleton<IPreprocessingService, Services.PreprocessingService>()
            .AddSingleton<IAlgorithmService, AlgorithmService>()
            .AddSingleton<IUserService, UserService>()
            .AddSingleton<PageUpdateHub>()
            .AddRegressionServices()
            .AddClassificationServices();
    }
    
    private static IServiceCollection AddRegressionServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<ILinearRegressionService, LinearRegressionService>();
    }
    
    private static IServiceCollection AddClassificationServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<ILogisticRegressionService, LogisticRegressionService>();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        return services
            .AddSingleton<DapperContext>()
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IAlgorithmRepository, AlgorithmRepository>();
    }
}