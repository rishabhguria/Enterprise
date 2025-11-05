using Prana.BusinessObjects.FIX;
using System;

namespace Prana.Fix.FixDictionary
{
    [Serializable]
    public abstract class FieldValidationSubRule
    {
        public abstract bool CheckIfRequiredField(PranaMessage pranaMessage);
    }
}
