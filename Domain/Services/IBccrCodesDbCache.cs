using Domain.Models;
using System.Collections.Generic;

namespace Domain.Services
{
	public interface IBccrCodesDbCache
	{
		Dictionary<string, string> GetBccrWebServiceExchangeCodeByEntity(string entityName);
		EntityMethodInfo GetEntityMethodInfo(int methodId);
		void SetBccrWebServiceExchangeCodeByEntity(string entityName, Dictionary<string, string> codes);
		void SetEntityMethodInfo(int methodId, EntityMethodInfo entityMethodInfo);
	}
}