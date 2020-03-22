using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            var tags = new List<string>();

            do
            {
                var timePrefix = $"[{DateTime.Now:HH:mm:ss}]";//TODO - move time format to configuration as well
                var tagsSuffix = GetTagsCollectionInLine(tags);

                Console.Write($"{timePrefix}{tagsSuffix} ");

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
                        Console.WriteLine(
                            $"[{msgPrefixHelp}] --add-tag=<tag-name> - will add tag to tags list for next records");
                        Console.WriteLine($"[{msgPrefixHelp}] --clear-tags - will clean tags list");
                    }
                    else if (Regex.IsMatch(record, "^(-x|--exit)"))
                    {
                        isExit = true;
                    }
                    else if (Regex.IsMatch(record, "^(--add-tag)"))
                    {
                        var index = "--add-tag=".Length;
                        var tag = index < record.Length ? record.Substring(index) : string.Empty;

                        if (!string.IsNullOrEmpty(tag))
                        {
                            tags.Add(tag);
                        }
                        else
                        {
                            Console.WriteLine($"[{msgPrefixError}] provided tag name was empty!");
                        }
                    }
                    else if (Regex.IsMatch(record, "^(--clear-tags)"))
                    {
                        tags.Clear();
                    }
                    else
                    {
                        Console.WriteLine($"[{msgPrefixError}] parameter'{record}' can't be recognized!");
                    }
                }
                else
                {
                    record = $"{timePrefix}{tagsSuffix} {record}";

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

        private static string GetTagsCollectionInLine(IEnumerable<string> tags)
        {
            if (tags == null || !tags.Any()) return string.Empty;

            var sb = new StringBuilder();

            foreach (var tag in tags)
            {
                sb.Append($"[{tag}]");
            }

            return sb.ToString();
        }
    }
}
