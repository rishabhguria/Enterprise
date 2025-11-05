using Prana.AuditManager.Definitions.Interface;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    /// <summary>
    /// Summary description for ClientAudit.
    /// </summary>
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.ClientCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.ClientUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.ClientDeleted, ShowAuditUI = true)]
    public partial class ClientAudit : UserControl, IAuditSource
    {
        /// <summary>
        /// Constructor to initialize Client Audit
        /// </summary>
        [AuditManager.Attributes.AuditSourceConstAttri]
        public ClientAudit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to initialize client details for audit
        /// </summary>
        /// <param name="userID"></param>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void InitializeControl(int clientID) { }
    }
}
