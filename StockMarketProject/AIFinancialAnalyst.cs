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
        List<List<StockProperties>> FullDataToTest = new List<List<StockProperties>>();

        public void AITransactionCycle()
        {
            DataSet.FinancialData();
            FullDataToTest = DataSet.SendForRegressionModel();
            AIStockSaleDecider();
            AIStockBuyDecider();
            PrintAIPortfolio();
        }
        public void PrintAIPortfolio()
        {
            foreach (Stock stock in AIAccount.Portfolio)
            {
                Console.WriteLine(string.Format("{0} ({1}) Price Paid: {2} Quantity Owned: {3}", stock.name, stock.symbol, stock.price, stock.quantityowned));
            }
        }
        public void StockRanker()
        {

        }


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



