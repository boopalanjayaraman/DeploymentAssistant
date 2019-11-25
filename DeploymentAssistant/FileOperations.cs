using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant
{
    /// <summary>
    /// File operations class
    /// This is to serve as a wrapper for file operations, and make code with file operations testable.
    /// </summary>
    internal class FileOperations : IFileOperations
    {
        /// <summary>
        /// Wrapper for File.Exists
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns>bool</returns>
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// Wrapper for File.Copy method
        /// </summary>
        /// <param name="sourceFileName">source file name</param>
        /// <param name="destFileName">destination file name</param>
        public void Copy(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
        }

        /// <summary>
        /// Wrapper for File.WriteAllText
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="contents">contents to write</param>
        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        /// <summary>
        /// Wrapper for File.ReadAllText
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>file contents string</returns>
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
