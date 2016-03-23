using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class AIFinancialAnalyst
    {
        Account AIAccount = new Account();
        YahooFinanceStockData DataSet = new YahooFinanceStockData();
        
        public void AITransactionCycle()
        {
            DataSet.FinancialData();
            AIStockSaleDecider();
            AIStockBuyDecider();
            PrintAIPortfolio();
        }
        public void PrintAIPortfolio()
        {
           foreach(Stock stock in AIAccount.Portfolio)
            {
                Console.WriteLine(string.Format("{0} ({1}) Price Paid: {2} Quantity Owned: {3}", stock.name, stock.symbol, stock.price, stock.quantityowned));
            }
        }

        //Create a boolean list of 8 props case switch if 8 are true buy 60% of possible 7 = 40% 6 = 25% 
           // 2 hold short 100, 1 hold short 250, 0 hold short 500, default do nothing 
          
        public void AIStockBuyDecider()
        {
            foreach (StockProperties stock in DataSet.YahooStockList)
            {
                if (stock.PercentFromMoveAve > 0)
                {
                    if (stock.ChangeDaily > 0)
                    {
                        AIAccount.BuyAI(stock.Symbol);
                    }
                }
            }
        }
        public void AIStockSaleDecider()
        {
            foreach (Stock stock in AIAccount.Portfolio)
            {
                foreach (StockProperties realStockPrice in DataSet.YahooStockList)
                {
                    if (stock.symbol == realStockPrice.Symbol)
                    {
                        if (stock.price < realStockPrice.Ask)
                        {
                            decimal percentGross = realStockPrice.Ask - stock.price / stock.price * 100;
                            if (2 < percentGross && percentGross < 5)
                            {
                                AIAccount.SellAI(stock.symbol, (stock.quantityowned / 2));
                            }
                            else if (percentGross > 5)
                            {
                                AIAccount.SellAI(stock.symbol, stock.quantityowned);
                            }
                        }
                    }
                }
            }
        }

    }
}
