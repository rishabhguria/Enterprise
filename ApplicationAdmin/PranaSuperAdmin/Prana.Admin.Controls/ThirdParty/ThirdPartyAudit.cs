using Prana.AuditManager.Definitions.Interface;
using System.Windows.Forms;

namespace Prana.Admin.Controls.ThirdParty
{
    /// <summary>
    /// Summary description for thirdPartyAudit.
    /// </summary>
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.ThirdPartyCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.ThirdPartyUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.ThirdPartyDeleted, ShowAuditUI = true)]
    public partial class ThirdPartyAudit : UserControl, IAuditSource
    {
        /// <summary>
        /// Constructor to initialize Thirdparty Audit
        /// </summary>
        [AuditManager.Attributes.AuditSourceConstAttri]
        public ThirdPartyAudit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to initialize Thirdparty details for audit
        /// </summary>
        /// <param name="thirdPartyID"></param>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void InitializeControl(int thirdPartyID) { }
    }
}
