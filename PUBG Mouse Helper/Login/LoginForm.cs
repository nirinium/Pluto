using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace Pluto
{
    public partial class LoginForm : Form
    {
        public bool IsUserAuthenticated { get; set; }

        public LoginForm()
        {
            InitializeComponent();
            maskedTextBox1.Text = Properties.UserPrefs.Default.UserPassword;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "rf" && maskedTextBox1.Text == "qwerty")
            {
                IsUserAuthenticated = true;

                if (checkBox1.Checked)
                {
                    Properties.UserPrefs.Default.UserPassword = maskedTextBox1.Text;
                    Properties.UserPrefs.Default.Save();
                }

                PUBG_Mouse_Helper.Form1 form1 = new PUBG_Mouse_Helper.Form1();
                form1.Show();

                this.Hide();
            }
            else
            {
                IsUserAuthenticated = false;

                for (int i = 0; i < 5; i++)
                {
                    this.Left += 5;
                    System.Threading.Thread.Sleep(25);
                    this.Left -= 5;
                    System.Threading.Thread.Sleep(25);

                }
            }

        }


        private void textBox1_Click(object sender, EventArgs e)
        {
            //textBox1.Clear();
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) //if enter pressed then login
            {
                button1.PerformClick();
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}


