using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class FinanceDataConverter
    {
        public int GroupedPullValue = 0;
        public string[] BreakToRows(string csvData)
        {
            string[] rowData = csvData.Replace("\r", "").Split('\n');
            return rowData;

        }
        public void CsvWriter(string[] rowData)
        {
            DateTime epoch = new DateTime(2016, 1, 1);
            TimeSpan Difference = DateTime.Now.Subtract(epoch);
            decimal TimeStamp = Convert.ToDecimal(Difference.TotalSeconds);
            StringBuilder csv = new StringBuilder();
            foreach (string row in rowData)
            {
                if (string.IsNullOrEmpty(row)) continue;
                csv.AppendLine(row + "," + TimeStamp + "," + GroupedPullValue);
            }
            File.AppendAllText(@"C: \Users\Derek Scheller\Documents\Visual Studio 2015\Projects\Stock Market Project\StockDataBase.csv", csv.ToString());
            GroupedPullValue++;
        }
        public List<List<StockProperties>> DatabaseGetter()
        {
            List<List<StockProperties>> HistoricData = new List<List<StockProperties>>();
            List<StockProperties> StockValueAtInterval = new List<StockProperties>();
            StockProperties NewStockToAdd = new StockProperties();
            StockValueAtInterval.Clear();
            StreamReader reader = new StreamReader(File.OpenRead(@"C: \Users\Derek Scheller\Documents\Visual Studio 2015\Projects\Stock Market Project\StockDataBase.csv"));
            string line = reader.ReadLine();
            NewStockToAdd = ConvertCSV(line);
            StockValueAtInterval.Add(NewStockToAdd);
            int IntervalGroup = NewStockToAdd.PullGroup;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                NewStockToAdd = ConvertCSV(line);
                if (IntervalGroup == NewStockToAdd.PullGroup)
                {
                    StockValueAtInterval.Add(NewStockToAdd);
                }
                else
                {
                    HistoricData.Add(StockValueAtInterval);
                    StockValueAtInterval = new List<StockProperties>();
                    StockValueAtInterval.Add(NewStockToAdd);
                    IntervalGroup = NewStockToAdd.PullGroup;
                }
            }
            return HistoricData;
        }
        public StockProperties ConvertCSV(string rowData)
        {
            string[] colData = rowData.Split(',');
            StockProperties StockToReturn = new StockProperties();
            if (colData.Count() == 18)
            {
                StockToReturn.Name = colData[14].Trim(new char[] { '"' }) + colData[15].Trim(new char[] { '"' });
                StockToReturn.TimeStamp = Convert.ToDecimal(colData[16]);
                StockToReturn.PullGroup = Convert.ToInt32(colData[17]);
            }
            else
            {
                StockToReturn.Name = colData[14].Trim(new char[] { '"' });
                StockToReturn.TimeStamp = Convert.ToDecimal(colData[15]);
                StockToReturn.PullGroup = Convert.ToInt32(colData[16]);
            }
            StockToReturn.Symbol = colData[0].Trim(new char[] { '"' });
            StockToReturn.PreviousClose = Convert.ToDecimal(colData[1]);
            StockToReturn.Open = Convert.ToDecimal(colData[2]);
            StockToReturn.PercentFromMoveAve = Convert.ToDecimal(colData[3]);
            StockToReturn.ChangeDaily = Convert.ToDecimal(colData[4]);
            StockToReturn.Ask = Convert.ToDecimal(colData[5]);
            StockToReturn.ShortRatio = Convert.ToDecimal(colData[6]);
            StockToReturn.PercentChangeDaily = Convert.ToDecimal(colData[7].Trim(new char[] { '%', '"' }));
            StockToReturn.PEGRatio = Convert.ToDecimal(colData[8]);
            StockToReturn.SharesOutstanding = Convert.ToDecimal(colData[9]);
            StockToReturn.PriceOverSales = Convert.ToDecimal(colData[10]);
            StockToReturn.PriceOverEPSCurrent = Convert.ToDecimal(colData[11]);
            StockToReturn.PriceOverEPSForecast = Convert.ToDecimal(colData[12]);
            StockToReturn.Volume = Convert.ToDecimal(colData[13]);
            return StockToReturn;
        }
        public List<StockProperties> ConverterRawPull(string csvData)
        {
            List<StockProperties> stockListImport = new List<StockProperties>();
            string[] rowData = csvData.Replace("\r", "").Split('\n');
            foreach (string row in rowData)
            {
                if (string.IsNullOrEmpty(row)) continue;
                string[] colData = row.Split(',');
                StockProperties StockToReturn = new StockProperties();
                if (colData.Count() == 16)
                {
                    StockToReturn.Name = colData[14].Trim(new char[] { '"' }) + colData[15].Trim(new char[] { '"' });
                }
                else
                {
                    StockToReturn.Name = colData[14].Trim(new char[] { '"' });
                }
                StockToReturn.Symbol = colData[0].Trim(new char[] { '"' });
                StockToReturn.PreviousClose = Convert.ToDecimal(colData[1]);
                StockToReturn.Open = Convert.ToDecimal(colData[2]);
                StockToReturn.PercentFromMoveAve = Convert.ToDecimal(colData[3]);
                StockToReturn.ChangeDaily = Convert.ToDecimal(colData[4]);
                StockToReturn.Ask = Convert.ToDecimal(colData[5]);
                StockToReturn.ShortRatio = Convert.ToDecimal(colData[6]);
                StockToReturn.PercentChangeDaily = Convert.ToDecimal(colData[7].Trim(new char[] { '%', '"' }));
                StockToReturn.PEGRatio = Convert.ToDecimal(colData[8]);
                StockToReturn.SharesOutstanding = Convert.ToDecimal(colData[9]);
                StockToReturn.PriceOverSales = Convert.ToDecimal(colData[10]);
                StockToReturn.PriceOverEPSCurrent = Convert.ToDecimal(colData[11]);
                StockToReturn.PriceOverEPSForecast = Convert.ToDecimal(colData[12]);
                StockToReturn.Volume = Convert.ToDecimal(colData[13]);
                stockListImport.Add(StockToReturn);
            }
            return stockListImport;
        }
    }
}
