using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tensorflow.Serving;
using TensorFlowServingClient.Utils;
using WebTensorFlowServingClient.Models;

namespace WebTensorFlowServingClient.Controllers
{
	[Route("api/[controller]")]
    public class MnistDeepController : Controller
    {
		private IConfiguration _configuration;

		public MnistDeepController(IConfiguration Configuration)
		{
			_configuration = Configuration;
		}

		[HttpPost("[action]")]
        public PredictionResult PredictNumber([FromBody]PredictionRequest model)
        {
			try
			{
				//Load Bitmap from input base64
				Bitmap convertedImage = null;

				using (var str = new MemoryStream(Convert.FromBase64String(model.ImageData)))
				{
					str.Position = 0;
					using (var bmp = Image.FromStream(str))
					{
						//Resize image and convert to rgb24
						convertedImage = ImageUtils.ResizeImage(bmp, 28, 28, 280, 280);
					}
				}

				//Create channel
				var channel = new Channel(_configuration.GetSection("TfServer")["ServerUrl"], ChannelCredentials.Insecure);
				var client = new PredictionService.PredictionServiceClient(channel);

				//Init predict request
				var request = new PredictRequest()
				{
					ModelSpec = new ModelSpec() { Name = "mnist", SignatureName = ModelMethodClasses.PredictImages }
				};

				//Convert image to 28x28 8bit per pixel image data array
				var imageData = ImageUtils.ConvertImageStreamToDimArrays(convertedImage);

				var textDebug = TextUtils.RenderImageData(imageData);

				//add image tensor
				request.Inputs.Add("images", TensorBuilder.CreateTensorFromImage(imageData, 255.0f));
				//add keep_prob tensor
				request.Inputs.Add("keep_prob", TensorBuilder.CreateTensor(1.0f));

				var predictResponse = client.Predict(request);

				var maxValue = predictResponse.Outputs["scores"].FloatVal.Max();
				var predictedValue = predictResponse.Outputs["scores"].FloatVal.IndexOf(maxValue);

				return new PredictionResult()
				{
					Success = true,
					Results = predictResponse.Outputs["scores"].FloatVal.Select(x => x).ToList(),
					PredictedNumber = predictedValue,
					DebugText = textDebug
				};

			}
			catch(Exception ex)
			{
				return new PredictionResult()
				{
					Success = false,
					ErrorMessage = ex.ToString()
				};
			}
		}
    }
}
