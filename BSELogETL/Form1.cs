using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BSELogETL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        //Select Logfile
        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.RestoreDirectory = true;

            var file = dialog.OpenFile();
            var reader = new StreamReader(file);
            var content = reader.ReadToEnd();
            Console.WriteLine(content);
        }

        //Show imported Logs
        private void button2_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        //Analyse 1
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Run(new An1_Filter());
        }

        //Analyse 2
        private void button4_Click(object sender, EventArgs e)
        {
            Application.Run(new An2_Filter());
        }

        //Analyse 3
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Run(new An3_Filter());
        }

        //Analyse 4
        private void button6_Click(object sender, EventArgs e)
        {
            Application.Run(new An4_Filter());
        }
    }
}