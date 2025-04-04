using Magna.Dexsys.FileHandler.Models;
using Magna.Dexsys.FileHandler.Services;
using System.Diagnostics;

namespace Magna.Dexsys.FileHandler.Cli;

public class Program
{
    private const string _fileLocation = "C:\\Users\\ibega\\Downloads\\generatedFiles\\generatedFiles";
    private const string _searchValue = "IVAN";

    public static void Main(string[] args)
    {
        int testCount = 0;
        int testLimit = 10;

        for (; testCount < testLimit; testCount++)
        {
            Test(_searchValue);
        }

        Console.ReadLine();
    }

    private static void Test(string searchValue)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        FileSearchService searchService = new();
        searchService.LocateFilesContainingSearchValue(_fileLocation, searchValue);
        stopwatch.Stop();

        foreach (FileDetails item in searchService.FilesLocated)
        {
            Console.WriteLine(item.Name, item.Content);
        }

        Console.WriteLine($"Completed in {stopwatch.ElapsedMilliseconds}");
    }
}
