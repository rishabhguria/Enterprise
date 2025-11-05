using Prana.BusinessObjects;
using System;
using System.Windows;

namespace Prana.SM.OTC
{

    public class CustomFieldsModel : BindableBase
    {

        /// <summary>
        /// ID
        /// </summary>
        private bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// ID
        /// </summary>
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; OnPropertyChanged("ID"); }
        }

        /// <summary>
        /// Name
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        /// <summary>
        /// 
        /// </summary>
        private string defaultValue;
        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; OnPropertyChanged("DefaultValue"); }
        }


        private BooleanValue defaultBooleanValue = BooleanValue.Yes;

        public BooleanValue DefaultBooleanValue
        {
            get { return defaultBooleanValue; }
            set { defaultBooleanValue = value; OnPropertyChanged("DefaultBooleanValue"); }
        }
        /// <summary>
        /// Data Type
        /// </summary>
        private DataTypes dataType = DataTypes.String;
        public DataTypes DataType
        {
            get { return dataType; }
            set
            {
                dataType = value;

                if (dataType == DataTypes.Bool)
                {
                    DefaultValue = DefaultBooleanValue.ToString();
                    IsVisibleText = Visibility.Collapsed;
                    IsVisibleBool = Visibility.Visible;
                    IsVisibleInt = Visibility.Collapsed;
                    IsVisibleDateTime = Visibility.Collapsed;
                }
                else if (dataType == DataTypes.Numeric)
                {
                    DefaultValue = "0.0";
                    IsVisibleText = Visibility.Collapsed;
                    IsVisibleBool = Visibility.Collapsed;
                    IsVisibleInt = Visibility.Visible;
                    IsVisibleDateTime = Visibility.Collapsed;
                }
                else if (dataType == DataTypes.Date)
                {
                    DefaultValue = DateTime.Now.ToString();
                    IsVisibleText = Visibility.Collapsed;
                    IsVisibleBool = Visibility.Collapsed;
                    IsVisibleInt = Visibility.Collapsed;
                    IsVisibleDateTime = Visibility.Visible;
                }
                else if (dataType == DataTypes.String)
                {
                    DefaultValue = "";
                    IsVisibleText = Visibility.Visible;
                    IsVisibleBool = Visibility.Collapsed;
                    IsVisibleInt = Visibility.Collapsed;
                    IsVisibleDateTime = Visibility.Collapsed;
                }
                OnPropertyChanged("DataType");
            }
        }

        /// <summary>
        /// UI Order
        /// </summary>
        private int uIOrder;
        public int UIOrder
        {
            get { return uIOrder; }
            set { uIOrder = value; OnPropertyChanged("UIOrder"); }
        }

        /// <summary>
        /// 
        /// </summary>
        private InstrumentType instrumentType = InstrumentType.EquitySwap;
        public InstrumentType InstrumentType
        {
            get { return instrumentType; }
            set { instrumentType = value; OnPropertyChanged("InstrumentType"); }
        }


        /// <summary>
        /// IsVisibleText
        /// </summary>
        Visibility isVisibleText = Visibility.Visible;
        public Visibility IsVisibleText
        {
            get { return isVisibleText; }
            set { isVisibleText = value; OnPropertyChanged("IsVisibleText"); }
        }

        /// <summary>
        /// IsVisibleCombo
        /// </summary>
        Visibility isVisibleBool = Visibility.Collapsed;
        public Visibility IsVisibleBool
        {
            get { return isVisibleBool; }
            set { isVisibleBool = value; OnPropertyChanged("IsVisibleBool"); }
        }

        /// <summary>
        /// IsVisibleInt
        /// </summary>
        Visibility isVisibleInt = Visibility.Collapsed;
        public Visibility IsVisibleInt
        {
            get { return isVisibleInt; }
            set { isVisibleInt = value; OnPropertyChanged("IsVisibleInt"); }
        }

        /// <summary>
        /// IsVisibleDateTime
        /// </summary>
        Visibility isVisibleDateTime = Visibility.Collapsed;
        public Visibility IsVisibleDateTime
        {
            get { return isVisibleDateTime; }
            set { isVisibleDateTime = value; OnPropertyChanged("IsVisibleDateTime"); }
        }

        public string View { get; set; }
        public string Delete { get; set; }

        bool isControlsEditable = true;
        public bool IsControlsEditable
        {
            get { return isControlsEditable; }
            set { isControlsEditable = value; OnPropertyChanged("IsControlsEditable"); }
        }

    }
}
