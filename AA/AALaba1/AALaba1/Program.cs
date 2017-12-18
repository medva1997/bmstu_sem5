using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace AALaba1
{
    class Program
    { 
        /// <summary>
        /// Алгоритм Левенштейна с использованием матрицы
        /// </summary>
        /// <param name="str1">первая строка</param>
        /// <param name="m">длина первой строки</param>
        /// <param name="str2">вторая строка</param>
        /// <param name="n">длина второй строки</param>
        /// <returns>значение редакционного расстояния</returns>
        static int BaseMatrix(string str1, int m, string str2, int n)
        {
            //Создание матрицы
            int[,] matrix =new int[m + 1,n + 1];
            
            //Заполнение колонки и столбца
            for (int i = 0; i < m + 1; i++)
                matrix[i,0] = i;
            for (int i = 0; i < n + 1; i++)
                matrix[0,i] = i;
            
            //Заполнение всей остальной матрицы
            for (int i = 1; i < m + 1; i++)
            {
                for (int j = 1; j < n + 1; j++)
                {
                    int insert = matrix[i - 1, j] + 1;
                    int delete = matrix[i, j - 1] + 1;
                    int replace=matrix[i - 1, j - 1] + Match(str1[i - 1], str2[j - 1]);
                    
                    matrix[i,j] = Math.Min(delete, Math.Min(insert, replace));
                    //Console.Write(matrix[i,j]+"\t");
                }
                //Console.WriteLine();
            }
            
            return matrix[m, n];

        }
        
        /// <summary>
        /// Модифицированный алгоритм Левенштейна с использованием матрицы
        /// </summary>
        /// <param name="str1">первая строка</param>
        /// <param name="m">длина первой строки</param>
        /// <param name="str2">вторая строка</param>
        /// <param name="n">длина второй строки</param>
        /// <returns>значение редакционного расстояния</returns>
        static int ModifMatrix(string str1, int m, string str2, int n)
        {
            //Создание матрицы
            int[,] matrix =new int[m + 1,n + 1];
            
            //Заполнение колонки и столбца
            for (int i = 0; i < m + 1; i++)
                matrix[i,0] = i;
            for (int i = 0; i < n + 1; i++)
                matrix[0,i] = i;
            
            //Заполнение всей остальной матрицы
            for (int i = 1; i < m + 1; i++)
            {
                for (int j = 1; j < n + 1; j++)
                {
                    int insert = matrix[i - 1, j] + 1;
                    int delete = matrix[i, j - 1] + 1;
                    int replace=matrix[i - 1, j - 1] + Match(str1[i - 1], str2[j - 1]);
                    //минимум по трем базовым операциям 
                    int tmpres = Math.Min(delete, Math.Min(insert, replace));
                    
                    //перестановка
                    if (i > 1 && j > 1 && 
                        str1[i - 1] == str2[j - 2] &&
                        str1[i - 2] == str2[j - 1])
                    {
                        
                        int swap = matrix[i - 2, j - 2] + 1;
                        //обновление результата
                        tmpres= Math.Min(swap, tmpres);
                    }
                    //фиксация результата
                    matrix[i, j] = tmpres;
                }
            }
            return matrix[m, n];

        }
        
       

        /// <summary>
        /// Алгоритм Левенштейна с использованием рекуррентной формулы
        /// </summary>
        /// <param name="str1">первая строка</param>
        /// <param name="str2">вторая строка</param>
        /// <param name="i">номер символа в первой строке(длина)</param>
        /// <param name="j">номер символа во второй строке(длина)</param>
        /// <returns>значение редакционного расстояния</returns>
        static int RecurAlgo(string str1, string str2, int i, int j)
        {
          
            if (i == 0 && j == 0) return 0;
            
            if (i>0 && j==0)
            {
                return i;
            }
            
            if (i==0 && j>=0)
            {
                return j;
            }

            
            int delete = RecurAlgo(str1, str2, i, j - 1) + 1;
            int insert = RecurAlgo(str1, str2, i - 1, j) + 1;
            int replace = RecurAlgo(str1, str2, i - 1, j - 1) + Match(str1[i - 1], str2[j - 1]);
            return Math.Min(delete, Math.Min(insert, replace));
        }

        /// <summary>
        /// равна нулю, если a=b и единице в противном случае
        /// </summary>
        /// <param name="c1">символ первой строки</param>
        /// <param name="c2">символ второй строки</param>
        /// <returns></returns>
        static int Match(char c1, char c2)
        {
            return c1==c2 ? 0 : 1;
        }

        
        //Проведение эксперимента n_test раз для обычной мартицы и рекурентного варианта
        static long[] TestTime(string str1, int m, string str2, int n, int n_tests)
        {
            Stopwatch st= new Stopwatch();
            long[] arr=new long[]{m,n,0,0};
            
            
            
            long frequency=0;
            for (int i = 0; i <= n_tests; i++)
            {
                st.Reset();
                st.Start();
                int rez= BaseMatrix(str1, m,str2, n);
                st.Stop();
                if (i!=0)
                {
                    frequency += st.ElapsedTicks;
                    //Console.WriteLine(st.ElapsedTicks+" "+  rez);
                }
            }
            //Console.WriteLine(" Base ticks avg= {0}",frequency/n_tests);
            arr[2] = frequency / n_tests;
            
            
           
            
           frequency=0;
            for (int i = 0; i <=n_tests; i++)
            {
                st=new Stopwatch();
                st.Start();
                int rez=RecurAlgo(str1, str2,m, n);
                st.Stop();
                if (i!=0)
                {
                    frequency += st.ElapsedTicks;
                    //Console.WriteLine(st.ElapsedTicks+" "+  rez);
                }
            }
            //Console.WriteLine(" Recur ticks avg= {0}",frequency/n_tests);
            arr[3] = frequency / n_tests;

            return arr;
        }
        
        /// <summary>
        /// Выполнение сравнения скорости работы алгоритмов
        /// </summary>
        static void Test()
        {
            string[] strarr1=new string[]{"A","aA","BaD","Word","World","WorldA","WorldAa","WordWord"};
            string[] strarr2=new string[]{"a","aa","BAD","word","Wokld","WowwdA","worldaa","wordword"};
            int n_tests = 10;
            List<long[]> lst= new List<long[]>();
            for (int i = 0; i < strarr1.Length; i++)
            {
                lst.Add( TestTime(strarr1[i], strarr1[i].Length,strarr2[i], strarr2[i].Length,n_tests));
                
            }
            Console.WriteLine("len"+"\t"+"Base"+"\t"+"recur");
            foreach (long[] arr in lst)
            {
                Console.WriteLine(arr[0]+"\t"+arr[2]+"\t"+arr[3]);
            }
        }

        static void HandTest()
        {
            Console.WriteLine("First line");
            string str1 = Console.ReadLine();
            Console.WriteLine("Second line");
            string str2 = Console.ReadLine();
            int res =0;
            res= BaseMatrix(str1, str1.Length, str2, str2.Length);
            Console.WriteLine("Base result {0}",res);
            res = ModifMatrix(str1, str1.Length, str2, str2.Length);
            Console.WriteLine("Modif. result {0}",res);
            res = RecurAlgo(str1, str2, str1.Length, str2.Length);
            Console.WriteLine("Recur. result {0}",res);
        }
        
        static void Main(string[] args)
        {
            while (true)
            {
                HandTest();
            }
            
            
            Console.ReadKey();
        }
    }
}