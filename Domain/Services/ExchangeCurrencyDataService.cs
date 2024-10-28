using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Models;
using DataAccess;
using Domain.Repositories;

namespace Domain.Services
{
	public class ExchangeCurrencyDataService : SqlServerBaseService, IExchangeCurrencyDataService
	{
		#region Private Attributes

		private readonly IBccrCurrencyService _bccrWebService;
		private readonly IBccrCodesRepository _bccrCodesRepository;
		private readonly IBccrCodesDbCache _bccrCodesDbCache;

		#endregion

		#region Constructor

		public ExchangeCurrencyDataService(
			IBccrCodesDbCache bccrCodesDbCache,
			IConnectionConfig connectionConfig,
			IBccrCurrencyService bccrCurrencyService,
			IBccrCodesRepository bccrCodesRepository) : base(connectionConfig)
		{
			System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			ServicePointManager
					.ServerCertificateValidationCallback +=
				(sender, cert, chain, sslPolicyErrors) => true;
			_bccrWebService = bccrCurrencyService;
			_bccrCodesRepository = bccrCodesRepository;
			_bccrCodesDbCache = bccrCodesDbCache;
		}

		#endregion

		#region Public Methods

		public EntityMethodInfo GetEntityMethodInfo(int methodId)
		{
			var cache = _bccrCodesDbCache.GetEntityMethodInfo(methodId);
			if (cache != null)
				return cache;

			var res = _bccrCodesRepository.GetEntityMethodInfo(methodId);
			_bccrCodesDbCache.SetEntityMethodInfo(methodId, res);
			return res;
		}

		public async Task<IEnumerable<BccrVentanillaModel>> GetBccrVentanillaModelAsync(string entityName, DateTime dateTime)
		{
			return await GetBccrVentanillaModelWebServiceAsync(entityName, dateTime);
		}

		#endregion

		#region Private Methods

		private async Task<IEnumerable<BccrVentanillaModel>> GetBccrVentanillaModelWebServiceAsync(string entityName, DateTime dateTime)
		{
			var codes = GetBccrWebServiceExchangeCodeByEntity(entityName);
			var sellList = await _bccrWebService.GetBccrSingleVentanillaModelsAsync(codes["sell"], dateTime);
			var purchaseList = await _bccrWebService.GetBccrSingleVentanillaModelsAsync(codes["purchase"], dateTime);
			var list = CreateBccrVentanillaModel(sellList, purchaseList);
			return list;
		}

		private Dictionary<string, string> GetBccrWebServiceExchangeCodeByEntity(string entityName)
		{
			var cache = _bccrCodesDbCache.GetBccrWebServiceExchangeCodeByEntity(entityName);
			if (cache != null)
				return cache;

			var res = _bccrCodesRepository.GetBccrWebServiceExchangeCodeByEntity(entityName);
			_bccrCodesDbCache.SetBccrWebServiceExchangeCodeByEntity(entityName, res);
			return res;
		}


		private static IEnumerable<BccrVentanillaModel> CreateBccrVentanillaModel(IEnumerable<BccrSingleVentanillaModel> sellList,
			IEnumerable<BccrSingleVentanillaModel> purchaseList)
		{
			var list = new List<BccrVentanillaModel>();
			if (sellList == null || purchaseList == null || sellList.Count() != purchaseList.Count())
				throw new ArgumentException("Lists must match or cannot be null");
			foreach (var singleSell in sellList)
			{
				var singlePurchase = purchaseList.FirstOrDefault(item => item.LastUpdate == singleSell.LastUpdate);
				if (singlePurchase == null)
					throw new Exception("Invalid BccrSingleVentanillaModel result");
				var model = new BccrVentanillaModel
				{
					LastUpdate = singleSell.LastUpdate,
					Purchase = singlePurchase.Value,
					Sell = singleSell.Value
				};
				list.Add(model);
			}
			return list;
		}

	}

	#endregion
}