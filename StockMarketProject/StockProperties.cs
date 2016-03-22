using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class StockProperties
    { 
            public string Name { get; set; }  
            public string Symbol { get; set; }  
            public decimal PreviousClose { get; set; }
            public decimal Open { get; set; }
            public decimal PercentFromMoveAve { get; set; }
            public decimal ChangeDaily { get; set; }
            public decimal Ask { get; set; }
            public decimal ShortRatio { get; set; } 
        }
}
