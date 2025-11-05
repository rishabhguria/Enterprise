using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;

namespace System
{
    /// <summary>
    /// Object Extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Toes the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static object ToType<T>(this object obj, T type)
        {

            //create instance of T type object:
            var tmp = Activator.CreateInstance(Type.GetType(type.ToString())); 

            //loop through the properties of the object you want to covert:          
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
              try 
              {   
                //get the value of property and try 
                //to assign it to the property of T type object:
                tmp.GetType().GetProperty(pi.Name).SetValue(tmp, 
                                          pi.GetValue(obj, null), null);
              }
              catch { }
             }  
           //return the T type object:         
           return tmp; 
        }

        /// <summary>
        /// returns the size, in bytes of the object.
        /// </summary>
        /// <param name="_Object">The _ object.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Int32 SizeOf(this object _Object)
        {
            try
            {
                // create new memory stream
                using (System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream())
                {
                    // create new BinaryFormatter
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter = new
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    // Serializes an object, or graph of connected objects, to the given stream.
                    _BinaryFormatter.Serialize(_MemoryStream, _Object);
                    // convert stream to byte array and return
                    return _MemoryStream.ToArray().Count();
                }
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

            // Error occured, return null
            return -1;
        }
        /// <summary>
        /// Convert an object to a stream.
        /// </summary>
        /// <param name="_Object">The _ object.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Stream ToStream(this object _Object)
        {
            try
            {
                // create new memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream();

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // Serializes an object, or graph of connected objects, to the given stream.
                _BinaryFormatter.Serialize(_MemoryStream, _Object);

                // convert stream to byte array and return
                return _MemoryStream;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

            // Error occured, return null
            return null;
        }

        /// <summary>
        /// Convert bytes to an object
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static object FromBytes(this MemoryStream stream)
        {
            //SoapFormatter _formatter = new SoapFormatter();
            BinaryFormatter _formatter = new BinaryFormatter();
            return _formatter.Deserialize(stream);
        }
        /// <summary>
        /// Function to get byte array from a object
        /// </summary>
        /// <param name="_Object">object to get byte array</param>
        /// <returns>Byte Array</returns>
        /// <remarks></remarks>
        public static byte[] ToBytes(this object _Object)
        {
            try
            {
                // create new memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream();

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();


                SoapFormatter _soapFormatter = new SoapFormatter();

                _BinaryFormatter.Serialize(_MemoryStream, _Object);

                _MemoryStream.Position = 0;
                object obj = FromBytes(_MemoryStream);

                //  _soapFormatter.Serialize(_MemoryStream, _Object);


                // Serializes an object, or graph of connected objects, to the given stream.
                //  _BinaryFormatter.Serialize(_MemoryStream, _Object);

                // convert stream to byte array and return
                return _MemoryStream.ToArray();
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

            // Error occured, return null
            return null;
        }
        /// <summary>
        /// Function to get byte array from a object
        /// </summary>
        /// <param name="_Object">object to get byte array</param>
        /// <param name="header">The header.</param>
        /// <returns>Byte Array</returns>
        /// <remarks></remarks>
        public static byte[] ToBytes(this object _Object, byte[] header)
        {
            try
            {
                header = new byte[] { 0xFF, 0x00, 0xFE, 0x01, 0xFD, 0x02 };

                // create new memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream();

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();


                _BinaryFormatter.Serialize(_MemoryStream, _Object);

                _MemoryStream.Position = 0;
                object obj = FromBytes(_MemoryStream);

                //  _soapFormatter.Serialize(_MemoryStream, _Object);


                // Serializes an object, or graph of connected objects, to the given stream.
                //  _BinaryFormatter.Serialize(_MemoryStream, _Object);

                // convert stream to byte array and return
                return _MemoryStream.ToArray();
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

            // Error occured, return null
            return null;
        }
        /// <summary>
        /// Decompresses the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] Decompress(this byte[] array, int size)
        {
            using (MemoryStream stream = new MemoryStream(array, 0, size))
            {
                using (GZipStream zstream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    //zstream.Read(array, 0, array.Count());
                    return stream.ToArray();

                }
            }
        }
        /// <summary>
        /// Compresses the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] Compress(this byte[] array)
        {
            int size = array.Count();
            using (MemoryStream stream = new MemoryStream())
            {
                using (GZipStream zstream = new GZipStream(stream, CompressionMode.Compress))
                {
                    zstream.Write(array, 0, size);
                    return stream.ToArray();

                }
            }
        }
        /// <summary>
        /// Compresses the specified _ object.
        /// </summary>
        /// <param name="_Object">The _ object.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] Compress(this object _Object)
        {
            return Compress(_Object.ToBytes());

        }

        /// <summary>
        /// Function to get object from byte array
        /// </summary>
        /// <param name="_ByteArray">byte array to get object</param>
        /// <returns>object</returns>
        /// <remarks></remarks>
        public static object ToObject(this byte[] _ByteArray)
        {
            try
            {
                // convert byte array to memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(_ByteArray);

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // set memory stream position to starting point
                _MemoryStream.Position = 0;

                // Deserializes a stream into an object graph and return as a object.
                return _BinaryFormatter.Deserialize(_MemoryStream);
            }
            catch (Exception _Exception)
            {
                // Error
                System.Diagnostics.Debug.Print("Exception caught in process: {0}\n{1}", _ByteArray.ToString(), _Exception.ToString());
            }

            // Error occured, return null
            return null;
        }

        /// <summary>
        /// Converts a bytearray to a Queue
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Collections.Queue ToQueue(this byte[] array)
        {
            System.Collections.Queue items = new System.Collections.Queue();
            BinaryFormatter bf = new BinaryFormatter();
            using (Stream ms = new MemoryStream(array))
            {
                while (ms.Position < array.Count())
                {
                    object obj = bf.Deserialize(ms);
                    items.Enqueue(obj);
                }
            }
            return items;

        }
        /// <summary>
        /// Convert a byte array to a queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Queue<T> ToQueue<T>(this byte[] array) where T : class, new()
        {
            Queue<T> items = new Queue<T>();
            BinaryFormatter bf = new BinaryFormatter();
            using (Stream ms = new MemoryStream(array))
            {
                while (ms.Position < array.Count())
                {
                    object obj = bf.Deserialize(ms);
                    items.Enqueue((T)obj);
                }
            }
            return items;
        }
        /// <summary>
        /// Convert a Byte Array to a List of Objects. Usally comes from single storage container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<T> ToListT<T>(this byte[] array) where T : class, new()
        {
            List<T> items = new List<T>();

            BinaryFormatter bf = new BinaryFormatter();
            using (Stream ms = new MemoryStream(array))
            {
                while (ms.Position < array.Count())
                {
                    object obj = bf.Deserialize(ms);
                    items.Add((T)obj);
                }
            }
            return items;
        }
        
        /// <summary>
        /// Returns an Object with the specified Type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">An Object that implements the IConvertible interface.</param>
        /// <returns>An object whose Type is conversionType (or conversionType's underlying type if conversionType
        /// is Nullable&lt;&gt;) and whose value is equivalent to value. -or- a null reference, if value is a null
        /// reference and conversionType is not a value type.</returns>
        /// <remarks>This method exists as a workaround to System.Convert.ChangeType(Object, Type) which does not handle
        /// nullables as of version 2.0 (2.0.50727.42) of the .NET Framework. The idea is that this method will
        /// be deleted once Convert.ChangeType is updated in a future version of the .NET Framework to handle
        /// nullable types, so we want this to behave as closely to Convert.ChangeType as possible.
        /// This method was written by Peter Johnson at:
        /// http://aspalliance.com/author.aspx?uId=1026.</remarks>
        public static T ChangeType<T>(object value)
        {
            Type conversionType = typeof(T);
            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null) { return default(T); }
                conversionType = Nullable.GetUnderlyingType(conversionType); ;
            }
            return (T)Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        /// Saves the specified items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool Save<T>(this List<T> items, string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<T>));

                ser.Serialize(fs, items);
                fs.Flush();
                fs.Close();
            }
            return true;
        }
        /// <summary>
        /// Loads the specified items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<T> Load<T>(this List<T> items, string file)
        {
            if (File.Exists(file) == false) return items;

            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<T>));
                items = (List<T>)ser.Deserialize(fs);
            }
            return items;
        }
    
    }
}
