//using ScriptCoreLib;



namespace Prana.Utilities.IO
{
    // http://www.j2medev.com/api/midp/java/io/DataInput.html
    /// <summary>
    /// The <code>DataInput</code> interface provides
    /// for reading bytes from a binary stream and
    /// reconstructing from them data in any of
    /// the Java primitive types. There is also
    /// a
    /// facility for reconstructing a <code>String</code>
    /// from data in Java modified UTF-8 format.
    /// </summary>
    //[Script(IsNative=true, ExternalTarget="DataInput")]
    public interface IDataInput
    {
        #region nested types


        #endregion

        #region properties


        #endregion

        #region methods

        string readLine();

        /// <summary>
        /// Reads four input bytes and returns an
        /// <code>int</code> value.
        /// </summary>
        int readInt();

        /// <summary>
        /// Reads and returns one input byte.
        /// </summary>
        sbyte readByte();

        /// <summary>
        /// Reads eight input bytes and returns
        /// a <code>long</code> value.
        /// </summary>
        long readLong();

        /// <summary>
        /// Reads two input bytes and returns
        /// a <code>short</code> value.
        /// </summary>
        short readShort();

        /// <summary>
        /// Reads in a string that has been encoded using a modified UTF-8 format.
        /// </summary>
        string readUTF();

        /// <summary>
        /// Reads an input <code>char</code> and returns the <code>char</code> value.
        /// </summary>
        char readChar();

        float readFloat();

        void readFully(params sbyte[] arg0);

        void readFully(sbyte[] arg0, int arg1, int arg2);

        /// <summary>
        /// Reads one input byte and returns
        /// <code>true</code> if that byte is nonzero,
        /// <code>false</code> if that byte is zero.
        /// </summary>
        bool readBoolean();

        double readDouble();

        /// <summary>
        /// Reads one input byte, zero-extends
        /// it to type <code>int</code>, and returns
        /// the result, which is therefore in the range
        /// <code>0</code>
        /// through <code>255</code>.
        /// </summary>
        int readUnsignedByte();

        /// <summary>
        /// Reads two input bytes and returns
        /// an <code>int</code> value in the range <code>0</code>
        /// through <code>65535</code>.
        /// </summary>
        int readUnsignedShort();

        /// <summary>
        /// Makes an attempt to skip over
        /// <code>n</code> bytes
        /// of data from the input
        /// stream, discarding the skipped bytes.
        /// </summary>
        int skipBytes(int @n);


        #endregion

    }
}
