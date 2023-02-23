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
        public int Ping { get; set; }
        public int TS { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Frames { get; set; }
        public int Area { get; set; }

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        CancellationToken token { get; set; }
        
        public Form1()
        {
            InitializeComponent();


           

        }

       
        Graphics g;

        public void Form1_Load(object sender, EventArgs e)
        {
            
            textBox2.Text = "C:\\c.csv";
            button1.Text = "START";
            button2.Text = "CANCEL";
            


        }
        async Task Draw(CancellationToken token)
        {

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }



                int ii, jj, ff;
                int counter = 0;

                for (int l = 0; l < Frames; l++)
                {
                    


                    ff = l + 1;



                    richTextBox1.Invoke((MethodInvoker)(() => label4.Text = "Frame " + ff));
                    richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text += "Frame " + ff + "\n"));
                    richTextBox2.Invoke((MethodInvoker)(() => richTextBox2.Text += "Frame " + ff + "\n"));


                    int countTS = 0;

                    {
                        for (int i = 0; i < 43; i++)
                        {

                            int j;
                            for (j = 0; j < 18; j++)
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
                                    g.FillRectangle(Brushes.Yellow, 251 + 15 * j, 21 + 15 * i, 14, 14);
                                    richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));
                                    richTextBox2.Invoke((MethodInvoker)(() => richTextBox2.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));



                                }

                                else if
                                   (supersorted[counter] > 1.5 * TS && supersorted[counter] <= 2 * TS)
                                {
                                    countTS++;
                                    g.FillRectangle(Brushes.Orange, 251 + 15 * j, 21 + 15 * i, 14, 14);

                                    richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));
                                    richTextBox2.Invoke((MethodInvoker)(() => richTextBox2.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));



                                }
                                else if
                                        (supersorted[counter] > 2 * TS)
                                {
                                    countTS++;
                                    g.FillRectangle(Brushes.Red, 251 + 15 * j, 21 + 15 * i, 14, 14);
                                    richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));
                                    richTextBox2.Invoke((MethodInvoker)(() => richTextBox2.Text += "[" + countTS + "] " + "X = " + jj + " Y = " + ii + " num: " + supersorted[counter] + "\n"));


                                }



                                else
                                {
                                    g.FillRectangle(Brushes.White, 251 + 15 * j, 21 + 15 * i, 14, 14);
                                }

                                counter++;


                            }
                            j = 0;



                        }
                        countTS = 0;
                        Thread.Sleep(Ping);
                        richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text = ""));
                    }

                }
            }
            catch(System.ArgumentOutOfRangeException)
            {
                
            }
            catch
            {
            }

            
        }
        async void ReadData(string Pathh)
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
        async private void button1_Click(object sender, EventArgs e)
            {
            CancellationToken token = new CancellationToken();
            try
                {


                Pathh = textBox2.Text;
                Ping = int.Parse(textBox3.Text);
                TS = int.Parse(textBox1.Text);
                await Task.Run(() => ReadData(Pathh));
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
                    g = CreateGraphics();
                    g.DrawRectangle(Pens.Black, 250, 5, 270, 645);

                    Area = Rows * Columns;
                    


                    await Task.Run(() => Draw(token));
                
                   






                }

                catch (System.ArgumentNullException)
                {
                    MessageBox.Show("Incorrect data fields");
                }
                catch(System.IO.FileNotFoundException)
                {
                    MessageBox.Show("Incorrect data fields");
                }
                catch (System.FormatException)
                {
                    MessageBox.Show("Incorrect data fields");
                }
                catch(System.ArgumentException)
                {
                    MessageBox.Show("Incorrect data fields");
                }








            }
        private void button2_Click(object sender, EventArgs e)
        {
            lines.Clear();
            supersorted.Clear();
            ints.Clear();
            
            tokenSource.Cancel();
            
            
        }
    }
    }

    





       
    


