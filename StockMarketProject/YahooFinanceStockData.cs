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
        FinanceDataConverter financeTool = new FinanceDataConverter();
        public void MainIntervalPullToWrite()
        {
            string AerospaceDefenseCSV;
            using (WebClient YahooData = new WebClient())
            {
                AerospaceDefenseCSV = YahooData.DownloadString("http://finance.yahoo.com/d/quotes.csv?s=AIR+AJRD+AVAV+ATRO+BEAV+CAE+CVU+CW+DCO+ESL+HEI+HXL+KLXI+LMIA+RTN+COL+SPR+TDY+TDG+TGI&f=spom7c1as7p2r5j2p5r6r7vn");
            }
            YahooStockList.Clear();
            FullDataToRegress.Clear();
            financeTool.CsvWriter(financeTool.BreakToRows(AerospaceDefenseCSV));
            FullDataToRegress.AddRange(financeTool.DatabaseGetter());
            YahooStockList.AddRange(FullDataToRegress.ElementAt(FullDataToRegress.Count() - 1));
        }
        public void SecondaryPullNoWrite()
        {
            string RawUpdate;
            using (WebClient RawPullForContinuedChecking = new WebClient())
            {
                RawUpdate = RawPullForContinuedChecking.DownloadString("http://finance.yahoo.com/d/quotes.csv?s=AIR+AJRD+AVAV+ATRO+BEAV+CAE+CVU+CW+DCO+ESL+HEI+HXL+KLXI+LMIA+RTN+COL+SPR+TDY+TDG+TGI&f=spom7c1as7p2r5j2p5r6r7vn");
            }
            YahooStockList.Clear();
            YahooStockList.AddRange(financeTool.ConverterRawPull(RawUpdate));
        }
        public List<StockProperties> SendNewestUpdate()
        {
            SecondaryPullNoWrite();
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