//using Prana.BusinessObjects;
//using Prana.Interfaces;

//namespace Prana.Utilities.UI.MiscUtilities
//{
//    public class FileFormatterFactory
//    {
//        private static FileFormatterFactory _singleton = null;
//        private static object _locker = new object();
//        public static FileFormatterFactory GetInstance()
//        {
//            if (_singleton == null)
//                lock (_locker)
//                    if (_singleton == null)
//                        _singleton = new FileFormatterFactory();
//            return _singleton;
//        }
//        public IFileFormatter GetFormatterClass(AutomationEnum.FileFormat formatterType)
//        {
//            IFileFormatter iFactory = null;
//            switch (formatterType)
//            {
//                case AutomationEnum.FileFormat.csv:
//                    iFactory = new CSVFileFormatter();
//                    break;
//                case AutomationEnum.FileFormat.xls:
//                    iFactory = new ExeclFileFormatter();
//                    //iFactory = new CSVFileFormatter(); 
//                    break;
//            }
//            return iFactory;
//        }
//    }
//}
