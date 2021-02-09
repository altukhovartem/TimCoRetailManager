using System;
using System.Configuration;

namespace TRMDesktopUI.Library.Helpers
{
	public class ConfigHelper : IConfigHelper
	{
		// Move this from config to API
		public decimal GetTaxRate()
		{
			string rateText = ConfigurationManager.AppSettings["taxRate"];
			bool isValidTaxRate = Decimal.TryParse(rateText, out decimal output);

			if (isValidTaxRate == false)
			{
				throw new ConfigurationErrorsException("The Tax rate is not set up properly");
			}

			return output;
		}
	}
}
