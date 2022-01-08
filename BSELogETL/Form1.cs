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
            // select files
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.RestoreDirectory = true;
            dialog.Multiselect = true;
            dialog.ShowDialog();

            // check if files selected
            if (dialog.FileNames.Length == 0)
            {
                MessageBox.Show("Please select at least one file.");
                return;
            }

            // remove non log files
            bool hasNonLogfile = false;
            var files = new List<string>();
            foreach (var fileName in dialog.FileNames)
            {
                if (!fileName.EndsWith(".log"))
                {
                    hasNonLogfile = true;
                    continue;
                }

                files.Add(fileName);
            }

            // message if non log files are selected
            if (hasNonLogfile && files.Count > 0)
            {
                MessageBox.Show("Please note that only logfiles will be imported.");
            }
            else if (hasNonLogfile && files.Count == 0)
            {
                MessageBox.Show("The selected files do not contain any log files.");
                return;
            }

            // set label
            if (files.Count == 1)
            {
                label1.Text = "Imported: " + Helper.GetBaseName(files[0]);
            }
            else
            {
                label1.Text = "Imported: Multiple";
            }

            ImportFiles(files);
        }

        //Show imported Logs
        private void button2_Click(object sender, EventArgs e)
        {
            var logDialog = new ImportedLogs(_connectionService);
            logDialog.Show();
        }

        //Analyse 1
        private void button3_Click(object sender, EventArgs e)
        {
            if ((Application.OpenForms["An1_Filter"] as An1_Filter) == null)
            {
                var filterDialog = new An1_Filter();
                filterDialog.Show();
            }
            else
            {
                MessageBox.Show("The dialog is already open", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Analyse 2
        private void button4_Click(object sender, EventArgs e)
        {
            if ((Application.OpenForms["An2_Filter"] as An2_Filter) == null)
            {
                var filterDialog = new An2_Filter();
                filterDialog.Show();
            }
            else
            {
                MessageBox.Show("The dialog is already open", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Analyse 3
        private void button5_Click(object sender, EventArgs e)
        {
            if ((Application.OpenForms["An3_Filter"] as An3_Filter) == null)
            {
                var filterDialog = new An3_Filter();
                filterDialog.Show();
            }
            else
            {
                MessageBox.Show("The dialog is already open", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Analyse 4
        private void button6_Click(object sender, EventArgs e)
        {
            if ((Application.OpenForms["An4_Filter"] as An4_Filter) == null)
            {
                var filterDialog = new An4_Filter();
                filterDialog.Show();
            }
            else
            {
                MessageBox.Show("The dialog is already open", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Add to Database
        private void ImportFiles(List<string> files)
        {
            var logs = _connectionService.GetPushedLogs();
            var importedFileNames = new List<string>();
            var importedEntries = new List<LogEntry>();
            
            foreach (var file in files)
            {
                string name = Helper.GetBaseName(file);
                if (logs.Contains(name))
                {
                    MessageBox.Show("The file " + name + " has already been imported.");
                    continue;
                }
                
                importedFileNames.Add(name);

                foreach (var line in File.ReadLines(file))
                {
                    importedEntries.Add(new LogEntry(line));
                }
            }

            _connectionService.PushFilenames(importedFileNames.ToArray());
            _connectionService.PushEntries(importedEntries.ToArray());

            MessageBox.Show("The action has been completed.", "",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}