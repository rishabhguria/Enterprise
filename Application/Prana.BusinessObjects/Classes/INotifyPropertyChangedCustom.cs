using System.ComponentModel;

namespace Prana.BusinessObjects
{
    public interface INotifyPropertyChangedCustom : INotifyPropertyChanged
    {
        void PropertyHasChanged();
    }
}
