using System;
namespace RatesConverter.Models
{
	public class ExchangeRate
	{
		public string Currency { get; set; } = string.Empty;
		public decimal Rate { get; set; }
	}
}

