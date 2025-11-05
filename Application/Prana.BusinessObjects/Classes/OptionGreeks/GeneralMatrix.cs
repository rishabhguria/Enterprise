using System;
using System.Text;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class GeneralMatrix
    {
        string _userID = string.Empty;
        StepAnalysisResponse[][] A;
        int m = 0;
        int n = 0;
        public GeneralMatrix()
        { }
        public GeneralMatrix(string data)
        {
            string[] dataArray = data.Split(Seperators.SEPERATOR_1);
            _symbol = dataArray[1];
            _underLyingsymbol = dataArray[2];
            Set(int.Parse(dataArray[3]), int.Parse(dataArray[4]), null);
            int count = 5;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (dataArray[count] != string.Empty)
                    {
                        A[i][j] = new StepAnalysisResponse(dataArray[count]);
                    }
                    count++;
                }
            }
        }
        public GeneralMatrix(int m, int n, StepAnalysisResponse s)
        {
            Set(m, n, s);
        }
        public void Set(int m, int n, StepAnalysisResponse s)
        {
            this.m = m;
            this.n = n;
            A = new StepAnalysisResponse[m][];
            for (int i = 0; i < m; i++)
            {
                A[i] = new StepAnalysisResponse[n];
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i][j] = s;
                }
            }
        }
        public void SetElement(int i, int j, StepAnalysisResponse s)
        {
            A[i][j] = s;
        }
        public StepAnalysisResponse GetElement(int i, int j)
        {
            return A[i][j];
        }
        public int RowLength
        {
            get { return m; }
        }
        public int ColumnLength
        {
            get { return n; }
        }
        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        private string _underLyingsymbol;

        public string UnderLyingSymbol
        {
            get { return _underLyingsymbol; }
            set { _underLyingsymbol = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_symbol.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_underLyingsymbol.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(m.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(n.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sb.Append(A[i][j].ToString());
                    sb.Append(Seperators.SEPERATOR_1);
                }
            }
            return sb.ToString();
        }

        public void SetInputParameters(InputParametersForGreeks inputParams, double value, StepAnalParameterCode code)
        {

            if (code == StepAnalParameterCode.UnderlyingPrice)
            {
                inputParams.SimulatedUnderlyingStockPrice = value;
            }
            else if (code == StepAnalParameterCode.Volatility)
            {
                inputParams.Volatility = value;
            }
            else if (code == StepAnalParameterCode.InterestRate)
            {
                inputParams.InterestRate = value;
            }
        }
    }

}
