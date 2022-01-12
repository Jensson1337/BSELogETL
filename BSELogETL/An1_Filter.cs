using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BSELogETL
{
    public partial class An1_Filter : Form
    {
        private readonly ConnectionService _connectionService;

        public An1_Filter(ConnectionService connectionService)
        {
            _connectionService = connectionService;

            InitializeComponent();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MM.yyyy - HH:mm:ss";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd.MM.yyyy - HH:mm:ss";

            var attributes = Helper.GetAvailableAttributes();
            foreach (var value in attributes.Values)
            {
                checkedListBox1.Items.Add(value, true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var value = textBox1.Text;
            if (value == string.Empty || !Helper.ValidateIpAddress(value))
            {
                MessageBox.Show("Not a valid ip address.");
                return;
            }

            listBox1.Items.Add(value);
            textBox1.Text = string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var selectString = GetSelectString();

            var whereString = string.Empty;
            
            if (checkBox1.Checked)
            {
                QueryHelper.AppendWhereInDateRange(ref whereString, dateTimePicker1.Value, dateTimePicker2.Value);
            }
            
            if (checkBox2.Checked)
            {
                QueryHelper.AppendWhereIpAddresses(ref whereString, listBox1.Items);    
            }

            var query = selectString + " " + whereString + ";";
            var entries = _connectionService.QueryToEntries(query);

            query = "SELECT COUNT(*) AS count FROM log_entries " + whereString + ";";
            var count = _connectionService.QueryToCount(query);

            var attributes = GetSelectedAttributes();
            var entryDialog = new ImportedEntries(
                entries,
                attributes.Count > 0 ? attributes : Helper.GetAvailableAttributes().Keys.ToList(),
                count
            );
            entryDialog.Show();
        }

        private string GetSelectString()
        {
            var selectString = "SELECT ";
            if (checkBox3.Checked)
            {
                bool isFirst = true;
                var selected = GetSelectedAttributes();

                if (selected.Count == 0)
                {
                    selected = new List<string>()
                    {
                        "*"
                    };
                }
                
                foreach (var attribute in selected)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        selectString += ", ";
                    }

                    selectString += Helper.ToUnderscoreCase(attribute);
                }
            }
            else
            {
                selectString += "*";
            }

            selectString += " FROM log_entries";

            return selectString;
        }

        private List<string> GetSelectedAttributes()
        {
            var attributes = new List<string>();
            var availableAttributes = Helper.GetAvailableAttributes();

            for (var index = 0; index < checkedListBox1.Items.Count; index++)
            {
                if (checkedListBox1.GetItemCheckState(index) == CheckState.Checked)
                {
                    attributes.Add(Helper.KeyByValue(availableAttributes, checkedListBox1.Items[index].ToString()));
                }
            }

            return attributes;
        }
    }
}