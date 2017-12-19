using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleGame
{
    
    /// <summary>
    /// Главная форма
    /// </summary>
    public partial class MainForm : Form
    {
        private const string Word = "программирование";
        
        private Stack<Button> stackUndo = new Stack<Button>();

        /// <summary>
        /// Стандартный цвет кнопки (для сброса в исходное состояние)
        /// </summary>
        private readonly Color _standartButtonColor = Button.DefaultBackColor;
        /// <summary>
        /// Цвет кнопки при наведении кнопки на нее
        /// </summary>
        private readonly Color _underMouseButtonColor = Color.BlueViolet;


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
                int x = 0;
                
                foreach (Char c in Word)
                {
                    Button b = new Button
                    {
                        Text = c.ToString(),
                        Location = new Point(x, 0),
                        Size = new Size(48, 48),
                        Font = new Font(FontFamily.GenericSansSerif, 28, FontStyle.Bold)
                    };

                    //Подписка на события
                    b.Click += LetterClick;
                    b.MouseEnter += MouseOnButton;
                    b.MouseLeave += MouseLeaveButton;

                    //b.Tag = count++;
                    letters.Controls.Add(b);
                    x += 55;                   
                }
                //Подписка на события
                this.KeyPress += MainForm_KeyPress;
                this.exitToolStripMenuItem.Click += ExitClick;
               
             }
            catch (Exception ex)
            {
                    MessageBox.Show(ex.Message,"Create error",MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }

}

        
        /// <summary>
        /// Обработка нажатия кнопки выход в выпадающем меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitClick(object sender, EventArgs e)
        {
            string message = "Вы уверены что хотите выйти из игры";
            if (MessageBox.Show(message, "Выход", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Close();
                
            }
           
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar; //введенный символ
            //int position=Word.IndexOf(c);

            //if (position >= 0)
            //{
            //    //явное приведение типов
            //    Button b = (Button)letters.Controls[position]; //Это очень плохо так 
            //    //как нет проверки на наличие и что это действительно кнопка
            //    if (b.Enabled == false) // нажимали ли мы эту кнопку до этого
            //    {
            //        MessageBox.Show("Данная буква уже выбрана");
            //    }
            //    else
            //    {
            //        text.Text += b.Text;
            //        b.Enabled = false;
            //        stackUndo.Push(b);
            //    }
                
            //}


            //Что делать если в Контроле не только кнопки???, но в нашем примере пока только кнопки
            foreach (var btmp in letters.Controls)
            {
                // то же самое выглядит еще и как: btmp is Button
                if (object.ReferenceEquals(btmp.GetType(), typeof(Button)))
                {
                    Button b = (Button) btmp;
                    if ((b.Enabled == true) && (b.Text.Equals(c.ToString())))
                    {
                        //высокая вложенность
                        //дублирование кода
                        text.Text += b.Text;
                        b.Enabled = false;
                        stackUndo.Push(b);
                        // можно выйти по break; 
                        return; //Не очень хорошо так выходить из цикла, можно заменить на поднятие флага
                    }
                }
                
            }

            // мне кажется, это не требуется - избыточно
            MessageBox.Show("Такой буквы нету или она уже использована");

        }

        /// <summary>
        /// Смена фона кнопки при выходе мышки за пределы кнопки 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeaveButton(object sender, EventArgs e)
        {
            try
            {
                //явное приведение типов
                Button b = (Button)sender;
                b.BackColor = _standartButtonColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Change color error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Смена фона кнопки при наведении на нее мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseOnButton(object sender, EventArgs e)
        {
            try
            {
                //явное приведение типов
                Button b = (Button) sender;
                b.BackColor = _underMouseButtonColor;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Change color error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Нажатие на кнопку-букву
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LetterClick(object sender, EventArgs e)
        {
            try
            {
                //явное приведение типов
                Button b = (Button)sender;
                text.Text += b.Text;
                b.Enabled = false;
                //stack.Push(((int)b.Tag));
                stackUndo.Push(b);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Click error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Обработка нажатия на кнопку сброса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetClick(object sender, EventArgs e)
        {
            text.Text = "";
            foreach (var b in letters.Controls) 
            {
                ((Button)b).Enabled = true;
            }
            stackUndo.Clear();
        }

        /// <summary>
        /// Отмена последней буквы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UndoClick(object sender, EventArgs e)
        {
            //выход если текста нету
            //if (text.Text == "")
            //    return;

            // int n = stack.Pop();
            //letters.Controls[n].Enabled = true;

            //Проверка стека и текста на пустоту
            if (stackUndo.Any() || text.Text!="")
            {
                //отрезезаем последнюю букву
                text.Text = text.Text.Substring(0, text.Text.Length - 1);
                stackUndo.Pop().Enabled = true;
                //b.Enabled = true;
            }
            else
            {
                MessageBox.Show( "Отмена не возможна");
            }


        }

       
    }
}
