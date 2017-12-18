using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Grpc.Core;
using Tensorflow.Serving;
using TensorFlowServingClient.Utils;

namespace ConsoleTensorFlowServingClient
{
	class Program
	{
		static void Main(string[] args)
		{
			//Create gRPC Channel
			var channel = new Channel(ConfigurationManager.AppSettings["ServerHost"], ChannelCredentials.Insecure);
			var client = new PredictionService.PredictionServiceClient(channel);

			//Check available MNIST model
			var responce = client.GetModelMetadata(new GetModelMetadataRequest()
			{
				ModelSpec = new ModelSpec() { Name = "mnist" },
				MetadataField = { "signature_def" }
			});

			Console.WriteLine($"Model Available: {responce.ModelSpec.Name} Ver.{responce.ModelSpec.Version}");

			var imagesFolder = ConfigurationManager.AppSettings["ImagesFolder"];

			//Process images prediction from 0 to 9 fromexample folder
			for (int number = 0; number < 10; number++)
			{

				//Create prediction request
				var request = new PredictRequest()
				{
					ModelSpec = new ModelSpec() {Name = "mnist", SignatureName = ModelMethodClasses.PredictImages}
				};

				//Add image tensor [1 - 784]
				using (Stream stream = new FileStream($"{AppDomain.CurrentDomain.BaseDirectory}/{imagesFolder}/{number}.bmp", FileMode.Open))
				{
					request.Inputs.Add("images", TensorBuilder.CreateTensorFromImage(stream, 255.0f));
				}

				//Add keep_prob tensor [1 - 1]
				request.Inputs.Add("keep_prob", TensorBuilder.CreateTensor(0.5f));

				var predictResponse = client.Predict(request);

				//Compute Max value from prediction array
				var maxValue = predictResponse.Outputs["scores"].FloatVal.Max();
				//Get index of predicted value
				var predictedValue = predictResponse.Outputs["scores"].FloatVal.IndexOf(maxValue);

				Console.WriteLine($"Predict: {number} {(number == predictedValue ? "Y" : "N")}");
				Console.WriteLine($"Result value: {predictedValue}, probability: {maxValue}");
				Console.WriteLine($"All values: {predictResponse.Outputs["scores"].FloatVal}");
				Console.WriteLine("");
			}

			channel.ShutdownAsync().Wait();
		}
	}
}
