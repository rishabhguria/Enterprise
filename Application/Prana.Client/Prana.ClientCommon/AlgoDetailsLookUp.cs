using Prana.AlgoStrategyControls;

namespace Prana.ClientCommon
{
    public class AlgoDetailsLookUp
    {
        public static string GetAlgoStrategyText(string ID)
        {
            //The previous call to AlgoControlsDictionary.GetInstance().GetAlgoStrategyText(ID) was causing hang issue,
            //as AlgoControlsDictionary.GetInstance() calls Setup() which was extracting details from MyControls.xml.
            //Setup() was also creating controls, that hangs the release if call has been made from background thread.
            //return AlgoControlsDictionary.GetInstance().GetAlgoStrategyText(ID);
            return AlgoControlsDictionary.GetInstance().GetAlgoStrategyText(ID);

        }
    }
}
