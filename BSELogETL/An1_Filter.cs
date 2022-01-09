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
            AppendWhereIpAddresses(ref whereString);
            AppendWhereInDateRange(ref whereString);

            var query = selectString + " " + whereString + ";";

            var entries = _connectionService.QueryToEntries(query);

            var attributes = GetSelectedAttributes();
            var entryDialog = new ImportedEntries(entries,
                attributes.Count > 0 ? attributes : Helper.GetAvailableAttributes().Keys.ToList()
            );
            entryDialog.Show();
        }


        private void AppendWhereIpAddresses(ref string whereString)
        {
            if (!checkBox2.Checked || listBox1.Items.Count == 0)
            {
                return;
            }

            var begin = "WHERE";
            if (whereString.Length > 0)
            {
                begin = "AND";
            }

            whereString += begin + " ip_address IN (";

            bool isFirst = true;
            foreach (var item in listBox1.Items)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    whereString += ", ";
                }

                whereString += "\"" + item + "\"";
            }

            whereString += ") ";
        }

        private void AppendWhereInDateRange(ref string whereString)
        {
            if (!checkBox1.Checked)
            {
                return;
            }

            var begin = "WHERE";
            if (whereString.Length > 0)
            {
                begin = "AND";
            }

            whereString += begin + " requested_at > \"" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")
                           + "\" AND requested_at < \"" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
        }


        private string GetSelectString()
        {
            var selectString = "SELECT ";
            if (checkBox3.Checked)
            {
                selectString = "(";

                bool isFirst = true;
                foreach (var attribute in GetSelectedAttributes())
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

                selectString += ")";
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