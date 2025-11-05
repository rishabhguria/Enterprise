using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.FIX
{
    [Serializable]
    public class RepeatingGroup 
    {
        public RepeatingGroup() {}
       
        public RepeatingGroup(MessageField countField)
        {
            CountField = countField;
        }
       
        public MessageField CountField { get; set; }
        public List<RepeatingMessageFieldCollection> MessageFields { get; set; } = new List<RepeatingMessageFieldCollection>();
        public SerializableDictionary<string, SerializableDictionary<string, RepeatingGroup>> ChildGroups { get; set; } = new SerializableDictionary<string, SerializableDictionary<string, RepeatingGroup>>();
    }
}
