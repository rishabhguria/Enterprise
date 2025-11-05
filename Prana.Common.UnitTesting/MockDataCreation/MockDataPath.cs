using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.UnitTesting.MockDataCreation
{
    public static class MockDataPath
    {
        /// <summary>
        /// To Get the folder path of MockDataCreation
        /// </summary>
        /// <returns>path</returns>
        public static string GetFolderPath()
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;

            // This will get the current PROJECT bin directory (ie ../bin/)
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            return projectDirectory + @"\MockDataCreation\";
        }

        /// <summary>
        /// To get the path of predefined file TestingTextFile.txt
        /// </summary>
        /// <returns>path</returns>
        public static string GetFilePath()
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;

            // This will get the current PROJECT bin directory (ie ../bin/)
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            return projectDirectory + @"\MockDataCreation\TestingTextFile.txt";
        }

        /// <summary>
        /// To get the path of file you are going to create
        /// </summary>
        /// <param name="filename">name of file you want to create</param>
        /// <returns>path</returns>
        public static string GetFilePath(string filename)
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;

            // This will get the current PROJECT bin directory (ie ../bin/)
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            return projectDirectory + @"\MockDataCreation\" + filename;
        }

        /// <summary>
        /// To get the path of Testing folder for file test
        /// </summary>
        /// <returns>path</returns>
        public static string GetTestingFolderPath()
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;

            // This will get the current PROJECT bin directory (ie ../bin/)
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            return projectDirectory + @"\MockDataCreation\TestingPath\";
        }
    }
}
