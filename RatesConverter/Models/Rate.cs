using System;
namespace RatesConverter.Models
{
	public class Rate
	{
        public int RateId { get; set; }
        public string? BaseCurrency { get; set; }
        public string? TargetCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}

