using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RatesConverter.Models;

namespace RatesConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly RatesDbContext _ratesDbContext;
        private readonly ICacheService _cacheService;
        private readonly ICurrencyExchangeService _currencyExchangeService;

        public RatesController(RatesDbContext dbContext, ICacheService cacheService,
            ICurrencyExchangeService currencyExchangeService)
        {
            _ratesDbContext = dbContext;
            _cacheService = cacheService;
            _currencyExchangeService = currencyExchangeService;
        }

        // GET: api/Rates
        [HttpGet("GetCurrencyConversion")]
        public async Task<Rate> Get(string baseCurrency, string targetCurrency, decimal amount)
        {
            var cacheData = _cacheService.GetData<IEnumerable<Rate>>("rate");
            if (cacheData != null)
            {
                var cacheResult = cacheData.FirstOrDefault(x => x.BaseCurrency == baseCurrency.ToUpper() &&
                                                           x.TargetCurrency == targetCurrency.ToUpper()); // get cache data

                if (cacheResult == null)
                {
                    var apiExchageRateCurrencies = new List<ExchangeRate>();
                    apiExchageRateCurrencies = await _currencyExchangeService.GetExchangeRate(baseCurrency); // get api data

                    var rateConversionResult = GetRateConversionResult(baseCurrency, targetCurrency, amount, apiExchageRateCurrencies);

                    var expirationTime = DateTimeOffset.Now.AddMinutes(15);

                    cacheData = _ratesDbContext.Rates.ToList(); // add to db

                    _cacheService.SetData("rate", cacheData, expirationTime);

                    return rateConversionResult;
                }
            }

            return new Rate();
        }

        // GET: api/Rates/
        [HttpGet("GetRateConversionHistory")]
        public Rate Get(string baseCurrency, string targetCurrency)
        {
            var cacheData = _cacheService.GetData<IEnumerable<Rate>>("rate");
            if (cacheData != null)
            {
                var rateResult = cacheData.FirstOrDefault(x => x.BaseCurrency == baseCurrency.ToUpper() &&
                                                               x.TargetCurrency == targetCurrency.ToUpper());

                return rateResult ?? throw new Exception();
            }

            return new Rate();
        }


        private static Rate GetRateConversionResult(string baseCurrency, string targetCurrency,
                                                decimal amount, List<ExchangeRate> apiExchageRateCurrencies)
        {
            var exchangeRate = apiExchageRateCurrencies.FirstOrDefault(x => x.Currency == baseCurrency);

            var conversionResult = amount * exchangeRate.Rate;

            var rate = new Rate
            {
                Amount = conversionResult,
                BaseCurrency = baseCurrency,
                TargetCurrency = targetCurrency
            };

            return rate;
        }
    }
}
