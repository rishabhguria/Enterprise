using Prana.BusinessObjects.AppConstants;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    public class CustomCondition : INotifyPropertyChanged
    {

        public string compareValue;
        private string columnName = "-Select-";
        public bool isComplement;
        public bool isDragging;



        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        public string ColumnName
        {
            get
            {
                return this.columnName;
            }
            set
            {
                if (this.columnName == value || this.columnName != null && this.columnName.Equals(value))
                    return;
                this.columnName = value;
                this.NotifyPropertyChanged("ColumnName");
            }
        }


        private EnumDescriptionAttribute.ConditionOperator _conditionOperatorType;

        public EnumDescriptionAttribute.ConditionOperator ConditionOperatorType
        {
            get { return _conditionOperatorType; }
            set
            {
                _conditionOperatorType = value;
                this.NotifyPropertyChanged("ConditionOperatorType");
            }
        }



        public string CompareValue
        {
            get
            {
                return this.compareValue;
            }
            set
            {
                //if (this.compareValue == value || this.compareValue != null && this.compareValue.Equals(value))
                //    return;
                this.compareValue = value;
                this.NotifyPropertyChanged("CompareValue");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged == null)
                return;
            this.PropertyChanged((object)this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }





    }
}
