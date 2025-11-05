using Infragistics.Win;
using Infragistics.Win.UltraWinForm;
using Prana.LogManager;
using Prana.TradingTicket.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.TradingTicket
{
    class QTTCreationFilter : IUIElementCreationFilter
    {
        private QuickTradingTicket _parentForm;

        public QTTCreationFilter(QuickTradingTicket form)
        {
            this._parentForm = form;
        }

        public void AfterCreateChildElements(UIElement parent)
        {
            if (parent is UltraFormDockAreaUIElement && parent.Control.Dock == DockStyle.Top)
            {
                try
                {
                    ButtonUIElement buttonLinking = new ButtonUIElement(parent);
                    Rectangle rect3 = parent.Rect;
                    rect3.Size = new Size(22, 22);
                    rect3.Location = new Point(_parentForm.Width - 118, 8);
                    buttonLinking.Rect = rect3;
                    Infragistics.Win.Appearance appearanceLink = new Infragistics.Win.Appearance();
                    appearanceLink.ImageBackground = global::Prana.TradingTicket.Properties.Resources.Link;
                    appearanceLink.ThemedElementAlpha = Alpha.Transparent;
                    appearanceLink.BackColor = Color.Transparent;
                    appearanceLink.BorderColor = Color.Transparent;
                    appearanceLink.BorderAlpha = Alpha.Transparent;
                    appearanceLink.ImageVAlign = VAlign.Middle;
                    buttonLinking.Appearance = appearanceLink;
                    parent.ChildElements.Add(buttonLinking);
                    buttonLinking.ElementClick += new UIElementEventHandler(buttonLinkBlotter_ElementClick);

                    ButtonUIElement buttonSaveLayout = new ButtonUIElement(parent);
                    Rectangle rect4 = parent.Rect;
                    rect4.Size = new Size(22, 22);
                    rect4.Location = new Point(_parentForm.Width - 96, 8);
                    buttonSaveLayout.Rect = rect4;
                    Infragistics.Win.Appearance appearanceSave = new Infragistics.Win.Appearance();
                    appearanceSave.ImageBackground = global::Prana.TradingTicket.Properties.Resources.SaveLayout;
                    appearanceSave.ThemedElementAlpha = Alpha.Transparent;
                    appearanceSave.BackColor = Color.Transparent;
                    appearanceSave.BorderColor = Color.Transparent;
                    appearanceSave.BorderAlpha = Alpha.Transparent;
                    appearanceSave.ImageVAlign = VAlign.Middle;
                    buttonSaveLayout.Appearance = appearanceSave;
                    parent.ChildElements.Add(buttonSaveLayout);
                    buttonSaveLayout.ElementClick += new UIElementEventHandler(buttonSaveLayout_ElementClick);
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
            }
        }

        void buttonLinkBlotter_ElementClick(object sender, UIElementEventArgs e)
        {
            Image img = global::Prana.TradingTicket.Properties.Resources.Link;
            ((ButtonUIElement)e.Element).Appearance.ImageBackground = img;
            _parentForm.ToggleBlotterLinking();
        }

        void buttonSaveLayout_ElementClick(object sender, UIElementEventArgs e)
        {
            try
            {
                _parentForm.SaveFiledPref();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public bool BeforeCreateChildElements(UIElement parent)
        {
            return false;
        }
    }
}
