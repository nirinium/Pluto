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
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "rf" && maskedTextBox1.Text == "qwerty")
            {
                IsUserAuthenticated = true;
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
    }
}


