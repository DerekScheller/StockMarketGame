using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class Account
    {
        public decimal AccountBalance = 10000m;
        public List<Stock> Portfolio = new List<Stock>();
        public List<Stock> ShortHoldings = new List<Stock>();
    }
}
