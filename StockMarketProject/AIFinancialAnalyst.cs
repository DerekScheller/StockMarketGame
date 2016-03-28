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
        List<Stock> BuyOrders = new List<Stock>();
        List<Stock> SellOrdersPlaced = new List<Stock>();
        decimal CurrentPortfolioWeight;
        public void AITransactionCycle()
        {
            PrintAIPortfolio();
            DataGenerator();
            AttemptSale();
            MainBuyCall(FullDataToTest);
            VolumeBalance();
            MainSellCall();
            AttemptBuy();
            PrintAIPortfolio();
        }
        public void PrintAIPortfolio()
        {
            Console.WriteLine("Currently your portfolio make up is:");
            foreach (Stock stock in AIAccount.Portfolio)
            {
                Console.WriteLine(string.Format("{0} ({1}) Price Paid: {2} Quantity Owned: {3}", stock.name, stock.symbol, stock.price, stock.quantityowned));
            }
            Console.WriteLine("You currently have the following ShortHoldings that need to be repaid.");
            foreach(Stock stock in AIAccount.ShortHoldings)
            {
                Console.WriteLine(string.Format("{0} ({1}) Price Paid: {2} Quantity Owned: {3}", stock.name, stock.symbol, stock.price, stock.quantityowned));
            }
            Console.WriteLine("You currently have the following pending sell bids: ");
            foreach (Stock stock in SellOrdersPlaced)
            {
                Console.WriteLine(string.Format("{0} ({1}) Price Paid: {2} Quantity Owned: {3}", stock.name, stock.symbol, stock.price, stock.quantityowned));
            }
            Console.WriteLine("You currently have the following pending buy bids: ");
            foreach (Stock stock in BuyOrders)
            {
                Console.WriteLine(string.Format("{0} ({1}) Price Paid: {2} Quantity Owned: {3}", stock.name, stock.symbol, stock.price, stock.quantityowned));
            }
        }
        public void VolumeBalance()
        {
            decimal balance = AIAccount.AccountBalance;
            decimal priceSum = 0m;
            int totalVolume = 0;
            int totalLongBuys = 0;
            foreach (Stock buyorder in BuyOrders)
            {
                if (!buyorder.heldShort)
                {
                    priceSum += buyorder.price;
                    totalLongBuys++;
                }
            }
            totalVolume = Convert.ToInt32(balance / priceSum);
            int sharesPerBuy = totalVolume / totalLongBuys;
            foreach (Stock buyorder in BuyOrders)
            {
                if (!buyorder.heldShort)
                {
                    buyorder.quantityowned = sharesPerBuy;
                }
            }
        }
        public void MainSellCall()
        {
            foreach (Stock buyOrder in BuyOrders)
            {
                Stock ProfitSale = new Stock();
                Stock TrailingStopSale = new Stock();
                if (!buyOrder.heldShort)
                {
                    ProfitSale = buyOrder;
                    TrailingStopSale = buyOrder;
                    ProfitSale.price += ProfitSale.price * .12m;
                    TrailingStopSale.price -= buyOrder.price * .1m;
                    ProfitSale.name = "Profit";
                    TrailingStopSale.name = "Stop";
                    SellOrdersPlaced.Add(ProfitSale);
                    SellOrdersPlaced.Add(TrailingStopSale);
                }
            }
        }
        public void MainBuyCall(List<List<StockProperties>> DataToBreak)
        {
            int StockCount = 0;
            while (StockCount > 7)
            {
                foreach (List<StockProperties> Interval in DataToBreak)
                {
                    foreach (StockProperties Datapoint in Interval)
                    {
                        List<decimal> bidPriceList = new List<decimal>();
                        List<List<decimal>> TimePriceVol = new List<List<decimal>>();
                        List<Stock> bidStockReference = new List<Stock>();
                        int indexPosition = 0;
                        TimePriceVol = ListCycler(DataToBreak, Datapoint.Symbol);
                        int Confidence = GetBids.BuyLongVsShortDeterminent(Datapoint);
                        bidPriceList = BidCreator(TimePriceVol, Datapoint.Symbol);
                        if (RiskAssesment(Confidence))
                        {
                            foreach (decimal bidprice in bidPriceList)
                            {
                                Stock stockApprovedToBuy = new Stock();
                                stockApprovedToBuy = TransactionReferenceCreator(Datapoint.Symbol, 0);
                                stockApprovedToBuy.price = bidprice;
                                if (Confidence < 0)
                                {
                                    stockApprovedToBuy.heldShort = true;
                                    stockApprovedToBuy.quantityowned = 100;
                                }
                                BuyOrders.Add(stockApprovedToBuy);
                                indexPosition++;
                            }
                        }
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
            foreach (List<StockProperties> Intervals in FullTestSet)
            {
                foreach (StockProperties datapoint in Intervals)
                {
                    if (Symbol == datapoint.Symbol)
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
        public bool RiskAssesment(int ShortLongWeight)
        {
            switch (ShortLongWeight)
            {
                case -5:
                    WeightPortfolioVolumeReturn(ShortLongWeight);
                    return true;
                case -4:
                    WeightPortfolioVolumeReturn(ShortLongWeight);
                    return true;
                case -3:
                    WeightPortfolioVolumeReturn(ShortLongWeight);
                    return true;
                case -2:
                    WeightPortfolioVolumeReturn(ShortLongWeight);
                    return true;
                case -1:
                    WeightPortfolioVolumeReturn(ShortLongWeight);
                    return true;
                case 5:
                    WeightPortfolioVolumeReturn(ShortLongWeight);
                    return true;
                case 6:
                    WeightPortfolioVolumeReturn(ShortLongWeight);
                    return true;
                case 7:
                    WeightPortfolioVolumeReturn(ShortLongWeight);
                    return true;
                case 8:
                    WeightPortfolioVolumeReturn(ShortLongWeight);
                    return true;
                case 9:
                    WeightPortfolioVolumeReturn(ShortLongWeight);
                    return true;
                default:
                    return false;
            }
        }
        public void WeightPortfolioVolumeReturn(int Confidence)
        {

            CurrentPortfolioWeight += Confidence;
        }
        public List<decimal> BidCreator(List<List<decimal>> StockRegressionData, string Symbol)
        {
            List<decimal> ListOfBids = GetBids.ReturnNextBid(StockRegressionData[0], StockRegressionData[1]);
            return ListOfBids;
        }
        public void AttemptBuy()
        {
            foreach (Stock buyorder in BuyOrders)
            {
                decimal cost = 0m;
                if (!buyorder.heldShort)
                {
                    cost = BuyingExpense(buyorder.symbol, buyorder.quantityowned);
                    AIAccount.BuyAI(buyorder, cost);
                }
                else
                {
                    cost = SellingProfit(buyorder.symbol, buyorder.quantityowned);
                    AIAccount.SellShortAI(buyorder, cost);
                }
            }
            BuyOrders.Clear();

        }
        public void AttemptSale()
        {
            decimal profit = 0m;
            foreach (Stock sellOrder in SellOrdersPlaced)
            {
                if (sellOrder.name == "Profit")
                {
                    foreach (Stock ownedStock in AIAccount.Portfolio)
                    {
                        if (sellOrder.symbol == ownedStock.symbol)
                        {
                            foreach (StockProperties RealtimePrice in NewestDataPoint)
                            {
                                if (sellOrder.price >= RealtimePrice.Ask)
                                {
                                    profit = SellingProfit(sellOrder.symbol, sellOrder.quantityowned);
                                    AIAccount.SellAI(sellOrder, profit);
                                    SellOrdersPlaced.Remove(sellOrder);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (Stock ownedStock in AIAccount.Portfolio)
                    {
                        if (sellOrder.symbol == ownedStock.symbol)
                        {
                            foreach (StockProperties RealtimePrice in NewestDataPoint)
                            {
                                if (sellOrder.price <= RealtimePrice.Ask)
                                {
                                    profit = SellingProfit(sellOrder.symbol, sellOrder.quantityowned);
                                    AIAccount.SellAI(sellOrder, profit);
                                    SellOrdersPlaced.Remove(sellOrder);
                                }
                            }
                        }
                    }
                }
            }

        }

    }
}