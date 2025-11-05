using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.IO;
using System.Reflection;

namespace Nirvana.TestAutomation.Factory
{
    public static class TestStepFactory
    {
        private const string AssemblyParentNamePrefix = "Nirvana.TestAutomation.";
        private const string AssemblyNamePrefix = "Nirvana.TestAutomation.Steps.";
        public static IOpenFinTestStep GetOpenFinStep(string applicationStartUpPath, string moduleName, string stepName)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(applicationStartUpPath + @"\TestAutomation.Steps\" + AssemblyNamePrefix + moduleName + ".dll");
                if (assembly == null)
                {
                    throw new NullReferenceException("Assembly not present in directory.");
                }

                Type type = assembly.GetType(AssemblyNamePrefix + moduleName + "." + stepName);
                if (type == null)
                {
                    throw new NullReferenceException("NamespaceName or Class not present in assembly.");
                }

                var methodInfo = type.GetMethod("RunOpenFinTest");
                if (methodInfo == null)
                {
                    throw new NullReferenceException("Method not present in class.");
                }

                return (IOpenFinTestStep)Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }


        public static ITestStep GetStep(string applicationStartUpPath, string moduleName, string stepName)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(applicationStartUpPath + @"\TestAutomation.Steps\" + AssemblyNamePrefix + moduleName + ".dll");
                if (assembly == null)
                {
                    throw new NullReferenceException("Assembly not present in directory.");
                }

                Type type = assembly.GetType(AssemblyNamePrefix + moduleName + "." + stepName);
                if (type == null)
                {
                    throw new NullReferenceException("NamespaceName or Class not present in assembly.");
                }

                var methodInfo = type.GetMethod("RunTest");
                if (methodInfo == null)
                {
                    throw new NullReferenceException("Method not present in class.");
                }

                return (ITestStep)Activator.CreateInstance(type);
            }
            catch
            {
                throw;
            }
        }
        public static StepWrapper GetStepWrapper(string applicationStartUpPath, string moduleName, string stepName) 
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(applicationStartUpPath + @"\TestAutomation.Steps\" + AssemblyNamePrefix + moduleName + ".dll");
                if (assembly == null)
                {
                    throw new NullReferenceException("Assembly not present in directory.");
                }

                Type type = assembly.GetType(AssemblyNamePrefix + moduleName + "." + stepName);
                if (type == null)
                {
                    throw new NullReferenceException("NamespaceName or Class not present in assembly.");
                }

                MethodInfo runTestMethod = type.GetMethod("RunTest");
                MethodInfo runUIAutomationTestMethod = type.GetMethod("RunUIAutomationTest");

                if (runTestMethod == null && runUIAutomationTestMethod == null)
                {
                    throw new MissingMethodException("Neither RunTest nor RunUIAutomationTest method found in the specified class.");
                }
                object instance = Activator.CreateInstance(type);

                return new StepWrapper(instance, runTestMethod, runUIAutomationTestMethod);
            }
            catch (Exception ex)
            {
                StepWrapper secStepWrapper = null;
                try
                {
                    secStepWrapper = TestStepFactory.GetDataModeStepWrapper(ApplicationArguments.ApplicationStartUpPath, ApplicationArguments.NirvanaUIAutomationModuleName, stepName);

                }
                catch
                {
                    Console.WriteLine("Exception thrown at TestStepFactory: GetStepWrapper-" + ex.Message);
                    throw;
                }
                if (secStepWrapper == null)
                {
                    Console.WriteLine("Exception thrown at TestStepFactory: GetStepWrapper-" + ex.Message);
                    throw;
                }
                ApplicationArguments.UIAutomationRunDataStep = true;
                return secStepWrapper;
               
                
            }
            
        }
        public static StepWrapper GetDataModeStepWrapper(string applicationStartUpPath, string moduleName, string stepName)
        {
            try
            {
                string assemblyPath = Path.Combine(applicationStartUpPath, AssemblyParentNamePrefix + moduleName + ".dll");
                Assembly assembly = Assembly.LoadFrom(assemblyPath);

                if (assembly == null)
                {
                    throw new NullReferenceException("Assembly not present in directory.");
                }

                Type type = assembly.GetType(AssemblyParentNamePrefix + moduleName + "." + ApplicationArguments.UIAutomationClass);
                if (type == null)
                {
                    throw new NullReferenceException("NamespaceName or Class not present in assembly.");
                }
                MethodInfo runUIAutomationTestMethod = type.GetMethod("RunUIAutomationTest");

                if (runUIAutomationTestMethod == null)
                {
                    throw new MissingMethodException(" RunUIAutomationTest method not found in the specified class.");
                }
                object instance = Activator.CreateInstance(type);
                
                MethodInfo validateMethod = type.GetMethod("ValidateAndGetActiveStepDataTable");
                object[] parameters = new object[] { null };
                
                bool isValid = (bool)validateMethod.Invoke(instance, parameters);
                
                if (!isValid)
                {
                    return null;
                }
                return new StepWrapper(instance,null,runUIAutomationTestMethod);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown at GetDataModeStepWrapper: GetStepWrapper-" + ex.Message);
                throw;
            }

        }
        public static StepWrapper GetMethodDynamically(string applicationStartUpPath, string moduleName, string stepName )
        {
            try
            {
                string assemblyPath = Path.Combine(applicationStartUpPath, AssemblyParentNamePrefix + moduleName + ".dll");
                Assembly assembly = Assembly.LoadFrom(assemblyPath);

                if (assembly == null)
                {
                    throw new NullReferenceException("Assembly not present in directory.");
                }

                Type type = assembly.GetType(AssemblyParentNamePrefix + moduleName + "." + ApplicationArguments.UIAutomationHelperClass);
                if (type == null)
                {
                    throw new NullReferenceException("NamespaceName or Class not present in assembly.");
                }
                MethodInfo runUIAutomationTestMethod = type.GetMethod(stepName);

                if (runUIAutomationTestMethod == null)
                {
                    throw new MissingMethodException(stepName +"  method not found in the specified class.");
                }
                object instance = Activator.CreateInstance(type);

                return new StepWrapper(instance, null, runUIAutomationTestMethod);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown at GetDataModeStepWrapper: GetStepWrapper-" + ex.Message);
                throw;
            }

        }
    }

    public class StepWrapper
    {
        public object Instance { get; private set; }
        public MethodInfo RunTestMethod { get; private set; }
        public MethodInfo RunUIAutomationTestMethod { get; private set; }

        public StepWrapper(object instance, MethodInfo runTestMethod, MethodInfo runUIAutomationTestMethod)
        {
            Instance = instance;
            RunTestMethod = runTestMethod;
            RunUIAutomationTestMethod = runUIAutomationTestMethod;
        }

        public bool CanRunTest
        {
            get { return RunTestMethod != null; }
        }

        public bool CanRunUIAutomationTest
        {
            get { return RunUIAutomationTestMethod != null; }
        }
    }


}