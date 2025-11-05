using System.Configuration;
namespace Prana.OptionCalculator.Common
{
    public class FeedBackDetails
    {
        public static int OPTIMISED_LOT = 35;
        public static readonly int MAX_THREADSALLOWEDTO_GREEK_CALC = int.Parse(ConfigurationManager.AppSettings["MAX_THREADSALLOWEDTO_GREEK_CALC"]);
        public const int MAX_THREADSALLOWEDTO_IMPLIED_CALC = 2;
        public const int NUMBER_OF_GREEKS = 5;
    }
}
