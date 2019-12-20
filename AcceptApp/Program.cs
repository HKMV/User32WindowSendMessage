using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AcceptApp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            String[] args = System.Environment.GetCommandLineArgs();
            //if (args.Contains("process"))
            //{
            //    Application.Run(new ProcessForm());
            //}
            //else
            //{
            //    Application.Run(new util.Form1());
            //}

            Application.Run(new ProcessForm());

        }
    }



}
