using System;

namespace CurrencyServiceCore.Models
{
	public class ProjectSettingsModel
	{
		public string BccrIndicadorBaseUrl { get; set; }

		public TimeSpan BccrExchangeCacheTime { get; set; }

		public TimeSpan BccrExchangeMaxTimeLastItem { get; set; }
	}
}
