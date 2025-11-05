using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.SM.OTC;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    public class ShowCustomTemplateViewModel : BindableBase, IDisposable
    {

        /// <summary>
        /// btn Add Additional Fields Command
        /// </summary>
        public DelegateCommand btnAddAdditionalFieldsCommand { get; set; }

        /// <summary>
        /// _customFields
        /// </summary>
        private ObservableCollection<CustomFieldsModel> _customFields = new ObservableCollection<CustomFieldsModel>();

        /// <summary>
        /// Custom Fields
        /// </summary>
        public ObservableCollection<CustomFieldsModel> CustomFields
        {
            get { return _customFields; }
            set
            {
                _customFields = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// ShowCustomTemplateViewModel
        /// </summary>
        public ShowCustomTemplateViewModel()
        {

        }



        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {

                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Set Data for Custom Fields
        /// </summary>
        /// <param name="list"></param>
        internal void SetData(List<OTCCustomFields> list)
        {
            try
            {
                CustomFields = new ObservableCollection<CustomFieldsModel>();
                foreach (var field in list)
                {
                    CustomFieldsModel model = new CustomFieldsModel()
                    {
                        DataType = (DataTypes)Enum.Parse(typeof(DataTypes), field.DataType.ToString()),
                        DefaultValue = field.DefaultValue,
                        DefaultBooleanValue = ((DataTypes)Enum.Parse(typeof(DataTypes), field.DataType.ToString())).Equals(DataTypes.Bool) ? (BooleanValue)Enum.Parse(typeof(BooleanValue), field.DefaultValue.ToString()) : BooleanValue.No,
                        ID = field.ID,
                        InstrumentType = (InstrumentType)Enum.Parse(typeof(InstrumentType), field.InstrumentType.ToString()),
                        Name = field.Name,
                        UIOrder = field.UIOrder
                    };
                    CustomFields.Add(model);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
