using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace VideoRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("GOTO:CODE SIMPLE FILE RENAMER");
            Console.WriteLine("Version: 1.0");
            Console.WriteLine($"Copyright (c){DateTime.Now.Year} Jonny Wilson.");
            Console.WriteLine("Web: GOTOCODE.UK");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Initialising...");
            Console.WriteLine("");

            if (Directory.Exists("Renamed"))
            {
                Console.WriteLine("RENAMED directory already exists, deleting...");
                Console.WriteLine("");
                Directory.Delete("Renamed", true);
                Thread.Sleep(3000);
            }
            else
            {
                Console.WriteLine("Creating a new RENAMED directory...");
                Console.WriteLine("");
                Directory.CreateDirectory("Renamed");
                Thread.Sleep(3000);
            }

            if (!Directory.Exists("Renamed"))
            {
                Directory.CreateDirectory("Renamed");
                Thread.Sleep(3000);
            }

            Console.WriteLine("Scanning subfolders for .mp4 videos...");
            Console.WriteLine("");

            var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.mp4", SearchOption.AllDirectories).ToList();

            Console.WriteLine($"Found {allFiles.Count} files");
            Console.WriteLine("");


            Dictionary<string, string> fileCheck = new Dictionary<string, string>();
            Dictionary<string, string> duplicateFiles = new Dictionary<string, string>();

            int fileCounter = 0;

            Console.WriteLine($"Sorting original and duplicates...");

            Console.WriteLine("");
            Console.WriteLine("");

            foreach (var file in allFiles)
            {
                var fileHash = GetHash(file);

                if (!fileCheck.ContainsKey(fileHash))
                {
                    Console.WriteLine($"Caching original file: {file} (Hash: {fileHash})");
                    fileCheck.Add(fileHash, file);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Sorting duplicate file: {file} (Hash: {fileHash})");
                    duplicateFiles.Add(fileHash, file);
                }
            }

            Console.WriteLine("");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var kvp in fileCheck)
            {
                Console.WriteLine($"Copying and renaming: {kvp.Value}");
                FileInfo fileInfo = new FileInfo(kvp.Value);
                File.Copy(kvp.Value, $"Renamed\\{fileCounter.ToString().PadLeft(5, '0')}.mp4", true);
                fileCounter++;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("All files in RENAMED folder are unique files");
            Console.ReadKey();
        }

        private static string GetHash(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                var sha1 = new SHA1Managed();
                byte[] sha1Checksum = sha1.ComputeHash(stream);
                return BitConverter.ToString(sha1Checksum).Replace("-", string.Empty).Replace(" ", string.Empty).ToLower();
            }
        }
    }
}
