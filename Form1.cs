using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using WallpaperEngine;

namespace ContorlCenter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            OpenTopForm(new Form2(this));
            OpenMainChildForm(new Welcome());
            HideSubMenu();
            load_Setings();
            ReadLessons();
            Gen_bg();
        }
        /// <summary>
        /// 载入Settings XML 文档
        /// </summary>
        public void load_Setings()
        {
            XDocument document = XDocument.Load(System.Windows.Forms.Application.StartupPath + "\\" + "Settings.xml");
            Console.WriteLine(System.Windows.Forms.Application.StartupPath + "\\" + "Settings.xml");
            XElement rootElement = document.Root;
            int r = 0;
            int g = 0;
            int b = 0;
            var settings = rootElement.Elements("Settings");
            foreach (var item in settings.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "课表桌面": pv.Desukutop = (item.Value == "开启") ? true : false; break;
                    case "左上角顶点X": pv.Draw_X = int.Parse(item.Value); break;
                    case "左上角顶点Y": pv.Draw_Y = int.Parse(item.Value); break;
                    case "绘图步长": pv.Step_Length = int.Parse(item.Value); break;
                    case "字符大小": pv.Chara_Size = float.Parse(item.Value); break;
                    case "字符颜色R": r = int.Parse(item.Value); break;
                    case "字符颜色G": g = int.Parse(item.Value); break;
                    case "字符颜色B": b = int.Parse(item.Value); break;
                    case "图片URI": pv.Img_path = item.Value == "NULL" ? null : item.Value; break;
                    case "壁纸X": pv.bg_X = int.Parse(item.Value); break;
                    case "壁纸Y": pv.bg_Y = int.Parse(item.Value); break;
                    case "壁纸文字大小": pv.bg_chaca_size = int.Parse(item.Value); break;
                    default:
                        MessageBox.Show("Problem Occurred at" + item.Name + " ,with value of " + item.Value + "无法识别的标签名称");
                        break;
                }
            }
            pv.chara_color = Color.FromArgb(r, g, b);
            Console.WriteLine();
        }

        #region 自定义函数
        private Form ActiveForm1 = null;    //活动窗口
        private Form ActiveButtomForm = null;
        private Form ActiveTopForm = null;
        private Form ActiveLeftForm = null;
        private Form ActiveRightForm = null;
        private void OpenMainChildForm(Form ChildForm)
        {
            if (ActiveForm1 != null)
                ActiveForm1.Close();
            ActiveForm1 = ChildForm;
            ChildForm.TopLevel = false;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            ChildForm.Dock = DockStyle.Fill;
            MainChildFormPanel.Controls.Add(ChildForm);
            MainChildFormPanel.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }
        private void OpenButtomForm(Form ChildForm)
        {
            if (ActiveButtomForm != null)
                ActiveButtomForm.Close();
            ActiveButtomForm = ChildForm;
            ChildForm.TopLevel = false;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            ChildForm.Dock = DockStyle.Fill;
            panel4.Controls.Add(ChildForm);
            panel4.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }
        private void OpenTopForm(Form ChildForm)
        {
            if (ActiveTopForm != null)
                ActiveTopForm.Close();
            ActiveTopForm = ChildForm;
            ChildForm.TopLevel = false;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            ChildForm.Dock = DockStyle.Fill;
            panel1.Controls.Add(ChildForm);
            panel1.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }
        private void OpenLeftForm(Form ChildForm)
        {
            if (ActiveLeftForm != null)
                ActiveLeftForm.Close();
            ActiveLeftForm = ChildForm;
            ChildForm.TopLevel = false;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            ChildForm.Dock = DockStyle.Fill;
            panel1.Controls.Add(ChildForm);
            panel1.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }
        private void OpenRightForm(Form ChildForm)
        {
            if (ActiveRightForm != null)
                ActiveRightForm.Close();
            ActiveRightForm = ChildForm;
            ChildForm.TopLevel = false;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            ChildForm.Dock = DockStyle.Fill;
            panel1.Controls.Add(ChildForm);
            panel1.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }
        private void customize_design()
        {

        }
        private bool ReadLessons()
        {
            bool t = false;
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "lessons.csv";
            string imgpath = System.AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(path))
            {
                try
                {
                    string[] strs = System.IO.Directory.GetFiles(imgpath);
                    foreach (var file in strs)
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        if ((fi.Extension == ".png" || fi.Extension == ".jpg" || fi.Extension == ".gif" || fi.Extension == ".bmp") && fi.Name.Split('.')[0] == "desktop")
                        {
                            org_desktop = new Bitmap(fi.FullName);
                            org_desktop.SetResolution(600, 600);
                            break;
                        }
                    }

                    using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                    {
                        string[] s = sr.ReadToEnd().Replace("\r", "").Split('\n');
                        foreach (var item in s)
                        {
                            if (item.Length != 0)
                            {
                                string[] q = item.Split(',');
                                Lessons.Add(q);
                                DrawStr.Add(new string[q.Length]);
                            }
                        }

                        if (Lessons.Count != 7)
                        {
                            MessageBox.Show("不完整的课表.");
                        }
                        t = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "   comment: 在瞎搞?");
                    throw;
                }
            }
            else
            {
                MessageBox.Show("不存在lessons.csv/lessons_info.xml");
                return false;
            }
            return t;
        }
        private void HideSubMenu()
        {
            if (buttompanel1.Visible == true)
                buttompanel1.Visible = false;
            if (buttompanel2.Visible == true)
                buttompanel2.Visible = false;
            if (buttommenu3.Visible == true)
                buttommenu3.Visible = false;
        }
        private void ShowSubMenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                HideSubMenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }
        #endregion
        public static List<string[]> Lessons = new List<string[]>();
        List<string[]> DrawStr = new List<string[]>();
        Bitmap org_desktop = null;
        Bitmap new_desktop = null;
        //Point AnchorP = new Point(960, 100);
        //float step = 70;                      //px
        //float Font_size = 7;
        int week_num = 0;
        public void Gen_bg()
        {
            new_desktop = org_desktop;
            string imgpath = System.AppDomain.CurrentDomain.BaseDirectory;
            string week = DateTime.Now.DayOfWeek.ToString();

            switch (week)
            {
                case "Monday":
                    week_num = 1;
                    break;
                case "Tuesday":
                    week_num = 2;
                    break;
                case "Wednesday":
                    week_num = 3;
                    break;
                case "Thursday":
                    week_num = 4;
                    break;
                case "Friday":
                    week_num = 5;
                    break;
                case "Saturday":
                    week_num = 6;
                    break;
                case "Sunday":
                    week_num = 7;
                    break;
            }

            //PAINT

            DrawStr[week_num - 1][0] = Lessons[week_num - 1][0] + " " + DateTime.Now.ToShortDateString();
            DrawStr[week_num - 1][DrawStr[week_num - 1].Length - 1] = GetOneDayAWord();
            Lessons[week_num - 1][DrawStr[week_num - 1].Length - 1] = DrawStr[week_num - 1][DrawStr[week_num - 1].Length - 1];
            Font font = new Font("微软雅黑", pv.Chara_Size, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(pv.chara_color);

            Graphics g = Graphics.FromImage(org_desktop);
            PointF Draw_P = new PointF(0, 0);
            for (int i = 0; i < Lessons[week_num - 1].Length; i++)
            {
                Draw_P = new PointF(pv.Draw_X, pv.Draw_Y + pv.Step_Length * i);
                g.DrawString("静态壁纸已弃用,看见此消息请打开EZClass", font, drawBrush, Draw_P);
            }
            #region Countdown
            DateTime t1 = DateTime.ParseExact("20210106060000", "yyyyMMddhhmmss", System.Globalization.CultureInfo.CurrentCulture);
            DateTime t2 = DateTime.ParseExact("20210607060000", "yyyyMMddhhmmss", System.Globalization.CultureInfo.CurrentCulture);
            Draw_P = new PointF(1470, 379);
            font = new Font("微软雅黑", 10, FontStyle.Bold);

            g.DrawString(((int)(DateTime.Now - t2).Duration().TotalDays).ToString(), font, drawBrush, Draw_P);
            Draw_P = new PointF(1470, 470);
            g.DrawString(((int)(DateTime.Now - t1).Duration().TotalDays).ToString(), font, drawBrush, Draw_P);
            #endregion
           // new_desktop.Save(imgpath + "tmp.bmp", ImageFormat.Bmp);
          //  changeImg(imgpath + "tmp.bmp");
            StartWp3();
        }

        public string GetOneDayAWord()
        {
            redo:
            int attempts_cnt = 0;
                string s = "";
            try
            {
                string json = Get("http://sentence.iciba.com/index.php?c=dailysentence&m=getdetail&title=" + DateTime.Now.Year.ToString().PadLeft(2, '0') + '-' + DateTime.Now.Month.ToString().PadLeft(2, '0') + '-' + DateTime.Now.Day.ToString().PadLeft(2, '0'));
                //string json = Get("http://sentence.iciba.com/index.php?c=dailysentence&m=getdetail&title=2020-10-01");
                //Thread.Sleep(10000);
                if (json != "")
                {
                    JsonReader reader = new JsonTextReader(new StringReader(json));

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.String)
                        {
                            if ((string)reader.Path == "note")
                            {
                                s = (string)reader.Value;
                                return s;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                attempts_cnt++;
                System.Threading.Thread.Sleep(10000);
                if (attempts_cnt>10)
                {
                    MessageBox.Show("尝试获取每日一句失败。（原因：网络超时）");
                    return s;
                }
                goto redo;
            }
            return s;


        }
        public static string Get(string url)
        {
                string result = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
                stream.Close();
            }
            finally
            {

            }
            return result;
        }
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfoA")]
        static extern Int32 SystemParametersInfo(Int32 uAction, Int32 uParam, string lpvParam, Int32 fuWinIni);
        private void changeImg(string pat)
        {
            string bmpPath = pat;//新图片要存储的位置

            int nResult;

            if (File.Exists(bmpPath))
            {
                nResult = SystemParametersInfo(20, 1, bmpPath, 0x1 | 0x2); //更换壁纸
                if (nResult == 0)
                {
                    MessageBox.Show("没有更新成功!");
                }
                else
                {
                    RegistryKey hk = Registry.CurrentUser;
                    RegistryKey run = hk.CreateSubKey(@"Control Panel\Desktop\");
                    run.SetValue("Wallpaper", bmpPath);
                }
            }
            else
            {
                MessageBox.Show("文件不存在!");
            }
        }

        //bool SidePanShow = true;
        private void button3_Click(object sender, EventArgs e)
        {

            //if (SidePanShow)
            //{
            //    button3.Text = "+";
            //    panel5.Visible = false;
            //    button3.Size = new Size(50, 50);
            //}
            //else
            //{
            //    button3.Text = "关闭侧面板";
            //    panel5.Visible = true;
            //    button3.Size = new Size(155, 50);

            //}
            //SidePanShow=!SidePanShow;
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowSubMenu(buttompanel1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowSubMenu(buttompanel2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenMainChildForm(new DesukuTopCtrl(this));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenMainChildForm(new szk());
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ShowSubMenu(buttommenu3);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bakamashiro.xyz");
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenMainChildForm(new mainsettings());
        }

        public Form timeform = null;
        public WallpaperEngine.MainWindow Wpform2 = null;
        public Window1 Wpform3 = null;

        private void StartWp1()
        {
           
        }
        private void button10_Click(object sender, EventArgs e)
        {
            #region Wasted

            /*-
            PrintVisibleWindowHandles(2);
            IntPtr progman = W32.FindWindow("Progman", null);
            IntPtr result = IntPtr.Zero;
            W32.SendMessageTimeout(progman,
                                   0x052C,
                                   new IntPtr(0),
                                   IntPtr.Zero,
                                   W32.SendMessageTimeoutFlags.SMTO_NORMAL,
                                   1000,
                                   out result);
            PrintVisibleWindowHandles(2);
            IntPtr workerw = IntPtr.Zero;
            W32.EnumWindows(new W32.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = W32.FindWindowEx(tophandle,
                                            IntPtr.Zero,
                                            "SHELLDLL_DefView",
                                            IntPtr.Zero);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = W32.FindWindowEx(IntPtr.Zero,
                                               tophandle,
                                               "WorkerW",
                                               IntPtr.Zero);
                }

                return true;
            }), IntPtr.Zero);
            Form form = new Form();
            timeform = form;
            form.Text = "time";
            form.FormBorderStyle = FormBorderStyle.None;
            form.Load += new EventHandler((s, _e) =>
            {
                form.Width = 1920;
                form.Height = 1080;
                form.Left = 0;
                form.Top = 0;
                form.WindowState = FormWindowState.Maximized;
                form.BackgroundImageLayout = ImageLayout.Zoom;
                //form.TopMost = true;
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
                if (new_desktop != null)
                {
                    form.BackColor = Color.Red;
                    //form.BackgroundImage = new_desktop;
                }
                Label lab = new Label() { Text = "BakaWallpaper" };
                form.Controls.Add(lab);
                lab.Left = pv.bg_X;
                lab.Top = pv.bg_Y;
                lab.Font = new Font("微软雅黑", pv.bg_chaca_size);
                lab.AutoSize = true;
                lab.TextAlign = ContentAlignment.MiddleCenter;
                //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                //timer.Interval = 1000;
                //timer.Tick += new EventHandler((_sender, eventArgs) =>
                //{
                //    lab.Text = DateTime.Now.ToLongTimeString();
                //});
                //timer.Start();
                W32.SetParent(form.Handle, progman);
            });
            form.Show();
            -*/
            #endregion

            StartWp1();
        }

        static void PrintVisibleWindowHandles(IntPtr hwnd, int maxLevel = -1, int level = 0)
        {
            bool isVisible = W32.IsWindowVisible(hwnd);

            if (isVisible && (maxLevel == -1 || level <= maxLevel))
            {
                StringBuilder className = new StringBuilder(256);
                W32.GetClassName(hwnd, className, className.Capacity);

                StringBuilder windowTitle = new StringBuilder(256);
                W32.GetWindowText(hwnd, windowTitle, className.Capacity);

                Console.WriteLine("".PadLeft(level * 2) + "0x{0:X8} \"{1}\" {2}", hwnd.ToInt64(), windowTitle, className);

                level++;

                // Enumerates all child windows of the current window
                W32.EnumChildWindows(hwnd, new W32.EnumWindowsProc((childhandle, childparamhandle) =>
                {
                    PrintVisibleWindowHandles(childhandle, maxLevel, level);
                    return true;
                }), IntPtr.Zero);
            }
        }
        static void PrintVisibleWindowHandles(int maxLevel = -1)
        {
            // Enumerates all existing top window handles. This includes open and visible windows, as well as invisible windows.
            W32.EnumWindows(new W32.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                PrintVisibleWindowHandles(tophandle, maxLevel);
                return true;
            }), IntPtr.Zero);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (timeform != null)
            {
                try
                {
                    (timeform as Form).Close();
                    timeform = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"新阳®动态壁纸需要打开Windows7的areo和其他的动态效果(win10不受影响)。Windows 7 prerequisites
go to start -> search for Adjust the appearance and performance of windows and click it, then make sure at least these are checked
Animate controls and elements inside windows
Enable desktop composition
Use visual styles on windows and buttons
if you don't enable these settings, weebp won't be able to put your animated wallpaper behind the icons.it will still work, but it will cover your desktop icons ");
        }
        private void StartWp2()
        {
            WallpaperEngine.MainWindow mmm = new WallpaperEngine.MainWindow();
            mmm.Show();
            Wpform2 = mmm;
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\SyzWallaper.exe"))
            {
                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + @"\SyzWallaper.exe");
            }

        }

        private void StartWp3()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in Lessons[week_num - 1])
            {
                if (!(item is ""))
                {
                    sb.Append(item + Environment.NewLine);
                }
            }
            WallpaperWpf.MainWindow mainWindow = new WallpaperWpf.MainWindow(sb.ToString());
            mainWindow.Show();
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\SyzWallaper.exe"))
            {
                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + @"\SyzWallaper.exe");
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            StartWp2();
        }

        private void StopWp2()
        {
            if (Wpform2 != null)
            {
                try
                {
                    Wpform2.Close();
                    Wpform2 = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            StopWp2();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (Wpform3 != null)
            {
                try
                {
                    Wpform3.Close();
                    Wpform3 = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }
    }
}
