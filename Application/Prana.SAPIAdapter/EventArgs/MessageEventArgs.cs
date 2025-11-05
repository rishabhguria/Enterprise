using System;

namespace Bloomberg.Library
{
    public class MessageEventArgs : EventArgs
    {
        public string Message;
        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
