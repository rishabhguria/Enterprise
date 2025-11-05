namespace Prana.Utilities.IO
{
    public interface IOutputStream
    {
        void Write(int i);

        void Write(byte[] bytes);

        void Write(byte[] bytes, int offset, int length);

        void Flush();

        void Close();
        System.IO.Stream getBaseStream();
    }
}