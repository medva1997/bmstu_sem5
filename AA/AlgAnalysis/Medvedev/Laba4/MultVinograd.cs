using System;
using System.Threading;

namespace Laba4
{
    public class MultVinograd
    {
        private readonly double[,] _firstMatrix;
        private readonly double[,] _secondMatrix;
        private readonly double[,] _resultMatrix;
        private readonly double[] _rowFactor;
        private readonly double[] _colFactor;
        private readonly int _startIndex;
        private readonly int _endIndex;

        public MultVinograd(double[,] firstMatrix, double[,] secondMatrix, double[,] resultMatrix, double[] rowFactor, double[] colFactor, int startIndex, int endIndex)
        {
            _firstMatrix = firstMatrix;
            _secondMatrix = secondMatrix;
            _resultMatrix = resultMatrix;
            _rowFactor = rowFactor;
            _colFactor = colFactor;
            _startIndex = startIndex;
            _endIndex = endIndex;
        }

        public void Run()
        {
            
            //Console.WriteLine($"VinoThread {Thread.CurrentThread.ManagedThreadId} calc from {_startIndex} to {_endIndex}");
            
            
            int colCount = _secondMatrix.GetLength(0);
            int halfRowCount = _secondMatrix.GetLength(0)/2;
            
            bool flag = colCount % 2 == 1;
            for (int i = _startIndex; i < _endIndex; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    var buffer = (flag ? _firstMatrix[i, colCount - 1] * _secondMatrix[colCount - 1, j] : 0);
                    buffer -= _rowFactor[i] + _colFactor[j];
                    for (int k = 0; k < halfRowCount; k++)
                    {
                        buffer +=(_firstMatrix[i, 2 * k] + _secondMatrix[2 * k + 1, j]) *
                                 (_firstMatrix[i, 2 * k + 1] + _secondMatrix[2 * k, j]);
                
                    }
                    _resultMatrix[i, j] = buffer;
                }
            }
            //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} finished.");
        }
    }
}