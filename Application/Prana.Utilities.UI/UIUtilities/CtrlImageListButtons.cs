using System.ComponentModel;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class CtrlImageListButtons : Component
    {
        public CtrlImageListButtons()
        {
            InitializeComponent();
        }

        public CtrlImageListButtons(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public System.Windows.Forms.ImageList GetImageList()
        {
            return imageList1;
        }
        public System.Drawing.Image GetImage(int index)
        {
            return imageList1.Images[index];
        }
    }
}
