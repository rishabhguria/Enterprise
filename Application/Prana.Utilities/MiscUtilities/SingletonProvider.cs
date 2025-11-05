namespace Prana.Utilities.MiscUtilities
{
    /// <summary>
    /// Provides Singleton Instance on http://www.codeproject.com/csharp/genericsingleton.asp
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonProvider<T> where T : new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonProvider&lt;T&gt;"/> class.
        /// </summary>
        SingletonProvider() { }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get { return SingletonCreator.instance; }
        }

        class SingletonCreator
        {
            /// <summary>
            /// Initializes the <see cref="SingletonCreator"/> class.
            /// </summary>
            static SingletonCreator() { }

            internal static readonly T instance = new T();
        }
    }
}
