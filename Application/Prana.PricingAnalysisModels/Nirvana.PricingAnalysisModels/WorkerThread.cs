//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Collections;
//using System.Threading;

//namespace Prana.PricingAnalysisModels
//{
//    class WorkerThread
//    {
//        Thread msgDispacher;
//        ManualResetEvent processsignaller;
//        public WorkerThread(object sender)
//        {
//            processsignaller = new ManualResetEvent(false);

//            ThreadPool.QueueUserWorkItem(new WaitCallback(GetValues));
//        }
//        private void GetValues(object state)
//        {
//            try
//            {
//                while (true)
//                {
//                    processsignaller.WaitOne(1000, false);
//                    processsignaller.Reset();
//                }
//            }
//            catch (Exception ex)
//            {
//                //Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
//            }
//        }
//    }
//}
