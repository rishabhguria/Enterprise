
using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prana.Utilities.UI.MiscUtilities
{
    public class EnumHelper
    {
        //Aman Seth
        //For handling Special Cases
        //Old Method : ConvertEnumForBindingWithCaption converts PostitonSide to Position Side, UDAAssets to U D A Assets
        //But This method returns PostitonSide to Position Side, UDAAssets to UDA Assets in EnumerationValue's DisplayText
        public static ValueList ConvertEnumForBindingGridColumn(Type enumType) // System.Enum enumeration)
        {
            ValueList results = new ValueList();
            // Use reflection to see what values the enum provides
            string[] members = Enum.GetNames(enumType);

            foreach (string member in members)
            {
                string name = member; // sud -- delete this no need for new "name reference"
                StringBuilder newText = new StringBuilder(name.Length + 4);
                //newText.Append(name[0]);
                for (int i = 0; i < name.Length; i++)
                {
                    if ((i + 1 < name.Length && char.IsUpper(name[i]) && char.IsLower(name[i + 1]) && i > 0) || (i - 1 >= 0 && (char.IsLower(name[i - 1]) && char.IsUpper(name[i]))))
                        newText.Append(' ');
                    newText.Append(name[i]);
                }
                results.ValueListItems.Add(name, newText.ToString());
            }
            return results;
        }

        /// <summary>
        /// Get Formated text with space from enum
        /// - om, sep-2013
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static String GetFormatedText(String Text)
        {
            StringBuilder newText = new StringBuilder(Text.Length + 4);
            try
            {


                //newText.Append(name[0]);
                for (int i = 0; i < Text.Length; i++)
                {
                    if ((i + 1 < Text.Length && char.IsUpper(Text[i]) && char.IsLower(Text[i + 1]) && i > 0) || (i - 1 >= 0 && (char.IsLower(Text[i - 1]) && char.IsUpper(Text[i]))))
                        newText.Append(' ');
                    newText.Append(Text[i]);
                }



            }
            catch (Exception)
            {

                throw;
            }
            return newText.ToString();
        }

        //Bharat Kumar Jangir (26 December 2013)
        //For handling Special Cases
        //Old Method : ConvertEnumForBindingWithCaption converts PostitonSide to Position Side, UDAAssets to U D A Assets
        //But This method returns PostitonSide to Position Side, UDAAssets to UDA Assets in EnumerationValue's DisplayText
        public static List<EnumerationValue> ConvertEnumForBindingWithCaption(Type enumType) // System.Enum enumeration)
        {
            List<EnumerationValue> results = new List<EnumerationValue>();
            // Use reflection to see what values the enum provides
            string[] members = Enum.GetNames(enumType);

            foreach (string member in members)
            {
                string name = member; // sud -- delete this no need for new "name reference"
                StringBuilder newText = new StringBuilder(name.Length + 4);
                //newText.Append(name[0]);
                for (int i = 0; i < name.Length; i++)
                {
                    if ((i + 1 < name.Length && char.IsUpper(name[i]) && char.IsLower(name[i + 1]) && i > 0) || (i - 1 >= 0 && (char.IsLower(name[i - 1]) && char.IsUpper(name[i]))))
                        newText.Append(' ');
                    newText.Append(name[i]);
                }
                results.Add(new EnumerationValue(newText.ToString(), name));
            }
            return results;
        }

        public static ValueList TransactionTypeHasToDisplay(ValueList valueListwithAllValues)
        {
            ValueList transactionTypeToDisplay = new ValueList();

            try
            {
                foreach (ValueListItem item in valueListwithAllValues.ValueListItems)
                {
                    if (TransactionTypeHasToDisplay(item.DataValue.ToString()))
                    {
                        transactionTypeToDisplay.ValueListItems.Add(item.DataValue, item.DisplayText);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return transactionTypeToDisplay;
        }

        private static bool TransactionTypeHasToDisplay(string transactionType)
        {
            if (transactionType.Equals(TradingTransactionType.Buy.ToString()) ||
                transactionType.Equals(TradingTransactionType.Sell.ToString()) ||
                transactionType.Equals(TradingTransactionType.SellShort.ToString()) ||
                transactionType.Equals(TradingTransactionType.BuytoCover.ToString()) ||
                transactionType.Equals(TradingTransactionType.BuytoClose.ToString()) ||
                transactionType.Equals(TradingTransactionType.BuytoOpen.ToString()) ||
                transactionType.Equals(TradingTransactionType.SelltoOpen.ToString()) ||
                transactionType.Equals(TradingTransactionType.SelltoClose.ToString()) ||
                 transactionType.Equals(TradingTransactionType.LongAddition.ToString()) ||
                transactionType.Equals(TradingTransactionType.LongWithdrawal.ToString()) ||
                transactionType.Equals(TradingTransactionType.ShortAddition.ToString()) ||
                transactionType.Equals(TradingTransactionType.ShortWithdrawal.ToString())
                )
            {
                return true;
            }

            return false;
        }
    }
}
