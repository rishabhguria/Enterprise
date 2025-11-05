using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public abstract class EPnLOrderBase
    {
        public abstract EPnLClassID ClassID
        {
            get;
        }
        public abstract void GetBindableObject(ExposurePnlCacheItem bindableObject);
    }
}
