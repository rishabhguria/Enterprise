using System;
using System.ServiceModel;
using System.Threading;

namespace Prana.WCFConnectionMgr
{
    /// <summary>
    /// Set a new sycnronisation context; restore the old one when disposed.
    /// </summary>
    public class ContextScope : IDisposable
    {
        /// <summary>
        /// The new synchronisation context.
        /// </summary>
        private readonly OperationContextPreservingSynchronizationContext _newContext;

        /// <summary>
        /// The old synchronisation context.
        /// </summary>
        private readonly SynchronizationContext _oldContext;

        /// <summary>
        /// The operation context scope (if any) that was already set for the calling thread when
        /// the scope was created.
        /// </summary>
        private readonly OperationContext _preexistingContext;

        /// <summary>
        /// Have we been disposed?
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Create a new context scope.
        /// </summary>
        /// <param name="newContext">The new context.</param>
        /// <param name="setAsCurrentForCallingThread">
        /// The operation context (if any) to set as the current context for the calling thread. If
        /// <c>null</c> , no operation context will be set for the calling thread.
        /// </param>
        public ContextScope(OperationContextPreservingSynchronizationContext newContext, OperationContext setAsCurrentForCallingThread = null)
        {
            if (newContext == null)
                throw new ArgumentNullException("newContext");
            _newContext = newContext;
            _oldContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(_newContext);
            if (setAsCurrentForCallingThread != null)
            {
                // Save it so we can restore it when we're disposed.
                _preexistingContext = OperationContext.Current;
                // Set-and-forget.
                new OperationContextScope(setAsCurrentForCallingThread);
            }
        }

        /// <summary>
        /// Release the scope.
        /// </summary>
        /// <remarks>
        /// We don't dispose the calling thread's synchronisation scope; we expect that it would
        /// already have gone out of scope due to async / await state machine behaviour.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                SynchronizationContext.SetSynchronizationContext(_oldContext);
                // Restore the existing operation context, if one was present when the scope was created.
                if (_preexistingContext != null)
                    new OperationContextScope(_preexistingContext);
            }
        }
    }
}