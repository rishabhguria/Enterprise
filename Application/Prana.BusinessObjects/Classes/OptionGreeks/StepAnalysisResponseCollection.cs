using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    public class StepAnalysisResponseCollection
    {
        List<StepAnalysisResponse> _stepAnalsisResponseList = new List<StepAnalysisResponse>();
        public void Add(StepAnalysisResponse stepAnalRes)
        {
            _stepAnalsisResponseList.Add(stepAnalRes);

        }
        public void Remove(StepAnalysisResponse stepAnalRes)
        {
            _stepAnalsisResponseList.Remove(stepAnalRes);

        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (StepAnalysisResponse stepRes in _stepAnalsisResponseList)
            {
                sb.Append(stepRes.ToString());
                sb.Append(Seperators.SEPERATOR_1);
            }
            return sb.ToString();
        }

    }
}
