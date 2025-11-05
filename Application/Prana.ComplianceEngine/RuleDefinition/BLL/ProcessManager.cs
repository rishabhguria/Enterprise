using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.ComplianceEngine.RuleDefinition.BLL
{
    public delegate void ProcessCompletedHandler<T>(Object sender, ProcessCompletedEventArg<T> args);
    internal class ProcessManager<T>
    {
        Object _lockerObject = new object();
        List<T> _approvedList = new List<T>();
        List<T> _unApprovedList = new List<T>();
        List<T> _failedList = new List<T>();
        Boolean _isProcessComplete = true;
        String _processTag = String.Empty;
        StringBuilder messageBuilder = new StringBuilder();
        internal event ProcessCompletedHandler<T> ProcessComplete;

        internal ProcessManager()
        {
        }

        /// <summary>
        /// Returns if any process in on going or completed.
        /// </summary>
        /// <returns></returns>
        internal Boolean IsProcessComplete()
        {
            try
            {
                lock (_lockerObject)
                {
                    return _isProcessComplete;
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
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void ResetProcess()
        {
            try
            {
                lock (_lockerObject)
                {
                    this._unApprovedList
                         .Clear();
                    this._approvedList.Clear();
                    this._failedList.Clear();
                    this._isProcessComplete = true;
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
        }


        /// <summary>
        /// Sets Process queue
        /// clears all list and assigns object to unapproved lists.
        /// ProcessTag- Operation to be performed.
        /// and starts process.
        /// </summary>
        /// <param name="unapprovedList"></param>
        /// <param name="processTag"></param>
        internal void SetProcessQueue(List<T> unapprovedList, String processTag)
        {
            try
            {
                lock (_lockerObject)
                {
                    if (_isProcessComplete)
                    {
                        this._approvedList.Clear();
                        this._failedList.Clear();
                        this._unApprovedList = unapprovedList;
                        this._processTag = processTag;
                        this.messageBuilder.Clear();
                        this.messageBuilder.AppendLine(processTag + " operation failed for these rules:");
                        this._isProcessComplete = false;
                    }
                    else
                        throw new Exception("Previous process not completed");
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
        }

        /// <summary>
        /// Adds object to approved list
        /// and raises event if unapproved list is empty
        /// </summary>
        /// <param name="obj"></param>
        internal void SetApproved(T obj)
        {
            try
            {
                Approve(obj);
                RaiseEventIfRequired();
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

        /// <summary>
        /// Adds object to approved list 
        /// </summary>
        /// <param name="obj"></param>
        private void Approve(T obj)
        {
            try
            {
                lock (_lockerObject)
                {
                    T tempObject = default(T);

                    foreach (T ob in _unApprovedList)
                    {
                        if (ob.Equals(obj))
                        {
                            tempObject = obj;
                            break;
                        }
                    }

                    if (!tempObject.Equals(default(T)))
                    {
                        _approvedList.Add(tempObject);
                        _unApprovedList.Remove(tempObject);
                    }
                    else
                        throw new Exception("Object does not exist in unapproved list");
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
        }

        /// <summary>
        /// Raises process complete event if unapproved list is empty.
        /// </summary>
        private void RaiseEventIfRequired()
        {
            try
            {
                lock (_lockerObject)
                {
                    if (_unApprovedList.Count == 0)
                    {
                        if (ProcessComplete != null)
                        {
                            ProcessCompletedEventArg<T> eventArg = new ProcessCompletedEventArg<T>();
                            //eventArg.FailedObjects = DeepCopyHelper.Clone(_failedList);
                            eventArg.FailedObjects = new List<T>(this._failedList);
                            eventArg.ApprovedObjects = new List<T>(this._approvedList);
                            eventArg.ProcessTag = this._processTag;
                            eventArg.Message = messageBuilder.ToString();
                            //eventArg.CompletedMessage = "";
                            _unApprovedList.Clear();
                            _approvedList.Clear();
                            _failedList.Clear();
                            _isProcessComplete = true;
                            ProcessComplete(this, eventArg);
                        }
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

        }

        /// <summary>
        /// Adds object to faled list
        /// Raises process complete event if unapproved list is empty.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="failedMessage"></param>
        internal void SetFailed(T obj, String failedMessage)
        {
            try
            {
                Fail(obj, failedMessage);
                RaiseEventIfRequired();
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

        /// <summary>
        /// Adds object to faled list
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="failedMessage"></param>
        private void Fail(T obj, String failedMessage)
        {
            try
            {
                lock (_lockerObject)
                {
                    T tempObject = default(T);

                    foreach (T ob in _unApprovedList)
                    {
                        if (ob.Equals(obj))
                        {
                            tempObject = obj;
                            break;
                        }
                    }

                    if (!tempObject.Equals(default(T)))
                    {
                        _failedList.Add(tempObject);
                        _unApprovedList.Remove(tempObject);
                        messageBuilder.AppendLine(failedMessage);
                    }
                    else
                        throw new Exception("Object does not exist in unapproved list");
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
        }

        /// <summary>
        /// returns object in unapproved list for the object in parameter.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal T GetUnApprovedObj(T obj)
        {
            try
            {
                lock (_lockerObject)
                {
                    foreach (T ob in _unApprovedList)
                    {
                        if (ob.Equals(obj))
                        {
                            return ob;
                        }
                    }
                    return default(T);
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
                return default(T);
            }
        }

        /// <summary>
        /// Checks if object exists in Unapproved list or not.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool ExistsInUnapproved(T obj)
        {
            try
            {
                lock (_lockerObject)
                {
                    foreach (T ob in _unApprovedList)
                    {
                        if (ob.Equals(obj))
                        {
                            return true;
                        }
                    }
                    return false;
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
                return false;

            }
        }
    }
}
