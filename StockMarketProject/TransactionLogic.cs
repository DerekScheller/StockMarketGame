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
        public void LiveDataPrint()
        {
            DataSet.StockSelect();
        }
        public void DataGenerator()
        {
            DataSet.FinancialData();
        }
        public decimal GetStockPrice(string Symbol)
        {
            decimal Price = 0m;
            foreach (StockProperties stock in DataSet.YahooStockList)
            {
                if (Symbol == stock.Symbol)
                {
                    Price = stock.Ask;
                }
            }
                return Price;
        }
        public Stock TransactionReferenceCreator(string Symbol, int Quantity)
        {
            string name = "Error";
            decimal price = 0m;
            foreach (StockProperties stock in DataSet.YahooStockList)
            {
                if (Symbol == stock.Symbol)
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
