namespace Prana.BusinessObjects.Classes.Allocation
{
    public enum FixedPreferenceCreationSource
    {
        /// <summary>
        /// Creation Source for Fixed Preference updation
        /// </summary>
        None = 0,

        /// <summary>
        /// Creation Source for scheme import
        /// </summary>
        SchemeImport = 1,

        /// <summary>
        ///  Creation Source for Dynamic fixed Preference created by staged order import 
        /// </summary>
        StagedOrderImport = 2,

        /// <summary>
        ///  Creation Source for prorata UI
        /// </summary>
        ProrataUI = 3


    }
}
