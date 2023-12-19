using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace MapTools
{
    class Program
    {

        static string currentDict = "";
        static Dictionary<int, char> colorDict = new Dictionary<int, char>();

        static void Main(string[] args)
        {
            Console.WriteLine("Verdant MapTools");

            ParseArguments(args);
        }

        static void ParseArguments(string[] args)
        {
            string imagePath = "";
            string dictPath = "default.mapdict";
            string outputPath = "output.txt";

            foreach (string a in args)
            {
                if (a.EndsWith(".mapdict"))
                    dictPath = a;
                else if (a.EndsWith(".txt"))
                    outputPath = a;
                else
                    imagePath = a;
            }

            if (!File.Exists(imagePath))
            {
                //it might be a directory path
                if (Directory.Exists(imagePath))
                {
                    //load each image file in directory
                    Console.WriteLine("Parsing all files in directory, output files will be named automatically");
                    int incompatibleCt = 0;
                    foreach (string p in Directory.GetFiles(imagePath))
                    {
                        if (p.EndsWith(".png") || p.EndsWith(".bmp") || p.EndsWith(".jpg") || p.EndsWith(".jpeg")) //thats pretty comprehensive...
                        {
                            //build new output path based on input name
                            string[] nameSplit = p.Split('\\');
                            string rawName = nameSplit[nameSplit.Length - 1].Split('.')[0];

                            string[] outputSplit = outputPath.Split('\\');
                            string newOutput = "";
                            for (int i = 0; i < outputSplit.Length - 1; i++)
                            {
                                newOutput += outputSplit[i] + "\\";
                            }
                            newOutput += rawName + ".txt";

                            BuildMap(p, dictPath, newOutput);
                        }
                        else
                            incompatibleCt++;
                    }
                    WriteHighlight("Skipped processing " + incompatibleCt + " incompatible files");
                }
                else //no input
                {
                    WriteError("Couldn't load input file '" + imagePath + "'");
                }
            }
            else
                BuildMap(imagePath, dictPath, outputPath);
        }

        static void BuildMap(string imagePath, string dictPath, string outputPath)
        {
            Console.WriteLine("Building map '" + imagePath + "' to '" + outputPath + "' with '" + dictPath + "'");

            if (!File.Exists(dictPath))
                WriteError("Dictionary file '" + dictPath + "' does not exist");
            else
                LoadDict(dictPath);

            Bitmap image = new Bitmap(imagePath);

            string output = image.Width + "," + image.Height + "\n";

            int unknownColors = 0;
            for (int j = 0; j < image.Height; j++)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    Color pixelColor = image.GetPixel(i, j);
                    int colorCode = GetColorCode(pixelColor.R, pixelColor.G, pixelColor.B);
                    if (!colorDict.ContainsKey(colorCode))
                    {
                        unknownColors++;
                        continue;
                    }
                    char c = colorDict[colorCode];
                    output += c;
                }
            }

            if (unknownColors > 0)
                WriteError(unknownColors + " unknown pixel(s) were skipped, map will likely parse incorrectly");

            File.WriteAllText(outputPath, output);
            WriteSuccess("Complete! " + output.Length + " characters -> '" + outputPath + "'");

        }

        static void LoadDict(string dictPath)
        {
            if (currentDict.Equals(dictPath))
                return;

            colorDict.Clear();
            string[] lines = File.ReadAllLines(dictPath);

            foreach (string l in lines)
            {
                if (l.StartsWith("#"))
                    continue;

                string[] firstSplit = l.Split('=');
                string[] colorSplit = firstSplit[0].Split(',');

                int r = Convert.ToInt32(colorSplit[0]);
                int g = Convert.ToInt32(colorSplit[1]);
                int b = Convert.ToInt32(colorSplit[2]);
                char c = firstSplit[1].ToCharArray()[0];

                colorDict.Add(GetColorCode(r, g, b), c);
            }
            Console.WriteLine("Dictionary built successfully");

        }

        static int GetColorCode(int r, int g, int b)
        {
            return r * 1000000 + g * 1000 + b;
        }

        static void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
        }
        static void WriteHighlight(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
