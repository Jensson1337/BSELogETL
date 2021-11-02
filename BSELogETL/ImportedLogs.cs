using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BSELogETL
{
    public partial class ImportedLogs : Form
    {
        private readonly List<string> _logs;
        public ImportedLogs(List<string> logs)
        {
            _logs = logs;
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.MultiColumn = true;
            foreach (var log in _logs)
            {
                listBox1.Items.Add(log);
            }
        }
    }
}