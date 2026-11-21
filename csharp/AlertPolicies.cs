using System.Text;

namespace Firewatch
{
    public sealed class SirenAlert : IAlertPolicy
    {
        public string AlertChannel => "SIREN";
        public string FormatAlert(ScanResult scan)
        {
            var sb = new StringBuilder("[СИРЕНА] ТРЕВОГА! Показания: ");
            bool first = true;
            foreach (var r in scan.Readings)
            {
                if (!r.Reliable) continue;
                if (!first) sb.Append(", ");
                sb.Append($"{r.Kind}={r.Value}");
                first = false;
            }
            return sb.ToString();
        }
    }

    public sealed class SilentLogAlert : IAlertPolicy
    {
        public string AlertChannel => "SILENT_LOG";
        public string FormatAlert(ScanResult scan)
        {
            var sb = new StringBuilder("[ЛОГ] Отклонение: ");
            bool first = true;
            foreach (var r in scan.Readings)
            {
                if (!r.Reliable) continue;
                if (!first) sb.Append("; ");
                sb.Append($"{r.Serial} {r.Kind}={r.Value}");
                first = false;
            }
            return sb.ToString();
        }
    }

    public sealed class EmercomCallAlert : IAlertPolicy
    {
        public string AlertChannel => "CALL_EMERCOM";
        public string FormatAlert(ScanResult scan)
        {
            var sb = new StringBuilder("[МЧС] Вызов бригады. Показания: ");
            bool first = true;
            foreach (var r in scan.Readings)
            {
                if (!first) sb.Append(", ");
                sb.Append($"{r.Serial}({r.Kind})={r.Value}");
                if (!r.Reliable) sb.Append(" [НЕДОСТОВЕРНО]");
                first = false;
            }
            sb.Append(". Требуется выезд.");
            return sb.ToString();
        }
    }
}