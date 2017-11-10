using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Spike
{
    public partial class MainForm : Form
    {
        private List<Entry> list = new List<Entry>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var logs = EventLog.GetEventLogs().Where(log => log.Log.Equals("System"));

            list = (from EventLogEntry entry in logs.First().Entries
                    where dateSearch.Value.Month == entry.TimeGenerated.Date.Month & dateSearch.Value.Year == entry.TimeGenerated.Date.Year
                    group entry by entry.TimeGenerated.Date into dias
                    orderby dias.Key descending
                    select new Entry
                    {
                        Dia = dias.First().TimeGenerated.Date.ToString("dd/MM/yyyy"),
                        Entrada = dias.First().TimeGenerated.ToString("HH:mm:ss"),
                        Saida = dias.Last().TimeGenerated.ToString("HH:mm:ss")
                    }).ToList();

            gridView.DataSource = list;

            gridView.Refresh();

            this.Cursor = Cursors.Default;
        }

        private void buttonExportar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var content = (from value in list select string.Format("{0},{1},{2}", value.Dia, value.Entrada, value.Saida)).ToList();

            content.Insert(0, "Dia,Entrada,Saida");

            var file = Path.Combine(Environment.CurrentDirectory, string.Format("{0}{1}", DateTime.Now.Ticks, ".csv"));

            File.WriteAllLines(file, content);

            Process.Start(file);

            this.Cursor = Cursors.Default
        }
    }

    public struct Entry
    {
        public string Dia { get; set; }

        public string Entrada { get; set; }

        public string Saida { get; set; }
    }
}
