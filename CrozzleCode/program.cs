#define DEBUGMODE
#define RELEASE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;




namespace CROZZLEGENERATOR
{
    class Program
    {
#if(RELEASE)
        //VALUES TO BE IMPLEMENTED FROM CROZZLE_HEADER. DO NOT COPY THESE VALUES
        public static vec2 WorldDimensions = new vec2(10, 15);
        public const int One = 1;
        public static List<WordClass> WordList = new List<WordClass>();

#endif

        /// <summary>
        /// THE MAIN METHOD
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            List<Dictionary<char, Wordmatch>> ListOfMatches = new List<Dictionary<char, Wordmatch>>();
            char[,] BuiltCrozzle = new char[WorldDimensions.y, WorldDimensions.x];

            WordlistSorter.InsertToWorklist(ref WordList);
            WordList = WordlistSorter.SortWordList_Greedy(ref WordList);
            BuildWordConnections(ref WordList, ref ListOfMatches, WorldDimensions, ref BuiltCrozzle);
            ConnectWords(ref BuiltCrozzle, ref ListOfMatches, ref WordList, new vec2(WorldDimensions.x, WorldDimensions.y));
            DisplayAndBuild.PrintArray(ref BuiltCrozzle, WorldDimensions);
            DisplayAndBuild.LogMatches(ref ListOfMatches, ref WordList);
            Console.Read();
        }

