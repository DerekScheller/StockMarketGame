using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    public class FinanceDataConverter
    {
        public List<StockProperties> ConvertCSV(string csvData)
        {
            List<StockProperties> stockListImport = new List<StockProperties>();
            string[] rowData = csvData.Replace("\r", "").Split('\n');
            foreach (string row in rowData)
            {
                if (string.IsNullOrEmpty(row)) continue;
                string[] colData = row.Split(',');
                StockProperties Stock = new StockProperties();
                if (colData.Count() == 16)
                {
                    Stock.Name = colData[14].Trim(new char[] { '"' }) + colData[15].Trim(new char[] { '"' });
                }
                else
                {
                    Stock.Name = colData[14].Trim(new char[] { '"' });
                }
                Stock.Symbol = colData[0].Trim(new char[] { '"' });
                Stock.PreviousClose = Convert.ToDecimal(colData[1]);
                Stock.Open = Convert.ToDecimal(colData[2]);
                Stock.PercentFromMoveAve = Convert.ToDecimal(colData[3]);
                Stock.ChangeDaily = Convert.ToDecimal(colData[4]);
                Stock.Ask = Convert.ToDecimal(colData[5]);
                Stock.ShortRatio = Convert.ToDecimal(colData[6]);
                Stock.PercentChangeDaily = Convert.ToDecimal(colData[7].Trim(new char[] { '%','"' }));
                Stock.PEGRatio = Convert.ToDecimal(colData[8]);
                Stock.SharesOutstanding = Convert.ToDecimal(colData[9]);
                Stock.PriceOverSales = Convert.ToDecimal(colData[10]);
                Stock.PriceOverEPSCurrent = Convert.ToDecimal(colData[11]);
                Stock.PriceOverEPSForecast = Convert.ToDecimal(colData[12]);
                Stock.Volume = Convert.ToDecimal(colData[13]);
                stockListImport.Add(Stock);
            }
            return stockListImport;
        }
    }
}
