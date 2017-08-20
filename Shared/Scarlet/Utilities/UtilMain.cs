using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scarlet.Utilities
{
    public static class UtilMain
    {

        /// <summary>
        /// Returns subarray of given array.
        /// </summary>
        /// <typeparam name="T">
        /// Datatype of array
        /// </typeparam>
        /// <param name="data">Array to manipulate</param>
        /// <param name="index">Starting index of subarray.</param>
        /// <param name="length">Length of wanted subarray.</param>
        /// <returns>
        /// Sub array of data[index:index+length-1] (inclusive)
        /// </returns>
        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Gives a user-readable representation of a byte array.
        /// </summary>
        /// <param name="Data">The array to format.</param>
        /// <param name="Spaces">Whether to add spaces between every byte in the output</param>
        /// <returns>A string formatted as such: "4D 3A 20 8C", or "4D3A208C", depending on the Spaces parameter.</returns>
        public static string BytesToNiceString(byte[] Data, bool Spaces)
        {
            if (Data == null || Data.Length == 0) { return string.Empty; }
            StringBuilder Output = new StringBuilder();
            for (int i = 0; i < Data.Length; i++)
            {
                Output.Append(Data[i].ToString("X2"));
                if (Spaces) { Output.Append(' '); }
            }
            if (Spaces) { Output.Remove(Output.Length - 1, 1); }
            return Output.ToString();
        }

        /// <summary>
        /// Takes in a string,
        /// converts the string
        /// into its byte representation.
        /// </summary>
        /// <param name="Data">
        /// String to convert into bytes</param>
        /// <returns>
        /// Byte array that represents the given string.</returns>
        public static byte[] StringToBytes(string Data)
        {
            List<byte> Output = new List<byte>();
            Data = Data.Replace(" ", "");
            for (int Chunk = 0; Chunk < Math.Ceiling(Data.Length / 2.000); Chunk++)
            {
                int Start = Data.Length - ((Chunk + 1) * 2);
                string Section;
                if (Start >= 0) { Section = Data.Substring(Start, 2); }
                else { Section = Data.Substring(0, 1); }
                Output.Add(Convert.ToByte(Section, 16));
            }
            return Output.ToArray();
        }

        /// <summary>
        /// Searches a file system starting in a given path,
        /// looking for a specific, given file. If it cannot
        /// find the file, a FileNotFoundException is thrown.
        /// </summary>
        /// <param name="SearchIn">The directory to begin the search in.</param>
        /// <param name="SearchFor">The file to search for.</param>
        /// <param name="Type">The type of search to do on the file system, either
        /// Depth First, Breadth First, or single directory.</param>
        /// <returns>The path to the file searched for.</returns>
        /// <exception cref="FileNotFoundException">If the file is not found in search scope.</exception>
        public static string SearchDirectory(string SearchIn, string SearchFor, SearchType Type = SearchType.BreadthFirst)
        {
            // Clean Input
            char LastChar = SearchIn[SearchIn.Length - 1];
            if (LastChar == '\\') { SearchIn = SearchIn.Remove(SearchIn.Length - 1) + "/"; }
            else if (LastChar != '/') { SearchIn += '/'; }

            // Switch on Search type to determine correct method
            string SearchReturn = "";
            switch (Type)
            {
                case (SearchType.BreadthFirst):
                    SearchReturn = SearchDirectoryBF(SearchIn, SearchFor);
                    break;
                case (SearchType.DepthFirst):
                    SearchReturn = SearchDirectoryDF(SearchIn, SearchFor);
                    break;
                case (SearchType.SingleFolder):
                    SearchReturn = SearchSingleDirectory(SearchIn, SearchFor);
                    break;
            }
            if (SearchReturn != UtilConstants.SEARCH_NOT_FOUND_STR) { return SearchReturn; }
            throw new FileNotFoundException("Could not find file " + SearchFor + " starting in " + SearchIn);
        }

        /// <summary>
        /// Searches a file system starting in a given path,
        /// looking for a specific, given file. Uses a breadth
        /// first search approach.
        /// </summary>
        /// <param name="SearchIn">The directory to begin the search in.</param>
        /// <param name="SearchFor">The file to search for.</param>
        /// <returns>The path to the file searched for.</returns>
        private static string SearchDirectoryBF(string SearchIn, string SearchFor)
        {
            // Note: SearchIn will be referred to in comments as the "root" directory
            // and SearchThis will be referred to as the "target" file
            // Assume that the user will not have two files with the same name in the 
            // system being searched. Thus, return the first file that matches the 
            // target file name
            string[] Directories = { "" };
            string[] Files = { "" };
            // First, get lists of all the directories and files in the root directory. If there is 
            // an error, stop.
            try
            {
                Directories = Directory.GetDirectories(SearchIn);
                Files = Directory.GetFiles(SearchIn);
            }
            catch { return UtilConstants.SEARCH_NOT_FOUND_STR; }
            bool Contained = false;
            // Check if the target file is in this directory. If so, return the filepath.
            foreach (string File in Files)
            {
                if (File.IndexOf(SearchFor) != -1)
                {
                    Contained = true;
                    break;
                }
            }
            if (Contained)
            {
                return SearchIn + SearchFor;
            }
            // At this point, the file has not been found. The subdirectories will now be searched.
            else
            {
                // Search the subdirectories to see if the target file is within them.
                foreach (string Dir in Directories)
                {
                    string Found = SearchSingleDirectory(Dir, SearchFor);
                    if (Found != UtilConstants.SEARCH_NOT_FOUND_STR)
                    {
                        return Found;
                    }

                }
                // If the target file is not in the next level of directories, proceed to recurse over 
                // the directories.
                foreach (string Dir in Directories)
                {
                    string Found = SearchDirectoryDF(Dir, SearchFor);
                    if (Found != UtilConstants.SEARCH_NOT_FOUND_STR)
                    {
                        return Found;
                    }
                }
            }
            // At this point, the file has not been found. Report to the user and exit.
            return UtilConstants.SEARCH_NOT_FOUND_STR;
        }

        /// <summary>
        /// Searches a file system starting in a given path,
        /// looking for a specific, given file. Uses a depth
        /// first search approach.
        /// </summary>
        /// <param name="SearchIn">The directory to begin the search in.</param>
        /// <param name="SearchFor">The file to search for.</param>
        /// <returns>The path to the file searched for.</returns>
        private static string SearchDirectoryDF(string SearchIn, string SearchFor)
        {
            // Note: SearchIn will be referred to in comments as the "root" directory
            // and SearchThis will be referred to as the "target" file
            string[] Directories = { "" };
            // First, get lists of all the directories and files in the root directory. If there is 
            // an error, stop.
            try
            {
                Directories = Directory.GetDirectories(SearchIn);
            }
            catch { return UtilConstants.SEARCH_NOT_FOUND_STR; }
            // Check if the SearchThis file is in this directory. If so, return the filepath.
            // Otherwise, exit.
            string Found = UtilConstants.SEARCH_NOT_FOUND_STR;
            foreach (string Dir in Directories)
            {
                Found = SearchDirectoryBF(Dir, SearchFor);
                if (Found != UtilConstants.SEARCH_NOT_FOUND_STR)
                {
                    break;
                }
            }
            return Found;
        }

        /// <summary>
        /// Searches a file system starting in a given path,
        /// looking for a specific, given file. Searched only
        /// a single folder.
        /// </summary>
        /// <param name="SearchIn">The directory to begin the search in.</param>
        /// <param name="SearchFor">The file to search for.</param>
        /// <returns>The path to the file searched for.</returns>
        private static string SearchSingleDirectory(string SearchIn, string SearchFor)
        {
            // Note: SearchIn will be referred to in comments as the "root" directory
            // and SearchThis will be referred to as the "target" file
            string[] Files = { "" };
            // First, get lists of all the directories and files in the root directory. If there is 
            // an error, stop.
            try
            {
                Files = Directory.GetFiles(SearchIn);
            }
            catch { return UtilConstants.SEARCH_NOT_FOUND_STR; }
            bool Contained = false;
            // Check if the SearchThis file is in this directory. If so, return the filepath.
            // Otherwise, exit.
            foreach (string File in Files)
            {
                if (File.IndexOf(SearchFor) != -1)
                {
                    Contained = true;
                    break;
                }
            }
            if (Contained)
            {
                return SearchIn + SearchFor;
            }
            else
            {
                return UtilConstants.SEARCH_NOT_FOUND_STR;
            }
        }

    }

}
