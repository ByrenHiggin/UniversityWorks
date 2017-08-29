using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROZZLEGENERATOR
{
    public class WordlistSorter
    {

        public static List<WordClass> SortWordList_Greedy(ref List<WordClass> WordList)
        {
            int CurrentMaxLength = 0;
            List<WordClass> NewList = new List<WordClass>();
            //Get Current Maximum Length
            foreach (WordClass x in WordList)
            {
                if (x.StringVal.Length > CurrentMaxLength)
                {
                    CurrentMaxLength = x.StringVal.Length;
                }

            }
            while (CurrentMaxLength != 0)
            {
                foreach (WordClass x in WordList)
                {
                    if (x.StringVal.Length == CurrentMaxLength)
                    {
                        NewList.Add(x);
                    }
                }
                CurrentMaxLength--;
            }
            return NewList;
        }

        /// <summary>
        /// BUILDS A WORD LIST FROM A STRING
        /// </summary>
        /// <param name="Wordlist"></param>
        public static void InsertToWorklist(ref List<WordClass> Wordlist)
        {
            string full = "AM,AND,ANTS,ASH,ASK,AT,ATRIUM,BEARS,BEE,BELT,BLEND,BREAK,BY,COPPER,DASH,EDIT,EXAMPLE,EXPORT,FAIR,FIX,GYPSY,HEARD,HUMAN,JEEP,KITE,MAST,MEAL,NO,NOD,OF,OH,ONE,OVERDRIVE,PEARLS,PI,PSYCHOLOGY,RANDOM,RED,SEEM,SHINE,SON,TAKE,TEA,TIGER,TO,TOO,TOYS,WEEK,YOU,ZOO";
            string[] SplitArray = full.Split(',');
            foreach (string x in SplitArray)
            {
                Wordlist.Add(new WordClass(x, false));
            }
        }
    }
}
