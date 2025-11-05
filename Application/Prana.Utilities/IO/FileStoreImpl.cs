using Prana.LogManager;
using System;
using System.Collections.Generic;

//using Wintellect.PowerCollections;
//using C5;

namespace Prana.Utilities.IO
{
    public class FileStoreImpl : IDisposable
    {
        private static String READ_OPTION = "r";
        private static String WRITE_OPTION = "w";
        private static String SYNC_OPTION = "d";
        private static String NOSYNC_OPTION = "";

        private C5.TreeDictionary<long, long[]> messageIndex;
        private MemoryCacheImpl cache = new MemoryCacheImpl();

        private String msgFileName;
        private String headerFileName;
        private String seqNumFileName;
        // private String sessionFileName;
        private bool syncWrites;
        private int maxCachedMsgs;

        private RandomAccessFile messageFileReader;
        private RandomAccessFile messageFileWriter;
        private DataOutputStream headerDataOutputStream;
        private FileOutputStream headerFileOutputStream;
        private RandomAccessFile sequenceNumberFile;
        public FileStoreImpl(String path, Session session, bool syncWrites, int maxCachedMsgs)
        {

            this.syncWrites = syncWrites;
            this.maxCachedMsgs = maxCachedMsgs;

            if (maxCachedMsgs > 0)
            {
                messageIndex = new C5.TreeDictionary<long, long[]>();
            }
            else
            {
                messageIndex = null;
            }
            string suffix = ".log";
            //String fullPath = new File(path == null ? "." : path).getAbsolutePath();
            //String sessionName = "Session" ;
            //String prefix = FileUtil.fileAppendPath(fullPath, sessionName + ".");
            String prefix = path + session.SessionID;
            msgFileName = prefix + "_body" + suffix;
            headerFileName = prefix + "_header" + suffix;
            seqNumFileName = prefix + "_seqnums" + suffix;
            // sessionFileName = prefix + "_session";

            File directory = new File(path);//.getParentFile();
            if (!directory.Exists())
            {
                directory.Mkdirs();
            }

            initialize(false);
        }

        void initialize(bool deleteFiles)
        {
            closeFiles();

            if (deleteFiles)
            {
                DeleteFiles();
            }

            messageFileWriter = new RandomAccessFile(msgFileName, getRandomAccessFileOptions());
            messageFileReader = new RandomAccessFile(msgFileName, READ_OPTION);
            sequenceNumberFile = new RandomAccessFile(seqNumFileName, getRandomAccessFileOptions());

            initializeCache();
        }

        private void initializeCache()
        {
            cache.reset();
            initializeMessageIndex();
            initializeSequenceNumbers();
            //initializeSessionCreateTime();
            messageFileWriter.Seek(messageFileWriter.Length());
        }

        private void initializeSessionCreateTime()
        {
            //File sessionTimeFile = new File(sessionFileName);
            //if (sessionTimeFile.Exists())
            //{
            //    DataInputStream sessionTimeInput = new DataInputStream(
            //           new FileInputStream(sessionTimeFile));
            //    try
            //    {

            //        cache.setCreationTime(DateTime.UtcNow);
            //    }
            //    catch (Exception e)
            //    {
            //        throw new System.IO.IOException(e.Message);
            //    }
            //    finally
            //    {
            //        sessionTimeInput.Close();
            //    }
            //}
            //else
            //{
            //    storeSessionTimeStamp();
            //}
        }

        private void storeSessionTimeStamp()
        {
            //DataOutputStream sessionTimeOutput = new DataOutputStream(new FileOutputStream(sessionFileName,false ));

            //try
            //{
            //    DateTime date = DateTime.UtcNow;
            //    cache.setCreationTime(date);
            //    sessionTimeOutput.writeUTF(date.ToString());
            //}
            //finally
            //{
            //    sessionTimeOutput.Close();
            //}
        }

        /* (non-Javadoc)
         * @see quickfix.MessageStore#getCreationTime()
         */
        public DateTime getCreationTime()
        {
            return cache.getCreationTime();
        }

        private void initializeSequenceNumbers()
        {
            sequenceNumberFile.Seek(0);
            if (sequenceNumberFile.Length() > 0)
            {
                String s = sequenceNumberFile.readUTF();
                int offset = s.IndexOf(':');
                if (offset < 0)
                {
                    throw new System.IO.IOException("Invalid sequenceNumbderFile '" + seqNumFileName
                            + "' character ':' is missing");
                }
                cache.setNextSenderMsgSeqNum(int.Parse(s.Substring(0, offset)));
                cache.setNextTargetMsgSeqNum(int.Parse(s.Substring(offset + 1)));
            }
        }

