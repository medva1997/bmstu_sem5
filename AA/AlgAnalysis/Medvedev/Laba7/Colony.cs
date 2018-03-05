using System;

namespace Laba7
{

    /// <summary>
    /// Стркутура муравья
    /// </summary>
    internal struct Ant
    {
        public int StartCity;
        public int CurrCity;

        /// <summary>
        /// длинна пути
        /// </summary>
        public double Lk;

        public double[] Route;
        public double[] Jk;
    };

    public struct Answer
    {
        public double Len;
        public double[] Route;
    }

    public class Colony
    {
        /// <summary>
        /// Генератор случайных чисел
        /// </summary>
        private readonly Random _rnd;

        /// <summary>
        /// Количество городов/муравьев и размерность матриц
        /// </summary>
        private readonly int _n;

        /// <summary>
        /// Матрица описывающия длины дорог между городами
        /// </summary>
        private double[,] _graph;

       


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="n"></param>
        public Colony(int n)
        {
            _n = n;
            _rnd = new Random();
            
        }

        /// <summary>
        /// поэлементное копирование массива
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        private void CopyArray(double[] dst, double[] src)
        {
            for (int i = 0; i < dst.Length; i++)
                dst[i] = src[i];
        }


        /// <summary>
        /// Начальное заполнение графа
        /// </summary>
        public void CreateRandomMatrix()
        {
            _graph = new double[_n, _n];

            for (int i = 0; i < _n; i++)
            {
                for (int j = i; j < _n; j++)
                {
                    if (i == j)
                        _graph[i, j] = 0;
                    else
                    {
                        _graph[i, j] = _rnd.Next(30) + 1;
                        _graph[j, i] = _graph[i, j];
                    }
                }
            }
        }



        /// <summary>
        /// Установка значений муравью
        /// </summary>
        /// <param name="ant"></param>
        private void InitAnt(Ant ant)
        {
            int start = ant.StartCity;
            ant.CurrCity = start;
            ant.Lk = 0;
            for (int i = 0; i < ant.Route.Length; i++)
            {
                ant.Route[i] = -1;
                ant.Jk[i] = 1;
            }

            ant.Route[0] = start;
            ant.Jk[start] = 0;
        }

        /// <summary>
        /// Создание массива муравьев
        /// </summary>
        /// <returns></returns>
        private Ant[] CreateAnts()
        {
            Ant[] ants = new Ant[_n];
            for (int i = 0; i < _n; i++)
            {
                ants[i] = new Ant
                {
                    StartCity = i,
                    CurrCity = i,
                    Lk = 0,
                    Route = new double[_n],
                    Jk = new double[_n]
                };

            }

            return ants;
        }

        /// <summary>
        /// Пересчет матрицы весов после каждой итерации
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="pheromon"></param>
        /// <param name="visib"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void RecalcWeight(double[,] weight, double[,] pheromon, double[,] visib, double a, double b)
        {
           
            for (int i = 0; i < _n; i++)
            for (int j = 0; j < _n; j++)
                weight[i, j] = Math.Pow(pheromon[i, j], a) * Math.Pow(visib[i, j], b);
        }

        /// <summary>
        /// Обновление значения феромона на дорогах
        /// </summary>
        /// <param name="pheromon"></param>
        /// <param name="dPheromon"></param>
        /// <param name="p">Коэффицент исперения</param>
        private void RecalcPheromon(double[,] pheromon, double[,] dPheromon, double p)
        {
            for (int i = 0; i < _n; i++)
            for (int j = 0; j < _n; j++)
            {
                pheromon[i, j] = pheromon[i, j] * (1 - p) + dPheromon[i, j];
                dPheromon[i, j] = 0;
            }
        }

        /// <summary>
        /// выбор пути на основе массивавероятностей
        /// </summary>
        /// <param name="prob">Массив вероятностей выбрать город</param>
        /// <returns></returns>
        private int ChooseNext(double[] prob)
        {
            int i = 0;
            double rand = _rnd.NextDouble();
            if (rand == 0)
            {
                while (prob[i++] <= 0) ;
                return --i;
            }

            while (rand > 0)
                rand -= prob[i++];

            return --i;
        }

