using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContorlCenter
{
    public partial class szk : Form
    {
        public szk()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文本文档（*.txt)";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string localFilePath = sfd.FileName.ToString();
                    StreamWriter FileWriter = new StreamWriter(localFilePath, true); //写文件
                    FileWriter.Write(richTextBox1.Text);
                    FileWriter.Flush();
                    FileWriter.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }

        CancellationTokenSource source = new CancellationTokenSource();
        bool GenRndFin = false;
        private void button2_Click(object sender, EventArgs e)
        {
            source = new CancellationTokenSource();
            //注册任务取消的事件
            source.Token.Register(() =>
            {
                if (!GenRndFin)
                {
                    this.Invoke(new Action(() =>
                    {
                        richTextBox1.AppendText("由于人为原因，生成任务被取消。/ 已完成的生成任务残余进程被人为退出。" + Environment.NewLine);
                        richTextBox1.AppendText("这可能是由于范围过小，已经没有可以再生成的数字导致的。" + Environment.NewLine);
                    }));
                }
            });

            GenRndFin = false;
            try
            {
                int min = (int)numericUpDown1.Value;
                int max = (int)numericUpDown2.Value;
                int number = (int)numericUpDown3.Value;
                List<int> target = new List<int>();


                //开启一个task执行任务
                Task task1 = new Task(() =>
                {
                    try
                    {
                        Console.WriteLine("1");
                        bool AllowSame = checkBox1.Checked;
                        Random rd = new Random();
                        int generated = 0;
                        while ((!source.IsCancellationRequested) && (generated < number))
                        {
                            if (AllowSame)
                            {
                                ++generated;
                                int tmp = rd.Next(min, max);
                                target.Add(tmp);
                                this.Invoke(new Action(() =>
                                {
                                    richTextBox1.AppendText("编号" + generated.ToString().PadLeft(7) + "  :" + tmp.ToString().PadRight(10) + Environment.NewLine);
                                }));
                            }
                            else
                            {
                                int tmp = rd.Next(min, max);
                                if (!target.Contains(tmp))
                                {
                                    ++generated;
                                    target.Add(tmp);
                                    this.Invoke(new Action(() =>
                                    {
                                        richTextBox1.AppendText("编号" + generated.ToString().PadLeft(7) + "  :" + tmp.ToString().PadRight(10) + Environment.NewLine);
                                    }));
                                }
                            }
                        }
                        GenRndFin = true;
                        
                        this.Invoke(new Action(() =>
                        {
                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                            richTextBox1.ScrollToCaret();
                        }));
                    }
                    catch (Exception ex)
                    {
                        richTextBox1.AppendText("错误（不要乱来）:" + ex.Message);
                        target.Clear();
                        throw;
                    }

                });
                task1.Start();
                //延时取消，效果等同于Thread.Sleep(5000);source.Cancel();
                try
                {
                    source.CancelAfter(5000);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            catch (Exception)
            {

                throw;
            }
            richTextBox1.AppendText("===========" + Environment.NewLine);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            source.Cancel();
        }

        private void szk_Load(object sender, EventArgs e)
        {


        }
    }
}
