using DContre.MyFinance.StUtilities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public static class BccrCurrencyRepositoryUtils
    {
        public static IEnumerable<BccrSingleVentanillaModel> Convert(DataSet dataSet, int expectedIndex)
        {
            if (dataSet == null || dataSet.Tables.Count < expectedIndex + 1)
            {
                return [];
            }

            var dataTable = dataSet.Tables[expectedIndex];
            return CreateBccrSingleVentanillaModel(dataTable);
        }

        private static IEnumerable<BccrSingleVentanillaModel> CreateBccrSingleVentanillaModel(DataTable dataTable)
        {
            if (dataTable == null)
            {
                return [];
            }
            var enumerable = dataTable.Rows.Cast<DataRow>();
            var list = new List<BccrSingleVentanillaModel>();
            foreach (DataRow row in enumerable)
            {
                list.Add(CreateBccrSingleVentanillaModel(row));
            }
            return list;
        }
        private static BccrSingleVentanillaModel CreateBccrSingleVentanillaModel(DataRow dataRow)
        {
            ArgumentNullException.ThrowIfNull(dataRow);
            var value = SystemDataUtilities.GetFloat(dataRow, "NUM_VALOR");
            var lastUpdate = SystemDataUtilities.GetDateTime(dataRow, "DES_FECHA");
            return new BccrSingleVentanillaModel
            {
                LastUpdate = lastUpdate,
                Value = (decimal)value
            };
        }
    }
}
