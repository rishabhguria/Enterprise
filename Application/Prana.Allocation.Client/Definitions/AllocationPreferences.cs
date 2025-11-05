using Prana.Allocation.Common.Definitions;
using Prana.BusinessObjects;
using System.Xml.Serialization;

namespace Prana.Allocation.Client.Definitions
{
    [XmlRoot("AllocationPreferences")]
    public class AllocationPreferences : IPreferenceData
    {
        #region Members

        /// <summary>
        /// The _auto grouping rules
        /// </summary>
        private AutoGroupingRules _autoGroupingRules;

        /// <summary>
        /// The _general rules
        /// </summary>
        private GeneralRules _generalRules;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the height of the allocation form.
        /// </summary>
        /// <value>
        /// The height of the allocation form.
        /// </value>
        public int AllocationFormHeight { get; set; }

        /// <summary>
        /// Gets or sets the width of the allocation form.
        /// </summary>
        /// <value>
        /// The width of the allocation form.
        /// </value>
        public int AllocationFormWidth { get; set; }

        /// <summary>
        /// Gets or sets the automatic grouping rules.
        /// </summary>
        /// <value>
        /// The automatic grouping rules.
        /// </value>
        public AutoGroupingRules AutoGroupingRules
        {
            get { return _autoGroupingRules; }
            set { _autoGroupingRules = value; }
        }

        /// <summary>
        /// Gets or sets the general rules.
        /// </summary>
        /// <value>
        /// The general rules.
        /// </value>
        public GeneralRules GeneralRules
        {
            get { return _generalRules; }
            set { _generalRules = value; }

        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationPreferences"/> class.
        /// </summary>
        public AllocationPreferences()
        {
            _autoGroupingRules = new AutoGroupingRules();
            _generalRules = new GeneralRules();
        }

        #endregion Constructors
    }
}
