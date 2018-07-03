using System;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace Pluto.Login
{
    public partial class LoginForm : Form
    {
        public bool Success { get; set; }

        public LoginForm()
        {
            InitializeComponent();
            PingLoginServer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "rf" && textBox2.Text == "rf")
            {
                Success = true;
                this.Dispose();
            }
            else
            {
                Success = false;

                for (int i = 0; i < 5; i++)
                {
                    this.Left += 10;
                    System.Threading.Thread.Sleep(25);
                    this.Left -= 10;
                    System.Threading.Thread.Sleep(25);
                }
            }
        }

        private void PingLoginServer()
        {
            Ping myPing = new Ping();
            PingReply reply = myPing.Send("www.nirinium.com", 100);

            if (reply != null)
            {
                toolStripStatusLabel1.Visible = true;
                loginSrvStatus.Text = "Server Connection: "; toolStripStatusLabel1.Text = reply.Status + "!  " + reply.RoundtripTime.ToString() + "ms";
            }
            else
            {
                loginSrvStatus.Text = "Server Connection:" + "Timed Out";
            }
        }
        
    }
}