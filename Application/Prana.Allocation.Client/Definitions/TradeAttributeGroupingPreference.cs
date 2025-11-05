namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class TradeAttributeGroupingPreference
    {
        /// <summary>
        /// The internal name or identifier of the trade attribute preference.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The display label used for the trade attribute preference in the UI.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Indicates whether this trade attribute preference is selected for grouping.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// The automation name used for the checkbox element in UI testing or automation tools.
        /// </summary>
        public string ChkBoxAutomationName { get; set; }

        /// <summary>
        /// The automation name used for the label element in UI testing or automation tools.
        /// </summary>
        public string LblAutomationName { get; set; }
    }
}
