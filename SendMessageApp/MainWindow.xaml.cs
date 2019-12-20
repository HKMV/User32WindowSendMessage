using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SendMessageApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 自定义数据
        /// </summary>
        public class Message
        {
            public const int USER = 0X0400;
            public const int WM_TEST = USER + 101;
            public const int WM_MSG = USER + 102;
            public const int PROC_MAXMIN = USER + 103;
            public const int PROC_VALUE = USER + 104;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartProc_Click(object sender, RoutedEventArgs e)
        {
            OpenProcess(@"../../../../AcceptApp\bin\Debug\AcceptApp.exe");
        }

        private void StopProc_Click(object sender, RoutedEventArgs e)
        {
            int hWnd = User32Utile.FindWindow(null, @"ProcessForm");
            if (hWnd <= 0)
            {
                MessageBox.Show("你怕是还没启动？？？");
                return;
            }
            CloseProcess(hWnd);
        }

        private void SendProcMessage_Click(object sender, RoutedEventArgs e)
        {
            int hWnd = User32Utile.FindWindow(null, @"ProcessForm");
            if (hWnd<=0)
            {
                MessageBox.Show("你怕是还没启动？？？");
                return;
            }

            string message = Mes.Text;
            string min = Min.Text;
            string max = Max.Text;
            string val = Val.Text;

            SendMsg(message, DataType.title, hWnd);
            SendMsg(min, DataType.min, hWnd);
            SendMsg(max, DataType.max, hWnd);
            SendMsg(val, DataType.value, hWnd);

            //SendMsg(message, DataType.title);
            //SendMsg(min, DataType.min);
            //SendMsg(max, DataType.max);
            //SendMsg(val, DataType.value); 
        }


        public enum DataType
        {
            title,
            max,
            min,
            value
        }


        public static IntPtr OpenProcess(string exePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = exePath;
            startInfo.Arguments = "process"; //传递的参数

            Process pro = new Process();
            pro.StartInfo = startInfo;
            pro.Start();        //开启
            IntPtr hWnd = pro.MainWindowHandle;    //进程的句柄
            return hWnd;
        }

        /// <summary>
        /// 干掉进度条进程
        /// </summary>
        /// <param name="hWnd"></param>
        public static void CloseProcess(int hWnd)
        {
            User32Utile.SendMessage(hWnd, Message.WM_TEST, 99, -1);//参数1、窗口句柄。2 、消息类型。 3、数据1。 4、数据2
        }


        #region 启动App并调整位置相关

        ///// <summary>
        ///// 打开-适用于Winform
        ///// </summary>
        ///// <param name="form"></param>
        ///// <returns></returns>
        //public static IntPtr OpenProcess(System.Windows.Forms.Form form)
        //{
        //    return OpenProcess(form.Location, form.Width, form.Height);
        //}
        ///// <summary>
        ///// 打开-适用于wpf
        ///// </summary>
        ///// <param name="window"></param>
        ///// <returns></returns>
        //public static IntPtr OpenProcess(System.Windows.Window window)
        //{
        //    Point point = new Point((int)window.Left, (int)window.Top);
        //    return OpenProcess(point, (int)window.Width, (int)window.Height);
        //}
        ///// <summary>
        ///// 打开
        ///// </summary>
        ///// <param name="point"></param>
        ///// <param name="width"></param>
        ///// <param name="height"></param>
        ///// <returns></returns>
        //public static IntPtr OpenProcess(Point point, int width, int height)
        //{
        //    ProcessStartInfo startInfo = new ProcessStartInfo();
        //    startInfo.FileName = @"C:\Users\86151\source\repos\DFDesign\YJMsgApp\YJMsgApp\bin\Debug\YJMsgApp.exe";
        //    startInfo.Arguments = "process"; //传递的参数

        //    Process pro = new Process();
        //    pro.StartInfo = startInfo;
        //    pro.Start();        //开启
        //    IntPtr hWnd = pro.MainWindowHandle;    //进程的句柄

        //    while (hWnd.ToInt32() > 0)
        //    {
        //        break;
        //    }
        //    if (point != null)
        //    {
        //        System.Threading.Thread.Sleep(800);
        //        //hWnd = new IntPtr(FindWindow(null, @"YJProcessForm"));
        //        hWnd = new IntPtr(User32Utile.FindWindow(null, @"DF-正在处理..."));

        //        int X = (point.X + width / 2) - 444 / 2;
        //        int Y = (point.Y + height / 2) - 109 / 2;
        //        User32Utile.MoveWindow(hWnd, X, Y, 444, 109, true);
        //    }
        //    return hWnd;
        //}

        #endregion 启动App并调整位置相关
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="hWnd"></param>
        public static void SendMsg(string message, DataType dataType, int hWnd = -1)
        {
            if (hWnd == -1)
                hWnd = (User32Utile.FindWindow(null, @"正在处理..."));

            int i = message.Length;
            byte[] sarr = System.Text.Encoding.Default.GetBytes(message);
            User32Utile.COPYDATASTRUCT cds;
            //cds.dataType = dataType.ToString();
            switch (dataType)
            {
                case DataType.title:
                    //cds.dataType = (IntPtr)1;
                    cds.dwData = (IntPtr)101;
                    break;
                case DataType.max:
                    //cds.dataType = (IntPtr)2;
                    cds.dwData = (IntPtr)102;
                    break;
                case DataType.min:
                    //cds.dataType = (IntPtr)3;
                    cds.dwData = (IntPtr)103;
                    break;
                case DataType.value:
                    //cds.dataType = (IntPtr)4;
                    cds.dwData = (IntPtr)104;
                    break;
                default:
                    //cds.dataType = (IntPtr)1;
                    cds.dwData = (IntPtr)101;
                    break;
            }
            //cds.dwData = (IntPtr)100;
            cds.lpData = message;
            cds.cbData = sarr.Length + 1;         //此值错误会引发接收端崩溃
            //MessageBox.Show("revit,"+cds.dwData.ToInt32()+","+cds.cbData);
            User32Utile.SendMessage(hWnd, User32Utile.WM_COPYDATA, 0, ref cds);
        }
    }
}
