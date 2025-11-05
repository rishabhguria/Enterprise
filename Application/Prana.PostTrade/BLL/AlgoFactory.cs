using Prana.BusinessObjects;


namespace Prana.PostTrade
{
    class AlgoFactory
    {
        public static AlgoBase GetAlgo(PostTradeEnums.CloseTradeAlogrithm algorithm)
        {
            AlgoBase algoClass = null;
            if (algorithm == PostTradeEnums.CloseTradeAlogrithm.LIFO)
            {
                algoClass = new LIFO();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.FIFO)
            {
                algoClass = new FIFO();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.HIFO)
            {
                algoClass = new HIFO();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.MFIFO)
            {
                algoClass = new MFIFO();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.ETM)
            {
                algoClass = new ETM();

            }

            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.LOWCOST)
            {
                algoClass = new LOWCOST();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.ACA)
            {
                algoClass = new ACA();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.HIHO)
            {
                algoClass = new HIHO();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.BTAX)
            {
                algoClass = new BTAX();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.TAXADV)
            {
                algoClass = new TAXADV();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.MANUAL)
            {
                algoClass = new MANUAL();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.PRESET)
            {
                algoClass = new FIFO();

            }
            else if (algorithm == PostTradeEnums.CloseTradeAlogrithm.HCST) // Added by omshiv, 2015 Jun PRANA-8247
            {
                algoClass = new HCST();

            }

            return algoClass;

        }
    }
}
