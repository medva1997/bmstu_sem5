using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for WaterSettings.xaml
    /// </summary>
    public partial class WaterSettings : Window
    {
        //копия списка
        private readonly List<WaveParams> _wParamsList;

       
        /// <summary>
        /// Создание окна настроек моря
        /// </summary>
        /// <param name="wParamsList">Список параметров волн</param>
        public WaterSettings(List<WaveParams> wParamsList)
        {
            _wParamsList = wParamsList;
            InitializeComponent();
            //На один больше для нулевого столбца с описанием параметра
            GenerateColloms(wParamsList.Count+1,6);
            AddElements();

            //Приклеиваем все объекты к разметке
            foreach (WaveParams t in wParamsList)
            {
                t.OpenSettings(MainGrid);
            }
            //При закрытии нужно отклить объеты от разметки, так как объекты будут использованы повторно
            Closing += WaterSettings_Closing;
        }

        /// <summary>
        /// отвязка объетов от рзметки при закрытии формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaterSettings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //отвязка объетов от разметки
            foreach (WaveParams t in _wParamsList)
            {
                t.Remove(MainGrid);
            }
        }

        /// <summary>
        /// Генерация разметки 
        /// </summary>
        /// <param name="col">количество колонок</param>
        /// <param name="row">количество строк</param>
        private void GenerateColloms(int col, int row)
        {
            //генерация столбцов окна
            for (int i = 0; i < col; i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            //генерация нужного количества строк столбце
            for (int i = 0; i < row; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }

            MainGrid.ShowGridLines = true;
            
        }

        /// <summary>
        /// Первый информационный столбец
        /// </summary>
        private void AddElements()
        {
            Label waveInfo = new Label {Content = "info"};
            Label waveAmpTb = new  Label {Content = "Амплитуда"};
            Label waveLenTb = new  Label {Content = "Длина"};
            Label waveSpeedTb = new  Label {Content = "Скорость"};
            Label waveDirTb = new  Label {Content = "Направление"};
            Label waveExpTb = new  Label {Content = "EXP"};

            //привязка к сетке
            Grid.SetRow(waveInfo, 0);
            Grid.SetColumn(waveInfo, 0);

            Grid.SetRow(waveAmpTb, 1);
            Grid.SetColumn(waveAmpTb, 0);

            Grid.SetRow(waveLenTb, 2);
            Grid.SetColumn(waveLenTb, 0);

            Grid.SetRow(waveSpeedTb, 3);
            Grid.SetColumn(waveSpeedTb, 0);

            Grid.SetRow(waveDirTb, 4);
            Grid.SetColumn(waveDirTb, 0);

            Grid.SetRow(waveExpTb, 5);
            Grid.SetColumn(waveExpTb, 0);

            //Закрепление за объектом формы
            MainGrid.Children.Add(waveInfo);
            MainGrid.Children.Add(waveAmpTb);
            MainGrid.Children.Add(waveDirTb);
            MainGrid.Children.Add(waveExpTb);
            MainGrid.Children.Add(waveLenTb);
            MainGrid.Children.Add(waveSpeedTb);

        }
    }

   
}
