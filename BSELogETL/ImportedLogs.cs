using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BSELogETL
{
    public partial class ImportedLogs : Form
    {
        public ImportedLogs(ConnectionService connectionService)
        {
            InitializeComponent();
            foreach (var log in connectionService.GetPushedLogs())
            {
                listBox1.Items.Add(log);
            }
        }
    }
}