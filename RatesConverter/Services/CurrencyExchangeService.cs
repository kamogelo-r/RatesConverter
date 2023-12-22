using System;
using System.Text.Json;

namespace RatesConverter.Models
{
	public class CurrencyExchangeService : ICurrencyExchangeService
	{
        private static readonly HttpClient _client;
        private readonly string _appId = ConfigurationManager.AppSetting["AppId"];

        static CurrencyExchangeService()
		{
            _client = new HttpClient()
            {
                BaseAddress = new Uri("https://openexchangerates.org/")
            };
        }

        public async Task<List<ExchangeRate>> GetExchangeRate(string currency)
        {
            var url = string.Format($"/api/latest.json?app_id={0}&base={1}", _appId, currency);

            var result = new List<ExchangeRate>();
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();

                result = JsonSerializer.Deserialize<List<ExchangeRate>>(stringResponse, new JsonSerializerOptions()
                                                            { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            return result ?? new List<ExchangeRate>();
        }
    }
}

