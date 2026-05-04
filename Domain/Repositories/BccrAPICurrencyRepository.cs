using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiBaseConsumer;

namespace Domain.Repositories
{
    public class BccrAPICurrencyRepository(IHttpClientFactory httpClientFactory, IProjectSettings projectSettings) : WebApiBaseService(httpClientFactory), IBccrCurrencyRepository
    {
        protected override string ControllerName => string.Empty;

        public async Task<IEnumerable<BccrSingleVentanillaModel>> GetIndicatorAsync(string indicator, DateTime initial, DateTime end)
        {
            var response = await GetSeriesIndicadoresEconomicosResponse(indicator, initial, end);
            throw new NotImplementedException();
        }

        private async Task<BccrAPIResponse<BccrAPIIndicatorSeriesData>> GetSeriesIndicadoresEconomicosResponse(string indicator, DateTime initial, DateTime end) 
        {
            var methodUrlBase = $"indicadoresEconomicos/{indicator}/series";
            var queryParams = new Dictionary<string, object>
            {
                { "fechaInicio", initial.ToString("yyyy/MM/dd") },
                { "fechaFin", end.ToString("yyyy/MM/dd") },
                {"idioma", "es"}
            };

            var url = projectSettings.BccrIndicadorAPIBaseUrl + CreateMethodUrl(methodUrlBase, queryParams);
            var token = projectSettings.BccrIndicadoresEconomicosToken;
            var request = new WebApiRequest(url, HttpMethod.Get)
            {
                Headers = new Dictionary<string, string>
                {
                    {"Connection", "close"},
                    {"Accept", "*/*"},
                    {"Cache-Control", "no-cache"},
                    {"Access-Control-Allow-Origin", "*"}
                },
                AccessToken = token,
                UseControllerBaseUrl = false
            };

            var response = await GetResponseAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();
            var newtonsoftDeserializedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<BccrAPIResponse<BccrAPIIndicatorSeriesData>>(responseData);
            return newtonsoftDeserializedResponse;
        }

        protected override string GetApiBaseDomain()
        {
            return projectSettings.BccrIndicadorAPIBaseUrl;
        }

        public class BccrAPIResponse<T>
        {
            public bool Estado { get; set; }
            public string Mensaje { get; set; }
            public T[] Datos { get; set; }
        }

        public class BccrAPIIndicatorSeriesData
        {
            public string CodigoIndicador { get; set; }
            public string NombreIndicador { get; set; }

            public IReadOnlyCollection<IndicatorSerie> Series { get; set; } = [];

            public class IndicatorSerie
            {
                public DateTime Fecha { get; set; }
                public decimal ValorDatoPorPeriodo { get; set; }
            }

        }

    }
}
