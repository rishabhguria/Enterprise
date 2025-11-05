using System;

namespace Prana.BusinessObjects.SMObjects
{
    [Serializable]
    public struct StructPricingField
    {
        public int FieldID;
        public string FieldName;
        public bool IsRealTime;
        public bool IsHistorical;
        public string Esignal;
        public string Bloomberg;
    }
}
