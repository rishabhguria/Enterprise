using Prana.AuditManager.Definitions.Interface;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.AUECCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.AUECUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.AUECDeleted, ShowAuditUI = true)]
    public partial class AUECAudit : UserControl, IAuditSource
    {
        /// <summary>
        /// Constructor to initialize AUEC Audit
        /// </summary>
        [AuditManager.Attributes.AuditSourceConstAttri]
        public AUECAudit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to initialize AUEC details for audit
        /// </summary>
        /// <param name="AUECID"></param>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void InitializeControl(int AUECID) { }
    }
}
