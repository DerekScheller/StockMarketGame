using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class Stock
    {
        public string name;
        public string symbol;
        public int quantityowned;
        public decimal price;
        public Stock(string Name, string Symbol, int Quantityowned, decimal Price)
        {
           string name = Name;
           string symbol = Symbol;
            int quantityowned = Quantityowned;
            decimal price = Price;
        }
    }
}
