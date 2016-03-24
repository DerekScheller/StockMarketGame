using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class TransactionLogic
    {
        YahooFinanceStockData DataSet = new YahooFinanceStockData();
        List<StockProperties> NewestDataPoint = new List<StockProperties>();
        public void DataGenerator()
        {
            NewestDataPoint.Clear();
            NewestDataPoint.AddRange(DataSet.SendNewestUpdate());
        }
        public decimal GetStockPrice(string Symbol)
        {
            DataGenerator();
            decimal Price = 0m;
            foreach (StockProperties stock in NewestDataPoint)
            {
                if (stock.Symbol.Contains(Symbol))
                {
                    Price = stock.Ask;
                }
            }
            return Price;
        }
        public Stock TransactionReferenceCreator(string Symbol, int Quantity)
        {
            DataGenerator();
            string name = "Error";
            decimal price = 0m;
            foreach (StockProperties stock in NewestDataPoint)
            {
                if (stock.Symbol.Contains(Symbol))
                {
                    name = stock.Name;
                    price = stock.Ask;
                }
            }
            Stock BuiltStock = new Stock();
            BuiltStock.name = name;
            BuiltStock.price = price;
            BuiltStock.symbol = Symbol;
            BuiltStock.quantityowned = Quantity;
            return BuiltStock;
        }
        public decimal SellingProfit(string Symbol, int Quantity)
        {
            Stock CurrentData = TransactionReferenceCreator(Symbol, Quantity);
            decimal TotalSale = CurrentData.price * Quantity;
            return TotalSale;
        }
        public decimal BuyingExpense(string Symbol, int Quantity)
        {
            Stock CurrentData = TransactionReferenceCreator(Symbol, Quantity);
            decimal TotalSale = CurrentData.price * Quantity;
            return TotalSale;
        }
    }
}
