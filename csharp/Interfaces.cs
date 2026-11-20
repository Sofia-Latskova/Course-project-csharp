namespace Firewatch
{
    // Контракт: умею снимать показание датчика
    public interface IScannable
    {
        double ReadValue();
        string SensorSerial { get; }
    }

    // Контракт: умею сообщать о своей исправности
    public interface IDiagnosable
    {
        bool IsOperational { get; }
        int HealthPercent { get; }
    }

    // Контракт: умею попасть в итоговый отчёт
    public interface IReportable
    {
        string ReportSection { get; }
        string ToReportLine();
    }

    // Объект поведения как подать сигнал тревоги
    public interface IAlertPolicy
    {
        string AlertChannel { get; }
        string FormatAlert(ScanResult scan);
    }
}