using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Spike
{
    public partial class MainForm : Form
    {
        private List<string> list = new List<string>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //var logs = EventLog.GetEventLogs().Where(log => log.Log.Equals("System"));

            /*list = (from EventLogEntry entry in logs.First().Entries
                    where dateSearch.Value.Month == entry.TimeGenerated.Date.Month & dateSearch.Value.Year == entry.TimeGenerated.Date.Year
                    group entry by entry.TimeGenerated.Date into dias
                    orderby dias.Key descending
                    select new Entry
                    {
                        Dia = dias.First().TimeGenerated.Date.ToString("dd/MM/yyyy"),
                        Entrada = dias.First().TimeGenerated.ToString("HH:mm:ss"),
                        Saida = dias.Last().TimeGenerated.ToString("HH:mm:ss"),
                        Diff = (dias.Last().TimeGenerated.TimeOfDay - dias.First().TimeGenerated.TimeOfDay).ToString()
                    }).ToList();*/

            using (var logs = new EventLogReader(@"C:\Users\Danilo\Desktop\log-asp-net-fault-iis-pool.evtx", PathType.FilePath))
            {
                List<EventRecord> records = new List<EventRecord>();
                EventRecord record;
                while ((record = logs.ReadEvent()) != null)
                {
                    records.Add(record);
                }

                list = (from entry in records
                        where entry.Properties[19].Value.ToString().IndexOf("barradorocha.ba.gov.br") < 0 &
                        entry.Properties[19].Value.ToString().IndexOf("varzeadopoco.ba.gov.br") < 0 &
                        entry.Properties[19].Value.ToString().IndexOf("sos.") < 0 &
                    entry.Properties[19].Value.ToString().IndexOf("sigad.") < 0 &
                    entry.Properties[19].Value.ToString().IndexOf("download.") < 0 &
                    entry.Properties[19].Value.ToString().IndexOf("/Login") < 0 &
                    entry.Properties[19].Value.ToString().IndexOf("/Programa") < 0 &
                    entry.Properties[19].Value.ToString().IndexOf("/programa") < 0 &
                    entry.Properties[19].Value.ToString().IndexOf("/EsicResposta") < 0 &
                    entry.Properties[19].Value.ToString().IndexOf("/matadesaojoao/") < 0 &
                    entry.Properties[19].Value.ToString().IndexOf("/Tag") < 0 &
                    entry.Properties[19].Value.ToString().IndexOf("sicaf.") < 0 
                        select $@"/******************************************************************/
{entry.Properties[3].Value.ToString()} - {entry.Properties[19].Value.ToString()}
{entry.Properties[17].Value.ToString()}
{entry.Properties[18].Value.ToString()}
/******************************************************************/").ToList();

                gridView.DataSource = list;

                gridView.Refresh();

                this.Cursor = Cursors.Default;
            }
        }

        private void buttonExportar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            /*var content = (from value in list select string.Format("{0},{1},{2},{3}", value.Dia, value.Entrada, value.Saida, value.Diff)).ToList();

            content.Insert(0, "Dia,Entrada,Saida,Diff");

            var file = Path.Combine(Environment.CurrentDirectory, string.Format("{0}{1}", DateTime.Now.Ticks, ".csv"));

            File.WriteAllLines(file, content);

            Process.Start(file);

            this.Cursor = Cursors.Default;*/
        }
    }

    public struct Entry
    {
        public string Dia { get; set; }

        public string Entrada { get; set; }

        public string Saida { get; set; }

        public string Diff { get; set; }
    }
}
