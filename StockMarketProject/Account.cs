using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class Account
    {
        TransactionLogic transaction = new TransactionLogic();
        Bank bank = new Bank();
        decimal AccountBalance = 10000m;
        public List<Stock> Portfolio = new List<Stock>();
        public List<Stock> ShortHoldings = new List<Stock>();
        public void Transaction()
        {
            transaction.DataGenerator();
            Console.WriteLine("What type of transaction would you like to make? 1) Buy 2) Sell");
            int TransType = int.Parse(Console.ReadLine());
            if (TransType == 1)
            {
                Buy();
            }
            else if (TransType == 2)
            {
                Sell();
            }
            Transaction();
        }
        public void Buy()
        {
            Stock TempStock = new Stock();
            transaction.LiveDataPrint();
            Console.WriteLine("Enter the Symbol of the stock you would like to buy.");
            string SymbolToBuy = Console.ReadLine();
            decimal CurrentPrice = transaction.GetStockPrice(SymbolToBuy);
            int QuantityAffordable = Convert.ToInt32(AccountBalance / CurrentPrice);
            Console.WriteLine("You can afford " + QuantityAffordable + " shares of " + SymbolToBuy + ".");
            Console.WriteLine("How many shares would you like to buy?");
            int Quantity = int.Parse(Console.ReadLine());
            AccountBalance -= transaction.BuyingExpense(SymbolToBuy, Quantity);
            TempStock = transaction.TransactionReferenceCreator(SymbolToBuy, Quantity);
            if (PreOwnedStock(SymbolToBuy))
            {
                foreach (Stock stock in Portfolio)
                {
                    if (stock.symbol == SymbolToBuy)
                    {
                        stock.price = stock.price * TempStock.price / 2;
                        stock.quantityowned += Quantity;
                    }
                }
            }
            else
            {
                Portfolio.Add(TempStock);
            }
        }
        public void Sell()
        {
            int StockIndex = 0;
            MyPortfolioPrint();
            if (Portfolio.Count == 0 && ShortHoldings.Count == 0)
            {
                Console.WriteLine("You have no Stocks to Sell. Go buy some Stock to get trading.");
            }
            else
            {
                Console.WriteLine("Enter the Symbol of the stock you would like to Sell.");
                string StockToSell = Console.ReadLine();
                int QuantityOwned = QuantityReturner(StockToSell);
                Console.WriteLine("You own " + QuantityOwned + " of " + StockToSell + "." + "\n How many shares would you like to sell?");
                int QuantityToSell = int.Parse(Console.ReadLine());
                foreach (Stock stock in Portfolio)
                {
                    if (QuantityOwned > QuantityToSell)
                    {
                        AccountBalance += transaction.SellingProfit(StockToSell, QuantityToSell);
                        stock.quantityowned -= QuantityToSell;
                    }
                    else if (QuantityToSell == QuantityOwned)
                    {
                        StockIndex = Portfolio.IndexOf(stock);
                        AccountBalance += transaction.SellingProfit(StockToSell, QuantityToSell);
                    }
                    else
                    {
                        Console.WriteLine("That is more shares than you own of " + StockToSell + ". \n Lets try again!");
                        Sell();
                    }
                }
                if (QuantityOwned == QuantityToSell)
                {
                    Portfolio.RemoveAt(StockIndex);
                }
            }
        }
        public void MyPortfolioPrint()
        {
            foreach (Stock stock in Portfolio)
            {
                Console.WriteLine(string.Format("{0} ({1}) Price Paid: {2} Quantity Owned: {3}", stock.name, stock.symbol, stock.price, stock.quantityowned));
            }
        }
        public bool PreOwnedStock(string Symbol)
        {
            bool IsAssetHeld = false;
            foreach (Stock assetHeld in Portfolio)
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
            foreach (Stock stock in Portfolio)
            {
                if (Symbol == stock.symbol)
                {
                    Quantity = stock.quantityowned;
                }
            }
            return Quantity;
        }
        public void BuyAI(string Symbol)
        {
            Stock TempStock = new Stock();
            decimal CurrentPrice = transaction.GetStockPrice(Symbol);
            int QuantityAffordable = Convert.ToInt32(AccountBalance / CurrentPrice);
            int Quantity = QuantityAffordable / 2;
            AccountBalance -= transaction.BuyingExpense(Symbol, Quantity);
            TempStock = transaction.TransactionReferenceCreator(Symbol, Quantity);
            if (PreOwnedStock(Symbol))
            {
                foreach (Stock stock in Portfolio)
                {
                    if (stock.symbol == Symbol)
                    {
                        stock.price = stock.price * TempStock.price / 2;
                        stock.quantityowned += Quantity;
                    }
                }
            }
            else
            {
                Portfolio.Add(TempStock);
            }
        }
        public void SellAI(string StockToSell, int QuantityToSell)
        {
            int StockIndex = 0;
            MyPortfolioPrint();
            int QuantityOwned = QuantityReturner(StockToSell);
            foreach (Stock stock in Portfolio)
            {
                if (QuantityOwned > QuantityToSell)
                {
                    AccountBalance += transaction.SellingProfit(StockToSell, QuantityToSell);
                    stock.quantityowned -= QuantityToSell;
                }
                else if (QuantityToSell == QuantityOwned)
                {
                    StockIndex = Portfolio.IndexOf(stock);
                    AccountBalance += transaction.SellingProfit(StockToSell, QuantityToSell);
                }
                else
                {
                    Console.WriteLine("That is more shares than you own of " + StockToSell + ". \n Lets try again!");
                    Sell();
                }
            }
            if (QuantityOwned == QuantityToSell)
            {
                Portfolio.RemoveAt(StockIndex);
            }
        }
        public void SellShortAI(string StockToSell, int QuantityOwedShort)
        {
            bank.LendShort(StockToSell, QuantityOwedShort);
            AccountBalance += transaction.SellingProfit(StockToSell, QuantityOwedShort);
            ShortHoldings.Add(transaction.TransactionReferenceCreator(StockToSell, QuantityOwedShort));       
        }
        public void ReturnShortPosition(string Symbol, int Quantity)
        {
            int ShortIndex = 1;
            int MainIndex = 1;
            int MainQuantity = 1;
            int ShortQuantity = 1;
            foreach (Stock stock in Portfolio)
            {
                if (stock.symbol == Symbol)
                {
                    bank.ShortStockReturn(Symbol, Quantity);
                    stock.quantityowned -= Quantity;
                    MainQuantity = stock.quantityowned;
                    MainIndex = Portfolio.IndexOf(stock);
                    foreach (Stock shortStock in ShortHoldings)
                    {
                        if (shortStock.symbol == Symbol)
                        {
                            shortStock.quantityowned -= Quantity;
                            ShortQuantity = shortStock.quantityowned;
                            ShortIndex = ShortHoldings.IndexOf(shortStock);
                        }
                    }
                }
            }
            if (ShortQuantity == 0)
            {
                ShortHoldings.RemoveAt(ShortIndex);
            }
            if (MainQuantity == 0)
            {
                Portfolio.RemoveAt(MainIndex);
            }
        }
    }
}
