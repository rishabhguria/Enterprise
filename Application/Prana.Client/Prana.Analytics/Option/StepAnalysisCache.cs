using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Analytics
{
    class StepAnalysisCache
    {
        private SortedDictionary<string, DataTable> _symbolDataCollection;
        CtrlStepAnalysis _formStepAnalysis;

        public StepAnalysisCache(CtrlStepAnalysis formStepAnalysis)
        {
            try
            {
                _formStepAnalysis = formStepAnalysis;
                _symbolDataCollection = new SortedDictionary<string, DataTable>();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private Dictionary<int, List<WeightedGreekCalcInputs>> weightedGreekCalcInputsForPortfolioDict = new Dictionary<int, List<WeightedGreekCalcInputs>>();

        public void AddNonOptionsDataTable(StepParameter stepParameter, SymbolData nonOptionData, String symbol)
        {
            try
            {
                int numSteps = stepParameter.Steps + 1;
                string parameterCode = stepParameter.ParameterCode.ToString();

                string tempSymbol = string.Empty;
                if (nonOptionData != null)
                {
                    tempSymbol = nonOptionData.Symbol;
                }
                else
                {
                    tempSymbol = symbol;
                }

                List<PranaPositionWithGreeks> listPosition = _formStepAnalysis.GetSelectedRows(tempSymbol);
                DataTable dtGraph = CreateDataTableSchema(tempSymbol, parameterCode);

                double xKey = stepParameter.Low;
                for (int rowNum = 0; rowNum < numSteps; rowNum++)
                {
                    List<WeightedGreekCalcInputs> weightedGreekCalcInputs = new List<WeightedGreekCalcInputs>();
                    foreach (PranaPositionWithGreeks position in listPosition)
                    {
                        WeightedGreekCalcInputs inputs = new WeightedGreekCalcInputs();
                        inputs.Symbol = position.Symbol;
                        inputs.AbsQuantity = Math.Abs(position.Quantity);
                        inputs.Delta = position.Delta;
                        inputs.UnderlyingDelta = position.UnderlyingDelta;
                        inputs.Gamma = position.Gamma;
                        inputs.Theta = position.Theta;
                        inputs.Vega = position.Vega;
                        inputs.Rho = position.Rho;
                        inputs.Volatility = System.Math.Round(position.Volatility, 4);
                        inputs.Multiplier = position.ContractMultiplier;
                        inputs.SideMultiplier = position.SideMultiplier;
                        inputs.UnderlyingSymbol = position.UnderlyingSymbol;
                        if (nonOptionData != null)
                        {
                            if (parameterCode == StepAnalParameterCode.UnderlyingPrice.ToString())
                            {
                                inputs.SimulatedUnderlyingStockPrice = nonOptionData.SelectedFeedPrice * (1 + xKey / 100);
                                inputs.SimulatedPrice = nonOptionData.SelectedFeedPrice * (1 + xKey / 100);
                                inputs.SimulatedUnderlyingStockPriceInBaseCurrency = inputs.SimulatedUnderlyingStockPrice * position.FXRate;
                                inputs.SimulatedPriceInBaseCurrency = inputs.SimulatedPrice * position.FXRate;
                            }
                            else
                            {
                                inputs.SimulatedUnderlyingStockPrice = nonOptionData.SelectedFeedPrice;
                                inputs.SimulatedPrice = nonOptionData.SelectedFeedPrice;
                                inputs.SimulatedUnderlyingStockPriceInBaseCurrency = inputs.SimulatedUnderlyingStockPrice * position.FXRate;
                                inputs.SimulatedPriceInBaseCurrency = inputs.SimulatedPrice * position.FXRate;
                            }
                            inputs.SelectedFeedPrice = nonOptionData.SelectedFeedPrice;
                            inputs.SelectedFeedPriceInBaseCurrency = inputs.SelectedFeedPrice * position.FXRate;
                        }
                        else
                        {
                            if (parameterCode == StepAnalParameterCode.UnderlyingPrice.ToString())
                            {
                                inputs.SimulatedUnderlyingStockPrice = position.SimulatedUnderlyingStockPrice * (1 + xKey / 100);
                                inputs.SimulatedPrice = position.SimulatedPrice * (1 + xKey / 100);
                                inputs.SimulatedUnderlyingStockPriceInBaseCurrency = inputs.SimulatedUnderlyingStockPrice * position.FXRate;
                                inputs.SimulatedPriceInBaseCurrency = inputs.SimulatedPrice * position.FXRate;
                            }
                            else
                            {
                                inputs.SimulatedUnderlyingStockPrice = position.SimulatedUnderlyingStockPrice;
                                inputs.SimulatedPrice = position.SimulatedPrice;
                                inputs.SimulatedUnderlyingStockPriceInBaseCurrency = inputs.SimulatedUnderlyingStockPrice * position.FXRate;
                                inputs.SimulatedPriceInBaseCurrency = inputs.SimulatedPrice * position.FXRate;
                            }
                            inputs.SelectedFeedPrice = position.SelectedFeedPrice;
                            inputs.SelectedFeedPriceInBaseCurrency = inputs.SelectedFeedPrice * position.FXRate;
                        }
                        inputs.AvgPrice = position.AvgPrice;
                        inputs.DaysToExpration = position.DaysToExpiration;
                        inputs.InterestRate = position.InterestRate;
                        if (position.PutOrCall == 0)
                        {
                            inputs.PutOrCalls = 'P';
                        }
                        else if (position.PutOrCall == 1)
                        {
                            inputs.PutOrCalls = 'C';
                        }
                        //inputs.PutOrCalls = position.PutOrCalls;
                        inputs.StrikePrice = position.StrikePrice;
                        inputs.CurrencyID = position.CurrencyID;
                        inputs.AvgPriceInBaseCurrency = inputs.AvgPrice * position.FXRate;
                        weightedGreekCalcInputs.Add(inputs);
                    }

                    DataRow dr = dtGraph.NewRow();
                    CalculateDetailsForRow(dr, weightedGreekCalcInputs);
                    dr[parameterCode + "MUL"] = dr[parameterCode];
                    dr[parameterCode] = System.String.Format("{0:0.0000}", xKey);
                    dtGraph.Rows.Add(dr);
                    xKey += stepParameter.StepDifference;
                    if (weightedGreekCalcInputsForPortfolioDict.Count < numSteps)
                    {
                        weightedGreekCalcInputsForPortfolioDict.Add(rowNum, weightedGreekCalcInputs);
                    }
                    else
                    {
                        weightedGreekCalcInputsForPortfolioDict[rowNum].AddRange(weightedGreekCalcInputs);
                    }
                }
                AddDatatable(dtGraph.TableName, dtGraph);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void AddOptionsDataTable(StepParameter stepParam, List<StepAnalysisResponse> stepResList)
        {
            try
            {
                if (stepResList.Count > 0)
                {

                    int numSteps = stepParam.Steps + 1;
                    int numOfSymbols = stepResList.Count / numSteps;
                    string parameterCode = stepParam.ParameterCode.ToString();

                    // FOR INDIVIDUAL OPTION SYMBOLS 
                    for (int symbolNum = 0; symbolNum < numOfSymbols; symbolNum++) // Once for each option symbol
                    {
                        string symbol = stepResList[symbolNum * numSteps].Symbol;
                        DataTable dtGraph = CreateDataTableSchema(symbol, parameterCode);  //Table For Option Symbol
                        List<PranaPositionWithGreeks> list = _formStepAnalysis.GetSelectedRows(symbol);
                        for (int rowNum = (symbolNum * numSteps); rowNum < ((symbolNum + 1) * numSteps); rowNum++) //Once for each row of that symbol
                        {
                            StepAnalysisResponse stepRes = stepResList[rowNum];
                            List<WeightedGreekCalcInputs> weightedGreekCalcInputs = new List<WeightedGreekCalcInputs>();

                            foreach (PranaPositionWithGreeks position in list)
                            {
                                WeightedGreekCalcInputs inputs = new WeightedGreekCalcInputs();
                                inputs.Symbol = stepRes.Symbol;
                                inputs.AbsQuantity = Math.Abs(position.Quantity);
                                inputs.Delta = stepRes.Greeks.Delta;
                                inputs.Gamma = stepRes.Greeks.Gamma;
                                inputs.Theta = stepRes.Greeks.Theta;
                                inputs.Vega = stepRes.Greeks.Vega;
                                inputs.Rho = stepRes.Greeks.Rho;
                                inputs.Volatility = System.Math.Round(stepRes.InputParameters.Volatility * 100, 4);
                                inputs.Multiplier = position.ContractMultiplier;
                                inputs.SideMultiplier = position.SideMultiplier;
                                inputs.UnderlyingSymbol = position.UnderlyingSymbol;
                                inputs.SimulatedUnderlyingStockPrice = stepRes.Greeks.SimulatedUnderlyingStockPrice;
                                inputs.SimulatedPrice = stepRes.Greeks.SimulatedPrice;
                                inputs.SelectedFeedPrice = stepRes.Greeks.SelectedFeedPrice;
                                inputs.AvgPrice = position.AvgPrice;
                                inputs.DaysToExpration = stepRes.Greeks.DaysToExpiration;
                                inputs.InterestRate = System.Math.Round(stepRes.InputParameters.InterestRate * 100, 2);
                                //inputs.PutOrCalls = position.PutOrCalls;
                                inputs.StrikePrice = position.StrikePrice;
                                inputs.CurrencyID = position.CurrencyID;
                                inputs.UnderlyingDelta = position.UnderlyingDelta;
                                inputs.Asset = position.AssetCategoryValue;
                                inputs.ExchangeName = position.ExchangeName;
                                inputs.SelectedFeedPriceInBaseCurrency = inputs.SelectedFeedPrice * position.FXRate;
                                inputs.SimulatedUnderlyingStockPriceInBaseCurrency = inputs.SimulatedUnderlyingStockPrice * position.FXRate;
                                inputs.SimulatedPriceInBaseCurrency = inputs.SimulatedPrice * position.FXRate;
                                inputs.AvgPriceInBaseCurrency = inputs.AvgPrice * position.FXRate;

                                weightedGreekCalcInputs.Add(inputs);
                            }
                            if (weightedGreekCalcInputs.Count > 0)
                            {
                                DataRow dr = dtGraph.NewRow();
                                CalculateDetailsForRow(dr, weightedGreekCalcInputs);
                                dr[parameterCode + "MUL"] = dr[parameterCode];
                                dr[parameterCode] = System.String.Format("{0:0.0000}", System.Convert.ToDouble(stepRes.InputParameters.Key));
                                dtGraph.Rows.Add(dr);
                                if (weightedGreekCalcInputsForPortfolioDict.Count < numSteps)
                                {
                                    weightedGreekCalcInputsForPortfolioDict.Add(rowNum, weightedGreekCalcInputs);
                                }
                                else
                                {
                                    weightedGreekCalcInputsForPortfolioDict[rowNum - (symbolNum * numSteps)].AddRange(weightedGreekCalcInputs);
                                }
                            }
                        }
                        AddDatatable(dtGraph.TableName, dtGraph);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void AddPortfolioDataTable(StepParameter stepParameter)
        {
            try
            {
                int numSteps = stepParameter.Steps + 1;
                string parameterCode = stepParameter.ParameterCode.ToString();
                DataTable dtPortfolio = CreateDataTableSchema("Portfolio", parameterCode);

                double xKey = stepParameter.Low;
                for (int count = 0; count < numSteps; count++)
                {
                    DataRow drPortfolio = dtPortfolio.NewRow();
                    if (weightedGreekCalcInputsForPortfolioDict.ContainsKey(count))
                    {
                        CalculateDetailsForRow(drPortfolio, weightedGreekCalcInputsForPortfolioDict[count]);
                        drPortfolio[parameterCode + "MUL"] = drPortfolio[parameterCode];
                        drPortfolio[parameterCode] = System.String.Format("{0:0.0000}", xKey);
                        dtPortfolio.Rows.Add(drPortfolio);
                        xKey += stepParameter.StepDifference;
                    }
                }
                AddDatatable("Portfolio", dtPortfolio);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AddDatatable(string key, DataTable dt)
        {
            try
            {
                if (!_symbolDataCollection.ContainsKey(key))
                {
                    _symbolDataCollection.Add(key, dt);
                }
                else
                {
                    _symbolDataCollection[key] = dt;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CalculateDetailsForRow(DataRow dr, List<WeightedGreekCalcInputs> weightedGreekCalcInputs)
        {
            try
            {
                dr["Symbol"] = OptionGreekCalculater.CalculateMergedSymbol(weightedGreekCalcInputs);
                dr["UnderLyingSymbol"] = OptionGreekCalculater.CalculateMergedUnderlyingSymbol(weightedGreekCalcInputs);
                dr[StepAnalParameterCode.UnderlyingPrice.ToString()] = OptionGreekCalculater.CalculateMergedStockPriceInBaseCurrency(weightedGreekCalcInputs);
                dr[StepAnalParameterCode.DaysToExpiration.ToString()] = OptionGreekCalculater.CalculateMergedDaysToExpiration(weightedGreekCalcInputs);
                dr[StepAnalParameterCode.Volatility.ToString()] = OptionGreekCalculater.CalculateMergedImpVolatility(weightedGreekCalcInputs);
                dr[StepAnalParameterCode.InterestRate.ToString()] = OptionGreekCalculater.CalculateMergedInterestRate(weightedGreekCalcInputs);
                dr[CalculatedParamters.Delta.ToString()] = OptionGreekCalculater.CalculateWeightedDelta(weightedGreekCalcInputs);
                dr[CalculatedParamters.Gamma.ToString()] = OptionGreekCalculater.CalculateWeightedGamma(weightedGreekCalcInputs);
                dr[CalculatedParamters.Theta.ToString()] = OptionGreekCalculater.CalculateWeightedTheta(weightedGreekCalcInputs);
                dr[CalculatedParamters.Vega.ToString()] = OptionGreekCalculater.CalculateWeightedVega(weightedGreekCalcInputs);
                dr[CalculatedParamters.Rho.ToString()] = OptionGreekCalculater.CalculateWeightedRho(weightedGreekCalcInputs);
                dr[CalculatedParamters.DollarDeltaInBaseCurrency.ToString()] = OptionGreekCalculater.CalculateNetDollarDeltaInBaseCurrency(weightedGreekCalcInputs);
                dr[CalculatedParamters.DollarGammaInBaseCurrency.ToString()] = OptionGreekCalculater.CalculateNetDollarGammaInBaseCurrency(weightedGreekCalcInputs);
                dr[CalculatedParamters.DollarThetaInBaseCurrency.ToString()] = OptionGreekCalculater.CalculateNetDollarTheta(weightedGreekCalcInputs);
                dr[CalculatedParamters.DollarVegaInBaseCurrency.ToString()] = OptionGreekCalculater.CalculateNetDollarVega(weightedGreekCalcInputs);
                dr[CalculatedParamters.DollarRhoInBaseCurrency.ToString()] = OptionGreekCalculater.CalculateNetDollarRho(weightedGreekCalcInputs);
                dr[CalculatedParamters.SimulatedPriceInBaseCurrency.ToString()] = OptionGreekCalculater.CalculateMergedOptionPriceInBaseCurrency(weightedGreekCalcInputs);
                dr[CalculatedParamters.DeltaAdjExposureInBaseCurrency.ToString()] = OptionGreekCalculater.CalculateNetDeltaAdjExposureInBaseCurrency(weightedGreekCalcInputs);
                dr[CalculatedParamters.CostBasisUnrealizedPnLInBaseCurrency.ToString()] = OptionGreekCalculater.CalculateNetCostBasisPnlInBaseCurrency(weightedGreekCalcInputs);
                dr[CalculatedParamters.SimulatedPnlInBaseCurrency.ToString()] = OptionGreekCalculater.CalculateNetDayPnlInBaseCurrency(weightedGreekCalcInputs);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private DataTable CreateDataTableSchema(string tableName, string xParameterCode)
        {
            DataTable dtGraph = new DataTable(tableName);
            try
            {

                dtGraph.Columns.Add("Symbol", typeof(string));
                dtGraph.Columns.Add("UnderLyingSymbol", typeof(string));
                dtGraph.Columns.Add(StepAnalParameterCode.UnderlyingPrice.ToString(), typeof(string));
                dtGraph.Columns.Add(StepAnalParameterCode.Volatility.ToString(), typeof(string));
                dtGraph.Columns.Add(StepAnalParameterCode.InterestRate.ToString(), typeof(string));
                dtGraph.Columns.Add(StepAnalParameterCode.DaysToExpiration.ToString(), typeof(string));
                dtGraph.Columns.Add(CalculatedParamters.Delta.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.Gamma.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.Theta.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.Vega.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.Rho.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.DollarDeltaInBaseCurrency.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.DollarGammaInBaseCurrency.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.DollarThetaInBaseCurrency.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.DollarVegaInBaseCurrency.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.DollarRhoInBaseCurrency.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.SimulatedPriceInBaseCurrency.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.DeltaAdjExposureInBaseCurrency.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.CostBasisUnrealizedPnLInBaseCurrency.ToString(), typeof(double));
                dtGraph.Columns.Add(CalculatedParamters.SimulatedPnlInBaseCurrency.ToString(), typeof(double));
                dtGraph.Columns.Add(xParameterCode + "MUL", typeof(string));
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtGraph;
        }

        public List<string> GetSymbols()
        {
            List<string> symbols = new List<string>();
            try
            {

                foreach (string symb in _symbolDataCollection.Keys)
                {
                    symbols.Add(symb);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return symbols;
        }

        public DataTable GetDataTable(string symbol)
        {
            return _symbolDataCollection[symbol];
        }

        public void ClearCache()
        {
            try
            {
                _symbolDataCollection.Clear();
                weightedGreekCalcInputsForPortfolioDict.Clear();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
