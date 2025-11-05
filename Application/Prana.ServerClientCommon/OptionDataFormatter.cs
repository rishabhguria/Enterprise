using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
namespace Prana.ServerClientCommon
{
   public  class OptionDataFormatter
    {
        public const string MSGTYPE_SUBSCRIBE_SYMBOLS = "SubSy";
        public const string MSGTYPE_UNSUBSCRIBE_SYMBOLS = "UnSubSy";
        public const string MSGTYPE_OptionLiveFeedData = "OPT_Live";
        public const string MSGTYPE_OptionGreeksData = "OPT_G";
        public const string MSGTYPE_OptionSnapShotData = "OPT_Snap";
       public const string MSGTYPE_OptionStelAnalData = "OPT_Step";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOfSymbols"></param>
        /// <returns></returns>


       public static string CreateMsgForOptionGreeks(Dictionary<string, OptionGreeks> optionGreeks,string msgType)
       {
           StringBuilder sb= new StringBuilder();
           sb.Append(msgType);
           sb.Append(Seperators.SEPERATOR_1);
           foreach (KeyValuePair<string, OptionGreeks> optionGreekKeyValue in optionGreeks)
           {
               sb.Append(optionGreekKeyValue.Value.ToString());
               sb.Append(Seperators.SEPERATOR_1);
           }
           return sb.ToString();
       }
       public static Dictionary<string, OptionGreeks> CreateOptionGreeksFromString(string msg)
       {
           string[] msgArray = msg.Split(Seperators.SEPERATOR_1);
           Dictionary<string, OptionGreeks> dictOptionGreeks = new Dictionary<string, OptionGreeks>();
           for(int count=1;count <msgArray.Length;count++)
           {
               if (msgArray[count] != string.Empty)
               {
                   OptionGreeks optionGreek = new OptionGreeks(msgArray[count]);
                   if (dictOptionGreeks.ContainsKey(optionGreek.Symbol))
                   {
                       ///Updated Rajat 29 June 2007.
                       dictOptionGreeks[optionGreek.Symbol] = optionGreek;
                   }
                   else
                   {
                       dictOptionGreeks.Add(optionGreek.Symbol, optionGreek);
                   }
                   
               }
               
           }
           return dictOptionGreeks;
       }
       public static string CreateMsgForSnapShot(InputParametersCollection inputparamsList)
       {
           StringBuilder sb = new StringBuilder();
           sb.Append(MSGTYPE_OptionSnapShotData);
           sb.Append(Seperators.SEPERATOR_1);
           sb.Append(inputparamsList.ToString());
           return sb.ToString();
       }

       public static string CreateRequestMsgForStepAnal(StepAnalysisRequestObject stepReqObj)
       {
           StringBuilder sb = new StringBuilder();
           sb.Append(MSGTYPE_OptionStelAnalData);
           sb.Append(Seperators.SEPERATOR_1);
           sb.Append(stepReqObj.ToString());
           return sb.ToString();
       }
       public static string CreateMsgForStepAnalysis(GeneralMatrix generalMatrix)
       {
           StringBuilder sb = new StringBuilder();
           sb.Append(MSGTYPE_OptionStelAnalData);
           sb.Append(Seperators.SEPERATOR_1);
           sb.Append(generalMatrix.ToString());           
           return sb.ToString();
       }
       public static GeneralMatrix CreateMsgFromStepAnalysis(string data)
       {
           GeneralMatrix generalMatrix = new GeneralMatrix(data);
           return generalMatrix;
       }
   }

   public class OptionMessageSubscribe
    {
        
        int  _userID = int.MinValue ;

        Dictionary<string, List<string>> _optionSymbolList = new Dictionary<string, List<string>>();

       public string CreateMsgForSubcribingSymbols(int userID,Dictionary<string, List<string>> optionSymbolList)
        {
            _optionSymbolList = optionSymbolList;
            _userID = userID;
           
            StringBuilder sb = new StringBuilder();
            sb.Append(OptionDataFormatter.MSGTYPE_SUBSCRIBE_SYMBOLS);
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_userID);

            foreach (KeyValuePair<string, List<string>> symbolData in _optionSymbolList)
            {
                sb.Append(Seperators.SEPERATOR_1);
                sb.Append(symbolData.Key);
                sb.Append(Seperators.SEPERATOR_2);
                foreach (string symbol in symbolData.Value)
                {
                    sb.Append(symbol);
                    sb.Append(Seperators.SEPERATOR_2);
                }
            }
            return sb.ToString();
        }

        public OptionMessageSubscribe(string message)
        {
            string[] symbolList = message.Split(Seperators.SEPERATOR_1);

            _userID =int.Parse(symbolList[1]);
            

            for (int count = 2; count < symbolList.Length; count++)
            {
                string[] symbolList1 = symbolList[count].Split(Seperators.SEPERATOR_2);

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
       public int  UserID
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

        int _userID = int.MinValue;

        List<string> _optionSymbolList = new List<string>();
       public  string CreateMsgForUnsubscribing(int userID, List<string> symbolList)
       {
           StringBuilder sb = new StringBuilder();
           try
           {

               sb.Append(OptionDataFormatter.MSGTYPE_UNSUBSCRIBE_SYMBOLS);
               sb.Append(Seperators.SEPERATOR_1);
               sb.Append(userID.ToString());
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
                _userID = int.Parse(symbolsArray[1]);

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
        public int UserID
        {
            get { return _userID; }
        }
        public  List<string> OptionSymbolsList
        {
            get { return _optionSymbolList; }
        }


    }
    
    

}
