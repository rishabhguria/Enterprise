using Prana.BusinessObjects;
using System.Collections.Generic;
using System.Text;

// Added By: Ankit Gupta
// On May 30, 2014
// To keep common functions of corporate action at a common place
namespace Prana.Utilities.MiscUtilities
{
    public class CAHelperClass
    {
        static CAHelperClass _caHelperClass = null;
        private static readonly object _lockerOnInstance = new object();

        private CAHelperClass()
        {

        }

        public static CAHelperClass GetInstance()
        {
            lock (_lockerOnInstance)
            {
                if (_caHelperClass == null)
                {
                    _caHelperClass = new CAHelperClass();
                }
                return _caHelperClass;
            }
        }
        /// <summary>
        /// This method is used to arrange taxlotids in a format which are used to unwind the closing in a single server call
        /// </summary>
        /// <param name="closingInfoList"></param>
        /// <param name="sbClosingID"></param>
        /// <param name="sbPositionalandClosingTaxlotID"></param>
        /// <param name="taxlotClosingIDWithClosingDate"></param>
        public void GetClosingTaxlotFormatedID(List<ClosingInfo> closingInfoList, ref StringBuilder sbClosingID, ref StringBuilder sbPositionalandClosingTaxlotID, ref StringBuilder taxlotClosingIDWithClosingDate)
        {

            Dictionary<string, StatusInfo> positionStatusDict = new Dictionary<string, StatusInfo>();

            // Unwinding is done with one server call
            foreach (ClosingInfo closingInfo in closingInfoList)
            {
                if (closingInfo != null)
                {
                    if (!((positionStatusDict.ContainsKey(closingInfo.PositionalTaxlotID)) || (positionStatusDict.ContainsKey(closingInfo.ClosingTaxlotID))))
                    {
                        sbClosingID.Append(closingInfo.ClosingID);
                        sbClosingID.Append(",");

                        taxlotClosingIDWithClosingDate.Append(closingInfo.ClosingID.ToString());
                        taxlotClosingIDWithClosingDate.Append('_');
                        taxlotClosingIDWithClosingDate.Append(closingInfo.ClosingTradeDate.ToString());
                        taxlotClosingIDWithClosingDate.Append(",");
                        sbPositionalandClosingTaxlotID.Append(closingInfo.PositionalTaxlotID.ToString());
                        sbPositionalandClosingTaxlotID.Append(",");
                        sbPositionalandClosingTaxlotID.Append(closingInfo.ClosingTaxlotID.ToString());
                        sbPositionalandClosingTaxlotID.Append(",");
                    }
                }
            }
        }
    }
}
