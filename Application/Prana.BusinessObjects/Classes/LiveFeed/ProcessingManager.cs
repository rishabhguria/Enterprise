using Prana.LogManager;
using System;
using System.Collections;
using System.Threading;
namespace Prana.BusinessObjects.LiveFeed
{
    public class ProcessingManager : IDisposable
    {
        Queue mmidQueue;
        Thread msgDispatcher;
        ManualResetEvent processSignaller;
        MMIDBook _mmidBook;
        System.Timers.Timer timer = new System.Timers.Timer();
        public ProcessingManager(MMIDBook mmidBook)
        {
            _mmidBook = mmidBook;

            mmidQueue = Queue.Synchronized(new Queue());

            processSignaller = new ManualResetEvent(false);

            msgDispatcher = new Thread(new ThreadStart(ProcessQueue));
            //msgDispatcher.Priority = ThreadPriority.;
            msgDispatcher.Start();

            timer.Interval = 200;
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //ProcessQueue();
        }

        public void Enqueue(object mmid)
        {
            try
            {
                mmidQueue.Enqueue(mmid);

                processSignaller.Set();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ProcessQueue()
        {
            try
            {
                while (true)
                {
                    // Once the dequeue is over it will wait here for the next signal. 
                    processSignaller.WaitOne(100, false);

                    processSignaller.Reset();

                    while (mmidQueue.Count > 0)
                    {
                        MarketMaker mmid = mmidQueue.Dequeue() as MarketMaker;

                        if (mmid != null)
                        {
                            _mmidBook.Process(mmid);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (processSignaller != null)
                        processSignaller.Dispose();
                    if (timer != null)
                        timer.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
