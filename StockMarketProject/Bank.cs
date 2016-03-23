using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class Bank
    {
        TransactionLogic transaction = new TransactionLogic();
        public List<Stock> StocksLentShort = new List<Stock>();
        YahooFinanceStockData StockPopList = new YahooFinanceStockData();
        public Bank()
        {
        }
        public void LendShort(string Symbol, int Quantity)
        {
           StocksLentShort.Add(transaction.TransactionReferenceCreator(Symbol, Quantity));
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
