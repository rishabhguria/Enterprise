using Infragistics.Controls.Editors;
using Prana.Allocation.Client.Constants;
using System.Collections.ObjectModel;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    public class TradeAttributeViewModel : ViewModelBase
    {
        private string _label;
        private string _selectedValue;
        private ObservableCollection<string> _values;
        private bool _isChecked;
        private CustomValueEnteredActions _customValueEnteredAction;
        private bool isEnabled;
        private string _attributeName;
        private string _elementAutomationName;
        private string _cmbAutomationName;

        /// <summary>
        /// This gets/sets label text for Trade Attribute name
        /// </summary>
        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                RaisePropertyChangedEvent("Label");
            }
        }

        /// <summary>
        /// This gets/sets the selected dropdown value
        /// </summary>
        public string SelectedValue
        {
            get
            {
                return _selectedValue;
            }
            set
            {
                _selectedValue = value;
                RaisePropertyChangedEvent("SelectedValue");
            }
        }

        /// <summary>
        /// The values contained in dropdown
        /// </summary>
        public ObservableCollection<string> Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
                RaisePropertyChangedEvent("Values");
            }
        }

        /// <summary>
        /// This specifies whether checkbox is checked or not
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChangedEvent("IsChecked");
            }
        }

        /// <summary>
        /// This gets/sets attributes keep records
        /// </summary>
        public CustomValueEnteredActions CustomValueEnteredAction
        {
            get { return _customValueEnteredAction; }
            set
            {
                _customValueEnteredAction = value;
                RaisePropertyChangedEvent("CustomValueEnteredAction");
            }
        }

        /// <summary>
        /// This specifies whether checkbox is enabled or not
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                RaisePropertyChangedEvent("IsEnabled");
            }
        }

        /// <summary>
        /// This property is used for attribute mapping in json and column major format
        /// </summary>
        public string AttributeName
        {
            get { return _attributeName; }
            set
            {
                _attributeName = value;
            }
        }

        /// <summary>
        /// This property is used for setting Name in automation utility
        /// </summary>
        public string ElementAutomationName
        {
            get { return _elementAutomationName; }
            set
            {
                _elementAutomationName = value;
            }
        }

        /// <summary>
        /// This property is used for setting AutomationId in automation utility
        /// </summary>
        public string CmbAutomationName
        {
            get { return _cmbAutomationName; }
            set
            {
                _cmbAutomationName = value;
            }
        }

        /// <summary>
        /// Gets or sets the attribute number (1-based index).
        /// </summary>
        public int AttributeNumber { get; set; }

        /// <summary>
        /// Gets the zero-based index corresponding to the AttributeNumber.
        /// </summary>
        public int Index
        {
            get
            {
                return AttributeNumber - 1;
            }
        }        
    }
}
