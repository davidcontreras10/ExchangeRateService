using CurrencyServiceCore.Models;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace CurrencyServiceCore.Implementations
{
	public class ProjectSettings(IOptions<ProjectSettingsModel> appSettings) : IProjectSettings
	{

		public string BccrIndicadorBaseUrl => appSettings.Value.BccrIndicadorBaseUrl;

		public TimeSpan BccrExchangeCacheTime => appSettings.Value.BccrExchangeCacheTime;

		public TimeSpan BccrExchangeMaxTimeLastItem => appSettings.Value.BccrExchangeMaxTimeLastItem;
	}
}
