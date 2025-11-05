namespace Prana.Utilities.IO
{
    public interface IInputStream
    {
        int Read();

        int Read(byte[] bytes);

        int Read(byte[] bytes, int offset, int length);

        void Close();
        long available();
        System.IO.Stream getBaseStream();
    }
}
