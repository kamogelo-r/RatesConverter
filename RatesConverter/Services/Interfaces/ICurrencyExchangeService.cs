using System;
namespace RatesConverter.Models
{
	public interface ICurrencyExchangeService
	{
        Task<List<ExchangeRate>> GetExchangeRate(string currency);
    }
}

