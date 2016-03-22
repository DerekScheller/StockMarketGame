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
                if(colData.Count() == 9)
                {
                    Stock.Name = colData[7].Trim(new char[] { '"' }) + colData[8].Trim(new char[] { '"' });
                }
                else
                {
                    Stock.Name = colData[7].Trim(new char[] { '"' });
                }
                Stock.Symbol = colData[0].Trim(new char[] { '"' });
                Stock.PreviousClose = Convert.ToDecimal(colData[1]);
                Stock.Open = Convert.ToDecimal(colData[2]);
                Stock.PercentFromMoveAve = Convert.ToDecimal(colData[3]);
                Stock.ChangeDaily = Convert.ToDecimal(colData[4]);
                Stock.Ask = Convert.ToDecimal(colData[5]);
                Stock.ShortRatio = Convert.ToDecimal(colData[6]);
                stockListImport.Add(Stock);
            }
            return stockListImport;
        }
    }
}
