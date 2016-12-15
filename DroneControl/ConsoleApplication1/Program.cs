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
            int[,] connectieArray = new int[15, 6]{
                {1, 5, 6, -1, -1, -1  },
                {0, 6, 2, -1, -1, -1  },
                {1, 7, 3, -1, -1, -1  },
                {2, 8, 4, -1, -1, -1  },
                {3, 9, 8, -1, -1, -1  },
                {0, 6, 10, -1, -1, -1  },
                {1, 5, 11, 7, 10, 0 },
                {6, 2, 8, 12, -1, -1  },
                {7, 3, 9, 13, 4, 14  },
                {8, 4, 14, -1, -1, -1  },
                {5, 11, 6, -1, -1, -1  },
                {10, 6, 12, -1, -1, -1  },
                {11, 7, 13, -1, -1, -1  },
                {12, 8, 14, -1, -1, -1  },
                {13, 9, 8, -1, -1, -1  }
            };

            //0 - 1 - 2
            //|   |   |
            //3 - 4 - 5

            int[] start = { 6, 8, 4, 2, 0, 14, 12, 7, 3, 1, 11, 10, 5, 9, 13};

            int j = 0;
            bool isVerbonden = false;
            bool isSwapped = false;

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
             //   Console.WriteLine("bezig met" + m);

                for (int l=0; l < connectieArray.GetLength(1); l++)
                {
                    if (m + 1 != start.Length && connectieArray[start[m], l] == start[m + 1])
                    {
                        isVerbonden = true;
                        //Console.WriteLine("verbonden op true gezet met de volgende.");
                    }
                }
                if (isVerbonden)
                {
                    isVerbonden = false;
                }
                else
                {
                 //   Console.WriteLine("verbonden met volgende is false, bezig met snijpunten ophalen");
                    int[] snijpuntA1 = new int[4];
                    int[] snijpuntA1plus1 = new int[4];
                  //  Console.WriteLine("snijpunten:");
                    for (int l = 0; l < connectieArray.GetLength(1); l++)
                    {

                        snijpuntA1[l] = connectieArray[start[m], l];
                        //Console.WriteLine(snijpuntA1[l] + "snijpunt a1");
                        if (m+1 != start.Length)
                        {
                            snijpuntA1plus1[l] = connectieArray[start[m+1], l];
                           // Console.WriteLine(snijpuntA1plus1[l] + "snijpunt a1+1");
                        }
                    }

                    foreach (int v in snijpuntA1plus1)
                    {
                      //  Console.Write(v);
                    }
                    Console.Read();

                    for (int p = m+2; p < start.GetLength(0); p++)
                    {
                        
                    //    Console.WriteLine("bezig met kijken snijpunten a1 - bezig met " + p + " dat is " + start[p] + " in  de start array" );
                        for (int l = 0; l < snijpuntA1.GetLength(0); l++)
                        {
                            //Console.Write(p);
                            //Console.Write(l);
                            if (start[p] == snijpuntA1[l])
                            {
                                
                             //   Console.WriteLine("snijpunt gevonden (" + p + ", in  start  " + start[p] + ")");
                                for(int k = 0; k < snijpuntA1.GetLength(0); k++)
                                {
                                    if (!(p + 1 >= start.Length) && start[p + 1] == snijpuntA1plus1[k])
                                    {
                                    //    Console.WriteLine(" startp+1 is gelijk aan snijpunta1+1, aan het swappen. start[P+1] =" + start[p + 1] + " snijpuntA1plus1[l] = " + snijpuntA1plus1[l]);
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
                                      
                                            Console.Read();
                                            isSwapped = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (p+1 == start.Length && !isSwapped)
                        {
                            int q = m;
                            int r = p;
                            while (q + 1 < r+1)
                            {
                                //Console.Write(q);
                                //Console.Write(r);
                                SwapInts(start, r, q + 1);
                                q++;
                                r--;

                                Console.ReadLine();
                            }
                        }
                    }
                    isSwapped = false;
                    
                }

                foreach (int v in start)
                {
                    Console.Write(v + "-");
                }
                Console.ReadLine();
            }

   
            Console.ReadLine();
            
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
            