        private void initializeMessageIndex()
        {
            try
            {
                // this part is unnecessary if no offsets are being stored in memory
                if (messageIndex != null)
                {
                    messageIndex.Clear();
                    File headerFile = new File(headerFileName);
                    if (headerFile.Exists())
                    {
                        DataInputStream headerDataInputStream = new DataInputStream(
                               (new FileInputStream(headerFile)));
                        try
                        {
                            while (headerDataInputStream.available() > 0)
                            {

                                long sequenceNumber = headerDataInputStream.readLong();
                                long offset = headerDataInputStream.readLong();
                                int size = headerDataInputStream.readInt();
                                updateMessageIndex(sequenceNumber, new long[] { offset, size });
                            }
                        }
                        finally
                        {
                            headerDataInputStream.Close();
                        }
                    }
                }
                headerFileOutputStream = new FileOutputStream(headerFileName);
                headerDataOutputStream = new DataOutputStream(headerFileOutputStream);
            }
            catch //(Exception ex)
            {
                throw;
            }

        }
        private void updateMessageIndex(Int64 sequenceNum, long[] offsetAndSize)
        {
            try
            {
                // Remove the lowest indexed sequence number if this addition
                // would result the index growing to larger than maxCachedMsgs.
                if (messageIndex.Count >= maxCachedMsgs && messageIndex[sequenceNum] == null)
                {
                    // Line below requires Java 6, using Java 5 approximation
                    //messageIndex.pollFirstEntry();
                    //messageIndex.DeleteMin();
                    messageIndex.Clear();
                }
                messageIndex.Add(sequenceNum, offsetAndSize);
                //logger.Info("Adding seqNumber=" + sequenceNum);
            }
            catch //(Exception ex)
            {
                throw;
            }
        }

        private String getRandomAccessFileOptions()
        {
            return READ_OPTION + WRITE_OPTION + (syncWrites ? SYNC_OPTION : NOSYNC_OPTION);
        }

        /**
         * Close the store's files.
         * @
         */
        public void closeFiles()
        {
            closeOutputStream(headerDataOutputStream);
            closeFile(messageFileWriter);
            closeFile(messageFileReader);
            closeFile(sequenceNumberFile);
        }

        private void closeFile(RandomAccessFile file)
        {
            if (file != null)
            {
                file.Close();
            }
        }

        private void closeOutputStream(IOutputStream stream)
        {
            if (stream != null)
            {
                stream.Close();
            }
        }

        public void DeleteFiles()
        {
            closeFiles();
            deleteFile(headerFileName);
            deleteFile(msgFileName);
        }

        private void deleteFile(String fileName)
        {
            File file = new File(fileName);
            if (file.Exists() && !file.Delete())
            {
                Logger.LoggerWrite("File delete failed: " + fileName);
            }
        }

        /* (non-Javadoc)
         * @see quickfix.MessageStore#getNextSenderMsgSeqNum()
         */
        public long getNextSenderMsgSeqNum()
        {
            return cache.getNextSenderMsgSeqNum();
        }

        /* (non-Javadoc)
         * @see quickfix.MessageStore#getNextTargetMsgSeqNum()
         */
        public long getNextTargetMsgSeqNum()
        {
            return cache.getNextTargetMsgSeqNum();
        }

        /* (non-Javadoc)
         * @see quickfix.MessageStore#setNextSenderMsgSeqNum(int)
         */
        public void setNextSenderMsgSeqNum(long next)
        {
            cache.setNextSenderMsgSeqNum(next);
            storeSequenceNumbers();
        }

        /* (non-Javadoc)
         * @see quickfix.MessageStore#setNextTargetMsgSeqNum(int)
         */
        public void setNextTargetMsgSeqNum(long next)
        {
            cache.setNextTargetMsgSeqNum(next);
            storeSequenceNumbers();
        }

        /* (non-Javadoc)
         * @see quickfix.MessageStore#incrNextSenderMsgSeqNum()
         */
        public void incrNextSenderMsgSeqNum()
        {
            cache.incrNextSenderMsgSeqNum();
            storeSequenceNumbers();
        }

        /* (non-Javadoc)
         * @see quickfix.MessageStore#incrNextTargetMsgSeqNum()
         */
        public void incrNextTargetMsgSeqNum()
        {
            cache.incrNextTargetMsgSeqNum();
            storeSequenceNumbers();
        }

