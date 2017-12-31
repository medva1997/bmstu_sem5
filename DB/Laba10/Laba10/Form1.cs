using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba10
{
    public partial class Form1 : Form
    {
        private static string connectionString = "server=127.0.0.1;database=MYWATCH2; user id=AAA; password=AAA";
        string tableName = "Company";
        public Form1()
        {
            InitializeComponent();
            checkedListBox1.CheckOnClick = true;
            radioButton1.Checked = false;
            radioButton1.Checked = true;
        }

        #region radioButtons_CheckedChanged
        
        /// <summary>
        /// Company Checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(new object[] {
                "CompID",
                "CompAdminMID",
                "CompName",
                "CompCity"
                });

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i,true);
            }
            tableName = radioButton1.Text;
        }

        /// <summary>
        /// Manager Checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(new object[] {
                "MID",
                "CompID",
                "LastName",
                "FirstName",
                "SecondName",
                "Email",
                "Password",
                "PhoneNumber"
            });
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            tableName = radioButton2.Text;
        }

        /// <summary>
        /// Visitor Checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(new object[] {
                "VID",
                "LastName",
                "FirstName",
                "SecondName",
                "Email",
                "Password",
                "PhoneNumber"
            });
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            tableName = radioButton3.Text;
        }

        /// <summary>
        /// Meeting Checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(new object[] {
                "Meet_ID",
                "MID",
                "VID",
                "TimeStart",
                "TimeEnd",
                "M_Confirmation",
                "V_confirmation",
                "Location"
            });
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            tableName = radioButton4.Text;
        }


        #endregion

        

        /// <summary>
        /// Нетипизированный DataSet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            SqlConnection connect = new SqlConnection(connectionString);
            CheckedListBox.CheckedItemCollection checkedItems = checkedListBox1.CheckedItems;
            if (checkedItems.Count == 0)
            {
                MessageBox.Show(@"Необходимо выбрать столбцы таблицы");
                return;
            }
            string select = @"SELECT ";
            foreach (var a in checkedItems)
                select = select + a + ", ";
            select = select.Substring(0, select.Length - 2);
            string from = @" FROM " + tableName;
            SqlCommand cmd = new SqlCommand(select + from, connect);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            //dataGridView1
            dataGridView1.DataSource = table;
        }


        /// <summary>
        /// Типизированный DataSet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            MyWatchDataSet set = new MyWatchDataSet();
            switch (tableName)
            {
                case "Company":
                    MyWatchDataSetTableAdapters.CompanyTableAdapter adapterC =
                        new MyWatchDataSetTableAdapters.CompanyTableAdapter();
                    adapterC.Fill(set.Company);
                    dataGridView1.DataSource = set.Company;
                    break;

                case "Manager":
                    MyWatchDataSetTableAdapters.ManagerTableAdapter adapterM =
                        new MyWatchDataSetTableAdapters.ManagerTableAdapter();
                    adapterM.Fill(set.Manager);
                    dataGridView1.DataSource = set.Manager;
                    break;

                case "Visitor":
                    MyWatchDataSetTableAdapters.VisitorTableAdapter adapterV =
                        new MyWatchDataSetTableAdapters.VisitorTableAdapter();
                    adapterV.Fill(set.Visitor);
                    dataGridView1.DataSource = set.Visitor;
                    break;

                case "Meeting":
                    MyWatchDataSetTableAdapters.MeetingTableAdapter adapterMeet =
                        new MyWatchDataSetTableAdapters.MeetingTableAdapter();
                    adapterMeet.Fill(set.Meeting);
                    dataGridView1.DataSource = set.Meeting;
                    break;
            }
        }

        /// <summary>
        /// Подключенный DataReader
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            SqlConnection connect = new SqlConnection(connectionString);
            CheckedListBox.CheckedItemCollection checkedItems = checkedListBox1.CheckedItems;
            if (checkedItems.Count == 0)
            {
                MessageBox.Show(@"Необходимо выбрать столбцы таблицы");
                return;
            }
            string select = @"SELECT ";
            string from = @" FROM " + tableName;
            foreach (object t in checkedItems)
            {
                dataGridView1.Columns.Add(t.ToString(), t.ToString());
                select += t + ", ";
            }

            SqlCommand cmd = new SqlCommand(select.Substring(0, select.Length - 2) + from, connect);
            connect.Open();
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                object[] obj = new object[checkedItems.Count];
                for (int i = 0; i < checkedItems.Count; i++)
                {
                    obj[i] = reader[checkedItems[i].ToString()];
                }
                dataGridView1.Rows.Add(obj);
            }
            reader.Close();
        }

        /// <summary>
        /// LINQ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            MyWatchDataSet set = new MyWatchDataSet();
            switch (tableName)
            {
                case "Company":
                    MyWatchDataSetTableAdapters.CompanyTableAdapter adapterC = new MyWatchDataSetTableAdapters.CompanyTableAdapter();
                    adapterC.Fill(set.Company);
                    dataGridView1.DataSource = (from c in set.Company
                        select c).AsDataView();
                    break;

                case "Manager":
                    MyWatchDataSetTableAdapters.ManagerTableAdapter adapterM = new MyWatchDataSetTableAdapters.ManagerTableAdapter();
                    adapterM.Fill(set.Manager);
                    dataGridView1.DataSource = (from c in set.Manager
                        select c).AsDataView();
                    break;

                case "Visitor":
                    MyWatchDataSetTableAdapters.VisitorTableAdapter adapterV = new MyWatchDataSetTableAdapters.VisitorTableAdapter();
                    adapterV.Fill(set.Visitor);
                    dataGridView1.DataSource = (from c in set.Visitor
                        select c).AsDataView();
                    break;

                case "Meeting":
                    MyWatchDataSetTableAdapters.MeetingTableAdapter adapterMeet = new MyWatchDataSetTableAdapters.MeetingTableAdapter();
                    adapterMeet.Fill(set.Meeting);
                    dataGridView1.DataSource = (from c in set.Meeting
                        select c).AsDataView();
                    break;
                   
            }
        }
    }
}
