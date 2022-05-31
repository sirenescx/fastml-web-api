// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: sgd_regressor.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Fast.ML.SGDRegressor.Grpc {
  public static partial class SGDRegressorService
  {
    static readonly string __ServiceName = "fast_ml_sgd_regressor.SGDRegressorService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Fast.ML.SGDRegressor.Grpc.TrainingRequest> __Marshaller_fast_ml_sgd_regressor_TrainingRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Fast.ML.SGDRegressor.Grpc.TrainingRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Fast.ML.SGDRegressor.Grpc.TrainingResponse> __Marshaller_fast_ml_sgd_regressor_TrainingResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Fast.ML.SGDRegressor.Grpc.TrainingResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Fast.ML.SGDRegressor.Grpc.PredictionRequest> __Marshaller_fast_ml_sgd_regressor_PredictionRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Fast.ML.SGDRegressor.Grpc.PredictionRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Fast.ML.SGDRegressor.Grpc.PredictionResponse> __Marshaller_fast_ml_sgd_regressor_PredictionResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Fast.ML.SGDRegressor.Grpc.PredictionResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Fast.ML.SGDRegressor.Grpc.TrainingRequest, global::Fast.ML.SGDRegressor.Grpc.TrainingResponse> __Method_Train = new grpc::Method<global::Fast.ML.SGDRegressor.Grpc.TrainingRequest, global::Fast.ML.SGDRegressor.Grpc.TrainingResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Train",
        __Marshaller_fast_ml_sgd_regressor_TrainingRequest,
        __Marshaller_fast_ml_sgd_regressor_TrainingResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Fast.ML.SGDRegressor.Grpc.PredictionRequest, global::Fast.ML.SGDRegressor.Grpc.PredictionResponse> __Method_Predict = new grpc::Method<global::Fast.ML.SGDRegressor.Grpc.PredictionRequest, global::Fast.ML.SGDRegressor.Grpc.PredictionResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Predict",
        __Marshaller_fast_ml_sgd_regressor_PredictionRequest,
        __Marshaller_fast_ml_sgd_regressor_PredictionResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Fast.ML.SGDRegressor.Grpc.SgdRegressorReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for SGDRegressorService</summary>
    public partial class SGDRegressorServiceClient : grpc::ClientBase<SGDRegressorServiceClient>
    {
      /// <summary>Creates a new client for SGDRegressorService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public SGDRegressorServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for SGDRegressorService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public SGDRegressorServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected SGDRegressorServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected SGDRegressorServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Fast.ML.SGDRegressor.Grpc.TrainingResponse Train(global::Fast.ML.SGDRegressor.Grpc.TrainingRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Train(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Fast.ML.SGDRegressor.Grpc.TrainingResponse Train(global::Fast.ML.SGDRegressor.Grpc.TrainingRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Train, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Fast.ML.SGDRegressor.Grpc.TrainingResponse> TrainAsync(global::Fast.ML.SGDRegressor.Grpc.TrainingRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return TrainAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Fast.ML.SGDRegressor.Grpc.TrainingResponse> TrainAsync(global::Fast.ML.SGDRegressor.Grpc.TrainingRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Train, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Fast.ML.SGDRegressor.Grpc.PredictionResponse Predict(global::Fast.ML.SGDRegressor.Grpc.PredictionRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Predict(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Fast.ML.SGDRegressor.Grpc.PredictionResponse Predict(global::Fast.ML.SGDRegressor.Grpc.PredictionRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Predict, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Fast.ML.SGDRegressor.Grpc.PredictionResponse> PredictAsync(global::Fast.ML.SGDRegressor.Grpc.PredictionRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PredictAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Fast.ML.SGDRegressor.Grpc.PredictionResponse> PredictAsync(global::Fast.ML.SGDRegressor.Grpc.PredictionRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Predict, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override SGDRegressorServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new SGDRegressorServiceClient(configuration);
      }
    }

  }
}
#endregion
