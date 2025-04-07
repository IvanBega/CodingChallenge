using Magna.Dexsys.FileHandler.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
        ArgumentException.ThrowIfNullOrEmpty(directory);
        ArgumentException.ThrowIfNullOrEmpty(searchValue);

        try
        {

            var files = Directory.EnumerateFiles(directory);

            Parallel.ForEach(files, file =>
            {
                string contents = File.ReadAllText(file, Encoding.Unicode);
                if (contents.Contains(searchValue))
                {
                    _filesLocatedBag.Add(new FileDetails(file, Path.GetFileName(file), contents.Length, contents));
                }
            });

            _filesLocated.AddRange(_filesLocatedBag);
        }
        catch (Exception)
        {
            throw;
        }

         
        return _filesLocatedBag.Count;
    }
}
