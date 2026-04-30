using DContre.MyFinance.StUtilities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebApiBaseConsumer;

namespace Domain.Repositories
{
    public class BccrBridgeWebServiceRepository(IHttpClientFactory httpClientFactory) : WebApiBaseService(httpClientFactory), IBccrCurrencyRepository
    {

        protected override string ControllerName => string.Empty;

        public static BccrServiceType ServiceType => BccrServiceType.BridgedWebService;

        public async Task<IEnumerable<BccrSingleVentanillaModel>> GetIndicatorAsync(string indicator, DateTime initial, DateTime end)
        {
            var inicio = initial.ToString("dd/MM/yyyy");
            var final = end.ToString("dd/MM/yyyy");
            var url = CreateRootUrl(new Dictionary<string, object>
            {
                { "Indicador", indicator },
                { "FechaInicio", inicio },
                { "FechaFinal", final }
			});
            var request = new WebApiRequest(url, HttpMethod.Get)
            {
                Headers = new Dictionary<string, string>
                {
                    {"User-Agent", "PostmanRuntime/7.29.2"},
                    {"Connection", "close"},
                    {"Accept", "*/*"},
                    {"Cache-Control", "no-cache"},
                    {"Access-Control-Allow-Origin", "*"}
                }
            };
            var response = await GetResponseAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            jsonResponse = HttpUtility.HtmlDecode(jsonResponse);
            var reader = new StringReader(jsonResponse);
            var theDataSet = new DataSet();
            theDataSet.ReadXml(reader);
            return Convert(theDataSet);
        }

        private static IEnumerable<BccrSingleVentanillaModel> Convert(DataSet dataSet)
        {
            if (dataSet == null || dataSet.Tables.Count < 2)
            {
                return [];
            }

            var dataTable = dataSet.Tables[1];
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
                Value = value
            };
        }

        protected override string GetApiBaseDomain()
        {
            return "https://myfinance-363522.ue.r.appspot.com/currency";
        }
    }
}
