using Infragistics.Win;
using Infragistics.Win.UltraWinForm;
using System.Drawing;
using System.Windows.Forms;

namespace Prana
{
    class CF : IUIElementCreationFilter
    {
        private PranaMain _mainForm;

        public CF(PranaMain form)
        {
            this._mainForm = form;
        }

        public void AfterCreateChildElements(UIElement parent)
        {
            if (parent is UltraFormDockAreaUIElement && parent.Control.Dock == DockStyle.Top)
            {
                try
                {

                    ImageUIElement logo = new ImageUIElement(parent, Prana.Properties.Resources.AppIconDefaultON_32.ToBitmap());
                    Rectangle rect0 = parent.Rect;
                    rect0.Size = new Size(22, 22);
                    rect0.Location = new Point(8, 8);
                    logo.Rect = rect0;
                    parent.ChildElements.Add(logo);

                    ButtonUIElement buttonClose = new ButtonUIElement(parent);
                    Rectangle rect1 = parent.Rect;
                    rect1.Size = new Size(22, 22);
                    rect1.Location = new Point(_mainForm.Width - 30, 4);
                    buttonClose.Rect = rect1;
                    Infragistics.Win.Appearance appearanceClose = new Infragistics.Win.Appearance();
                    appearanceClose.ImageBackground = global::Prana.Properties.Resources.Close;
                    appearanceClose.ThemedElementAlpha = Alpha.Transparent;
                    appearanceClose.BackColor = Color.Transparent;
                    appearanceClose.BorderColor = Color.Transparent;
                    appearanceClose.BorderAlpha = Alpha.Transparent;
                    appearanceClose.ImageVAlign = VAlign.Middle;
                    buttonClose.Appearance = appearanceClose;
                    parent.ChildElements.Add(buttonClose);
                    buttonClose.ElementClick += new UIElementEventHandler(buttonClose_ElementClick);

                    ButtonUIElement buttonMinimize = new ButtonUIElement(parent);
                    Rectangle rect2 = parent.Rect;
                    rect2.Size = new Size(22, 22);
                    rect2.Location = new Point(_mainForm.Width - 52, 4);
                    buttonMinimize.Rect = rect2;
                    Infragistics.Win.Appearance appearanceMinimise = new Infragistics.Win.Appearance();
                    appearanceMinimise.ImageBackground = global::Prana.Properties.Resources.Minimise;
                    appearanceMinimise.ThemedElementAlpha = Alpha.Transparent;
                    appearanceMinimise.BackColor = Color.Transparent;
                    appearanceMinimise.BorderColor = Color.Transparent;
                    appearanceMinimise.BorderAlpha = Alpha.Transparent;
                    appearanceMinimise.ImageVAlign = VAlign.Middle;
                    buttonMinimize.Appearance = appearanceMinimise;
                    parent.ChildElements.Add(buttonMinimize);
                    buttonMinimize.ElementClick += new UIElementEventHandler(buttonMinimize_ElementClick);

                    ButtonUIElement buttonUserProfile = new ButtonUIElement(parent);
                    Rectangle rect3 = parent.Rect;
                    rect3.Size = new Size(22, 22);
                    rect3.Location = new Point(_mainForm.Width - 96, 4);
                    buttonUserProfile.Rect = rect3;
                    Infragistics.Win.Appearance appearanceUser = new Infragistics.Win.Appearance();
                    appearanceUser.ImageBackground = global::Prana.Properties.Resources.UserProfile;
                    appearanceUser.ThemedElementAlpha = Alpha.Transparent;
                    appearanceUser.BackColor = Color.Transparent;
                    appearanceUser.BorderColor = Color.Transparent;
                    appearanceUser.BorderAlpha = Alpha.Transparent;
                    appearanceUser.ImageVAlign = VAlign.Middle;
                    buttonUserProfile.Appearance = appearanceUser;
                    parent.ChildElements.Add(buttonUserProfile);
                    buttonUserProfile.ElementClick += new UIElementEventHandler(buttonUserProfile_ElementClick);

                    ButtonUIElement buttonSaveLayout = new ButtonUIElement(parent);
                    Rectangle rect4 = parent.Rect;
                    rect4.Size = new Size(22, 22);
                    rect4.Location = new Point(_mainForm.Width - 74, 4);
                    buttonSaveLayout.Rect = rect4;
                    Infragistics.Win.Appearance appearanceSave = new Infragistics.Win.Appearance();
                    appearanceSave.ImageBackground = global::Prana.Properties.Resources.SaveLayout;
                    appearanceSave.ThemedElementAlpha = Alpha.Transparent;
                    appearanceSave.BackColor = Color.Transparent;
                    appearanceSave.BorderColor = Color.Transparent;
                    appearanceSave.BorderAlpha = Alpha.Transparent;
                    appearanceSave.ImageVAlign = VAlign.Middle;
                    buttonSaveLayout.Appearance = appearanceSave;
                    parent.ChildElements.Add(buttonSaveLayout);
                    buttonSaveLayout.ElementClick += new UIElementEventHandler(buttonSaveLayout_ElementClick);
                }
                catch
                {
                }
            }
        }

        void buttonClose_ElementClick(object sender, UIElementEventArgs e)
        {
            _mainForm.Close();
        }

        void buttonMinimize_ElementClick(object sender, UIElementEventArgs e)
        {
            _mainForm.WindowState = FormWindowState.Minimized;
        }

        void buttonUserProfile_ElementClick(object sender, UIElementEventArgs e)
        {
            _mainForm.LaunchUserProfile();
        }

        void buttonSaveLayout_ElementClick(object sender, UIElementEventArgs e)
        {
            _mainForm.FileLayoutSave_Click();
        }

        public bool BeforeCreateChildElements(UIElement parent)
        {
            return false;
        }
    }
}
