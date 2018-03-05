using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Laba4
{
    public class Multiplication
    {
        public static double[,] BaseMultiplication(double[,] matrix1, double[,] matrix2)
        {
            int resRow = matrix1.GetLength(0);
            int resCol = matrix2.GetLength(1);
            int kk = matrix2.GetLength(0);
            double[,] resultmatrix = new double[resRow, resCol];

            for (int i = 0; i < resRow; i++)
            {
                for (int j = 0; j < resCol; j++)
                {
                    for (int k = 0; k < kk; k++)
                    {
                        resultmatrix[i, j] = resultmatrix[i, j] + matrix1[i, k] * matrix2[k, j];
                    }
                }
            }

            return resultmatrix;
        }

        public static double[,] VinogradMultiplication(double[,] matrix1, double[,] matrix2)
        {
            int resRow = matrix1.GetLength(0);
            int resCol = matrix2.GetLength(1);
            int kk = matrix2.GetLength(0);
            double[,] resultmatrix = new double[resRow, resCol];

            double[] rowFactor = new double[resRow];
            double[] colFactor = new double[resCol];

            for (int i = 0; i < resRow; i++)
            {
                rowFactor[i] = matrix1[i, 0] * matrix1[i, 1];
                for (int j = 1; j < kk / 2; j++)
                {
                    rowFactor[i] = rowFactor[i] + matrix1[i, 2 * j] * matrix1[i, 2 * j + 1];
                }
            }

            for (int i = 0; i < resCol; i++)
            {
                colFactor[i] = matrix2[0, i] * matrix2[1, i];
                for (int j = 1; j < kk / 2; j++)
                {
                    colFactor[i] = colFactor[i] + matrix2[2 * j, i] * matrix2[2 * j + 1, i];
                }
            }

            for (int i = 0; i < resRow; i++)
            {
                for (int j = 0; j < resCol; j++)
                {
                    resultmatrix[i, j] = -rowFactor[i] - colFactor[j];
                    for (int k = 0; k < kk / 2; k++)
                    {
                        resultmatrix[i, j] = resultmatrix[i, j] +
                                             (matrix1[i, 2 * k] + matrix2[2 * k + 1, j]) *
                                             (matrix1[i, 2 * k + 1] + matrix2[2 * k, j]);
                    }
                }
            }

            if (kk % 2 == 1)
            {
                for (int i = 0; i < resRow; i++)
                {
                    for (int j = 0; j < resCol; j++)
                    {
                        resultmatrix[i, j] = resultmatrix[i, j] + matrix1[i, kk - 1] * matrix2[kk - 1, j];
                    }
                }
            }

            return resultmatrix;
        }

        public static double[,] BetterVinogradMultiplication(double[,] matrix1, double[,] matrix2)
        {
            int resRow = matrix1.GetLength(0); //m
            int resCol = matrix2.GetLength(1); //n

            int n = matrix2.GetLength(0);
            int kk = matrix2.GetLength(0) / 2;
            bool flag = n % 2 == 1;

            double[,] resultmatrix = new double[resRow, resCol];
            double[] rowFactor = new double[resRow];
            double[] colFactor = new double[resCol];

            for (int i = 0; i < resRow; i++)
            {
                rowFactor[i] = matrix1[i, 0] * matrix1[i, 1];
                for (int j = 1; j < kk; j++)
                {
                    rowFactor[i] += matrix1[i, 2 * j] * matrix1[i, 2 * j + 1];
                }
            }


            for (int i = 0; i < resCol; i++)
            {
                colFactor[i] = matrix2[0, i] * matrix2[1, i];
                for (int j = 1; j < kk; j++)
                {
                    colFactor[i] += matrix2[2 * j, i] * matrix2[2 * j + 1, i];
                }
            }

            double buffer;
            for (int i = 0; i < resRow; i++)
            {
                for (int j = 0; j < resCol; j++)
                {
                    buffer = (flag ? matrix1[i, n - 1] * matrix2[n - 1, j] : 0);
                    buffer -= rowFactor[i] + colFactor[j];
                    for (int k = 0; k < kk; k++)
                    {
                        buffer += (matrix1[i, 2 * k] + matrix2[2 * k + 1, j]) *
                                  (matrix1[i, 2 * k + 1] + matrix2[2 * k, j]);
                    }

                    resultmatrix[i, j] = buffer;
                }
            }

            return resultmatrix;
        }

        public static double[,] ParallelBaseMultiplication(double[,] matrix1, double[,] matrix2, int nThreads)
        {
            int resRow = matrix1.GetLength(0);
            int resCol = matrix2.GetLength(1);
            double[,] resultmatrix = new double[resRow, resCol];
            List<Thread> threads = new List<Thread>();
            int rowsForThread = resRow / nThreads;

            int firstInd = 0;
            for (int i = 0; i < nThreads; i++)
            {
                int lastInd = firstInd + rowsForThread;
                if (i == nThreads - 1)
                    lastInd = resRow;

                MultThread tmp = new MultThread(matrix1, matrix2, resultmatrix, firstInd, lastInd);
                threads.Add(new Thread(new ThreadStart(tmp.Run)));
                firstInd = lastInd;
            }

            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Console.WriteLine("Finish");
            return resultmatrix;
        }

        public static double[,] ParallelVinogradMultiplication(double[,] matrix1, double[,] matrix2, int nThreads)
        {
            int resRow = matrix1.GetLength(0); //m
            int resCol = matrix2.GetLength(1); //n

            int n = matrix2.GetLength(0);
            int kk = matrix2.GetLength(0) / 2;


            double[,] resultmatrix = new double[resRow, resCol];
            double[] rowFactor = new double[resRow];
            double[] colFactor = new double[resCol];

            for (int i = 0; i < resRow; i++)
            {
                rowFactor[i] = matrix1[i, 0] * matrix1[i, 1];
                for (int j = 1; j < kk; j++)
                {
                    rowFactor[i] += matrix1[i, 2 * j] * matrix1[i, 2 * j + 1];
                }
            }


            for (int i = 0; i < resCol; i++)
            {
                colFactor[i] = matrix2[0, i] * matrix2[1, i];
                for (int j = 1; j < kk; j++)
                {
                    colFactor[i] += matrix2[2 * j, i] * matrix2[2 * j + 1, i];
                }
            }

            List<Thread> threads = new List<Thread>();
            int rowsForThread = resRow / nThreads;
            int firstInd = 0;

            for (int i = 0; i < nThreads; i++)
            {
                int lastInd = firstInd + rowsForThread;
                if (i == nThreads - 1)
                    lastInd = resRow;

                MultVinograd tmp = new MultVinograd(matrix1, matrix2, resultmatrix, rowFactor, colFactor, firstInd,
                    lastInd);

                threads.Add(new Thread(new ThreadStart(tmp.Run)));
                firstInd = lastInd;
            }

            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Console.WriteLine("Finish");

            return resultmatrix;
        }
    }
}