using Prana.BusinessObjects;
using System.Collections.Generic;
using System.Data;

namespace Prana.AlgoStrategyControls
{
    public class FixSymbolValueTable
    {
        public FixSymbolValueTable()
        {

        }

        public static DataTable GetAlgoDetailsInDataTable(string cpID, string strategyID, Dictionary<string, string> algoTagValues)
        {
            AlgoStrategy algostrat = AlgoControlsDictionary.GetInstance().GetAlgoStrategyDatils(cpID, strategyID);

            DataTable fixsymboldatatable = new DataTable();
            object[] Row = new object[algoTagValues.Count + 1 - algostrat.StrategyTagValues.Count];
            int j = 1;
            fixsymboldatatable.Columns.Add("StrategyName");
            Row[0] = algostrat.StrategyName;


            foreach (KeyValuePair<string, string> keyvalvarUC in algoTagValues)
            {

                foreach (AlgoStrategyParameters algoStrategyParam in algostrat.AlgoparametersList)
                {
                    if (algoStrategyParam.IDtoNameMapping.ContainsKey(keyvalvarUC.Key))
                        fixsymboldatatable.Columns.Add(algoStrategyParam.IDtoNameMapping[keyvalvarUC.Key]);

                }

                if (!algostrat.StrategyTagValues.ContainsKey(keyvalvarUC.Key))
                {
                    Row[j] = keyvalvarUC.Value;
                    j++;
                }

            }

            fixsymboldatatable.Rows.Add(Row);
            return fixsymboldatatable;

        }


    }
}
