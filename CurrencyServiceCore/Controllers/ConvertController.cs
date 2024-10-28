﻿using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace CurrencyServiceCore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConvertController : ControllerBase
	{
		private readonly IDolarColonesBccrService _dolarColonesBccrService;

		public ConvertController(IDolarColonesBccrService dolarColonesBccrService)
		{
			_dolarColonesBccrService = dolarColonesBccrService;
		}

		[Route("ExchangeRateResultByMethodId")]
		[HttpGet]
		public async Task<ExchangeRateResult> ExchangeRateResultByMethodId(int methodId, DateTime dateTime, bool isPurchase)
		{
			System.Diagnostics.Trace.TraceInformation("ExchangeRateResultByMethodId called Log");
			try
			{
				return await GetExchangeRateResultAsync(methodId, dateTime, isPurchase);
			}
			catch (Exception e)
			{

				System.Diagnostics.Trace.TraceError($"Exchange Error:{Environment.NewLine}{e}");
				throw;
			}

		}

		[Route("ExchangeRateResultByMethodIds")]
		[HttpPost]
		public async Task<IEnumerable<ExchangeRateResult>> ExchangeRateResultByMethodIds([FromBody] ExchangeRateResultModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (model.MethodIds == null || !model.MethodIds.Any())
				throw new ArgumentException("Cannot be null or empty", nameof(model));
			var list = new List<ExchangeRateResult>();
			foreach (var request in model.MethodIds)
			{
				var dateTime = request.DateTime ?? model.DateTime;
				list.Add(await GetExchangeRateResultAsync(request.Id, dateTime, request.IsPurchase));
			}
			return list;
		}

		private async Task<ExchangeRateResult> GetExchangeRateResultAsync(int methodId, DateTime dateTime, bool isPurchase)
		{
			if (methodId == 0)
			{
				throw new ArgumentException("Cannot be 0", nameof(methodId));
			}

			var result = await _dolarColonesBccrService.GetExchangeRateResultByMethodIdAsync(methodId, isPurchase, dateTime);
			result.MethodId = methodId;
			return result;
		}
	}
}
