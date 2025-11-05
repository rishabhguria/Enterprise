using Prana.BusinessObjects.NewLiveFeed;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.PricingService2UI.EsignalDM
{
    public partial class SubscribedServices : Form
    {
        /// <summary>
        /// The source
        /// </summary>
        BindingSource source;

        /// <summary>
        /// Prevents a default instance of the <see cref="SubscribedServices"/> class from being created.
        /// </summary>
        public SubscribedServices()
        {
            InitializeComponent();
            source = new BindingSource(new BindingList<DMServiceData>(new List<DMServiceData>()), null);
            this.dataGridView.DataSource = source;
        }

        /// <summary>
        /// Updates the source.
        /// </summary>
        /// <param name="services">The services.</param>
        public void updateSource(List<DMServiceData> services)
        {
            try
            {
                foreach (DMServiceData srvc in services)
                {
                    source.Add(srvc);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
