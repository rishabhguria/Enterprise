using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using Prana.ClientCommon;
using Prana.ClientPreferences;
using Prana.LogManager;
using Prana.TradingTicket.Classes;
using System;
using System.Windows.Forms;

namespace Prana.TradingTicket.Controls
{
    /// <summary>
    /// Control For Quick TT preference Tab
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class QuickTTPreference : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickTTPreference"/> class.
        /// </summary>
        public QuickTTPreference()
        {
            InitializeComponent();
            foreColorPickers = new UltraColorPicker[]
            {
                foreColor1,
                foreColor2,
                foreColor3,
                foreColor4,
                foreColor5,
                foreColor6,
                foreColor7,
                foreColor8,
                foreColor9,
                foreColor10
            };
            backColorPickers = new UltraColorPicker[]
            {
                backColor1,
                backColor2,
                backColor3,
                backColor4,
                backColor5,
                backColor6,
                backColor7,
                backColor8,
                backColor9,
                backColor10
            };
            instanceNameEditors = new UltraLabel[]
            {
                txtName1,
                txtName2,
                txtName3,
                txtName4,
                txtName5,
                txtName6,
                txtName7,
                txtName8,
                txtName9,
                txtName10
            };
        }

        /// <summary>
        /// Setups the specified instance count.
        /// </summary>
        /// <param name="instanceCount">The instance count.</param>
        internal void Setup(int instanceCount)
        {
            try
            {
                QuickTTPrefs pref = TradingTktPrefs.QuickTTPrefs;
                for (int i = 0; i < instanceCount; i++)
                {
                    foreColorPickers[i].Visible = true;
                    foreColorPickers[i].Color = pref.InstanceForeColors[i];
                    backColorPickers[i].Visible = true;
                    backColorPickers[i].Color = pref.InstanceBackColors[i];
                    instanceNameEditors[i].Visible = true;
                    instanceNameEditors[i].Text = pref.InstanceNames[i];
                }
                for (int i = instanceCount; i < 10; i++)
                {
                    foreColorPickers[i].Visible = false;
                    backColorPickers[i].Visible = false;
                    instanceNameEditors[i].Visible = false;
                }
                hotQuantityEditor1.Value = pref.HotButtonQuantities[0];
                hotQuantityEditor2.Value = pref.HotButtonQuantities[1];
                hotQuantityEditor3.Value = pref.HotButtonQuantities[2];
                accountChkBox.Checked = pref.UseAccountForLinking;
                venueChkBox.Checked = pref.UseVenueForLinking;
                _instanceCount = instanceCount;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        internal void Save()
        {
            try
            {
                QuickTTPrefs pref = TradingTktPrefs.QuickTTPrefs;
                for (int i = 0; i < _instanceCount; i++)
                {
                    pref.InstanceForeColors[i] = foreColorPickers[i].Color;
                    pref.InstanceBackColors[i] = backColorPickers[i].Color;
                }
                pref.HotButtonQuantities[0] = Convert.ToInt32(hotQuantityEditor1.Value);
                pref.HotButtonQuantities[1] = Convert.ToInt32(hotQuantityEditor2.Value);
                pref.HotButtonQuantities[2] = Convert.ToInt32(hotQuantityEditor3.Value);
                pref.UseAccountForLinking = accountChkBox.Checked;
                pref.UseVenueForLinking = venueChkBox.Checked;
                TradingTktPrefs.QuickTTPrefs = pref;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The instance count
        /// </summary>
        private int _instanceCount;
        /// <summary>
        /// The fore color pickers
        /// </summary>
        private UltraColorPicker[] foreColorPickers;
        /// <summary>
        /// The back color pickers
        /// </summary>
        private UltraColorPicker[] backColorPickers;
        /// <summary>
        /// The instance name editors
        /// </summary>
        private UltraLabel[] instanceNameEditors;

        /// <summary>
        /// Handles the ColorChanged event of the clrPck control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void clrPck_ColorChanged(object sender, System.EventArgs e)
        {
            UltraColorPicker objUltraColorPicker = (UltraColorPicker)sender;

            objUltraColorPicker.Appearance.BackColor = objUltraColorPicker.Color;
            objUltraColorPicker.Appearance.BorderColor = objUltraColorPicker.Color;
            objUltraColorPicker.Appearance.ForeColor = objUltraColorPicker.Color;
        }

        /// <summary>
        /// Handles the ValueChanged event of the hotQtyEditor1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void hotQtyEditor1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                UltraNumericEditor hotQtyEditor = (UltraNumericEditor)sender;
                lblHotQty1.Text = "=  " + QTTHelper.GetLabelFromQuantity((int)hotQtyEditor.Value);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the hotQtyEditor2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void hotQtyEditor2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                UltraNumericEditor hotQtyEditor = (UltraNumericEditor)sender;
                lblHotQty2.Text = "=  " + QTTHelper.GetLabelFromQuantity((int)hotQtyEditor.Value);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Handles the ValueChanged event of the hotQtyEditor3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void hotQtyEditor3_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                UltraNumericEditor hotQtyEditor = (UltraNumericEditor)sender;
                lblHotQty3.Text = "=  " + QTTHelper.GetLabelFromQuantity((int)hotQtyEditor.Value);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
