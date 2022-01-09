using System.Collections.Generic;
using System.Windows.Forms;

namespace BSELogETL
{
    public partial class IpAddressCountDataGrid : Form
    {
        public IpAddressCountDataGrid(List<Dictionary<string, string>> entries)
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 2;

            dataGridView1.Columns[0].Name = "IP Address";
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns[1].Name = "Request Count";
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (var entry in entries)
            {
                var entryList = new List<object>
                {
                    entry["IpAddress"],
                    entry["IpAddressCount"]
                };

                dataGridView1.Rows.Add(entryList.ToArray());
            }
        }
    }
}