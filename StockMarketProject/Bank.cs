using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class Bank
    {
        public List<Stock> StocksLentShort = new List<Stock>();
        public Bank()
        {
        }
        public void LendShort(Stock shortAquisition)
        {
           StocksLentShort.Add(shortAquisition);
        }
        public void ReturnReminder()
        {
            foreach(Stock stock in StocksLentShort)
            {
                Console.WriteLine("You currently owe the bank " + stock.quantityowned + " shares of " + stock.name+ ".");
            }
        }
        public void ShortStockReturn(string Symbol, int Quantity)
        {
            foreach (Stock stock in StocksLentShort)
            {
                if (Symbol == stock.symbol)
                {
                    stock.quantityowned -= Quantity;
                    if(stock.quantityowned == 0)
                    {
                        StocksLentShort.Remove(stock);
                    }
                }
            }
        }
    }
}