        public static void ConnectWords(ref char[,] BuiltCrozzle, ref List<Dictionary<char, Wordmatch>> ListOfMatches, ref List<WordClass> WordList, vec2 WorldDimensions)
        {
            try
            {
                int lastplacedWordindex = 0;

                WordList[0].SetPos(WorldDimensions.x - WordList[0].StringVal.Length, WorldDimensions.x - WordList[0].StringVal.Length / 3, true);
                WordList[0].DisableWord();
                DisplayAndBuild.easyScore += WordList[0].StringVal.Length;
                InsertWordIntoChararray(WordList[0], ref BuiltCrozzle);

                for (int i = 0; i < ListOfMatches.Count; i++)
                {
                    foreach (KeyValuePair<char, Wordmatch> c in ListOfMatches[i])
                    {
                        vec2 wordindex = c.Value.WordIndexes();
                        vec2 charindex = c.Value.CharIndexes();

                        //Check that the word is not already used
                        if (WordList[wordindex.y].BoolVal == false)
                        {
                            //Get the shared character between the 2 words
                            char x = c.Key;
                            Debug.Assert(x != '\0');
                            bool flag = false;

#if(DEBUGMODE)
                            Debug.Assert(WordList[wordindex.y].StringVal != "");
                            Console.WriteLine("SCANNED TO WORD {0}", WordList[wordindex.y].StringVal);
#endif

                            //Check if there is not already 2 or more connecting words to the first word, and the first word has been placed
                            if (WordList[wordindex.x].ConnectingWords <= 1 && WordList[wordindex.x].BoolVal == true)
                            {
                                //Check Previous Word and make horizontal
                                if (WordList[wordindex.x].OrientHorizontal)
                                {
                                    if (CheckWordBounds(WordList[wordindex.x], WordList[wordindex.y], charindex))
                                    {
                                        int SecondWordXPosition = WordList[wordindex.x].position.x + charindex.x;
                                        int SecondWordYPosition = WordList[wordindex.x].position.y - charindex.y;
                                        bool InvertedOrientation = !WordList[wordindex.x].OrientHorizontal;

                                        if (WordList[wordindex.y].SetPos(SecondWordXPosition, SecondWordYPosition, InvertedOrientation))
                                        {
                                            if (CheckWordInsertion(ref BuiltCrozzle, WordList[wordindex.y], charindex))
                                            {
#if(DEBUGMODE)
                                                Console.WriteLine("INSERTING STRING VALUE {0} INTO CROZZLE", WordList[wordindex.y].StringVal);
                                                Debug.Assert((WordList[wordindex.y].position.y + WordList[wordindex.y].StringVal.Length) < WorldDimensions.y);
#endif
                                                InsertWordIntoChararray(WordList[wordindex.y], ref BuiltCrozzle);
                                                DisplayAndBuild.easyScore += WordList[wordindex.y].StringVal.Length;
                                                flag = true;
                                            }
                                        }
                                    }
                                }
                                //MAKE A VERTICAL WORD
                                else
                                {
                                    if (CheckWordBounds(WordList[wordindex.x], WordList[wordindex.y], charindex))
                                    {

                                        int SecondWordXPosition = WordList[wordindex.x].position.x - charindex.y;
                                        int SecondWordYPosition = WordList[wordindex.x].position.y + charindex.x;
                                        bool InvertedOrientation = !WordList[wordindex.x].OrientHorizontal;

                                        if (WordList[wordindex.y].SetPos(SecondWordXPosition, SecondWordYPosition, InvertedOrientation))
                                        {
                                            if (CheckWordInsertion(ref BuiltCrozzle, WordList[wordindex.y], charindex))
                                            {
#if(DEBUGMODE)
                                                Console.WriteLine("INSERTING STRING VALUE {0} INTO CROZZLE", WordList[wordindex.y].StringVal);
                                                Debug.Assert((WordList[wordindex.y].position.x + WordList[wordindex.y].StringVal.Length) < WorldDimensions.x);
#endif
                                                InsertWordIntoChararray(WordList[wordindex.y], ref BuiltCrozzle);
                                                DisplayAndBuild.easyScore += WordList[wordindex.y].StringVal.Length;
                                                flag = true;
                                            }
                                        }
                                    }
                                }

#if(DEBUGMODE)
                                DisplayAndBuild.PrintArray(ref BuiltCrozzle, WorldDimensions);
#endif
                                if (flag)
                                {
                                    WordList[wordindex.x].ConnectingWords++;
                                    WordList[wordindex.y].ConnectingWords++;
                                    WordList[wordindex.y].DisableWord();
                                    lastplacedWordindex = wordindex.y;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
#if(DEBUGMODE)
                Console.WriteLine("EXCEPTION DETECTED WHEN CONNECTING WORDS! ERROR MESSAGE {0}", e.Message);
#endif
            }
        }

        private static bool CheckWordBounds(WordClass ConnectedWordX, WordClass ConnectedWordY, vec2 Offsets)
        {
            bool flag = false;
            try
            {

                if (ConnectedWordX.BoolVal)
                {
                    if (ConnectedWordX.position.x - Offsets.x < One)
                    {
                        flag = true;
                    }
                }
                else
                {
                    if (ConnectedWordX.position.x + Offsets.x < One)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception e)
            {
#if(DEBUGMODE)
                Console.WriteLine("EXCEPTION DETECTED WITH WORD BOUNDS! AREA OUTSIDE BOUNDS PROBABLY CHECKED ERROR MESSAGE {0}", e.Message);
#endif
            }
            return flag;
        }

        private static bool CheckWordInsertion(ref char[,] BuiltCrozzle, WordClass word, vec2 charIndexToSkip)
        {

            int x = word.position.x;
            int y = word.position.y;
            int length = word.StringVal.Length;
            bool orientation = word.OrientHorizontal;
            int FirstWordIndex = charIndexToSkip.x;
            int SecondWordIndex = charIndexToSkip.y;
            bool flag = true;
            try
            {
                if (SecondWordIndex != default(int))
                {
                    //Check if there is a horizontal left to right
                    if (orientation)
                    {
                        if (SpacingCharCheck(ref BuiltCrozzle, new vec2(x, y)))
                        {
                            flag = false;
                        }
                        if (!IterationCharCheck(ref BuiltCrozzle, true, new vec2(x, y), SecondWordIndex, length))
                        {
                            flag = false;
                        }
                    }

                    //Check for a Vertical collision up and down    
                    else
                    {
                        if ((x + length < WorldDimensions.x && y > 0))
                        {
                            if (SpacingCharCheck(ref BuiltCrozzle, new vec2(x, y)))
                            {
                                flag = false;
                            }
                            if (!IterationCharCheck(ref BuiltCrozzle, false, new vec2(x, y), SecondWordIndex, length))
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            flag = false;
                        }

                    }
                }

                    //check when an index is = 0. 
                else
                {
                    if ((y + word.StringVal.Length) < WorldDimensions.y)
                    {
                        if (orientation)
                        {
                            for (int j = x; j < (x + charIndexToSkip.x); j++)
                            {
                                if (SpacingCharCheck(ref BuiltCrozzle, new vec2(x, y)))
                                {
                                    flag = false;
                                }
                                if (flag == false)
                                {
                                    break;
                                }
                                if (CheckCells(ref BuiltCrozzle, new vec2(j, x), true))
                                {
                                    flag = false;
                                }

                            }
                        }
                        else
                        {
                            //Check and run if the index of the game is 0
                            for (int j = y + 1; j < (y + length + 1); j++)
                            {

                                if (flag == false)
                                {
                                    break;
                                }
                                if (CheckCells(ref BuiltCrozzle, new vec2(j, x), true))
                                {
                                    flag = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return flag;

        }

        private static bool IterationCharCheck(ref char[,] BuiltCrozzle, bool horizontal, vec2 positionvec, int SecondWordIndex, int StringLength)
        {
            bool flag = true;
            int position = 0;
            if (horizontal)
                position = positionvec.x;
            else
                position = positionvec.y;

            for (int j = position; j < (position + SecondWordIndex); j++)
            {
                vec2 v = (horizontal) ? new vec2(j, positionvec.y) : new vec2(positionvec.x, j);
                if (CheckCells(ref BuiltCrozzle, v, horizontal))
                {
                    flag = false;
                    break;
                }
            }
            for (int j = position + SecondWordIndex + One; j < (position + StringLength); j++)
            {
                if (flag == false)
                {
                    break;
                }
                vec2 v = (horizontal) ? new vec2(j, positionvec.y) : new vec2(positionvec.x, j);
                if (CheckCells(ref BuiltCrozzle, v, horizontal))
                {
                    flag = false;
                }
            }
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BuiltCrozzle"></param>
        /// <param name="Posi"></param>
        /// <param name="horizontal"></param>
        /// <returns></returns>
        private static bool CheckCells(ref char[,] BuiltCrozzle, vec2 Posi, bool horizontal)
        {
            bool flag = false;
            try
            {

                if (Posi.x >= 0 && Posi.x < WorldDimensions.x && Posi.y >= 0 && Posi.y < WorldDimensions.x)
                {
                    if (horizontal)
                    {
                        if (Posi.x >= 1 && Posi.x < WorldDimensions.x && Posi.y >= 1 && Posi.y < WorldDimensions.x)
                        {
                            if (BuiltCrozzle[Posi.y - 1, Posi.x] != '\0' || BuiltCrozzle[Posi.y + 1, Posi.x] != '\0')
                            {
                                flag = true;
                            }
                        }
                        if (BuiltCrozzle[Posi.y, Posi.x] != '\0')
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        if (Posi.x >= 1 && Posi.x < WorldDimensions.x && Posi.y >= 1 && Posi.y < WorldDimensions.x)
                        {
                            if (BuiltCrozzle[Posi.y, Posi.x - 1] != '\0' || BuiltCrozzle[Posi.y, Posi.x + 1] != '\0')
                            {
                                flag = true;
                            }
                        }
                        if (BuiltCrozzle[Posi.y, Posi.x] != '\0')
                        {
                            flag = true;
                        }

                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("INDEX OUT OF RANGE! WORD IN LIST EXCEEDS MAXIMUM PLACEABLE LIMIT >> {0}", e.Message);
            }
            return flag;

        }

        private static bool SpacingCharCheck(ref char[,] BuiltCrozzle, vec2 Posi)
        {
            bool flag = false;
            try
            {
                if (Posi.x > 0)
                {
                    if (BuiltCrozzle[Posi.y, Posi.x - 1] != '\0')
                    {
                        flag = true;
                    }
                }
                if (Posi.y > 0)
                {
                    if (BuiltCrozzle[Posi.y - 1, Posi.x] != '\0')
                    {
                        flag = true;
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("INDEX OUT OF RANGE! POSITION ERROR >> {0}", e.Message);
            }
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        private static void BuildWordConnections(ref List<WordClass> WordList, ref List<Dictionary<char, Wordmatch>> ListOfMatches, vec2 WorldDimensions, ref char[,] BuiltCrozzle)
        {
            try
            {
                //A List of dictionaries, which contain the word index in the word list and the co-ordinates/orientation for said word. this can be used to build a 2Dimensional char array.
                ListOfMatches.Add(new Dictionary<char, Wordmatch>());
                for (int i = 0; i < WordList.Count; i++)
                {
                    for (int j = i + 1; j < WordList.Count; j++)
                    {
                        int cx = 0;
                        foreach (char c in WordList[i].StringVal)
                        {
                            int cy = 0;
                            foreach (char ci in WordList[j].StringVal)
                            {
                                if (c == ci)
                                {
                                    int cou = ListOfMatches.Count - 1;
                                    //Add a new word to the word list. 
                                    ListOfMatches[cou].Add(c, new Wordmatch(i, j, cx, cy));
                                    ListOfMatches[cou][c].CurrentConnectionScore = WordList[i].StringVal.Length + WordList[j].StringVal.Length;
                                    ListOfMatches.Add(new Dictionary<char, Wordmatch>());
                                }
                                cy++;
                            }
                            cx++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR ENCOUNTERED IN BUILDWORDSCONNECTIONS METHOD: {0}", e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void InsertWordIntoChararray(WordClass wordObject, ref char[,] BuiltCrozzle)
        {
            bool horizontal = wordObject.OrientHorizontal;
            int yco = wordObject.position.y;
            int xco = wordObject.position.x;
            string strarr = wordObject.StringVal;
            //Horizontal Word

            if (horizontal)
            {
                int x = 0;
                try
                {

                    foreach (char c in strarr)
                    {
                        BuiltCrozzle[yco, xco + x] = c;
#if(DEBUGMODE)
                        Console.WriteLine("INSERTING CHAR {0} INTO POSITION [{1},{2}]", c, yco, xco + x);
#endif
                        x++;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("TRIED PLACING HORIZONTAL WORD OUT OF RANGE IN INSERTWORDINTOCHARARRAY(). {0} PLACED AT {1}{2} RESULTED IN EXCEPTION {3}", strarr, xco + x, yco, e.Message);
                }
            }
            //Vertical Word
            else
            {
                int y = 0;
                try
                {

                    foreach (char c in strarr)
                    {
                        BuiltCrozzle[yco + y, xco] = c;
#if(DEBUGMODE)
                        Console.WriteLine("INSERTING CHAR {0} INTO POSITION [{1},{2}]", c, yco + y, xco);
#endif
                        y++;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("TRIED PLACING VERTICAL WORD OUT OF RANGE IN INSERTWORDINTOCHARARRAY(). {0} PLACED AT {1}{2} RESULTED IN EXCEPTION {3}", strarr, xco, yco + y, e.Message);
                }
            }
        }
    }

    //Structures
    public class WordClass
    {
        string _value;
        bool _flag, _orientHorizontal;
        int _ConnectingWords;

        public vec2 position;

        public WordClass(string sarg, bool barg)
        {
            this._value = sarg;
            this._flag = barg;
            position = new vec2(0, 0);
        }
        public int ConnectingWords
        {
            get { return _ConnectingWords; }
            set { _ConnectingWords = value; }
        }
        public string StringVal
        {
            get { return _value; }
        }
        public bool BoolVal
        {
            get { return _flag; }
        }
        public bool OrientHorizontal
        {
            get { return _orientHorizontal; }
            set { _orientHorizontal = value; }
        }
        public void DisableWord()
        {
            if (this._flag == false)
            {
                this._flag = true;
            }
        }
        public bool SetPos(int x, int y, bool Orient)
        {
            bool flag = false;
            if (x >= 0 && y >= 0)
            {
                this.position.x = x;
                this.position.y = y;
                this.OrientHorizontal = Orient;
                flag = true;
                Debug.Assert(!(x < 0) && !(y < 0));
            }
            else
            {
#if(DEBUGMODE)
                Console.WriteLine("SetPos Failed");
#endif
            }
            return flag;

        }
    }

    public class vec2
    {
        public int x, y;
        public vec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public vec2()
            : this(0, 0)
        {

        }
    }
    public class Wordmatch
    {
        int _FirstWordIndex, _SecondWordIndex;
        int _FirstCharIndex, _SecondCharIndex;


        int currentConnectionScore;

        public Wordmatch(int wordy, int wordx, int chary, int charx)
        {
            this._FirstWordIndex = wordy;
            this._SecondWordIndex = wordx;
            this._FirstCharIndex = chary;
            this._SecondCharIndex = charx;
        }
        public override string ToString()
        {
            return _FirstWordIndex.ToString() + " " + _SecondWordIndex.ToString();
        }
        public vec2 WordIndexes()
        {
            return new vec2(_FirstWordIndex, _SecondWordIndex);
        }
        public vec2 CharIndexes()
        {
            return new vec2(_FirstCharIndex, _SecondCharIndex);
        }

        public int CurrentConnectionScore
        {
            get { return currentConnectionScore; }
            set { currentConnectionScore = value; }
        }
    }
}
