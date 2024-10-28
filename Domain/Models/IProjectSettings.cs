using System;

namespace Domain.Models
{
	public interface IProjectSettings
	{
		string BccrIndicadorBaseUrl { get; }
		TimeSpan BccrExchangeCacheTime { get; }
		TimeSpan BccrExchangeMaxTimeLastItem { get; }
		int BccrCodesDbCacheHours { get; }
	}
}
