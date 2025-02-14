using System;
using System.Diagnostics;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;

namespace WebPToPNGCoverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool userContinue = true;

            while (userContinue)
            {
                Console.WriteLine("*** WebP to PNG Converter ***\n");
                Console.Write("Enter the folder with the to be converted images: ");
                string folderPath = Console.ReadLine() ?? string.Empty;
                bool userInputValid = false;
                while (!userInputValid)
                {
                    Console.Write("Do you want to replace WebP files (y/n): ");
                    string userInput = Console.ReadLine() ?? string.Empty;

                    if (userInput == "y")
                    {
                        ConvertImages(folderPath, true);
                        userInputValid = true;
                    }
                    else if (userInput == "n")
                    {
                        ConvertImages(folderPath, false);
                        userInputValid = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                    }
                }

                Console.Write("Do you want to convert more images (y): ");
                if (Console.ReadLine() != "y")
                {
                    userContinue = false;
                }
            }
        }

        static void ConvertImages(string path, bool replaceFiles)
        {
            Stopwatch stopWatchSearchFiles = new Stopwatch();
            Stopwatch stopWatchConversion = new Stopwatch();
            stopWatchSearchFiles.Start();

            if (Directory.Exists(path))
            {
                string[] webpFiles = Directory.GetFiles(path, "*.webp");
                foreach (string file in webpFiles)
                {
                    Console.WriteLine($"Found: {file}");
                }
                stopWatchSearchFiles.Stop();

                Console.WriteLine($"Found {webpFiles.Length} WebP files in {path}. Elapsled time: {stopWatchSearchFiles.Elapsed.TotalSeconds} s");

                if (webpFiles.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("No WebP files found in specified directory");
                    Console.ResetColor();
                }
                else
                {
                    stopWatchConversion.Start();
                    foreach (string file in webpFiles)
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        string pngFile = Path.ChangeExtension(file, ".png");
                        using (Image image = Image.Load(file))
                        {
                            image.Save(pngFile, new PngEncoder());
                        }

                        if (replaceFiles)
                        {
                            File.Delete(file);
                        }

                        stopwatch.Stop();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Successfully converted {file}. Elapsed time: {stopwatch.Elapsed.TotalSeconds} s");
                        Console.ResetColor();
                    }

                    stopWatchConversion.Stop();
                    Console.WriteLine($"Converted {webpFiles.Length} WebP files to PNG. Elapsed time: {stopWatchConversion.Elapsed.TotalSeconds} s");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The specified directory does not exist.");
                Console.ResetColor();
            }
        }
    }
}
