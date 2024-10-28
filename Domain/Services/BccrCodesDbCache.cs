using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Services
{
	public class BccrCodesDbCache(IAppMemoryCache appMemoryCache) : IBccrCodesDbCache
	{
		private const int DefaultCacheExpirationInHours = 3;

		public Dictionary<string, string> GetBccrWebServiceExchangeCodeByEntity(string entityName)
		{
			var key = GetCodesKey(entityName);
			return appMemoryCache.Get<Dictionary<string, string>>(key);
		}

		public void SetBccrWebServiceExchangeCodeByEntity(string entityName, Dictionary<string, string> codes)
		{
			ArgumentNullException.ThrowIfNull(codes);
			var key = GetCodesKey(entityName);
			appMemoryCache.Set(key, codes, TimeSpan.FromHours(DefaultCacheExpirationInHours));
		}

		public EntityMethodInfo GetEntityMethodInfo(int methodId)
		{
			var key = GetEntityMethodInfoKey(methodId);
			return appMemoryCache.Get<EntityMethodInfo>(key);
		}

		public void SetEntityMethodInfo(int methodId, EntityMethodInfo entityMethodInfo)
		{
			ArgumentNullException.ThrowIfNull(entityMethodInfo);
			var key = GetEntityMethodInfoKey(methodId);
			appMemoryCache.Set(key, entityMethodInfo, TimeSpan.FromHours(DefaultCacheExpirationInHours));
		}

		private static string GetCodesKey(string entityName)
		{
			return $"BccrCodes_{entityName}";
		}

		public static string GetEntityMethodInfoKey(int methodId)
		{
			return $"EntityMethodInfo_{methodId}";
		}

	}
}