        /* (non-Javadoc)
         * @see quickfix.MessageStore#get(int, int, java.util.Collection)
         */
        public void get(long startSequence, long endSequence, SortedList<long, String> messages, out long lastSeqNumber)
        {
            try
            {
                lock (locker)
                {
                    lastSeqNumber = startSequence;
                    List<long> uncachedOffsetMsgIds = new List<long>();
                    // Use a treemap to make sure the messages are sorted by sequence num
                    SortedList<long, String> messagesFound = new SortedList<long, String>();
                    for (long i = startSequence; i <= endSequence; i++)
                    {
                        String message = getMessage(i);
                        if (message != null)
                        {
                            messagesFound.Add(i, message);
                        }
                        else
                        {
                            uncachedOffsetMsgIds.Add(i);
                        }
                    }

                    if (uncachedOffsetMsgIds.Count != 0)
                    {
                        // parse the header file to find missing messages
                        File headerFile = new File(headerFileName);
                        DataInputStream headerDataInputStream = new DataInputStream(
                               (new FileInputStream(headerFile)));
                        try
                        {
                            while (headerDataInputStream.available() > 0)
                            {
                                long sequenceNumber = headerDataInputStream.readLong();
                                long offset = headerDataInputStream.readLong();
                                int size = headerDataInputStream.readInt();
                                if (uncachedOffsetMsgIds.Remove(sequenceNumber))
                                {
                                    String message = getMessage(new long[] { offset, size },
                                           sequenceNumber);
                                    if (message != null)
                                    {
                                        messagesFound.Add(sequenceNumber, message);
                                    }
                                }
                                if (uncachedOffsetMsgIds.Count == 0)
                                {
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            headerDataInputStream.Close();
                        }
                    }
                    foreach (KeyValuePair<long, string> keyValuePair in messagesFound)
                    {
                        messages.Add(keyValuePair.Key, keyValuePair.Value);
                        lastSeqNumber = keyValuePair.Key;
                        Logger.LoggerWrite("index=" + keyValuePair.Key, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
                    }

                    //messages.AddRange(messagesFound.Values);
                }
            }
            catch //(Exception ex)
            {
                throw;
            }
        }

        /**
         * This method is here for JNI API consistency but it's not 
         * implemented. Use get(int, int, Collection) with the same 
         * start and end sequence.
         * 
         */
        public long get()
        {
            throw new Exception("not supported");
        }

        private String getMessage(long i)
        {
            String message = null;
            if (messageIndex != null)
            {
                if (messageIndex.Contains(i))
                {
                    long[] offsetAndSize = messageIndex[(i)];
                    if (offsetAndSize != null)
                    {
                        message = getMessage(offsetAndSize, i);
                    }
                }
            }
            return message;
        }

        private String getMessage(long[] offsetAndSize, long i)
        {
            long offset = offsetAndSize[0];
            messageFileReader.Seek(offset);
            int size = (int)offsetAndSize[1];
            byte[] data = new byte[size];
            int sizeRead = messageFileReader.Read(data);
            if (sizeRead != size)
            {
                throw new System.IO.IOException("Truncated input while reading message: messageIndex=" + i
                        + ", offset=" + offset + ", expected size=" + size + ", size read from file="
                        + sizeRead);
            }

            String message = StringByteConversion.GetString(data);
            messageFileReader.Seek(messageFileReader.Length());
            return message;
        }
        readonly object locker = new object();

        /* (non-Javadoc)
         * @see quickfix.MessageStore#set(int, java.lang.String)
         */
        public bool set(Int64 sequence, String message)
        {
            lock (locker)
            {
                long offset = messageFileWriter.getFilePointer();
                int size = message.Length;
                if (messageIndex != null)
                {
                    updateMessageIndex(sequence, new long[] { offset, size });
                }

                headerDataOutputStream.writeInt64(sequence);
                headerDataOutputStream.writeLong(offset);
                headerDataOutputStream.writeInt(size);
                headerDataOutputStream.Flush();
                if (syncWrites)
                {
                    headerFileOutputStream.getFD().sync();
                }

                messageFileWriter.Write(StringByteConversion.GetBytes(message));
            }
            return true;
        }

        private void storeSequenceNumbers()
        {
            sequenceNumberFile.Seek(0);
            // I changed this from explicitly using a StringBuffer because of
            // recommendations from Sun. The performance also appears higher
            // with this implementation. -- smb.
            // http://bugs.sun.com/bugdatabase/view_bug.do;:WuuT?bug_id=4259569
            sequenceNumberFile.writeUTF("" + cache.getNextSenderMsgSeqNum() + ':'
                    + cache.getNextTargetMsgSeqNum());
        }

        String getHeaderFileName()
        {
            return headerFileName;
        }

        String getMsgFileName()
        {
            return msgFileName;
        }

        //String getSeqNumFileName()
        //{
        //    return seqNumFileName;
        //}

        /*
         * (non-Javadoc)
         * @see quickfix.RefreshableMessageStore#refresh()
         */
        public void refresh()
        {
            initialize(false);
        }

        /* (non-Javadoc)
         * @see quickfix.MessageStore#reset()
         */
        public void reset()
        {
            initialize(true);
        }


        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                headerDataOutputStream.Close();
                messageFileReader.Close();
                messageFileWriter.Close();
                sequenceNumberFile.Close();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
