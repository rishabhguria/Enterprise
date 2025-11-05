using PostSharp.Aspects;
using Prana.AuditManager.BLL;
using Prana.AuditManager.Definitions.Interface;
using System;

namespace Prana.AuditManager.Attributes
{
    [Serializable]
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuditRefreshMethAttri : OnMethodBoundaryAspect
    {
        AuditMehodType auditType = AuditMehodType.ReturnType;
        int argsIndex = -1;

        public AuditRefreshMethAttri(AuditMehodType methodType, int indexOfArgument)
        {
            this.auditType = methodType;
            this.argsIndex = indexOfArgument;
        }


        public override void OnEntry(MethodExecutionArgs args)
        {

        }

        public override void OnExit(MethodExecutionArgs args)
        {
            int dataToAudit = -1;

            switch (auditType)
            {
                case AuditMehodType.ReturnType:
                    dataToAudit = Convert.ToInt32(args.ReturnValue);
                    break;
                case AuditMehodType.Arguments:
                    dataToAudit = Convert.ToInt32(args.Arguments[argsIndex]);
                    break;
            }

            var generator = args.Instance as IAuditSource;


            AuditHandler.GetInstance().RefreshAuditDataForGivenInstance(generator, dataToAudit);


        }

    }
}