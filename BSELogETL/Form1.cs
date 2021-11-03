using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace BSELogETL
{
    public partial class Form1 : Form
    {
        //private bool _fileSelected;
        private readonly ConnectionService _connectionService;
        private string[] _files;

        public Form1(
            ConnectionService connectionService)
        {
            _connectionService = connectionService;
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
            dialog.Multiselect = true;


            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filepath = dialog.FileName;
                var fileStream = dialog.OpenFile();
                var reader = new StreamReader(fileStream);
            }

            if (dialog.SafeFileName != "")
            {
                if (dialog.SafeFileNames.Length > 1)
                {
                    bool clear = true;
                    foreach (var file in dialog.SafeFileNames)
                    {
                        if (!file.EndsWith(".log"))
                        {
                            clear = false;
                        }
                    }

                    if (!clear)
                    {
                        MessageBox.Show("Please note that only logfiles will be transmitted to db", "Attention",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    label1.Text = "Selected: Multiple";
                    //_fileSelected = true;
                }
                else
                {
                    if (!dialog.SafeFileName.EndsWith(".log"))
                    {
                        MessageBox.Show("Please only select Logfiles", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        label1.Text = "Selected: " + filepath;
                        //_fileSelected = true;
                    }
                }

                _files = dialog.SafeFileNames;
            }
        }

        //Show imported Logs
        private void button2_Click(object sender, EventArgs e)
        {
            List<string> Logs = _connectionService.GetPushedLogs();
            var logDialog = new ImportedLogs(Logs);
            logDialog.Show();
        }

        //Analyse 1
        private void button3_Click(object sender, EventArgs e)
        {
            var filterDialog = new An1_Filter();
            filterDialog.Show();
        }

        //Analyse 2
        private void button4_Click(object sender, EventArgs e)
        {
            var filterDialog = new An2_Filter();
            filterDialog.Show();
        }

        //Analyse 3
        private void button5_Click(object sender, EventArgs e)
        {
            var filterDialog = new An3_Filter();
            filterDialog.Show();
        }

        //Analyse 4
        private void button6_Click(object sender, EventArgs e)
        {
            var filterDialog = new An4_Filter();
            filterDialog.Show();
        }

        //Add to Database
        private void button7_Click(object sender, EventArgs e)
        {
            if (_files == Array.Empty<string>())
            {
                MessageBox.Show("Please select a file", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                List<string> Logs = _connectionService.GetPushedLogs();
                List<LogEntry> EntryList = new List<LogEntry> { };
                var entries = new LogEntry[] { };
                foreach(var log in Logs)
                {
                    LogEntry file = new LogEntry
                    {
                        IpAddress = "",
                        HttpMethod = "",
                        HttpLocation = "",
                        HttpCode = "",
                        PackageSize = "",
                        RequestedAt = ""
                    };
                    EntryList.Add(file);
                }
                var pushed = _connectionService.PushEntries(entries);
                if (pushed)
                {
                    _connectionService.PushFilenames(_files);
                    MessageBox.Show("The file has been added to the database", "Success!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }
    }
}