using System;

namespace CurrencyServiceCore.Models
{
	public class ProjectSettingsModel
	{
		public string BccrIndicadorBaseUrl { get; set; }

		public TimeSpan BccrExchangeCacheTime { get; set; }

		public TimeSpan BccrExchangeMaxTimeLastItem { get; set; }

        public BccrCodesDbCacheSection BccrCodesDbCache { get; set; }

        public class BccrCodesDbCacheSection
		{
            public int DefaultCacheExpirationInHours { get; set; }
        }
	}
}