        /// <summary>
        /// Длинна маршрута
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        private double LengthOfRoute(double[] route)
        {
            double length = 0;
            for (int i = 0; i < _n - 1; i++)
                length += _graph[(int) route[i], (int) route[i + 1]];
            return length;
        }


        /// <summary>
        /// Изменение количества феромонов
        /// </summary>
        /// <param name="dPheromon">Значение феромонов на путях</param>
        /// <param name="route"></param>
        /// <param name="lk">длина пути пройденная муравьем</param>
        /// <param name="q">параметр, порядка оптимальной длинны</param>
        private void AddPheromon(double[,] dPheromon, double[] route, double lk, int q)
        {
            double dFer = q / lk;
            for (int i = 0; i < _n - 1; i++)
                dPheromon[(int) route[i], (int) route[i + 1]] += dFer;
        }

        private void GoAnt(ref Ant ant, double[,] weight, double[,] dPheromon, int q)
        {

            int i = 1;
            double[] prob = new double[_n];
            while (i < _n)
            {
                double sumWeight = 0;
                for (int j = 0; j < _n; j++)
                    sumWeight += weight[ant.CurrCity, j] * ant.Jk[j];
                for (int j = 0; j < _n; j++)
                {
                    prob[j] = weight[ant.CurrCity, j] / sumWeight * ant.Jk[j];
                }

                int next = ChooseNext(prob);
                ant.CurrCity = next;
                ant.Jk[next] = 0;
                ant.Route[i++] = next;
            }

            ant.Lk = LengthOfRoute(ant.Route);
            AddPheromon(dPheromon, ant.Route, ant.Lk, q);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alfa">Величина определяющая стадность</param>
        /// <param name="betta">Величина определяющая жадность</param>
        /// <param name="p">Коэффицент испарения  феромона</param>
        /// <param name="q">параметр, имеющий значение порядка оптимального пути</param>
        /// <param name="tMax">Количество итераций</param>
        /// <returns></returns>
        public Answer Solve(double alfa, double betta, double p, int q, int tMax)
        {

            //один муравей на один город
            Ant[] ants = CreateAnts(); 

            //установка начального значения феромонов
            double[,] pheromon = new double[_n, _n];
            for (int i = 0; i < _n; i++)
            for (int j = 0; j < _n; j++)
                pheromon[i, j] = 0.5;

            //"зрение" муравьев
            double[,] visib = new double[_n, _n];
            for (int i = 0; i < _n; i++)
            {
                for (int j = i; j < _n; j++)
                {
                    if (i != j)
                    {
                        visib[i, j] = 1 / _graph[i, j];
                        visib[j, i] = visib[i, j];
                    }
                    else
                        visib[i, j] = 666;
                }
            }

            double[,] weight = new double[_n, _n];
            RecalcWeight(weight, pheromon, visib, alfa, betta);
 
            double[,] dPheromon = new double[_n, _n];
            for (int i = 0; i < _n; i++)
            for (int j = 0; j < _n; j++)
                dPheromon[i, j] = 0;

            int bestL = Int32.MaxValue;
            double[] route = new double[_n];

            for (int t = 0; t < tMax; t++)
            {
                for (int k = 0; k < _n; k++)
                {
                    InitAnt(ants[k]);
                    GoAnt(ref ants[k], weight, pheromon, q);
                }

                int best = -1;
                for (int i = 0; i < _n; i++)
                    if (ants[i].Lk < bestL)
                    {
                        best = i;
                        bestL = (int) ants[i].Lk;
                    }

                if (best != -1)
                    CopyArray(route, ants[best].Route);


                RecalcPheromon(pheromon, dPheromon, p);
                RecalcWeight(weight, pheromon, visib, alfa, betta);
            }

            return new Answer() {Len = bestL, Route = route};
        }
    }
}