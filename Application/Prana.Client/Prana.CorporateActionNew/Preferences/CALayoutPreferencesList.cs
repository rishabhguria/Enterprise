using Prana.BusinessObjects;
using System;
using System.Xml.Serialization;

namespace Prana.CorporateActionNew.Forms
{
    //[XmlRoot("CAApplyLayoutPreferencesList")]
    //public class CAApplyLayoutPreferencesList : SerializableDictionary<string, CAApplyLayoutPreferences>
    //{
    //}
    //[XmlRoot("CARedoLayoutPreferencesList")]
    //public class CARedoLayoutPreferencesList : SerializableDictionary<string, CARedoLayoutPreferences>
    //{
    //}
    //[XmlRoot("CAUndoLayoutPreferencesList")]
    //public class CAUndoLayoutPreferencesList : SerializableDictionary<string, CAApplyLayoutPreferences>
    //{
    //}

    [XmlRoot("CALayoutPreferences")]
    [Serializable]
    public class CALayoutPreferencesList
    {
        public int CounterPartyApply = int.MinValue;

        public int CounterPartyUnApplied = int.MinValue;

        [XmlElement("CAApplyLayoutPreferencesList")]
        public SerializableDictionary<string, CAApplyLayoutPreferences> CAApplyLayoutPreferencesList = new SerializableDictionary<string, CAApplyLayoutPreferences>();

        [XmlElement("CARedoLayoutPreferencesList")]
        public SerializableDictionary<string, CARedoLayoutPreferences> CARedoLayoutPreferencesList = new SerializableDictionary<string, CARedoLayoutPreferences>();

        [XmlElement("CAUndoLayoutPreferencesList")]
        public SerializableDictionary<string, CAUndoLayoutPreferences> CAUndoLayoutPreferencesList = new SerializableDictionary<string, CAUndoLayoutPreferences>();

    }
}
