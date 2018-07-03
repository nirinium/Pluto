using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;

namespace Pluto.Testing
{
    public partial class MySQLForm : Form
    {
        private MySqlConnection connection;
            
        public MySQLForm()
        {
            InitializeComponent();
        }        

        private void MySQLForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                sqlFormToolStripStatusLabel.Text = "Not Connected";
                sqlFormToolStripStatusLabel.ForeColor = Color.Red;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
                sqlFormToolStripStatusLabel.Text = "Connected";
                sqlFormToolStripStatusLabel.ForeColor = Color.Green;
            }
        }
        /// <summary>
        /// Populate Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Server=96.29.49.47;Port=3306;Database=pluto;Uid=nirinium;Pwd=pwd;SslMode=none");
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM pluto.plutousers", connection);
            try
            {
                connection.Open();
                DataSet ds = new DataSet();
                adapter.Fill(ds, "username");
                dataGridView1.DataSource = ds.Tables["username"];
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// Connect Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            
            try
            {
                connection = new MySqlConnection("Server=96.29.49.47;Port=3306;Database=pluto;Uid=nirinium;Pwd=pwd;SslMode=none");
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    sqlFormToolStripStatusLabel.Text = "Connected";
                    sqlFormToolStripStatusLabel.ForeColor = Color.Green;
                }
                else
                {
                    sqlFormToolStripStatusLabel.Text = "Not Connected";
                    sqlFormToolStripStatusLabel.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void func()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            void bw_DoWork(object sender, DoWorkEventArgs e)
            {

            }

            void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                Console.WriteLine("Comp");
            }            
        }

        

    }
}