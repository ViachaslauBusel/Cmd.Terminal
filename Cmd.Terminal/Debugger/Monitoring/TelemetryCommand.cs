using Cmd.Terminal.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmd.Terminal.Debugger.Monitoring
{
    public class TelemetryCommand : BaseCommand
    {
        private Telemetry m_telemetry;
        public string FileName { get; set; } = "telemetry";

        public TelemetryCommand(Telemetry telemetry, string commandName = "telemetry", string description = "Working with data monitoring")
        {
            m_telemetry = telemetry;
            Command = commandName;
            Description = description;

            Flags.Add(new Flag('h', Help));
            Flags.Add(new Flag('s', Show));
            Flags.Add(new Flag('w', Write));
        }

        private void Help()
        {
            Terminal.PrintHelp("telemetry -s", "Outputting a table to a console.");
            Terminal.PrintHelp("telemetry -w", "Outputting a table to a file.");
        }

        private void Write()
        {
            if (!Directory.Exists(@"Logs")) { Directory.CreateDirectory(@"Logs"); }
            string file = $"{FileName}_{DateTime.UtcNow.Date}";
            using (TextWriter stream_out = File.CreateText(Path.Combine(@"Logs/", $"{FileName}.log")))
            {
                string[] table = CreateTable().GetText();
                foreach (string row in table) { stream_out.WriteLine(row); }
            }
            System.Console.WriteLine($"{m_telemetry.ProbesCount} probe were recorded in a file {FileName}");
        }

        private void Show()
        {

            System.Console.WriteLine($"{m_telemetry.ProbesCount} probe available for output");
            var table = CreateTable();
            table.Draw();

        }

        private ConsoleTable CreateTable()
        {
            var table = new ConsoleTable("NAME", "MIN", "MID", "MAX", "COUNT");

            foreach (Probe probe in m_telemetry.GetProbes)
            {
                table.AddRow(probe.Name, $"{probe.MinValue.ToString("0.####")} ms", $"{probe.MidValue.ToString("0.####")} ms", $"{probe.MaxValue.ToString("0.####")} ms", probe.Count.ToString());
            }
            table.SortingBy("MID");
            return table;
        }


    }
}
