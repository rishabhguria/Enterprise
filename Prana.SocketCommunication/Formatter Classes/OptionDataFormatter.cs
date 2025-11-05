using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;
namespace Prana.SocketCommunication
{
    public class OptionDataFormatter
    {
        public const string MSGTYPE_SUBSCRIBE_SYMBOLS = "SubSy";
        public const string MSGTYPE_UNSUBSCRIBE_SYMBOLS = "UnSubSy";
        public const string MSGTYPE_PricingData = "PricingData";
        public const string MSGTYPE_PricingDataFile = "PricingDataFile";
        public const string MSGTYPE_OptionGreeksData = "OPT_G";
        public const string MSGTYPE_OptionSnapShotData = "OPT_Snap";
        public const string MSGTYPE_SnapShotData = "SnapShot";
        public const string MSGTYPE_OptionStelAnalData = "OPT_Step";
        public const string MSGTYPE_PREFS_REFRESH = "Prefs_Refresh";
        public const string MSGTYPE_OMI_UPDATED = "OMI_Updated";
        public const string MSGTYPE_USERInputData = "UserInputData";
        public static string CreateMsgForOptionGreeks(Dictionary<string, SymbolData> optionGreeks, string msgType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(msgType);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(Seperators.SEPERATOR_4);
            foreach (KeyValuePair<string, SymbolData> optionGreekKeyValue in optionGreeks)
            {
                sb.Append(optionGreekKeyValue.Value.ToString());
                sb.Append(Seperators.SEPERATOR_4);
            }
            return sb.ToString();
        }
        public static Dictionary<string, SymbolData> CreateOptionGreeksFromString(string msg)
        {
            string[] msgArray = msg.Split(Seperators.SEPERATOR_4);
            Dictionary<string, SymbolData> dictOfCalculatedOptionGreeks = new Dictionary<string, SymbolData>();
            for (int count = 1; count < msgArray.Length; count++)
            {
                if (msgArray[count] != string.Empty)
                {
                    SymbolData obj = LiveFeedDataInstanceCreater.CreateDataInstance(msgArray[count]);
                    try
                    {
                        if (obj != null)
                        {
                            if (dictOfCalculatedOptionGreeks.ContainsKey(obj.Symbol))
                            {
                                ///Updated Rajat 29 June 2007.
                                dictOfCalculatedOptionGreeks[obj.Symbol] = obj;
                            }
                            else
                            {
                                dictOfCalculatedOptionGreeks.Add(obj.Symbol, obj);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return dictOfCalculatedOptionGreeks;
        }
        public static string CreateMsgForSnapShot(InputParametersCollection inputparamsList)
        {
            // StringBuilder slower for less than 4-5 string concatenation 
            // If adding values use the commented stringbuilder :Harsh
            //StringBuilder sb = new StringBuilder();
            //sb.Append(MSGTYPE_OptionSnapShotData);
            //sb.Append(Seperators.SEPERATOR_2);
            //sb.Append(inputparamsList.ToString());
            //return sb.ToString();
            return MSGTYPE_OptionSnapShotData + Seperators.SEPERATOR_2 + inputparamsList.ToString();
        }
        public static string CreateMsgForStepAnalysis(GeneralMatrix generalMatrix)
        {
            // StringBuilder slower for less than 4-5 string concatenation 
            // If adding values use the commented stringbuilder :Harsh
            //StringBuilder sb = new StringBuilder();
            //sb.Append(MSGTYPE_OptionStelAnalData);
            //sb.Append(Seperators.SEPERATOR_2);
            //sb.Append(generalMatrix.ToString());           
            //return sb.ToString();
            return MSGTYPE_OptionStelAnalData + Seperators.SEPERATOR_2 + generalMatrix.ToString();
        }
        public static GeneralMatrix CreateMsgFromStepAnalysis(string data)
        {
            GeneralMatrix generalMatrix = new GeneralMatrix(data);
            return generalMatrix;
        }
    }

    public class OptionMessageSubscribe
    {

        string _userID = string.Empty;

        Dictionary<string, List<string>> _optionSymbolList = new Dictionary<string, List<string>>();

        public string CreateMsgForSubcribingSymbols(string userID, Dictionary<string, List<string>> optionSymbolList)
        {
            _optionSymbolList = optionSymbolList;
            _userID = userID;

            StringBuilder sb = new StringBuilder();
            sb.Append(OptionDataFormatter.MSGTYPE_SUBSCRIBE_SYMBOLS);
            sb.Append(Seperators.SEPERATOR_2);
            sb.Append(_userID);

            foreach (KeyValuePair<string, List<string>> symbolData in _optionSymbolList)
            {
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(symbolData.Key);
                sb.Append(Seperators.SEPERATOR_3);
                foreach (string symbol in symbolData.Value)
                {
                    sb.Append(symbol);
                    sb.Append(Seperators.SEPERATOR_3);
                }
            }
            return sb.ToString();
        }

        public OptionMessageSubscribe(string message)
        {
            string[] symbolList = message.Split(Seperators.SEPERATOR_2);

            _userID = symbolList[1];


            for (int count = 2; count < symbolList.Length; count++)
            {
                string[] symbolList1 = symbolList[count].Split(Seperators.SEPERATOR_3);

                string underLyingSymbol = symbolList1[0];

                _optionSymbolList.Add(underLyingSymbol, new List<string>());

                for (int count1 = 1; count1 < symbolList1.Length; count1++)
                {
                    if (symbolList1[count1] != string.Empty)
                    {
                        _optionSymbolList[underLyingSymbol].Add(symbolList1[count1]);
                    }
                }
            }
        }

        public OptionMessageSubscribe()
        {

        }
        public string UserID
        {
            get { return _userID; }
        }
        public Dictionary<string, List<string>> OptionSymbolsList
        {
            get { return _optionSymbolList; }
        }


    }

    public class OptionMessageUnSubscribe
    {

        string _userID = string.Empty;

        List<string> _optionSymbolList = new List<string>();
        public string CreateMsgForUnsubscribing(string userID, List<string> symbolList)
        {
            StringBuilder sb = new StringBuilder();
            try
            {

                sb.Append(OptionDataFormatter.MSGTYPE_UNSUBSCRIBE_SYMBOLS);
                sb.Append(Seperators.SEPERATOR_1);
                sb.Append(userID);
                sb.Append(Seperators.SEPERATOR_1);
                foreach (string symbol in symbolList)
                {
                    sb.Append(symbol);
                    sb.Append(Seperators.SEPERATOR_2);
                }
            }
            catch (Exception)
            {
                throw new Exception("Message Format Error at CreateMsgForUnsubscribing");
            }
            return sb.ToString();
        }

        public OptionMessageUnSubscribe(string message)
        {




            try
            {
                string[] symbolsArray = message.Split(Seperators.SEPERATOR_1);
                _userID = symbolsArray[1];

                string[] symbolsToUnsubsCribe = symbolsArray[2].Split(Seperators.SEPERATOR_2);
                foreach (string symbolToUnsubs in symbolsToUnsubsCribe)
                {
                    if (symbolToUnsubs != string.Empty)
                    {
                        _optionSymbolList.Add(symbolToUnsubs);
                    }
                }
            }
            catch (Exception)
            {

                throw new Exception("Message Format Error at CreateMsgForUnsubscribing");
            }
        }

        public OptionMessageUnSubscribe()
        {

        }
        public string UserID
        {
            get { return _userID; }
        }
        public List<string> OptionSymbolsList
        {
            get { return _optionSymbolList; }
        }


    }



}
