using System;
using System.Windows.Forms;

namespace BSELogETL
{
    public partial class An3_Filter : Form
    {
        private readonly ConnectionService _connectionService;

        public An3_Filter(ConnectionService connectionService)
        {
            _connectionService = connectionService;

            InitializeComponent();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MM.yyyy - HH:mm:ss";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd.MM.yyyy - HH:mm:ss";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var whereString = "";
            AppendWhereHttpMethods(ref whereString);
            
            if (checkBox1.Checked)
            {
                QueryHelper.AppendWhereInDateRange(ref whereString, dateTimePicker1.Value, dateTimePicker2.Value);
            }
            
            if (checkBox2.Checked)
            {
                QueryHelper.AppendWhereIpAddresses(ref whereString, listBox1.Items);    
            }

            var query = "SELECT http_method, COUNT(*) as method_count FROM log_entries " + whereString + " GROUP BY http_method;";

            var results = _connectionService.QueryToHttpMethodCount(query);

            var printString = string.Empty;
            foreach (var result in results)
            {
                printString += "\n" + result["HttpMethod"] + ": " + result["HttpMethodCount"];
            }

            if (printString == string.Empty)
            {
                printString = "No matching entries found.";
            }
            else
            {
                printString = "The results have been loaded.\n" + printString;
            }

            MessageBox.Show(printString, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AppendWhereHttpMethods(ref string whereString)
        {
            if (!checkBox3.Checked)
            {
                return;
            }

            if (!checkBox4.Checked && !checkBox5.Checked && !checkBox6.Checked)
            {
                return;
            }

            var begin = "WHERE";
            if (whereString.Length > 0)
            {
                begin = "AND";
            }

            whereString += begin + " http_method IN (";
            
            bool isFirst = true;
            if (checkBox4.Checked)
            {
                whereString += "\"GET\"";
                isFirst = false;
            }
            if (checkBox6.Checked)
            {
                whereString += isFirst ? "\"POST\"" : ", \"POST\"";
                isFirst = false;
            }
            if (checkBox5.Checked)
            {
                whereString += isFirst ? "\"HEAD\"" : ", \"HEAD\"";
            }
            
            whereString += ") ";
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
    }
}