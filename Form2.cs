using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContorlCenter
{
    public partial class Form2 : Form
    {
        Form1 father = null;
        public Form2(Form1 f)
        {
            father = f;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            father.WindowState = FormWindowState.Minimized;

        }
        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标签是否为左键

        private void Form2_Load(object sender, EventArgs e)
        {
            father.WindowState = FormWindowState.Minimized;

        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }



        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                father.Location = mouseSet;
            }
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
