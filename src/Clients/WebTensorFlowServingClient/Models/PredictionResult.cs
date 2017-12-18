using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTensorFlowServingClient.Models
{
	public class PredictionResult
	{
		public List<float> Results { get; set; }

		public bool Success { get; set; }

		public string ErrorMessage { get; set; }

		public int PredictedNumber { get; set; }

		public string DebugText { get; set; }
	}
}
