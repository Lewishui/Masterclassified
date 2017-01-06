using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MasterClassified
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (IsAlreadyRunning())
            {
                MessageBox.Show("The tool is running!");
                return;
            }

            FileInfo Log4NetFile = new FileInfo("./Log4Net.Config");
            log4net.Config.XmlConfigurator.Configure(Log4NetFile);

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            Control.CheckForIllegalCrossThreadCalls = false;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            log4net.ILog objLogger = log4net.LogManager.GetLogger("SystemExceptionLogger");
            objLogger.Fatal("System Error " + e.Exception.Message.ToString() + ", Exception Detail Info :" + e.Exception.StackTrace + "Time" + DateTime.Now.ToString());

            MessageBox.Show("System Error，Please contact with admin!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static bool IsAlreadyRunning()
        {
            Process processCurrent = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (processCurrent.Id != process.Id)
                {
                    if (processCurrent.ProcessName == process.ProcessName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
