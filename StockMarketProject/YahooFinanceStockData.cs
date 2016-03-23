using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace StockMarketProject
{
    public class YahooFinanceStockData
    {
        public List<List<StockProperties>> DataHistory = new List<List<StockProperties>>();
        public List<StockProperties> YahooStockList = new List<StockProperties>();
        public void FinancialData()
        {
            FinanceDataConverter financeTool = new FinanceDataConverter();
            string FinanceDataCSV;
            using (WebClient YahooData = new WebClient())
            {
                FinanceDataCSV = YahooData.DownloadString("http://finance.yahoo.com/d/quotes.csv?s=JMBA+FDP+STKL+TFM+WFM+CVGW+LMNR&f=spom7c1as7p2r5j2p5r6r7vn");
            }
            YahooStockList = financeTool.ConvertCSV(FinanceDataCSV);
            DataHistory.Add(YahooStockList);
        }
        public void StockSelect()
        {
            foreach (StockProperties stock in YahooStockList)
                Console.WriteLine(string.Format("{0} ({1}) Current Price: {2} Open: {3} Previous Close: {4} Daily Change: {5}", stock.Name, stock.Symbol, stock.Ask, stock.Open, stock.PreviousClose, stock.ChangeDaily));
        }
    }
}
