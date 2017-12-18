using System.Drawing;
using System.IO;
using Tensorflow;

namespace TensorFlowServingClient.Utils
{
	public class TensorBuilder
	{
		public static TensorProto CreateTensor(float value)
		{
			var tensor = new TensorProto();
			tensor.FloatVal.Add(value);
			tensor.TensorShape = new TensorShapeProto();
			tensor.Dtype = DataType.DtFloat;
			var dim = new TensorShapeProto.Types.Dim();
			dim.Size = 1;
			tensor.TensorShape.Dim.Add(dim);

			return tensor;
		}

		public static TensorProto CreateTensorFromImage(Bitmap image, float revertsBits = 1.0f)
		{
			var imageData = ImageUtils.ConvertImageStreamToDimArrays(image);
			return CreateTensorFromImage(imageData, revertsBits);
		}

		public static TensorProto CreateTensorFromImage(Stream stream, float revertsBits = 1.0f)
		{
			var imageData = ImageUtils.ConvertImageStreamToDimArrays(stream);
			return CreateTensorFromImage(imageData, revertsBits);
		}

		public static TensorProto CreateTensorFromImage(int[][] imageData, float revertsBits)
		{
			var imageFeatureShape = new TensorShapeProto();

			imageFeatureShape.Dim.Add(new TensorShapeProto.Types.Dim() { Size = 1 });
			imageFeatureShape.Dim.Add(new TensorShapeProto.Types.Dim() { Size = imageData.Length * imageData.Length });

			var imageTensorBuilder = new TensorProto();
			imageTensorBuilder.Dtype = DataType.DtFloat;
			imageTensorBuilder.TensorShape = imageFeatureShape;

			for (int i = 0; i < imageData.Length; ++i)
			{
				for (int j = 0; j < imageData.Length; ++j)
				{
					imageTensorBuilder.FloatVal.Add(imageData[i][j] / revertsBits);
				}
			}

			return imageTensorBuilder;
		}
	}
}
