using Domain.Models;
using System;

namespace CurrencyServiceCore.Models
{
	public class ProjectSettingsModel
	{
		public string BccrBridgeBaseUrl { get; set; }

        public string BccrIndicadorBaseUrl { get; set; }

		public TimeSpan BccrExchangeCacheTime { get; set; }

		public TimeSpan BccrExchangeMaxTimeLastItem { get; set; }
        public BancoCentralSettings BancoCentral { get; set; }

        public BccrCodesDbCacheSection BccrCodesDbCache { get; set; }
        public BccrServiceType BccrIndicadorActiveMethod { get; set; }
		public string BccrIndicadorAPIBaseUrl { get; set; }

        public class BccrCodesDbCacheSection
		{
            public int DefaultCacheExpirationInHours { get; set; }
        }

        public class BancoCentralSettings
        {
            public string IndicadoresEconomicosToken { get; set; } = string.Empty;
        }
    }
}
