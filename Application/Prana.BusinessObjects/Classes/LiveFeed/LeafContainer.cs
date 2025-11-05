using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
namespace Prana.BusinessObjects.LiveFeed
{
    public class LeafContainer : Container, IEnumerable, IEnumerator, IDisposable
    {
        private int rowPos = -1;
        private IComparer<MarketMaker> sortPriorityMMID = new MMIDPriority() as IComparer<MarketMaker>;
        ArrayList mmidStore = ArrayList.Synchronized(new ArrayList());
        System.Timers.Timer timer = new System.Timers.Timer();
        /// <summary>
        /// TODO : Need to remove the windows timer to some other and remove the windows reference.
        /// </summary>
        System.Windows.Forms.Timer windowsFormtimer = new System.Windows.Forms.Timer();
        TimeZone exchangeTimezone = null;
        int addDayLightSavingHours = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("AddDayLightSavingHours"));
        readonly MarketMakerCollection mmidCollection = new MarketMakerCollection();
        public LeafContainer(string name)
            : base(name)
        {
            mmidCollection.RaiseListChangedEvents = false;

            //Changed Rajat 13 July 2007
            //TODO : In case of any other exchange, we can use FindTimeZone method of TimeZoneInfo or create similar properties for
            //TODO : other time zones.
            exchangeTimezone = TimeZoneInfo.EasternTimeZone;

            timer.Interval = 500;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();


        }

        public LeafContainer(string name, MarketMakerCollection _mmidCollection)
            : base(name)
        {
            mmidCollection = _mmidCollection;
            mmidCollection.RaiseListChangedEvents = false;
            //Changed Rajat 13 July 2007
            //TODO : In case of any other exchange, we can use FindTimeZone method of TimeZoneInfo or create similar properties for
            //TODO : other time zones.
            exchangeTimezone = TimeZoneInfo.EasternTimeZone;

            timer.Interval = 500;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        BindingList<MarketMaker> mmidGenericList = new BindingList<MarketMaker>();
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateBindingListToUI();
        }

        private void UpdateBindingListToUI()
        {
            try
            {
                lock (mmidCollection)
                {
                    //mmidCollection.Sort(sortPriorityPriceSize);
                    mmidCollection.CustomListChanged();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public MarketMakerCollection DataCollection
        {
            get { return mmidCollection; }
        }
        public BindingList<MarketMaker> DataGenericList
        {
            get { return mmidGenericList; }
        }
        public override void ProcessMMID(MarketMaker mmid)
        {

            // StoreDataInGenericHash(mmid);

            ProcessDataUsingIBindingList(mmid);

        }

        private void ProcessDataUsingIBindingList(MarketMaker mmid)
        {

            int i = 0;
            try
            {
                //lock (mmidCollection)
                //{
                //    mmidCollection.Sort(sortPriorityMMID);
                //}
                if (mmid.Time != null)
                {
                    DateTime recdBidTime = Convert.ToDateTime(mmid.Time); /// We are getting the time in GMT from the depth server
                    DateTime ESTTime = new DateTime(recdBidTime.Ticks);
                    if (exchangeTimezone != null)
                    {
                        ESTTime = new DateTime(recdBidTime.Ticks - (exchangeTimezone.Bias.Ticks));
                    }

                    mmid.Time = String.Format("{0:HH}:{0:mm}:{0:ss}", ESTTime.AddHours(addDayLightSavingHours));
                }
                //instLevel2Data.BidInfo.tTime.ToString() ;
                lock (mmidCollection)
                {
                    i = mmidCollection.Exists(mmid, sortPriorityMMID);
                    if (i >= 0)
                    {
                        switch (mmid.UpdateFlag)
                        {
                            case 'U':
                            case 'A':

                                mmidCollection[i].Price = mmid.Price;
                                mmidCollection[i].Size = mmid.Size;
                                mmidCollection[i].Time = mmid.Time;// String.Format("{0:HH}:{0:mm}:{0:ss}", ESTTime.AddHours(addDayLightSavingHours));
                                                                   // mmidCollection[i].PropertyHasChanged();
                                break;
                            case 'D':
                                mmidCollection.RemoveAt(i);
                                break;
                                // mmidCollection.RemoveAt(i);
                                // mmidCollection.Insert(i, mmid);
                                // break;
                        }
                    }
                    else
                    {
                        if (mmid.UpdateFlag != 'D')
                        {
                            //Using Bitwise Complement operator (Take positive value and then decrease it by 1)
                            //To find the 
                            mmidCollection.Insert(~i, mmid);
                        }
                    }

                    //  mmidCollection.Sort(sortPriorityPriceSize);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public override void ClearContainer()
        {
            //base.ClearContainer();
            mmidCollection.Clear();
        }



        #region IEnumerator Members

        public object Current
        {
            get { return mmidStore[rowPos]; }
        }

        /// <summary>
        /// Return true if able to move to the next Element in the list else return false
        /// Also deletes any entries with size 0 in the list.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            rowPos++;
            while (rowPos < mmidStore.Count)
            {
                MarketMaker curMMID = mmidStore[rowPos] as MarketMaker;
                if (curMMID.Size == 0)
                {
                    mmidStore.RemoveAt(rowPos);
                }
                else
                {
                    return true;
                }
            }
            Reset();
            return false;
        }

        public void Reset()
        {
            rowPos = -1;
        }

        #endregion

        #region IEnumerable Members

        public override IEnumerator GetEnumerator()
        {
            Reset();
            return this;
        }

        #endregion

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
                    if (windowsFormtimer != null)
                        windowsFormtimer.Dispose();
                    if (timer != null)
                        timer.Dispose();
                    if (mmidCollection != null)
                        mmidCollection.Dispose();
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
