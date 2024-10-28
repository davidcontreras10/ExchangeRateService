using DataAccess;
using Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using Ut = DContre.MyFinance.StUtilities.SystemDataUtilities;

namespace Domain.Repositories
{
	public class BccrCodesRepository(IConnectionConfig config) : SqlServerBaseService(config), IBccrCodesRepository
	{
		public EntityMethodInfo GetEntityMethodInfo(int methodId)
		{
			var methodIdParameter = new SqlParameter("@pMethodId", methodId);
			var dataSet = ExecuteStoredProcedure("EntityNameKeyByMethodIdList", methodIdParameter);
			if (dataSet == null || dataSet.Tables.Count == 0)
				return null;
			return CreateEntityMethodInfo(dataSet.Tables[0]);
		}

		public Dictionary<string, string> GetBccrWebServiceExchangeCodeByEntity(string entityName)
		{
			var entityParameter = new SqlParameter("@pEntityName", entityName);
			var dataSet = ExecuteStoredProcedure("EntityIndicatorCodesByEntityNameList", entityParameter);
			if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count < 1)
				throw new BccrWebServiceEntityNotFoundException(entityName);
			var dataRow = dataSet.Tables[0].Rows[0];
			var dictionary = new Dictionary<string, string>
			{
				{"sell", Ut.GetString(dataRow, "SellCode")},
				{"purchase", Ut.GetString(dataRow, "PurchaseCode")}
			};
			return dictionary;
		}

		private static EntityMethodInfo CreateEntityMethodInfo(DataTable dataTable)
		{
			return dataTable == null || dataTable.Rows.Count == 0 ? null : CreateEntityMethodInfo(dataTable.Rows[0]);
		}

		private static EntityMethodInfo CreateEntityMethodInfo(DataRow dataRow)
		{
			ArgumentNullException.ThrowIfNull(dataRow);
			return new EntityMethodInfo
			{
				Colones = Ut.GetBool(dataRow, "Colones"),
				EntityName = Ut.GetString(dataRow, "EntityName"),
				EntitySearchKey = Ut.GetString(dataRow, "EntitySearchKey")
			};
		}
	}
}
