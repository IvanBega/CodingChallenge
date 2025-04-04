using Magna.Dexsys.FileHandler.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Magna.Dexsys.FileHandler.Services;
public class FileSearchService
{
    public IReadOnlyList<FileDetails> FilesLocated => _filesLocated.AsReadOnly();

    private readonly List<FileDetails> _filesLocated = [];

    private readonly ConcurrentBag<FileDetails> _filesLocatedBag = [];

    /// <summary>
    /// Populate the instance's FilesLocated member with a collection of
    /// files which contain the partialContent value anywhere in the 
    /// file.
    /// </summary>
    /// <param name="directory">Directory containing files to search</param>
    /// <param name="searchValue">Data to search for in files</param>
    /// <returns>Return the number of files located</returns>
    public int LocateFilesContainingSearchValue(string directory, string searchValue)
    {
        var files = Directory.EnumerateFiles(directory); 
        int count = 0;

        Parallel.ForEach(files, file =>
        {
            string contents = File.ReadAllText(file);
            if (contents.IndexOf(searchValue) >= 0)
            {
                Interlocked.Increment(ref count);
                _filesLocatedBag.Add(new FileDetails(directory, file, contents.Length, contents));
                count++;
            }
        });

        _filesLocated.AddRange(_filesLocatedBag);
        return count;
    }
}
