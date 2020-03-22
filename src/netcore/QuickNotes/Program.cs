using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace QuickNotes
{
    //TODO - ability to add/clear tags for records
    class Program
    {
        static void Main(string[] args)
        {
            //TODO - move to config file, create classes to work with this configuration
            const string folderWithNotes = @"E:\out\qnotes";
            var currentNotesFile = $"{DateTime.Now:yyyyMMdd}.qnote";
            var fullPathToNotesFile = Path.Combine(folderWithNotes, currentNotesFile);
            var isExit = !(args.Length == 1 && args[0] == "-a");

            const string msgPrefixError = "ERROR";
            const string msgPrefixHelp = "HELP";

            do
            {
                var timePrefix = $"[{DateTime.Now:HH:mm:ss}] ";//TODO - move time format to configuration as well

                Console.Write(timePrefix);

                var rawRecord = Console.ReadLine();

                if (string.IsNullOrEmpty(rawRecord))
                {
                    Console.WriteLine($"[{msgPrefixError}] you can't save empty record to notes!");
                    continue;
                }

                var record = rawRecord.Trim();

                if (Regex.IsMatch(record, "^(-|--)"))//TODO - logic with parsing parameters must be moved to separate classes
                {
                    if (Regex.IsMatch(record, "^(-h|--help)"))
                    {
                        Console.WriteLine($"[{msgPrefixHelp}] -h (--help) - will show this help");
                        Console.WriteLine($"[{msgPrefixHelp}] -x (--exit) - will close tool");
                    }
                    else if (Regex.IsMatch(record, "^(-x|--exit)"))
                    {
                        isExit = true;
                    }
                    else
                    {
                        Console.WriteLine($"[{msgPrefixError}] parameter'{record}' can't be recognized!");
                    }
                }
                else
                {
                    record = timePrefix + record;
                    if (File.Exists(fullPathToNotesFile))
                    {
                        File.AppendAllLines(fullPathToNotesFile, new[] { record });
                    }
                    else
                    {
                        File.WriteAllLines(fullPathToNotesFile, new[] { record });
                    }
                }
            } while (!isExit);
        }
    }
}
