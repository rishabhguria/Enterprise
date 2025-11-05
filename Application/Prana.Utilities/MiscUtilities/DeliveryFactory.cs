using Prana.Interfaces;

namespace Prana.Utilities.MiscUtilities
{
    public class DeliveryFactory
    {
        private static DeliveryFactory _singleton = null;
        private static readonly object _locker = new object();
        public static DeliveryFactory GetInstance()
        {
            if (_singleton == null)
                lock (_locker)
                    if (_singleton == null)
                        _singleton = new DeliveryFactory();
            return _singleton;
        }
        public IDelivery GetDeliveryClass(string deliveryType)
        {
            IDelivery iFactory = null;
            switch (deliveryType.ToLower())
            {
                case "ftp": iFactory = new FTPDelivery(); break;
                case "mail": iFactory = new MailDelivery(); break;
            }
            return iFactory;
        }
        //public object GetDeliveryClass(string deliveryType)
        //{
        //    object oDelivery = null;
        //    switch (deliveryType.ToLower())
        //    {
        //        //case "ftp": oDelivery = new FTPDelivery(); break;
        //        case "mail": oDelivery = new MailDelivery(); break;
        //    }
        //    return oDelivery;
        //}
    }
}
