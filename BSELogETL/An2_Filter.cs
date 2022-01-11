using System;
using System.Windows.Forms;

namespace BSELogETL
{
    public partial class An2_Filter : Form
    {
        private readonly ConnectionService _connectionService;
        
        public An2_Filter(ConnectionService connectionService)
        {
            _connectionService = connectionService;
            
            InitializeComponent();
            
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MM.yyyy - HH:mm:ss";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd.MM.yyyy - HH:mm:ss";
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
            var whereString = "";
            
            if (checkBox1.Checked)
            {
                QueryHelper.AppendWhereInDateRange(ref whereString, dateTimePicker1.Value, dateTimePicker2.Value);
            }
            
            if (checkBox2.Checked)
            {
                QueryHelper.AppendWhereIpAddresses(ref whereString, listBox1.Items);    
            }
            
            var query = "SELECT ip_address, COUNT(*) as ip_count FROM log_entries " + whereString + " GROUP BY ip_address;";

            var results = _connectionService.QueryToIpAddressCount(query);

            var ipAddressCountDataGrid = new IpAddressCountDataGrid(results);
            ipAddressCountDataGrid.Show();
        }
    }
}