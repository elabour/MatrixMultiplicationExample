using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplicationExample
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int[,] matrixA = GenerateRandomMatrix(1000, 1000);
            int[,] matrixB = GenerateRandomMatrix(1000, 1000);

            // Sequential matrix multiplication
            Console.WriteLine("Sequential Matrix Multiplication:");
            DateTime sequentialStart = DateTime.Now;
            int[,] resultSequential = MultiplyMatricesSequential(matrixA, matrixB);
            TimeSpan sequentialDuration = DateTime.Now - sequentialStart;
            Console.WriteLine($"Execution time: {sequentialDuration.TotalMilliseconds} ms");

            // Parallel matrix multiplication
            Console.WriteLine("\nParallel Matrix Multiplication:");
            DateTime parallelStart = DateTime.Now;
            int[,] resultParallel = MultiplyMatricesParallel(matrixA, matrixB);
            TimeSpan parallelDuration = DateTime.Now - parallelStart;
            Console.WriteLine($"Execution time: {parallelDuration.TotalMilliseconds} ms");

            // Verify that the results are the same
            VerifyResults(resultSequential, resultParallel);

            Console.ReadLine();
        }


        static int[,] MultiplyMatricesSequential(int[,] matrixA, int[,] matrixB)
        {
            int rowsA = matrixA.GetLength(0);
            int colsA = matrixA.GetLength(1);
            int colsB = matrixB.GetLength(1);

            int[,] result = new int[rowsA, colsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    for (int k = 0; k < colsA; k++)
                    {
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }

            return result;
        }

        static int[,] MultiplyMatricesParallel(int[,] matrixA, int[,] matrixB)
        {
            int rowsA = matrixA.GetLength(0);
            int colsA = matrixA.GetLength(1);
            int colsB = matrixB.GetLength(1);

            int[,] result = new int[rowsA, colsB];
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Environment.ProcessorCount;
          
            Parallel.For(0, rowsA,options, i =>
            {
                for (int j = 0; j < colsB; j++)
                {
                    for (int k = 0; k < colsA; k++)
                    {
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            });

            return result;
        }

        static void VerifyResults(int[,] resultSequential, int[,] resultParallel)
        {
            int rows = resultSequential.GetLength(0);
            int cols = resultSequential.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (resultSequential[i, j] != resultParallel[i, j])
                    {
                        Console.WriteLine("Verification failed: Results do not match!");
                        return;
                    }
                }
            }

            Console.WriteLine("Verification passed: Results match.");
        }

        static int[,] GenerateRandomMatrix(int rows, int cols)
        {
            Random rand = new Random();
            int[,] matrix = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = rand.Next(1, 100); // Adjust the range based on your requirements
                }
            }

            return matrix;
        }

    }
}
