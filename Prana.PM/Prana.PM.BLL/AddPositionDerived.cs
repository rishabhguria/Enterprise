using Prana.BusinessObjects.PositionManagement;
using System;

namespace Prana.PM.BLL
{
    [Serializable]
    public class AddPositionDerived : Position
    {


        public AddPositionDerived()
        {
            MarkAsChild();
        }

        private bool _toBeIncluded;

        /// <summary>
        /// Gets or sets the position inclusion.
        /// </summary>
        /// <value>The position inclusion.</value>

        public bool ToBeIncluded
        {
            get { return _toBeIncluded; }
            set { _toBeIncluded = value; }
        }


        private int _assetID;

        /// <summary>
        /// Gets or sets the AssetID.
        /// </summary>
        /// <value>The AssetID.</value>

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        private string _assetName;

        /// <summary>
        /// Gets or sets the Asset Name.
        /// </summary>
        /// <value>The Asset Name.</value>

        public string AssetName
        {
            get { return _assetName; }
            set { _assetName = value; }
        }

        private int _underLyingID;

        /// <summary>
        /// Gets or sets the UnderLyingID.
        /// </summary>
        /// <value>The UnderLyingID.</value>

        public int UnderLyingID
        {
            get { return _underLyingID; }
            set { _underLyingID = value; }
        }

        private string _underLyingName;

        /// <summary>
        /// Gets or sets the UnderLying Name.
        /// </summary>
        /// <value>The UnderLying Name.</value>

        public string UnderLyingName
        {
            get { return _underLyingName; }
            set { _underLyingName = value; }
        }

        private int _venueID;

        /// <summary>
        /// Gets or sets the Venue ID.
        /// </summary>
        /// <value>The Venue ID.</value>

        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        private string _venueName;

        /// <summary>
        /// Gets or sets the Venue Name.
        /// </summary>
        /// <value>The Venue Name.</value>

        public string VenueName
        {
            get { return _venueName; }
            set { _venueName = value; }
        }

        private string _currencyName;

        /// <summary>
        /// Gets or sets the Currency Name.
        /// </summary>
        /// <value>The Currency Name.</value>

        public string CurrencyName
        {
            get { return _currencyName; }
            set { _currencyName = value; }
        }

        private int _symbolConventionID;

        /// <summary>
        /// Gets or sets the Symbol Convention ID.
        /// </summary>
        /// <value>The Symbol Convention ID.</value>

        public int SymbolConventionID
        {
            get { return _symbolConventionID; }
            set { _symbolConventionID = value; }
        }

        private int _strategyID;

        /// <summary>
        /// Gets or sets the Strategy ID.
        /// </summary>
        /// <value>The Strategy ID.</value>

        public new int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private string _strategyName;

        /// <summary>
        /// Gets or sets the Strategy Name.
        /// </summary>
        /// <value>The Strategy Name.</value>

        public string StrategyName
        {
            get { return _strategyName; }
            set { _strategyName = value; }
        }

        private string _exchangeName;

        /// <summary>
        /// Gets or sets the Exchange Name.
        /// </summary>
        /// <value>The Exchange Name.</value>

        public string ExchangeName
        {
            get { return _exchangeName; }
            set { _exchangeName = value; }
        }

        private int _counterPartyID;

        /// <summary>
        /// Gets or sets the CounterPartyID.
        /// </summary>
        /// <value>The CounterPartyID.</value>

        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        private string _counterPartyName;

        /// <summary>
        /// Gets or sets the CounterParty Name.
        /// </summary>
        /// <value>The CounterParty Name.</value>

        public string CounterPartyName
        {
            get { return _counterPartyName; }
            set { _counterPartyName = value; }
        }

        private string _description;

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>The Description.</value>

        public new string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _positionState;

        /// <summary>
        /// Gets or sets the Position State.
        /// </summary>
        /// <value>The Position State.</value>

        public new string PositionState
        {
            get { return _positionState; }
            set { _positionState = value; }
        }

        private double _realizedPNL;
        public double RealizedPNL
        {
            get
            {
                return _realizedPNL;
            }
            set
            {
                _realizedPNL = value;
            }
        }

        protected DateTime _positionEndDate;

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public virtual DateTime PositionEndDate
        {
            get { return _positionEndDate; }
            set { _positionEndDate = value; }
        }

        /// <summary>
        /// Gets the id value.
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _assetID;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

    }
}
