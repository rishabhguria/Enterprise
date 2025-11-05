using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using Prana.LogManager;

namespace ExportGridsData
{
    public static class InstanceManager
    {
        public static event Action<Type, object> InstanceCreated;


        private static readonly Dictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static void RegisterInstance<T>(T instance) where T : class
        {
            try
            {
                if (instance == null) throw new ArgumentNullException(nameof(instance));

                var type = typeof(T);

                if (Instances.ContainsKey(type))
                {
                    return;
                }

                Instances[type] = instance;

                InstanceCreated?.Invoke(type, instance);
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
        }

        public static IExportGridData GetInstance<T>(Type additionalType = null) where T : class
        {
            var type = typeof(T);
            try
            {
                foreach (var _instance in Instances.Keys)
                {
                    if (_instance.Name == additionalType.Name)
                    {
                        IExportGridData inst = (IExportGridData)Instances[_instance];
                        return inst;
                    }

                }
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
            return Instances.TryGetValue(type, out var instance) ? instance as IExportGridData : null;
        }

        public static void ReleaseInstance(Type type)
        {
            try
            {
                Instances.Remove(type);
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
        }

        // Overloaded method to release an instance by generic type
        public static void ReleaseInstance<T>() where T : class
        {
            ReleaseInstance(typeof(T));
        }
    }
}
