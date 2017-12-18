using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp3
{
    class OldGeneratorscs
    {

        #region func4
        //file:///C:/Users/medva/Downloads/1109.6494v1.pdf
        private List<double> Amp = new List<double>();
        private List<Vector> Kvect = new List<Vector>();
        private List<double> Wlist = new List<double>();
        private double func4(int x, int z, double t)
        {
            if (!Amp.Any())
            {
                Amp.Add(0.1);
                Kvect.Add(new Vector(3, 5));
                Wlist.Add(0.1);

                Amp.Add(0.2);
                Kvect.Add(new Vector(3, 10));
                Wlist.Add(1);

                Amp.Add(0.2);
                Kvect.Add(new Vector(10, 1));
                Wlist.Add(2);
            }


            double rez = 0;

            for (int i = 0; i < Amp.Count; i++)
            {
                rez += Amp[i] * Math.Cos(Kvect[i].X * x * u + Kvect[i].Y * z * u + Wlist[i] * t / 5);
            }
            return rez;
        }


        #endregion

        #region func3
        //https://habrahabr.ru/company/mailru/blog/169257/


        private double w = 1;
        //фаза
        private double q = 0.5;
        private double func3(int x, int y, double t)
        {
            double rez = M(x, y, t) * A * Math.Sin(kz - w * t + q);
            return rez;
        }

        //длинна сегмента волны
        private double u = 30;
        //скорость появления/исчезания волн в данном фронте
        private double v = 0.05;
        private double M(int x, int y, double t)
        {
            double rez = 0.5 * Math.Pow(Math.Sin(u * x), 2) * Math.Pow(Math.Sin(u * y), 2) * (1 + Math.Sin(v * t));
            return rez;

        }
        #endregion

        #region func2

        //высота волны
        private double A = 1;
        //направление
        private int kx = 10;
        private int kz = 1;
        //скорее всего длинна, так как влиет на скорость изменения
        private double Omega = 2 / Math.PI;

        private double func2(int x, int z, double t)
        {
            double rez = 0;
            if (x % 2 == 1)
                rez = A * Math.Cos(kx * (x - 1) + kz * z - Omega * t);
            else if (z % 2 == 1)
            {
                rez = A * Math.Cos(kx * x + kz * (z - 1) - Omega * t);
            }
            else if ((z % 2 == 1) && (x % 2 == 1))
            {
                rez = A * Math.Cos(kx * (x - 1) + kz * (z - 1) - Omega * t);
            }
            else
            {
                rez = A * Math.Cos(kx * x + kz * z - Omega * t);
            }



            //Trace.Write(rez+"\n");
            return rez;
        }
        #endregion
    }
}
