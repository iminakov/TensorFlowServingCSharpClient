using System.Text;

namespace TensorFlowServingClient.Utils
{
	public class TextUtils
	{
		public static string RenderImageData(int[][] image)
		{
			var sb = new StringBuilder();

			for (int row = 0; row < image.Length; row++)
			{
				sb.Append("|");
				for (int col = 0; col < image[row].Length; col++)
				{
					int pixelVal = image[row][col];
					if (pixelVal == 0)
						sb.Append(" ");
					else if (pixelVal < 256 / 3)
						sb.Append(".");
					else if (pixelVal < 2 * (256 / 3))
						sb.Append("x");
					else
						sb.Append("X");
				}
				sb.Append("|\n");
			}

			return sb.ToString();
		}
	}
}
