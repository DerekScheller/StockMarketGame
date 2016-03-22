using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    class Program
    { 
        static void Main(string[] args)
        {
            YahooFinanceStockData test = new YahooFinanceStockData();
            test.FinancialData();
            test.StockSelect();
            Console.ReadLine();
        }
    }
}
