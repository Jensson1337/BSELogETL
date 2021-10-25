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
            var filepath = string.Empty;
            var content = string.Empty;
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filepath = dialog.FileName;
                var fileStream = dialog.OpenFile();
                var reader = new StreamReader(fileStream);
                content = reader.ReadToEnd();
            }

            label1.Text = "Selected File: " + filepath;
        }

        //Show imported Logs
        private void button2_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        //Analyse 1
        private void button3_Click(object sender, EventArgs e)
        {
            var filterdialog = new An1_Filter();
            filterdialog.Show();
        }

        //Analyse 2
        private void button4_Click(object sender, EventArgs e)
        {
            var filterdialog = new An2_Filter();
            filterdialog.Show();
        }

        //Analyse 3
        private void button5_Click(object sender, EventArgs e)
        {
            var filterdialog = new An3_Filter();
            filterdialog.Show();
        }

        //Analyse 4
        private void button6_Click(object sender, EventArgs e)
        {
            var filterdialog = new An4_Filter();
            filterdialog.Show();
        }
    }
}