//using System;
//using System.Collections.Generic;
//using System.Text;

//using Prana.BusinessObjects.PositionManagement;
//using Csla;
//using Csla.Validation;

//namespace Prana.PM.BLL
//{
//    [Serializable()]
//    public class CloseTradeInterface : BusinessBase<CloseTradeInterface>
//    {
//        private static CloseTradeInterface _closeTradeInterface;

//        #region Singleton Instance of CloseTradeInterface

//        static CloseTradeInterface()
//        {
//            if (_closeTradeInterface == null)
//            {
//                _closeTradeInterface = new CloseTradeInterface();
//            }
//        }

//        public static CloseTradeInterface GetInstance()
//        {          
//            return _closeTradeInterface;
//        } 
//        #endregion

//        private CloseTradePreferences _preferences;

//        /// <summary>
//        /// Gets or sets the preferences.
//        /// </summary>
//        /// <value>The preferences.</value>
//        public CloseTradePreferences CloseTradeFilter
//        {
//            get 
//            {
//                if (_preferences == null)
//                {
//                    _preferences = new CloseTradePreferences();
//                }  
//                return _preferences; 
//            }
//            set 
//            { 
//                _preferences = value;
//                PropertyHasChanged();
//            }
//        }

//        private AllocatedTradesList _allocatedTrades;

//        /// <summary>
//        /// Gets or sets the allocated tax lots.
//        /// </summary>
//        /// <value>The allocated trades data.</value>
//        public AllocatedTradesList AllocatedTrades
//        {
//            get 
//                {
//                    if (_allocatedTrades == null)
//                    {
//                        _allocatedTrades = new AllocatedTradesList();                        
//                    }
//                    return _allocatedTrades;
//                }
//            set 
//            {
//                _allocatedTrades = value;
//                PropertyHasChanged();
//            }
//        }

//        private AllocatedTradesList _strategyAllocatedTrades;

//        /// <summary>
//        /// Gets or sets the allocated tax lots.
//        /// </summary>
//        /// <value>The allocated trades data.</value>
//        public AllocatedTradesList StrategyAllocatedTrades
//        {
//            get
//            {
//                if (_strategyAllocatedTrades == null)
//                {
//                    _strategyAllocatedTrades = new AllocatedTradesList();
//                }
//                return _strategyAllocatedTrades;
//            }
//            set
//            {
//                _strategyAllocatedTrades = value;
//                PropertyHasChanged();
//            }
//        }

//        private NetPositionList _netPositions;

//        /// <summary>
//        /// Gets or sets the net positions.
//        /// </summary>
//        /// <value>The net positions.</value>
//        public NetPositionList NetPositions
//        {
//            get 
//            {
//                if (_netPositions == null)
//                {
//                    _netPositions = new NetPositionList();
//                }
//                return _netPositions; 
//            }
//            set 
//            { 
//                _netPositions = value;
//                PropertyHasChanged();
//            }
//        }

//        private NetPositionList _strategynetPositions;

//        /// <summary>
//        /// Gets or sets the strategy net positions.
//        /// </summary>
//        /// <value>The strategy net positions.</value>
//        public NetPositionList StrategyNetPositions
//        {
//            get
//            {
//                if (_strategynetPositions == null)
//                {
//                    _strategynetPositions = new NetPositionList();
//                }
//                return _strategynetPositions;
//            }
//            set
//            {
//                _strategynetPositions = value;
//                PropertyHasChanged();
//            }
//        }


//        /// <summary>
//        /// Gets a value indicating whether this instance is valid.
//        /// </summary>
//        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
//        public override bool IsValid
//        {
//            get { return base.IsValid; }
//        }

//        /// <summary>
//        /// Gets a value indicating whether this instance is dirty.
//        /// </summary>
//        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
//        public override bool IsDirty
//        {
//            get { return base.IsDirty; }
//        }

//        int _id;
//        /// <summary>
//        /// TODO : Can't afford to have this ID property in multiuer environment
//        /// </summary>
//        /// <returns></returns>
//        protected override object GetIdValue()
//        {
//            return _id;
//        }

//        //private AllocatedTradesList _sellSideData;


//        ///// <summary>
//        ///// Gets or sets the sell side data.
//        ///// </summary>
//        ///// <value>The sell side data.</value>
//        //public AllocatedTradesList SellSideData
//        //{
//        //    get { return _sellSideData; }
//        //    set { _sellSideData = value; }
//        //}

//    }
//}
