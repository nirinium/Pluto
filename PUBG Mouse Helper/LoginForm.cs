using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pluto
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "123" && maskedTextBox1.Text == "123")
            {
                PUBG_Mouse_Helper.Form1 login = new PUBG_Mouse_Helper.Form1();
                login.ShowDialog();
               
                this.Hide();
            }
            else
            {
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
            textBox1.Clear();
        }
    }
}
