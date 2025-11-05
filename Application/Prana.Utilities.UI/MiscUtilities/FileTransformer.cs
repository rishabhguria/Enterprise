//using Prana.LogManager;
//using System;

//namespace Prana.Utilities.UI
//{
//    public class FileTransformer
//    {
//        public static void GetTransformedXML(string inputXML, string outputXML, string xsltPath)
//        {
//            try
//            {
//                Prana.Utilities.XMLUtilities.XMLUtilities.GetTransformedXML(inputXML, outputXML, xsltPath);
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }
//    }
//}
