using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace WpfApp3
{
    public class WaveParams
    {
        Label   WaveInfo = new Label { Content = "info" };
        TextBox WaveAmpTB = new TextBox { Text = "Амплитуда", Name = "WaveAmpTB" };
        TextBox WaveLenTB = new TextBox { Text = "Длина", Name = "WaveLenTB" };
        TextBox WaveSpeedTB = new TextBox { Text = "Скорость", Name = "WaveSpeedTB" };
        TextBox WaveDirTB = new TextBox { Text = "Направление", Name = "WaveDirTB" };
        TextBox WaveExpTB = new TextBox { Text = "EXP", Name = "WaveExpTB" };

        /// <summary>
        /// Индекс волны
        /// </summary>
        private readonly int _indexOfWave;

        /// <summary>
        /// Направление волны
        /// </summary>
        private Vector3D _directionVector;

        private double _directionAngle;

        /// <summary>
        /// Длина волны
        /// </summary>
        private double _waveLength ;

        /// <summary>
        /// Длина волны
        /// </summary>
        private double _waveSpeed;

        /// <summary>
        /// Амплитуда волны
        /// </summary>
        private double _waveAmplitude;

        /// <summary>
        /// "Экспонента волны
        /// </summary>
        private double _wavekexp;

        //преобразование угла в вектор
        private void SetDirection(double val)
        {
            _directionAngle = val;
            // convert deg to radians
            double angle = ((val / 360.0) * 2.0 * Math.PI);
            Vector3D unitvector = new Vector3D(1, 0, 0);
            _directionVector.X = (unitvector.X * Math.Cos(angle)) - (unitvector.Y * Math.Sin(angle));
            _directionVector.Y = (unitvector.X * Math.Cos(angle)) + (unitvector.X * Math.Sin(angle));
        }

        public WaveParams(int indexOfWave)
        {
            this._indexOfWave = indexOfWave;
            Init();
            SetData();
        }

        public void SetParams(double wavelength,
                              double amplitude,
                              double kexp,
                              double speed,
                              double angle)
        {
            _waveLength = wavelength;
            _waveAmplitude = amplitude;
            _wavekexp = kexp;
            _waveSpeed = speed;
            SetDirection(angle);
        }

        private void Init()
        {
            SetDirection(355);
            _directionVector = new Vector3D(10, 15, 0);
            _waveLength = 1;
            _waveSpeed = 0.05;
            _waveAmplitude = 0.8;
            _wavekexp = 10;

            WaveAmpTB.TextChanged += TextBox_TextChanged;
            WaveDirTB.TextChanged += TextBox_TextChanged;
            WaveExpTB.TextChanged += TextBox_TextChanged;
            WaveLenTB.TextChanged += TextBox_TextChanged;
            WaveSpeedTB.TextChanged += TextBox_TextChanged;

            Grid.SetRow(WaveInfo, 0);
            Grid.SetColumn(WaveInfo, _indexOfWave);

            Grid.SetRow(WaveAmpTB, 1);
            Grid.SetColumn(WaveAmpTB, _indexOfWave);

            Grid.SetRow(WaveLenTB, 2);
            Grid.SetColumn(WaveLenTB, _indexOfWave);

            Grid.SetRow(WaveSpeedTB, 3);
            Grid.SetColumn(WaveSpeedTB, _indexOfWave);

            Grid.SetRow(WaveDirTB, 4);
            Grid.SetColumn(WaveDirTB, _indexOfWave);

            Grid.SetRow(WaveExpTB, 5);
            Grid.SetColumn(WaveExpTB, _indexOfWave);
        }

        public void OpenSettings(Grid dinamic)
        {
            SetData();
            dinamic.Children.Add(WaveInfo);
            dinamic.Children.Add(WaveAmpTB);
            dinamic.Children.Add(WaveDirTB);
            dinamic.Children.Add(WaveExpTB);
            dinamic.Children.Add(WaveLenTB);
            dinamic.Children.Add(WaveSpeedTB);

        }

        public void Remove(Grid dinamic)
        {
            dinamic.Children.Remove(WaveInfo);
            dinamic.Children.Remove(WaveAmpTB);
            dinamic.Children.Remove(WaveDirTB);
            dinamic.Children.Remove(WaveExpTB);
            dinamic.Children.Remove(WaveLenTB);
            dinamic.Children.Remove(WaveSpeedTB);

        }


        private void SetData()
        {
            WaveInfo.Content = "Волна №" + _indexOfWave;
            WaveAmpTB.Text = _waveAmplitude.ToString();
            WaveDirTB.Text = _directionAngle.ToString();
            WaveExpTB.Text = _wavekexp.ToString();
            WaveLenTB.Text = _waveLength.ToString();
            WaveSpeedTB.Text = _waveSpeed.ToString();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox tb = (TextBox)sender;
            string s = tb.Text;        
            

            if (tb.Text == "")
                return;
            if (tb.Text == "-")
                return;
            if (!double.TryParse(s, out double data))
            {
                MessageBox.Show("Некорретное значение");
                return;
            }

            if (tb.Name == "WaveDirTB")
            {
                for (; data < 0; data += 360)
                { }
            }


            if (data < 0)
            {
                MessageBox.Show("Значение должно быть больше 0");
                tb.Text = "1";
                return;
            }

            switch (tb.Name)
            {
                case "WaveAmpTB":
                    _waveAmplitude = data;
                    Trace.WriteLine("Change"+this.GetHashCode() + " " + _waveAmplitude);
                    break;
                case "WaveLenTB":
                    _waveLength = data;
                    break;
                case "WaveSpeedTB":
                    _waveSpeed = data;
                    break;
                case "WaveDirTB":
                    SetDirection(data);
                    break;
                case "WaveExpTB":
                    _wavekexp = data;
                    break;
                default:
                    MessageBox.Show("Поле не найдено");
                    break;

            }
        }


        public double WaveGenFunc(int x, int z, double t)
        {
            Vector3D posVect = new Vector3D(x, z, 0);
            //Trace.WriteLine( this.GetHashCode()+" "+ _waveAmplitude);

            double dotresult = Vector3D.DotProduct(_directionVector, posVect); ;
            dotresult *= (2 * Math.PI) / _waveLength;
            double phaseConstant = t * ((_waveSpeed * 2 * Math.PI) / _waveLength);

            double rez = (dotresult + phaseConstant);
            rez = (Math.Sin(rez) + 1.0) / 2.0;
            rez = _waveAmplitude * Math.Pow(rez, _wavekexp);
            return rez;
        }
    }
}
