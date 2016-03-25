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
        StatisticalEquations GetBids = new StatisticalEquations();
        YahooFinanceStockData DataSet = new YahooFinanceStockData();
        List<List<StockProperties>> FullDataToTest = new List<List<StockProperties>>();
        List<StockProperties> NewestDataPoint = new List<StockProperties>();
        List<Stock> BuyOrdersPlaced = new List<Stock>();
        List<Stock> SellOrdersPlaced = new List<Stock>();

        public void AITransactionCycle()
        {
            DataGenerator();
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
        public void SymbolSend(List<List<StockProperties>> DataToBreak)
        {
            int StockCount = 0;
            while(StockCount > 7)
            {
            foreach(List<StockProperties> Interval in DataToBreak)
            {
                foreach(StockProperties Datapoint in Interval)
                {
                   List<List<decimal>> TimePriceVol = new List<List<decimal>>();
                   TimePriceVol = ListCycler(DataToBreak, Datapoint.Symbol);
                   int Confidence = GetBids.BuyLongVsShortDeterminent(Datapoint);
                   SendBids(Confidence, Datapoint.Symbol);
                   StockCount++;
                }
            }
            }
        }
        public List<List<decimal>> ListCycler(List<List<StockProperties>> FullTestSet, string Symbol)
        {
            List<decimal> TimeInterval = new List<decimal>();
            List<decimal> Price = new List<decimal>();
            List<decimal> Volume = new List<decimal>();
            List<decimal> ChangeDaily = new List<decimal>();
            List<List<decimal>> ReturnForRegression = new List<List<decimal>>();
                foreach(List<StockProperties> Intervals in FullTestSet)
                {
                    foreach(StockProperties datapoint in Intervals)
                    {
                        if(Symbol == datapoint.Symbol)
                    {
                        TimeInterval.Add(datapoint.TimeStamp);
                        Price.Add(datapoint.Ask);
                        Volume.Add(datapoint.Volume);
                        ChangeDaily.Add(datapoint.ChangeDaily);
                    }
                    }
                }
            ReturnForRegression.Add(TimeInterval);
            ReturnForRegression.Add(Price);
            ReturnForRegression.Add(Volume);
            ReturnForRegression.Add(ChangeDaily);
            return ReturnForRegression;
        }

        public void AIStockBuyDecider()
        {
            foreach (StockProperties stock in DataSet.YahooStockList)
            {
                if (stock.PercentFromMoveAve > 0)
                {
                    if (stock.ChangeDaily > 0)
                    {
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
                            }
                            else if (percentGross > 5)
                            {

                            }
                        }
                    }
                }
            }
        }
        public void DataGenerator()
        {
            DataSet.FinancialData();
            NewestDataPoint.Clear();
            FullDataToTest.Clear();
            NewestDataPoint.AddRange(DataSet.SendNewestUpdate());
            FullDataToTest = DataSet.SendForRegressionModel();
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
        public void SendBids(int ShortLongWeight,string Symbol,)
        {
            switch (ShortLongWeight)
            {
                case -5:
                    stockbid1 = TransactionReferenceCreator(Symbol, (5 * 25));
                    BuyOrdersPlaced.Add
                case -4:
                case -3:
                case -2:
                case -1:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                default:
                    break;


            }
        }
        public List<decimal> BidCreator(List<List<decimal>> StockRegressionData,  string Symbol)
        {
            List<decimal> ListOfBids = GetBids.ReturnNextBid(StockRegressionData[0], StockRegressionData[1]);
            return ListOfBids;
        }

    }
}



