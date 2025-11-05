using PostSharp.Aspects;
using Prana.AuditManager.BLL;
using Prana.AuditManager.Definitions.Interface;
using Prana.Utilities.UI.UIUtilities;
using System;

namespace Prana.AuditManager.Attributes
{
    [Serializable]
    [System.AttributeUsage(System.AttributeTargets.Constructor, AllowMultiple = true, Inherited = true)]
    public class AuditSourceConstAttri : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {

        }

        public override void OnExit(MethodExecutionArgs args)
        {
            if (!CustomThemeHelper.IsDesignMode())
            {
                AuditHandler.GetInstance().Register(args.Instance as IAuditSource);
            }
        }
    }
}