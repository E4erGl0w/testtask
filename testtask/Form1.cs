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

namespace testtask
{
    public partial class Form1 : Form
    {
        public static string path;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                //int threshold = Int32.Parse(textBox2.Text);
                path = @"C:\users\marsa\c.csv";
                List<string> lines = new List<string>();
                List<int> ints = new List<int>();
                List<int> supersorted = new List<int>();
                List<int> hth= new List<int>();


            using (var reader = new StreamReader(path))
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
            int rows = int.Parse(lines[0]);
            int columns = int.Parse(lines[1]);
            foreach (var item in lines)
            {
                int.TryParse(item, out int num);
                ints.Add(num);
            }


            for (int r = rows*columns; r < ints.Count; r++)
            {
                supersorted.Add(ints[r]);

            }
            int frames = supersorted.Count / (rows * columns);
            //int count = 0;
            //do
            //{
            //    for (int j = 0; j < columns; j++)
            //    {
            //        richTextBox1.Text += supersorted[count].ToString() + "      ";
            //        count++;

            //    }
            //    richTextBox1.Text += "\n";
            //}
            //while (count < supersorted.Count);
            richTextBox1.Text += supersorted.Count.ToString() + " " + frames.ToString();
            

            //foreach (int num in hth)
            //{
            //    richTextBox1.Text += num.ToString();
            //}
            //richTextBox1.Text = lines.Count.ToString();





        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
