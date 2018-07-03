using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUBG_Mouse_Helper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Pluto.Login.LoginForm login = new Pluto.Login.LoginForm();

            Application.Run(login);
            if (login.Success)
            {
                Application.Run(new Form1());
            }
        }
    }
}
