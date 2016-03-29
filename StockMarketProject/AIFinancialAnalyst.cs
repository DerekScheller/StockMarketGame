using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace StockMarketProject
{
    public class AIFinancialAnalyst
    {
        Account AIAccount = new Account();
        Bank bank = new Bank();
        StatisticalEquations GetBids = new StatisticalEquations();
        YahooFinanceStockData DataSet = new YahooFinanceStockData();
        List<List<StockProperties>> FullDataToTest = new List<List<StockProperties>>();
        List<StockProperties> NewestDataPoint = new List<StockProperties>();
        List<Stock> BuyOrders = new List<Stock>();
        List<Stock> SellOrdersPlaced = new List<Stock>();
        decimal CurrentPortfolioWeight;
        public void AITransactionCycle()
        {
            int BuySellPause = 0;
            PrintAIPortfolio();
            DataGenerator();
            AttemptSale();
            MainBuyCall(FullDataToTest);
            VolumeBalance();
            MainSellCall(BuyOrders);
            AttemptBuy();
            PrintAIPortfolio();
            while (BuySellPause < 10)
            {
                System.Threading.Thread.Sleep(30000);
                AttemptBuy();
                AttemptSale();
                PrintAIPortfolio();
            }
            AITransactionCycle();
        }
        public void PrintAIPortfolio()
        {
            Console.WriteLine("Your current liquid assets are:" + AIAccount.AccountBalance);
            Console.WriteLine("Currently your portfolio make up is:");
            foreach (Stock stock in AIAccount.Portfolio)
            {
                Console.WriteLine(string.Format("{0} ({1}) Price Paid: {2} Quantity Owned: {3}", stock.name, stock.symbol, stock.price, stock.quantityowned));
            }
            Console.WriteLine("You currently have the following ShortHoldings that need to be repaid.");
            foreach (Stock stock in AIAccount.ShortHoldings)
            {
                Console.WriteLine(string.Format("{0} ({1}) Original Loan Quantity: {2}", stock.name, stock.symbol, stock.quantityowned));
            }
            Console.WriteLine("You currently have the following pending sell bids: ");

            foreach (Stock stock in SellOrdersPlaced)
            {
                if (PreOwnedStock(stock.symbol))
                {
                    Console.WriteLine(string.Format("Type of sell bid: {0} ({1}) Sale Price: {2} Quantity To Sell: {3}", stock.name, stock.symbol, stock.price, stock.quantityowned));
                }
            }
            Console.WriteLine("You currently have the following pending buy bids: ");
            foreach (Stock stock in BuyOrders)
            {
                Console.WriteLine(string.Format("{0} ({1}) Bid Ask Price: {2} Bid Volume: {3}", stock.name, stock.symbol, stock.price, stock.quantityowned));
            }
        }
        public void VolumeBalance()
        {
            decimal balance = AIAccount.AccountBalance;
            decimal priceSum = 0m;
            int totalVolume = 0;
            int totalLongBuys = 1;
            int sharesPerBuy = 0;
            foreach (Stock buyorder in BuyOrders)
            {
                if (!buyorder.heldShort)
                {
                    priceSum += buyorder.price;
                    totalLongBuys++;
                }
            }
            if (priceSum != 0m)
            {
                priceSum = priceSum / totalLongBuys;
                totalVolume = Convert.ToInt32(balance / priceSum);
                sharesPerBuy = totalVolume / totalLongBuys;
            }
            foreach (Stock buyorder in BuyOrders)
            {
                if (!buyorder.heldShort)
                {
                    buyorder.quantityowned = sharesPerBuy;
                }
            }
        }
        public void MainSellCall(List<Stock> PurchasePointsToProtect)
        {
            foreach (Stock buyOrder in PurchasePointsToProtect)
            {
                if (!buyOrder.heldShort)
                {
                    PositiveReturn(buyOrder);
                    TrailingStopPlacement(buyOrder);
                }
            }
        }
        public void PositiveReturn(Stock PurchasePointToProtect)
        {
            Stock ProfitSale = new Stock();
            ProfitSale.name = "Profit";
            ProfitSale.price = PurchasePointToProtect.price * 1.12m;
            ProfitSale.symbol = PurchasePointToProtect.symbol;
            ProfitSale.quantityowned = PurchasePointToProtect.quantityowned;
            SellOrdersPlaced.Add(ProfitSale);
        }
        public void TrailingStopPlacement(Stock PurchasePointToProtect)
        {
            Stock TrailingStopSale = new Stock();
            TrailingStopSale.name = "Stop";
            TrailingStopSale.price = PurchasePointToProtect.price * .9m;
            TrailingStopSale.symbol = PurchasePointToProtect.symbol;
            TrailingStopSale.quantityowned = PurchasePointToProtect.quantityowned;
            SellOrdersPlaced.Add(TrailingStopSale);
        }
        public void MainBuyCall(List<List<StockProperties>> DataToBreak)
        {
            List<StockProperties> Interval = new List<StockProperties>();
            Interval = DataToBreak.ElementAt(DataToBreak.Count - 1);
            foreach (StockProperties Datapoint in Interval)
            {
                List<decimal> bidPriceList = new List<decimal>();
                List<List<decimal>> TimePriceVol = new List<List<decimal>>();
                List<Stock> bidStockReference = new List<Stock>();
                TimePriceVol = ListCycler(DataToBreak, Datapoint.Symbol);
                bidPriceList = BidCreator(TimePriceVol);
                decimal RValue = RegressionStrength(TimePriceVol);
                int Confidence = GetBids.BuyLongVsShortDeterminent(Datapoint);
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
            DataSet.MainIntervalPullToWrite();
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
            NewestDataPoint.Clear();
            NewestDataPoint.AddRange(DataSet.SendNewestUpdate());
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
        public List<decimal> BidCreator(List<List<decimal>> StockRegressionData)
        {
            List<decimal> ListOfBids = GetBids.ReturnNextBid(StockRegressionData[0], StockRegressionData[1]);
            return ListOfBids;
        }
        public decimal RegressionStrength(List<List<decimal>> StockRegressionData)
        {
            decimal CorrelationCoefficient = GetBids.CorrelationCoefficientR(StockRegressionData[0], StockRegressionData[1]);
            return CorrelationCoefficient;
        }
        public void AttemptBuy()
        {
            List<StockProperties> Realtime = new List<StockProperties>();
            Realtime = DataSet.SendNewestUpdate();
            foreach (Stock buyorder in BuyOrders)
            {
                foreach (StockProperties realTimeTicker in Realtime)
                {
                    if (realTimeTicker.Symbol == buyorder.symbol && realTimeTicker.Ask <= buyorder.price)
                    {

                        decimal cost = 0m;
                        if (!buyorder.heldShort)
                        {
                            cost = BuyingExpense(buyorder.symbol, buyorder.quantityowned);
                            BuyFinalizedAccountSend(buyorder, cost);
                        }
                        else
                        {
                            cost = SellingProfit(buyorder.symbol, buyorder.quantityowned);
                            SellShortAccountUpdate(buyorder, cost);
                        }
                        BuyOrders.Remove(buyorder);
                    }
                }
            }
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
                                if (sellOrder.price <= RealtimePrice.Ask)
                                {
                                    profit = SellingProfit(sellOrder.symbol, sellOrder.quantityowned);
                                    SellFinalizedAccountUpdate(sellOrder, profit);
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
                                if (sellOrder.price >= RealtimePrice.Ask)
                                {
                                    profit = SellingProfit(sellOrder.symbol, sellOrder.quantityowned);
                                    SellFinalizedAccountUpdate(sellOrder, profit);
                                    SellOrdersPlaced.Remove(sellOrder);
                                }
                            }
                        }
                    }
                }
            }

        }
        public bool PreOwnedStock(string Symbol)
        {
            bool IsAssetHeld = false;
            foreach (Stock assetHeld in AIAccount.Portfolio)
            {
                if (Symbol == assetHeld.symbol)
                {
                    IsAssetHeld = true;
                }
            }
            return IsAssetHeld;
        }
        public int QuantityReturner(string Symbol)
        {
            int Quantity = 0;
            foreach (Stock stock in AIAccount.Portfolio)
            {
                if (Symbol == stock.symbol)
                {
                    Quantity = stock.quantityowned;
                }
            }
            return Quantity;
        }
        public void BuyFinalizedAccountSend(Stock StockToBuy, decimal TransactionCost)
        {
            Stock TempStock = new Stock();
            int Quantity = 0;
            decimal CurrentPrice = StockToBuy.price;
            int QuantityAffordable = Convert.ToInt32(AIAccount.AccountBalance / CurrentPrice);
            Quantity = QuantityAffordable / 2;
            AIAccount.AccountBalance -= TransactionCost;
            if (PreOwnedStock(StockToBuy.symbol))
            {
                foreach (Stock stock in AIAccount.Portfolio)
                {
                    if (stock.symbol == StockToBuy.symbol)
                    {
                        stock.price = stock.price * TempStock.price / 2;
                        stock.quantityowned += Quantity;
                    }
                }
            }
            else
            {
                AIAccount.Portfolio.Add(TempStock);
            }
        }
        public void SellFinalizedAccountUpdate(Stock StockToSell, decimal Gain)
        {
            int StockIndex = 0;
            int QuantityOwned = QuantityReturner(StockToSell.symbol);
            foreach (Stock stock in AIAccount.Portfolio)
            {
                if (QuantityOwned > StockToSell.quantityowned)
                {
                    AIAccount.AccountBalance += Gain;
                    stock.quantityowned -= StockToSell.quantityowned;
                }
                else if (StockToSell.quantityowned == QuantityOwned)
                {
                    StockIndex = AIAccount.Portfolio.IndexOf(stock);
                    AIAccount.AccountBalance += Gain;
                }
            }
            if (QuantityOwned == StockToSell.quantityowned)
            {
                AIAccount.Portfolio.RemoveAt(StockIndex);
            }
        }
        public void SellShortAccountUpdate(Stock ShortTracker, decimal Gain)
        {
            LendShort(ShortTracker);
            AIAccount.AccountBalance += Gain;
            AIAccount.ShortHoldings.Add(ShortTracker);
        }
        public void ReturnShortPosition(string Symbol, int Quantity)
        {
            int ShortIndex = 1;
            int MainIndex = 1;
            int MainQuantity = 1;
            int ShortQuantity = 1;
            foreach (Stock stock in AIAccount.Portfolio)
            {
                if (stock.symbol == Symbol)
                {
                    ShortStockReturn(Symbol, Quantity);
                    stock.quantityowned -= Quantity;
                    MainQuantity = stock.quantityowned;
                    MainIndex = AIAccount.Portfolio.IndexOf(stock);
                    foreach (Stock shortStock in AIAccount.ShortHoldings)
                    {
                        if (shortStock.symbol == Symbol)
                        {
                            shortStock.quantityowned -= Quantity;
                            ShortQuantity = shortStock.quantityowned;
                            ShortIndex = AIAccount.ShortHoldings.IndexOf(shortStock);
                        }
                    }
                }
            }
            if (ShortQuantity == 0)
            {
                AIAccount.ShortHoldings.RemoveAt(ShortIndex);
            }
            if (MainQuantity == 0)
            {
                AIAccount.Portfolio.RemoveAt(MainIndex);
            }
        }
        public void ReturnReminder()
        {
            foreach (Stock stock in bank.StocksLentShort)
            {
                Console.WriteLine("You currently owe the bank " + stock.quantityowned + " shares of " + stock.name + ".");
            }
        }
        public void ShortStockReturn(string Symbol, int Quantity)
        {
            foreach (Stock stock in bank.StocksLentShort)
            {
                if (Symbol == stock.symbol)
                {
                    stock.quantityowned -= Quantity;
                    if (stock.quantityowned == 0)
                    {
                        bank.StocksLentShort.Remove(stock);
                    }
                }
            }
        }
        public void LendShort(Stock shortAquisition)
        {
            bank.StocksLentShort.Add(shortAquisition);
        }

    }
}