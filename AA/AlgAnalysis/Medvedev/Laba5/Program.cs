using System;

namespace Laba5
{
    class Program
    {
        public static void BubleSort(int[] array)
        {
            for (int i = array.Length; i >=0; i--)
            {
                //Флаг того что мы что-то поменяли
                bool flag = true;
                
                for (int j = 1; j < i; j++)
                {
                    if (array[j-1]>=array[j])
                    {
                        int tmp = array[j - 1];
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
        static void Main(string[] args)
        {
            int[] array = new int[] {50, 25, 70, 36, 1, 0, 5};
            BubleSort(array);
            foreach (int val in array)
            {
                Console.Write($"{val}   ");
            }
            Console.WriteLine();
            
            Console.WriteLine("Hello World!");
        }
    }
}