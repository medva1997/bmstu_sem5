using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace SampleGame
{
    /// <summary>
    /// Главная форма программы
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Длинное слово
        /// </summary>
        private string Word;

        /// <summary>
        /// Стек номеров кнопок
        /// </summary>
        private Stack<Button> stack = new Stack<Button>();

        /// <summary>
        /// Словарь (XML)
        /// </summary>
        private Worterbuch dict;

        /// <summary>
        /// База данных
        /// </summary>
        private DB.GAMEEntities db;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработка загрузки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                /*
                // Путь к исполняемому файлу
                string path = Assembly.GetExecutingAssembly().CodeBase;
                // Имя каталога, где располагается исполняемый файл
                path = System.IO.Path.GetDirectoryName(path);
                // Имя файла словаря
                path += @"\Dict.xml";
                // Загрузка словаря
                dict = Worterbuch.Load(path);
                // Выбор длинного слова
                Random rand = new Random();
                Word = dict.GetLongWord(rand.Next(1, dict.LongWordCount()));
                */

                InitButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Проблема", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void InitButtons()
        {
            // Подключение к базе
            db = new DB.GAMEEntities();
            // Выбор слова из БД
            int max = db.Dictionary.Max(a => a.LongWord);
            Random rand = new Random();
            int r = rand.Next(1, max);
            DB.Dictionary d = db.Dictionary.Where(a => a.LongWord == r).First();
            Word = d.Word;
            labelTotal.Text = d.LongCount.ToString();
            // Очистка списка
            listWords.Items.Clear();
            // Счетчик найденных
            labelFound.Text = listWords.Items.Count.ToString();

            // Создание кнопок из слова
            int x = 0;
            int count = 0;
            foreach (Char ch in Word)
            {
                Button b = new Button();
                b.Text = ch.ToString();
                b.Size = new Size(48, 48);
                b.Location = new Point(x, 0);
                b.Font = new Font(FontFamily.GenericSansSerif, 28, FontStyle.Bold);
                b.Tag = count++;
                b.Click += LetterClick;
                letters.Controls.Add(b);
                x += 60;
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LetterClick(object sender, EventArgs e)
        {
            try
            {
                // Явное приведение типа
                Button b = (Button)sender;
                // Добавить букву в индикатор
                text.Text += b.Text;
                // Блокировка кнопки
                b.Enabled = false;
                // Запомнить кнопку
                stack.Push(b);
                // проверка слова по словарю
                var x = db.Dictionary.Where(a => a.Word == text.Text).FirstOrDefault();
                if (x != null)
                {
                    // Проверка, не находили ли мы это слово ранее?
                    if (!listWords.Items.Contains(x.Word))
                    {
                        // Добавить слово в список
                        listWords.Items.Add(x.Word);
                        // Сброс нажатых кнопок
                        reset_Click(null, null);
                        // Счетчик найденных
                        labelFound.Text = listWords.Items.Count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Проблема", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Кнопка сброс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reset_Click(object sender, EventArgs e)
        {
            text.Text = "";
            // Включение всех кнопок
            foreach (Button b in letters.Controls)
            {
                b.Enabled = true;
            }
        }

        /// <summary>
        /// Удаление последней буквы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, EventArgs e)
        {
            // Удаление последней буквы
            text.Text = text.Text.Substring(0, text.Text.Length - 1);
            // Извлечь из стека кнопку и включить ее
            stack.Pop().Enabled = true;
        }

        /// <summary>
        /// Новая игра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void new_Click(object sender, EventArgs e)
        {
            // Сброс нажатых кнопок
            reset_Click(null, null);
            // Очистка кнопок
            letters.Controls.Clear();
            // Инициализация кнопок
            InitButtons();
        }

        /// <summary>
        /// Добавление слова в словарь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_Click(object sender, EventArgs e)
        {
            // Добавить слово в список
            listWords.Items.Add(text.Text);
            // Счетчик найденных
            labelFound.Text = listWords.Items.Count.ToString();
            // Добавить слово в базу
            db.Dictionary.Add(
                new DB.Dictionary()
                {
                    Word = text.Text,
                    LongCount = 0,
                    LongWord = 0
                }
            );
            // Фактическая запись в базу
            db.SaveChanges();
        }
    }
}
