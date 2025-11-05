using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.Global;
using Prana.HeatMap.DAL.AmpqConnector;
using Prana.HeatMap.Delegates;
using Prana.HeatMap.Enums;
using Prana.HeatMap.EventArguments;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Prana.HeatMap.BLL
{
    class HeatMapManager
    {
        /// <summary>
        /// Locker object
        /// </summary>
        private static ReaderWriterLock _locker = new ReaderWriterLock();

        /// <summary>
        /// The current selected heat
        /// </summary>
        private Heats _currentSelectedHeat = Heats.PnL;

        /// <summary>
        /// Current Grouping
        /// </summary>
        private List<String> _currentGrouping = null;

        /// <summary>
        /// stores the drill values as a query
        /// </summary>
        private List<String> _drillValue = null;

        /// <summary>
        /// Storing the drill query so that it does not have to be created over and pver.
        /// Will be updated when user drills up or down
        /// </summary>
        private String _drillQuery = "";

        /// <summary>
        /// Contains the grouping level currently being viewed
        /// </summary>
        private int _openGroup = 0;

        public event UpdateData updateData;

        public event UpdateStatusBarHandler updateStatusBarHandler;

        private Boolean _esperConnected = false;

        public event EventHandler<EventArgs<Boolean>> UpdateConnectionStatus;

        #region SingiltonHandler

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static HeatMapManager singiltonInstance;

        /// <summary>
        /// Private Constructor
        /// </summary>
        private HeatMapManager()
        { }

        /// <summary>
        /// Provides the singiltan instance
        /// </summary>
        /// <returns></returns>
        internal static HeatMapManager GetInstance()
        {
            try
            {
                if (singiltonInstance == null)
                {
                    _locker.AcquireWriterLock(Timeout.Infinite);
                    if (singiltonInstance == null)
                        singiltonInstance = new HeatMapManager();
                    _locker.ReleaseWriterLock();
                }
                return singiltonInstance;
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
                return null;
            }
        }

        #endregion

        #region FilterRegion
        /// <summary>
        /// Update the filter cache
        /// </summary>
        /// <param name="conditions"></param>
        internal void UpdateFilterCache(List<SearchCondition> conditions)
        {
            try
            {
                HeatMapFilterCache.GetInstance().UpdateFilterCache(conditions);
                DataTable data = GetData(true);

                if (data != null && updateData != null)
                    updateData(this, new UpdateDataEventArgs() { Data = data });
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

        /// <summary>
        /// Get the Filter cache
        /// </summary>
        /// <returns></returns>
        internal List<SearchCondition> GetFiltercache()
        {
            try
            {
                return HeatMapFilterCache.GetInstance().GetFilterCache();
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
                return null;
            }
        }

        #endregion

        #region DataMgmtRegion

        /// <summary>
        /// Returns the data as per the current drilling
        /// Also raises apropriate status messages
        /// </summary>
        /// <param name="force"></param>
        /// <param name="drillDown"></param>
        /// <param name="drilledValue"></param>
        /// <returns></returns>
        private DataTable GetData(Boolean force)
        {
            try
            {
                if (_currentGrouping == null || _currentGrouping.Count == 0)
                    return null;

                DataTable data;
                if (force || DataCache.GetInstance().IsNewDataAvailable())
                    data = DataCache.GetInstance().GetDataCache();
                else
                    return null;

                GroupingAttributes attribute = (GroupingAttributes)Enum.Parse(typeof(GroupingAttributes), _currentGrouping[_openGroup]);
                String columnName = ((XmlEnumAttribute)typeof(GroupingAttributes)
                            .GetMember(attribute.ToString())[0]
                            .GetCustomAttributes(typeof(XmlEnumAttribute), false)[0])
                            .Name;
                String heatColumnName = ((XmlEnumAttribute)typeof(Heats)
                            .GetMember(_currentSelectedHeat.ToString())[0]
                            .GetCustomAttributes(typeof(XmlEnumAttribute), false)[0])
                            .Name;


                DataTable dataToSend = new DataTable();
                dataToSend.Columns.Add(columnName, typeof(System.String));
                dataToSend.Columns.Add(heatColumnName, typeof(System.Double));
                dataToSend.Columns.Add("color", typeof(System.Double));

                if (data == null || data.Rows.Count == 0)
                {
                    if (updateStatusBarHandler != null)
                        updateStatusBarHandler(this, new StatusBarEventArgs() { Status = _esperConnected ? "No data to display. Try changing grouping or filter" : "No data to display. Check connection to calculation engine." });
                    if (updateData != null)
                        updateData(this, new UpdateDataEventArgs() { Data = dataToSend });
                    return null;
                }

                //apply drilling if any
                if (_openGroup > 0)
                {
                    var datarows = data.Copy().Select(_drillQuery);
                    if (datarows.Length > 0)
                        data = datarows.CopyToDataTable();
                    else
                    {
                        if (updateStatusBarHandler != null)
                            updateStatusBarHandler(this, new StatusBarEventArgs() { Status = "No data to display. Try changing grouping or filter" });
                        if (updateData != null)
                            updateData(this, new UpdateDataEventArgs() { Data = dataToSend });
                        return null;
                    }
                }

                var groupedData = data.AsEnumerable().GroupBy(row => row.Field<String>(columnName)).ToList();

                double divisor = 0;
                foreach (var group in groupedData)
                {
                    DataRow row = dataToSend.NewRow();
                    String key = group.Key;
                    Double sum = group.Sum(x => x.Field<System.Double>(heatColumnName));
                    if (Math.Abs(sum) > divisor)
                        divisor = Math.Abs(sum);
                    row[columnName] = key;
                    row[heatColumnName] = sum;
                    dataToSend.Rows.Add(row);
                }
                dataToSend.Columns["color"].Expression = String.Format("{0}/{1}", heatColumnName, divisor);

                //if (updateStatusBarHandler != null)
                //   updateStatusBarHandler(this, new StatusBarEventArgs() { Status = "" });
                return dataToSend;
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
                return null;
            }
        }

        /// <summary>
        /// Sets the heatmap grouping level
        /// </summary>
        /// <param name="currentGrouping"></param>
        public void SetGrouping(List<String> currentGrouping)
        {
            try
            {
                _currentGrouping = currentGrouping;
                _openGroup = 0;
                _drillValue = new List<string>();
                _drillQuery = "";

                DataTable data = GetData(true);

                if (data == null)
                    data = new DataTable();

                if (updateData != null)
                    updateData(this, new UpdateDataEventArgs() { Data = data });
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

        /// <summary>
        /// Drills down the selected tile
        /// </summary>
        /// <param name="value"></param>
        public void DrillDown(String value)
        {
            try
            {
                if (_openGroup == _currentGrouping.Count - 1) // if max drilled down
                    return;

                //string drillColumn = ((XmlEnumAttribute)typeof(GroupingAttributes)
                //   .GetMember(_currentGrouping[_openGroup])[0]
                //   .GetCustomAttributes(typeof(XmlEnumAttribute), false)[0])
                //   .Name;
                _drillValue.Add(value);
                UpdateDrillQuery();

                _openGroup++;

                DataTable data = GetData(true);

                if (data == null)
                    data = new DataTable();

                if (updateData != null)
                    updateData(this, new UpdateDataEventArgs() { Data = data });
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

        #endregion

        /// <summary>
        /// Initialize the HeatMApManager
        /// Wires the different events
        /// </summary>
        public void Initialise()
        {
            try
            {
                AmqpConnectionManager.Initialise();
                AmqpConnectionManager.dsreceived += AmqpConnectionManager_dsreceived;
                AmqpConnectionManager.EsperConnected += AmqpConnectionManager_EsperConnected;
                AmqpConnectionManager.EsperDisconnected += AmqpConnectionManager_EsperDisconnected;

                new Task(() => { UpdateData(); }).Start();

                //_timer = new System.Timers.Timer(10000);
                //_timer.Elapsed += _timer_Elapsed;
                //_timer.Start();
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

        /// <summary>
        /// Handle esper disconnection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AmqpConnectionManager_EsperDisconnected(object sender, Global.EventArgs<BusinessObjects.ConnectionProperties> e)
        {
            try
            {
                _esperConnected = false;
                UpdateConnectionStatus(this, new EventArgs<Boolean>(false));
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

        /// <summary>
        /// Handle Esper connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AmqpConnectionManager_EsperConnected(object sender, Global.EventArgs<BusinessObjects.ConnectionProperties> e)
        {
            try
            {
                _esperConnected = true;
                UpdateConnectionStatus(this, new EventArgs<Boolean>(true));
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

        /// <summary>
        /// Returns the esper connection status
        /// </summary>
        /// <returns></returns>
        public Boolean IsEsperConnected()
        {
            try
            {
                return _esperConnected;
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
                return false;
            }
        }

        /// <summary>
        /// Serf calling function that regularly updates the heatmap if new data is available
        /// </summary>
        void UpdateData()
        {
            try
            {
                DataTable data = GetData(false);

                if (data != null)
                {
                    if (updateData != null)
                        updateData(this, new UpdateDataEventArgs() { Data = data });
                }

                Thread.Sleep(2000);
                new Task(() => { UpdateData(); }).Start();

                //  if (updateStatusBarHandler != null)
                //  updateStatusBarHandler(this, new StatusBarEventArgs { Status = "Data Updated", IsCompleteState = true });
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

        /// <summary>
        /// Updates the datacache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AmqpConnectionManager_dsreceived(object sender, EsperDataReceivedEventArgs e)
        {
            try
            {
                foreach (DataRow row in e.Data.Tables[0].Rows)
                {
                    DataCache.GetInstance().UpdateDataCache(row);
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

        /// <summary>
        /// Returns the current grouping
        /// </summary>
        /// <returns></returns>
        public List<String> GetGrouping()
        {
            try
            {
                return _currentGrouping;
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
                return null;
            }
        }

        /// <summary>
        /// Returns the grouping as a string
        /// </summary>
        /// <returns></returns>
        public String GetGroupingText()
        {
            try
            {
                if (_currentGrouping == null || _currentGrouping.Count == 0)
                    return "Select a Grouping To View";

                StringBuilder grpMsg = new StringBuilder();
                for (int i = 0; i < _currentGrouping.Count; i++)
                {
                    String group = _currentGrouping[i];
                    grpMsg.Append(group);
                    if (i < _drillValue.Count)
                        grpMsg.AppendFormat(" ({0})", _drillValue[i]);
                    grpMsg.Append(" > ");
                }

                if (_currentGrouping.Count > 0)
                    grpMsg.Length -= 3;

                return grpMsg.ToString();
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
                return null;
            }
        }

        /// <summary>
        /// Set the current heat
        /// </summary>
        /// <param name="heat"></param>
        public void SetCurrentHeat(Heats heat)
        {
            try
            {
                _currentSelectedHeat = heat;

                DataTable data = GetData(true);

                if (data == null)
                    data = new DataTable();

                if (updateData != null)
                    updateData(this, new UpdateDataEventArgs() { Data = data });
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

        /// <summary>
        /// Drill up the heat map
        /// </summary>
        public void DrillUp()
        {
            try
            {
                if (_openGroup == 0) // if max drilled down
                    return;

                _openGroup--;

                _drillValue.RemoveAt(_openGroup);
                UpdateDrillQuery();

                DataTable data = GetData(true);

                if (data == null)
                    data = new DataTable();

                if (updateData != null)
                    updateData(this, new UpdateDataEventArgs() { Data = data });
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

        /// <summary>
        /// Update the drill query, Called after drill-up or drill-down
        /// </summary>
        private void UpdateDrillQuery()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _drillValue.Count; i++)
            {
                GroupingAttributes attribute = (GroupingAttributes)Enum.Parse(typeof(GroupingAttributes), _currentGrouping[i]);
                String columnName = ((XmlEnumAttribute)typeof(GroupingAttributes)
                            .GetMember(attribute.ToString())[0]
                            .GetCustomAttributes(typeof(XmlEnumAttribute), false)[0])
                            .Name;
                sb.AppendFormat("{0} = '{1}' AND ", columnName, _drillValue[i]);
            }
            sb.Append("true");
            _drillQuery = sb.ToString();
        }

        /// <summary>
        /// Getting Current Heat i.e Pnl and Exposure etc.
        /// </summary>
        /// <returns>state</returns>
        public Heats GetCurrentHeat()
        {
            try
            {
                return _currentSelectedHeat;
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
                return 0;
            }
        }

        /// <summary>
        /// Returns the lates data
        /// </summary>
        /// <returns></returns>
        public DataTable GetLatestData()
        {
            try
            {
                return GetData(true);
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
                return null;
            }
        }
    }
}
