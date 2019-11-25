using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant
{
    /// <summary>
    /// This is to serve as a wrapper for file operations, and make code with file operations testable.
    /// </summary>
    internal interface IFileOperations
    {
        /// <summary>
        /// Wrapper for File.Exists
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns>bool</returns>
        bool Exists(string filePath);

        /// <summary>
        /// Wrapper for File.Copy method
        /// </summary>
        /// <param name="sourceFileName">source file name</param>
        /// <param name="destFileName">destination file name</param>
        void Copy(string sourceFileName, string destFileName);

        /// <summary>
        /// Wrapper for File.WriteAllText
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="contents">contents to write</param>
        void WriteAllText(string path, string contents);

        /// <summary>
        /// Wrapper for File.ReadAllText
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>file contents string</returns>
        string ReadAllText(string path);
    }
}
