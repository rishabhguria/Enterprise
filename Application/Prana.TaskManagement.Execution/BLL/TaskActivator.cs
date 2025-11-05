using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using System;
using System.Reflection;

namespace Prana.TaskManagement.Execution
{
    public class TaskActivator
    {

        #region Singleton Instance
        //Singleton is created as property see http://www.dotnetperls.com/singleton

        private static readonly TaskActivator _instance = new TaskActivator();
        internal static TaskActivator Instance
        {
            get
            {
                return _instance;
            }
        }
        private TaskActivator()
        {

        }

        #endregion



        #region ITaskActivator Members

        public NirvanaTask CreateTaskInstanceFor(TaskInfo taskInfo)
        {
            NirvanaTask task = null;

            try
            {
                Assembly asmAssemblyContainingForm = Assembly.LoadFrom(taskInfo.AssemblyName);
                Type typeToLoad = asmAssemblyContainingForm.GetType(taskInfo.QClassName);
                task = (NirvanaTask)Activator.CreateInstance(typeToLoad);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return task;
        }

        #endregion
    }
}
