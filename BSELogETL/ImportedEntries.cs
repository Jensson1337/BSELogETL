using System.Collections.Generic;
using System.Windows.Forms;

namespace BSELogETL
{
    public partial class ImportedEntries : Form
    {
        public ImportedEntries(List<LogEntry> entries, List<string> attributes)
        {
            InitializeComponent();
            label1.Text = "Loaded entries: " + entries.Count;
            
            var availableAttributes = Helper.GetAvailableAttributes();
            dataGridView1.ColumnCount = attributes.Count;

            for (var index = 0; index < dataGridView1.ColumnCount; index++)
            {
                dataGridView1.Columns[index].Name = availableAttributes[attributes[index]];
                dataGridView1.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            foreach (var entry in entries)
            {
                var entryList = new List<object>();
                
                foreach (var attribute in attributes)
                {
                    entryList.Add(entry.GetType().GetProperty(attribute)?.GetValue(entry, null).ToString());
                }

                dataGridView1.Rows.Add(entryList.ToArray());
            }
        }
    }
}