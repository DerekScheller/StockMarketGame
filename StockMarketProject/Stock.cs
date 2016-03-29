using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class Stock
    {
        public string name { get; set; }
        public string symbol { get; set; }
        public int quantityowned { get; set; }
        public decimal price { get; set; }
        public bool heldShort { get; set; }
    }
}
