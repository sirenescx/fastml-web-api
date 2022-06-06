using Fast.ML.Database;
using Fast.ML.Hubs;
using Fast.ML.Registries;
using Fast.ML.Registries.Interfaces;
using Fast.ML.Services;
using Fast.ML.Services.Interfaces;
using Fast.ML.Services.ModelServices.Classification;
using Fast.ML.Services.ModelServices.Regression;
using Grpc.Core;
using PreprocessingService = Fast.ML.Preprocessor.Grpc.PreprocessingService;
using ServiceOptions = Fast.ML.Options.ServiceOptions;
using LinearRegressionServiceClient = 
    Fast.ML.Linear.Regression.Grpc.LinearRegressionService
    .LinearRegressionServiceClient;
using LinearClassificationServiceClient = 
    Fast.ML.Linear.Classification.Grpc.LinearClassificationService
    .LinearClassificationServiceClient;

namespace Fast.ML.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        return services
            .AddGrpcClients(configuration)
            .AddDatabase()
            .AddServices()
            .AddOptions(configuration);
    }

    private static IServiceCollection AddGrpcClients(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddGrpcClient<PreprocessingService.PreprocessingServiceClient>(
            options => options.Address =
                new Uri(
                    GetServiceUri(
                        configuration, 
                        "FastMLPreprocessingApiGrpcClientOptions"))
        ).ConfigureChannel(options =>
        {
            options.Credentials = ChannelCredentials.Insecure;
            options.LoggerFactory = LoggerFactory.Create(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Trace);
            });
        });

        services.AddGrpcClient<LinearRegressionServiceClient>(
            options => options.Address =
                new Uri(
                    GetServiceUri(
                        configuration, 
                        "FastMLLinearRegressionApiGrpcClientOptions"))
        ).ConfigureChannel(options =>
        {
            options.Credentials = ChannelCredentials.Insecure;
            options.LoggerFactory = LoggerFactory.Create(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Trace);
            });
        });

        services
            .AddGrpcClient<LinearClassificationServiceClient>(
                options => options.Address =
                    new Uri(
                        GetServiceUri(
                            configuration, 
                            "FastMLLinearClassificationApiGrpcClientOptions"))
            ).ConfigureChannel(options =>
            {
                options.Credentials = ChannelCredentials.Insecure;
                options.LoggerFactory = LoggerFactory.Create(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Trace);
                });
            });

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddSingleton<IMachineLearningService, MachineLearningService>()
            .AddSingleton<IPreprocessingService, Services.PreprocessingService>()
            .AddSingleton<IAlgorithmService, AlgorithmService>()
            .AddSingleton<IUserService, UserService>()
            .AddSingleton<PageUpdateHub>()
            .AddRegressionServices()
            .AddClassificationServices();

    private static IServiceCollection AddRegressionServices(
        this IServiceCollection services) =>
        services
            .AddSingleton<ILinearRegressionService, LinearRegressionService>();

    private static IServiceCollection AddClassificationServices(
        this IServiceCollection services) =>
        services
            .AddSingleton<ILinearClassificationService, LinearClassificationService>();

    private static IServiceCollection AddDatabase(
        this IServiceCollection services) =>
        services
            .AddSingleton<DapperContext>()
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IAlgorithmRepository, AlgorithmRepository>();

    private static IServiceCollection AddOptions(
        this IServiceCollection services, 
        IConfiguration configuration) =>
        services.Configure<ServiceOptions>(configuration.GetSection("ServiceOptions"));

    private static string GetServiceUri(IConfiguration configuration, string section) =>
        configuration
            .GetSection(section)
            .GetSection("Target").Value;
}