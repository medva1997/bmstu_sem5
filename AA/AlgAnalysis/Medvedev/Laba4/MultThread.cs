using System;
using System.ComponentModel;
using System.Threading;

namespace Laba4
{
    public class MultThread
    {
        private double[,] firstMatrix;
        private double[,] secondMatrix;
        private double[,] resultMatrix;
        private int startIndex;
        private int endIndex;

        public MultThread(double[,] firstMatrix, double[,] secondMatrix, double[,] resultMatrix, int startIndex, int endIndex)
        {
            this.firstMatrix = firstMatrix;
            this.secondMatrix = secondMatrix;
            this.resultMatrix = resultMatrix;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
            
        }

        public void Run()
        {
            //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} calc from {startIndex} to {endIndex}");
            
            int colCount= secondMatrix.GetLength(1);

            for (int i = startIndex; i < endIndex; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < secondMatrix.GetLength(0); k++)
                    {
                        sum += firstMatrix[i, k] * secondMatrix[k, j];

                    }

                    resultMatrix[i, j] = sum;

                }
                
            }
            //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} finished.");
        }
    }
}