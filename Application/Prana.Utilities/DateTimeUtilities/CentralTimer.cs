using System;
using System.Timers;

namespace Prana.Utilities.DateTimeUtilities
{
    /// <summary>
    /// Singleton timer class to facilitate the need for the Prana application
    /// </summary>
    public class CentralTimer : IDisposable
    {
        private static CentralTimer _centralTimer = null;
        static Timer tmrCentral = new Timer();

        #region Singleton Implementation

        private CentralTimer()
        {
            tmrCentral.Elapsed += new ElapsedEventHandler(tmrCentral_Elapsed);
            tmrCentral.Start();
        }

        private static readonly object _lockerObj = new object();

        /// <summary>
        /// Get Single instance 
        /// </summary>
        /// <returns></returns>
        public static CentralTimer GetInstance()
        {
            if (_centralTimer == null)
            {
                lock (_lockerObj)
                {
                    if (_centralTimer == null)
                    {
                        _centralTimer = new CentralTimer();
                        tmrCentral.Interval = 100; // Default timer interval is 100 miliseconds
                    }
                }
            }
            return _centralTimer;
        }

        #endregion

        #region Property
        /// <summary>
        /// User can set the interval for the timer.
        /// </summary>
        private double _timerInterval;
        public double TimerInterval
        {
            get
            {
                return _timerInterval;
            }
            set
            {
                _timerInterval = value;
                tmrCentral.Interval = _timerInterval;
            }
        }

        #endregion

        #region Event Declaration

        /// <summary>
        /// Following event will notify all modules about the central timer firing.
        /// Modules can register with any event of their interest
        /// </summary>
        public event EventHandler TenTimesPerSecond; // Every 100 milisecond 
        public event EventHandler FiveTimesPerSecond; // Every 200 milisecond
        public event EventHandler ThreeTimesPerSecond; // Every 500 milisecond 
        public event EventHandler TwoTimesPerSecond; // Every 500 milisecond 
        public event EventHandler PerSecond; //Every second
        public event EventHandler PerFiveSeconds; //After Every 5 Seconds
        public event EventHandler PerTenSeconds; //After Every 10 Seconds
        public event EventHandler PerTwentySeconds; //After Every 20 Seconds
        public event EventHandler PerMinute; //After Every 60 Seconds
        public event EventHandler PerFiveMinute; //After Every 5 minutes

        #endregion

        #region Fire Periodical Events



        static int timerCount = 1;
        /// <summary>
        /// It is assumed that, the timer interval has been set to 100 miliseconds
        /// On timer elasped fire various events every 100,200,500,1000 miliseconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrCentral_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (TenTimesPerSecond != null)
                TenTimesPerSecond(this, EventArgs.Empty);
            //EventHelper.FireAsync(TenTimesPerSecond,this,EventArgs.Empty);

            if (timerCount % 2 == 0) // Fires every 200 miliseconds
            {
                if (FiveTimesPerSecond != null) // Fires every 500 miliseconds
                    FiveTimesPerSecond(this, EventArgs.Empty);
                //EventHelper.FireAsync(FiveTimesPerSecond,this,EventArgs.Empty);
            }

            if (timerCount % 3 == 0) // Fires every 300 miliseconds
            {
                if (ThreeTimesPerSecond != null) // Fires every 500 miliseconds
                    ThreeTimesPerSecond(this, EventArgs.Empty);
                //EventHelper.FireAsync(ThreeTimesPerSecond,this,EventArgs.Empty);
            }

            if (timerCount % 5 == 0) // Fires every 500 miliseconds
            {
                if (TwoTimesPerSecond != null)
                    TwoTimesPerSecond(this, EventArgs.Empty);
                //EventHelper.FireAsync(TwoTimesPerSecond,this,EventArgs.Empty);

                //				Debug.WriteLine("In TwoTimesPerSecond : " + timerCount + " " + DateTime.Now.Minute + " " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);
            }

            if (timerCount % 10 == 0) // Fires every 1000 miliseconds
            {
                if (PerSecond != null)
                    PerSecond(this, EventArgs.Empty);
                //EventHelper.FireAsync(PerSecond,this,EventArgs.Empty);
            }
            if (timerCount % 50 == 0) // Fires after every 5 seconds
            {
                if (PerFiveSeconds != null)
                    PerFiveSeconds(this, EventArgs.Empty);
                //EventHelper.FireAsync(PerFiveSeconds,this,EventArgs.Empty);
            }
            if (timerCount % 100 == 0) // Fires after every 10 seconds
            {
                if (PerTenSeconds != null)
                    PerTenSeconds(this, EventArgs.Empty);
                //EventHelper.FireAsync(PerTenSeconds,this,EventArgs.Empty);
            }
            if (timerCount % 200 == 0) // Fires after every 20 seconds
            {
                if (PerTwentySeconds != null)
                    PerTwentySeconds(this, EventArgs.Empty);
                //EventHelper.FireAsync(PerTwentySeconds,this,EventArgs.Empty);
            }
            if (timerCount % 600 == 0) // Fires after every 1 minute
            {
                if (PerMinute != null)
                    PerMinute(this, EventArgs.Empty);
                //EventHelper.FireAsync(PerMinute,this,EventArgs.Empty);
            }
            if (timerCount % 3000 == 0) // Fires after every 5 minute
            {
                if (PerFiveMinute != null)
                    PerFiveMinute(this, EventArgs.Empty);
                //EventHelper.FireAsync(PerFiveMinute, this, EventArgs.Empty);
            }



            ++timerCount;
            if (timerCount == 3000)
                timerCount = 1;

            //			Debug.WriteLine(timerCount + " " + DateTime.Now.Minute + " " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            tmrCentral.Stop();
            tmrCentral.Elapsed -= new ElapsedEventHandler(tmrCentral_Elapsed);
            tmrCentral.Dispose();
        }

        #endregion
    }
}
