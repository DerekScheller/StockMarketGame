using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class Account
    {
        Bank bank = new Bank();
        public decimal AccountBalance = 10000m;
        public List<Stock> Portfolio = new List<Stock>();
        public List<Stock> ShortHoldings = new List<Stock>();
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
        public void BuyAI(Stock StockToBuy, decimal TransactionCost)
        {
            Stock TempStock = new Stock();
            int Quantity = 0;
            decimal CurrentPrice = StockToBuy.price;
            int QuantityAffordable = Convert.ToInt32(AccountBalance / CurrentPrice);
            Quantity = QuantityAffordable / 2;
            AccountBalance -= TransactionCost;
            if (PreOwnedStock(StockToBuy.symbol))
            {
                foreach (Stock stock in Portfolio)
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
                Portfolio.Add(TempStock);
            }
        }
        public void SellAI(Stock StockToSell, decimal Gain)
        {
            int StockIndex = 0;
            MyPortfolioPrint();
            int QuantityOwned = QuantityReturner(StockToSell.symbol);
            foreach (Stock stock in Portfolio)
            {
                if (QuantityOwned > StockToSell.quantityowned)
                {
                    AccountBalance += Gain;
                    stock.quantityowned -= StockToSell.quantityowned;
                }
                else if (StockToSell.quantityowned == QuantityOwned)
                {
                    StockIndex = Portfolio.IndexOf(stock);
                    AccountBalance += Gain;
                }
            }
            if (QuantityOwned == StockToSell.quantityowned)
            {
                Portfolio.RemoveAt(StockIndex);
            }
        }
        public void SellShortAI(Stock ShortTracker, decimal Gain)
        {
            bank.LendShort(ShortTracker);
            AccountBalance += Gain;
            ShortHoldings.Add(ShortTracker);       
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
