using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace StockMarketProject
{
    public class YahooFinanceStockData
    {
        public List<List<StockProperties>> FullDataToRegress = new List<List<StockProperties>>();
        public List<StockProperties> YahooStockList = new List<StockProperties>();
        public void FinancialData()
        {
            FinanceDataConverter financeTool = new FinanceDataConverter();
            string FinanceDataCSV;
            using (WebClient YahooData = new WebClient())
            {
                FinanceDataCSV = YahooData.DownloadString("http://finance.yahoo.com/d/quotes.csv?s=JMBA+FDP+STKL+TFM+WFM+CVGW+LMNR&f=spom7c1as7p2r5j2p5r6r7vn");
            }
            YahooStockList.Clear();
            FullDataToRegress.Clear();
            financeTool.CsvWriter(financeTool.BreakToRows(FinanceDataCSV));
            FullDataToRegress.AddRange(financeTool.DatabaseGetter());
            YahooStockList.AddRange(FullDataToRegress.ElementAt(FullDataToRegress.Count() - 1));
        }
        public List<StockProperties> SendNewestUpdate()
        {
            return YahooStockList;
        }
        public List<List<StockProperties>> SendForRegressionModel()
        {
            return FullDataToRegress;
        }
        public void StockSelect()
        {
            foreach (StockProperties stock in YahooStockList)
                Console.WriteLine(string.Format("{0} ({1}) Current Price: {2} Open: {3} Previous Close: {4} Daily Change: {5}", stock.Name, stock.Symbol, stock.Ask, stock.Open, stock.PreviousClose, stock.ChangeDaily));
        }
    }
}