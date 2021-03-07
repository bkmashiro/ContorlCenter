using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ContorlCenter
{
    public partial class DesukuTopCtrl : Form
    {
        private Form1 father = null;
        public DesukuTopCtrl(Form1 f)
        {
            InitializeComponent();
            father = f;
        }

        private void DesukuTopCtrl_Load(object sender, EventArgs e)
        {
            LoadXML();
            label7.ForeColor = pv.chara_color;
            label8.ForeColor = pv.chara_color;
        }

        private void LoadXML()
        {
            XDocument document = XDocument.Load("Settings.xml");
            XElement rootElement = document.Root;
            int r = 0;
            int g = 0;
            int b = 0;
            var settings = rootElement.Elements("Settings");
            foreach (var item in settings.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "课表桌面": if (item.Value == "开启") { checkBox1.Checked = true; } else { checkBox1.Checked = false; }; break;
                    case "左上角顶点X": textBox1.Text = item.Value; break;
                    case "左上角顶点Y": textBox2.Text = item.Value; break;
                    case "绘图步长": textBox3.Text = item.Value; break;
                    case "字符大小": textBox4.Text = item.Value; break;
                    case "字符颜色R": textBox5.Text = item.Value; break;
                    case "字符颜色G": textBox6.Text = item.Value; break;
                    case "字符颜色B": textBox7.Text = item.Value; break;
                    case "图片URI": textBox8.Text = item.Value; break;
                    case "壁纸X": textBox9.Text = item.Value; break;
                    case "壁纸Y": textBox11.Text = item.Value; break;
                    case "壁纸文字大小": textBox10.Text = item.Value; break;
                    default:
                        MessageBox.Show("Problem Occurred at" + item.Name + " ,with value of " + item.Value + "无法识别的标签名称");
                        break;
                }
            }
            pv.chara_color = Color.FromArgb(r, g, b);
            Console.WriteLine();
        }
        private void GenXML()
        {
            XDocument xDoc = new XDocument();
            XDeclaration xDec = new XDeclaration("1.0", "utf-8", null);
            // 设置文档定义
            xDoc.Declaration = xDec;
            //2、创建根节点
            XElement rootElement = new XElement("Baka");
            xDoc.Add(rootElement);
            //3、循环创建节点

            XElement PersonElement = new XElement("Settings");
            PersonElement.SetElementValue("课表桌面", checkBox1.Checked?"开启":"FALSE");
            PersonElement.SetElementValue("左上角顶点X", textBox1.Text);
            PersonElement.SetElementValue("左上角顶点Y", textBox2.Text);
            PersonElement.SetElementValue("绘图步长", textBox3.Text);
            PersonElement.SetElementValue("字符大小", textBox4.Text);
            PersonElement.SetElementValue("字符颜色R", pv.chara_color.R.ToString());
            PersonElement.SetElementValue("字符颜色G", pv.chara_color.G.ToString());
            PersonElement.SetElementValue("字符颜色B", pv.chara_color.B.ToString());
            PersonElement.SetElementValue("图片URI", textBox8.Text);
            PersonElement.SetElementValue("壁纸X", textBox9.Text);
            PersonElement.SetElementValue("壁纸Y", textBox11.Text);
            PersonElement.SetElementValue("壁纸文字大小", textBox10.Text);
            rootElement.Add(PersonElement);
            xDoc.Save(System.Windows.Forms.Application.StartupPath + "\\" + "Settings.xml");
            Console.WriteLine("ok");
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenXML();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pv.chara_color = colorDialog1.Color;
                textBox5.Text = pv.chara_color.R.ToString();
                textBox6.Text = pv.chara_color.G.ToString();
                textBox7.Text = pv.chara_color.B.ToString();
                label7.ForeColor = pv.chara_color;
                label8.ForeColor = pv.chara_color;

            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            int q;
            if (int.TryParse(textBox4.Text,out q))
            {
                if (q>0)
                {
                    label7.Font = new Font("微软雅黑",q);
                    label8.Font = new Font("微软雅黑", q,FontStyle.Italic);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            father.load_Setings();
            father.Gen_bg();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
