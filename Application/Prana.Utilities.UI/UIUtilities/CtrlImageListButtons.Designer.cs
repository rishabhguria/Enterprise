namespace Prana.Utilities.UI.UIUtilities
{
    partial class CtrlImageListButtons
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlImageListButtons));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "HotTrackedGreen.png");
            this.imageList1.Images.SetKeyName(1, "HotTrackedNeutral.png");
            this.imageList1.Images.SetKeyName(2, "HotTrackedRed.png");
            this.imageList1.Images.SetKeyName(3, "NormalGreen.png");
            this.imageList1.Images.SetKeyName(4, "NormalNeutral.png");
            this.imageList1.Images.SetKeyName(5, "NormalRed.png");

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
    }
}
