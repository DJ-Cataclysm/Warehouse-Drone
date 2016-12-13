using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {

            //a(0) b(1) c(2) d(3) e(4) f(5)
            int[,] connectieArray = new int[6, 3]{
                {1, 3, -1},
                {0, 2, 4},
                {1, 5, -1},
                {0, 4, -1},
                {3, 1, 5},
                {4, 2, -1}
            };

            //f b a d e c 

            int[] start = { 5, 1, 4, 2, 3, 0 };

            int counter = 0;
            int v1plus1 = 0;
            int j = 0;
            bool isVerbonden = false;

            //Checkverbonden
            //  Ja -> doorgaan
            //  Nee {
            //      A1: Haal snijpunten op
            //      A1+1: Haal snijpunten i+1 op
            //        Voor elk getal na i+1 {
            //          Heeft snijpunt met A1?
            //            Nee -> ga verder door lijst
            //            Ja {
            //              Heeft volgend getal snijpunt met A1+1
            //                Ja -> Getal = J
            //                Verwissel J en i+1
            //                Nee -> Herhaal

            for (int m = 0; m < start.GetLength(0); m++)
            {
                Console.WriteLine("bezig met" + m);

                for (int l=0; l < connectieArray.GetLength(1); l++)
                {
                    if (m + 1 != start.Length && connectieArray[start[m], l] == start[m + 1])
                    {
                        isVerbonden = true;
                        Console.WriteLine("verbonden op true gezet met de volgende.");
                    }
                }
                if (isVerbonden)
                {
                    isVerbonden = false;
                }
                else
                {
                    Console.WriteLine("verbonden met volgende is false, bezig met snijpunten ophalen");
                    int[] snijpuntA1 = new int[3];
                    int[] snijpuntA1plus1 = new int[3];
                    Console.WriteLine("snijpunten:");
                    for (int l = 0; l < connectieArray.GetLength(1); l++)
                    {

                        snijpuntA1[l] = connectieArray[start[m], l];
                        Console.WriteLine(snijpuntA1[l] + "snijpunt a1");
                        if (m+1 != start.Length)
                        {
                            snijpuntA1plus1[l] = connectieArray[start[m+1], l];
                            Console.WriteLine(snijpuntA1plus1[l] + "snijpunt a1+1");
                        }
                    }

                    foreach (int v in snijpuntA1plus1)
                    {
                        Console.Write(v);
                    }
                    Console.Read();

                    for (int p = m+2; p < start.GetLength(0); p++)
                    {
                        Console.WriteLine("bezig met kijken snijpunten a1 - bezig met " + p + " dat is " + start[p] + " in  de start array" );
                        for (int l = 0; l < snijpuntA1.GetLength(0); l++)
                        {
                            //Console.Write(p);
                            //Console.Write(l);
                            if (start[p] == snijpuntA1[l])
                            {
                                
                                Console.WriteLine("snijpunt gevonden (" + p + ", in  start  " + start[p] + ")");
                                for(int k = 0; k < snijpuntA1.GetLength(0); k++)
                                {
                                    if (!(p + 1 >= start.Length) && start[p + 1] == snijpuntA1plus1[k])
                                    {
                                        Console.WriteLine(" startp+1 is gelijk aan snijpunta1+1, aan het swappen. start[P+1] =" + start[p + 1] + " snijpuntA1plus1[l] = " + snijpuntA1plus1[l]);
                                        j = start[p];
                                        int q = m;
                                        int r = p;
                                        while (q + 1 < r)
                                        {
                                            //Console.Write(q);
                                            //Console.Write(r);
                                            SwapInts(start, r, q + 1);
                                            q++;
                                            r--;
                                            foreach (int v in start)
                                            {
                                                Console.Write(v);
                                            }
                                            Console.Read();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            foreach (int v in start)
            {
                Console.Write(v);
            }
            Console.Read();
        }

        static void SwapInts(int[] array, int position1, int position2)
        {
            //
            // Swaps elements in an array.
            //
            int temp = array[position1]; // Copy the first position's element
            array[position1] = array[position2]; // Assign to the second element
            array[position2] = temp; // Assign to the first element
        }
    }
}
            