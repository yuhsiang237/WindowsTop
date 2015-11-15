using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Runtime.InteropServices;

namespace windowTop
{

    public partial class Form1 : Form
    {
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_SHOWWINDOW = 0x0040;

        IntPtr beforeHwnd = new IntPtr(-1);

        enum WindwosState : int { Hook = 0, NotHook = 1, Normal = 2 };

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            IntPtr handle = FindWindow(null, comboBox1.Text);

            if (handle != IntPtr.Zero)
            {
                try
                {
                    SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);

                    if ((int)beforeHwnd == -1)
                    {
                        beforeHwnd = handle;
                    }
                    else
                    {
                        if (beforeHwnd != handle)
                        {
                            SetWindowPos(beforeHwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
                            beforeHwnd = handle;
                        }
                    }
                    msgState((int)WindwosState.Hook);
                }
                catch (Exception ex)
                {
                    msgState((int)WindwosState.NotHook);
                }
            }
            else
            {
                msgState((int)WindwosState.NotHook);
            }
        }
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            ((ComboBox)sender).Items.Clear();
            Process[] procs = Process.GetProcesses();
            foreach (Process proc in procs)
            {
                if (proc.MainWindowTitle != "")
                {
                    ((ComboBox)sender).Items.Add(proc.MainWindowTitle);
                }

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox1.Items.Count; SetWindowPos((FindWindow(null, comboBox1.Items[i++].ToString())), HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE)) ;
            msgState((int)WindwosState.Normal);
        }

        private void msgState(int i)
        {
            switch (i)
            {
                case 0:
                    Msg.Text = "目前至頂視窗" + comboBox1.Text;
                    break;
                case 1:
                    Msg.Text = "並未至頂任何視窗，請確認錯誤";
                    break;
                case 2:
                    Msg.Text = "所有視窗已恢復原始狀態";
                    break;
                default:
                    break;
            }
        }
    }
}
