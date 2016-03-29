using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketProject
{
    class StatisticalEquations
    {
        public decimal Mean(List<decimal> GetMeanList)
        {
            decimal sum = 0m;
            decimal finalMean = 0m;
            foreach(decimal datapoint in GetMeanList)
            {
                sum += datapoint;
            }
            finalMean = sum / GetMeanList.Count;
            return finalMean;
        }
        public decimal WeightMean(List<decimal> NumeratorList, List<decimal> DenominatorList)
    {
        decimal numerator = 0m;
        decimal denominator = 0m;
        decimal finalweightedMean = 0m;
        for (int i = 0; i < NumeratorList.Count; i++)
        {
            numerator += NumeratorList.ElementAt(i) * DenominatorList.ElementAt(i);
            denominator += DenominatorList.ElementAt(i);
        }
        finalweightedMean = numerator / denominator;
        return finalweightedMean;
    }
        public decimal GetListSumToPowerOf(List<decimal> SumList, int power)
        {
            decimal sum = 0m;
            foreach (decimal datapoint in SumList)
            {
                sum += Convert.ToDecimal(Math.Pow(Convert.ToDouble(datapoint),power));
            }
            return sum;
        }
        public decimal GetTwoListSum (List<decimal> ListValueOne, List<decimal> ListValueTwo, int power)
        {
            decimal sumOfTwoLists = 0m;
            int numberOfCycles = ListValueOne.Count;
            int cycleCount = 0;
            while (cycleCount < numberOfCycles)
            {
                sumOfTwoLists += Convert.ToDecimal(Math.Pow(Convert.ToDouble(ListValueOne.ElementAt(cycleCount)),power)) * ListValueTwo.ElementAt(cycleCount);
                cycleCount++;
            }
            return sumOfTwoLists;
        }
        public decimal StandardDev(List<decimal> GetStandardDevList)
        {
            decimal meanvalue = Mean(GetStandardDevList);
            decimal numeratorSum = 0m;
            decimal varience = 0m;
            decimal finalStandardDev = 0m;
            foreach(decimal datapoint in GetStandardDevList)
            {
                numeratorSum += Convert.ToDecimal(Math.Pow(Convert.ToDouble(datapoint - meanvalue), 2));
            }
            varience = numeratorSum / (GetStandardDevList.Count - 1);
            finalStandardDev =Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(varience)));
            return finalStandardDev;
        }
        public decimal SampleCovariance(List<decimal> ListValueOne, List<decimal> ListValueTwo)
        {
            decimal meanvalueOne = Mean(ListValueOne);
            decimal meanvalueTwo = Mean(ListValueTwo);
            decimal numeratorSum = 0m;
            decimal coVarience = 0m;
            for (int i = 0; i < ListValueOne.Count; i++)
            {
                numeratorSum += (ListValueOne.ElementAt(i) - meanvalueOne) * (ListValueTwo.ElementAt(i) - meanvalueTwo);
            }
            coVarience = numeratorSum / (ListValueOne.Count - 1);
            return coVarience;
        }
        public decimal CorrelationCoefficientR(List<decimal> ListValueOne, List<decimal> ListValueTwo)
        {
            int numberOfCases = ListValueOne.Count;
            decimal SumListOne = GetListSumToPowerOf(ListValueOne, 1);
            decimal SumListTwo = GetListSumToPowerOf(ListValueTwo, 1);
            decimal SumListOneSquared = GetListSumToPowerOf(ListValueOne, 2);
            decimal SumListTwoSquared = GetListSumToPowerOf(ListValueTwo, 2);
            decimal SumOfBothLists = GetTwoListSum(ListValueOne, ListValueTwo, 1);
            decimal slope = 0m;
            decimal numerator = 0m;
            decimal denominator = 0m;
            numerator = numberOfCases * SumOfBothLists - SumListOne * SumListTwo;
            denominator = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(numberOfCases * SumListOneSquared - SumListOne * SumListOne))* Math.Sqrt(Convert.ToDouble(numberOfCases * SumListTwoSquared - SumListTwo * SumListTwo)));
            slope = numerator / denominator;
            return slope;
        }
        public decimal SlopeReturn(List<decimal> ListValueOne, List<decimal> ListValueTwo)
        {
            int numberOfCases = ListValueOne.Count;
            decimal SumListOne = GetListSumToPowerOf(ListValueOne, 1);
            decimal SumListTwo = GetListSumToPowerOf(ListValueTwo, 1);
            decimal SumListOneSquared = GetListSumToPowerOf(ListValueOne, 2);
            decimal SumOfBothLists = GetTwoListSum(ListValueOne, ListValueTwo, 1);
            decimal slope = 0m;
            decimal numerator = 0m;
            decimal denominator = 0m;
                numerator = numberOfCases * SumOfBothLists - SumListOne * SumListTwo;
                denominator = numberOfCases * SumListOneSquared - SumListOne * SumListOne;
            slope = numerator / denominator;
            return slope;
        }
        public decimal InterceptReturn(List<decimal> ListValueOne, List<decimal> ListValueTwo)
        {
            int numberOfCases = ListValueOne.Count;
            decimal SumListOne = GetListSumToPowerOf(ListValueOne, 1);
            decimal SumListTwo = GetListSumToPowerOf(ListValueTwo, 1);
            decimal SumListOneSquared = GetListSumToPowerOf(ListValueOne, 2);
            decimal SumOfBothLists = GetTwoListSum(ListValueOne, ListValueTwo, 1);
            decimal intercept = 0m;
            decimal numerator = 0m;
            decimal denominator = 0m;
            numerator = SumListTwo * SumListOneSquared - SumListOne * SumOfBothLists;
            denominator = numberOfCases * SumListOneSquared - SumListOne * SumListOne;
            intercept = numerator / denominator;
            return intercept;
        }
        public decimal EquationGuessLinearRegression(List<decimal> ListValueOne, List<decimal> ListValueTwo, int Multiplier)
        {
            decimal guess = 0m;
            decimal slope = SlopeReturn(ListValueOne,ListValueTwo);
            decimal intercept = InterceptReturn(ListValueOne,ListValueTwo);
            guess = intercept + slope * (ListValueOne.ElementAt(ListValueOne.Count - 1) + (120m * Multiplier));
            return guess;

        }
        //public decimal EquationGuessPolynomialRegression(List<decimal> ListValueOne, List<decimal> ListValueTwo, int Multiplier)
        //{
        //    decimal polynomialGuess = 0m;
        //    decimal Anot = 0m;
        //    decimal AOne = 0m;
        //    decimal ATwo = 0m;
        //    int numberOfValues = ListValueOne.Count;
        //    decimal SumTime = GetListSumToPowerOf(ListValueOne, 1);
        //    decimal SumTimeSquared = GetListSumToPowerOf(ListValueOne, 2);
        //    decimal SumTimeCubed = GetListSumToPowerOf(ListValueOne, 3);
        //    decimal SumTimeQuad = GetListSumToPowerOf(ListValueOne, 4);
        //    decimal SumPrice = GetListSumToPowerOf(ListValueTwo, 1);
        //    decimal SumTwolist = GetTwoListSum(ListValueOne, ListValueTwo, 1);
        //    decimal SumTwolistTimeSquared = GetTwoListSum(ListValueOne, ListValueTwo, 2);
        //    polynomialGuess = Anot + AOne * (ListValueOne.ElementAt(ListValueOne.Count - 1) + (120m * Multiplier)) + ATwo * Convert.ToDecimal(Math.Pow((Convert.ToDouble(ListValueOne.ElementAt(ListValueOne.Count - 1) + (120m * Multiplier))), 2));
        //    return polynomialGuess;
        //}
        public List<decimal> ReturnNextBid(List<decimal> ListValueOne, List<decimal> ListValueTwo)
        {
            decimal guessOne = 0m;
            decimal guessTwo = 0m;
            decimal guessThree = 0m;
            List<decimal> BidList = new List<decimal>();
            for(int i = 1; i < 4; i++)
            {
                switch (i)
                {
                    case 1:
                        guessOne = EquationGuessLinearRegression(ListValueOne, ListValueTwo, i);
                        break;
                    case 2:
                        guessTwo = EquationGuessLinearRegression(ListValueOne, ListValueTwo, i);
                        break;
                    case 3:
                        guessThree = EquationGuessLinearRegression(ListValueOne, ListValueTwo, i);
                        break;
                    default:
                        break;
                }    
            }
            BidList.Add(guessOne);
            BidList.Add(guessTwo);
            BidList.Add(guessThree);
            return BidList;
        }
        public int BuyLongVsShortDeterminent(StockProperties StockToCheck)
        {
            int LongShortConfidence = 0;
            if(StockToCheck.ShortRatio < 40)
            {
                LongShortConfidence += 1;              
            }
            if(StockToCheck.ShortRatio > 50)
            {
                LongShortConfidence -= 2;
            }
            if(StockToCheck.PercentFromMoveAve > 0)
            {
                if (StockToCheck.PriceOverEPSCurrent < StockToCheck.PriceOverEPSForecast)
                {
                    LongShortConfidence += 1;
                }
                    LongShortConfidence += 1;
            }
            if(StockToCheck.PercentFromMoveAve > 5)
            {
                    LongShortConfidence += 1;
            }
            if(StockToCheck.PercentFromMoveAve < -5)
            {
                LongShortConfidence -= 1;
            }
            if(StockToCheck.PriceOverSales > 1.5m)
            {
                LongShortConfidence -= 1;
                if(StockToCheck.PriceOverSales > 2)
                {
                    LongShortConfidence -= 1;
                }
            }
            if(StockToCheck.PriceOverSales < 1.5m)
            {
                LongShortConfidence += 1;
                if (StockToCheck.PriceOverSales < 1)
                {
                    LongShortConfidence += 1;
                    if (StockToCheck.PriceOverSales < .5m)
                    {
                        LongShortConfidence += 1;
                    }
                }
            }
            if(StockToCheck.PEGRatio < 1)
            {
                if(StockToCheck.Volume > 100000)
                {
                    LongShortConfidence += 1;
                }
                LongShortConfidence += 1;
            }
            return LongShortConfidence;
        }
        public decimal MovingAverageConvergenceDivergence(List<List<StockProperties>> GetExpoList, string Symbol)
        {
           
            // This needs to saved to a CSV and read over time in comparison with a Stocastic operator
            // Once proper data update increments are implimented i will be inrimented by the value of 1 day
            decimal newestAverage = 0m;
            decimal oldestAverage = 0m;
            decimal TotalSetExpoMovingAve = 0m;
            decimal NewestSetExpoMovingAve = 0m;
            List<StockProperties> InitialSMAList = new List<StockProperties>();
            decimal multiplier = (2 / (GetExpoList.Count + 1));
            int listLength = GetExpoList.Count;
            for(int i = 0; i < 10; i++)
            {
                InitialSMAList = GetExpoList.ElementAt(i);
                foreach (StockProperties SMA in InitialSMAList)
                {
                    if(SMA.Symbol == Symbol)
                    {
                        oldestAverage += SMA.Ask;
                    }
                }
            }
            oldestAverage = oldestAverage / 10;
            TotalSetExpoMovingAve = oldestAverage;
            for (int i = 10; i < 40; i++)
            {
                InitialSMAList = GetExpoList.ElementAt(listLength - i - 1);
                foreach (StockProperties SMA in InitialSMAList)
                {
                    if (SMA.Symbol == Symbol)
                    {
                        newestAverage += SMA.Ask;
                    }
                }
            }
            newestAverage = newestAverage / 10;
            for (int i = 0; i < 10; i++)
            {
                InitialSMAList = GetExpoList.ElementAt(listLength - i - 1);
                foreach (StockProperties NewestDataPoints in InitialSMAList)
                {
                    if (NewestDataPoints.Symbol == Symbol)
                    {
                        NewestSetExpoMovingAve += ((NewestDataPoints.PreviousClose - NewestSetExpoMovingAve) * .181818m + NewestSetExpoMovingAve);
                    }
                }
            }
            foreach (List<StockProperties> Interval in GetExpoList)
            {
                foreach(StockProperties datapoint in Interval)
                {
                    if(datapoint.Symbol == Symbol)
                    {
                        TotalSetExpoMovingAve = ((datapoint.PreviousClose - TotalSetExpoMovingAve) * multiplier + TotalSetExpoMovingAve);
                    }
                }
            }
            return (NewestSetExpoMovingAve - TotalSetExpoMovingAve);
        }
    }
}
