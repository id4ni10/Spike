using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spike
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var logs = EventLog.GetEventLogs().Where(log => log.Log.Equals("System"));

            var result = (from EventLogEntry entry in logs.First().Entries
                          where dateSearch.Value.Month == entry.TimeGenerated.Date.Month & dateSearch.Value.Year == entry.TimeGenerated.Date.Year
                          group entry by entry.TimeGenerated.Date into dias
                          orderby dias.Key descending
                          select new Entry
                          {
                              Dia = dias.First().TimeGenerated.Date.ToString("dd/MM/yyyy"),
                              Entrada = dias.First().TimeGenerated.ToString("HH:mm:ss"),
                              Saida = dias.Last().TimeGenerated.ToString("HH:mm:ss")
                          }).ToList();

            gridView.DataSource = result;

            gridView.Refresh();

            this.Cursor = Cursors.Default;
        }

        private void buttonExportar_Click(object sender, EventArgs e)
        {

        }
    }

    public struct Entry
    {
        public string Dia { get; set; }

        public string Entrada { get; set; }

        public string Saida { get; set; }
    }
}
