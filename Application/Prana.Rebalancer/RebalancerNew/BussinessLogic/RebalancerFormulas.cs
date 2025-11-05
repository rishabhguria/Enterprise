namespace Prana.Rebalancer.RebalancerNew.BussinessLogic
{
    public static class RebalancerFormulas
    {
        public static decimal GetWeightedPrice(decimal prevTargetPercentage, decimal prevPrice, decimal targetPercenrage, decimal price)
        {
            decimal weightedPrice = 0;
            if (prevTargetPercentage + targetPercenrage != 0)
                weightedPrice = ((prevTargetPercentage * prevPrice) + (targetPercenrage * price)) / (prevTargetPercentage + targetPercenrage);
            return weightedPrice;

        }
    }
}
