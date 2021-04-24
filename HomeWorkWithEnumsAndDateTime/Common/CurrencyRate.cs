using HomeWorkWithEnumsAndDateTime.Enums;

namespace HomeWorkWithEnumsAndDateTime.Common
{
    /// <summary>
    /// Курсы валют
    /// </summary>
    public struct CurrencyRate
    {
        public Currency Currency;
        public decimal Value;

        public CurrencyRate(Currency currency, decimal value)
        {
            Currency = currency;
            Value = value;
        }
    }
}
