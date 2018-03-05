using System;
using System.Threading;

namespace Laba3
{
    public static class Sort
    {

        /// <summary>
        /// Сортировка Пузырьком
        /// </summary>
        /// <param name="array">массив</param>
        public static void BubleSort<T>(T[] array) where T : IComparable<T>
        {
            for (int i = array.Length; i >=0; i--)
            {
                //Флаг того что мы что-то поменяли
                bool flag = true;
                
                for (int j = 1; j < i; j++)
                {
                    if (array[j-1].CompareTo(array[j])>=0)
                    {
                        T tmp = array[j - 1];
                        array[j - 1] = array[j];
                        array[j] = tmp;
                        flag = false;
                    }
                }
                
                //выход, если мы ничего не изменили
                if(flag)
                    break;
            }
        }


        
        /// <summary>
        /// Быстрая сортировка
        /// </summary>
        /// <param name="array">Массив значений</param>
        /// <param name="begin">Начало подмножества</param>
        /// <param name="end">Конец подмножества</param>
        /// <typeparam name="T"></typeparam>
        private static void QuickSort<T>( T[] array, int begin, int end) where T : IComparable<T>
        {
            int i = begin, j = end;
            T mid = array[(begin+end)/2];

            if (begin >= end)
            {
                return;
            }

            while (i<=j)
            {
                while (array[i].CompareTo(mid)<0)
                {
                    i++;
                }

                while (array[j].CompareTo(mid)>0)
                {
                    j--;
                }

                if (i <= j)
                {
                    T tmp = array[j];
                    array[j] = array[i];
                    array[i] = tmp;
                    i++;
                    j--;
                }
            }
            
            QuickSort(  array, i, end);
            QuickSort(  array, begin, j);
            
        }

        /// <summary>
        /// Быстрая сортировка
        /// </summary>
        /// <param name="array">массив</param>
        /// <typeparam name="T"></typeparam>
        public static void QuickSort<T>( T[] array) where T : IComparable<T>
        {
            QuickSort(  array, 0, array.Length-1);
        }

        /// <summary>
        /// Сортировка вставками
        /// </summary>
        /// <param name="array">Массив</param>
        /// <typeparam name="T"></typeparam>
        public static void InsertionSort<T>( T[] array) where T : IComparable<T>
        {
            for (int i = 1; i < array.Length; i++)
            {
                var tmp = array[i];
                var location = i - 1;
                //              1   1            1                   1
                while (location>=0 && (array[location].CompareTo(tmp)>0))
                {
                    array[location + 1] = array[location];
                    location--;
                    
                }
                array[location + 1] = tmp;
            }
        }
        
    }
}