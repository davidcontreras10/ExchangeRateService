using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class BccrAPICurrencyRepository(IHttpClientFactory httpClientFactory) : IBccrCurrencyRepository
    {
        public static BccrServiceType ServiceType => BccrServiceType.IndicatorsApi;

        public Task<IEnumerable<BccrSingleVentanillaModel>> GetIndicatorAsync(string indicator, DateTime initial, DateTime end)
        {
            throw new NotImplementedException();
        }

        private class BccrAPIResponse<T>
        {
            public bool Estado { get; set; }
            public string Mensaje { get; set; }
            public T Datos { get; set; }
        }

        private class BccrAPIIndicatorSeriesData
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
