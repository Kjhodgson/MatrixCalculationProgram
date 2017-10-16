using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MatrixCalculationProgram
{
    class Program
    {
        private static int numRows = 800;
        private static int numCols = 800;
        static int numberOfCores = Environment.ProcessorCount;

        static void Main(string[] args)
        {
            Console.WriteLine("This program will run a matrix calculations on a single 2-dimensional array.\n");
            Console.WriteLine("This is the Matrix to be calculated:");

            int[,] array = new int[numRows, numCols];
            int[,] copiedArray1 = (int[,])array.Clone();
            int[,] copiedArray2 = (int[,])array.Clone();
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
                }
            }
            //PrintArray(array);
            
            Console.WriteLine("\nNow running the matrix calculation without parallelism....");
            sw.Start();
            copiedArray1 = (int[,])MatrixCalculationProgram(array, copiedArray1).Clone();
            sw.Stop();
            //PrintArray(copiedArray1);
            Console.WriteLine("\nThe calculation took {0} (ms)", sw.ElapsedMilliseconds);
            decimal timeSeconds = decimal.Divide((decimal)sw.ElapsedMilliseconds, (decimal)1000);
            Console.WriteLine("Time in seconds: {0}", timeSeconds);
           
            Console.WriteLine("\nNow running the matrix calculation with parallelism....");
            sw.Start();
            copiedArray2 = (int[,])MatrixCalculationParallelProgram(array,copiedArray2).Clone();
            sw.Stop();
            //PrintArray(copiedArray2);
            Console.WriteLine("\nThe calculation took {0} (ms)", sw.ElapsedMilliseconds);
            timeSeconds = decimal.Divide((decimal)sw.ElapsedMilliseconds, (decimal)1000);
            Console.WriteLine("Time in seconds: {0}", timeSeconds);

            Console.WriteLine("\nNow running tests to make sure that the arrays are equal.");

            Arraytest(copiedArray1, copiedArray2);

            Console.WriteLine("\nThe program is done, please press a key to exit!");
            Console.ReadKey();
        }


        private static int[,] MatrixCalculationProgram(int[,] array, int[,] copiedArray1)
        {           
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
                    copiedArray1[i, j] = sumCol * sumRow;             
                }
            }
            return (copiedArray1);            
        }

        private static int[,] MatrixCalculationParallelProgram(int[,] array, int[,] copiedArray2)
        {
            Console.WriteLine("\nCloning the array.....");

            Task[] tasks = new Task[numberOfCores];

            int coreRows = numRows / numberOfCores;

            int[] sumRows = new int[numRows];
            int[] sumCols = new int[numCols];

            Console.WriteLine("num coreRows: {0}", coreRows);

            Console.WriteLine("Beginning the calculations.");
            for (int core = 0; core < numberOfCores; core++)
            {
                int coreNumber = core;
                Console.WriteLine("Working on Core: {0}", coreNumber);

                tasks[coreNumber] = Task.Factory.StartNew(() =>
                {
                    
                    int rowSegmentUpperBound = (coreNumber + 1) * coreRows;

                    int rowSegmentLowerBound = coreNumber * coreRows;

                    for (int i = rowSegmentLowerBound; i < rowSegmentUpperBound; i++)
                    {
                        for (int j = 0; j < numCols; j++)
                        {
                            sumRows[coreNumber] = 0;
                            sumCols[coreNumber] = 0;
                            for (int k = 0; k < numRows; k++)
                            {
                                sumRows[coreNumber] += array[k, j];
                            }
                            for (int l = 0; l < numCols; l++)
                            {
                                sumCols[coreNumber] += array[i, l];
                            }
                            copiedArray2[i, j] = sumCols[coreNumber] * sumRows[coreNumber];
                        }
                    }
                });               
            }
            Task.WaitAll(tasks);
            return copiedArray2;
        }

        private static void Arraytest(int[,] copiedArray1, int[,] copiedArray2)
        {
            Console.WriteLine("\nNow beginning tests...");
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (copiedArray1[i, j] == copiedArray2[i, j])
                    { }
                    else
                    {
                        Console.WriteLine("Test Failed: The Arrays are not equal.\n");
                        break;
                    }                        
                }
                
            }
            Console.WriteLine("Test Passed: The arrays were equal.");
            Console.Write("\n");
        }

        private static void PrintArray(int[,] array)
        {
            //prints the array 
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    Console.Write(string.Format("{0} ", array[i, j]));
                }
                Console.Write("\n");
            }
        }

    }
}
