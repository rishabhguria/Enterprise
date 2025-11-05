using Prana.BusinessObjects.PositionManagement;
using System.Windows.Forms;
namespace Prana.PM.Admin.UI.Forms
{
    public partial class MapAUEC : Form
    {
        public MapAUEC()
        {
            InitializeComponent();
        }

        public MapAUEC(ThirdPartyNameID dataSourceNameID)
        {
            InitializeComponent();
            ctrlMapAUEC1.InitControl(dataSourceNameID);
        }
    }
}