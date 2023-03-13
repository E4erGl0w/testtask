using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;
using static System.Windows.Forms.LinkLabel;
using System.Threading;
using System.CodeDom.Compiler;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using System.Reflection;

namespace testtask
{

    public partial class Form1 : Form

    {
        List<string> lines = new List<string>();
        List<int> ints = new List<int>();
        List<int> supersorted = new List<int>();
        List<int> rows1 = new List<int>();
        List<int> col1 = new List<int>();
        List<int> rows2 = new List<int>();
        List<int> col2 = new List<int>();
        List<int> address = new List<int>();
        
        public string Pathh { get; set; }
        public decimal Ping { get; set; }
        public int TS { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Frames { get; set; }
        //float xSub = 0;
        Graphics g;
        //private Rectangle richTextBox1Original, richTextBox2Original, label1original, label6original;
        //private Rectangle originalFormSize;
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "START";
            button2.Text = "CANCEL";
            //originalFormSize = new Rectangle(this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height);
            //richTextBox1Original = new Rectangle(richTextBox1.Location.X, richTextBox1.Location.Y, richTextBox1.Width, richTextBox1.Height);
            //richTextBox2Original = new Rectangle(richTextBox2.Location.X, richTextBox2.Location.Y, richTextBox2.Width, richTextBox2.Height);
            //label1original = new Rectangle(label1.Location.X, label1.Location.Y, label1.Width, label1.Height);
            //label6original = new Rectangle(label6.Location.X, label6.Location.Y, label6.Width, label6.Height);
            g = CreateGraphics();
            g.DrawRectangle(Pens.Black, 0, 0, 750, 710);
        }
        void Draw()
        {
            int pingint = Convert.ToInt32(Ping);
            try
            {
                int ii, jj, ff;
                int counter = 0;
                g = CreateGraphics();
                for (int l = 0; l < Frames; l++)
                {
                    g.DrawRectangle(Pens.White, 250, 5, 270, 645);
                    ff = l + 1;
                    richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text += "Frame " + ff + "\n"));
                    richTextBox2.Invoke((MethodInvoker)(() => richTextBox2.Text += "Frame " + ff + "\n"));
                    {
                        for (int i = 0; i < Rows; i++)
                        {
                            int j;
                            for (j = 0; j < Columns; j++)
                            {
                                ii = i + 1;
                                jj = j + 1;
                                if(supersorted[counter] > TS)
                                {
                                    col1.Add(j);
                                    rows1.Add(i);
                                    address.Add(counter);
                                }
                                counter++;
                            }
                            j = 0;
                        }
                        if (address.Count > 0)
                        {
                            Task.Run(() => Draw1());
                        }
                        Thread.Sleep(pingint);
                        richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text = ""));
                    }
                    g.FillRectangle(Brushes.White, 251, 6, 269, 644);
                }
                lines.Clear();
                supersorted.Clear();
                ints.Clear();
            }

            catch (System.ArgumentOutOfRangeException)
            {
            }
            catch
            {
                //MessageBox.Show("Exception occurs");
            }
        }
        async void Draw1()
        {
            int sumX = 0;
            int sumY = 0;
            int sumX2 = 0;
            int sumY2 = 0;
            int trigger = 1;
            int temp = 0;
            
            for(int i = 1; i < address.Count; i++)
            {
                if (address[i] - address[i - 1] > Columns*2)
                {
                    temp = i;
                    trigger++;
                }
                else
                    continue;
            }
            if (trigger == 1)
            {
                float drawX = 0;
                float drawY = 0;
                foreach (int num in col1)
                {
                    sumX += num;
                }
                drawX = sumX / col1.Count();
                foreach (int num in rows1)
                {
                    sumY += num;
                }
                drawY = sumY / rows1.Count();
                g.FillEllipse(Brushes.Black, 236 + 15 * drawX, -9 + 15 * drawY, 15, 15);
                await Task.Run(() => Print(richTextBox1, drawX, drawY));
                await Task.Run(() => Print(richTextBox2, drawX, drawY));
                col1.Clear();
                rows1.Clear();
                address.Clear();
                sumX = 0; sumY = 0;
                drawX = 0; drawY = 0;
            }
            else
            {
                int i;
                for(i = temp; i < address.Count; i++)
                {
                    rows2.Add(rows1[i]);
                    col2.Add(col1[i]);
                }
                for (i = temp; i < address.Count; i++)
                {
                    col1.Remove(i);
                    rows1.Remove(i);
                }
                i = 0;
                float drawX1 = 0;
                float drawY1 = 0;
                float drawX2 = 0;
                float drawY2 = 0;
                foreach (int num in col1)
                {
                    sumX += num;
                }
                drawX1 = sumX / col1.Count();
                foreach (int num in rows1)
                {
                    sumY += num;
                }
                drawY1 = sumY / rows1.Count();
                foreach (int num in col2)
                {
                    sumX2 += num;
                }
                drawX2 = sumX2 / col2.Count();
                foreach (int num in rows2)
                {
                    sumY2 += num;
                }
                drawY2 = sumY2 / rows2.Count();
                g.FillEllipse(Brushes.Black, 236 + 15 * drawX1, -9 + 15 * drawY1, 15, 15);
                g.FillEllipse(Brushes.Black, 236 + 15 * drawX2, -9 + 15 * drawY2, 15, 15);
                await Task.Run(() => Print(richTextBox1, drawX1, drawY1));
                await Task.Run(() => Print(richTextBox2, drawX1, drawY1));
                await Task.Run(() => Print(richTextBox1, drawX2, drawY2));
                await Task.Run(() => Print(richTextBox2, drawX2, drawY2));
                col1.Clear();
                rows1.Clear();
                col2.Clear();
                rows2.Clear();
                address.Clear();
                sumX = 0; sumY = 0; sumX2 = 0; sumY2 = 0;
            }
            temp = 0;
            trigger = 1;
        }
        void Print(Control c, float x, float y)
        {
            c.Invoke((MethodInvoker)(() => c.Text += "X = " + x + " Y = " + y + " num: " + "\n"));
        }
        void ReadData(string Pathh)
        {
            try
            {
                using (var reader = new StreamReader(Pathh))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        foreach (var item in values)
                        {
                            lines.Add(item);
                        }
                    }

                }
            }
            catch (System.ArgumentException)
            {
                MessageBox.Show("Error.\n Fill path field");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("Error. \nIncorrect path");
            }
            catch (System.Reflection.TargetInvocationException)
            {

            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Error. \nIncorrect path");
            }
            catch
            {

            }


        }
        async private void button1_Click(object sender, EventArgs e)
        {
            Pathh = textBox2.Text;
            Ping = numericUpDown1.Value;
            try
            {
                await Task.Run(() => ReadData(Pathh));
                TS = int.Parse(textBox1.Text);
                Rows = int.Parse(lines[0]);
                Columns = int.Parse(lines[1]);
                foreach (var item in lines)
                {
                    int.TryParse(item, out int num);
                    ints.Add(num);
                }
                for (int r = Rows * Columns; r < ints.Count; r++)
                {
                    supersorted.Add(ints[r]);
                }
                Frames = supersorted.Count / (Rows * Columns);
                await Task.Run(() => Draw());
                lines.Clear();
            }
            catch (System.Reflection.TargetInvocationException)
            {
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Error\nIncorrect file format or threshold");
            }
            catch
            {
            }
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            lines.Clear();
            supersorted.Clear();
            ints.Clear();
            g = CreateGraphics();
            g.FillRectangle(Brushes.Black,250, 5, 271, 660);
        }
        //private void Form1_Resize(object sender, EventArgs e)
        //{
        //    g.Clear(Color.Black);
        //    resizeControl(richTextBox1Original, richTextBox1);
        //    resizeControl(richTextBox2Original, richTextBox2);
        //    resizeControl(label1original, label1);
        //    resizeControl(label6original, label6);
        //}
        //private void resizeControl(Rectangle r, Control c)
        //{
        //    xSub = (float)this.Width - originalFormSize.Width;
        //    float ySub = (float)this.Height;
        //    int newX = (int)(r.X + xSub);
        //    int newY = (int)r.Y;
        //    int newWidth = (int)r.Width;
        //    int newHeight = (int)r.Height;
        //    c.Location = new Point(newX, newY);
        //    c.Size = new Size(newWidth, newHeight);
        //}
    }
}