using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class TransactionLogic : YahooFinanceStockData
    {   
        public decimal BuyingSubtraction(int Quantity, decimal Price)
        {
            decimal TotalPrice = Quantity * Price;
            return TotalPrice;
        }
        public decimal SellingAddition(int Quantity, decimal Price)
        {
            decimal TotalGain = Quantity * Price;
            return TotalGain;
        }
        public decimal GetStockPrice(string Symbol)
        {
            decimal Price = 0m;
            foreach (StockProperties stock in Stock)
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
            String name;
            decimal price;
            foreach (StockProperties stock in Stock)
            {
                if (Symbol == stock.Symbol)
                {
                    name = stock.Name;
                    price = stock.Ask;
                }
            }
            Stock BuiltStock = new Stock(name, Symbol, Quantity, price);
            return BuiltStock;
        }
        public void SellingLogic()
        {

        }
    }
}
