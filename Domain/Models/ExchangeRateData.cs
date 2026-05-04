
namespace Domain.Models
{
    public class ExchangeRateData
    {
        #region Attributes

        public decimal Purchase { get; set; }
        public ExchangeRateResult.ResultError ErrorType { get; set; }
        public int MethodId { get; set; }
        public decimal Sell { get; set; }
        public ExchangeRateResult.ResultType ResultTypeValue { get; set; }
        public bool Success { get; set; }

        #endregion
    }
}