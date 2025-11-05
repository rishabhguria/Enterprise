using System.Windows;

namespace Prana.CustomChromeLibrary
{
    public class CloseButton : CaptionButton
    {
        static CloseButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseButton), new FrameworkPropertyMetadata(typeof(CloseButton)));
        }

        protected override void OnClick()
        {
            base.OnClick();
            Microsoft.Windows.Shell.SystemCommands.CloseWindow(Window.GetWindow(this));
        }
    }
}
