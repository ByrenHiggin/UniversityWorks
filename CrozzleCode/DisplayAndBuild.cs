using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROZZLEGENERATOR
{
    static class DisplayAndBuild
    {
        public static int easyScore = 0;

        public static void PrintArray(ref char[,] BuiltCrozzle, vec2 WorldDImensions)
        {
            
            Console.Write("  ");
            for (int y = 0; y < WorldDImensions.y; y++)
            {
                Console.Write("{0,2}", y);

            }
            Console.Write("\n");
            for (int y = 0; y < WorldDImensions.y; y++)
            {
                Console.Write("{0,3}", y);
                for (int x = 0; x < WorldDImensions.x; x++)
                {
                    if (BuiltCrozzle[y, x] == '\0')
                    {
                        Console.Write("# ");
                    }
                    else
                    {
                        PrintColouredTect(BuiltCrozzle[y, x]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }


                }
                Console.Write('\n');
            }
            Console.WriteLine("Score of {0}", easyScore);

        }

        public static void PrintColouredTect(char c)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0} ", c);
        }

        public static void LogMatches(ref List<Dictionary<char, Wordmatch>> ListOfMatches, ref List<WordClass> WordList)
        {
            string folderName = AppDomain.CurrentDomain.BaseDirectory;
            string pathstring = System.IO.Path.Combine(folderName, "LogEasy");
            System.IO.Directory.CreateDirectory(pathstring);
            string filename = "logfile.txt";
            pathstring = System.IO.Path.Combine(pathstring, filename);
            System.IO.StreamWriter file = new System.IO.StreamWriter(pathstring, true);
            // WriteAllText creates a file, writes the specified string to the file, 
            // and then closes the file.    You do NOT need to call Flush() or Close().
            using (file)
            {
                for (int i = 0; i < ListOfMatches.Count; i++)
                {
                    foreach (var c in ListOfMatches[i])
                    {
                        string[] x = c.Value.ToString().Split(' ');
                        int numone, numtwo;
                        int.TryParse(x[0], out numone);
                        int.TryParse(x[1], out numtwo);

                        file.WriteLine("Wordlist word " + WordList[numtwo].StringVal + " and Word " + WordList[numone].StringVal + " Share Character:  " + c.Key.ToString() + " Score of " + (WordList[numone].StringVal.Length + WordList[numtwo].StringVal.Length));
                    }
                }
            }
            file.Close();

        }


    }
}
