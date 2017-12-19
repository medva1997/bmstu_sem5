using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiThread
{
    public partial class MainForm : Form
    {
        private long n;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public MainForm()
        {
            var r = new Random();
            InitializeComponent();
            n = r.Next(int.MaxValue / 2, int.MaxValue );
            nTextBox.Text = n.ToString();
        }

        /// <summary>
        /// Проверка числа на простоту
        /// </summary>
        /// <param name="l">Число</param>
        /// <returns></returns>
        private bool IsPrime(long l)
        {
            for (int i = 2; i < l; i++)
            {
                if (i % 10000000 == 0)
                {
                    // Проверка на аварийный останов
                    if (worker.CancellationPending)
                    {
                        break;
                    }
                    worker.ReportProgress(1);
                }
                if (l % i == 0) return false;
            }
            return true;
        }      

        /// <summary>
        /// Нажатие на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void go_Click(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
            {
                progressBar.Value = 0;
                progressStatus.Value = 0;
                go.ForeColor = Color.HotPink;
                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Изменение числа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nTextBox_TextChanged(object sender, EventArgs e)
        {
            long i;
            if (long.TryParse(nTextBox.Text, out i))
            {
                n = i;
                nTextBox.ForeColor = Color.Black;
            }
            else
            {
                nTextBox.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Фоновый поток 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Поиск следующего простого числа
            while (!IsPrime(++n))
            {
                // Проверка на аварийный останов
                if (worker.CancellationPending)
                {
                    return;
                }
                // Отчет о ходе процесса
                worker.ReportProgress(1);
            }
        }

        /// <summary>
        /// Завершение фонового потока
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Фиксация результата
            nTextBox.Text = n.ToString();
            go.ForeColor = Color.Black;
            progressBar.Value = 0;
            progressStatus.Value = 0;
        }

        /// <summary>
        /// Извещение о состоянии процесса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            nTextBox.Text = n.ToString();

            progressBar.Value++;
            if (progressBar.Value == progressBar.Maximum)
            {
                progressBar.Value = progressBar.Minimum;
            }
            progressStatus.Value++;
            if (progressStatus.Value == progressStatus.Maximum)
            {
                progressStatus.Value = progressStatus.Minimum;
            }
        }

        /// <summary>
        /// Аварийный останов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stop_Click(object sender, EventArgs e)
        {
            worker.CancelAsync();
        }
    }
}
