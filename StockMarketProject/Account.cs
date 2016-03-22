using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    class Account : User
    {
        TransactionLogic transaction = new TransactionLogic();
        decimal AccountBalance;
        List<Stock> UserPortfolio = new List<Stock>();
        List<Stock> UserShortList = new List<Stock>();
        public void Buy(decimal Price)
        {
            decimal AffordableShareTotal = AccountBalance / Price;
            Console.WriteLine("You can afford " + AffordableShareTotal + " shares.\n" + "How many shares would you like to buy?");
            int Quantity = int.Parse(Console.ReadLine());
            decimal Total = transaction.BuyingSubtraction(Quantity, Price);
            if (AccountBalance > Total)
            {
                AccountBalance -= Total;
            }
            else
            {
                Console.WriteLine("You do not have enough funds to make this purchase");
            }
        }
        public void Sell(decimal Price, int QuantityOwned)
        {
            Console.WriteLine("You own " + QuantityOwned + " shares.\n" + "How many shares would you like to sell?");
            int Quantity = int.Parse(Console.ReadLine());
            decimal Total = transaction.SellingAddition(Quantity, Price);
            AccountBalance += Total;
        }
        public void HoldShort()
        {

        }
        public void PreOwnedStock(string Symbol, int Quantity)
        {
            foreach (Stock assetHeld in UserPortfolio)
            {
                if (Symbol == assetHeld.symbol)
                {
                    foreach (StockProperties stock in transaction.Stock)
                    {
                        if (Symbol == stock.Symbol)
                        {
                            assetHeld.price = assetHeld.price * stock.Ask / 2;
                            assetHeld.quantityowned = assetHeld.quantityowned + Quantity;
                        }
                    }
                }
            }
        }
        public void StockNotOwned(string Symbol, int Quantity)
        {
            foreach (StockProperties stock in transaction.Stock)
            {
                if (Symbol == stock.Symbol)
                {
                    UserPortfolio.Add(new Stock(stock.Name, stock.Symbol, Quantity, stock.Ask));
                }
            }
        }
        public void TransactionLogCreator(string Symbol, int Quantity)
        {
            Stock TempRefStock = transaction.TransactionReferenceCreator(Symbol, Quantity);
            foreach (Stock assetHeld in UserPortfolio)
            {
                if (Symbol == assetHeld.symbol)
                {
                    assetHeld.price = assetHeld.price * TempRefStock.price / 2;
                    assetHeld.quantityowned = assetHeld.quantityowned + Quantity;
                    decimal TransactionCost = transaction.BuyingSubtraction(Quantity, TempRefStock.price);
                    AccountBalance -= TransactionCost;
                }
                else if ()
                    UserPortfolio.Add(TempRefStock);
                }
                else {
                    StockNotOwned(Symbol, Quantity);
                }
            }

        }
    }
}
