﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    class Program
    {
        static void Main(string[] args)
        {
            AIFinancialAnalyst TestRun = new AIFinancialAnalyst();
            TestRun.AITransactionCycle();
        }
    }
}