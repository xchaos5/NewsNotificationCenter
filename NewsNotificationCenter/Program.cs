using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NewsNotificationCenter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "app.config"); 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(LoginForm.GetInstance());
        }
    }
}
