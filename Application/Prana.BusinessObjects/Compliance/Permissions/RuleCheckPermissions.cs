using Prana.BusinessObjects.Compliance.Enums;

namespace Prana.BusinessObjects.Compliance.Permissions
{
    public class RuleCheckPermissions
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is pre trade enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pre trade enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsPreTradeEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is override permission.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is override permission; otherwise, <c>false</c>.
        /// </value>
        public bool IsOverridePermission { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is apply to manual.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is apply to manual; otherwise, <c>false</c>.
        /// </value>
        public bool IsApplyToManual { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is trading.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is trading; otherwise, <c>false</c>.
        /// </value>
        public bool IsTrading { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is staging.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is staging; otherwise, <c>false</c>.
        /// </value>
        public bool IsStaging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [default pre pop up enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [default pre pop up enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool DefaultPrePopUpEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [default post pop up enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [default post pop up enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool DefaultPostPopUpEnabled { get; set; }

        /// <summary>
        /// The _default over ride type
        /// </summary>
        private RuleOverrideType _defaultOverRideType = RuleOverrideType.Soft;

        /// <summary>
        /// Gets or sets the default type of the over ride.
        /// </summary>
        /// <value>
        /// The default type of the over ride.
        /// </value>
        public RuleOverrideType DefaultOverRideType
        {
            get { return _defaultOverRideType; }
            set { _defaultOverRideType = value; }
        }

    }
}
