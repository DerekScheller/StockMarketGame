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
        public List<StockProperties> YahooStockList = new List<StockProperties>();
        public void FinancialData()
        {
            FinanceDataConverter financeTool = new FinanceDataConverter();
            string FinanceDataCSV;
            using (WebClient YahooData = new WebClient())
            {
                FinanceDataCSV = YahooData.DownloadString("http://finance.yahoo.com/d/quotes.csv?s=JMBA+FDP+STKL+TFM+WFM+CVGW+SENEA+ALCO+LMNR+LBIX+SPU&f=spom7c1as7n");
            }
            YahooStockList = financeTool.ConvertCSV(FinanceDataCSV);
        }
        public void StockSelect()
        {
            foreach (StockProperties stock in YahooStockList)
                Console.WriteLine(string.Format("{0} ({1}) Current Price: {2} Open: {3} Previous Close: {4}",stock.Name, stock.Symbol, stock.Ask, stock.Open, stock.PreviousClose));
        }
    }
}
