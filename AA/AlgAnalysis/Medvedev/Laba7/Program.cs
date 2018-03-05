using System;

namespace Laba7
{
    class Program
    {
        static void Main(string[] args)
        {
            
            int n = 100;
            Colony colony= new Colony(n);
            colony.CreateRandomMatrix();
            int q = 350;
            Answer ans;
            for (double i = 0; i <= 1.01; i+= 0.1)
            {

                ans = colony.Solve( i, 1-i, 0.5, q, 200);

                /*for (int j = 0; j < ans.route.Length; j++)
                {
                    Console.Write($"{ans.route[j]}  ");
                }*/
                Console.WriteLine("alpha          :{0}\n"+
                                  "beta           :{1}\n"+
                                  "Length         :{2}\n"+
                                  "-----------------------\n",i, 1-i, ans.Len);
            }
            for (int i = 100; i <= 1000; i+= 100)
            {

                ans = colony.Solve( 0.5, 0.5, 0.5, q, i);

                Console.WriteLine("LifeTime       :{0}\n"+
                                  "Length         :{1}\n"+
                                  "-----------------------\n",i, ans.Len);
            }
        }
    }
}