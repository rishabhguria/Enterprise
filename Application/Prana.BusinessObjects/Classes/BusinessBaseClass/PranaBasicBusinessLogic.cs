namespace Prana.BusinessObjects
{
    public class PranaBasicBusinessLogic
    {
        public static bool IsLongSide(string orderSideTagValue)
        {
            if (orderSideTagValue == FIXConstants.SIDE_Buy ||
                orderSideTagValue == FIXConstants.SIDE_Buy_Open ||
                orderSideTagValue == FIXConstants.SIDE_Buy_Cover ||
                orderSideTagValue == FIXConstants.SIDE_Buy_Closed ||
                orderSideTagValue == FIXConstants.SIDE_BuyMinus
                )
            {
                return true;
            }
            else
                return false;
        }
    }
}
