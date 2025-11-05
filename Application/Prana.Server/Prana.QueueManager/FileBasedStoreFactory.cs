using System;

namespace Prana.QueueManager
{
    public class FileBasedStoreFactory
    {
        public static FileBasedStore CreateFileStore(string path, Int64 lastSeqNumber)
        {
            FileBasedStore store = new FileBasedStore(path, lastSeqNumber);
            return store;

        }
    }
}
