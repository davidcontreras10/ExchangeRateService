using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Services
{
	public class BccrCodesDbCache(IAppMemoryCache appMemoryCache, IProjectSettings projectSettings) : IBccrCodesDbCache
	{
		public Dictionary<string, string> GetBccrWebServiceExchangeCodeByEntity(string entityName)
		{
			var key = GetCodesKey(entityName);
			return appMemoryCache.Get<Dictionary<string, string>>(key);
		}

		public void SetBccrWebServiceExchangeCodeByEntity(string entityName, Dictionary<string, string> codes)
		{
			ArgumentNullException.ThrowIfNull(codes);
			var key = GetCodesKey(entityName);
			appMemoryCache.Set(key, codes, TimeSpan.FromHours(projectSettings.BccrCodesDbCacheHours));
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
			appMemoryCache.Set(key, entityMethodInfo, TimeSpan.FromHours(projectSettings.BccrCodesDbCacheHours));
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
