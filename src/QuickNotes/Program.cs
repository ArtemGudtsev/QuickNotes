using System;
using System.IO;

namespace QuickNotes
{
    class Program
    {
        static void Main(string[] args)
        {
            const string folderWithNotes = @"E:\out\qnotes";
            var currentNotesFile = $"{DateTime.Now:yyyyMMdd}.qnote";
            var fullPathToNotesFile = Path.Combine(folderWithNotes, currentNotesFile);
            var isExit = !(args.Length == 1 && args[0] == "-a");

            do
            {
                var timePrefix = $"[{DateTime.Now:HH:mm:ss}] ";

                Console.Write(timePrefix);

                var record = Console.ReadLine();

                if (record == "x")
                {
                    isExit = true;
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
