using System;
using System.IO;

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

            do
            {
                var timePrefix = $"[{DateTime.Now:HH:mm:ss}] ";//TODO - move time format to configuration as well

                Console.Write(timePrefix);

                var record = Console.ReadLine();

                if (string.IsNullOrEmpty(record))
                {
                    //TODO - error about empty record to the notes
                }
                else if (record == "-x")//TODO - logic with parsing parameters must be moved to separate classes
                {
                    isExit = true;
                }
                else if (record[0] == '-')
                {
                    //TODO - error in case if parameter can't be recognized
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
