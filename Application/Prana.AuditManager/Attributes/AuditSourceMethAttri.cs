using PostSharp.Aspects;
using Prana.AuditManager.BLL;
using Prana.AuditManager.Definitions.Enum;
using Prana.AuditManager.Definitions.Interface;
using System;

namespace Prana.AuditManager.Attributes
{
    public enum AuditMehodType
    {
        ReturnType,
        Arguments
    }

    [Serializable]
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuditSourceMethAttri : OnMethodBoundaryAspect
    {

        AuditMehodType auditType = AuditMehodType.ReturnType;
        int argsIndex = -1;
        AuditAction action = AuditAction.NotDefined;

        public AuditSourceMethAttri(AuditMehodType methodType, int indexOfArgument, AuditAction action)
        {
            this.auditType = methodType;
            this.argsIndex = indexOfArgument;
            this.action = action;
        }


        public override void OnEntry(MethodExecutionArgs args)
        {

        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Object dataToAudit = null;

            switch (auditType)
            {
                case AuditMehodType.ReturnType:
                    dataToAudit = args.ReturnValue;
                    break;
                case AuditMehodType.Arguments:
                    dataToAudit = args.Arguments[argsIndex];
                    break;
            }

            var generator = args.Instance as IAuditSource;


            AuditHandler.GetInstance().AuditDataForGivenInstance(generator, dataToAudit, action);


        }




    }
}
