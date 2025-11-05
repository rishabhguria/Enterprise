using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities.IO;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.IO
{
    public class FileStoreImplTests
    {
        FileStoreImpl fileStore = null;
        string path = MockDataPath.GetFolderPath();

        private void InitFileStoreImpl(string SessionName)
        {
            fileStore = new FileStoreImpl(path + @"/", new Session(SessionName), true, 10000);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileStoreImpl")]
        public void getCreationTime_ReturnCurrentTime()
        {
            string SessionName = "test1";
            // Arrange
            InitFileStoreImpl(SessionName);
            var ExpectedValue = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm");

            // Act
            var ActualValue = fileStore.getCreationTime().ToString("MM/dd/yyyy HH:mm");

            // Assert
            Assert.Equal(ActualValue, ExpectedValue);

            DeleteFile(SessionName);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileStoreImpl")]
        public void getNextSenderMsgSeqNum_ReturnsSequenceNumberOfNextMessage()
        {
            string SessionName = "test2";
            // Arrange
            InitFileStoreImpl(SessionName);
            long expectedSeqNum = 1; // 1 cause from 1 sequence will start
            fileStore.setNextSenderMsgSeqNum(expectedSeqNum);

            // Act
            var actualSeqNum = fileStore.getNextSenderMsgSeqNum();

            // Assert
            Assert.Equal(expectedSeqNum, actualSeqNum);

            DeleteFile(SessionName);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileStoreImpl")]
        public void getNextTargetMsgSeqNum_ReturnsNextTargetMsgSeqNumFromCache()
        {
            string SessionName = "test3";
            // Arrange
            InitFileStoreImpl(SessionName);
            long expectedSeqNum = 10;
            fileStore.setNextTargetMsgSeqNum(expectedSeqNum);

            // Act
            long actualSeqNum = fileStore.getNextTargetMsgSeqNum();
            // Assert
            Assert.Equal(expectedSeqNum, actualSeqNum);

            DeleteFile(SessionName);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileStoreImpl")]
        public void setNextSenderMsgSeqNum_ReturnsSequenceNumberWhichIsSet()
        {
            string SessionName = "test4";
            // Arrange
            InitFileStoreImpl(SessionName);
            long expectedSeqNum = 10;

            // Act
            fileStore.setNextSenderMsgSeqNum(expectedSeqNum);

            // Assert
            var atualSeqNum = fileStore.getNextSenderMsgSeqNum();
            Assert.Equal((long)expectedSeqNum, atualSeqNum);

            DeleteFile(SessionName);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileStoreImpl")]
        public void setNextTargetMsgSeqNum_ReturnsSequenceNumberWhichIsSet()
        {
            string SessionName = "test5";
            // Arrange
            InitFileStoreImpl(SessionName);
            long expectedSeqNum = 10;

            // Act
            fileStore.setNextTargetMsgSeqNum(expectedSeqNum);

            // Assert
            var atualSeqNum = fileStore.getNextTargetMsgSeqNum();
            Assert.Equal((long)expectedSeqNum, atualSeqNum);

            DeleteFile(SessionName);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileStoreImpl")]
        public void get_PopulatesMessagesInRange()
        {
            string SessionName = "test6";
            // Arrange
            InitFileStoreImpl(SessionName);
            //string _path = MockDataPath.GetFolderPath();
            //FileStoreImpl _fileStore = new FileStoreImpl(_path + @"/", new Session("ARCA-TEST1"), true, 10000);
            long startSequence = 1;
            long endSequence = 3;
            SortedList<long, string> messages = new SortedList<long, string>();
            long lastSeqNumber = 0;

            // Act
            fileStore.get(startSequence, endSequence, messages, out lastSeqNumber);

            // Assert
            Assert.Equal(1, lastSeqNumber);

            DeleteFile(SessionName);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileStoreImpl")]
        public void Set_PassedTextToWrite_ReturnTrue()
        {
            string SessionName = "test7";
            // Arrange
            InitFileStoreImpl(SessionName);
            bool expectedresult = true;

            // Act
            var atucalresult = fileStore.set(2, "Test Message");

            // Assert
            Assert.Equal(expectedresult, atucalresult);

            DeleteFile(SessionName);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileStoreImpl")]
        public void Set_PassedNullToWrite_ReturnNullReferenceException()
        {
            string SessionName = "test8";
            // Act
            InitFileStoreImpl(SessionName);
            Action action = () => fileStore.set(1, null);

            // Assert
            Assert.Throws<NullReferenceException>(action);

            DeleteFile(SessionName);
        }

        private void DeleteFile(string SessionName)
        {
            fileStore.Dispose();
            // Check if file exists than delete it
            if (System.IO.File.Exists(path + SessionName + "_body.log"))
            {
                System.IO.File.Delete(path + SessionName + "_body.log");
            }
            if (System.IO.File.Exists(path + SessionName + "_header.log"))
            {
                System.IO.File.Delete(path + SessionName + "_header.log");
            }
            if (System.IO.File.Exists(path + SessionName + "_seqnums.log"))
            {
                System.IO.File.Delete(path + SessionName + "_seqnums.log");
            }
        }
    }
}
