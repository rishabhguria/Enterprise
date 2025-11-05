using System.Windows;

namespace Prana.Rebalancer.Classes
{
    public class DataContextProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new DataContextProxy();
        }

        public object ProxiedObject
        {
            get { return (object)GetValue(ProxiedObjectProperty); }
            set { SetValue(ProxiedObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProxiedObject.    
        //   This enables animation, styling, binding, etc...   
        public static readonly DependencyProperty ProxiedObjectProperty =
            DependencyProperty.Register("ProxiedObject", typeof(object),
                                         typeof(DataContextProxy), null);

    }
}
