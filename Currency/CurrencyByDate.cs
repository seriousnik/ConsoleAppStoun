using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Currency
{
    class CurrencyByDate : Currency
    {
		public string Date { get; set; } //dateTime

		private string value;
		public string Value { get { return value; } set { this.value = value.Replace(',','.');  } } //double

	}
}
