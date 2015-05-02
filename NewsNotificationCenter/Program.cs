using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NewsNotificationCenter
{
    static class Program
    {
        [DllImport( "User32.dll" )]
        private static extern bool ShowWindowAsync( IntPtr hWnd, int cmdShow );

        [DllImport( "User32.dll" )]
        private static extern bool SetForegroundWindow( IntPtr hWnd );

        private const int WS_SHOWNORMAL = 1;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process instance = RunningInstance();
            if (instance == null)
            {
                AppDomain.CurrentDomain.SetData( "APP_CONFIG_FILE", "app.config" );
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault( false );
                Application.Run( LoginForm.GetInstance() );
            }
            else
            {
                // Already launched
                HandleRunningInstance(instance);
            }
        }

        private static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName( current.ProcessName );

            foreach( Process process in processes )
            {
                if( process.Id != current.Id )
                {
                    if( Assembly.GetExecutingAssembly().Location.Replace( "/", "//" ) == current.MainModule.FileName )
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        private static void HandleRunningInstance( Process instance )
        {
            ShowWindowAsync( instance.MainWindowHandle, WS_SHOWNORMAL );
            SetForegroundWindow( instance.MainWindowHandle );
        }
    }
}
