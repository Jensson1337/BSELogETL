using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BSELogETL
{
    public partial class ImportedLogs : Form
    {
        private readonly ConnectionService _connectionService;

        public ImportedLogs(ConnectionService connectionService)
        {
            _connectionService = connectionService;
            InitializeComponent();

            foreach (var log in _connectionService.GetPushedLogs())
            {
                listBox1.Items.Add(log);
            }
        }
    }
}