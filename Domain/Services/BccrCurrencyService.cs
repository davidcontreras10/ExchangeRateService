using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
	public interface IBccrCurrencyService
	{
		Task<IEnumerable<BccrSingleVentanillaModel>> GetBccrSingleVentanillaModelsAsync(string indicador, DateTime dateTime);
	}

	public class BccrCurrencyService(IBccrCurrencyRepository bccrCurrencyRepository, IBccrExchangeCache bccrExchangeCache, ILogger<BccrCurrencyService> logger) : IBccrCurrencyService
	{
		private readonly bool _allowCache = true;

		public async Task<IEnumerable<BccrSingleVentanillaModel>> GetBccrSingleVentanillaModelsAsync(string indicador, DateTime dateTime)
		{
			dateTime = AdjustDateTime(dateTime);
			var initialDate = dateTime.Date.AddMonths(-1);
			var endDate = dateTime.Date.AddDays(1);
			if (!_allowCache)
			{
				logger.LogInformation("Bccr Cache is disabled");
				var res = await GetFromDbBccrSingleVentanillaModelsAsync(indicador, dateTime, initialDate, endDate);
				return res.BccrSingleVentanillaModels;
			}

			var cache = bccrExchangeCache.Get(indicador, dateTime);
			if (cache != null)
			{
				logger.LogInformation("Bccr Cache hit");
				return [cache];
			}

			logger.LogInformation("Bccr Cache miss");
			var results = await GetFromDbBccrSingleVentanillaModelsAsync(indicador, dateTime, initialDate, endDate);
			if (results?.BccrSingleVentanillaModels == null || !results.BccrSingleVentanillaModels.Any())
			{
				return [];
			}

			bccrExchangeCache.Set(indicador, results);
			return results.BccrSingleVentanillaModels;
		}

		private async Task<BccrSingleVentanillaModelResponse> GetFromDbBccrSingleVentanillaModelsAsync(string indicador, DateTime reqDate, DateTime initialDate, DateTime endDate)
		{
			var results = await bccrCurrencyRepository.GetIndicatorAsync(indicador, initialDate, endDate);
			if (results == null || !results.Any())
			{
				results = [];
			}

			return new BccrSingleVentanillaModelResponse(results, reqDate, initialDate)
			{
				EndReqDate = endDate
			};
		}

		private static DateTime AdjustDateTime(DateTime value)
		{
			// Get the server's current time zone
			TimeZoneInfo serverTimeZone = TimeZoneInfo.Local;
			DateTime serverNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, serverTimeZone);

			// Determine how to treat the 'value' based on its DateTimeKind
			DateTime valueInServerTimeZone;

			if (value.Kind == DateTimeKind.Utc)
			{
				// Value is in UTC, convert it to server time
				valueInServerTimeZone = TimeZoneInfo.ConvertTimeFromUtc(value, serverTimeZone);
			}
			else if (value.Kind == DateTimeKind.Local)
			{
				// Value is in local time (assumed to be the server's local time)
				valueInServerTimeZone = value;
			}
			else
			{
				// DateTimeKind.Unspecified, assuming it's in server time zone for lack of other info
				valueInServerTimeZone = value;
			}

			// If the value in server time is greater than the server's "now", set it to "now"
			if (valueInServerTimeZone > serverNow)
			{
				return serverNow;
			}

			return valueInServerTimeZone;
		}
	}
}