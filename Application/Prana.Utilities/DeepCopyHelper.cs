using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Nirvana.Utilities
{
    /// <summary>
    /// Added Rajat 21 Aug 2006
    /// Warning : This class functionality is slow as it uses reflection 
    /// http://dotnetjunkies.com/WebLog/anoras/archive/2005/11/28/134032.aspx
    /// This class helps to create a deep copy of any object, so now one don't has to 
    /// apply the deep copy mechanism at each point of object graph. Rather than this,
    /// user can simply use this class and get deep copies.
    /// USAGE ............
    /// [Serializable]
    //  public class MyClass : ICloneable
    //  {
    //    public object Clone()
    //    {
    //        return DeepCopyHelper.Clone(this);
    //    }
    //  }
    /// </summary>
    
    public class DeepCopyHelper
    {

        /// <summary>
        /// TODO : Ideally every object should derive from IClonable and should have clone method
        /// In this way deep copying will be faster.
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="source">Object reference of which we need to make the clone</param>
        /// <returns>Returns the deep copy of the passed object using reflection</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

    }
}
