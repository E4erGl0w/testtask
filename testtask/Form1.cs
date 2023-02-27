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

namespace testtask
{

    public partial class Form1 : Form

    {
        List<string> lines = new List<string>();
        List<int> ints = new List<int>();
        List<int> supersorted = new List<int>();

        public string Pathh { get; set; }
        public decimal Ping { get; set; }
        public int TS { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Frames { get; set; }
        float xSub = 0;
        Graphics g;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        private Rectangle richTextBox1Original, richTextBox2Original, label1original, label6original;
        private Rectangle originalFormSize;

        public Form1()
        {
            InitializeComponent();




        }




        public void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "START";
            button2.Text = "CANCEL";
            textBox2.Text = "C:\\cc.csv";
            textBox1.Text = "500";
            originalFormSize = new Rectangle(this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height);
            richTextBox1Original = new Rectangle(richTextBox1.Location.X, richTextBox1.Location.Y, richTextBox1.Width, richTextBox1.Height);
            richTextBox2Original = new Rectangle(richTextBox2.Location.X, richTextBox2.Location.Y, richTextBox2.Width, richTextBox2.Height);
            label1original = new Rectangle(label1.Location.X, label1.Location.Y, label1.Width, label1.Height);
            label6original = new Rectangle(label6.Location.X, label6.Location.Y, label6.Width, label6.Height);
            g = CreateGraphics();
            g.DrawRectangle(Pens.Black, 0, 0, 750, 710);


        }
        async Task Draw(CancellationToken token)
        {
            int pingint = Convert.ToInt32(Ping);
            try
            {

                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                int ii, jj, ff;
                int counter = 0;
                g = CreateGraphics();

                for (int l = 0; l < Frames; l++)
                {




                    g.DrawRectangle(Pens.Black, xSub + 250, 5, 270 + xSub, 645);
                    ff = l + 1;
                    richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text += "Frame " + ff + "\n"));
                    richTextBox2.Invoke((MethodInvoker)(() => richTextBox2.Text += "Frame " + ff + "\n"));
                    int countTS = 0;

                    {
                        for (int i = 0; i < Rows; i++)
                        {

                            int j;
                            for (j = 0; j < Columns; j++)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    token.ThrowIfCancellationRequested();

                                }
                                ii = i + 1;
                                jj = j + 1;


                                if (supersorted[counter] > TS && supersorted[counter] <= 1.5 * TS)
                                {
                                    countTS++;
                                    g.FillRectangle(Brushes.Yellow, xSub / 2 + 251 + 15 * j, 21 + 15 * i, 14, 14);
                                    richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));
                                    richTextBox2.Invoke((MethodInvoker)(() => richTextBox2.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));
                                }
                                else if (supersorted[counter] > 1.5 * TS && supersorted[counter] <= 2 * TS)
                                {
                                    countTS++;
                                    g.FillRectangle(Brushes.Orange, xSub / 2 + 251 + 15 * j, 21 + 15 * i, 14, 14);
                                    richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));
                                    richTextBox2.Invoke((MethodInvoker)(() => richTextBox2.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));
                                }
                                else if (supersorted[counter] > 2 * TS)
                                {
                                    countTS++;
                                    g.FillRectangle(Brushes.Red, xSub / 2 + 251 + 15 * j, 21 + 15 * i, 14, 14);
                                    richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));
                                    richTextBox2.Invoke((MethodInvoker)(() => richTextBox2.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));
                                }
                                else
                                {
                                    g.FillRectangle(Brushes.White, xSub / 2 + 251 + 15 * j, 21 + 15 * i, 14, 14);
                                }

                                counter++;
                            }
                            j = 0;



                        }
                        countTS = 0;
                        Thread.Sleep(pingint);
                        richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text = ""));
                    }

                }
            }
            catch (System.ArgumentOutOfRangeException)
            {

            }
            catch
            {
                MessageBox.Show("Exception occurs");

            }



        }


        async void ReadData(string Pathh)
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


        }
        async private void button1_Click(object sender, EventArgs e)
        {
            CancellationToken token = new CancellationToken();
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

                await Task.Run(() => Draw(token));
            }
            catch (System.Reflection.TargetInvocationException)
            {

            }
            catch (System.FormatException)
            {
                MessageBox.Show("Error\nIncorrect file format or threshold");
            }
        
    
          
          

        }
        private void button2_Click(object sender, EventArgs e)
        {
            lines.Clear();
            supersorted.Clear();
            ints.Clear();
            tokenSource.Cancel();
            g = CreateGraphics();
            g.FillRectangle(Brushes.Black, xSub + 250, 5, 270 + xSub, 660);
            
        }
        
        private void Form1_Resize(object sender, EventArgs e)
        {
            g = CreateGraphics();
            g.FillRectangle(Brushes.Black, 0, 0, 750,710);
            resizeControl(richTextBox1Original, richTextBox1);
            resizeControl(richTextBox2Original, richTextBox2);
            resizeControl(label1original, label1);
            resizeControl(label6original, label6);
            
            
        }

        private void resizeControl(Rectangle r, Control c)
        {
            xSub = (float)this.Width - originalFormSize.Width;
            float ySub = (float)this.Height;
            int newX = (int)(r.X + xSub);
            int newY = (int)r.Y;
            int newWidth = (int)r.Width;
            int newHeight = (int)r.Height;
            c.Location = new Point(newX,newY);
            c.Size = new Size(newWidth, newHeight);
        }
       
        
    }
}

    





       
    


