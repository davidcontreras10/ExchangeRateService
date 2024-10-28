using CurrencyServiceCore.Models;
using Domain.Models;
using Microsoft.Extensions.Options;
using System;

namespace CurrencyServiceCore.Implementations
{
	public class ProjectSettings(IOptions<ProjectSettingsModel> appSettings) : IProjectSettings
	{
		private readonly ProjectSettingsModel _settings = appSettings.Value;

		public string BccrIndicadorBaseUrl => _settings.BccrIndicadorBaseUrl;

		public TimeSpan BccrExchangeCacheTime => _settings.BccrExchangeCacheTime;

		public TimeSpan BccrExchangeMaxTimeLastItem => _settings.BccrExchangeMaxTimeLastItem;

		public int BccrCodesDbCacheHours => _settings.BccrCodesDbCache.DefaultCacheExpirationInHours;
	}
}
