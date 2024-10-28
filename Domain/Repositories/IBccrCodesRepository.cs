using Domain.Models;
using System.Collections.Generic;

namespace Domain.Repositories
{
	public interface IBccrCodesRepository
	{
		Dictionary<string, string> GetBccrWebServiceExchangeCodeByEntity(string entityName);
		EntityMethodInfo GetEntityMethodInfo(int methodId);
	}
}