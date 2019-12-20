using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AcceptApp
{
    public partial class ProcessForm : Form
    {
        public ProcessForm()
        {
            InitializeComponent();
            //System.Windows.Forms.SystemInformation.WorkingArea.;//获取屏幕大小(using System.Drawing)
            //int x = (System.Windows.Forms.SystemInformation.WorkingArea.Width - this.Size.Width) / 2;
            //int y = (System.Windows.Forms.SystemInformation.WorkingArea.Height - this.Size.Height) / 2;
            this.StartPosition = FormStartPosition.Manual; //窗体的位置由Location属性决定
            this.Location = (Point)new Size(40, 40);         //窗体的起始位置为(x,y)

        }
        const int WM_COPYDATA = 0x004A;
        const int WM_MYSYMPLE = 0x005A;

        protected override void DefWndProc(ref System.Windows.Forms.Message m)          //重构此函数接收数据
        {
            //MessageBox.Show(string.Format("1.{0},{1},{2}", m.Msg, m.WParam, m.LParam));
            switch (m.Msg)
            {
                case Message.WM_TEST:  //确定类型后，处理数据
                    //string message = string.Format("收到消息！参数为：{0},{1},{2}", m.Msg, m.WParam, m.LParam);
                    string message = string.Format("{0},{1},{2}", m.Msg, m.WParam, m.LParam);
                    label1.Text = message;
                    if (m.LParam.ToInt32() == -1)
                    {
                        Application.Exit();
                    }

                    break;
                case WM_COPYDATA:
                    COPYDATASTRUCT myStr = new COPYDATASTRUCT();
                    Type myType = myStr.GetType();
                    myStr = (COPYDATASTRUCT)m.GetLParam(myType);    //m中获取LParam参数以myType类型的方式，让后转换问结构体。
                    //MessageBox.Show("dwdata:"+myStr.dwData.ToInt32()+
                    //                ",cbdata:"+myStr.cbData+
                    //                ",lp:"+myStr.lpData);
                    switch (myStr.dwData.ToInt32())
                    {
                        case 101://"title"
                            {
                                /*MessageBox.Show("2.title,Lp:" + m.LParam +
                                                ",LpInt32:" + m.LParam.ToInt32().ToString() +
                                                ",Wp:" + m.WParam);*/
                                label1.Text = myStr.lpData;
                                break;
                            }
                        case 102://"max"
                            {

                                progressBar1.Maximum = int.Parse(myStr.lpData);
                                break;
                            }
                        case 103://"min"
                            {
                                progressBar1.Minimum = int.Parse(myStr.lpData);
                                break;
                            }
                        case 104://"value"
                            {
                                progressBar1.Value = int.Parse(myStr.lpData);
                                break;
                            }
                    }
                    break;
                //case Message.PROC_MAXMIN:
                //    MessageBox.Show("3.minmax,Lp" + m.LParam + ",Wp" + m.WParam);
                //    COPYDATASTRUCT procMaxMin = new COPYDATASTRUCT();
                //    procMaxMin = (COPYDATASTRUCT)m.GetLParam(procMaxMin.GetType());    //m中获取LParam参数以myType类型的方式，让后转换问结构体。
                //    //MessageBox.Show(procMaxMin.lpData);
                //    //string[] maxMin = procMaxMin.lpData.Split(',');
                //    //progressBar1.Maximum = int.Parse(maxMin[1]);
                //    //progressBar1.Minimum = int.Parse(maxMin[0]);
                //    label1.Text = procMaxMin.lpData;
                //    break;
                //case Message.PROC_VALUE:
                //    MessageBox.Show("4.value");
                //    COPYDATASTRUCT procValue = new COPYDATASTRUCT();
                //    Type myType2 = procValue.GetType();
                //    procValue = (COPYDATASTRUCT)m.GetLParam(myType2);    //m中获取LParam参数以myType类型的方式，让后转换问结构体。
                //    //progressBar1.Value = int.Parse(procValue.lpData);
                //    label1.Text = procValue.lpData;
                //    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }
    }


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
    public enum DataType
    {
        title,
        max,
        min,
        value
    }

    #region 交互数据 详细请看User32Utile

    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;//用户定义数据
        public int cbData;//数据大小 
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData; //指向数据的指针
        //[MarshalAs(UnmanagedType.LPStr)]
        //public IntPtr dataType;
    }

    #endregion 交互数据 详细请看User32Utile
    

}
