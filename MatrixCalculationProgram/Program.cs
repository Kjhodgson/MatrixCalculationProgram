using System;
using System.Diagnostics;

namespace MatrixCalculationProgram
{
    class Program
    {
        private static int numRows = 500;
        private static int numCols = 400;
        static void Main(string[] args)
        {
            Console.WriteLine("This program will run a matrix calculations on a single 2-dimensional array.\n");
            Console.WriteLine("This is the Matrix to be calculated.");

            int[,] array = new int[numRows, numCols];
            Console.Write(numRows + ": Number of Rows\n");
            Console.Write(numCols + ": Number of Cols\n");
            var sw = new Stopwatch();

            Random rand = new Random();

            // Generating the array and displaying
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    array[i, j] = rand.Next(1, 3);
                    Console.Write(string.Format("{0} ", array[i, j]));
                }
                Console.Write("\n");
            }
            sw.Start();
            Console.WriteLine("Now running the matrix calculation....");

            MatrixCalculationProgram(array);
            sw.Stop();
            Console.WriteLine("\nThe calculation took {0} (ms)", sw.ElapsedMilliseconds);
            decimal timeSeconds = decimal.Divide((decimal)sw.ElapsedMilliseconds, (decimal)1000);
            Console.WriteLine("Time in seconds: {0}", timeSeconds);
            Console.WriteLine("\nThe program is done, please press a key to exit!");
            Console.ReadKey();

        }

        private static void MatrixCalculationProgram(int[,] array)
        {
            Console.WriteLine("\nCloning the array.....");
            int[,] copiedArray = (int[,])array.Clone();
            int sumRow;
            int sumCol;
            Console.WriteLine("Beginning the calculations.");
            // will be doing row by column to obtain a single cell
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    sumRow = 0;
                    sumCol = 0;
                    for (int k = 0; k < numRows; k++)
                    {
                        sumRow += array[k, j];
                    }
                    for (int l = 0; l < numCols; l++)
                    {
                        sumCol += array[i, l];
                    }
                    copiedArray[i, j] = sumCol * sumRow;
                    Console.Write(string.Format("{0} ", copiedArray[i, j]));
                }
                Console.Write("\n");
            }
        }
    }
}
